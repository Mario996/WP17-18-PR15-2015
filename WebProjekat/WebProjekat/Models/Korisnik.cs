using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebProjekat
{
    public class Korisnik
    {
        static int brojacKorisnika = 0;

        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public PolOsobe Pol { get; set; }
        public string JMBG { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public TipVoznje TipVoznje { get; set; }

        public Korisnik() {
            brojacKorisnika++;
            ID = brojacKorisnika;
        }
    }
}
