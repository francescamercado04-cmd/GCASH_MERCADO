using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace GcashPaymentDataService
{
    public class GcashDBData : IGcashDataService
    {
        private string connectionString =
            "Data Source=localhost\\SQLEXPRESS;Initial Catalog=Gcash;Integrated Security=True;TrustServerCertificate=True;";

        public GcashDBData()
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            using var check = new SqlCommand("SELECT COUNT(*) FROM Wallet", conn);

            if (Convert.ToInt32(check.ExecuteScalar()) == 0)
            {
                var defaults = new GcashPaymentModels.GcashModels();

                using var ins = new SqlCommand(
                    "INSERT INTO Wallet (Balance, MPIN) VALUES (@Balance,@MPIN)", conn);

                ins.Parameters.AddWithValue("@Balance", defaults.Balance);
                ins.Parameters.AddWithValue("@MPIN", defaults.MPIN);
                ins.ExecuteNonQuery();
            }
        }

        public double GetBalance()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT Balance FROM Wallet", conn);
                return Convert.ToDouble(command.ExecuteScalar());
            }
        }

        public void UpdateBalance(double amount)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("UPDATE Wallet SET Balance = @Balance", conn);
                command.Parameters.AddWithValue("@Balance", amount);
                command.ExecuteNonQuery();
            }
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
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(
                    "INSERT INTO Transactions (Number, Amount, Date) VALUES (@Number, @Amount, @Date)",
                    conn);

                command.Parameters.AddWithValue("@Number", number);
                command.Parameters.AddWithValue("@Amount", amount);
                command.Parameters.AddWithValue("@Date", DateTime.Now);

                command.ExecuteNonQuery(); 
            }
        }

        public List<string> GetTransactionHistory()
        {
            List<string> history = new List<string>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(
                    "SELECT Number, Amount, Date FROM Transactions", conn);

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
            }

            return history;
        }
    }
}