namespace ColorWebsite.Data.Migrations
{

    using System.Data.Entity.Migrations;
    using Entities;
    using Microsoft.Azure.Mobile.Server.Tables;
    internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "ColorWebsite.Data.DataContext";

            SetSqlGenerator("System.Data.SqlClient", new EntityTableSqlGenerator());
        }

        protected override void Seed(ColorWebsite.Data.DataContext context)
        {
            //  This method will be called after migrating to the latest version.

            context.Colors.AddOrUpdate(
              p => p.Name,
              new DemoColor { Name = "Orange" },
              new DemoColor { Name = "Blue" },
              new DemoColor { Name = "DarkGray" }
            );
            
        }
    }
}
