using System.Collections.Generic;
using GcashPaymentDataService;

namespace GcashPaymentAppService
{
    public class GcashAppService
    {
        private GcashDataService data = new GcashDataService();

        public double GetBalance()
        {
            return data.GetBalance();
        }

        public void CashIn(double amount)
        {
            double balance = data.GetBalance();
            balance += amount;
            data.UpdateBalance(balance);
        }

        public bool ExpressSend(string number, double amount)
        {
            double balance = data.GetBalance();

            if (amount <= balance)
            {
                balance -= amount;
                data.UpdateBalance(balance);
                data.AddTransaction(number, amount);
                return true;
            }

            return false;
        }

        public string GetMPIN()
        {
            return data.GetMPIN() ?? "0121";
        }

        public List<string> GetTransactionHistory()
        {
            return data.GetTransactionHistory() ?? new List<string>();
        }
    }
}