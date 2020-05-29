using System;
using System.Collections.Generic;
using System.Text;
using Blocks.Framework.Utility.TypeHelper;

namespace BlocksCore.Utility.TypeHelper
{
    public class DateTimeHelper
    {
        /// <summary>
        /// 将DateTime类型转换为String类型，如果传入的DateTime不能转换为String，返回空字符串
        /// </summary>
        /// <param name="dTime">待转换DateTime</param>
        /// <returns>返回String类型值，如果传入的DateTime不能转换为String，返回空字符串</returns>
        public static string ToDateTimeString(DateTime dTime, TimeFormatEnum timeFormatEnum)
        {

            string strDatetime = string.Empty;
            if (dTime != null && !dTime.Equals(DateTime.MaxValue) && !dTime.Equals(DateTime.MinValue))
            {
                strDatetime = dTime.ToString(SafeConvert.DateTimeFormat[(Int32)(timeFormatEnum)]);
            }
            else
            {
                strDatetime = string.Empty;
            }

            return strDatetime;
        }

        /// <summary>
        /// 将DateTime类型转换为String类型，如果传入的DateTime不能转换为String，返回空字符串
        /// </summary>
        /// <param name="dTime">待转换DateTime</param>
        /// <returns>返回String类型值，如果传入的DateTime不能转换为String，返回空字符串</returns>
        public static string ToDateTimeStringByFormat(DateTime dTime, string format)
        {

            string strDatetime = string.Empty;
            if (dTime != null && !dTime.Equals(DateTime.MaxValue) && !dTime.Equals(DateTime.MinValue))
            {
                strDatetime = dTime.ToString(format);
            }
            else
            {
                strDatetime = string.Empty;
            }

            return strDatetime;
        }

        /// <summary>
        /// 将DateTime类型转换为String类型，如果传入的DateTime不能转换为String，返回空字符串
        /// </summary>
        /// <param name="dTime">待转换DateTime</param>
        /// <returns>返回String类型值，如果传入的DateTime不能转换为String，返回空字符串</returns>
        public static string ToDateTimeStringByFormat(DateTime? dTime, string format)
        {

            string strDatetime = string.Empty;
            if (dTime.HasValue)
            {
                strDatetime = dTime.Value.ToString(format);
            }
            else
            {
                strDatetime = string.Empty;
            }

            return strDatetime;
        }


        /// <summary>
        /// 将DateTime类型转换为String类型，如果传入的DateTime不能转换为String，返回空字符串
        /// </summary>
        /// <param name="dTime">待转换DateTime</param>
        /// <returns>返回String类型值，如果传入的DateTime不能转换为String，返回空字符串</returns>
        public static string ToDateTimeString(string dTime, TimeFormatEnum timeFormatEnum)
        {
            return ToDateTimeString(SafeConvert.ToDateTime(dTime), timeFormatEnum);
        }

        /// <summary>
        /// 将DateTime类型转换为String类型，如果传入的DateTime不能转换为String，返回空字符串
        /// </summary>
        /// <param name="dTime">待转换DateTime</param>
        /// <returns>返回String类型值，如果传入的DateTime不能转换为String，返回空字符串</returns>
        public static string ToDateTimeString(DateTime? dTime, TimeFormatEnum timeFormatEnum)
        {
            return ToDateTimeString(SafeConvert.ToDateTime(dTime), timeFormatEnum);
        }
        /// <summary>
        /// 将DateTime类型转换为yyyy-MM-dd HH:mm:ss类型，如果传入的DateTime不能转换为String，返回空字符串
        /// </summary>
        /// <param name="dTime">待转换DateTime</param>
        /// <returns>返回String类型值，如果传入的DateTime不能转换为String，返回空字符串</returns>
        public static string ToDateTime(DateTime dTime)
        {
            return ToDateTimeString(dTime, TimeFormatEnum.DateTime);
        }

