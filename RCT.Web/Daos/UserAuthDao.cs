﻿using RCT.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 功能說明：使用者權限、作業範圍相關
/// 初版作者：20190703 李彥賢
/// 修改歷程：20190703 李彥賢 
///           需求單號：201707240447-01 
///           初版
/// </summary>
/// 
namespace RCT.Web.Daos
{
    public class UserAuthDao
    {
        /// <summary>
        /// 檢核使用者是否有使用特定功能的權限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="funcId"></param>
        /// <returns></returns>
        public String[] qryOpScope(string sysCd, string userId, string funcId)
        {
            String[] result = new string[] { };

            using (dbFGLEntities db = new dbFGLEntities())
            {      
                //AwarkMainDetailModel awarkMainDetalModel = new AwarkMainDetailModel();
                var rows = (from userRole in db.CODE_USER_ROLE
                            join role in db.CODE_ROLE on userRole.ROLE_ID equals role.ROLE_ID
                            join roleFunc in db.CODE_ROLE_FUNC on userRole.ROLE_ID.Trim() equals roleFunc.ROLE_ID.Trim()
                            join func in db.CODE_FUNC on roleFunc.FUNC_ID.Trim() equals func.FUNC_ID.Trim()
                            where userRole.USER_ID == userId
                              & func.SYS_CD == sysCd
                              & func.FUNC_URL.StartsWith(funcId)
                              & func.IS_DISABLED.Trim() == "N"
                              & role.IS_DISABLED.Trim() == "N"

                            select new //AwarkMainDetailModel
                            {
                                roleId = role.ROLE_ID,
                                funcName = func.FUNC_NAME
                            }).FirstOrDefault();

                if (rows != null)
                {
                    result = new String[] { rows.roleId.Trim(), rows.funcName.Trim() };
                }

                return result;
            }
        }
    }
}