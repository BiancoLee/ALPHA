using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace RCT.Web.ViewModels
{
    public class RctCompSearchViewModel
    {
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
        /// 作廢否代碼
        /// </summary>
        [Description("作廢否代碼")]
        public string vIsDisabled_code { get; set; }
        
        /// <summary>
        /// 目前使用者
        /// </summary>
        [Description("目前使用者")]
        public string vCurrent_Uid { get; set; }
    }
}