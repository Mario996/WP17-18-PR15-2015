using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebProjekat
{
    public class Vozac : Korisnik
    {
        public Lokacija TrenutnaLokacija { get; set; }
        public Automobil Automobil { get; set; }
    }
}
