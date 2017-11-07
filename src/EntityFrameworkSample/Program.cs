using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkSample
{
    public class Phone
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public int Price { get; set; }
    }

    public class MobileContext : DbContext
    {
        public MobileContext() : base("DefaultConnection")
        {

        }
        public DbSet<Phone> Phones { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var db = new MobileContext();
            db.Phones.Load(); // загружаем данные
            var list = db.Phones.Local.ToBindingList();
            db.Phones.Add(new Phone()
            {
                Company = "Siemence",
                Price = 15000,
                Title = "Siemence XF100"
            });
        }
    }
}
