﻿using System;
using CosmosObjectVisitorUT.Model;

namespace CosmosObjectVisitorUT.Model
{
    public class NiceAct
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime Birthday { get; set; }

        public Country Country { get; set; }

        public bool IsValid { get; set; }
    }
}