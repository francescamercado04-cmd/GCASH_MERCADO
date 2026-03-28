using GcashPaymentModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace GcashPaymentDataService
{
    public class GcashJsonData
    {
        private GcashModels wallet = new GcashModels();
        private List<TransactionItem> transactions = new List<TransactionItem>();

        private string _walletFile;
        private string _transactionFile;

        public GcashJsonData()
        {
            _walletFile = $"{AppDomain.CurrentDomain.BaseDirectory}/gcash.json";
            _transactionFile = $"{AppDomain.CurrentDomain.BaseDirectory}/transactions.json";

            if (!File.Exists(_walletFile))
            {
                wallet = new GcashModels();
                SaveWallet();
            }

            if (!File.Exists(_transactionFile))
            {
                transactions = new List<TransactionItem>();
                SaveTransactions();
            }

            LoadWallet();
            LoadTransactions();
        }

        private class TransactionItem
        {
            public string Number { get; set; }
            public double Amount { get; set; }
            public DateTime Date { get; set; }
        }

        private void LoadWallet()
        {
            try
            {
                string json = File.ReadAllText(_walletFile);

                if (string.IsNullOrWhiteSpace(json))
                {
                    wallet = new GcashModels();
                    return;
                }

                wallet = JsonSerializer.Deserialize<GcashModels>(json);

                if (wallet == null)
                    wallet = new GcashModels();
            }
            catch
            {
                wallet = new GcashModels();
            }
        }

        private void SaveWallet()
        {
            File.WriteAllText(_walletFile,
                JsonSerializer.Serialize(wallet, new JsonSerializerOptions { WriteIndented = true }));
        }

        private void LoadTransactions()
        {
            try
            {
                string json = File.ReadAllText(_transactionFile);

                if (string.IsNullOrWhiteSpace(json))
                {
                    transactions = new List<TransactionItem>();
                    return;
                }

                transactions = JsonSerializer.Deserialize<List<TransactionItem>>(json);

                if (transactions == null)
                    transactions = new List<TransactionItem>();
            }
            catch
            {
                transactions = new List<TransactionItem>();
            }
        }

        private void SaveTransactions()
        {
            File.WriteAllText(_transactionFile,
                JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true }));
        }

        public double GetBalance()
        {
            LoadWallet();
            return wallet.Balance;
        }

        public void UpdateBalance(double amount)
        {
            LoadWallet();
            wallet.Balance = amount;
            SaveWallet();
        }

        public string GetMPIN()
        {
            LoadWallet();
            return wallet.MPIN ?? "0121";
        }

        public void AddTransaction(string number, double amount)
        {
            LoadTransactions();

            transactions.Add(new TransactionItem
            {
                Number = number,
                Amount = amount,
                Date = DateTime.Now
            });

            SaveTransactions();
        }

        public List<string> GetTransactionHistory()
        {
            LoadTransactions();

            List<string> history = new List<string>();

            int i = 1;

            foreach (var t in transactions)
            {
                history.Add("#" + i + " | Number: " + t.Number + " | Amount: P " + t.Amount + " | Date: " + t.Date);
                i++;
            }

            return history;
        }
    }
}