using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class FeeCalculationResult
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid TransactionId { get; set; }
        public decimal CalculatedFee { get; set; }
        public string AppliedRule { get; set; } = default!;
        public string InputDataJson { get; set; } = default!;
        public string OutputDataJson { get; set; } = default!;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

}
