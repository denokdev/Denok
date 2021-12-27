using System;
using System.Drawing;
using System.Threading.Tasks;
using QRCoder;
using Denok.Lib.Ext;
using Denok.Lib.Shared;

namespace Denok.Lib.Qr 
{
    public static class QrGenerator
    {
        private static string LOGO = "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";

        private static QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();

        public static async Task<Result<Bitmap,string>> Generate(string text)
        {
            return await Task.Run<Result<Bitmap, string>>(() => {
                try
                {
                    Bitmap logo = LOGO.Base64StrToBitmap();
                    if (logo == null)
                    {
                        return Result<Bitmap,string>.From(null, "error generate qr code logo");
                    }
                    QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
                    QRCode qRCode = new QRCode(qRCodeData);

                    Bitmap qrCodeImage = qRCode.GetGraphic(10, Color.Black, Color.White, logo);
                    return Result<Bitmap,string>.From(qrCodeImage, null);
                } catch(Exception)
                {
                    return Result<Bitmap,string>.From(null, "error generate qr code");
                }
            });
        }
    }
}