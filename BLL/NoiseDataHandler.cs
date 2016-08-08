using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections;
using Entity;

namespace BLL
{
    /// <summary>
    /// 提供对噪声数据的处理，如FFT变换、功率谱计算
    /// </summary>
    public static class NoiseDataHandler
    {
        static NoiseParmBLL noiseparmBll = new NoiseParmBLL();
        public static string TestPath="";
        /// <summary>
        /// 傅立叶转换数组大小（要求为2的整数次幂）
        /// </summary>
        public static int num = 256;
        public static double cDis = 2000.0 / (double)num; // 频率分辨率，控制在10Hz以内

        //private static List<double[]> dataList = new List<double[]>(); // for test
		/// <summary>
		/// 集合的长度为128，代表128个频率。每一个double数组长度为采集的次数,存储该频率下每次采集的傅里叶数据值。
		/// </summary>
		public static List<double[]> FourierData = new List<double[]>();

		/// <summary>
		/// 初始化傅里叶数据数组
		/// </summary>
		/// <param name="recNum">当前采集次数</param>
		public static void InitData(int recNum)
		{
			FourierData.Clear();
			for (int i = 0; i < num / 2; i++)
				FourierData.Add(new double[recNum]);
		}

		/// <summary>
		/// 计算最小相对噪声幅度（%）
		/// </summary>
		/// <param name="real_dBs">FFT变换后的相对噪声强度数组</param>
		/// <param name="min_dB">最小噪声强度</param>
		/// <param name="Hz">最小噪声强度频率</param>
		private static void MinimumAmplitudeCalc(double[] real_dBs, double leakAmp, ref double min_dB, ref double min_Hz)
        {

			List<double> minAmp = new List<double>();
			List<double> minFrq = new List<double>();
            for (int i = 0; i < real_dBs.Length; i++)
            {
                if (real_dBs[i] > leakAmp)
                {
					minAmp.Add(real_dBs[i]);
					minFrq.Add((i + 1) * cDis);
                }
            }

			if (minAmp.Count != 0)
			{
				min_dB = minAmp.Min();
				min_Hz = minFrq[minAmp.IndexOf(min_dB)];
			}


			//// 若最小相对噪声强度为100%，则选取其中频率最低的采样噪声
			//if (min_dB == 100)
			//{
			//    min_Hz = cDis;
			//}
        }

        /// <summary>
        /// 计算一组噪声时域原始采样数据的相对噪声幅度（利用FFT变换）
        /// </summary>
        /// <param name="data">一组噪声时域原始采样数据</param>
		/// <param name="real_dB">返回最小相对噪声强度（单位：%）</param>
		public static bool AmpCalc(int c, double[] data, ref double[] real_dB)
        {
            string str_DCLen = noiseparmBll.GetParm(ConstValue.DCComponentLen); //获取设定的直流分量长度
            int DCComponentLen = 10;
            if (!string.IsNullOrEmpty(str_DCLen))
                DCComponentLen = Convert.ToInt32(str_DCLen);

            SpSubMinValue(ref data);
            double[] real_Part = new double[num];// 实数部分
            double[] vir_Part = new double[num]; // 虚数部分
            double[] tmp_fourier = new double[num];// 存储傅里叶转换后的数据

            // 为满足频率分辨率，需要增加采样点数到num，然后补0
            for (int i = 0; i < num; i++)
            {
                if (i < data.Length )
                    real_Part[i] = data[i];
                else
                    real_Part[i] = 0;
            }

            // 使用FFT进行时域转频域
            FFTTransfer(real_Part, vir_Part, num, 1);

            // 对变换后的FFT序列进行处理
            tmp_fourier = PowerSpectralCacl(real_Part, vir_Part, num);
            tmp_fourier=Skip(tmp_fourier, DCComponentLen);
            //////////////////////////////////////////////////////////////////////////////////////////
            //dataList.Add(tmp_fourier); // for test

            if (!string.IsNullOrEmpty(TestPath))
            {
                StreamWriter sw = new StreamWriter(string.Format("{0}转换后数据{1}.txt", TestPath, c + 1));
                for (int i = 0; i < tmp_fourier.Length; i++)
                {
                    sw.WriteLine(tmp_fourier[i]);
                }
                sw.Flush();
                sw.Close();
            }
            //////////////////////////////////////////////////////////////////////////////////////////

            // 计算相对噪声幅度
			//real_dB = Trans2Amp(tmp_dB);

            return true;
        }

		/// <summary>
		/// 计算一组噪声时域原始采样数据的相对噪声幅度（利用FFT变换）
		/// </summary>
		/// <param name="data">噪声原始采集数据</param>
		/// <param name="min_amp">最小相对幅度值数组</param>
		/// <param name="min_frq">对应的频率数组</param>
        public static bool AmpCalc(List<double[]> data, ref double[] max_amp, ref double[] max_frq,ref double[] max_am,int leakvalue)
        {
            string str_DCLen = noiseparmBll.GetParm(ConstValue.DCComponentLen);  //获取设定的直流分量长度
            int DCComponentLen = 6;
            if (!string.IsNullOrEmpty(str_DCLen))
                DCComponentLen = Convert.ToInt32(str_DCLen);

            //求出32个数值中最小数值，将32个数值分别减去最小数值，得到新的32个数值
            SpSubMinValue(ref data);

            #region 去掉一个突变的最大
            int remove_index = -1;
            if (data.Count > 3)
            {
                List<double> lst_ave = new List<double>();
                for (int i = 0; i < data.Count; i++)
                {
                    lst_ave.Add(GetAverage(data[i]));
                }
                int max_index = 0;
                double maxvalue = lst_ave[0];
                double ave_butone = lst_ave[0];
                for (int i = 1; i < lst_ave.Count; i++)
                {
                    if (maxvalue < lst_ave[i])
                    {
                        maxvalue = lst_ave[i];
                        max_index = i;
                    }
                    ave_butone += lst_ave[i];
                }
                ave_butone -= maxvalue;
                ave_butone = ave_butone / (lst_ave.Count - 1);
                ave_butone = maxvalue / (maxvalue + ave_butone) * 100;
                if (ave_butone > 85)
                {
                    remove_index = max_index;
                }
            }
            #endregion

            for (int index = 0; index < data.Count; index++)
            {
                double[] real_Part = new double[num];// 实数部分
                double[] vir_Part = new double[num]; // 虚数部分
                double[] tmp_fourier = new double[num/2];

                // 为满足频率分辨率，需要增加采样点数到num，然后补0
                for (int i = 0; i < num; i++)
                {
                    if (i < data[index].Length)
                        real_Part[i] = data[index][i];
                    else
                        real_Part[i] = 0;
                }

                // 使用FFT进行时域转频域
                FFTTransfer(real_Part, vir_Part, num, 1);

                // 对变换后的FFT序列进行处理
                tmp_fourier = PowerSpectralCacl(real_Part, vir_Part, num);
                tmp_fourier = Skip(tmp_fourier, DCComponentLen);

                if (!(remove_index > -1 && remove_index == index))
                {
                    for (int i = 0; i < num / 2; i++)
                    {
                        FourierData[i][index] = tmp_fourier[i];
                    }
                }

                //////////////////////////////////////////////////////////////////////////////////////////
                //dataList.Add(tmp_fourier); // for test
                if (!string.IsNullOrEmpty(TestPath))
                {
                    StreamWriter sw = new StreamWriter(string.Format("{0}转换后数据{1}.txt", TestPath, index + 1));
                    for (int i = 0; i < tmp_fourier.Length; i++)
                    {
                        sw.WriteLine(tmp_fourier[i]);
                    }
                    sw.Flush();
                    sw.Close();
                }
                //////////////////////////////////////////////////////////////////////////////////////////
            }

            //int c = Settings.Instance.GetInt(SettingKeys.Calc);

            //if (c == 1)
            //    MinimumAmpCalc2(ref min_amp, ref min_frq);
            //else if (c == 2)
            //    MinimumAmpCalc(ref min_amp, ref min_frq);

            if (remove_index > -1)
            {
                data.RemoveAt(remove_index);
            }
            MinimumAmpCalc(DCComponentLen, data.Count, leakvalue, out max_amp, out max_frq, out max_am);
            return true;
        }

