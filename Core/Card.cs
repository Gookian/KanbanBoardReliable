using System;
using System.Drawing;

namespace Core
{
    public class Card
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int StoryPoint { get; set; }

        public DateTime Date { get; set; }

        public string Color { get; set; }

        public Guid ColumnId { get; set; }

        public Column Column { get; set; } = new Column();
    }
}
