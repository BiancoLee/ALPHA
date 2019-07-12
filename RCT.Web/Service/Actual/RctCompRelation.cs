using RCT.Web.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RCT.Web.ViewModels;
using RCT.Web.Models;
using RCT.Web.Utility;
using FRT.Web.Daos;
using System.Globalization;
using RCT.Web.Enum;
using FRT.Web.BO;
using System.Data.Entity.Infrastructure;

/// <summary>
/// 功能說明：Table 維護作業
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
namespace RCT.Web.Service.Actual
{
    public class RctCompRelation : Common, IRctCompRelation
    {
        public List<RctCompDetailViewModel> GetSearchData(RctCompSearchViewModel searchModel)
        {
            List<RctCompDetailViewModel> result = new List<RctCompDetailViewModel>();

            using (dbFGLEntities db = new dbFGLEntities())
            {
                var dbRCT_COMPANY_RELATION = db.RCT_COMPANY_RELATION.AsNoTracking();
                var dbRCT_COMPANY_RELATION_HIS = db.RCT_COMPANY_RELATION_HIS.AsNoTracking();
                var dbRCT_COMPANY_CODE = db.RCT_COMPANY_CODE.AsNoTracking();
                var dbRct_type = db.SYS_CODE.AsNoTracking()
                    .Where(x => x.SYS_CD == "RCT")
                    .Where(x => x.CODE_TYPE == "RECIEPT_TYPE").ToList();
                var dbPayment = db.SYS_CODE.AsNoTracking()
                    .Where(x => x.SYS_CD == "RCT")
                    .Where(x => x.CODE_TYPE == "PAYMENT_TYPE").ToList();
                var dbStatus = db.SYS_CODE.AsNoTracking()
                    .Where(x => x.SYS_CD == "RCT")
                    .Where(x => x.CODE_TYPE == "DATA_STATUS").ToList();
                var dbAction = db.SYS_CODE.AsNoTracking()
                    .Where(x => x.SYS_CD == "RCT")
                    .Where(x => x.CODE_TYPE == "EXEC_ACTION").ToList();
                var dbIsDisabled = db.SYS_CODE.AsNoTracking()
                    .Where(x => x.SYS_CD == "RCT")
                    .Where(x => x.CODE_TYPE == "IS_DISABLED").ToList();

                result = dbRCT_COMPANY_RELATION
                    .Where(x => x.reciept_type == searchModel.vRct_type_code, searchModel.vRct_type_code != "All")
                    .Where(x => x.company_code == searchModel.vCompany_code, searchModel.vCompany_code != "All")
                    .Where(x => x.is_disabled == searchModel.vIsDisabled_code, searchModel.vIsDisabled_code != "All")
                    .AsEnumerable()
                    .Select(x => new RctCompDetailViewModel
                    {
                        vRelation_id = x.relation_id,
                        vRct_type_code = x.reciept_type,
                        vRct_type_name = dbRct_type.FirstOrDefault(y => y.CODE == x.reciept_type)?.CODE_VALUE,
                        vCompany_code = x.company_code,
                        vCompany_name = dbRCT_COMPANY_CODE.FirstOrDefault(y => y.company_code == x.company_code)?.company_name,
                        vCompany_vat_no = dbRCT_COMPANY_CODE.FirstOrDefault(y => y.company_code == x.company_code)?.company_vat_no,
                        vPayment_code = x.payment_type,
                        vPayment_value = dbPayment.FirstOrDefault(y => y.CODE == x.payment_type)?.CODE_VALUE,
                        vCurrency_code = x.currency_code,
                        vData_status_code = x.data_status,
                        vData_status_value = dbStatus.FirstOrDefault(y => y.CODE == x.data_status)?.CODE_VALUE,
                        vExec_action_code = dbRCT_COMPANY_RELATION_HIS.FirstOrDefault(y => y.b_reciept_type == x.reciept_type && y.b_company_code == x.company_code && y.b_payment_type == x.payment_type && y.b_currency_code == x.currency_code)?.exec_action,
                        vExec_action_value = dbAction.FirstOrDefault(z => z.CODE == dbRCT_COMPANY_RELATION_HIS.FirstOrDefault(y => y.b_reciept_type == x.reciept_type && y.b_company_code == x.company_code && y.b_payment_type == x.payment_type && y.b_currency_code == x.currency_code)?.exec_action)?.CODE_VALUE,
                        vIsDisabled_code = x.is_disabled,
                        vIsDisabled_value = dbIsDisabled.FirstOrDefault(y => y.CODE == x.is_disabled)?.CODE_VALUE,
                        vUpdateTime = x.update_datetime
                    }).ToList();
            }
            return result;
        }

