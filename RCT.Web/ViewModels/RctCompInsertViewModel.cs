using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace RCT.Web.ViewModels
{
    public class RctCompInsertViewModel
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
        /// 公司代碼
        /// </summary>
        [Description("公司代碼")]
        public string vCompany_code { get; set; }

        /// <summary>
        /// 繳款方式
        /// </summary>
        [Description("繳款方式代碼")]
        public string vPayment_code { get; set; }

        /// <summary>
        /// 付款幣別
        /// </summary>
        [Description("付款幣別")]
        public string vCurrency_code { get; set; }

        /// <summary>
        /// 作廢否
        /// </summary>
        [Description("作廢否代碼")]
        public string vIsDisabled_code { get; set; }

        /// <summary>
        /// RowId
        /// </summary>
        [Description("RowId")]
        public string vRowId { get; set; }       
    }
}