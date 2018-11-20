namespace Jwell.ServiceManage.Application.Services.Dtos
{
    public class AuthInfoDto
    {
        /// <summary>
        /// 服务编号
        /// </summary>
        public string ServiceNumber { get; set; }

        /// <summary>
        /// 服务标识
        /// </summary>
        public string ServiceSign { get; set; }

        /// <summary>
        /// 服务开发者
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// 授权状态
        /// </summary>
        public byte State { get; set; }
    }
}