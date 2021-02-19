using Medical.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Side.Models
{
    public class DoctorContext:DbContext
    {
        public DoctorContext(DbContextOptions<DoctorContext> options) : base(options)
        {

        }

        public DbSet<State> STATETB { get; set; }

        public DbSet<City> CITYTB { get; set; }

        public DbSet<Payment> PAYMENTTB { get; set; }

        public DbSet<Category> CATEGORYTB { get; set; }

        public DbSet<DocClinic> CLINICTB { get; set; }

        public DbSet<DoctorReg> DOCTORTB { get; set; }

        public DbSet<Patient> PATIENTTB { get; set; }

        public DbSet<DocReview> REVIEWTB { get; set; }
        public DbSet<DocAppointment> APPOINTMENTTB { get; set; }


    }
}
