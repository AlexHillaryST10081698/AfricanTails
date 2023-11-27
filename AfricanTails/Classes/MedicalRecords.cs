using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfricanTails.Classes
{
    internal class MedicalRecords
    {
        public string MedicalRecordID { get; set; }
        public string AnimalID { get; set; }
        public DateTime Sterilization { get; set; }
        public DateTime FirstVaccination { get; set; }
        public DateTime SecondVaccination { get; set; }
        public DateTime ThirdVaccination { get; set; }
        public string DewormedTest { get; set; }
        public DateTime DewormedTestDate { get; set; }
        public string RabiesTest { get; set; }
        public DateTime RabiesTestDate { get; set; }
        public string MedicalTreatmentTest { get; set; }
        public DateTime MedicalTreatmentTestDate { get; set; }
        public string FleaTreatmentTest { get; set; }
        public DateTime FleaTreatmentTestDate { get; set; }
    }
}
