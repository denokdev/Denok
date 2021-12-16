using System;

namespace Denok.Lib.LinkGenerator
{
    public static class LinkGenerator
    {
        private static string ALPHABET = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static string Generate()
        {
            return Nanoid.Nanoid.Generate(alphabet: ALPHABET, size:7);
        }
    }
}