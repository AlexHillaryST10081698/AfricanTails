using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfricanTails.Classes
{
    internal class Animal
    {
        public string AnimalID { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public string Colour { get; set; }
        public string Sex { get; set; }
        public long Microchip { get; set; }
        public string Status { get; set; }
        public DateTime DateofBirth { get; set; }
        public DateTime DateAdopted { get; set; }
        public DateTime DateFostered { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
