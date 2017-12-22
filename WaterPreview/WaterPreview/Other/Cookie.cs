using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WaterPreview.Other
{
    public class Cookie
    {
        /// <summary>
        /// Cookies赋值
        /// </summary>
        /// <param name="strName">主键</param>
        /// <param name="strValue">键值</param>
        /// <param name="strDay">有效天数</param>
        /// <returns></returns>
        public static bool SetCookie(string strName, string strValue, int strDay)
        {
            try
            {
                HttpCookie cookie = new HttpCookie(strName);
                cookie.Domain = ConfigurationManager.AppSettings["DomainName"];//当要跨域名访问的时候,给cookie指定域名即可,格式为.xxx.com
                cookie.Expires = DateTime.Now.AddDays(strDay);
                cookie.Value = strValue;
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 读取Cookies
        /// </summary>
        /// <param name="strName">主键</param>
        /// <returns></returns>

        public static string GetCookie(string strName)
        {
            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[strName];
            if (cookie != null)
            {
                return cookie.Value.ToString();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 删除Cookies
        /// </summary>
        /// <param name="strName">主键</param>
        /// <returns></returns>
        public bool DelCookie(string strName)
        {
            try
            {
                HttpCookie cookie = new HttpCookie(strName);
                cookie.Domain = ConfigurationManager.AppSettings["DomainName"];//当要跨域名访问的时候,给cookie指定域名即可,格式为.xxx.com
                cookie.Expires = DateTime.Now.AddDays(-1);
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}