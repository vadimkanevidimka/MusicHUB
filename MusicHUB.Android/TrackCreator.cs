﻿using Android.Media;
using Java.IO;
using MusicHUB.Interfaces;
using MusicHUB.Models;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MusicHUB.Droid
{
    class TrackCreator
    {
        public Track CreateTrack(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException(nameof(filename));
            }

            using (FileInputStream inputStream = new FileInputStream(new Java.IO.File(filename)))
            {
                MediaMetadataRetriever mmdr = new MediaMetadataRetriever();
                mmdr.SetDataSource(inputStream.FD);
                int min = (Convert.ToInt32(mmdr.ExtractMetadata(MetadataKey.Duration)) / 1000) / 60;
                int sec = (Convert.ToInt32(mmdr.ExtractMetadata(MetadataKey.Duration)) / 1000) % 60;
                return new Track()
                {
                    Title = mmdr.ExtractMetadata(MetadataKey.Title) is null ? filename.Substring(filename.LastIndexOf('/') + 1, filename.Length - filename.LastIndexOf('/') - 1).Replace(".mp3", "") : mmdr.ExtractMetadata(MetadataKey.Title),
                    Duration = Convert.ToInt32(mmdr.ExtractMetadata(MetadataKey.Duration)),
                    DurationString = sec < 10 ? $"{min}:0{sec}" : $"{min}:{sec}",
                    Artist = mmdr.ExtractMetadata(MetadataKey.Artist),
                    Year = mmdr.ExtractMetadata(MetadataKey.Year),
                    Album = mmdr.ExtractMetadata(MetadataKey.Album),
                    Uri = filename,
                };
            }
        }
    }
}