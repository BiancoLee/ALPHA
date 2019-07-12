using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace RCT.Web.ViewModels
{
    public class RctCompDetailViewModel
    {
        /// <summary>
        /// 流水號
        /// </summary>
        [Description("流水號")]

        public string vRelation_id { get; set; }
        /// <summary>
        /// 收據類別編號
        /// </summary>
        [Description("收據類別編號")]
        public string vRct_type_code { get; set; }

        /// <summary>
        /// 收據類別名稱
        /// </summary>
        [Description("收據類別名稱")]
        public string vRct_type_name { get; set; }

        /// <summary>
        /// 公司代碼
        /// </summary>
        [Description("公司代碼")]
        public string vCompany_code { get; set; }

        /// <summary>
        /// 公司名稱
        /// </summary>
        [Description("公司名稱")]
        public string vCompany_name { get; set; }

        /// <summary>
        /// 統一編號
        /// </summary>
        [Description("統一編號")]
        public string vCompany_vat_no { get; set; }

        /// <summary>
        /// 繳款方式
        /// </summary>
        [Description("繳款方式代碼")]
        public string vPayment_code { get; set; }

        /// <summary>
        /// 繳款方式
        /// </summary>
        [Description("繳款方式")]
        public string vPayment_value { get; set; }

        /// <summary>
        /// 付款幣別
        /// </summary>
        [Description("付款幣別")]
        public string vCurrency_code { get; set; }

        /// <summary>
        /// 資料狀態代碼
        /// </summary>
        [Description("資料狀態代碼")]
        public string vData_status_code { get; set; }

        /// <summary>
        /// 資料狀態名稱
        /// </summary>
        [Description("資料狀態")]
        public string vData_status_value { get; set; }

        /// <summary>
        /// 執行功能
        /// </summary>
        [Description("執行功能代碼")]
        public string vExec_action_code { get; set; }

        /// <summary>
        /// 執行功能名稱
        /// </summary>
        [Description("執行功能")]
        public string vExec_action_value { get; set; }

        /// <summary>
        /// 作廢否
        /// </summary>
        [Description("作廢否代碼")]
        public string vIsDisabled_code { get; set; }

        /// <summary>
        /// 作廢否
        /// </summary>
        [Description("作廢否")]
        public string vIsDisabled_value { get; set; }

        /// <summary>
        /// 修改時間
        /// </summary>
        [Description("修改時間")]
        public DateTime? vUpdateTime { get; set; } 
    }
}