        /// <summary>
        /// 将指定长度(len)的数据用0替换
        /// </summary>
        private static double[] Skip(double[] datas,int Len)
        {
            int olddatalen = datas.Length;
            if (olddatalen <= Len)
                return datas;

            List<double> lstnewdata = new List<double>();
            lstnewdata.AddRange(datas.Skip(Len).ToArray());
            for (int i = 0; i < Len; i++)
            {
                lstnewdata.Add(0);
            }
            return lstnewdata.ToArray();
        }

        /// <summary>
        /// 获取数组中最小值
        /// </summary>
        private static double GetMinValue(double[] datas)
        {
            double minvalue = 0;
            int i = 0;
            for (minvalue = datas[0], i = 0; i < datas.Length; i++)
            {
                if (minvalue > datas[i])
                    minvalue = datas[i];
            }
            return minvalue;
        }

        /// <summary>
        /// 减去每组中的最小值，得到新的值
        /// </summary>
        private static void SpSubMinValue(ref List<double[]> lstDatas)
        {
            double minvalue = 0;
            int i,j;
            for(i= 0; i< lstDatas.Count; i++)
            {
                minvalue = GetMinValue(lstDatas[i]);  //求出32个数值中最小数值，将32个数值分别减去最小数值，得到新的32个数值
                for (j = 0; j < lstDatas[i].Length; j++)
                {
                    lstDatas[i][j] -= minvalue;
                }
            }
        }

        private static void SpSubMinValue(ref double[] Datas)
        {
            double minvalue = 0;
            minvalue = GetMinValue(Datas);
            for (int i = 0; i < Datas.Length; i++)
            {
                Datas[i] -= minvalue;
            }
        }

        /// <summary>
        /// 计算每一个频段下的最小相对噪声幅度（去最值求平均值）
        /// </summary>
        /// <param name="min_amp">最小相对幅度值数组</param>
        /// <param name="min_frq">对应的频率数组</param>
        private static void MinimumAmpCalc(ref double[] min_amp, ref double[] min_frq)
		{
			double[] amp = new double[FourierData.Count];
			min_frq = new double[FourierData.Count];
			for (int i = 0; i < FourierData.Count; i++)
			{
				min_frq[i] = i * cDis;
				//List<double> tmp = new List<double>();		
				if (FourierData[i].Length < 3)
				{
                    //double avg = FourierData[i].ToList().Average();   // 每一个频率下的傅里叶数据的平均值
                    //for (int j = 0; j < FourierData[i].Length; j++)
                    //{
                    //    if (FourierData[i][j] >= avg)
                    //    {
                    //        tmp.Add(FourierData[i][j]);
                    //    }
                    //}
                    amp[i] = FourierData[i].Min();
				}
				else
				{
					// 去掉最大最小后再求平均值
					List<double> list = FourierData[i].ToList();
					list.Remove(list.Max());
					list.Remove(list.Min());
                    //double avg = list.Average();
                    //for (int j = 0; j < list.Count; j++)
                    //{						
                    //    if (list[j] >= avg)
                    //    {
                    //        tmp.Add(list[j]);
                    //    }
                    //}
                    amp[i] = list.Min();
				}
			}

			//////////////////////////////////////////////////////////////////////////////////////////
			//dataList.Add(amp); // for test
            if (!string.IsNullOrEmpty(TestPath))
            {
                StreamWriter sw = new StreamWriter(TestPath + "最小噪声数据.txt");
                for (int i = 0; i < amp.Length; i++)
                {
                    sw.WriteLine(amp[i]);
                }
                sw.Flush();
                sw.Close();
            }
			//////////////////////////////////////////////////////////////////////////////////////////

			Data2Amp(amp, ref min_amp, ref min_frq);
		}

		/// <summary>
		/// 计算每一个频段下的最小相对噪声幅度(直接求平均值)
		/// </summary>
		/// <param name="min_amp">最小相对幅度值数组</param>
		/// <param name="min_frq">对应的频率数组</param>
		private static void MinimumAmpCalc2(ref double[] min_amp, ref double[] min_frq)
		{
			double[] amp = new double[FourierData.Count];
			min_frq = new double[FourierData.Count];
			for (int i = 0; i < FourierData.Count; i++)
			{
				min_frq[i] = i * cDis;
				//List<double> tmp = new List<double>();

                //double avg = FourierData[i].ToList().Average();   // 每一个频率下的傅里叶数据的平均值
                //for (int j = 0; j < FourierData[i].Length; j++)
                //{
                //    if (FourierData[i][j] >= avg)
                //    {
                //        tmp.Add(FourierData[i][j]);
                //    }
                //}
                amp[i] = FourierData[i].Min();
			}

			//////////////////////////////////////////////////////////////////////////////////////////
            //dataList.Add(amp); // for test
            if (!string.IsNullOrEmpty(TestPath))
            {
                StreamWriter sw = new StreamWriter(TestPath + "最小噪声数据.txt");
                for (int i = 0; i < amp.Length; i++)
                {
                    sw.WriteLine(amp[i]);
                }
                sw.Flush();
                sw.Close();
            }
			//////////////////////////////////////////////////////////////////////////////////////////

			Data2Amp(amp, ref min_amp, ref min_frq);
		}