        /// <summary>
        /// 将DateTime类型转换为yyyy-MM-dd HH:mm:ss类型，如果传入的DateTime不能转换为String，返回空字符串
        /// </summary>
        /// <param name="dTime">待转换DateTime</param>
        /// <returns>返回String类型值，如果传入的DateTime不能转换为String，返回空字符串</returns>
        public static string ToDateTime(string dTime)
        {
            return ToDateTimeString(SafeConvert.ToDateTime(dTime), TimeFormatEnum.DateTime);
        }

        /// <summary>
        /// 将DateTime类型转换为yyyy-MM-dd HH:mm:ss类型，如果传入的DateTime不能转换为String，返回空字符串
        /// </summary>
        /// <param name="dTime">待转换DateTime</param>
        /// <returns>返回String类型值，如果传入的DateTime不能转换为String，返回空字符串</returns>
        public static string ToDateTime(DateTime? dTime)
        {
            return ToDateTimeString(SafeConvert.ToDateTime(dTime), TimeFormatEnum.DateTime);
        }


        /// <summary>
        /// 将DateTime类型转换为yyyy-MM-dd类型，如果传入的DateTime不能转换为String，返回空字符串
        /// </summary>
        /// <param name="dTime">待转换DateTime</param>
        /// <returns>返回String类型值，如果传入的DateTime不能转换为String，返回空字符串</returns>
        public static string ToDate(DateTime dTime)
        {
            return ToDateTimeString(dTime, TimeFormatEnum.Date);
        }

        /// <summary>
        /// 将DateTime类型转换为yyyy-MM-dd类型，如果传入的DateTime不能转换为String，返回空字符串
        /// </summary>
        /// <param name="dTime">待转换DateTime</param>
        /// <returns>返回String类型值，如果传入的DateTime不能转换为String，返回空字符串</returns>
        public static string ToDate(string dTime)
        {
            return ToDateTimeString(SafeConvert.ToDateTime(dTime), TimeFormatEnum.Date);
        }

        /// <summary>
        /// 将DateTime类型转换为yyyy-MM-dd类型，如果传入的DateTime不能转换为String，返回空字符串
        /// </summary>
        /// <param name="dTime">待转换DateTime</param>
        /// <returns>返回String类型值，如果传入的DateTime不能转换为String，返回空字符串</returns>
        public static string ToDate(DateTime? dTime)
        {
            return ToDateTimeString(SafeConvert.ToDateTime(dTime), TimeFormatEnum.Date);
        }


        /// <summary>
        /// 将DateTime类型转换为yyyy-MM-dd类型，如果传入的DateTime不能转换为String，返回空字符串
        /// </summary>
        /// <param name="dTime">待转换DateTime</param>
        /// <returns>返回String类型值，如果传入的DateTime不能转换为String，返回空字符串</returns>
        public static string ToMonth(DateTime dTime)
        {
            return ToDateTimeString(dTime, TimeFormatEnum.Month);
        }

        /// <summary>
        /// 将DateTime类型转换为yyyy-MM-dd类型，如果传入的DateTime不能转换为String，返回空字符串
        /// </summary>
        /// <param name="dTime">待转换DateTime</param>
        /// <returns>返回String类型值，如果传入的DateTime不能转换为String，返回空字符串</returns>
        public static string ToMonth(string dTime)
        {
            return ToDateTimeString(SafeConvert.ToDateTime(dTime), TimeFormatEnum.Month);
        }

        /// <summary>
        /// 将DateTime类型转换为yyyy-MM-dd类型，如果传入的DateTime不能转换为String，返回空字符串
        /// </summary>
        /// <param name="dTime">待转换DateTime</param>
        /// <returns>返回String类型值，如果传入的DateTime不能转换为String，返回空字符串</returns>
        public static string ToMonth(DateTime? dTime)
        {
            return ToDateTimeString(SafeConvert.ToDateTime(dTime), TimeFormatEnum.Month);
        }
        /// <summary>
        /// 将DateTime类型转换为yyyy-MM-dd HH:mm:ss.fff类型，如果传入的DateTime不能转换为String，返回空字符串
        /// </summary>
        /// <param name="dTime">待转换DateTime</param>
        /// <returns>返回String类型值，如果传入的DateTime不能转换为String，返回空字符串</returns>
        public static string ToDateTimeWithMilliseconds(DateTime dTime)
        {
            return ToDateTimeString(dTime, TimeFormatEnum.Milliseconds);
        }

