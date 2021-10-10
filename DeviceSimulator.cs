using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Az.Function
{
    public class NewCurrencyValue
    {
        public string DeviceId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public double Value { get; set; }

        public string currencyId { get; set; } 
    }
    public static class DeviceSimulator
    {
        [Function("DeviceSimulator")]
        [QueueOutput("currency", Connection="AzureWebJobsStorage")]
        public static NewCurrencyValue Run([TimerTrigger("* * * * *")] MyInfo myTimer, FunctionContext context)
        {
            var logger = context.GetLogger("DeviceSimulator");
            logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var rng = new Random();

            var value = new NewCurrencyValue{
                DeviceId = "1",
                Timestamp = DateTimeOffset.UtcNow,
                Value = 0.1 * (Convert.ToDouble(DateTime.Today.ToOADate()) * rng.NextDouble()),
                currencyId = "USD"
            };

            return value;
        }
    }

    public class MyInfo
    {
        public bool IsPastDue { get; set; }
    }

}
