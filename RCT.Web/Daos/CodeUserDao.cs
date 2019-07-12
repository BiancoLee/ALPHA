using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;

/// <summary>
/// 功能說明：
/// 初版作者：20171023 黃黛鈺
/// 修改歷程：20171023 黃黛鈺 
///           需求單號：201707240447-01 
///           初版
/// ==============================================
/// 修改日期/修改人：20180221 黃黛鈺 
/// 需求單號：201801230413-00 
/// 修改內容：加入覆核功能
/// ==============================================
/// </summary>
/// 

namespace RCT.Web.Daos
{
    public class CodeUserDao
    {
        /// <summary>
        /// 以鍵項查詢使用者資料
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        //public CODE_USER qryUserByKey(String userId)
        //{
        //    using (new TransactionScope(
        //           TransactionScopeOption.Required,
        //           new TransactionOptions
        //           {
        //               IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
        //           }))
        //    {
        //        using (dbFGLEntities db = new dbFGLEntities())
        //        {
        //            CODE_USER codeUser = db.CODE_USER.Where(x => x.USER_ID == userId).FirstOrDefault<CODE_USER>();

        //            return codeUser;
        //        }
        //    }
        //}


    }
}