﻿using System;

namespace Core
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public Token Token { get; set; }
    }
}
