﻿using System.Security.Cryptography;

namespace Utils.Extensions
{
    public static class RNGCryptoServiceProviderExt
    {
        public static byte NextByte(this RNGCryptoServiceProvider rnd)
        {
            var arr = new byte[1];
            rnd.GetBytes(arr);
            return arr[0];
        }
    }
}
