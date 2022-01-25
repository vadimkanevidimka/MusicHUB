using MusicHUB.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace MusicHUB.Droid
{
    class MusicFilesCollector
    {
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
                string[] MusicPaths = Directory.GetFiles(Path, "*.mp3", SearchOption.AllDirectories);
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
        public ObservableCollection<Track> GetTracks(IEnumerable<string> directories)
        {
            int id = 0;
            ObservableCollection<Track> tracks = new ObservableCollection<Track>();
            var creator = new TrackCreator();
            var files = GetAllFiles(directories);
            foreach (var file in files)
            {
                var track = creator.CreateTrack(file);
                track.Id = id++;
                tracks.Add(track);
            }

            return tracks;
        }
    }
}