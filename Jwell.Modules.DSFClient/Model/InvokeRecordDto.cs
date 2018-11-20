using System;

namespace Jwell.Modules.DSFClient.Model
{
    public class InvokeRecordDto
    {
        /// <summary>
        /// 调用服务标识
        /// </summary>
       public string InvokeServiceSign { get; set; }

       public string SVID { get; set; }

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
        /// 总调用次数
        /// </summary>
        public int InvokeCount { get; set; }

        /// <summary>
        /// 最近一次调用次数
        /// </summary>
        public DateTime InvokeDateTime { get; set; }
    }
}
