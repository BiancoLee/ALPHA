using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace RCT.Web.Enum
{
    public partial class Ref
    {
        /// <summary>
        /// 回傳訊息格式統一
        /// </summary>
        public enum MessageType
        {
            /// <summary>
            /// 資料已經儲存過了
            /// </summary>
            [Description("資料已經儲存過了!")]
            already_Save,

            /// <summary>
            /// 資料已異動(請重新查詢)
            /// </summary>
            [Description("資料已異動(請重新查詢)")]
            already_Change,

            /// <summary>
            /// 新增成功
            /// </summary>
            [Description("新增成功!")]
            insert_Success,

            /// <summary>
            /// 新增失敗
            /// </summary>
            [Description("新增失敗!")]
            insert_Fail,

            /// <summary>
            /// 儲存成功
            /// </summary>
            [Description("儲存成功!")]
            save_Success,

            /// <summary>
            /// 儲存失敗
            /// </summary>
            [Description("儲存失敗!")]
            save_Fail,

            /// <summary>
            /// 修改成功
            /// </summary>
            [Description("修改成功!")]
            update_Success,

            /// <summary>
            /// 修改失敗
            /// </summary>
            [Description("修改失敗!")]
            update_Fail,

            /// <summary>
            /// 刪除成功
            /// </summary>
            [Description("刪除成功!")]
            delete_Success,

            /// <summary>
            /// 刪除失敗
            /// </summary>
            [Description("刪除失敗!")]
            delete_Fail,

            /// <summary>
            /// 無更新資料
            /// </summary>
            [Description("無更新資料!")]
            not_Find_Update_Data,

            /// <summary>
            /// 無申請覆核資料
            /// </summary>
            [Description("無申請覆核資料!")]
            not_Find_Audit_Data,

            /// <summary>
            /// 沒有找到搜尋的資料
            /// </summary>
            [Description("沒有找到搜尋的資料!")]
            query_Not_Find,

            /// <summary>
            /// 下載成功
            /// </summary>
            [Description("下載成功!")]
            download_Success,

            /// <summary>
            /// 下載失敗
            /// </summary>
            [Description("下載失敗!")]
            download_Fail,

            /// <summary>
            /// 上傳失敗
            /// </summary>
            [Description("上傳失敗!")]
            upload_Fail,

            /// <summary>
            /// 請選擇上傳檔案
            /// </summary>
            [Description("請選擇上傳檔案!")]
            upload_Not_Find,

            /// <summary>
            /// 無比對到資料!
            /// </summary>
            [Description("無比對到資料!")]
            data_Not_Compare,

            /// <summary>
            /// 傳入參數錯誤!
            /// </summary>
            [Description("傳入參數錯誤!")]
            parameter_Error,

            /// <summary>
            /// 時間停滯太久請重新上一動作!
            /// </summary>
            [Description("時間停滯太久請重新上一動作!")]
            time_Out,

            /// <summary>
            /// 申請覆核成功
            /// </summary>
            [Description("申請覆核成功!")]
            Apply_Audit_Success,

            /// <summary>
            /// 申請覆核失敗
            /// </summary>
            [Description("申請覆核失敗!")]
            Apply_Audit_Fail,

            /// <summary>
            /// 覆核成功
            /// </summary>
            [Description("覆核成功!")]
            Audit_Success,

            /// <summary>
            /// 覆核失敗
            /// </summary>
            [Description("覆核失敗!")]
            Audit_Fail,

            /// <summary>
            /// 閒置太久請重新登入!
            /// </summary>
            [Description("閒置太久請重新登入!")]
            login_Time_Out,

            /// <summary>
            /// 已有重複的相同資料!
            /// </summary>
            [Description("已有重複的相同資料!")]
            Already_Same_Data,

            /// <summary>
            /// 暫存資料尚未申請覆核,是否繼續動作?
            /// </summary>
            [Description("暫存資料尚未申請覆核,是否繼續動作?")]
            application_Audit_Confirm,
        }
    }
}