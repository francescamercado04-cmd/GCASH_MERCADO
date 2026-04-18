using System;
using System.Linq;
using GcashPaymentAppService;

class GCASH
{
    static GcashAppService app = new GcashAppService();

    static void Main(string[] args)
    {
        CheckMPIN();

        int choice;

        do
        {
            Console.WriteLine("\n===== GCASH MENU =====");
            Console.WriteLine("Current Balance: P " + app.GetBalance());
            Console.WriteLine("1. Cash In");
            Console.WriteLine("2. Express Send");
            Console.WriteLine("3. View Transaction History");
            Console.WriteLine("4. Exit");
            Console.WriteLine("5. Buy Load");
            Console.WriteLine("6. Transfer to Bank");
            Console.Write("Choose: ");

            choice = GetNumberOnlyInt();

            if (choice == 1) CashIn();
            else if (choice == 2) ExpressSend();
            else if (choice == 3) ViewTransactionHistory();
            else if (choice == 5) BuyLoad();
            else if (choice == 6) TransferToBank();

        } while (choice != 4);

        Console.WriteLine("Thank You for using GCash!");
    }

    
         static int GetNumberOnlyInt()
    {
        int value;

        while (!int.TryParse(Console.ReadLine(), out value))
        {
            Console.Write("Numbers only. Try again: ");
        }

        return value;
    }

    static double GetValidAmount()
    {
        double amount;

        while (true)
        {
            if (!double.TryParse(Console.ReadLine(), out amount))
            {
                Console.Write("Numbers only. Enter amount: ");
                continue;
            }

            if (amount <= 0)
            {
                Console.Write("Amount must be greater than 0: ");
                continue;
            }

            return amount;
        }
    }

    static string GetLettersOnly(string message)
    {
        string input;

        do
        {
            Console.Write(message);
            input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input cannot be empty.");
                continue;
            }

            if (!input.All(char.IsLetter))
            {
                Console.WriteLine("Letters only. Try again.");
                input = "";
            }

        } while (string.IsNullOrWhiteSpace(input));

        return input;
    }

    static string GetDigitsOnly(string message)
    {
        string input;

        do
        {
            Console.Write(message);
            input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input cannot be empty.");
                continue;
            }

            if (!input.All(char.IsDigit))
            {
                Console.WriteLine("Numbers only. Try again.");
                input = "";
            }

        } while (string.IsNullOrWhiteSpace(input));

        return input;
    }

    
    static void CheckMPIN()
    {
        string input;
        string mpin = app.GetMPIN();

        do
        {
            input = GetDigitsOnly("Enter MPIN: ");

            if (input != mpin)
                Console.WriteLine("Incorrect MPIN.");

        } while (input != mpin);

        Console.WriteLine("Access Granted!");
    }

    static void CashIn()
    {
        Console.Write("Enter Amount: P ");
        double amount = GetValidAmount();

        app.CashIn(amount);

        Console.WriteLine("Cash In Successful!");
    }

    static void ExpressSend()
    {
        string number = GetDigitsOnly("Send To (numbers only): ");

        Console.Write("Enter Amount: P ");
        double amount = GetValidAmount();

        Random rnd = new Random();
        int otp = rnd.Next(100000, 999999);

        Console.WriteLine("OTP: " + otp);

        int userOtp;
        Console.Write("Enter OTP: ");
        userOtp = GetNumberOnlyInt();

        if (userOtp == otp)
        {
            bool sent = app.ExpressSend(number, amount);
            Console.WriteLine(sent ? "Sent Successfully!" : "Insufficient Balance.");
        }
        else
        {
            Console.WriteLine("Invalid OTP.");
        }
    }

    static void BuyLoad()
    {
        string number = GetDigitsOnly("Enter Mobile Number: ");

        Console.Write("Enter Amount: P ");
        double amount = GetValidAmount();

        bool success = app.BuyLoad(number, amount);

        Console.WriteLine(success ? "Load Purchased Successfully!" : "Insufficient Balance.");
    }

    static void TransferToBank()
    {
        string bank = GetLettersOnly("Enter Bank Name: ");
        string acc = GetDigitsOnly("Enter Account Number: ");

        Console.Write("Enter Amount: P ");
        double amount = GetValidAmount();

        bool success = app.TransferToBank(bank, acc, amount);

        Console.WriteLine(success ? "Transfer Successful!" : "Insufficient Balance.");
    }

    static void ViewTransactionHistory()
    {
        var history = app.GetTransactionHistory();

        if (history.Count == 0)
        {
            Console.WriteLine("No transactions yet.");
            return;
        }

        foreach (var item in history)
        {
            Console.WriteLine(item);
        }
    }
}