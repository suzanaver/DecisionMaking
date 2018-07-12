using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class DecisionDocument
    {
        public int ID { get; set; }
        public string openeddate { get; set; }
        public string Department { get; set; }
        public string Subject { get; set; }
        public string Requireddecision { get; set; }
        public string Currentstatedesc { get; set; }
        public string Recandreason { get; set; }
        public string Decision { get; set; }
        public string Comments { get; set; }
        public IEnumerable<DecisionOption> DecisionOptions { get; set; }
        public IEnumerable<StatusProgress> StatusProgresses { get; set; }
        public IEnumerable<NextStep> NextSteps { get; set; }

        public string DeterminesTheDecision { get; set; }

        public string Status { get; set; }
    }
}
