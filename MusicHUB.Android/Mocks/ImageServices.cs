using Android.Graphics;
using Android.Media;
using Java.IO;
using MusicHUB.Droid.Mocks;
using MusicHUB.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImageServices))]

namespace MusicHUB.Droid.Mocks
{
    class ImageServices : IImage
    {
        private double Red = 200;
        private double Green = 200;
        private double Blue = 200;
        private Android.Graphics.Bitmap image;

        public async Task<Xamarin.Forms.Color> ConfigColorAsync(string filename)
        {
            GetBitmap(filename);
            return await GetColorAsync(image);
        }

        private void GetBitmap(string filename)
        {
            try
            {
                MediaMetadataRetriever mediaMetadataRetriever = new MediaMetadataRetriever();
                Java.IO.File file = new Java.IO.File(filename);
                FileInputStream inputStream = new FileInputStream(file);
                mediaMetadataRetriever.SetDataSource(inputStream.FD);
                byte[] picture = mediaMetadataRetriever.GetEmbeddedPicture();
                image = Android.Graphics.BitmapFactory.DecodeByteArray(picture, 0, picture.Length);
            }
            catch (Exception ex)
            {
                image = null;
                System.Console.WriteLine(ex.Message);
            }
        }
        private async Task<Xamarin.Forms.Color> GetColorAsync(Android.Graphics.Bitmap bitmap)
        {
            return await Task.Run(() => GetColor(image));
        }
        private Xamarin.Forms.Color GetColor(Android.Graphics.Bitmap bitmap)
        {
            try
            {
                long redBucket = 0;
                long greenBucket = 0;
                long blueBucket = 0;
                long pixelCount = 0;
                for (int y = 30; y < bitmap.Height / 4; y++)
                {
                    for (int x = 30; x < bitmap.Width / 4; x++)
                    {
                        Android.Graphics.Color c = new Android.Graphics.Color(bitmap.GetPixel(x, y));

                        pixelCount++;
                        redBucket += c.R;
                        greenBucket += c.G;
                        blueBucket += c.B;
                    }
                }

                Red = image is null ? 200 : Convert.ToInt32(redBucket / pixelCount);
                Green = image is null ? 200 : Convert.ToInt32(greenBucket / pixelCount);
                Blue = image is null ? 200 : Convert.ToInt32(blueBucket / pixelCount);
                return new Xamarin.Forms.Color(Red, Green, Blue);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return Xamarin.Forms.Color.Black;
            }
        }

        ImageSource IImage.GetTrackPic(string fileName)
        {
            try
            {
                MediaMetadataRetriever mediaMetadataRetriever = new MediaMetadataRetriever();
                Java.IO.File file = new Java.IO.File(fileName);
                FileInputStream inputStream = new FileInputStream(file);
                mediaMetadataRetriever.SetDataSource(inputStream.FD);
                byte[] picture = mediaMetadataRetriever.GetEmbeddedPicture();
                if (picture is null || picture.Length == 0) throw new ArgumentNullException();
                return ImageSource.FromStream(() => new MemoryStream(picture));
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return ImageSource.FromFile("BoyOnNine.jpg");
            }
        }

        ImageSource IImage.GetLowerResImage(string fileName)
        {
            try
            {
                MediaMetadataRetriever mediaMetadataRetriever = new MediaMetadataRetriever();
                Java.IO.File file = new Java.IO.File(fileName);
                FileInputStream inputStream = new FileInputStream(file);
                mediaMetadataRetriever = new MediaMetadataRetriever();
                mediaMetadataRetriever.SetDataSource(inputStream.FD);
                byte[] picture = mediaMetadataRetriever.GetEmbeddedPicture();
                Android.Graphics.Bitmap bitmap = Android.Graphics.BitmapFactory.DecodeByteArray(picture, 0, picture.Length);
                using (MemoryStream stream = new MemoryStream(picture))
                {
                    MemoryStream memory = new MemoryStream();
                    bitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 20, memory);
                    byte[] lowpic = memory.ToArray();
                    return ImageSource.FromStream(() => new MemoryStream(lowpic));

                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return ImageSource.FromFile("BoyOnNine.jpg");
            }
        }
    }
}