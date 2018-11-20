using System;

namespace Jwell.Modules.Cache.Redis
{
    public sealed class RedisCache
    {
        private static readonly object objLock = new object();

        public RedisCache(long db = 0)
        {
            //db默认数量
            if (db >= 0 && db < 16)
            {
                RedisManagement.RedisClient.ChangeDb(db);
            }
            else
            {
                throw new ArgumentOutOfRangeException("db", db, "必须在[0,15]的范围");
            }
        }

        public T Get<T>(string key)
        {
            T t = default(T);
            lock (objLock) // 线程安全，单线程操作
            {
                if (RedisManagement.RedisClient.Exists(key) > 0)
                {
                    t = RedisManagement.RedisClient.Get<T>(key);
                }
            }
            return t;
        }
         
        public bool RemoveCache(string key)
        {
            bool success = false;
            lock (objLock) // 线程安全，单线程操作
            {
                success = RedisManagement.RedisClient.Remove(key);
            }
            return success;
        }

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireTime">秒为单位</param>
        /// <returns></returns>
        public bool Set<T>(string key, T value, long expireTime)
        {
            bool result = false;
            lock (objLock) // 线程安全，单线程操作
            {
                result = RedisManagement.RedisClient.Set<T>(key, value, DateTime.Now.AddSeconds(expireTime));
            }
            return result;
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsExist(string key)
        {
            bool isExist = false;
            lock (objLock) // 线程安全，单线程操作
            {
                isExist = RedisManagement.RedisClient.Exists(key) > 0;
            }
            return isExist;
        }
    }
}
