using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;

namespace Jwell.ServiceManage.Repository.Context
{
    public class JwellKpiDbContext: DbContext
    {
        public JwellKpiDbContext() : base("JwellKpi")
        {

        }

        public JwellKpiDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        protected JwellKpiDbContext(DbCompiledModel model) : base(model)
        {
        }


        #region 配置信息
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<DecimalPropertyConvention>();
            modelBuilder.Conventions.Add(new DecimalPropertyConvention(18, 4));
            modelBuilder.HasDefaultSchema("jwell");
            base.OnModelCreating(modelBuilder);
            Dasebase databaseType = GetDatabaseType();
            SetDefaultSchema(databaseType, modelBuilder);

        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                //var sb = new StringBuilder();
                //foreach (var error in ex.EntityValidationErrors)
                //{
                //    foreach (var item in error.ValidationErrors)
                //    {
                //        sb.AppendLine(item.PropertyName + ": " + item.ErrorMessage);
                //    }
                //}
                // ReSharper disable once PossibleIntendedRethrow
                throw ex;
            }
        }


        // ReSharper disable once UnusedMember.Local
        private void InitializeContext()
        {
            Configuration.UseDatabaseNullSemantics = true;
            Configuration.ValidateOnSaveEnabled = false;
        }

        protected virtual void SetDefaultSchema(Dasebase databaseType, DbModelBuilder modelBuilder)
        {
            switch (databaseType)
            {
                case Dasebase.SqlServer:
                    modelBuilder.HasDefaultSchema("dbo");
                    break;
                case Dasebase.Oracle:
                    {
                        var text = Database.Connection.ConnectionString.Split(new[] { ";" },StringSplitOptions.RemoveEmptyEntries)
                            .FirstOrDefault(p => p.Trim().StartsWith("User Id", StringComparison.CurrentCultureIgnoreCase));
                        if (!string.IsNullOrWhiteSpace(text))
                        {
                            var schema = text.ToUpper().Replace("USER ID", string.Empty).Replace("=", string.Empty)
                                .Trim();
                            modelBuilder.HasDefaultSchema(schema);
                        }
                        break;
                    }
            }
        }

        protected virtual Dasebase GetDatabaseType()
        {
            var name = Database.Connection.GetType().Name;
            if (name != "SqlConnection")
            {
                if (name == "OracleConnection")
                {
                    return Dasebase.Oracle;
                }
            }
            return Dasebase.SqlServer;
        }
        #endregion
    }
}