        private static void MinimumAmpCalc(int DCComponentLen,int array_count,int leakvalue,out double[] lst_maxamp,out double[] lst_maxfrq,out double[] lst_max_am)
        {
            List<double> lst_data = new List<double>();
            double[] min_amp = new double[FourierData.Count - DCComponentLen];
            try
            {
                for (int i = 0; i < FourierData.Count - DCComponentLen && FourierData[i].Length>0; i++)
                {
                    min_amp[i] = FourierData[i].Min();
                }
            }
            catch (Exception ex)
            {
                ;
            }
            if (!string.IsNullOrEmpty(TestPath))
            {
                StreamWriter sw = new StreamWriter(TestPath + "最小噪声数据.txt");
                for (int i = 0; i < min_amp.Length; i++)
                {
                    sw.WriteLine(min_amp[i]);
                }
                sw.Flush();
                sw.Close();
            }

            lst_maxamp = new double[array_count];
            lst_maxfrq = new double[array_count];
            lst_max_am = new double[array_count];
            
            for (int i = 0; i < array_count; i++)
            {
                lst_data.Clear();
                for (int j = 0; j < FourierData.Count - DCComponentLen; j++)
                {
                    lst_data.Add(FourierData[j][i]);
                }
                //double[] low_amp =null;
                //double[] high_amp = null;

                Data2Amp_LH(DCComponentLen, lst_data.ToArray(), out lst_max_am[i], out lst_maxamp[i], out lst_maxfrq[i], leakvalue);
            }

            //根据幅度去掉最小，最大值
            //int maxindex=0,minindex = 0;
            //double tmpmax = lst_max_am[0];
            //double tmpmin = lst_max_am[0];
            //for (int i = 1; i < lst_max_am.Length; i++)
            //{
            //    if (lst_max_am[i] > tmpmax)
            //    {
            //        tmpmax = lst_max_am[i];
            //        maxindex = i;
            //    }

            //    if (lst_maxamp[i] < tmpmin)
            //    {
            //        tmpmin = lst_max_am[i];
            //        minindex = i;
            //    }
            //}

            //lst_data.Clear();
            //lst_data.AddRange(lst_maxamp);
            //lst_data.RemoveAt(minindex);
            //if (maxindex > minindex)
            //    maxindex--;
            //lst_data.RemoveAt(maxindex);
            //lst_maxamp = lst_data.ToArray();

            //lst_data.Clear();
            //lst_data.AddRange(lst_maxfrq);
            //lst_data.RemoveAt(minindex);
            //lst_data.RemoveAt(maxindex);
            //lst_maxfrq = lst_data.ToArray();

            //lst_data.Clear();
            //lst_data.AddRange(lst_max_am);
            //lst_data.RemoveAt(minindex);
            //lst_data.RemoveAt(maxindex);
            //lst_max_am = lst_data.ToArray();
        }

        /// <summary>
        /// 计算每一个频段下的最小相对噪声幅度（去最值求平均值平均）
        /// </summary>
        /// <param name="min_amp">最小相对幅度值数组</param>
        /// <param name="min_frq">对应的频率数组</param>
        private static void MinimumAmpCalc3(ref double[] min_amp, ref double[] min_frq)
        {
            double[] amp = new double[FourierData.Count];
            min_frq = new double[FourierData.Count];
            for (int i = 0; i < FourierData.Count; i++)
            {
                min_frq[i] = i * cDis;
                List<double> tmp = new List<double>();
                if (FourierData[i].Length < 3)
                {
                    double avg = FourierData[i].ToList().Average();   // 每一个频率下的傅里叶数据的平均值
                    for (int j = 0; j < FourierData[i].Length; j++)
                    {
                        if (FourierData[i][j] >= avg)
                        {
                            tmp.Add(FourierData[i][j]);
                        }
                    }
                    amp[i] = tmp.Min();
                }
                else
                {
                    // 去掉最大最小后再求平均值
                    List<double> list = FourierData[i].ToList();
                    list.Remove(list.Max());
                    list.Remove(list.Min());
                    double avg = list.Average();
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (list[j] >= avg)
                        {
                            tmp.Add(list[j]);
                        }
                    }
                    amp[i] = tmp.Average();
                }
            }

            //////////////////////////////////////////////////////////////////////////////////////////
            //dataList.Add(amp); // for test
            if (!string.IsNullOrEmpty(TestPath))
            {
                StreamWriter sw = new StreamWriter(TestPath + "最小噪声数据.txt");
                for (int i = 0; i < amp.Length; i++)
                {
                    sw.WriteLine(amp[i]);
                }
                sw.Flush();
                sw.Close();
            }
            //////////////////////////////////////////////////////////////////////////////////////////

            Data2Amp(amp, ref min_amp, ref min_frq);
        }

        /// <summary>
        /// 计算相对噪声幅度（%）
        /// </summary>
        /// <param name="data">傅里叶变换后的数组</param>
        private static void Data2Amp(double[] data, ref double[] min_amp, ref double[] min_frq)
        {
            double max1 = noiseparmBll.GetParmD(ConstValue.Max1);
            double max2 = noiseparmBll.GetParmD(ConstValue.Max2);
            double min1 = noiseparmBll.GetParmD(ConstValue.Min1);
            double min2 = noiseparmBll.GetParmD(ConstValue.Min2);
            double f = noiseparmBll.GetParmD(ConstValue.LeakHZ_Template);
			min_frq = new double[data.Length];
			min_amp = new double[data.Length];

			for (int i = 0; i < data.Length; i++)
			{
				double hz = i * cDis;

				if (hz >= 0 && hz <= f)
				{
					double a = max1 - min1;
					double per = (data[i] - min1) / a;
					if (per < 0)
						per = 0;
					else if (per > 1)
						per = 1;
					min_amp[i] = Math.Round(per * 100);
					min_frq[i] = hz;
				}
				else if (hz >= (f + 1) && hz < 1000)
				{
					double a = max2 - min2;
					double per = (data[i] - min2) / a;
					if (per < 0)
						per = 0;
					else if (per > 1)
						per = 1;
					min_amp[i] = Math.Round(per * 100);
					min_frq[i] = hz;
				}
			}
        }

