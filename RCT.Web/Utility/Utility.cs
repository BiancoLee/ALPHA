using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RCT.Web.Utility
{
    public class SelectOption
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public string Code { get; set; }
    }

    public class CheckBoxListInfo
    {
        public string DisplayText { get; set; }
        public bool IsChecked { get; set; }
        public string Value { get; set; }
    }
}