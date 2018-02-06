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

        public static T get<T>(Func<T> init, string key, int dbindex = 1)
        {
            using (RedisClient rc = new RedisClient(ConnectionString))
            {
                rc.Select(dbindex);
                var name = key;
                if (!rc.Exists(name))
                {
                    var result = init();
                    rc.Set(name, JsonConvert.SerializeObject(result));
                    //设置值的过期时间为24小时
                    rc.Expire(name, new TimeSpan(24, 0, 0));
                    return result;
                }
                return JsonConvert.DeserializeObject<T>(rc.Get(name));

            }
        }
        /// <summary>
        /// 更新并获取key对应的init返回的值,不设置过期时间
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
                //rc.Expire(name, new TimeSpan(24, 0, 0));
                return list;
            }
        }

        public static T getT<T>(Func<T> init, string key, int dbindex = 1) 
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

        public static T getAndFreshT<T>(Func<T> init, string key, int dbindex = 1)
        {
            using (RedisClient rc = new RedisClient(ConnectionString))
            {
                rc.Select(dbindex);
                var name = key;
                var list = init();
                rc.Set(name, JsonConvert.SerializeObject(list));
                //设置值的过期时间为24小时
                //rc.Expire(name, new TimeSpan(24, 0, 0));
                return list;
            }
        }

        /// <summary>
        /// 设置init并获取key对应的值，不设置过期时间，当前仅使用在存储访问次数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="init"></param>
        /// <param name="key"></param>
        /// <param name="dbindex"></param>
        /// <returns></returns>
        public static T getWithNoExpire<T>(Func<T> init,string key,int dbindex =1)
        {
            using (RedisClient rc = new RedisClient(ConnectionString))
            {
                rc.Select(dbindex);
                var name = key;
                if (!rc.Exists(name))
                {
                    var list = init();
                    rc.Set(name, JsonConvert.SerializeObject(list));
                    return list;
                }

                return JsonConvert.DeserializeObject<T>(rc.Get(name));

            }
        }

        /// <summary>
        /// 设置过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="datetime"></param>
        public static bool SetExpire(string key,int dbindex =1)
        {
            try
            {
                using (IRedisClient Redis = new RedisClient(ConnectionString))
                {
                    Redis.Select(dbindex);
                    var name = key;
                    if (Redis.Exists(name))
                    {
                        Redis.Expire(key, new TimeSpan(0, 0, 0));//立即过期
                        return true;
                    }
                    else return false;
                }
            }
            catch { return false; }
        }

       

     

    }
}