        /// <summary>
        /// 按低频和高频段返回
        /// </summary>
        private static void Data2Amp_LH(int DCComponentLen,double[] data, out double max_am, out double max_amp, out double max_frq,double leakvalue)
        {
            double max1 = noiseparmBll.GetParmD(ConstValue.Max1);
            double max2 = noiseparmBll.GetParmD(ConstValue.Max2);
            double min1 = noiseparmBll.GetParmD(ConstValue.Min1);
            double min2 = noiseparmBll.GetParmD(ConstValue.Min2);
            double f = noiseparmBll.GetParmD(ConstValue.LeakHZ_Template);
            List<double> lst_low_amp = new List<double>();
            List<double> lst_high_amp = new List<double>();
            List<double> lst_low_frq = new List<double>();
            List<double> lst_high_frq = new List<double>();
            List<double> lst_low_data = new List<double>();
            List<double> lst_high_data = new List<double>();

            for (int i = 0; i < data.Length; i++)
            {
                double hz = (i + DCComponentLen) * cDis;  //去除了DCComponentLen长度的直流分量

                if (hz >= 0 && hz <= f)
                {
                    double a = max1 - min1;
                    double per = (data[i] - min1) / a;
                    if (per < 0)
                        per = 0;
                    else if (per > 1)
                        per = 1;
                    lst_low_amp.Add(Math.Round(per * 100));
                    lst_low_frq.Add(hz);
                    lst_low_data.Add(data[i]);
                }
                else if (hz >= (f + 1) && hz < 1000)
                {
                    double a = max2 - min2;
                    double per = (data[i] - min2) / a;
                    if (per < 0)
                        per = 0;
                    else if (per > 1)
                        per = 1;
                    lst_high_amp.Add(Math.Round(per * 100));
                    lst_high_frq.Add(hz);
                    lst_high_data.Add(data[i]);
                }
            }

            //low_amp = ((lst_low_amp.Count > 0) ? lst_low_amp.ToArray() : null);
            //high_amp = ((lst_high_amp.Count > 0) ? lst_high_amp.ToArray() : null);

            double tmplow_amp = lst_low_amp.Max();
            double tmphigh_amp = lst_high_amp.Max();
            int index = -1;

            if ((tmplow_amp > leakvalue) && (tmphigh_amp > leakvalue))
            {
                max_amp = tmphigh_amp;
                index=lst_high_amp.IndexOf(max_amp);
                max_frq = lst_high_frq[index];
                max_am = lst_high_data[index];
            }
            else
            {
                if (tmplow_amp > tmphigh_amp)
                {
                    max_amp = tmplow_amp;
                    index = lst_low_amp.IndexOf(max_amp);
                    max_frq = lst_low_frq[index];
                    max_am = lst_low_data[index];
                }
                else if (tmplow_amp < tmphigh_amp)
                {
                    max_amp = tmphigh_amp;
                    index = lst_high_amp.IndexOf(max_amp);
                    max_frq = lst_high_frq[index];
                    max_am = lst_high_data[index];
                }
                else
                {
                    max_amp = tmplow_amp;
                    index = lst_low_amp.IndexOf(max_amp);
                    max_frq = lst_low_frq[index];
                    max_am = lst_low_data[index];
                }
            }
        }

        /// <summary>
        /// 漏水判断(-1:处于最大最小标准值中间，需要通过别的方法综合判断 1：漏水 0:不漏水)
        /// </summary>
        /// <param name="data">噪声原始数据32个值一组</param>
        /// <param name="standvalue">静态漏水标准幅度值</param>
        /// <returns></returns>
        public static int IsLeak1(int GroupID, int RecorderID, List<double[]> lstdatas, out double energyvalue, out double leakprobability)
        {
            energyvalue = 0;  //能量值
            leakprobability = 0; //漏水概率
            double maxstandvalue = noiseparmBll.GetParmD(ConstValue.MaxStandardAMP);
            double minstandvalue = noiseparmBll.GetParmD(ConstValue.MinStandardAMP);
            short[] standdata = NoiseDataBaseHelper.GetStandData(-1, RecorderID);  //GroupID全部取-1，因为ID不重复

            if (standdata == null)
                return -1;

            int i = 0;
            double standaverage = 0;
            standaverage=GetAverage(standdata);

            List<double> lstaverage = new List<double>();
            if (lstdatas != null && lstdatas.Count > 0)
            {
                for (i = 0; i < lstdatas.Count; i++)
                {
                    lstaverage.Add(GetAverage(lstdatas[i]));
                }
            }

            int isleak = 1;
            double[] record_average = new double[lstaverage.Count];
            lstaverage.CopyTo(record_average);
            //for (i = 0; i < lstaverage.Count; i++)
            //{
            //    lstaverage[i] = Math.Abs(standaverage - lstaverage[i]);
            //    if (lstaverage[i] <= maxstandvalue)
            //    {
            //        isleak = 0;
            //    }
            //}

            energyvalue = GetAverage(record_average.ToArray());
            energyvalue = Math.Abs(standaverage - energyvalue);
            if (!string.IsNullOrEmpty(TestPath))
            {
                StreamWriter sw = new StreamWriter(string.Format("{0}能量强度变化数据.txt", TestPath));
                sw.WriteLine(standaverage);  //先写入标准平均值
                sw.WriteLine(energyvalue);   //写入能量值
                for (i = 0; i < record_average.Length; i++)
                {
                    sw.WriteLine(record_average[i]);
                }
                sw.Flush();
                sw.Close();
            }

            if (energyvalue >= maxstandvalue)
            {
                isleak = 1;
                leakprobability = 1;
            }
            else if (energyvalue <= minstandvalue)
            {
                isleak = 0;
                leakprobability = 0;
            }
            else
            {
                isleak = -1;
                leakprobability = 0.5 * (energyvalue - minstandvalue) / (maxstandvalue - minstandvalue);  //能量强度权重50%
            }

            return isleak;
        }

