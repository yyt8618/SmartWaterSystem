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
        public HTTPRespEntity Login(string username, string pwd)
        {
            HTTPRespEntity resp = new HTTPRespEntity();
            resp.code = HttpRespCode.Success;
            resp.msg = "";
            try
            {
                long userid =dal.Login(username, pwd);
                if(userid >0)
                {
                    resp.code = HttpRespCode.Success;
                    resp.msg = "登录成功";
                    resp.data = userid.ToString();
                }
                else
                {
                    resp.code = HttpRespCode.Fail;
                    resp.msg = "登录失败";
                    resp.data = "";
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("Login", ex);
                resp.code = HttpRespCode.Excp;
                resp.msg = "服务器异常";
            }
            return resp;
        }
        

    }
}
