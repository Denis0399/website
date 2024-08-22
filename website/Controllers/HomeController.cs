using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Data.SqlClient;
using website.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Html;
using Korzh.EasyQuery.Linq;
using LinqToDB;
using System.IO.Pipelines;
using System;

namespace website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        private readonly bookstoreDBconext _context;
        private bookstoreDBconext Context { get; }
        public HomeController(ILogger<HomeController> logger, bookstoreDBconext dbContext)
        {
            _logger = logger;
            _context = dbContext;
            this.Context = _context;
        }

        public IActionResult Index()
        {
          
            var genres1 = _context.genres.ToList();
            ViewBag.genres1 = genres1;
       
            SqlConnection conn = new SqlConnection(@"Data Source=192.168.1.14,49170;Database=bookstore; User id=sa; Password=123;");
            conn.Open();
            List<webbookstore> Webbookstore = new List<webbookstore>();
            SqlCommand cmd = new SqlCommand($"SELECT * FROM webbookstore  ORDER BY  releasedate  DESC  ", conn);
            cmd.ExecuteNonQuery();
            SqlDataReader reader3 = cmd.ExecuteReader();


            while (reader3.Read())
            {
                byte[] imagedata = reader3["imagename"] as byte[];
                Webbookstore.Add(new webbookstore
                {
                    Id = Convert.ToInt32(reader3["id"]),
                    bookname = reader3["bookname"].ToString(),
                    price = reader3["price"].ToString(),
                    descrip = reader3["descrip"].ToString(),
                    genre = reader3["genre"].ToString(),
                  
                    stock = Convert.ToInt32(reader3["stock"]),
                    releasedate = Convert.ToDateTime(reader3["releasedate"]),
                    imagename = imagedata
                });

            }
            reader3.Close();
          
            Webbookstore = Webbookstore.Take(5).ToList();
            ViewBag.books = Webbookstore;
           
            return View( );
        }
        [HttpPost]
        public IActionResult allgoods(  string genrename )
        {
            List<webbookstore> Webbookstore2 = new List<webbookstore>();
            SqlConnection conn = new SqlConnection(@"Data Source=192.168.1.14,49170;Database=bookstore; User id=sa; Password=123;");
            conn.Open();
            SqlCommand cmd = new SqlCommand($"SELECT * FROM webbookstore WHERE genre='{genrename}' ORDER BY  releasedate  DESC  ", conn);
            cmd.ExecuteNonQuery();
            SqlDataReader reader3 = cmd.ExecuteReader();


            while (reader3.Read())
            {
                byte[] imagedata = reader3["imagename"] as byte[];
                Webbookstore2.Add(new webbookstore
                {
                    Id = Convert.ToInt32(reader3["id"]),
                    bookname = reader3["bookname"].ToString(),
                    price = reader3["price"].ToString(),
                    descrip = reader3["descrip"].ToString(),
                    genre = reader3["genre"].ToString(),
                   
                    stock = Convert.ToInt32(reader3["stock"]),
                    releasedate = Convert.ToDateTime(reader3["releasedate"]),
                    imagename = imagedata
                });

            }
            reader3.Close();
            Webbookstore2= Webbookstore2.Take(5).ToList();

            var books = _context.webbookstore.ToList();
            var books2 = _context.webbookstore.ToList();
            books2.Clear();
          
            ViewBag.genre = genrename;
            foreach (webbookstore webbookstore in books)
            {

                    if (webbookstore.genre == genrename)
                    {
                        books2.Add(webbookstore);

                       
                    }
                
            }
            ViewBag.booksnew = Webbookstore2;

            ViewBag.books = books2;
            conn.Close();
            return View( );
        }
        [HttpPost]
        public IActionResult orderconfirmation( int ordernumber,string bookname,string price,string customername,string orderadress, string courer,string self, string cash, string card)
        {
          
            var orders = _context.orders.ToList();
            foreach (orders order in orders)
            {
                if (order.ordernumber == ordernumber)
                {
                    Random r = new Random();
                    ordernumber = r.Next(10000000, 99999999);



                }

            }
            string orderadressshow = "адрес доставкии";

            if (courer != null)
            {

                ViewBag.orderadress = orderadressshow;
            }
            else {
                orderadressshow = "";
                ViewBag.orderadress = orderadressshow;

            }

            string cs = "Data Source=DESKTOP-OFJ100G\\SQLEXPRESS;Database=bookstore; User=sa; Password=123;";

            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            if (courer == null && card==null)
    {
                 cmd = new SqlCommand($"INSERT INTO orders VALUES({ordernumber},'{bookname}','{price}','{customername}','{orderadress}','{cash}','{self}','готовиться')", con);
                cmd.ExecuteNonQuery();
            }
            else if (self == null && card == null) 
            {
                 cmd = new SqlCommand($"INSERT INTO orders VALUES({ordernumber},'{bookname}','{price}','{customername}','{orderadress}','{cash}','{courer}','готовиться')", con);
                cmd.ExecuteNonQuery();
            }
            else if (self == null && cash == null)
            {
                 cmd = new SqlCommand($"INSERT INTO orders VALUES({ordernumber},'{bookname}','{price}','{customername}','{orderadress}','{card}','{courer}','готовиться')", con);
                cmd.ExecuteNonQuery();
            }
            else if (courer == null && cash == null)
            {
                 cmd = new SqlCommand($"INSERT INTO orders VALUES({ordernumber},'{bookname}','{price}','{customername}','{orderadress}','{card}','{self}','готовиться')", con);
                cmd.ExecuteNonQuery();
            }
          

            var books = _context.webbookstore.ToList();
			foreach (webbookstore webbookstore in books) {
                if (webbookstore.bookname==bookname) {
                    cmd = new SqlCommand($"UPDATE webbookstore SET stock =stock-1 WHERE bookname='{bookname}';", con);
                    cmd.ExecuteNonQuery();
                    break;
                }
            }


            con.Close();

        
            orders.Clear();
             orders = _context.orders.ToList();

            var orders1 = _context.orders.ToList();
            orders1.Clear();
            foreach (orders order in orders)
            {
                if (ordernumber == order.ordernumber)
                {

                    orders1.Add(order);


                }
            }
            ViewBag.orders = orders1;

            return View(); }
        [HttpPost]
        public IActionResult orderpage(string bookname ,string price)
        { 
            ViewBag.bookname = bookname;
            ViewBag.price = price;
            
            
            return View(); }
      
        public IActionResult checkorder(string ordercheck)
        {
            var orders = _context.orders.ToList();
            var orders2 = _context.orders.ToList();
            orders2.Clear();
            int order=Convert.ToInt32(ordercheck);
            bool eror = true;
            ViewBag.eror = eror;
           
            

            foreach (orders order1 in orders)
        {
                if (order1.ordernumber == order)
                {
                    orders2.Add(order1);
                    eror = false;
                    string orderadressshow = "адрес доставкии";

                    if (order1.deliverymethod == "самовывоз")
                    {
                        orderadressshow = "";
                        ViewBag.orderadress = orderadressshow;
                       
                    }
                    else
                    {

                        ViewBag.orderadress = orderadressshow;

                    }

                    ViewBag.eror = eror;
                    break;

                }
                else if (order1.ordernumber != order)
                {

                    eror = true;

                    ViewBag.eror = eror;
                }
            }
            string ordererorr = "убедитесь что вы парвильно ввели номер вашего заказа";
            ViewBag.ordererorr = ordererorr;
         
            ViewBag.order = orders2;
            return View(); }

        [HttpPost]
        public IActionResult goodspage(  string bookname1)
        {
           
         
            var books = _context.webbookstore.ToList();
            var books1 = _context.webbookstore.ToList();
         
             books1.Clear();
            bool eror = true;

           


            foreach (webbookstore  webbookstore in books)
            {
                if (webbookstore.bookname == bookname1)
                {
                  
                    books1.Add(webbookstore);
                 
                }

              

            }
           

            ViewBag.book1 = books;
            ViewBag.book=books1;
            return View();
        }
        public IActionResult cancelorder(string ordercheck)
        {
            var orders = _context.orders.ToList();
         
            int order = Convert.ToInt32(ordercheck);
            bool eror = true;
            ViewBag.eror = eror;
            string bookname;
            foreach (orders order1 in orders)
            {
                if (order1.ordernumber == order)
                {
                    bookname= order1.bookname;
                    string cs = "Data Source=DESKTOP-OFJ100G\\SQLEXPRESS;Database=bookstore; User=sa; Password=123;";

                    SqlConnection con = new SqlConnection(cs);
                    con.Open();


                    SqlCommand cmd = new SqlCommand($"DELETE FROM orders WHERE ordernumber={order1.ordernumber}", con);
                    cmd.ExecuteNonQuery();


                    cmd = new SqlCommand($"UPDATE webbookstore SET stock =stock+1 WHERE bookname='{bookname}';", con);
                            cmd.ExecuteNonQuery();
                     
                     

                    con.Close();



                    eror = false;

                    ViewBag.eror = eror;
                    break;

                }
                else if (order1.ordernumber != order)
                {

                    eror = true;

                    ViewBag.eror = eror;
                }
            }
            string ordererorr = "убедитесь что вы парвильно ввели номер вашего заказа";
            ViewBag.ordererorr = ordererorr;

            return View();
        }
        public IActionResult search(string search) {
             List<webbookstore> Webbookstore = new List<webbookstore>();

        string cs = "Data Source=DESKTOP-OFJ100G\\SQLEXPRESS;Database=bookstore; User=sa; Password=123;";

            SqlConnection con = new SqlConnection(cs);
            con.Open();

            SqlCommand cmd = new SqlCommand($"SELECT * FROM webbookstore WHERE bookname LIKE'%{search}%' ", con);
            cmd.ExecuteNonQuery();
            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                byte[] imagedata = reader["imagename"] as byte[];
                Webbookstore.Add(new webbookstore
                {
                    Id = Convert.ToInt32(reader["id"]),
                    bookname = reader["bookname"].ToString(),
                    price = reader["price"].ToString(),
                    descrip = reader["descrip"].ToString(),
                    genre = reader["genre"].ToString(),
                   
                    stock = Convert.ToInt32(reader["stock"]),
                    imagename = imagedata
                });

            }
            reader.Close();
            ViewBag.webbookstore = Webbookstore;
            return View();
        }
    }
}