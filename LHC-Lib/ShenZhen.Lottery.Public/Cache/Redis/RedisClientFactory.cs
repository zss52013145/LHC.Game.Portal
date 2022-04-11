using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenZhen.Lottery.Public.Cache.Redis
{
   public static class RedisClientFactory
    {
        private static RedisConfigInfo _redisConfig = RedisConfigs.GetConfig();
        private static PooledRedisClientManager _prcm;

        static RedisClientFactory()
        {
            CreateManager();
        }

        #region -- 连接信息 --
        public static IRedisClient Client
        {
            get
            {
                try
                {
                    if (_prcm == null)
                    {
                        CreateManager();
                    }

                    return _prcm.GetClient();
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("初始化Redis客户端实例失败" + ex);

                    throw ex;
                }
            }
        }


        /// <summary>
        /// 创建链接池管理对象
        /// </summary>
        private static void CreateManager()
        {
            //IP地址中可以加入auth验证 password@ip: port

           // string[] readWriteHosts =new string[] { "dingsheng@10.10.10.239:6379" };

            string[] readWriteHosts = SplitString(_redisConfig.WriteServerList, ",");
            string[] readOnlyHosts = SplitString(_redisConfig.ReadServerList, ",");

            // 支持读写分离，均衡负载  
            PooledRedisClientManager manager = new PooledRedisClientManager(readWriteHosts, readOnlyHosts, new RedisClientManagerConfig
            {
                DefaultDb = _redisConfig.DefaultDbIndex,
                //只写服务器的最大连接池数
                MaxWritePoolSize = _redisConfig.MaxWritePoolSize,
                //只读服务器的最大连接池数
                MaxReadPoolSize = _redisConfig.MaxReadPoolSize,
                //是否自动启动
                AutoStart = _redisConfig.AutoStart,

            });

            //manager.ConnectTimeout = _redisConfig.ConnectTimeout;

            _prcm = manager;
        }

        private static string[] SplitString(string strSource, string split)
        {
            return strSource.Split(split.ToArray());
        }


        #endregion
    }
}
