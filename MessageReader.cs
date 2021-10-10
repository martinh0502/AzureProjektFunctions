using System;
using Az.Function;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Az.MessageReader
{
    public static class MessageReader
    {
        [Function("MessageReader")]
        [TableOutput("Currency", Connection = "AzureWebJobsStorage")]
        public static TableData Run([QueueTrigger("currency", Connection = "AzureWebJobsStorage")] NewCurrencyValue currency,
            FunctionContext context)
        {
            var logger = context.GetLogger("MessageReader");
            logger.LogInformation($"C# Queue trigger function processed: {currency}");

            return new TableData
            {
                PartitionKey = currency.DeviceId,
                RowKey = $"{(DateTimeOffset.MaxValue.Ticks-currency.Timestamp.Ticks):d10}-{Guid.NewGuid():N}",
                Value = currency.Value,
                Timestamp = currency.Timestamp,
                CurrencyId = currency.currencyId
            };
        }

        
    }

    public class TableData
        {
            public string PartitionKey { get; set; }
            public string RowKey { get; set; }

            public DateTimeOffset Timestamp { get; set; }

            public double Value { get; set; }

            public string CurrencyId { get; set; }
        }
}
