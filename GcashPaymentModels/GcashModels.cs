using System;

namespace GcashPaymentModels
{
    public class GcashModels
    {
        public double Balance { get; set; }
        public string MPIN { get; set; }

        public GcashModels()
        {
            Balance = 10000;
            MPIN = "0121";
        }
    }
}