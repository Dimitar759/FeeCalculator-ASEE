using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class FeeRule
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = default!;
        public string ConditionJson { get; set; } = default!; 
        public string CalculationJson { get; set; } = default!; 
        public int Priority { get; set; } 
        public bool IsActive { get; set; } = true;
    }

}
