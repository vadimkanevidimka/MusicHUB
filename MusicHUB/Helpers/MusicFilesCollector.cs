using MusicHUB.DataBaseServices;
using MusicHUB.Interfaces;
using MusicHUB.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MusicHUB.Droid
{
    public sealed class MusicFilesCollector
    {
        public MusicFilesCollector()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        private List<string> GetFilesFromDirecory(string Path)
        {
            if (string.IsNullOrEmpty(Path))
            {
                throw new ArgumentNullException(nameof(Path));
            }


            List<string> MusicFiles = new List<string>();

            try
            {
                string[] MusicPaths = Directory.GetFiles(Path, "*.mp3", SearchOption.TopDirectoryOnly);
                foreach (var item in MusicPaths)
                {
                    if (item.Contains(".mp3"))
                    {
                        MusicFiles.Add(item);
                    }
                }
            }
            finally { }

            return MusicFiles;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        private List<string> GetAllFiles(IEnumerable<string> paths)
        {
            if (paths is null)
            {
                throw new ArgumentNullException(nameof(paths));
            }

            List<string> AllFiles = new List<string>();
            foreach (var path in paths)
            {
                AllFiles.AddRange(GetFilesFromDirecory(path));
            }
            return AllFiles;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public async Task<List<Track>> GetTracks(IEnumerable<string> directories)
        {
            if (directories is null)
            {
                return await GetTracksFromDB();
            }
           
            ICreateTrack creator = DependencyService.Get<ICreateTrack>();
            int id = 0;
            var files = GetAllFiles(directories);
            List<Track> Tracks = new List<Track>();
            foreach (var file in files)
            {
                var track = creator.CreateTrack(file);
                var bdtracks = await App.Connections.BaseDataBaseService.DataBase.Table<Track>().ToListAsync();
                if (!bdtracks.Exists((c) => c.Uri == track.Uri))
                {
                    await App.Connections.BaseDataBaseService.DataBase.InsertAsync(track, typeof(DBLikedTracks));
                }
            }
            Tracks.AddRange(await App.Connections.BaseDataBaseService.DataBase.Table<Track>().ToListAsync());
            return Tracks;
        }

        public async Task<List<Track>> GetTracksFromDB()
        {
            return await App.Connections.BaseDataBaseService.DataBase.Table<Track>().ToListAsync();
        }

        //private async Task<IEnumerable<Track>> GetTracks()
        //{
        //    return await DataBaseService.DataBase.Table<Track>().ToListAsync();
        //}
    }
}