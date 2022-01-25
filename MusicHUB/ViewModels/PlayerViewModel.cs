using MusicHUB.Interfaces;
using MusicHUB.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Net;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;
using System.Globalization;
using System.Xml.Serialization;
using System.IO;

namespace MusicHUB.ViewModels
{
    public class PlayerViewModel : BindableObject
    {
        private readonly IAudio Audio = DependencyService.Get<IAudio>();

        private Timer timer;

        private Action OnOptionsClick;

        public int VolumeLevel { get; set; }

        public int MaxVolumeLevel { get; set; }

        public NotifyTaskCompletion<Color> PrimaryColor { get => new NotifyTaskCompletion<Color>(DependencyService.Get<IImage>().ConfigColorAsync(CurrentTrack.Uri)); } 

        public ResourceClassPlayerPage ResourceClass { get; set; }

        public List<Artist> Artists { get => GetResponce(CurrentTrack.Artist); }

        public ObservableCollection<Track> Tracks { get => Audio.Tracks; }

        public Color BackGround { get => BackGround; 
            set 
            {
                BackGround = value;
            }
        }

        public string PlayedTimeString { get 
            {
                OnPropertyChanged(nameof(PlayedTime));
                int min = (PlayedTime / 1000) / 60;
                int sec = (PlayedTime / 1000) % 60;
                return sec < 10 ? $"{min}:0{sec}" : $"{min}:{sec}";
            }
        }

        public int PlayedTime { get; set; }

        public ICommand TrackOptions
        {
            get
            {
                return OnOptionsClick != null ?
                new Command(OnOptionsClick) : null;
            }
        }

        public ICommand SetVolumeCommand { 
            get => new Command(() => Audio.SetVolumeLevel(VolumeLevel));
        }

        public Track CurrentTrack
        {
            get
            {
                return Audio.GetCurrentTrack();
            }
        }

        public ICommand TrackChange
        {
            get => new Command(c =>
            {
                timer.Dispose();
                Audio.PlayAudioFile((Track)c);
                OnPropertyChanged(nameof(CurrentTrack));
                OnPropertyChanged(nameof(Artists));
                InintTimer();
            });
        }

        public ICommand NextTrack {
            get => new Command(() =>
            {
                timer.Dispose();
                Audio.Next();
                OnPropertyChanged(nameof(CurrentTrack));
                InintTimer();
            });
        }

        public ICommand PrevTrack {
            get => new Command(() =>
            {
                timer.Dispose();
                Audio.Prev();
                OnPropertyChanged(nameof(CurrentTrack));
                InintTimer();
            }); 
        }

        public ICommand PlayPause {
            get => new Command(() =>
            {
                Audio.PlayPause();
                ResourceClass.PlayPause = Audio.IsPlaying ? ResourceClassPlayerPage.PauseImg : ResourceClassPlayerPage.PlayImg;
                OnPropertyChanged(nameof(ResourceClass));
            });
        }

        public Command ShuffleCommand {
            get => new Command(() =>
            {
                Audio.Shuffle();
                ResourceClass.Shufle = ResourceClassPlayerPage.ShuffledImg;
                OnPropertyChanged(nameof(ResourceClass));
            });
        }

        public Command RepeatCommand {
            get => new Command(() =>
            {
                Audio.LoopChange();
                ResourceClass.Repeat = Audio.IsLooping ? ResourceClassPlayerPage.LoopedImg : ResourceClassPlayerPage.LoopImg;
                OnPropertyChanged(nameof(ResourceClass));
            });
        }

        public PlayerViewModel(Action onoptionsClick)
        {
            MaxVolumeLevel = Audio.GetMaxVolumeLevel;
            VolumeLevel = Audio.GetVolumeLevel;
            this.OnOptionsClick = onoptionsClick;
            ResourceClass = new ResourceClassPlayerPage();
            Audio.OnCompleted += (obj, e) => OnPropertyChanged(nameof(CurrentTrack));
            OnPropertyChanged(nameof(CurrentTrack));
            OnPropertyChanged(nameof(Artists));
            InintTimer();
        }

        private List<Artist> GetResponce(string artists)
        {
            List<Artist> images = new List<Artist>(); 
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    HttpClient client = new HttpClient();
                    foreach (var item in artists.Split(new char[] { ',' }))
                    {
                        var artiststring = item.TrimStart(' ');
                        string json = client.GetStringAsync($"https://ws.audioscrobbler.com/2.0/?method=artist.getinfo&artist={CurrentTrack.Artist}&api_key=0be0ae24e354e80d2558bd06c94cb2c7&format=xml&lang=Ru").Result;
                        XmlSerializer serializer = new XmlSerializer(typeof(Artist));
                        var xml = json.Substring(json.IndexOf("<artist>")).Replace("</lfm>", "");
                        var artist = (Artist)serializer.Deserialize(new StringReader(xml));
                        artist.Image.uri = artist.Image.uri ?? "https://lastfm.freetls.fastly.net/i/u/34s/2a96cbd8b46e442fc41c2b86b821562f.png";
                        images.Add(artist);
                    }
                    return images;
                }
                return images;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return images;
            }
        }

        private void InintTimer()
        {
            TimerCallback callback = CurrentTime;
            timer = new Timer(callback, null, 0, 500);
        }

        private NetworkAccess Ethernet()
        {
            return Connectivity.NetworkAccess;
        }

        private void CurrentTime(object c)
        {
            PlayedTime = Audio.Time;
            OnPropertyChanged(nameof(PlayedTimeString));
        }

    }
}
