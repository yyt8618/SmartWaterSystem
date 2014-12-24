using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace NoiseAnalysisSystem
{
    /// <summary>
    /// 提供对噪声数据的处理，如FFT变换、功率谱计算
    /// </summary>
    public static class NoiseDataHandler
    {
        /// <summary>
        /// 傅立叶转换数组大小（要求为2的整数次幂）
        /// </summary>
        public static int num = 256;
        public static double cDis = 2000.0 / (double)num; // 频率分辨率，控制在10Hz以内

        private static List<double[]> dataList = new List<double[]>(); // for test
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

            //////////////////////////////////////////////////////////////////////////////////////////
            dataList.Add(tmp_fourier); // for test

            StreamWriter sw = new StreamWriter(string.Format("{0}转换后数据{1}.txt", GlobalValue.TestPath, c + 1));
            for (int i = 0; i < tmp_fourier.Length; i++) 
            {
                sw.WriteLine(tmp_fourier[i]);
            }
            sw.Flush();
            sw.Close();
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
        public static bool AmpCalc(List<double[]> data, ref double[] min_amp, ref double[] min_frq)
        {
            //求出32个数值中最小数值，将32个数值分别减去最小数值，得到新的32个数值
            SpSubMinValue(ref data);

            for (int index = 0; index < data.Count; index++)
            {
                double[] real_Part = new double[num];// 实数部分
                double[] vir_Part = new double[num]; // 虚数部分
                double[] tmp_fourier = new double[num];

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

                for (int i = 0; i < num / 2; i++)
                {
                    FourierData[i][index] = tmp_fourier[i];
                }

                //////////////////////////////////////////////////////////////////////////////////////////
                dataList.Add(tmp_fourier); // for test

                StreamWriter sw = new StreamWriter(string.Format("{0}转换后数据{1}.txt", GlobalValue.TestPath, index + 1));
                for (int i = 0; i < tmp_fourier.Length / 2; i++)
                {
                    sw.WriteLine(tmp_fourier[i]);
                }
                sw.Flush();
                sw.Close();
                //////////////////////////////////////////////////////////////////////////////////////////
            }

            int c = Convert.ToInt32(AppConfigHelper.GetAppSettingValue("Calc"));

            if (c == 1)
                MinimumAmpCalc2(ref min_amp, ref min_frq);
            else if (c == 2)
                MinimumAmpCalc(ref min_amp, ref min_frq);

            return true;
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
					amp[i] = tmp.Min();
				}
			}

			//////////////////////////////////////////////////////////////////////////////////////////
			dataList.Add(amp); // for test

            StreamWriter sw = new StreamWriter(GlobalValue.TestPath + "最小噪声数据.txt");
			for (int i = 0; i < amp.Length; i++)
			{
				sw.WriteLine(amp[i]);
			}
			sw.Flush();
			sw.Close();
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
				List<double> tmp = new List<double>();

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

			//////////////////////////////////////////////////////////////////////////////////////////
			dataList.Add(amp); // for test

            StreamWriter sw = new StreamWriter(GlobalValue.TestPath + "最小噪声数据.txt");
			for (int i = 0; i < amp.Length; i++)
			{
				sw.WriteLine(amp[i]);
			}
			sw.Flush();
			sw.Close();
			//////////////////////////////////////////////////////////////////////////////////////////

			Data2Amp(amp, ref min_amp, ref min_frq);
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
            dataList.Add(amp); // for test

            StreamWriter sw = new StreamWriter(GlobalValue.TestPath + "最小噪声数据.txt");
            for (int i = 0; i < amp.Length; i++)
            {
                sw.WriteLine(amp[i]);
            }
            sw.Flush();
            sw.Close();
            //////////////////////////////////////////////////////////////////////////////////////////

            Data2Amp(amp, ref min_amp, ref min_frq);
        }

        /// <summary>
        /// 计算相对噪声幅度（%）
        /// </summary>
        /// <param name="data">傅里叶变换后的数组</param>
        private static void Data2Amp(double[] data, ref double[] min_amp, ref double[] min_frq)
        {
			double max1 = Convert.ToDouble(AppConfigHelper.GetAppSettingValue("Max1"));
			double max2 = Convert.ToDouble(AppConfigHelper.GetAppSettingValue("Max2"));
			double min1 = Convert.ToDouble(AppConfigHelper.GetAppSettingValue("Min1"));
			double min2 = Convert.ToDouble(AppConfigHelper.GetAppSettingValue("Min2"));
			double f = Convert.ToDouble(AppConfigHelper.GetAppSettingValue("LeakHZ_Template"));
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
        /// 漏水判断(0:不漏水 1：漏水)
        /// </summary>
        /// <param name="data">噪声原始数据32个值一组</param>
        /// <param name="standvalue">静态漏水标准幅度值</param>
        /// <returns></returns>
        public static int IsLeak1(int GroupID,int RecorderID,List<double[]> lstdatas)
        {
            double standvalue = Convert.ToDouble(AppConfigHelper.GetAppSettingValue("StandardAMP"));
            short[] standdata = NoiseDataBaseHelper.GetStandData(GroupID, RecorderID);

            if (standdata == null)
                return 1;   //?

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

            for (i = 0; i < lstaverage.Count; i++)
            {
                lstaverage[i] = Math.Abs(standaverage - lstaverage[i]);
                if (lstaverage[i] <= standvalue)
                {
                    return 0;
                }
            }
            return 1;
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

        private static double GetAverage(short[] datas)
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
			double leakFrq = Convert.ToDouble(AppConfigHelper.GetAppSettingValue("LeakHZ_Template"));
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
            double[] psd = new double[x.Length];
            for (int i = 0; i < x.Length; i++)
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
    }
}
