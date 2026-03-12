using System;
using System.Transactions;
using GcashPaymentAppService;
using GcashPaymentModels;
class GCASH
{
    static double balance = 10000;

    static GcashAppService app = new GcashAppService();
    static GcashModels model = new GcashModels();

    static void Main(String[] args) {

        
        CheckMPIN();

        int choice;

do
{
    Console.WriteLine("\n===== GCASH MENU =====");
    Console.WriteLine("Current Balance: P " + balance);
    Console.WriteLine("1. Cash in");
    Console.WriteLine("2. Express Send");
    Console.WriteLine("3. Exit ");
    Console.Write("Choose: ");
    choice = Convert.ToInt32(Console.ReadLine());

    if (choice == 1)
        CashIn();
    else if (choice == 2)
        ExpressSend();

}while (choice != 3);

Console.WriteLine("Thank You for using GCash!");
}
    static void CheckMPIN()
    {
        string input;

        do
        {
            Console.Write("Enter MPIN: ");
            input = Console.ReadLine();

            if (input != app.GetMPIN())
            {
                Console.WriteLine("Incorrect MPIN. Try again.");
            }

        } while (input != app.GetMPIN());

        Console.WriteLine("Access Granted!\n");
    }
    static void CashIn()
{
    Console.WriteLine("\n ----- CASH IN OPTIONS -----");
    Console.WriteLine("1. Over the Counter ");
    Console.WriteLine("2. Local Banks ");
    Console.WriteLine("3. Global Banks & Partners ");
    
    Console.Write("Choose Option:  ");
    int option = Convert.ToInt32(Console.ReadLine());

    Console.Write("Enter Amount to cash in: P");
    double amount = Convert.ToDouble(Console.ReadLine());

    balance += amount;

    Console.WriteLine("Cash In Successful!");
    Console.WriteLine("New Balance: P" + balance);
}

static void ExpressSend()
    {
    Console.Write("\nSend To (Enter Number):  ");
    string number = Console.ReadLine();

    Console.Write("Enter Amount to Send: P" );
    double amount = Convert.ToDouble(Console.ReadLine());

        Random rnd = new Random();
        int otp = rnd.Next(100000, 999999);

        Console.WriteLine("OTP: " + otp);
        Console.Write("Enter OTP: ");

        int userOtp = Convert.ToInt32(Console.ReadLine());

        if (userOtp == otp)
        {
            bool sent = app.ExpressSend(amount);

            if (sent)
            {
                Console.WriteLine("\nConfirmed");
                Console.WriteLine("Sent Successfully!");

                Console.WriteLine("\n-----RECEIPT-----");
                Console.WriteLine("To: " + number);
                Console.WriteLine("Amount Sent: P" + amount);
                Console.WriteLine("Remaining Balance: P" + balance);
            }
            else
            {
                Console.WriteLine("Insufficient Balance.");
            }
        }
        else
        {
            Console.WriteLine("Invalid OTP. Transaction Cancelled.");
        }
    }

}
