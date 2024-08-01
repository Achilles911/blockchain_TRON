using Newtonsoft.Json;

class Program
{
    public class TransactionInfo
    {
        public string ContractType { get; set; }
        public long Amount { get; set; }
        public long Timestamp { get; set; }
    }

    public class RiskAnalysis
    {
        public string TransactionId { get; set; }
        public string TransactionType { get; set; }
        public long Amount { get; set; }
        public long Timestamp { get; set; }
        public string RiskLevel { get; set; }
    }

    static async Task Main(string[] args)
    {
        string transactionId = "853793d552635f533aa982b92b35b00e63a1c1add062c099da2450a15119bcb2";
        var riskAnalysis = await GetTransactionRiskLevel(transactionId);
        if (riskAnalysis != null)
        {
            Console.WriteLine($"Transaction ID: {riskAnalysis.TransactionId}");
            Console.WriteLine($"Transaction Type: {riskAnalysis.TransactionType}");
            Console.WriteLine($"Amount: {riskAnalysis.Amount}");
            Console.WriteLine($"Timestamp: {riskAnalysis.Timestamp}");
            Console.WriteLine($"Risk Level: {riskAnalysis.RiskLevel}");
        }
    }

    public static async Task<RiskAnalysis> GetTransactionRiskLevel(string transactionId)
    {
        string url = $"https://apilist.tronscan.org/api/transaction-info?hash={transactionId}";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                TransactionInfo transactionInfo = JsonConvert.DeserializeObject<TransactionInfo>(responseBody);

                string riskLevel = DetermineRiskLevel(transactionInfo);

                return new RiskAnalysis
                {
                    TransactionId = transactionId,
                    TransactionType = transactionInfo.ContractType,
                    Amount = transactionInfo.Amount,
                    Timestamp = transactionInfo.Timestamp,
                    RiskLevel = riskLevel
                };
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Ошибка при выполнении запроса: {e.Message}");
                return null;
            }
        }
    }

    public static string DetermineRiskLevel(TransactionInfo transactionInfo)
    {
        
        if (transactionInfo.ContractType == "TransferContract" && transactionInfo.Amount > 1000000)//ну, тут сумма роляет
        {
            return "High";
        }
        return "Low";
    }
}