        /// <summary>
        /// 漏水判断(根据漏水噪声幅度判断,IsLeak1方法辅助办法) 1：漏水 0:不漏水
        /// </summary>
        public static int IsLeak3(double[] amps,double[] frqs,int leakvalue,
            out double max_amp,out double max_frq,out double min_amp,out double min_frq,out double leak_amp,out double leak_frq)
        {
            max_amp = 0; max_frq = 0; min_amp = 0; min_frq = 0; leak_amp = 0; leak_frq = 0;
            if (amps == null || amps.Length == 0)
                return 0;
            double[] max_amps = new double[amps.Length];
            amps.CopyTo(max_amps, 0);
            double[] max_frqs = new double[frqs.Length];
            frqs.CopyTo(max_frqs, 0);

            int divid_hz = noiseparmBll.GetParmI(ConstValue.LeakHZ_Template);  //频率分界值
            //按频率从低到高排列,幅度同样根据频率排序
            for (int i = 0; i < max_frqs.Length; i++)
            {
                for (int j = i+1; j < max_frqs.Length; j++)
                {
                    if (max_frqs[j] < max_frqs[i])
                    {
                        double tmp = max_frqs[i];
                        max_frqs[i] = max_frqs[j];
                        max_frqs[j] = tmp;

                        tmp = max_amps[i];  //幅度也跟着变
                        max_amps[i] = max_amps[j];
                        max_amps[j] = tmp;
                    }
                }
            }

            double tmp_max_amp = 0;
            double tmp_min_amp = 0;
            List<int> maxvalue_index = new List<int>(); //最大幅度值对应索引值
            List<int> minvalue_index = new List<int>(); //最小幅度值对应索引值
            //获得最大幅度、最大频率, 有相同的最大幅度值，取频率高的
            tmp_max_amp = max_amps[0];
            //maxvalue_index.Add(0);
            tmp_min_amp = max_amps[0];
            //minvalue_index.Add(0);
            for (int i = 0; i < max_amps.Length; i++)  
            {
                if (tmp_max_amp < max_amps[i])  //得到最大幅度值
                {
                    tmp_max_amp = max_amps[i];

                    maxvalue_index.Clear();
                    maxvalue_index.Add(i);
                }
                else if(tmp_max_amp == max_amps[i])
                {
                    maxvalue_index.Add(i);
                }

                if (tmp_min_amp > max_amps[i])
                {
                    tmp_min_amp = max_amps[i];

                    minvalue_index.Clear();
                    minvalue_index.Add(i);
                }
                else if (tmp_min_amp == max_amps[i])
                {
                    minvalue_index.Add(i);
                }
            }

            max_amp = max_amps[maxvalue_index[maxvalue_index.Count - 1]];  //最大的取最高频
            max_frq = max_frqs[maxvalue_index[maxvalue_index.Count - 1]];  //最小的取最低频

            min_amp = max_amps[minvalue_index[0]];
            min_frq = max_frqs[minvalue_index[0]];

            //if (min_amp < leakvalue)
            //{ min_amp = 0; min_frq = 0; }
            //漏水幅度、频率(分高低频,如果只有低频有，取低频段中最小频率对应的值，如果高频有，则取高频段中最小频率对应的值)
            if (max_frqs[maxvalue_index[0]] <= divid_hz)  //低频有,获得低频段最小频率对应的值
            {
                leak_amp = max_amps[maxvalue_index[0]];
                leak_frq = max_frqs[maxvalue_index[0]];
            }
            if (max_frqs[maxvalue_index[maxvalue_index.Count - 1]] > divid_hz)  //高频段有，取高频段最小频率对应值
            {
                for (int i = 0; i < maxvalue_index.Count; i++)
                {
                    if (max_frqs[maxvalue_index[i]] > divid_hz)
                    {
                        leak_amp = max_amps[maxvalue_index[i]];
                        leak_frq = max_frqs[maxvalue_index[i]];
                        break;
                    }
                }
            }

            if (leak_amp >= leakvalue)
            {
                return 1;
            }
            else
                return 0;
        }

        private static double GetAverage(double[] datas)
        {
            double average = 0;
            if (datas != null && datas.Length > 0)
            {
                for (int i = 0; i < datas.Length; i++)
                {
                    average += datas[i];
                }
                average /= datas.Length;
            }
            return average;
        }

        public static double GetAverage(short[] datas)
        {
            double average = 0;
            if (datas != null && datas.Length > 0)
            {
                for (int i = 0; i < datas.Length; i++)
                {
                    average += datas[i];
                }
                average /= datas.Length;
            }
            return average;
        }

        /// <summary>
        /// 漏水判断（0：不漏水 1：漏水），返回漏水的幅度及频率
        /// </summary>
        /// <param name="db">最小相对噪声幅度数值</param>
        /// <param name="leakAmp">设定的漏水幅度</param>
        /// <param name="leakFrq">设定的漏水频率</param>
        public static int IsLeak2(double[] db, double leakAmp, ref double[] re)
		{
            double leakFrq = noiseparmBll.GetParmD(ConstValue.LeakHZ_Template);
			List<double> amp = new List<double>();
			List<double> frq = new List<double>();
			int num = (int)((leakFrq + 1) / cDis);
			for (int i = num; i < db.Length; i++)
			{
				if (db[i] > leakAmp)
				{
					amp.Add(db[i]);
					frq.Add(i * cDis);
				}
			}

			// 如果高频段出现漏水,返回
			if (amp.Count != 0)
			{
				for (int i = 0; i < amp.Count; i++)
				{
					if (amp[i] == 100)
					{
						re[0] = amp[i];
						re[1] = frq[i];
						return 1;
					}
				}

				re[0] = amp.Min();
				re[1] = frq[amp.IndexOf(re[0])];
				return 1;
			}
			
			// 如果低频段出现漏水
			for (int i = 4; i < num; i++)
			{
				if (db[i] > leakAmp)
				{
					amp.Add(db[i]);
					frq.Add(i * cDis);
				}
			}
			
			
			if (amp.Count != 0)
			{
				for (int i = 0; i < amp.Count; i++)
				{
					if (amp[i] == 100)
					{
						re[0] = amp[i];
						re[1] = frq[i];
						return 1;
					}
				}

				re[0] = amp.Min();
				re[1] = frq[amp.IndexOf(re[0])];
				return 1;
			}

			return 0;
		}

		#region FFT算法、功率谱算法

