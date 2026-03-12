using System;
using GcashPaymentDataService;

namespace GcashPaymentAppService
{
    public class GcashAppService
    {
        private GcashDataService repo = new GcashDataService();

        public double GetBalance()
        {
            return repo.GetBalance();
        }

        public void CashIn(double amount)
        {
            double balance = repo.GetBalance();
            balance += amount;
            repo.UpdateBalance(balance);
        }

        public bool ExpressSend(double amount)
        {
            double balance = repo.GetBalance();

            if (amount <= balance)
            {
                balance -= amount;
                repo.UpdateBalance(balance);
                return true;
            }

            return false;
        }

        public string GetMPIN()
        {
            return repo.GetMPIN();
        }
    }
}