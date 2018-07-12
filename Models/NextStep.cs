using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class NextStep
    {
        public int ID { get; set; }
        public string Mission { get; set; }
        public string Responsibility { get; set; }
        public string Due_date { get; set; }
        public string Date_postponed { get; set; }
        public int DecisionID { get; set; }
    }
}
