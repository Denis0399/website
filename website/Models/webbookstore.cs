using LinqToDB.Mapping;
using System.ComponentModel.DataAnnotations.Schema;
using Xamarin.Forms;

namespace website.Models
{
    public class webbookstore
    {
        public int Id { get; set; }


        public string bookname { get; set; }


        public string price { get; set; }
        public string descrip { get; set; }
        public string genre { get; set; }

      

        public int stock { get; set; }
        public DateTime releasedate { get; set; }
        public byte[] imagename { get; set; }
        [NotMapped]
        public string ImageBase64 => imagename != null ? Convert.ToBase64String(imagename) : null;

    }

   
}