        /// <summary>
        /// 将DateTime类型转换为yyyy-MM-dd HH:mm:ss.fff类型，如果传入的DateTime不能转换为String，返回空字符串
        /// </summary>
        /// <param name="dTime">待转换DateTime</param>
        /// <returns>返回String类型值，如果传入的DateTime不能转换为String，返回空字符串</returns>
        public static string ToDateTimeWithMilliseconds(string dTime)
        {
            return ToDateTimeString(SafeConvert.ToDateTime(dTime), TimeFormatEnum.Milliseconds);
        }

        /// <summary>
        /// 将DateTime类型转换为yyyy-MM-dd HH:mm:ss.fff类型，如果传入的DateTime不能转换为String，返回空字符串
        /// </summary>
        /// <param name="dTime">待转换DateTime</param>
        /// <returns>返回String类型值，如果传入的DateTime不能转换为String，返回空字符串</returns>
        public static string ToDateTimeWithMilliseconds(DateTime? dTime)
        {
            return ToDateTimeString(SafeConvert.ToDateTime(dTime), TimeFormatEnum.Milliseconds);
        }

        /// <summary>
        /// 将DateTime类型转换为HH:mm:ss类型，如果传入的DateTime不能转换为String，返回空字符串
        /// </summary>
        /// <param name="dTime">待转换DateTime</param>
        /// <returns>返回String类型值，如果传入的DateTime不能转换为String，返回空字符串</returns>
        public static string ToTime(DateTime dTime)
        {
            return ToDateTimeString(dTime, TimeFormatEnum.Time);
        }

        /// <summary>
        /// 将DateTime类型转换为HH:mm:ss类型，如果传入的DateTime不能转换为String，返回空字符串
        /// </summary>
        /// <param name="dTime">待转换DateTime</param>
        /// <returns>返回String类型值，如果传入的DateTime不能转换为String，返回空字符串</returns>
        public static string ToTime(string dTime)
        {
            return ToDateTimeString(SafeConvert.ToDateTime(dTime), TimeFormatEnum.Time);
        }

        /// <summary>
        /// 将DateTime类型转换为HH:mm:ss类型，如果传入的DateTime不能转换为String，返回空字符串
        /// </summary>
        /// <param name="dTime">待转换DateTime</param>
        /// <returns>返回String类型值，如果传入的DateTime不能转换为String，返回空字符串</returns>
        public static string ToTime(DateTime? dTime)
        {
            return ToDateTimeString(SafeConvert.ToDateTime(dTime), TimeFormatEnum.Time);
        }

        /// <summary>
        /// 将DateTime类型转换为yyyyMMdd类型，如果传入的DateTime不能转换为String，返回空字符串
        /// </summary>
        /// <param name="dTime"></param>
        /// <returns></returns>
        public static string ToDateTimeString(string dTime)
        {
            return ToDateTimeString(SafeConvert.ToDateTime(dTime), TimeFormatEnum.DateTimeName);
        }

        /// <summary>
        /// 将DateTime类型转换为yyyy-MM-dd类型带时区，如果传入的DateTime不能转换为String，返回空字符串
        /// 服务器时区
        /// </summary>
        /// <param name="dTime"></param>
        /// <returns></returns>
        public static string ToGMTDateTime(DateTime? dTime)
        {
            if (!dTime.HasValue)
                return string.Empty;
            return ToGMTDateTime(dTime.Value);
        }

        /// <summary>
        /// 将DateTime类型转换为yyyy-MM-dd类型带时区，如果传入的DateTime不能转换为String，返回空字符串
        /// 服务器时区
        /// </summary>
        /// <param name="dTime"></param>
        /// <returns></returns>
        public static string ToUTCDateTime(DateTime dTime)
        {
            return ToDateTimeString(dTime, TimeFormatEnum.UTCDateTime);
        }
    }
}
