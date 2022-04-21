using System;
using System.Collections.Generic;

namespace Core
{
    public class Card
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string StoryPoint { get; set; }

        public DateTime Date { get; set; }
    }
}
