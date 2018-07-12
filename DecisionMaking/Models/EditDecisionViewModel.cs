using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace DecisionMaking.Models
{
    public class EditDecisionViewModel
    {
        public DecisionDocument CreateDecision { get; set; }
        public IEnumerable<string> Departments { get; set; }


    }
}