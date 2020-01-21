using ColorWebsite.Data.Entities;
using System.Data.Entity;
using System.Configuration;
using Microsoft.Azure.Mobile.Server.Tables;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace ColorWebsite.Data
{
    public class DataContext : DbContext
    {

        public DataContext() : base(ConfigurationManager.ConnectionStrings["PluralsightColorWebsite_db"].ConnectionString)
        {

        }
        public DbSet<DemoColor> Colors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(
                new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
                    "ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));
        }
    }
}
