using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Medical.Models;

namespace Medical.Models
{
    public class MedContext : DbContext
    {
        public MedContext(DbContextOptions<MedContext> options) : base(options)
        {

        }

        public DbSet<State> STATETB { get; set; }

        public DbSet<City> CITYTB { get; set; }

        public DbSet<Payment> PAYMENTTB { get; set; }

        public DbSet<Category> CATEGORYTB { get; set; }

        public  DbSet<Medicine> MEDICINETB { get; set; }
        public DbSet<Medicine_Image> MEDICINE_IMAGETB { get; set; }

        public DbSet<Clinic> CLINICTB { get; set; }

        public DbSet<Doctor> DOCTORTB  { get; set; }

        public DbSet<Patient> PATIENTTB { get; set; }

        public DbSet<Review> REVIEWTB { get; set; }
        public DbSet<ProductReview> PRODUCTREVIEWTB { get; set; }

        public DbSet<Order> ORDERTB { get; set; }
        public DbSet<OrderMedical> ORDER_MEDICALTB { get; set; }

        public ViewModel ViewModel { get; set; }

        //public DbSet<Order> ORDER_MEDICALTB { get; set; }

        public DbSet<Login> ADMINTB { get; set; }
        public DbSet<Cart> CARTTB { get; set; }
        public DbSet<Bill> BILLTB { get; set; }

        public DbSet<Appointment> APPOINTMENTTB { get; set; }
        public DbSet<Career> CAREERTB { get; set; }

       // public DbSet<Medical.Models.Medicine_Image> Medicine_Images { get; set; }

        
    }
}
