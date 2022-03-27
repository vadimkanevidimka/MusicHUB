using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Provider;
using Android.Widget;
using Java.Lang;
using MusicHUB.Interfaces;
using MusicHUB.Models;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using Uri = Android.Net.Uri;

namespace MusicHUB.ViewModels.ContextActions
{
    class DeleteContextAction : BaseContextAction
    {
        public override string ImageURl => "deletebutton.png";
        public override string DescriptionText => "Удалить";

        public async override void ExcecuteAction<T>(object someobject, Action outAction = null)
        {
            if (someobject == null)
            {
                return;
            }

            if (typeof(T) == typeof(Track))
            {
                Track track = someobject as Track;
                DependencyService.Get<IAudio>().Pause();
                App.Connections.BaseDataBaseService.DataBase.DeleteAsync(track);
                DependencyService.Get<IAudio>().Next();
                DependencyService.Get<IAudio>().RemoveFromQueue(track);
                var contentresolver = DependencyService.Get<IAndroidSystem>().AppContext.ContentResolver;
                Uri trackUri = new Uri.Builder().Path(track.Uri).Build();
                try
                {
                    Java.IO.File file = new Java.IO.File(track.Uri);

                    string where = MediaStore.MediaColumns.Data + "=?";
                    string[] selectionArgs = new string[] { file.AbsolutePath };
                    Android.Net.Uri filesUri = MediaStore.Files.GetContentUri("external");

                    if (file.Exists())
                    {
                        contentresolver.Delete(filesUri, where, selectionArgs);
                    }
                    base.MakeToast($"{track.Title} удален");
                }
                catch (IllegalAccessException)
                {
                    base.MakeToast($"{track.Title} не удален");
                }
                catch (SecurityException e)
                {
                    IntentSender intentSender;
                    if(Build.VERSION.SdkInt >= BuildVersionCodes.R)
                    {
                        intentSender = MediaStore.CreateDeleteRequest(contentresolver, new List<Uri>() { trackUri }).IntentSender;
                    }
                    else if(Build.VERSION.SdkInt >= BuildVersionCodes.Q)
                    {
                        var recoverableSecurityException = e as RecoverableSecurityException;
                        intentSender = recoverableSecurityException?.UserAction?.ActionIntent?.IntentSender;
                    }
                    else
                    {
                        intentSender = null;
                    }

                    Intent intent = new Intent(Intent.ActionDelete);
                    intent.SetType("music");
                    intent.PutExtra(Intent.ExtraTitle, track.Uri);
                    intentSender?.SendIntent(DependencyService.Get<IAndroidSystem>().AppContext, Result.FirstUser, intent, null, new Handler((Message) => base.MakeToast($"{track.Title} удален")));
                }
            }
            else if (typeof(T) == typeof(Album))
            {
                await App.Connections.BaseDataBaseService.DataBase.DeleteAsync((Album)someobject);
            }

            if (outAction != null) outAction();
        }
    }
}
