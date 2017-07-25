using DAL;
using Entity;
using System;

namespace BLL
{
    public class LoginBLL
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("LoginBLL");
        LoginDAL dal = new LoginDAL();
        
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="userid">用户名</param>
        /// <param name="pwd">密码-base64</param>
        /// <returns></returns>
        public HTTPRespEntity Login(string userid, string pwd)
        {
            HTTPRespEntity resp = new HTTPRespEntity();
            resp.code = 1;
            resp.msg = "";
            try
            {
                dal.Login(userid, pwd);
            }
            catch (Exception ex)
            {
                logger.ErrorException("Login", ex);
                resp.code = -1;
                resp.msg = "服务器异常";
            }
            return resp;
        }
        

    }
}
