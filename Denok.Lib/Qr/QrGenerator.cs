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
        private static QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();

        public static async Task<Result<Bitmap,string>> Generate(string text, string base64Logo)
        {
            return await Task.Run<Result<Bitmap, string>>(() => {
                try
                {
                    QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
                    QRCode qRCode = new QRCode(qRCodeData);

                    Bitmap qrCodeImage = qRCode.GetGraphic(10);
                    if (!String.IsNullOrEmpty(base64Logo))
                    {
                        Bitmap logo = base64Logo.Base64StrToBitmap();
                        if (logo == null)
                        {
                            return Result<Bitmap,string>.From(null, "error generate qr code logo");
                        }

                        qrCodeImage = qRCode.GetGraphic(10, Color.Black, Color.White, logo);
                    }
                    
                    return Result<Bitmap,string>.From(qrCodeImage, null);
                } catch(Exception)
                {
                    return Result<Bitmap,string>.From(null, "error generate qr code");
                }
            });
        }
    }
}