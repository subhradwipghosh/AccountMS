using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountMicroservice.Models
{
    public class AccountSummary
    {
        public int CAid { get; set; }
        public double CAbal { get; set; }
        public int SAid { get; set; }
        public double SAbal { get; set; }
    }
}
