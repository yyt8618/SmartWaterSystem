using Entity;
using SmartWaterSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BLL
{
    /// <summary>
    /// 图片处理(上传)
    /// </summary>
    public class PicBLL
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("PicBLL");

        /// <summary>
        /// 保存图片至临时文件夹,当该图片使用到时，将图片从临时文件移动到正式目录
        /// </summary>
        /// <param name="database64">图片base64编码数据</param>
        /// <param name="netpathhead">网络路径的头,返回的是网络路径</param>
        /// <param name="suffix">后缀</param>
        /// <returns></returns>
        public HTTPRespEntity UpLoadPic(string database64, string savelocaltmppath, string netpathhead, string suffix)
        {
            HTTPRespEntity resp = new HTTPRespEntity();
            try
            {
                if (string.IsNullOrEmpty(database64))
                {
                    resp.code = -1;
                    resp.msg = "图片不能为空";
                    return resp;
                }
                //string picbase64=System.Web.HttpUtility.UrlDecode(database64);
                byte[] bs = Convert.FromBase64String(database64);
                if (!suffix.StartsWith("."))
                    suffix = "." + suffix;
                string filename = (new PicHelper()).SavePicToFile(savelocaltmppath, bs, suffix);
                //将本地路径地址替换为网络地址
                //picfullpath = picfullpath.Replace(savelocaltmppath, netpathhead);
                //picfullpath = picfullpath.Replace("\\", "/");
                resp.data = filename;
                resp.code = 1;
            }
            catch (Exception ex)
            {
                logger.ErrorException("UploadPic", ex);
                resp.code = -1;
                resp.msg = "服务器异常";
            }
            return resp;
        }


        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="savepath"></param>
        /// <returns></returns>
        public HTTPRespEntity GetPicData(string filepath)
        {
            HTTPRespEntity resp = new HTTPRespEntity();
            try
            {
                if (string.IsNullOrEmpty(filepath))
                {
                    resp.code = -1;
                    resp.msg = "路径不能为空";
                    return resp;
                }
                if (!File.Exists(filepath))
                {
                    resp.code = -1;
                    resp.msg = "文件不存在";
                    return resp;
                }
                resp.data = (new PicHelper()).GetPicData(filepath);
                resp.code = 1;
            }
            catch (Exception ex)
            {
                logger.ErrorException("GetPicData", ex);
                resp.code = -1;
                resp.msg = "服务器异常";
            }
            return resp;
        }


    }
}
