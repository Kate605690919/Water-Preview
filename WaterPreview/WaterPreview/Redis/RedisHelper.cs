using CSRedis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterPreview.Redis
{
        public class RedisHelper
        {
            /// <summary>
            /// redis的主机
            /// </summary>
            public static string host = "192.168.2.70";
            /// <summary>
            /// redis的数据库引索，有点像sql server的数据库名
            /// </summary>
            public static int DB_INDEX = 1;
            /// <summary>
            /// 获取键为name的值 使用泛型可以获取任何数据
            /// </summary>
            /// <typeparam name="T">需要获取的类型</typeparam>
            /// <param name="name">键名</param>
            /// <returns></returns>
            public static T get<T>(string name)
            {
                //连接客户端
                using (RedisClient rc = new RedisClient(host))
                {
                    //选择缓存引索
                    rc.Select(DB_INDEX);
                    //获取name的字符串值，并且由json转换为T类型
                    return JsonConvert.DeserializeObject<T>(rc.Get(name));
                }
            }
            /// <summary>
            /// 获取未转换类型的原始json格式字符串
            /// </summary>
            /// <param name="name">键名</param>
            /// <returns></returns>
            public static string getJson(string name)
            {
                using (RedisClient rc = new RedisClient(host))
                {
                    rc.Select(DB_INDEX);
                    return rc.Get(name);
                }
            }
            /// <summary>
            /// 设置键名为T的值为value
            /// </summary>
            /// <typeparam name="T">设置值的类型</typeparam>
            /// <param name="name">键名</param>
            /// <param name="value">值</param>
            public static void set<T>(string name, T value)
            {
                using (RedisClient rc = new RedisClient(host))
                {
                    rc.Select(DB_INDEX);
                    //转换为json格式后存储
                    rc.Set(name, JsonConvert.SerializeObject(value));
                }
            }
        
    }
}