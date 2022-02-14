using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MusicHUB.Interfaces
{
    public interface IFileProvider
    {
        string[] GetFiles();
        string GetFile();
        bool DeleteFile(string path);
        Task<byte[]> PickImageFromFiles(PickOptions options);
    }
}
