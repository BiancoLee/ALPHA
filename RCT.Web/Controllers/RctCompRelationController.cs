using RCT.Web.ActionFilter;
using RCT.Web.Enum;
using RCT.Web.Service.Actual;
using RCT.Web.Service.Interface;
using RCT.Web.Utility;
using RCT.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// 功能說明：收據與公司維護作業
/// 初版作者：201906028 李彥賢
/// 修改歷程：201906028 李彥賢 
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
    public class RctCompRelationController : CommonController
    {
        private IRctCompRelation RctCompRelation;

        public RctCompRelationController()
        {
            RctCompRelation = new RctCompRelation();
        }

        static private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 畫面初始
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            string[] opScopeFuncName = GetopScope("~/RctCompRelation/");

            ViewBag.opScope = opScopeFuncName[0];
            ViewBag.funcName = opScopeFuncName[1];
            ViewBag.dRecieptType = new SelectList(new Common().GetSysCode("RCT", "RECIEPT_TYPE", true), "Value", "Text");
            ViewBag.dRecieptTypeMod = new SelectList(new Common().GetSysCode("RCT", "RECIEPT_TYPE", false), "Value", "Text");
            ViewBag.dCompany = new SelectList(new Common().GetCompList(true), "Value", "Text");
            ViewBag.dCompanyMod = new SelectList(new Common().GetCompList(false), "Value", "Text");
            ViewBag.dIsDisabled = new SelectList(new Common().GetSysCode("RCT", "IS_DISABLED", true), "Value", "Text");
            ViewBag.dIsDisabledMod = new SelectList(new Common().GetSysCode("RCT", "IS_DISABLED", false), "Value", "Text");
            ViewBag.dPaymentTypeMod = new SelectList(new Common().GetSysCode("RCT", "PAYMENT_TYPE", false), "Value", "Text");
            ViewBag.dCurrencyMod = new SelectList(new Common().GetSysCode("RCT", "CURRENCY_CODE", false), "Value", "Value");

            return View();
        }

        /// <summary>
        /// 表單查詢
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SearchData(RctCompSearchViewModel searchModel)
        {
            MSGReturnModel<string> result = new MSGReturnModel<string>();
            result.RETURN_FLAG = false;
            result.DESCRIPTION = Ref.MessageType.query_Not_Find.GetDescription();
            Cache.Invalidate(CacheList.RctCompSearchData);
            Cache.Set(CacheList.RctCompSearchData, searchModel);

            var datas = RctCompRelation.GetSearchData(searchModel);

            Cache.Invalidate(CacheList.RctCompViewData);
            Cache.Set(CacheList.RctCompViewData, datas);
            result.RETURN_FLAG = true;

            return Json(result);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult InsertData(RctCompInsertViewModel model)
        {
            MSGReturnModel<string> result = new MSGReturnModel<string>();
            result.RETURN_FLAG = false;
            result.DESCRIPTION = Ref.MessageType.insert_Fail.GetDescription();
            var _RCTType = new Common().GetSysCode("RCT", "RECIEPT_TYPE", false);
            var _company = new Common().GetCompList(false);
            var _payment = new Common().GetSysCode("RCT", "PAYMENT_TYPE", false);
            var _action = new Common().GetSysCode("RCT", "EXEC_ACTION", false);
            var _status = new Common().GetSysCode("RCT", "DATA_STATUS", false);
            var _isDisabled = new Common().GetSysCode("RCT", "IS_DISABLED", false);

            if (Cache.IsSet(CacheList.RctCompViewData))
            {
                var tempData = (List<RctCompDetailViewModel>)Cache.Get(CacheList.RctCompViewData);
                if (tempData != null)
                {
                    tempData.ForEach(x =>
                    {
                        if (x.vRct_type_code == model.vRct_type_code && x.vCompany_code == model.vCompany_code && x.vPayment_code == model.vPayment_code && x.vCurrency_code == model.vCurrency_code)
                        {
                            result.DESCRIPTION = Ref.MessageType.Already_Same_Data.GetDescription();
                        }
                    });
                }

                if (result.DESCRIPTION != Ref.MessageType.Already_Same_Data.GetDescription())
                {
                    RctCompDetailViewModel insertData = new RctCompDetailViewModel()
                    {
                        vRelation_id = model.vRelation_id,
                        vRct_type_code = model.vRct_type_code,
                        vRct_type_name = _RCTType.FirstOrDefault(x => x.Value == model.vRct_type_code)?.Text?.Trim(),
                        vCompany_code = model.vCompany_code,
                        vCompany_name = _company.FirstOrDefault(x => x.Value == model.vCompany_code)?.Text?.Trim(),
                        vCompany_vat_no = _company.FirstOrDefault(x => x.Value == model.vCompany_code)?.Code?.Trim(),
                        vPayment_code = model.vPayment_code,
                        vPayment_value = _payment.FirstOrDefault(x => x.Value == model.vPayment_code)?.Text?.Trim(),
                        vCurrency_code = model.vCurrency_code,
                        vExec_action_code = "A",
                        vExec_action_value = _action.FirstOrDefault(x => x.Value == "A")?.Text?.Trim(),
                        vData_status_code = "1",
                        vData_status_value = _status.FirstOrDefault(x => x.Value == "1")?.Text?.Trim(),
                        vIsDisabled_code = model.vIsDisabled_code,
                        vIsDisabled_value = _isDisabled.FirstOrDefault(x => x.Value == model.vIsDisabled_code)?.Text?.Trim(),
                    };

                    tempData.Insert(0, insertData);
                    Cache.Invalidate(CacheList.RctCompViewData);
                    Cache.Set(CacheList.RctCompViewData, tempData);
                    result.RETURN_FLAG = true;
                    result.DESCRIPTION = Ref.MessageType.insert_Success.GetDescription();
                }
            }
            return Json(result);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateData(RctCompInsertViewModel model)
        {
            MSGReturnModel<string> result = new MSGReturnModel<string>();
            result.RETURN_FLAG = false;
            result.DESCRIPTION = Ref.MessageType.login_Time_Out.GetDescription();

            var _RCTType = new Common().GetSysCode("RCT", "RECIEPT_TYPE", false);
            var _company = new Common().GetCompList(false);
            var _payment = new Common().GetSysCode("RCT", "PAYMENT_TYPE", false);
            var _action = new Common().GetSysCode("RCT", "EXEC_ACTION", false);
            var _isDisabled = new Common().GetSysCode("RCT", "IS_DISABLED", false);
            int rid;

            if (Cache.IsSet(CacheList.RctCompViewData))
            {
                var tempData = (List<RctCompDetailViewModel>)Cache.Get(CacheList.RctCompViewData);

                if (tempData != null)
                {
                    tempData.ForEach(x =>
                    {
                        if (x.vRct_type_code == model.vRct_type_code && x.vCompany_code == model.vCompany_code && x.vPayment_code == model.vPayment_code && x.vCurrency_code == model.vCurrency_code)
                        {
                            result.DESCRIPTION = Ref.MessageType.Already_Same_Data.GetDescription();
                        }
                    });
                }

                if (result.DESCRIPTION == Ref.MessageType.Already_Same_Data.GetDescription())
                {
                    return Json(result);
                }

                var rowData = tempData.FirstOrDefault(x => x.vRelation_id == model.vRelation_id);

                if (rowData != null)
                {
                    rowData.vRct_type_code = model.vRct_type_code;
                    rowData.vRct_type_name = _RCTType.FirstOrDefault(x => x.Value == model.vRct_type_code)?.Text?.Trim();
                    rowData.vCompany_code = model.vCompany_code;
                    rowData.vCompany_name = _company.FirstOrDefault(x => x.Value == model.vCompany_code)?.Text?.Trim();
                    rowData.vCompany_vat_no = _company.FirstOrDefault(x => x.Value == model.vCompany_code)?.Code?.Trim();
                    rowData.vPayment_code = model.vPayment_code;
                    rowData.vPayment_value = _payment.FirstOrDefault(x => x.Value == model.vPayment_code)?.Text?.Trim();
                    rowData.vExec_action_code = rowData.vExec_action_code == "A" ? "A" : "U";
                    rowData.vExec_action_value = rowData.vExec_action_code == "A" ? _action.FirstOrDefault(x => x.Value == "A")?.Text?.Trim() : _action.FirstOrDefault(x => x.Value == "U")?.Text?.Trim();
                    rowData.vIsDisabled_code = model.vIsDisabled_code;
                    rowData.vIsDisabled_value = _isDisabled.FirstOrDefault(x => x.Value == model.vIsDisabled_code)?.Text?.Trim();

                    Cache.Invalidate(CacheList.RctCompViewData);
                    Cache.Set(CacheList.RctCompViewData, tempData);
                    result.RETURN_FLAG = true;
                    result.DESCRIPTION = Ref.MessageType.update_Success.GetDescription();
                }
                else
                {
                    result.RETURN_FLAG = false;
                    result.DESCRIPTION = Ref.MessageType.update_Fail.GetDescription();
                }
            }
            return Json(result);
        }

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult DeleteData(RctCompInsertViewModel model)
        {
            MSGReturnModel<bool> result = new MSGReturnModel<bool>();
            result.RETURN_FLAG = false;
            result.DESCRIPTION = Ref.MessageType.login_Time_Out.GetDescription();
            var _action = new Common().GetSysCode("RCT", "EXEC_ACTION", false);
            int rid;
            if (Cache.IsSet(CacheList.RctCompViewData))
            {
                var tempData = (List<RctCompDetailViewModel>)Cache.Get(CacheList.RctCompViewData);
                var rowData = tempData.FirstOrDefault(x => x.vRelation_id == model.vRelation_id);

                if (rowData != null)
                {
                    //判斷是否新增資料
                    if (rowData.vExec_action_code == "A")
                        tempData.Remove(rowData);
                    else
                    {
                        rowData.vExec_action_code = "D";
                        rowData.vExec_action_value = _action.FirstOrDefault(x => x.Value == "U")?.Text?.Trim();
                    }
                    Cache.Invalidate(CacheList.RctCompViewData);
                    Cache.Set(CacheList.RctCompViewData, tempData);
                    result.RETURN_FLAG = true;
                    result.DESCRIPTION = Ref.MessageType.delete_Success.GetDescription();
                    result.Datas = tempData.Where(x => x.vExec_action_code != null).Any();
                }
                else
                {
                    result.RETURN_FLAG = false;
                    result.DESCRIPTION = Ref.MessageType.update_Fail.GetDescription();
                }

            }
            return Json(result);
        }

        /// <summary>
        /// 申請覆核
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ApplyData()
        {
            MSGReturnModel<string> result = new MSGReturnModel<string>();
            if (Cache.IsSet(CacheList.RctCompSearchData) && Cache.IsSet(CacheList.RctCompViewData))
            {
                var searchModel = (RctCompSearchViewModel)Cache.Get(CacheList.RctCompSearchData);
                var _data = (List<RctCompDetailViewModel>)Cache.Get(CacheList.RctCompViewData);
                searchModel.vCurrent_Uid = AccountController.CurrentUserId;
                result = RctCompRelation.ApplyCompData(_data.Where(x => x.vExec_action_code != null && x.vData_status_code == "1").ToList(), searchModel);
            }
            else
            {
                result.RETURN_FLAG = false;
                result.DESCRIPTION = Ref.MessageType.Apply_Audit_Fail.GetDescription();
            }
            return Json(result);
        }

        /// <summary>
        /// 取消申請(清空tempData)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ResetData()
        {
            MSGReturnModel<string> result = new MSGReturnModel<string>();
            var searchModel = (RctCompSearchViewModel)Cache.Get(CacheList.RctCompSearchData);
            var _data = RctCompRelation.GetSearchData(searchModel);
            Cache.Invalidate(CacheList.RctCompViewData);
            Cache.Set(CacheList.RctCompViewData, _data);

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetCacheData(jqGridParam jdata, string type)
        {
            switch (type)
            {
                case "Search":
                    var Datas = (List<RctCompDetailViewModel>)Cache.Get(CacheList.RctCompViewData);
                    return Json(jdata.modelToJqgridResult(Datas.OrderBy(x => x.vRct_type_code).ToList()));
            }
            return null;
        }
    }
}