using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using ZXing;
using ZXing.Common;

namespace Noticer.Services
{
    public class QrGenerator
    {
        public WriteableBitmap GenerateQr(int width, int height, string text)
        {
            var bw = new BarcodeWriter();
            var encOptions = new EncodingOptions {Width = width, Height = height, Margin = 0};
            bw.Options = encOptions;
            bw.Format = BarcodeFormat.QR_CODE;
            var temp = bw.Write(text).Pixel;
            var byteArray = new byte[temp.Length];
            temp.CopyTo(byteArray, 0);



            var result = new WriteableBitmap(200,200);
                result.FromByteArray(byteArray);
          //   result.SetSourceAsync(randomAccessStream);
            return result;
        }

        public async Task<Uri> SaveImage(WriteableBitmap bitmap)
        {
            var file = await WriteableBitmapToStorageFile(bitmap, FileFormat.Png);
            return new Uri("ms-appdata:///temp/" + file.Name);
        }
        private async Task<StorageFile> WriteableBitmapToStorageFile(WriteableBitmap WB, FileFormat fileFormat)
        {
            var FileName = "MyFile.";
            var BitmapEncoderGuid = BitmapEncoder.JpegEncoderId;
            switch (fileFormat)
            {
                case FileFormat.Jpeg:
                    FileName += "jpeg";
                    BitmapEncoderGuid = BitmapEncoder.JpegEncoderId;
                    break;

                case FileFormat.Png:
                    FileName += "png";
                    BitmapEncoderGuid = BitmapEncoder.PngEncoderId;
                    break;

                case FileFormat.Bmp:
                    FileName += "bmp";
                    BitmapEncoderGuid = BitmapEncoder.BmpEncoderId;
                    break;

                case FileFormat.Tiff:
                    FileName += "tiff";
                    BitmapEncoderGuid = BitmapEncoder.TiffEncoderId;
                    break;

                case FileFormat.Gif:
                    FileName += "gif";
                    BitmapEncoderGuid = BitmapEncoder.GifEncoderId;
                    break;
            }

            var file =
                await
                    ApplicationData.Current.TemporaryFolder.CreateFileAsync(FileName,
                        CreationCollisionOption.GenerateUniqueName);
            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoderGuid, stream);
                var pixelStream = WB.PixelBuffer.AsStream();
                var pixels = new byte[pixelStream.Length];
                await pixelStream.ReadAsync(pixels, 0, pixels.Length);

                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore,
                    (uint) WB.PixelWidth,
                    (uint) WB.PixelHeight,
                    96.0,
                    96.0,
                    pixels);
                await encoder.FlushAsync();
            }
            return file;
        }

        private enum FileFormat
        {
            Jpeg,
            Png,
            Bmp,
            Tiff,
            Gif
        }
    }
}