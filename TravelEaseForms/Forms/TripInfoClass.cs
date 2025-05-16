using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelEaseForms.Forms
{
    public class TripInfoClass
    {
        public string type { get; set; }
        public string desc { get; set; }
        public string departurePlace { get; set; }

        public string Destination { get; set; }
        public int cost { get; set; }

        public double rating { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
