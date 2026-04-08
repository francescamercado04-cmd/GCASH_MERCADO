using System;
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

            choice = Convert.ToInt32(Console.ReadLine());

            if (choice == 1) CashIn();
            else if (choice == 2) ExpressSend();
            else if (choice == 3) ViewTransactionHistory();
            else if (choice == 5) BuyLoad();
            else if (choice == 6) TransferToBank();

        } while (choice != 4);

        Console.WriteLine("Thank You for using GCash!");
    }

    static void CheckMPIN()
    {
        string input;
        string mpin = app.GetMPIN();

        do
        {
            Console.Write("Enter MPIN: ");
            input = Console.ReadLine();

            if (input != mpin)
                Console.WriteLine("Incorrect MPIN.");

        } while (input != mpin);

        Console.WriteLine("Access Granted!");
    }

    static void CashIn()
    {
        Console.Write("Enter Amount: P ");
        double amount = Convert.ToDouble(Console.ReadLine());

        app.CashIn(amount);

        Console.WriteLine("Cash In Successful!");
    }

    static void ExpressSend()
    {
        Console.Write("Send To: ");
        string number = Console.ReadLine();

        Console.Write("Enter Amount: P ");
        double amount = Convert.ToDouble(Console.ReadLine());

        Random rnd = new Random();
        int otp = rnd.Next(100000, 999999);

        Console.WriteLine("OTP: " + otp);
        Console.Write("Enter OTP: ");

        int userOtp = Convert.ToInt32(Console.ReadLine());

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
        Console.Write("Enter Mobile Number: ");
        string number = Console.ReadLine();

        Console.Write("Enter Amount: P ");
        double amount = Convert.ToDouble(Console.ReadLine());

        bool success = app.BuyLoad(number, amount);

        Console.WriteLine(success ? "Load Purchased Successfully!" : "Insufficient Balance.");
    }

    static void TransferToBank()
    {
        Console.Write("Enter Bank Name: ");
        string bank = Console.ReadLine();

        Console.Write("Enter Account Number: ");
        string acc = Console.ReadLine();

        Console.Write("Enter Amount: P ");
        double amount = Convert.ToDouble(Console.ReadLine());

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