using DAL;
using Entity;
using System;

namespace BLL
{
    public class LoginBLL
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("LoginBLL");
        LoginDAL dal = new LoginDAL();
        HttpTerMagBLL termagbll = new HttpTerMagBLL();
        
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="userid">用户名</param>
        /// <param name="pwd">密码-base64</param>
        /// <returns></returns>
        public LoginRespEntity Login(string username, string pwd)
        {
            LoginRespEntity resp = new LoginRespEntity();
            resp.code = HttpRespCode.Success;
            resp.msg = "";
            try
            {
                long userid =dal.Login(username, pwd);
                if(userid >0)
                {
                    resp.code = HttpRespCode.Success;
                    resp.msg = "登录成功";
                    resp.userid = userid.ToString();
                    resp.maxtertypetime=termagbll.GetMaxTerTypeModifytime().ToString(ConstValue.DateTimeFormat);
                    resp.maxbreakdowntime = termagbll.GetMaxBreakdownInfoModifytime().ToString(ConstValue.DateTimeFormat);
                }
                else
                {
                    resp.code = HttpRespCode.Fail;
                    resp.msg = "登录失败";
                    resp.userid = "";
                    resp.maxtertypetime = "";
                    resp.maxbreakdowntime = "";
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
