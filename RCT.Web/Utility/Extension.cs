using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace RCT.Web.Utility
{
    public static class Extension
    {
        /// <summary>
        /// 產生ChcekBox 文字
        /// </summary>
        public static StringBuilder CheckBoxString(string name, List<CheckBoxListInfo> listInfo, IDictionary<string, object> htmlAttributes, int number)
        {
            StringBuilder sb = new StringBuilder();
            int lineNumber = 0;
            sb.Append("<table style='width:100%;'><tr>");
            foreach (CheckBoxListInfo info in listInfo)
            {
                lineNumber++;
                TagBuilder builder = new TagBuilder("input");
                if (info.IsChecked)
                {
                    builder.MergeAttribute("checked", "checked");
                }
                builder.MergeAttributes<string, object>(htmlAttributes);
                builder.MergeAttribute("type", "checkbox");
                builder.MergeAttribute("value", info.Value);
                builder.MergeAttribute("class", "styled");
                builder.InnerHtml = string.Format(" <label>{0}</label> ", info.DisplayText);
                sb.Append(
                    string.Format("<td><div class='checkbox checkbox-info'>{0}</div></td>",
                    builder.ToString(TagRenderMode.Normal)));
                if (number == 0)
                {
                    //sb.Append("<br />");
                    sb.Append("</tr><tr>");
                }
                else if (lineNumber % number == 0)
                {
                    //sb.Append("<br />");
                    sb.Append("</tr><tr>");
                }
            }
            if (number == 0 || lineNumber % number == 0)
                sb.Remove(sb.Length - 4, 4);
            else
                sb.Append("</tr>");
            sb.Append("</table>");
            return sb;
        }

            /// <summary>
            /// 錯誤訊息
            /// </summary>
            /// <param name="ex"></param>
            /// <returns></returns>
        public static string exceptionMessage(this Exception ex)
        {
            return $"message: {ex.Message}" +
                   $", inner message {ex.InnerException}";
            //$", inner message {ex.InnerException?.InnerException?.Message}";
        }

        /// <summary>
        /// 判斷文字是否為Null 或 空白
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace
                                    (this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static IQueryable<T> Where<T>
           (this IQueryable<T> source, Expression<Func<T, bool>> predicate, bool flag)
        {
            if (flag)
                return source.Where(predicate);
            return source;
        }

        public static IEnumerable<T> Where<T>
            (this IEnumerable<T> source, Func<T, bool> predicate, bool flag)
        {
            if (flag)
                return source.Where(predicate);
            return source;
        }

        public static string GetDescription<T>(this T enumerationValue, string title = null, string body = null)
            where T : struct
        {
            var type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException($"{nameof(enumerationValue)} must be of Enum type", nameof(enumerationValue));
            }

            var memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo.Length > 0)
            {
                var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs.Length > 0)
                {
                    if (!title.IsNullOrWhiteSpace() && !body.IsNullOrWhiteSpace())
                        return string.Format("{0} : {1} => {2}",
                            title,
                            ((DescriptionAttribute)attrs[0]).Description,
                            body
                            );
                    if (!title.IsNullOrWhiteSpace())
                        return string.Format("{0} : {1}",
                            title,
                            ((DescriptionAttribute)attrs[0]).Description
                            );
                    if (!body.IsNullOrWhiteSpace())
                        return string.Format("{0} => {1}",
                            ((DescriptionAttribute)attrs[0]).Description,
                            body
                            );
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return enumerationValue.ToString();
        }

        public static string formateThousand(this string value)
        {
            decimal d = 0;
            try
            {
                if (decimal.TryParse(value, out d))
                {
                    if (value.IndexOf(".") > -1)
                    {
                        Int64 strNumberWithoutDecimals = Convert.ToInt64(value.Substring(0, value.IndexOf(".")).Replace(",", ""));
                        string strNumberDecimals = value.Substring(value.IndexOf("."));
                        return strNumberWithoutDecimals.ToString("#,##0") + strNumberDecimals;
                    }
                    return Convert.ToInt64(value.Replace(",", "")).ToString("#,##0");
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return value;
        }

        public static string getValidateString(this IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors)
        {
            string result = string.Empty;
            if (errors.Any())
                result = string.Join(" ", errors.Select(y => string.Join(",", y.ValidationErrors.Select(z => z.ErrorMessage))));
            return result;
        }
    }
}
