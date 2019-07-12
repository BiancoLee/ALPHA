using RCT.Web.Models;
using RCT.Web.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RCT.Web.Service.Actual
{
    public class Common
    {
        /// <summary>
        /// get SysCode
        /// </summary>
        /// /// <param name="sysCd"></param>
        /// <param name="codeType"></param>
        /// <param name="isAll"></param>
        /// <returns></returns>
        public List<SelectOption> GetSysCode(string sysCd, string codeType, bool isAll = false)
        {
            var result = new List<SelectOption>();
            if (sysCd.IsNullOrWhiteSpace() || codeType.IsNullOrWhiteSpace())
                return result;
            using (dbFGLEntities db = new dbFGLEntities())
            {
                if (isAll)
                    result.Add(new SelectOption() { Text = "All", Value = "All" });

                result.AddRange(db.SYS_CODE.AsNoTracking()
                    .Where(x => x.SYS_CD == sysCd)
                    .Where(x => x.CODE_TYPE == codeType)
                    .OrderBy(x => x.ISORTBY)
                    .AsEnumerable()
                    .Select(x => new SelectOption()
                    {
                        Value = x.CODE,
                        Text = x.CODE_VALUE
                    }).ToList());
            }
            return result;
        }
        /// <summary>
        /// get SysCode by CodeType
        /// </summary>
        /// <param name="isAll"></param>
        /// <returns></returns>
        public List<SelectOption> GetCompList(bool isAll = false)
        {
            var result = new List<SelectOption>();
            using (dbFGLEntities db = new dbFGLEntities())
            {
                if (isAll)
                    result.Add(new SelectOption() { Text = "All", Value = "All" });

                result.AddRange(db.RCT_COMPANY_CODE.AsNoTracking()
                    .Where(x => x.is_disabled == "N")
                    .AsEnumerable()
                    .Select(x => new SelectOption()
                    {
                        Value = x.company_code,
                        Text = x.company_name,
                        Code = x.company_vat_no
                    }).ToList());
            }
            return result;
        }
    }
}