using System;
using System.Collections.Generic;
using GcashPaymentModels;

namespace GcashPaymentDataService
{
    public class GcashInMemoryData : IGcashDataService
    {
        private GcashModels wallet = new GcashModels();
        private List<TransactionItem> transactions = new List<TransactionItem>();

        private class TransactionItem
        {
            public string Number { get; set; }
            public double Amount { get; set; }
            public DateTime Date { get; set; }
        }

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
            if (wallet == null)
                wallet = new GcashModels();

            if (string.IsNullOrWhiteSpace(wallet.MPIN))
                return "0121";

            return wallet.MPIN;
        }

        public void AddTransaction(string number, double amount)
        {
            transactions.Add(new TransactionItem
            {
                Number = number,
                Amount = amount,
                Date = DateTime.Now
            });
        }

        public List<string> GetTransactionHistory()
        {
            List<string> history = new List<string>();

            int i = 1;

            foreach (var t in transactions)
            {
                history.Add(
                    "#" + i +
                    " | Number: " + t.Number +
                    " | Amount: P " + t.Amount +
                    " | Date: " + t.Date
                );

                i++;
            }

            return history;
        }
    }
}