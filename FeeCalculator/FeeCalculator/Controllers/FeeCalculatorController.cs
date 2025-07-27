using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using Domain;

namespace FeeCalculator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeeCalculatorController : ControllerBase
    {
        private readonly IFeeCalculatorService _feeCalculatorService; private readonly DataAccess.ApplicationDbContext _context;

        public FeeCalculatorController(IFeeCalculatorService feeCalculatorService, DataAccess.ApplicationDbContext context)
        {
            _feeCalculatorService = feeCalculatorService;
            _context = context;
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> CalculateFee([FromBody] Transaction transaction)
        {
            var result = await _feeCalculatorService.CalculateFeeAsync(transaction);
            return Ok(result);
        }

        [HttpPost("calculate-batch")]
        public async Task<IActionResult> CalculateBatch([FromBody] List<Transaction> transactions)
        {
            var results = await _feeCalculatorService.CalculateBatchAsync(transactions);
            return Ok(results);
        }

        [HttpGet("history/{transactionId}")]
        public IActionResult GetHistoryByTransactionId(Guid transactionId)
        {
            var records = _context.FeeCalculationResults
                .Where(r => r.TransactionId == transactionId)
                .OrderByDescending(r => r.Timestamp)
                .ToList();

            if (!records.Any()) return NotFound("No history found for transaction.");

            return Ok(records);
        }

        [HttpGet("history")]
        public IActionResult GetHistory()
        {
            var records = _context.FeeCalculationResults
                .OrderByDescending(r => r.Timestamp)
                .ToList();

            return Ok(records);
        }

        [HttpPost("rules")]
        public IActionResult AddRule([FromBody] FeeRule rule)
        {
            _context.FeeRules.Add(rule);
            _context.SaveChanges();
            return Ok(rule);
        }
    }

}