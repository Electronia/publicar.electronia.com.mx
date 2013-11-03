using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace publicar.electronia.com.mx.Models
{
    public class Category
    {
        public string category_id { get; set; }
        public string category_name { get; set; } 
    }

    public class Item
    {
        public string itemId { get; set; }
        public string title { get; set; }
        public string categoryId { get; set; }
        //public int userId { get; set; } // podemos pedirlo dentro del un parametro de authenticacion
        public int state { get; set; }
        public decimal price { get; set; }
        public string typeCurrency { get; set; }
        public string status { get; set; }
        public string typeItem { get; set; }
        public DateTime dateRegistration { get; set; }
        public DateTime dateExpired { get; set; }
        public DateTime dateActivation { get; set; }
        public DateTime dateRenovation { get; set; }
        public string description { get; set; }
        public string usage { get; set; }
        public List<Foto> pictures { get; set; }

    }

    public class Foto
    {
        public int id { set; get; }
        public string item_id { set; get; }
        public int num_photo { set; get; }
        public string url_general { set; get; }
        public string path_general { set; get; }
    }
}