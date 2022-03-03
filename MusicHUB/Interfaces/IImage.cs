using Android.Graphics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Color = Xamarin.Forms.Color;

namespace MusicHUB.Interfaces
{
    public interface IImage
    {
        Task<Color> ConfigColorAsync(string filename);
        ImageSource GetTrackPic(string fileName);
        ImageSource GetLowerResImage(string fileName);
        Bitmap GetBitmap(string filename);
    }
}
