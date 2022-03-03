using MusicHUB.Interfaces;
using MusicHUB.Models;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace MusicHUB.Helpers
{
    class MusicFileTrimmer
    {
        public void TrimWavFile(string inPath, string outPath, TimeSpan cutFromStart, TimeSpan cutFromEnd, Track tracktoCut)
        {
            using (var inputStream = new FileStream(inPath, FileMode.Open, FileAccess.Read))
            {
                int bytesPerMillisecond = (int)(tracktoCut.Bitrate / 1000 / 8);

                long startPos = cutFromStart.Ticks * bytesPerMillisecond;

                long endBytes = cutFromEnd.Ticks * bytesPerMillisecond;

                long endPos = (int)inputStream.Length - endBytes;

                TrimWavFile(inputStream, new FileStream(outPath, FileMode.Create, FileAccess.ReadWrite), startPos, endPos);
            }
        }

        private void TrimWavFile(FileStream reader, FileStream writer, long startPos, long endPos)
        {
            reader.Position = startPos;
            byte[] buffer = new byte[1024];
            while (reader.Position < endPos)
            {
                int bytesRequired = (int)(endPos - reader.Position);
                if (bytesRequired > 0)
                {
                    int bytesToRead = Math.Min(bytesRequired, buffer.Length);
                    int bytesRead = reader.Read(buffer, 0, bytesToRead);
                    if (bytesRead > 0)
                    {
                        writer.Write(buffer, 0, bytesRead);
                    }
                }
            }
        }

        public void SetAsRingtone(string inPath, string outPath, TimeSpan cutFromStart, TimeSpan cutFromEnd, Track tracktoCut)
        {
            TrimWavFile(inPath, outPath, cutFromStart, cutFromEnd, tracktoCut);
            DependencyService.Get<IAndroidSystem>().SetAsRingtone(outPath);
        }
    }
}