		/// <summary>
		/// 快速傅立叶变换
		/// </summary>
		/// <param name="x">时域序列的实数部分</param>
		/// <param name="y">时域序列的虚数部分</param>
		/// <param name="n">FFT点个数</param>
		/// <param name="sign">1：正转换 2：反转换</param>
		public static void FFTTransfer(double[] x, double[] y, int n, int sign)
        {
            int m = 0;
            int i, j, k, l, n1, n2;
            double c, c1, e, s, s1, t, tr, ti;
            //Calculate i = log2N
            for (j = 1, i = 1; i < 16; i++)
            {
                m = i;
                j = 2 * j;
                if (j == n)
                    break;
            }
            //计算蝶形图的输入下标（码位倒读）
            n1 = n - 1;
            for (j = 0, i = 0; i < n1; i++)
            {
                if (i < j)
                {
                    tr = x[j];
                    ti = y[j];
                    x[j] = x[i];
                    y[j] = y[i];
                    x[i] = tr;
                    y[i] = ti;
                }
                k = n / 2;
                while (k < (j + 1))
                {
                    j = j - k;
                    k = k / 2;
                }
                j = j + k;
            }
            //计算每一级的输出，l为某一级，i为同一级的不同群，使用同一内存（即位运算）
            n1 = 1;
            for (l = 1; l <= m; l++)
            {
                n1 = 2 * n1;
                n2 = n1 / 2;
                e = 3.1415926 / n2;
                c = 1.0;
                s = 0.0;
                c1 = Math.Cos(e);
                s1 = -sign * Math.Sin(e);
                for (j = 0; j < n2; j++)
                {
                    for (i = j; i < n; i += n1)
                    {
                        k = i + n2;
                        tr = c * x[k] - s * y[k];
                        ti = c * y[k] + s * x[k];
                        x[k] = x[i] - tr;
                        y[k] = y[i] - ti;
                        x[i] = x[i] + tr;
                        y[i] = y[i] + ti;
                    }
                    t = c;
                    c = c * c1 - s * s1;
                    s = t * s1 + s * c1;
                }
            }
            //如果是求IFFT，再除以N
            if (sign == -1)
            {
                for (i = 0; i < n; i++)
                {
                    x[i] /= n;
                    y[i] /= n;
                }
            }
        }

        /// <summary>
        /// 计算功率谱
        /// </summary>
        /// <param name="x">实数部分</param>
        /// <param name="y">虚数部分</param>
        public static double[] PowerSpectralCacl(double[] x, double[] y, int n)
        {
            double[] psd = new double[x.Length/2];
            for (int i = 0; i < x.Length/2; i++)
                psd[i] = Math.Sqrt(Math.Pow(x[i], 2) + Math.Pow(y[i], 2) / (double)(n / 2));

            return psd;
        }

        /// <summary>
        /// 直接法计算功率谱（用Welch的平均周期图方法来计算信号的自功率谱和相关函数）
        /// </summary>
        /// <param name="x">x-双精度实型一维数组，长度为n，输入信号x（i）</param>
        /// <param name="n">n-整形变量，输入信号的长度</param>
        /// <param name="m">m-整形变量，分段的长度</param>
        /// <param name="nfft">nfft-整形变量，估计功率谱所用FFT的长度，它必须是2的整数次幂且nfft>=m</param>
        /// <param name="win">win-整形变量，窗函数的类型。win=1，表示矩形窗；win=2，表示海明窗</param>
        /// <param name="fs">fs-双精度实型变量，采样频率</param>
        /// <param name="r">r-双精度实型一维数组，长度为（nfft/2+1),用于存放相关函数r（i）的值</param>
        /// <param name="freq">freq-双精度实型一维数组，长度为（nfft/2+1),用于存放与功率谱相对应的频率值</param>
        /// <param name="sxx">sxx-双精度实型一维数组，长度为（nfft/2+1),用于存放功率谱的值</param>
        /// <param name="sdb">sdb-整形变量，用于表示功率谱的类型，sdb=0，表示线形谱；sdb=1，表示以dB为单位的对数谱</param>
        public static void pmpse(double[] x, int n, int m, int nfft, int win, double fs, double[] r, double[] freq, double[] sxx, int sdb)
        { 
            int i, j, k, s;
            int m2, nrd, kmax, ns1, nsectp;
            int nfft21, NumOfSections, NumUsed;
            double u, fl, xsum, norm, twopi, rexmn, imxmn, xmean;
            double[] xa, xreal, ximag, window;
            xa = new double[nfft];
            xreal = new double[nfft];
            ximag = new double[nfft];
            window = new double[nfft];
            nfft21 = nfft / 2 + 1;
            NumOfSections = (n - m / 2) / (m / 2);
            NumUsed = NumOfSections * (m / 2) + m / 2;
            s = 0;
            xsum = 0.0;
            ns1 = NumOfSections + 1;//分配的段数
            m2 = m / 2;

            for (k = 0; k < ns1; k++)
            {
                for (i = 0; i < m2; i++)
                {
                    xa[i] = x[s + i];
                }
                for (i = 0; i < m2; i++)
                {
                    xsum = xsum + xa[i];
                }
                s += m2;
            }

            xmean = xsum / NumUsed;
            rexmn = xmean;
            imxmn = xmean;
            u = (double)m;//计算u，使用海明窗
            if (win == 2)
            {
                u = 0.0;
                twopi = 8.0 * Math.Atan(1.0);
                i = m2 - 1;//////////////////////////
                fl = m - 1.0;
                {
                    window[i] = 0.54 - 0.46 * Math.Cos(twopi * i / fl);
                    u += window[i] * window[i];
                }
            }
            s = 0;
            for (i = 0; i < nfft21; i++)//功率谱数组首先清零
            {
                sxx[i] = 0.0;
            }
            m2 = m / 2;
            for (i = 0; i < m2; i++)
            { xa[i + m2] = x[s + i]; }
            s += m2;
            kmax = (NumOfSections + 1) / 2;
            nsectp = (NumOfSections + 1) / 2;
            nrd = m;

            for (k = 0; k < kmax; k++)//计算功率谱的值
            {
                for (i = 0; i < m2; i++)
                {
                    j = m2 + i;
                    xreal[i] = xa[j];
                    ximag[i] = 0.0;
                }
                if ((k == (kmax - 1)) && (nsectp != NumOfSections))
                {
                    for (i = m2; i < nrd; i++)
                    { xa[i] = 0.0; }
                    nrd = m / 2;
                }
                for (i = 0; i < nrd; i++)
                { xa[i] = x[s + i]; }
                for (i = 0; i < m2; i++)
                {
                    j = m2 + i;
                    xreal[j] = xa[i] - rexmn;
                    ximag[j] = xa[j] - imxmn;
                    xreal[i] = xreal[i] - rexmn;
                    ximag[i] = xa[i] - imxmn;
                }

                if ((k == (kmax - 1)) && (nsectp != NumOfSections))
                {
                    for (i = 0; i < m; i++)
                    { ximag[i] = 0.0; }
                }
                s = s + nrd;
                if (win == 2)//给每段加窗函数
                {
                    for (i = 0; i < m; i++)
                    {
                        xreal[i] *= window[i];
                        ximag[i] *= window[i];
                    }
                }
                if (m != nfft)//不够的补零
                {
                    for (i = m; i < nfft; i++)
                    {
                        xreal[i] = 0.0;
                        ximag[i] = 0.0;
                    }
                }
                FFTTransfer(xreal, ximag, nfft, 1);
                for (i = 1; i < nfft21; i++)
                {
                    j = nfft - i;
                    sxx[i] += xreal[i] * xreal[i] + ximag[i] * ximag[i];
                    sxx[i] += xreal[j] * xreal[j] + ximag[j] * ximag[j];
                }
                sxx[0] += xreal[0] * xreal[0] * 2.0;
                sxx[0] += ximag[0] * ximag[0] * 2.0;
            }


            norm = 2.0 * u * NumOfSections;
            for (i = 0; i < nfft21; i++)
            {
                sxx[i] = sxx[i] / norm;
                xreal[i] = sxx[i];
                ximag[i] = 0.0;
                j = nfft - i;
                xreal[j] = xreal[i];
                ximag[j] = ximag[i];
            }
            FFTTransfer(xreal, ximag, nfft, -1);
            for (i = 0; i < nfft21; i++)
            { r[i] = xreal[i]; }
            for (i = 0; i < nfft21; i++)
            {
                freq[i] = i * fs / (double)nfft;
                if (sdb == 1)
                {
                    if (sxx[i] == 0.0)
                        sxx[i] = 1.0e-15;
                    sxx[i] = 20.0 * Math.Log10(sxx[i]);
                }
            }
            //free(xa);
            //free(xreal);
            //free(ximag);
            //free(window);

        }

