using RCT.Web.Utility;
using RCT.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCT.Web.Service.Interface
{
    interface IRctCompRelation
    {
        #region Get
        /// <summary>
        /// 查詢資料
        /// </summary>
        /// <param name="searchModel">查詢ViwModel</param>
        /// <returns></returns>
        List<RctCompDetailViewModel> GetSearchData(RctCompSearchViewModel searchModel);
        #endregion

        #region Save
        /// <summary>
        /// 申請覆核
        /// </summary>
        /// <param name="saveData">申請覆核的資料</param>
        /// <param name="searchModel">查詢ViwModel</param>
        /// <returns></returns>
        MSGReturnModel<string> ApplyCompData(List<RctCompDetailViewModel> saveData, RctCompSearchViewModel searchModel);
        #endregion
    }
}
