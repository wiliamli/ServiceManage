namespace Jwell.Modules.DSFClient
{
    /// <summary>
    /// 证书
    /// </summary>
    public class Act
    {
        /// <summary>
        /// 调用者服务标识
        /// </summary>
        public string ServiceSign { get; set; }

        /// <summary>
        /// 调用者服务编码
        /// </summary>
        public string ServiceNumber { get; set; }

        public Act(string serviceSign,string serviceNumber)
        {
            ServiceSign = serviceSign;
            ServiceNumber = serviceNumber;
        }
    }
}
