using System;
using System.Collections.Generic;

namespace Core
{
    public class Board
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<Column> Column { get; set; }
    }
}
