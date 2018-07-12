using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace DecisionMaking.Models
{
    public class CreateDecisionViewModel
    {
        public CreateDecisionViewModel()
        {
            Departments = new List<string>();
        }
        public IEnumerable<string> UserDetails { get; set; }

        public DecisionDocument CreateDecision { get; set; }
        public IEnumerable<string> Departments { get; set; }
    }
}