namespace website.Models
{
	public class orders
	{
		public int Id { get; set; }
		public int ordernumber { get; set; }
	    public string bookname { get; set; }
	    public string 	price { get; set; }
		public string customername { get; set; }
		public string orderadress { get; set; }
        public string paymentmethod { get; set; }
        public string deliverymethod { get; set; }
        public string orderstatus { get; set; }

    }
}
