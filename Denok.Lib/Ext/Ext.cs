using System;
using System.Drawing;
using System.IO;

namespace Denok.Lib.Ext 
{
    public static class Extension
    {
        public static Bitmap Base64StrToBitmap(this string base64Str)
        {
            Bitmap base64Bitmap = null;
            MemoryStream stream = null;

            try
            {
                byte[] base64Bytes = Convert.FromBase64String(base64Str);
                stream = new MemoryStream(base64Bytes);

                stream.Position = 0;

                base64Bitmap = (Bitmap) Bitmap.FromStream(stream);
            } catch(FormatException)
            {
                return null;
            } finally
            {
                stream.Close();
                stream = null;
            }

            return base64Bitmap;
        }

        public static string ToBase64Str(this Bitmap bmp, System.Drawing.Imaging.ImageFormat format)
        {
            string base64Str = string.Empty;
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream();
                bmp.Save(stream, format);

                stream.Position = 0;
                byte[] bmpByteStream = stream.ToArray();

                base64Str = Convert.ToBase64String(bmpByteStream);

            } catch(FormatException)
            {
                return null;
            } finally
            {
                stream.Close();
                stream = null;
            }

            return base64Str;
        }
    }
}