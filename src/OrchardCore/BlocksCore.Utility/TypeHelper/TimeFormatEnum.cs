using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Utility.TypeHelper
{
    public enum TimeFormatEnum
    {
        /// <summary>
        /// 时间格式yyyy-MM-dd
        /// </summary>
        Date = 0,

        /// <summary>
        /// 时间格式yyyy-MM-dd HH:mm:ss
        /// </summary>
        DateTime = 1,

        /// <summary>
        /// 时间格式yyyy-MM-dd HH:mm:ss.fff
        /// </summary>
        Milliseconds = 2,

        /// <summary>
        /// 时间格式HH:mm:ss
        /// </summary>
        Time = 3,

        /// <summary>
        /// 时间格式yyMMdd
        /// </summary>
        DateName = 4,

        /// <summary>
        /// 时间格式yy
        /// </summary>
        Year = 5,

        /// <summary>
        /// 时间格式yyyyMMdd
        /// </summary>
        DateTimeName = 6,

        /// <summary>
        /// 时间格式HHmmss
        /// </summary>
        ShortTime = 7,

        /// <summary>
        /// 时间格式yyyyMM
        /// </summary>
        DateMonthName = 8,

        /// <summary>
        /// yyMM
        /// </summary>
        ShortYearAndMonth = 9,

        /// <summary>
        /// yyyyMMddHHmmss
        /// </summary>
        DateTimeFileName = 10,
        /// <summary>
        /// yyyyMMddHHmmssfff
        /// </summary>
        DateTimeVersionName = 11,

        Month = 12,

        UTCDateTime = 13,

    }
}
