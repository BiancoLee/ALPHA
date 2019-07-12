using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RCT.Web.Utility
{
    /// <summary>
    /// cache命名 目的為不重複cache名稱 避免資料被覆蓋
    /// </summary>
    public static class CacheList
    {
        #region Cache資料
        /// <summary>
        /// 收據與公司維護作業查詢條件
        /// </summary>
        public static string RctCompSearchData { get; private set; }
        /// <summary>
        /// 收據與公司維護作業查詢結果
        /// </summary>
        public static string RctCompViewData { get; private set; }
        #endregion Cache資料
        static CacheList()
        {
            #region Cache資料
            RctCompSearchData = "RctCompSearchData";
            RctCompViewData = "RctCompViewData";
            #endregion Cache資料
        }
    }
}