        public MSGReturnModel<string> ApplyCompData(List<RctCompDetailViewModel> saveData, RctCompSearchViewModel searchModel)
        {
            MSGReturnModel<string> result = new MSGReturnModel<string>();
            result.RETURN_FLAG = false;
            DateTime now = DateTime.Now;
            string _aply_no = string.Empty;

            try
            {
                if (saveData.Any())
                {
                    using (dbFGLEntities db = new dbFGLEntities())
                    {
                        //取得流水號
                        SysSeqDao sysSeqDao = new SysSeqDao();
                        string qPreCode = DateUtil.getCurChtDateTime(now.Year.ToString().Length);
                        var cId = sysSeqDao.qrySeqNo("RCT", "M1", qPreCode).ToString().PadLeft(3, '0');
                        _aply_no = $@"M1{qPreCode}{cId}";//M1 + 系統日期YYYMMDD(民國年) + 3碼流水號

                        foreach (var rowData in saveData)
                        {
                            var _relationId = string.Empty;
                            var _RCR = new RCT_COMPANY_RELATION();
                            #region 收據類別與公司別主檔
                            //判斷執行功能
                            switch (rowData.vExec_action_code)
                            {
                                case "A"://新增
                                    _relationId = "";
                                    break;
                                case "U": //修改
                                    _RCR = db.RCT_COMPANY_RELATION.FirstOrDefault(y => y.relation_id == rowData.vRelation_id);
                                    if (_RCR.update_datetime != null && _RCR.update_datetime > rowData.vUpdateTime)
                                    {
                                        result.DESCRIPTION = Ref.MessageType.already_Change.GetDescription();
                                        return result;
                                    }
                                    _relationId = rowData.vRelation_id;
                                    _RCR.reciept_type = rowData.vRct_type_code;
                                    _RCR.company_code = rowData.vCompany_code;
                                    _RCR.payment_type = rowData.vPayment_code;
                                    _RCR.currency_code = rowData.vCurrency_code;
                                    _RCR.update_datetime = now;
                                    _RCR.update_id = searchModel.vCurrent_Uid;
                                    _RCR.data_status = "2"; //凍結中
                                    //logStr += "|";
                                    //logStr += _RCR.modelToString();
                                    break;
                                default:
                                    break;
                            }
                            #endregion
                            #region 收據類別與公司別異動檔
                            switch (rowData.vExec_action_code)
                            {

                                case "A"://新增
                                    var _RCRH = new RCT_COMPANY_RELATION_HIS()
                                    {
                                        aply_no = _aply_no,
                                        relation_id = _relationId,
                                        exec_action = rowData.vExec_action_code,
                                        a_reciept_type = rowData.vRct_type_code,
                                        a_company_code = rowData.vCompany_code,
                                        a_payment_type = rowData.vPayment_code,
                                        a_currency_code = rowData.vCurrency_code,
                                        a_is_disabled = rowData.vIsDisabled_code,
                                        apply_status = "1", //表單申請
                                        apply_id = searchModel.vCurrent_Uid,
                                        apply_datetime = now
                                    };
                                    db.RCT_COMPANY_RELATION_HIS.Add(_RCRH);
                                    break;
                                case "U": //修改
                                    var db_RCR = db.RCT_COMPANY_RELATION.AsNoTracking().FirstOrDefault(y => y.relation_id == rowData.vRelation_id);
                                    if (db_RCR != null)
                                    {
                                        var _RCRHU = new RCT_COMPANY_RELATION_HIS()
                                        {
                                            aply_no = _aply_no,
                                            relation_id = rowData.vRelation_id,
                                            exec_action = rowData.vExec_action_code,
                                            b_reciept_type = db_RCR.reciept_type,
                                            b_company_code = db_RCR.company_code,
                                            b_payment_type = db_RCR.payment_type,
                                            b_currency_code = db_RCR.currency_code,
                                            b_is_disabled = db_RCR.is_disabled,
                                            a_reciept_type = rowData.vRct_type_code,
                                            a_company_code = rowData.vCompany_code,
                                            a_payment_type = rowData.vPayment_code,
                                            a_currency_code = rowData.vCurrency_code,
                                            a_is_disabled = rowData.vIsDisabled_code,
                                            apply_datetime = now,
                                            apply_id = searchModel.vCurrent_Uid,
                                            apply_status = "1" //表單申請
                                        };
                                        db.RCT_COMPANY_RELATION_HIS.Add(_RCRHU);
                                        //logStr += "|";
                                        //logStr += _RCRHU.modelToString();
                                    }
                                    break;
                            }
                            #endregion
                        }
                        var validateMessage = db.GetValidationErrors().getValidateString();
                        if (validateMessage.Any())
                        {
                            result.DESCRIPTION = validateMessage;
                        }
                        else
                        {
                            try
                            {
                                db.SaveChanges();

                                #region LOG
                                ////申請覆核LOG
                                //Log log = new Log();
                                //log.CFUNCTION = "申請覆核-收據類別與公司別";
                                //log.CACTION = "A";
                                //log.CCONTENT = logStr;
                                //LogDao.Insert(log, searchModel.vCurrent_Uid);
                                #endregion

                                result.RETURN_FLAG = true;
                                result.DESCRIPTION = Ref.MessageType.Apply_Audit_Success.GetDescription(null, $@"單號為{_aply_no}");
                            }
                            catch (DbUpdateException ex)
                            {
                                result.DESCRIPTION = ex.exceptionMessage();
                            }
                        }
                    }
                }
                else
                {
                    result.DESCRIPTION = Ref.MessageType.not_Find_Audit_Data.GetDescription();
                }
            }
            catch (Exception ex)
            {
                result.DESCRIPTION = ex.exceptionMessage();
            }
            return result;
        }
    }
}