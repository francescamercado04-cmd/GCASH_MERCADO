using System;
using GcashPaymentModels;

namespace GcashPaymentDataService
{
    public class GcashDataService
    {
        private GcashModels wallet = new GcashModels();

        public double GetBalance()
        {
            return wallet.Balance;
        }

        public void UpdateBalance(double amount)
        {
            wallet.Balance = amount;
        }

        public string GetMPIN()
        {
            return wallet.MPIN;
        }
    }
}