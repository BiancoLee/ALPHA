﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SSO.Web;
using SSO.Web.Daos;
using SSO.Web.Models;
using SSO.Web.BO;
using SSO.Web.ViewModels;
using SSO.Web.Utils;
using SSO.Web.ActionFilter;
using Microsoft.Reporting.WebForms;
using System.Data;

/// <summary>
/// 功能說明：權限報表作業
/// 初版作者：20180115 Daiyu
/// 修改歷程：20180115 Daiyu
///           需求單號：201801230413-00
///           初版
/// </summary>
///

namespace SSO.WebControllers
{
    [Authorize]
    [CheckSessionFilterAttribute]
    public class AuthRptController : BaseController
    {

        static private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        /// <summary>
        /// 畫面初始
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            UserAuthUtil authUtil = new UserAuthUtil();

            string opScope = "";
            string funcName = "";


            String[] roleInfo = authUtil.chkUserFuncAuth(Session["UserID"].ToString(), "~/AuthRpt/");
            if (roleInfo != null && roleInfo.Length == 2)
            {
                opScope = "1";
                funcName = roleInfo[1];
            }


            ViewBag.opScope = opScope;
            ViewBag.funcName = funcName;


            return View();
        }


        /// <summary>
        /// 畫面執行"查詢"
        /// </summary>
        /// <param name="cRptType"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult loadData(String cRptType)
        {
            CodeUserDao codeUserDao = new CodeUserDao();
            List<UserMgrModel> rows = codeUserDao.qryUserRole();


            var jsonData = new { success = true, rows };
            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// 畫面執行"列印"功能
        /// </summary>
        /// <param name="cRptType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Print(String cRptType)
        {

            bool exists = System.IO.Directory.Exists(Server.MapPath("~/Temp/"));

            if (!exists)
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Temp/"));

            switch (cRptType) {
                case "userRole":
                    return printUserRole();
                case "roleFunc":
                    return printRoleFunc();
                default :
                    return null;
            }
            //if ("userRole".Equals(cRptType))
            //    return printUserRole();

            //else
            //    return printRoleFunc();


        }
        


        public class Employee
        {
            public string UnitId { get; set; }
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string WorkUnit { get; set; }
            public string WorkUnitNm { get; set; }
            public string RoleName { get; set; }
        }


        private ActionResult printRoleFunc()
        {
            CodeRoleFunctionDao codeRoleFunctionDao = new CodeRoleFunctionDao();
            List<FuncRoleModel> rptData = codeRoleFunctionDao.qryFuncRole();

  

            if (rptData.Count == 0)
            {
                return Json(new { success = false, err = "查無資料!!" });
            }
            else
            {
                String guid = Guid.NewGuid().ToString();

                try
                {
                    String htmlText = "";

                    htmlText = genRptRoleFunc(rptData);


                    ReportUtil reportUtil = new ReportUtil();
                    byte[] pdfFile = reportUtil.ConvertHtmlTextToPDF(htmlText, guid, Server.MapPath("~/Temp/"));

                    string url = Server.MapPath("~/Temp/") + guid + ".pdf";

                    return Json(new { success = true, guid = guid });

                }
                catch (Exception e)
                {
                    logger.Error("[printFuncRole]其它錯誤：" + e.ToString());
                    return Json(new { success = false, err = "其它錯誤，請洽系統管理員!!" });
                }

            }
        }

        /// <summary>
        /// 列印"使用者角色"組織權限報表
        /// </summary>
        /// <returns></returns>
        private ActionResult printUserRole()
        {
            CodeUserDao codeUserDao = new CodeUserDao();
            List<UserMgrModel> rptData = codeUserDao.qryUserRole();

            if (rptData.Count == 0)
            {
                return Json(new { success = false, err = "查無資料!!" });
            }
            else
            {
                using (DB_INTRAEntities db = new DB_INTRAEntities())
                {
                    OaEmpDao oaEmpDao = new OaEmpDao();
                    for (int i = 0; i < rptData.Count; i++) {
                        rptData[i] = oaEmpDao.getUserOaData(rptData[i], db);
                    }
                }


                    String guid = Guid.NewGuid().ToString();

                try
                {
                    String htmlText = "";

                    htmlText = genRptUserRole(rptData);


                    ReportUtil reportUtil = new ReportUtil();
                    byte[] pdfFile = reportUtil.ConvertHtmlTextToPDF(htmlText, guid, Server.MapPath("~/Temp/"));

                    string url = Server.MapPath("~/Temp/") + guid + ".pdf";

                    return Json(new { success = true, guid = guid });

                }
                catch (Exception e)
                {
                    logger.Error("[printUserRole]其它錯誤：" + e.ToString());
                    return Json(new { success = false, err = "其它錯誤，請洽系統管理員!!" });
                }

            }
        }


        /// <summary>
        /// 產生角色功能報表
        /// </summary>
        /// <param name="rptData"></param>
        /// <returns></returns>
        private String genRptRoleFunc(List<FuncRoleModel> rptData)
        {

            int totCnt = 0;

            string strTable = "<html><span style='font-size:12px;'>";


            //查出要列印的角色LIST
            List<FuncRoleModel> roleList = rptData.GroupBy(o => new { o.cRoleId, o.cRoleName, o.vMemo })
                .Select(group => new FuncRoleModel
                {
                    cRoleId = group.Key.cRoleId,
                    cRoleName = group.Key.cRoleName,
                    vMemo = group.Key.vMemo
                }).ToList<FuncRoleModel>();


            //表頭
            strTable = strTable + "<table border='0' width='100%' style='font-size:16px;'>";

            strTable += genRptHeader("funcRole", "", "");


            foreach (FuncRoleModel funcRoleModel in roleList)
            {
                totCnt++;

                strTable += "<br/>";

                strTable = strTable + "<table align='center' cellpadding='2' cellspacing='0' width='100%'  style='font-size:12px;border-top:#000000 1px solid;border-left:#000000 1px solid'>";
                strTable = strTable + "<tr>";
                strTable = strTable + "<td colspan='4' style='border-right:#000000 1px solid;' align='left' width='10%'>角色名稱：" + funcRoleModel.cRoleName.Trim() + "</td>";
                strTable = strTable + "</tr>";


                strTable = strTable + "<tr>";
                strTable = strTable + "<td colspan='4' style='border-right:#000000 1px solid;' align='left' width='10%'>備註：" + funcRoleModel.vMemo.Trim() + "</td>";
                strTable = strTable + "</tr>";

                strTable = strTable + "<tr>";
                strTable = strTable + "<td colspan='4' style='border-top:#000000 1px solid;border-right:#000000 1px solid;' align='left' width='10%'>作業功能：</td>";
                strTable = strTable + "</tr>";

                foreach (FuncRoleModel pFunc in rptData.Where(x => x.funcUrl.Trim() == ""
                                                            && x.cRoleId.Equals(funcRoleModel.cRoleId.Trim()))
                                                            .GroupBy(x => new { x.cParentFunctionID, x.cParentFunctionName })
                                                            .Select(group => new FuncRoleModel
                                                            {
                                                                cParentFunctionID = group.Key.cParentFunctionID
                                                                ,cParentFunctionName = group.Key.cParentFunctionName
                                                            }).ToList<FuncRoleModel>()

                                                            )
                {
                    int dCnt = 0;
                    strTable = strTable + "<tr>";
                    strTable = strTable + "<td colspan='4' style='border-right:#000000 1px solid;' align='left' width='10%'>　　　" + pFunc.cParentFunctionName.Trim() + "＞＞</td>";
                    strTable = strTable + "</tr>";

                    foreach (FuncRoleModel cFunc in rptData.Where(
                        x => x.cParentFunctionID.Equals(pFunc.cParentFunctionID)
                        && x.cRoleId.Equals(funcRoleModel.cRoleId.Trim())
                        ))
                    {
                        if ((dCnt % 3).CompareTo(0) == 0)
                        {
                            strTable = strTable + "<tr>";
                            strTable = strTable + "<td ></td><td >" + cFunc.cFunctionName + "</td>";
                        }
                        else if ((dCnt % 3).CompareTo(1) == 0)
                        {
                            strTable = strTable + "<td >" + cFunc.cFunctionName + "</td>";
                        }
                        else
                        {
                            strTable = strTable + "<td style='border-right:#000000 1px solid;'>" + cFunc.cFunctionName + "</td></tr>";
                        }


                        dCnt++;
                    }

                    if ((dCnt % 3).CompareTo(0) > 0)
                    {
                        strTable = strTable + "<td colspan='" + (3 - (dCnt % 3)) + "' style='border-right:#000000 1px solid;'></td>";
                        strTable = strTable + "</tr>";
                    }

                }

                strTable = strTable + "<tr>";
                strTable = strTable + "<td colspan='4' style='border-top:#000000 1px solid;border-right:#000000 1px solid;' align='left' width='10%'></td>";
                strTable = strTable + "</tr>";
                strTable = strTable + "</table>";

            }
            //表尾
            strTable += genRptFooter();

            //strTable = strTable + "</table>";

            strTable = strTable + "</span></html>";


            return strTable;

        }


        /// <summary>
        /// 使用者角色權限報表
        /// </summary>
        /// <param name="rptData"></param>
        /// <returns></returns>
        private String genRptUserRole(List<UserMgrModel> rptData)
        {

            int totCnt = 0;

            string strTable = "<html><span style='font-size:12px;'>";


            //查出要列印的單位LIST
            List<UserMgrModel> depList = rptData.GroupBy(o => new { o.cWorkUnitCode })
                .Select(group => new UserMgrModel
                {
                    cWorkUnitCode = group.Key.cWorkUnitCode
                }).OrderBy(x => x.cWorkUnitCode).ToList<UserMgrModel>();





            //查詢db_intra查單位資料
            OaDeptDao oaDeptDao = new OaDeptDao();
            Dictionary<string, string> pDepDic = new Dictionary<string, string>();


            foreach (UserMgrModel userMgrModel in depList)
            {
                if (!"".Equals(StringUtil.toString(userMgrModel.cWorkUnitCode)))
                {
                    VW_OA_DEPT oaDept = oaDeptDao.qryByDptCd(userMgrModel.cWorkUnitCode.ToUpper());

                    if (oaDept != null)
                    {
                        userMgrModel.cWorkUnitDesc = StringUtil.toString(oaDept.DPT_NAME);
                        userMgrModel.upDptCd = StringUtil.toString(oaDept.UP_DPT_CD);
                        userMgrModel.dptType = StringUtil.toString(oaDept.Dpt_type);


                        if ("04".Equals(userMgrModel.dptType))    //科
                        {
                            if (!pDepDic.ContainsKey(userMgrModel.upDptCd))
                            {
                                VW_OA_DEPT pOaDept = oaDeptDao.qryByDptCd(userMgrModel.upDptCd);
                                if (oaDept != null)
                                {
                                    if (!pDepDic.ContainsKey(StringUtil.toString(pOaDept.DPT_CD)))
                                        pDepDic.Add(StringUtil.toString(pOaDept.DPT_CD), StringUtil.toString(pOaDept.DPT_NAME));
                                }
                                else {
                                    if (!pDepDic.ContainsKey(StringUtil.toString(oaDept.UP_DPT_CD)))
                                        pDepDic.Add(StringUtil.toString(oaDept.UP_DPT_CD), "");
                                }
                                    
                            }
                        }
                        else
                        {  //部以上
                            userMgrModel.upDptCd = StringUtil.toString(oaDept.DPT_CD);

                            if(!pDepDic.ContainsKey(StringUtil.toString(oaDept.DPT_CD)))
                                pDepDic.Add(StringUtil.toString(oaDept.DPT_CD), StringUtil.toString(oaDept.DPT_NAME));
                        }
                    }
                }
            }

            List<UserMgrModel> pDepList = depList.GroupBy(o => new { o.upDptCd })
                .Select(group => new UserMgrModel
                {
                    upDptCd = group.Key.upDptCd
                }).ToList<UserMgrModel>();


            foreach (UserMgrModel pUserMgrModel in pDepList)
            {
                totCnt++;

                //表頭
                //表頭
                if (totCnt == 1)
                    strTable = strTable + "<table border='0' width='100%' style='font-size:16px;'>";
                else
                    strTable = strTable + "<table border='0' width='100%' style='font-size:16px;page-break-before:always;'>";


                string dptCd = StringUtil.toString(pUserMgrModel.upDptCd);
                string dptName = "";
                try
                {
                    dptName = pDepDic[pUserMgrModel.upDptCd];
                }
                catch (Exception e)
                {
                }
                strTable += genRptHeader("userRole", dptCd, dptName);



                foreach (UserMgrModel authRptModel in depList.Where(x => x.upDptCd == pUserMgrModel.upDptCd))
                {

                    //處理符合該"單位"的使用者資料
                    List<UserMgrModel> userList = rptData.Where(x => x.cWorkUnitCode == authRptModel.cWorkUnitCode).GroupBy(o => new { o.cUserID })
                    .Select(group => new UserMgrModel
                    {
                        cUserID = group.Key.cUserID
                    }).ToList<UserMgrModel>();

                    strTable += "<br/>";
                    strTable = strTable + "<table border='0' width='100%' style='font-size:12px;'>";
                    strTable = strTable + "<tr>";
                    strTable = strTable + "<td align='left'>" + "單位：" + StringUtil.toString(authRptModel.cWorkUnitCode)
                        + StringUtil.toString(authRptModel.cWorkUnitDesc) + "</td>";
                    strTable = strTable + "</tr>";
                    strTable = strTable + "</table>";

                    strTable = strTable + "<table align='center' cellpadding='6' cellspacing='0' width='100%'  style='font-size:12px;border-top:#000000 1px solid;border-left:#000000 1px solid'>";
                    strTable = strTable + "<tr>";

                    strTable = strTable + "<td style='border-bottom:#000000 1px solid;border-right:#000000 1px solid;' align='center' width='10%'>網路帳號</td>";
                    strTable = strTable + "<td style='border-bottom:#000000 1px solid;border-right:#000000 1px solid;' align='center'  width='10%'>中文姓名</td>";
                    //strTable = strTable + "<td style='border-bottom:#000000 1px solid;border-right:#000000 1px solid;' align='center'  width='20%'>單位</td>";
                    strTable = strTable + "<td style='border-bottom:#000000 1px solid;border-right:#000000 1px solid;' align='center'  width='15%' >角色</td>";
                    strTable = strTable + "<td style='border-bottom:#000000 1px solid;border-right:#000000 1px solid;' align='center'>備註</td>";

                    strTable = strTable + "</tr>";

                    foreach (UserMgrModel authUser in userList)
                    {


                        List<UserMgrModel> roleList = rptData.Where(x => x.cUserID == authUser.cUserID).ToList<UserMgrModel>();
                        string strRole = "";
                        foreach (UserMgrModel role in roleList)
                        {
                            strRole += role.roleName + "<br/>";
                        }


                        strTable = strTable + "<tr>";
                        strTable = strTable + "<td style='border-bottom:#000000 1px solid;border-right:#000000 1px solid;' align='center'>" + StringUtil.toString(authUser.cUserID) + "</td>";
                        strTable = strTable + "<td style='border-bottom:#000000 1px solid;border-right:#000000 1px solid;' align='center'>" + StringUtil.toString(roleList[0].cUserName) + "</td>";
                        //strTable = strTable + "<td style='border-bottom:#000000 1px solid;border-right:#000000 1px solid;' align='center'>" + StringUtil.toString(roleList[0].cWorkUnitDesc) + "</td>";
                        strTable = strTable + "<td style='border-bottom:#000000 1px solid;border-right:#000000 1px solid;' align='center'>" + strRole + "</td>";

                        strTable = strTable + "<td style='border-bottom:#000000 1px solid;border-right:#000000 1px solid;' align='center'>  </td>";
                        strTable = strTable + "</tr>";





                    }
                    strTable = strTable + "</table>";


                }

                //表尾
                strTable += genRptFooter();


            }


            strTable = strTable + "</span></html>";


            return strTable;

        }


        private String genRptHeader(String rptType, string pDetCd, string pDetName)
        {
            String strTable = "";

            strTable = strTable + "<tr rowspan= '2' align='center' >";
            if ("userRole".Equals(rptType))
            {

                strTable = strTable + "<td colspan='2' align='center'>" + "財會管理系統-使用者角色報表" + "</td>";
                strTable = strTable + "</tr>";
                strTable = strTable + "<tr>";
                strTable = strTable + "<td  colspan='2' align='center'>" + "<br/> 單位：" + pDetCd + pDetName + "</td>";

                strTable = strTable + "</tr>";

            }

            else
            {
                strTable = strTable + "<td colspan='2' align='center'>" + "財會管理系統-角色功能報表" + "</td>";
                strTable = strTable + "</tr>";
            }


            strTable = strTable + "<tr>";
            strTable = strTable + "<td>" + "<br/> 製表人：" + Session["UserName"].ToString() + "</td>";
            strTable = strTable + "<td  align='right'>" + "<br/> 製表時間：" + DateUtil.formatDateTimeDbToSc(DateTime.Now.ToString("yyyyMMdd HHmmss"), "DT") + "</td>";

            strTable = strTable + "</tr>";

            strTable = strTable + "</table>";



            return strTable;

        }


        /// <summary>
        /// 表尾
        /// </summary>
        /// <returns></returns>
        private String genRptFooter()
        {
            string strTable = "";
            strTable += "<br/>";
            strTable += "<table border='0' width='100%' style='font-size:12px;'>";
            strTable += "<tr>";
            strTable = strTable + "<td >" + "<br/> 管理者主管簽名：</td>";
            strTable = strTable + "<td  width='20%'></td>";
            strTable = strTable + "<td >" + "<br/> 管理者簽名：</td>";
            strTable = strTable + "<td width='20%'></td>";
            strTable += "</tr>";
            strTable += "</table>";


            return strTable;

        }


        public FileContentResult downloadRpt(String id)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Temp/") + id + ".pdf");


            string fullPath = Server.MapPath("~/Temp/") + id + ".pdf";
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }


            return File(fileBytes, "application/pdf", "組織權限報表.pdf");
        }
    }
}