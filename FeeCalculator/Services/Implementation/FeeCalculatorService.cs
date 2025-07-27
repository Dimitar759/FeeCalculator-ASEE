using Application.Services;
using DataAccess;
using Domain;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Services.Interface;

namespace Application.Services
{
    public class FeeCalculatorService : IFeeCalculatorService
    {
        private readonly ApplicationDbContext _context;

        public FeeCalculatorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FeeCalculationResult> CalculateFeeAsync(Transaction tx)
        {
            var rules = await _context.FeeRules
                .Where(r => r.IsActive)
                .OrderBy(r => r.Priority)
                .ToListAsync();

            decimal fee = 0;
            string appliedRule = "No Rule";

            foreach (var rule in rules)
            {
                var condition = JsonConvert.DeserializeObject<Dictionary<string, object>>(rule.ConditionJson);

                bool match = true;

                foreach (var cond in condition)
                {
                    switch (cond.Key)
                    {
                        case "Type":
                            if (!string.Equals(tx.Type, cond.Value.ToString(), StringComparison.OrdinalIgnoreCase))
                                match = false;
                            break;
                        case "MaxAmount":
                            if (tx.Amount > Convert.ToDecimal(cond.Value)) match = false;
                            break;
                        case "MinAmount":
                            if (tx.Amount < Convert.ToDecimal(cond.Value)) match = false;
                            break;
                        case "CreditScoreGreaterThan":
                            if (tx.Client.CreditScore <= Convert.ToInt32(cond.Value)) match = false;
                            break;
                    }
                }

                if (!match) continue;

                var calculation = JsonConvert.DeserializeObject<Dictionary<string, object>>(rule.CalculationJson);

                switch (calculation["Type"].ToString())
                {
                    case "fixed":
                        fee += Convert.ToDecimal(calculation["Value"]);
                        appliedRule = rule.Name;
                        break;
                    case "percentage":
                        fee += tx.Amount * (Convert.ToDecimal(calculation["Percent"]) / 100);
                        appliedRule = rule.Name;
                        break;
                    case "percentage_plus_fixed_cap":
                        var percent = tx.Amount * (Convert.ToDecimal(calculation["Percent"]) / 100);
                        var fixedPart = Convert.ToDecimal(calculation["Fixed"]);
                        var total = percent + fixedPart;
                        var max = Convert.ToDecimal(calculation["Max"]);
                        fee += total > max ? max : total;
                        appliedRule = rule.Name;
                        break;
                    case "discount_percent":
                        var discount = (fee * Convert.ToDecimal(calculation["Value"])) / 100;
                        fee -= discount;
                        appliedRule += $" + {rule.Name}";
                        break;
                }
            }

            var result = new FeeCalculationResult
            {
                TransactionId = tx.Id,
                CalculatedFee = Math.Round(fee, 2),
                AppliedRule = appliedRule,
                InputDataJson = JsonConvert.SerializeObject(tx),
                OutputDataJson = JsonConvert.SerializeObject(new { Fee = fee })
            };

            _context.FeeCalculationResults.Add(result);
            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<List<FeeCalculationResult>> CalculateBatchAsync(List<Transaction> transactions)
        {
            var results = new List<FeeCalculationResult>();
            var tasks = transactions.Select(t => CalculateFeeAsync(t)).ToList();
            results = (await Task.WhenAll(tasks)).ToList();
            await _context.SaveChangesAsync(); // Save all results at once
            return results;
        }
    }

}