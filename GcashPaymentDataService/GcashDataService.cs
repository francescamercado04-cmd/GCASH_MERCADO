using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using GcashPaymentModels;

namespace GcashPaymentDataService
{
    public class GcashDataService
    {
        private GcashModels wallet = new GcashModels();

        private List<TransactionItem> transactions = new List<TransactionItem>();

        private string _walletFile;
        private string _transactionFile;

        public GcashDataService()
        {
            _walletFile = $"{AppDomain.CurrentDomain.BaseDirectory}/gcash.json";
            _transactionFile = $"{AppDomain.CurrentDomain.BaseDirectory}/transactions.json";

            Initialize();
        }

        private void Initialize()
        {
            if (!File.Exists(_walletFile))
                SaveWallet();

            if (!File.Exists(_transactionFile))
                SaveTransactions();

            LoadWallet();
            LoadTransactions();
        }

        private void SaveWallet()
        {
            File.WriteAllText(_walletFile, JsonSerializer.Serialize(wallet, new JsonSerializerOptions { WriteIndented = true }));
        }

        private void LoadWallet()
        {
            try
            {
                if (!File.Exists(_walletFile))
                {
                    wallet = new GcashModels();
                    SaveWallet();
                    return;
                }

                string json = File.ReadAllText(_walletFile);

                if (string.IsNullOrWhiteSpace(json))
                {
                    wallet = new GcashModels();
                    SaveWallet();
                    return;
                }

                wallet = JsonSerializer.Deserialize<GcashModels>(json) ?? new GcashModels();
            }
            catch
            {
                wallet = new GcashModels();
                SaveWallet();
            }
        }

        // ================= TRANSACTION (NO SEPARATE FILE CLASS) =================

        private class TransactionItem
        {
            public string Number { get; set; }
            public double Amount { get; set; }
            public DateTime Date { get; set; }
        }

        private void SaveTransactions()
        {
            File.WriteAllText(_transactionFile,
                JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true }));
        }

        private void LoadTransactions()
        {
            string json = File.ReadAllText(_transactionFile);

            if (!string.IsNullOrWhiteSpace(json))
                transactions = JsonSerializer.Deserialize<List<TransactionItem>>(json) ?? new List<TransactionItem>();
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

        // ================= WALLET =================

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

            if (wallet == null)
                wallet = new GcashModels();

            if (string.IsNullOrWhiteSpace(wallet.MPIN))
                return "0121";

            return wallet.MPIN;
        }
    }
}