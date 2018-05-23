using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebProjekat
{
    public class Voznja
    {
        static int brojacVoznji = 0;

        public int ID { get; set; }
        public DateTime DatumNarucivanja { get; set; }
        public Lokacija PocetnaLokacija { get; set; }
        public Korisnik Musterija { get; set; }
        public Lokacija Odrediste { get; set; }
        public Korisnik Dispecer { get; set; }
        public Korisnik Vozac { get; set; }
        public double Iznos { get; set; }
        public Komentar Komentar { get; set; }
        public StatusVoznje Status { get; set; }

        public Voznja() {
            brojacVoznji++;
            ID = brojacVoznji;
        }
    }
}
