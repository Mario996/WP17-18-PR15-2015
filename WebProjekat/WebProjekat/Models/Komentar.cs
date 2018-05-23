using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebProjekat
{
    public class Komentar
    {
        static int brojacKomentara = 0;

        public int ID { get; set; }
        public string Opis { get; set; }
        public DateTime DatumObjave { get; set; }
        public Korisnik Autor { get; set; }
        public Voznja Voznja { get; set; }
        public int OcenaVoznje { get; set; }

        public Komentar() {
            brojacKomentara++;
            ID = brojacKomentara;
        }
    }
}
