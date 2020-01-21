using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ColorWebsite.Api.Models
{
    public class DataContext : DbContext
    {

        public DataContext() : base(ConfigurationManager.ConnectionStrings["PluralsightColorWebsite_db"].ConnectionString)
        {

        }
        public DbSet<DemoColor> Colors { get; set; }

    }
}