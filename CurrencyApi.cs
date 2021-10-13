using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Az.MessageReader;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Az.CurrencyApi
{
    public static class CurrencyApi
    {
        [Function("CurrencyApi")]
        public static async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
            [TableInput("Currency", "1", Take = 60)] TableData[] currency,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("CurrencyApi");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            
            if(currency.Any())
            {
                var apiResponse = new ApiResponse
                {
                    Values = new List<double>(),
                    CurrencyId = currency.First().CurrencyId,
                    Start = currency.Last().Timestamp,
                    End = currency.First().Timestamp
                };
                
                foreach (var item in currency)
                {
                    apiResponse.Values.Add(item.Value);
                }

                await response.WriteAsJsonAsync(apiResponse);
            }

            return response;
        }
    }

    public class ApiResponse
    {
        public List<double> Values { get; set; }
        public string CurrencyId { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
    }
}
