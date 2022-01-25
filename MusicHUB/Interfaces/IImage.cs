using System.Threading.Tasks;
using Xamarin.Forms;

namespace MusicHUB.Interfaces
{
    public interface IImage
    {
        Task<Color> ConfigColorAsync(string filename);
        ImageSource GetTrackPic(string fileName);
        ImageSource GetLowerResImage(string fileName);
    }
}
