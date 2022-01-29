using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHUB.Interfaces
{
    interface IFileProvider
    {
        string[] GetFiles();
        string GetFile();
        bool DeleteFile(string path);
    }
}
