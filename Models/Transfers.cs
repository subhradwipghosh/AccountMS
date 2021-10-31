using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountMicroservice.Models
{
    public class Transfers
    {
        public int source_accid { get; set; }
        public int destination_accid { get; set; }
        public int amount { get; set; }
    }
}
