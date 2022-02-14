using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using MusicHUB.Interfaces;
using System.Text;
using System.IO;
using Xamarin.Forms;
using MusicHUB.Droid.Mocks;
using System.Threading.Tasks;
using Xamarin.Essentials;

[assembly: Dependency(typeof(FileProviderService))]
namespace MusicHUB.Droid.Mocks
{
    class FileProviderService : IFileProvider
    {
        public bool DeleteFile(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    return false;
                }

                if (File.Exists(path))
                {
                    return new Java.IO.File(path).Delete();
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GetFile()
        {
            throw new NotImplementedException();
        }

        public string[] GetFiles()
        {
            throw new NotImplementedException();
        }

        public async Task<byte[]> PickImageFromFiles(PickOptions options)
        {
            try
            {
                var result = await FilePicker.PickAsync(options);
                if (result != null)
                {
                    if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                        result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                    {
                        var stream = await result.OpenReadAsync();
                        var bitmap = new byte[stream.Length];
                        await stream.ReadAsync(bitmap, 0, bitmap.Length);
                        return bitmap;
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(Android.App.Application.Context, ex.Message, ToastLength.Short).Show();
            }
            return null;

        }
    }
}