        /// <summary>
        /// 间接法计算功率谱
        /// </summary>
        /// <param name="x">x--双精度实型=维数组,长度为n。输入信号x(i)</param>
        /// <param name="y">y--双精度实型=维数组,长度为n。输入信号y(i)</param>
        /// <param name="n">n--整型变量。输入信号的长度</param>
        /// <param name="m">m--整型变量。分段的长度</param>
        /// <param name="mode">mode--整型变量。相关函数的类型。mode=0,表示自相关;mode=1,表示互相关;mode=2,表示自协方差;mode=3,表示互协方差</param>
        /// <param name="win">win--整型变量。窗函数的类型。win=1,表示矩形窗;win=2,表示海明窗</param>
        /// <param name="fs">fs--双精度实型变量。采样频率(以赫兹为单位)</param>
        /// <param name="lag">lag--整型变量。估计功率谱所用相关函数的点数。</param>
        /// <param name="nfft">nfft--整型变量。估计功率谱所用FFT的长度。nfft≥2*lag=1</param>
        /// <param name="r">r--双精度实型=维数组,长度为(等+1)。用于存放相关函数r。(i)的值</param>
        /// <param name="freq">freq--双精度实型=维数组,长度为(下nttt+1)。用于存放与功率谱相对应的频率值</param>
        /// <param name="sxy">sxy--双精度实型=维数组,长度为(下nfft+1)。用于存放互功率谱的值</param>
        /// <param name="sdb">sdb--整型变量。用于指示功率谱的单位。sdb=0,表示线性谱;sdb=1,表示以dB为单位的对数谱</param>
        public static void cmpse(double[] x, double[] y, int n, int m, int mode, int win, double fs, int lag, int nfft, double[] r, double[] freq, double[] sxy, int sdb)
        {
            int i, j, k, s;
            int nrd, nrdy, nrdx, shft, mhlfl, nsect1;
            int nrdxl, nlast, nhfl;
            double xmnc, xmns, xic, xis, yic, yis;
            double pi, xsum, ysum, xmean, ymean, nsect;
            double[] xa, xc, zc, zs, xs;
            xmnc = 0;
            xmns = 0;
            xa = new double[nfft];
            xc = new double[nfft]; ;
            xs = new double[nfft];
            zc = new double[m];
            zs = new double[m];
            pi = 4.0 * Math.Atan(1.0);
            shft = m / 2;
            mhlfl = shft + 1;
            nsect = (n + shft - 1.0) / shft;
            if (mode >= 2)
            {
                s = 0;
                nrd = shft;
                xsum = 0.0;
                ysum = 0.0;
                for (k = 0; k < nsect; k++)
                {
                    if (k == (nsect - 1))
                        nrd = n - ((int)nsect - 1) * nrd;
                    for (i = 0; i < nrd; i++) { xa[i] = x[s + i]; }
                    for (i = 0; i < nrd; i++) { xsum += xa[i]; }
                    if (mode != 2)
                    {
                        for (i = 0; i < nrd; i++) { xa[i] = y[s + i]; }
                        for (i = 0; i < nrd; i++) { ysum = ysum + xa[i]; }
                    }
                    s = s + nrd;
                }
                xmean = xsum / n;
                ymean = ysum / n;
                if (mode == 2) ymean = xmean;
                xmnc = xmean;
                xmns = ymean;
            }
            s = 0;
            nrdy = m;
            nrdx = shft;
            for (i = 0; i < mhlfl; i++) { zc[i] = 0.0; zs[i] = 0.0; }
            for (k = 0; k < nsect; k++)
            {
                nsect1 = (int)nsect - 2;
                if (k >= nsect1)
                {
                    nrdy = n - k * shft;
                    if (k == (nsect - 1)) nrdx = nrdy;
                    if (nrdy != m)
                    {
                        for (i = nrdy; i < m; i++) { xc[i] = 0.0; xs[i] = 0.0; }
                    }
                }
                for (i = 0; i < nrdy; i++) { xa[i] = x[s + i]; }
                for (i = 0; i < nrdy; i++) { xc[i] = xa[i]; xs[i] = xa[i]; }
                if ((mode != 0) && (mode != 2))
                {
                    for (i = 0; i < nrdy; i++) { xa[i] = y[s + i]; }
                    for (i = 0; i < nrdy; i++) { xs[i] = xa[i]; }
                }
                if (mode >= 2)
                {
                    for (i = 0; i < nrdy; i++) { xc[i] = xc[i] - xmnc; xs[i] = xs[i] - xmns; }
                }
                nrdxl = nrdx;
                for (i = nrdxl; i < m; i++) { xc[i] = 0.0; }
                FFTTransfer(xc, xs, m, 1);
                for (i = 1; i < shft; i++)
                {
                    j = m - i;
                    xic = (xc[i] + xc[j]) * 0.5;
                    xis = (xs[i] = xs[j]) * 0.5;
                    yic = (xs[i] + xs[j]) * 0.5;
                    yis = (xc[j] = xc[i]) * 0.5;
                    zc[i] = zc[i] + xic * yic + xis * yis;
                    zs[i] = zs[i] + xic * yis - xis * yic;
                }
                zc[0] = zc[0] + xc[0] * xs[0];
                zc[shft] = zc[shft] + xc[shft] * xs[shft];
                s = s + shft;
            }
            for (i = 1; i < shft; i++)
            {
                j = m - i;
                xc[i] = zc[i];
                xs[i] = zs[i];
                xc[0] = zc[i];
                xs[0] = -zs[i];
            }
            xc[0] = zc[0];
            xs[0] = zs[0];
            xc[shft] = zc[shft];
            xs[shft] = zs[shft];
            FFTTransfer(xc, xs, m, -1);
            for (i = 0; i < mhlfl; i++) { xa[i] = xc[i] / n; }
            for (i = 0; i < mhlfl; i++) { r[i] = xa[i]; }
            for (i = 1; i < lag; i++)
            {
                if (win != 1) { xa[i] = xa[i] * (0.54 + 0.46 * Math.Cos(i * pi / (lag - 1))); }
                if ((mode != 1) && (mode != 3)) { j = nfft - i; xa[j] = xa[i]; }
            }
            nlast = nfft - lag;
            if ((mode == 1) || (mode == 3)) nlast = nfft;
            for (i = lag; i < nlast; i++) { xa[i] = 0; }
            for (i = 0; i < nfft; i++)
            { xc[i] = xa[i]; xs[i] = 0.0; }
            FFTTransfer(xc, xs, nfft, 1);
            nhfl = nfft / 2 + 1;
            for (i = 0; i < nhfl; i++)
            {
                freq[i] = i * fs / (double)nfft;
                if (sdb == 0) { sxy[i] = Math.Sqrt(xc[i] * xc[i] + xs[i] * xs[i]); }
                else { sxy[i] = 20.0 * Math.Log10(Math.Sqrt(xc[i] * xc[i] + xs[i] * xs[i])); }
            }
        }

