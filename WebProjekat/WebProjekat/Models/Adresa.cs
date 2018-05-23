﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebProjekat
{
    public class Adresa
    {
        public string Ulica { get; set; }
        public string Broj { get; set; }
        public string Mesto { get; set; }
        public int PozivniBroj { get; set; }

        public override string ToString() {
            return Ulica + " " + Broj + ", " + Mesto + " " + PozivniBroj;
        }
    }
}