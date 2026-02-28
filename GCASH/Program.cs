using System;
using System.Transactions;

class GCASH
{
    static double balance = 10000;

    static void Main(String[] args) { 


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

    if (amount <= balance)
{
    balance -= amount;


    Console.WriteLine("\nConfirmed");
    Console.WriteLine("Sent Successfully!");

    Console.WriteLine("\n-----RECEIPT-----");
    Console.WriteLine("To: " + number);
    Console.WriteLine("Amount Sent: P" + amount);
    Console.WriteLine("Remaining Balance: P" + balance);
}
else
{
    Console.WriteLine("Insufficient Balance");
}
}

}