        #endregion   
     
        public static string CallbackReaded(Dictionary<short, short[]> result, List<NoiseRecorder> recList, string textPath, ref List<NoiseRecorder> recorderList)
        {
            try
            {
                if (result == null)
                {
                    return "";
                }

                int fla = 1;
                foreach (short key in result.Keys)
                {
                    NoiseRecorder recorder = (from item in recList.AsEnumerable()
                                              where item.ID == key
                                              select item).ToList()[0];

                    //int recNum = recorder.RecordNum;
                    int spanCount = 32; // 连续采集32个点
                    List<double[]> data = new List<double[]>();// 将采集数据分32个点为一组存放
                    List<double[]> data_isleak1 = new List<double[]>();// 将采集数据分32个点为一组存放,用于IsLeak1函数运算

                    for (int j = 0; j < result[key].Length / spanCount; j++)
                    {
                        double[] tmpData = new double[spanCount];
                        double[] tmpData_IsLeak = new double[spanCount];
                        for (int k = 0; k < spanCount; k++)
                        {
                            tmpData[k] = result[key][k + j * spanCount];
                        }

                        if (tmpData.Max() < 0)
                        {
                            ; //无效数据
                        }
                        else
                        {
                            //////////////////////////////////////////////////////////////////////////////////////////
                            // for test
                            textPath = string.Format(textPath, recorder.ID);
                            if (!string.IsNullOrEmpty(textPath))
                            {
                                if (!Directory.Exists(textPath))
                                    Directory.CreateDirectory(textPath);

                                StreamWriter sw = new StreamWriter(string.Format("{0}转换前数据{1}.txt", textPath, fla));
                                foreach (var item in tmpData)
                                {
                                    sw.WriteLine(item);
                                }
                                sw.Flush();
                                sw.Close();
                            }

                            //////////////////////////////////////////////////////////////////////////////////////////

                            data.Add(tmpData);
                            tmpData.CopyTo(tmpData_IsLeak, 0);
                            data_isleak1.Add(tmpData_IsLeak);
                            fla++;
                        }
                    }
                    double[] amp = null;
                    double[] frq = null;
                    double[] max_am = null;
                    // 计算每一个频段下的最小幅度
                    NoiseDataHandler.InitData(data.Count);//result[key].Length / spanCount);
                    NoiseDataHandler.AmpCalc(data, ref amp, ref frq, ref max_am, recorder.LeakValue);

                    NoiseData da = new NoiseData();
                    da.RecorderID = recorder.ID;
                    da.GroupID = recorder.GroupID;
                    da.UploadTime = DateTime.Now;
                    da.ReadTime = DateTime.Now;
                    da.OriginalData = result[key];
                    da.Frequency = frq;
                    da.Amplitude = amp;
                    da.UploadFlag = result[key].Length / spanCount;
                    recorder.Data = da;

                    double[] ru = new double[2];
                    double energyvalue = 0;
                    double leakprobability = 0;
                    int isLeak1 = NoiseDataHandler.IsLeak1(recorder.GroupID, recorder.ID, data_isleak1, out energyvalue, out leakprobability);
                    // int isLeak2 = NoiseDataHandler.IsLeak2(amp, recorder.LeakValue, ref ru);

                    double max_amp, max_frq, min_amp, min_frq;//, leak_amp, leak_frq;
                    int leaktmp = NoiseDataHandler.IsLeak3(amp, frq, recorder.LeakValue, out max_amp, out max_frq, out min_amp, out min_frq, out ru[0], out ru[1]);
                    //if (-1 == isLeak1)
                    //{
                    //    isLeak1 = leaktmp;
                    //    leakprobability += ru[0] * 0.5 / 100;
                    //}
                    if(0 == isLeak1)  //能量强度小于设定最小值
                    {
                        isLeak1 = leaktmp;
                        if (ru[0] >= 100)
                            leakprobability = 1;
                        else
                            leakprobability = 0;
                    }
                    else if(-1 == isLeak1)  //能量值处于最大最小标准值中间
                    {
                        isLeak1 = leaktmp;
                        leakprobability += ru[0] * 0.5 / 100;
                    }

                    NoiseResult re = new NoiseResult();
                    re.GroupID = recorder.GroupID;
                    re.IsLeak = (isLeak1);// | isLeak2);
                    re.RecorderID = recorder.ID;
                    re.LeakAmplitude = ru[0];
                    re.LeakFrequency = ru[1];
                    re.UploadTime = DateTime.Now;
                    re.ReadTime = recorder.Data.ReadTime;
                    re.EnergyValue = energyvalue;
                    re.LeakProbability = leakprobability;
                    recorder.Result = re;

                    for (int i = 0; i < recorderList.Count; i++)
                    {
                        if (recorderList[i].ID == recorder.ID)
                            recorderList[i] = recorder;
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                //XtraMessageBox.Show("读取发生错误：" + ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "读取发生错误：" + ex.Message;
            }
        }
    }
}
