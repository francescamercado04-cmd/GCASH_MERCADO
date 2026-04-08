using System;
using System.Collections.Generic;
using System.Text;

namespace GcashPaymentDataService
{
    public interface IGcashDataService
    {
        double GetBalance();
        void UpdateBalance(double amount);
        string GetMPIN();
        void AddTransaction(string number, double amount);
        List<string> GetTransactionHistory();
    }
}
