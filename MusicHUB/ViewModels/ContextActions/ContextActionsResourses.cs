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
                new DeleteContextAction(),
                new GetLirycsContextAction(),
                new QueueContextAction(),
            };
            set => TrackContextActions = value;
        }
    }
}
