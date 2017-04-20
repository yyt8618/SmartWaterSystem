using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace SmartWaterSystem
{
    public class PicHelper
    {
        /// <summary>
        /// 保存图片至文件
        /// </summary>
        /// <param name="Savepath">保存路径</param>
        /// <param name="picdata">数据</param>
        /// <param name="suffix">后缀(jpg)</param>
        /// <returns></returns>
        public string SavePicToFile(string Savepath, byte[] picdata, string suffix)
        {
            //string fullpath = "";
            string newFullPath = "";
            Savepath = Path.GetDirectoryName(Savepath);
            Savepath = Path.Combine(Savepath, DateTime.Now.ToString("yyyyMMdd"));
            if (!Directory.Exists(Savepath))
            {
                Directory.CreateDirectory(Savepath);
            }

            //生成文件名
            Random rd = new Random();
            string filename = "";
            do
            {
                filename = DateTime.Now.ToString("yyyyMMddHHmmssffff") + rd.Next(10000) + suffix;
                newFullPath = Path.Combine(Savepath, filename);
            } while (System.IO.File.Exists(newFullPath));
            //保存文件
            MemoryStream ms = new MemoryStream(picdata);
            Image image = Image.FromStream(ms);
            if (!string.IsNullOrEmpty(newFullPath))
                image.Save(newFullPath);
            //fullpath = newFullPath;

            return filename;
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="filefullpath"></param>
        public void DelPic(string filefullpath)
        {
            try
            {
                if (!string.IsNullOrEmpty(filefullpath) && File.Exists(filefullpath))
                {
                    File.Delete(filefullpath);
                }
            }
            catch
            { }
        }

        /// <summary>
        /// 读取图片数据(文件)
        /// </summary>
        /// <param name="filefullpath"></param>
        /// <returns></returns>
        public string GetPicData(string filefullpath)
        {
            List<byte> lstdata = new List<byte>();
            FileStream fs = new FileStream(filefullpath, FileMode.Open);
            int len = 1024, readcount = 0;
            byte[] bs = new byte[len];
            do
            {
                readcount = fs.Read(bs, 0, len);
                for (int i = 0; i < readcount; i++)
                {
                    lstdata.Add(bs[i]);
                }
            } while (readcount > 0);
            if (lstdata != null && lstdata.Count > 0)
            {
                byte[] tmpbs = lstdata.ToArray();
                return Convert.ToBase64String(tmpbs);
            }
            return "";
        }


        /// <summary>
        /// 获取网络地址的图片换成本地临时图片地址并将本地临时图片移动到正式目录的本地临时路径和本地正式路径，但并不移动
        /// </summary>
        /// <param name="netfullpath">网络图片全路径</param>
        /// <param name="nettmppathhead">临时网络图片地址头</param>
        /// <param name="netpathhead">正式网络图片地址头</param>
        /// <param name="localtmphead">本地临时路径头</param>
        /// <param name="localhead">本地正式路径头</param>
        /// <param name="localtmppath">本地临时路径</param>
        /// <param name="localpath">本地正式路径</param>
        /// <param name="newnetfullpath">返回本地正式地址对应的网络地址</param>
        public static void GetNetPic2LocalFileName(long UserId, string netfullpath, string nettmppathhead, string netpathhead, string localtmphead, string localhead,
            out string localtmppath, out string localpath, out string newnetfullpath)
        {
            localtmppath = localpath = newnetfullpath = "";
            //将网络地址格式转成本地路径格式
            string tempath = netfullpath.Replace(nettmppathhead, localtmphead);
            tempath = tempath.Replace("/", "\\");
            if (!File.Exists(tempath))  //不存在就直接返回
            {
                return;
            }
            string suffix = tempath.Replace(localtmphead, "");
            string filetype = "";
            filetype = suffix.Substring(suffix.IndexOf('.'));  //得到文件类型
            string newFullPath = Path.Combine(localhead, suffix);

            //生成文件名
            Random rd = new Random();
            string dir = Path.GetDirectoryName(newFullPath); //路径
            while (System.IO.File.Exists(newFullPath))
            {
                string filename = UserId + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + rd.Next(10000) + filetype;
                newFullPath = Path.Combine(dir, filename);
            }
            localtmppath = tempath;
            localpath = newFullPath;
            //将本地路径地址替换为网络地址，在图片获得认可后转移到正式路径
            newnetfullpath = localpath.Replace(localhead, netpathhead);
            newnetfullpath = newnetfullpath.Replace("\\", "/");
        }

        /// <summary>
        /// 移动文件(锁)
        /// </summary>
        /// <param name="sourceFile">源文件</param>
        /// <param name="destFile">目标文件</param>
        public void MoveFile(string sourceFile, string destFile)
        {
            if (File.Exists(sourceFile) && !File.Exists(destFile))
            {
                string path = Path.GetDirectoryName(destFile);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                File.Move(sourceFile, destFile);
            }
        }

        /// <summary>
        /// 拷贝文件(锁)
        /// </summary>
        /// <param name="sourceFile">源文件</param>
        /// <param name="destFile">目标文件</param>
        public void CopyFile(string sourceFile, string destFile)
        {
            if (File.Exists(sourceFile) && !File.Exists(destFile))
            {
                string path = Path.GetDirectoryName(destFile);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                File.Copy(sourceFile, destFile);
            }
        }

    }
}
