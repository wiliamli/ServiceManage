using System;

namespace Jwell.Modules.DSFClient.Model
{
    public class CacheInvokeRecord
    {
        /// <summary>
        /// 允许被调用次数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 调用失败次数
        /// </summary>
        public int FailedCount { get; set; }

        /// <summary>
        /// 调用成功次数
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// 最近一次调用时间
        /// </summary>
        public DateTime InvokeDateTime { get; set; }
    }
}