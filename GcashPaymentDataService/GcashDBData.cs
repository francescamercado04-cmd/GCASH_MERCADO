using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;

namespace GcashPaymentDataService
{
    public class GcashDBData : IGcashDataService
    {
        private string connectionString =
            "Data Source=localhost\\SQLEXPRESS01;Initial Catalog=Gcash;Integrated Security=True;TrustServerCertificate=True;";

        private SqlConnection sqlconnection;
        public GcashDBData()
        {
            sqlconnection = new SqlConnection(connectionString);

            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var check = new SqlCommand("SELECT COUNT(*) FROM Wallet", conn);
            if (Convert.ToInt32(check.ExecuteScalar()) == 0)
            {
                var defaults = new GcashPaymentModels.GcashModels();
                using var ins = new SqlCommand("INSERT INTO Wallet (Balance, MPIN) VALUES (@Balance,@MPIN)", conn);
                ins.Parameters.AddWithValue("@Balance", defaults.Balance);
                ins.Parameters.AddWithValue("@MPIN", defaults.MPIN);
                ins.ExecuteNonQuery();
            }
        }
        public double GetBalance()
        {
            sqlconnection.Open();
            SqlCommand command = new SqlCommand("SELECT Balance FROM Wallet", sqlconnection);
            double balance = Convert.ToDouble(command.ExecuteScalar());
            sqlconnection.Close();
            return balance;
        }

        public void UpdateBalance(double amount)
        {
            sqlconnection.Open();
            SqlCommand command = new SqlCommand("UPDATE Wallet SET Balance = @Balance", sqlconnection);
            command.Parameters.AddWithValue("@Balance", amount);
            command.ExecuteNonQuery();
            sqlconnection.Close();
        }

        public string GetMPIN()
        {
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT TOP(1) MPIN FROM Wallet", conn))
            {
                conn.Open();
                var result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                    return null;
                return result.ToString();
            }
        }

        public void AddTransaction(string number, double amount)
        {
            sqlconnection.Open();
            SqlCommand command = new SqlCommand("INSERT INTO Transactions (Number, Amount, Date) VALUES (@Number, @Amount, @Date)", sqlconnection);
            command.Parameters.AddWithValue("@Number", number);
            command.Parameters.AddWithValue("@Amount", amount);
            command.Parameters.AddWithValue("@Date", DateTime.Now);
            
            sqlconnection.Close();
        }

        public List<string> GetTransactionHistory()
        {
            List<string> history = new List<string>();
            sqlconnection.Open();
            SqlCommand command = new SqlCommand("SELECT Number, Amount, Date FROM Transactions", sqlconnection);
            SqlDataReader reader = command.ExecuteReader();
            int i = 1;
            while (reader.Read())
            {
                string number = reader["Number"].ToString();
                double amount = Convert.ToDouble(reader["Amount"]);
                DateTime date = Convert.ToDateTime(reader["Date"]);
                history.Add(
                    "#" + i +
                    " | Number: " + number +
                    " | Amount: P " + amount +
                    " | Date: " + date.ToString("g")
                );
                i++;
            }
            reader.Close();
            sqlconnection.Close();
            return history;


        }
    }
}