namespace BadmintonBooking.ViewModels
{
    public class InvoiceViewModel
    {
        public int PId;
        public string formattedDate;
        public string formattedTime;
        public string toUser;
        public string typeOfBooking;
        public string courtName;
        public decimal amount;
        public int? Quantity; 
        public decimal amountMinus2
        {
            get { return amount; }
            set { amountMinus2 = value; }
        }

    }
}
