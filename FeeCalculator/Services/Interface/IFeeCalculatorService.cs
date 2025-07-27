using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IFeeCalculatorService
    {
        Task<FeeCalculationResult> CalculateFeeAsync(Transaction transaction);
        Task<List<FeeCalculationResult>> CalculateBatchAsync(List<Transaction> transactions);
    }
}
