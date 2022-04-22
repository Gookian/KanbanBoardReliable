using System;
using System.Collections.Generic;

namespace Core
{
    public class Column
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public virtual List<Card> Card { get; set; } = new List<Card>();
    }
}
