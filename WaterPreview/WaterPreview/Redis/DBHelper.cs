using CSRedis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WaterPreview.Redis
{
    public class DBHelper
    {
        /// <summary>
        /// redis的主机
        /// </summary>
        private static readonly string ConnectionString;
        /// <summary>
        /// redis的数据库引索，有点像sql server的数据库名
        /// </summary>
        public static int DB_INDEX;

        static DBHelper()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["RedisConnectionString"].ConnectionString;
            DB_INDEX = 1;
        }       


        public static List<T> get<T>(Func<List<T>> init, string key, int dbindex = 1)
        {
            using (RedisClient rc = new RedisClient(ConnectionString))
            {
                rc.Select(dbindex);
                var name = key;
                if (!rc.Exists(name))
                {
                    var list = init();
                    rc.Set(name, JsonConvert.SerializeObject(list));
                    //设置值的过期时间为24小时
                    rc.Expire(name, new TimeSpan(24, 0, 0));
                    return list;
                }
                return JsonConvert.DeserializeObject<List<T>>(rc.Get(name));

            }
        }

        /// <summary>
        /// 更新并获取key对应的init返回的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="init"></param>
        /// <param name="key"></param>
        /// <param name="dbindex"></param>
        /// <returns></returns>
        public static List<T> getAndFresh<T>(Func<List<T>> init, string key, int dbindex = 1)
        {
            using (RedisClient rc = new RedisClient(ConnectionString))
            {
                rc.Select(dbindex);
                var name = key;

                var list = init();
                rc.Set(name, JsonConvert.SerializeObject(list));
                //设置值的过期时间为24小时
                rc.Expire(name, new TimeSpan(24, 0, 0));
                return list;
            }
        }

        public static T get<T>(Func<T> init, string key, int dbindex = 1) 
        {
            using (RedisClient rc = new RedisClient(ConnectionString))
            {
                rc.Select(dbindex);
                var name = key;
                if (!rc.Exists(name))
                {
                    var list = init();
                    rc.Set(name, JsonConvert.SerializeObject(list));
                    //设置值的过期时间为24小时
                    rc.Expire(name, new TimeSpan(24, 0, 0));
                    return list;
                }
                return JsonConvert.DeserializeObject<T>(rc.Get(name));

            }
        }

        public static void ClearCache()
        {
            try
            {
                //if (!IsOpenCache) return;
                using (IRedisClient Redis = new RedisClient(ConnectionString))
                {
                    Redis.FlushAll();
                }
            }
            catch { return; }
        } 

     

    }
}