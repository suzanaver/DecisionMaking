using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class DecisionOption
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public int DecisionID { get; set; }
        public IEnumerable<OptionAdvantage> OptionAdvantages { get; set; }
        public IEnumerable<OptionRisk> OptionRisks { get; set; }




        public string Status { get; set; }
    }
}
