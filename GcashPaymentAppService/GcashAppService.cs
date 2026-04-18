using System.Collections.Generic;
using GcashPaymentDataService;

namespace GcashPaymentAppService
{
    public class GcashAppService
    {
        private GcashDataService data = new GcashDataService(new GcashDBData());

        public double GetBalance()
        {
            return data.GetBalance();
        }

        public void CashIn(double amount)
        {
            if (amount <= 0) return; 

            double balance = data.GetBalance();
            balance += amount;
            data.UpdateBalance(balance);
        }

        public bool ExpressSend(string number, double amount)
        {
            if (amount <= 0) return false; 

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

        public bool BuyLoad(string number, double amount)
        {
            if (amount <= 0) return false; 

            double balance = data.GetBalance();

            if (amount <= balance)
            {
                balance -= amount;
                data.UpdateBalance(balance);
                data.AddTransaction("LOAD-" + number, amount);
                return true;
            }

            return false;
        }

        public bool TransferToBank(string bank, string account, double amount)
        {
            if (amount <= 0) return false; 

            double balance = data.GetBalance();

            if (amount <= balance)
            {
                balance -= amount;
                data.UpdateBalance(balance);
                data.AddTransaction(bank + "-" + account, amount);
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