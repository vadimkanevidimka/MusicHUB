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
            };
            set => TrackContextActions = value;
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
            };
            set => TrackContextActions = value;
        }
    }
}
