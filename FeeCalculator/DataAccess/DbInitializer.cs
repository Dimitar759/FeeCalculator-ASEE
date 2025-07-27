using Domain;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DataAccess
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.Migrate();

            if (context.FeeRules.Any()) return;

            var rules = new List<FeeRule>
            {
                new FeeRule
                {
                    Name = "POS Fee",
                    Priority = 1,
                    ConditionJson = JsonConvert.SerializeObject(new {
                        Type = "POS",
                        MaxAmount = 100
                    }),
                    CalculationJson = JsonConvert.SerializeObject(new {
                        Type = "fixed",
                        Value = 0.2
                    })
                },
                new FeeRule
                {
                    Name = "POS Fee > 100€",
                    Priority = 2,
                    ConditionJson = JsonConvert.SerializeObject(new {
                        Type = "POS",
                        MinAmount = 100.01
                    }),
                    CalculationJson = JsonConvert.SerializeObject(new {
                        Type = "percentage",
                        Percent = 0.2
                    })
                },
                new FeeRule
                {
                    Name = "E-Commerce Fee",
                    Priority = 3,
                    ConditionJson = JsonConvert.SerializeObject(new {
                        Type = "e-commerce"
                    }),
                    CalculationJson = JsonConvert.SerializeObject(new {
                        Type = "percentage_plus_fixed_cap",
                        Percent = 1.8,
                        Fixed = 0.15,
                        Max = 120.0
                    })
                },
                new FeeRule
                {
                    Name = "High CreditScore Discount",
                    Priority = 4,
                    ConditionJson = JsonConvert.SerializeObject(new {
                        CreditScoreGreaterThan = 400
                    }),
                    CalculationJson = JsonConvert.SerializeObject(new {
                        Type = "discount_percent",
                        Value = 1.0
                    })
                }
            };

            context.FeeRules.AddRange(rules);
            context.SaveChanges();
        }
    }
}
