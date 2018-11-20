using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwell.Modules.Cache.Redis
{
    /// <summary>
    /// 主要是为后期，由统一配置根据系统分配服务器IP，账户等
    /// 开数据库访问
    /// 目前先由非静态处理，再到后期单例
    /// </summary>
    public class RedisManagement
    {
        public static RedisClient RedisClient { get; private set; }

        #region 静态单例 
        static RedisManagement()
        {
            RedisClient = new RedisClient(RedisConstant.IP, RedisConstant.PORT, RedisConstant.PASSWORD);
        }
        #endregion
    }
}
