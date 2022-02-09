using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHUB.ViewModels.ContextActions
{
    public class ContextActionsResourses
    {
        public IList<IContextAction> TrackContextActions
        {
            get => new List<IContextAction>()
            {
                new ShareContextAction(),
                new DeleteContextAction(),
                new GetLirycsContextAction(),
                new QueueContextAction(),
                new AddToQueueContextAction(),
                new RemoveFromQueueContextAction(),
                new CutContextAction(),
                new SetAsRingtoneContextAction(),
            };
            set => TrackContextActions = value;
        }
    }
}
