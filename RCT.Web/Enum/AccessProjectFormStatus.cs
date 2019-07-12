using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace RCT.Web.Enum
{
    public partial class Ref
    {
        public enum AccessProjectFormStatus
        {
            /// <summary>
            /// 登錄暫存
            /// </summary>
            [Description("登錄暫存")]
            A10,

            /// <summary>
            /// 申請覆核
            /// </summary>
            [Description("申請覆核")]
            A20,
        }
    }
}