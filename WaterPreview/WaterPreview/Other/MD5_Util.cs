using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WaterPreview.Other
{
    public class MD5_Util
    {
        public static string MD5Encrypt(string strText)
        {
            byte[] b = Encoding.Default.GetBytes(strText);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');
            return ret;
        }
    }
}