using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHUB.ViewModels.ContextActions
{
    public class ContextActionsResourses
    {
        public IList<IContextAction> TrackListContextActions
        {
            get => new List<IContextAction>()
            {
                new ShareContextAction(),
                new DeleteContextAction(),
                new QueueContextAction(),
                new AddToQueueContextAction(),
                new RemoveFromQueueContextAction(),
                new SetAsRingtoneContextAction(),
                new EditMetaDataContextAction(),
            };
        }

        public IList<IContextAction> TrackContextActions
        {
            get => new List<IContextAction>()
            {
                new ShareContextAction(),
                new DeleteContextAction(),
                new GetLirycsContextAction(),
                new QueueContextAction(),
                new CutContextAction(),
                new SetAsRingtoneContextAction(),
                new AddToPlayListContextAction(),
                new EditMetaDataContextAction(),
            };
        }

        public IList<IContextAction> AlbumListContextActions
        {
            get => new List<IContextAction>()
            {
                new DeleteContextAction(),
                new AddToQueueContextAction(),
                new EditMetaDataContextAction(),
            };
        }

        public IList<IContextAction> AlbumPageContextActions
        {
            get => new List<IContextAction>()
            {
                new AddToQueueContextAction(),
                new EditMetaDataContextAction(),
            };
        }

        public IList<IContextAction> TrackInAlbumListContextActions
        {
            get => new List<IContextAction>()
            {
                new ShareContextAction(),
                new DeleteFromAlbumContextAction(),
                new QueueContextAction(),
                new AddToQueueContextAction(),
                new RemoveFromQueueContextAction(),
            };
        }
    }
}
