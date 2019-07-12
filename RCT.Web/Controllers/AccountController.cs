using RCT.Web.ActionFilter;
using RCT.Web.Daos;
using RCT.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// 功能說明：登出入作業
/// 初版作者：20190626 李彥賢
/// 修改歷程：20170626 李彥賢 
///           需求單號：
///           初版
/// ==============================================
/// 修改日期/修改人：
/// 需求單號：
/// 修改內容：
/// ==============================================
/// </summary>
/// 
namespace RCT.Web.Controllers
{
    [Authorize]
    [CheckSessionFilterAttribute]
    public class AccountController : Controller
    {
        static private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 登入的 userID
        /// </summary>
        static public string CurrentUserId
        {
            get
            {
                var httpContext = System.Web.HttpContext.Current;
                var _userId = httpContext.Session["UserID"];
                if (_userId != null)
                    return _userId.ToString();
                return null;
            }
        }

        /// <summary>
        /// 判斷是不是保管科
        /// </summary>
        static public bool CustodianFlag
        {
            get
            {
                var httpContext = System.Web.HttpContext.Current;
                var _Unit = httpContext.Session["UserUnit"];
                if (_Unit != null)
                    //return _Unit.ToString() == Properties.Settings.Default["CustodianFlag"]?.ToString(); //改為動態
                    return _Unit.ToString() == ConfigurationManager.AppSettings["CustodianFlag"];
                return false;
            }
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Login(LoginModel loginModel)
        //{
        //    logger.Info("[AccountController][Login]UserId:" + loginModel.UserId);
        //    bool hasuser = System.Web.HttpContext.Current.User != null;
        //    bool isAuthenticated = hasuser && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;

        //    if (ModelState.IsValid)
        //    {
        //        logger.Info("[AccountController][Login]IsValid");
        //        this.HttpContext.Response.RemoveOutputCacheItem(Url.Action("MenuByUser", "NavigationController"));
        //        string ADPath = System.Configuration.ConfigurationManager.AppSettings.Get("ADPath");
        //        loginModel.UserId = loginModel.UserId.ToUpper();

        //        DirectoryEntry entry = new DirectoryEntry(ADPath, loginModel.UserId, loginModel.Password);

        //        try
        //        {
        //            string objectSid = (new SecurityIdentifier((byte[])entry.Properties["objectSid"].Value, 0).Value);

        //            //AD驗證成功，檢查該user是否有系統權限
        //            CodeUserDao codeUserDao = new CodeUserDao();

        //            CODE_USER codeUser = codeUserDao.qryUserByKey(loginModel.UserId);
        //            if(codeUser != null)
        //            {
        //                if ("N".Equals(codeUser.IS_DISABLED))
        //                {
        //                    Session["UserID"] = loginModel.UserId;

        //                    Session["UserName"] = "";
        //                    Session["UserUnit"] = "";
        //                    OaEmpDao oaEmpDao = new OaEmpDao();

        //                    try
        //                    {
        //                        using (DB_INTRAEntities dbIntra = new DB_INTRAEntities())
        //                        {
        //                            V_EMPLY2 emp = oaEmpDao.qryByUsrId(loginModel.UserId, dbIntra);
        //                            if (emp != null)
        //                            {
        //                                Session["UserName"] = StringUtil.toString(emp.EMP_NAME);
        //                                Session["UserUnit"] = StringUtil.toString(emp.DPT_CD);
        //                            }
        //                        }

        //                        writeLog("I", true, loginModel.UserId, codeUser);

        //                        LoginProcess(loginModel.UserId, false);

        //                        return RedirectToAction("Index", "Home");
        //                    }
        //                    catch (Exception e)
        //                    {

        //                    }
        //                }
        //            }
        //        }
        //        catch(Exception e)
        //        {

        //        }
        //    }
        //    else
        //    {

        //    }
        //}

    //    private void writeLog(String type, bool bSuccess, String userId, CODE_USER codeUser)
    //    {
    //        CommonUtil commonUtil = new CommonUtil();
    //        //logModel
    //        Log log = new Log();

    //        log.CFUNCTION = "I".Equals(type) ? "登入作業" : "登出作業";
    //        log.CACTION = "L";
    //        log.CCONTENT = "UserId：" + userId + "| UserName：" + commonUtil.GetIPAddress() + "|" + ("I".Equals(type) ? "登入成功" : "登出成功");

    //        //PiaLogMainModel
    //        PIA_LOG_MAIN piaLogMain = new PIA_LOG_MAIN();
    //        piaLogMain.TRACKING_TYPE = "B";
    //        piaLogMain.ACCESS_ACCOUNT = userId;
    //        piaLogMain.ACCOUNT_NAME = "";
    //        piaLogMain.PROGFUN_NAME = "AccountController";
    //        piaLogMain.EXECUTION_CONTENT = userId;
    //        piaLogMain.AFFECT_ROWS = 0;
    //        piaLogMain.PIA_TYPE = "0000000000";

    //        if (bSuccess)
    //        {
    //            CodeUserDao codeUserDao = new CodeUserDao();
    //            //更新login/logout日期時間
    //            if ("I".Equals(type))
    //                codeUserDao.updateLogInOut(userId, "I");
    //            //codeUser.cLoginDateTime = DateTime.Now;
    //            else
    //                codeUserDao.updateLogInOut(userId, "O");
    //            //codeUser.cLogoutDateTime = DateTime.Now;

    //            //寫入系統LOG
    //            LogDao.Insert(log, userId);

    //            //寫入稽核軌跡
    //            //piaLogMain.ACCOUNT_NAME = codeUser.CUSERNAME;
    //            piaLogMain.EXECUTION_TYPE = "I".Equals(type) ? "LS" : "LO";
    //            piaLogMain.ACCESSOBJ_NAME = "CodeUser";
    //            PiaLogMainDao piaLogMainDao = new PiaLogMainDao();
    //            piaLogMainDao.Insert(piaLogMain);
    //        }
    //        else
    //        {
    //            //寫入系統LOG
    //            log.CCONTENT = "UserId：" + userId + "| UserName：" + commonUtil.GetIPAddress() + "|" + "登入失敗";
    //            LogDao.Insert(log, userId);

    //            //寫入稽核軌跡
    //            piaLogMain.EXECUTION_TYPE = "LF";
    //            piaLogMain.ACCESSOBJ_NAME = "AD";
    //            PiaLogMainDao piaLogMainDao = new PiaLogMainDao();
    //            piaLogMainDao.Insert(piaLogMain);

    //        }
    //    }
    }
}