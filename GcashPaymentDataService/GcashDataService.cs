using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace GcashPaymentDataService
{
    public class GcashDataService
    {
        IGcashDataService _dataService;

        public GcashDataService(IGcashDataService dataService)
        {
            _dataService = dataService;
        }

        public double GetBalance()
        {
            return _dataService.GetBalance();
        }

        public void UpdateBalance(double balance) 
        {
          _dataService.UpdateBalance(balance);
        }

        public string GetMPIN()
        {
            return _dataService.GetMPIN();
        }

        public void AddTransaction(string number, double amount)
        {
            _dataService.AddTransaction(number, amount);
        }

        public List<string> GetTransactionHistory()
        {
            return _dataService.GetTransactionHistory();
        }
    }
}
