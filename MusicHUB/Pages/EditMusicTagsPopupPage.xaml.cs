using MusicHUB.Interfaces;
using MusicHUB.Models;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TagLib;
using TagLib.Id3v1;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using File = TagLib.File;

namespace MusicHUB.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditMusicTagsPopupPage : ContentPage
    {
        public Track TrackToEdit { get => tracktoedit; set { tracktoedit = value; OnPropertyChanged(nameof(TrackToEdit)); } }
        private Track tracktoedit;
        public File File { get => file; set { file = value; OnPropertyChanged(nameof(File)); } }
        private File file;

        public string TrackTitle { get => title; set { title = value; OnPropertyChanged(nameof(TrackTitle)); } }
        public string Artist { get => artist; set { artist = value; OnPropertyChanged(nameof(Artist)); } }
        public string Album { get => album; set { album = value; OnPropertyChanged(nameof(Album)); } }
        public uint Year { get => year; set { year = value; OnPropertyChanged(nameof(Year)); } }
        public ByteVector Image { get => image; set { image = value; OnPropertyChanged(nameof(Image)); } }
        public ImageSource CoverArt { get => coverart; set { coverart = value; OnPropertyChanged(nameof(CoverArt)); } }

        private ImageSource coverart;
        private string artist;
        private string title;
        private string album;
        private uint year;
        private ByteVector image;


        public EditMusicTagsPopupPage(Track track)
        {
            InitializeComponent();
            BindingContext = this;
            TrackToEdit = track;
            File = TagLib.File.Create(track.Uri);

            TrackTitle = File.Tag.Title;
            Artist = File.Tag.FirstPerformer;
            Album = File.Tag.Album;
            Year = File.Tag.Year;
            CoverArt = TrackToEdit.ImageSource;
        }

        public ICommand SaveCommand
        {
            get => new AsyncCommand( async () =>
            {
                try
                {
                    File.Tag.Title = TrackTitle;
                    File.Tag.Performers = new string[] { Artist };
                    File.Tag.Album = Album;
                    File.Tag.Year = Year;
                    if (File.Tag.Pictures != null && Image != null)
                    {
                        File.Tag.Pictures = new IPicture[] { new TagLib.Picture(Image) };
                    }
                    File.Save();
                    TrackToEdit.Title = TrackTitle ?? TrackToEdit.Title;
                    TrackToEdit.Artist = Artist ?? TrackToEdit.Artist;
                    TrackToEdit.Album = Album ?? TrackToEdit.Album;
                    TrackToEdit.Year = Year.ToString() ?? TrackToEdit.Year;
                    await App.Connections.BaseDataBaseService.DataBase.UpdateAsync(TrackToEdit);
                    await App.Current.MainPage.Navigation.PopAsync();
                }
                catch (System.UnauthorizedAccessException)
                {

                }
            });
        }

        public ICommand ChangeCoverCommand
        {
            get => new AsyncCommand(async () =>
            {
                Image = new ByteVector(await DependencyService.Get<IFileProvider>().PickImageFromFiles(PickOptions.Images));
                CoverArt = ImageSource.FromStream(() => new MemoryStream(image.Data));
            });
        }
    }
}