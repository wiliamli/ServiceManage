using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Jwell.ServiceManage.Domain.Entities.ServiceMgr;

namespace Jwell.ServiceManage.Repository.Context
{
    public class JwellDbContext: DbContext
    {       
        public DbSet<Service> Services { get; set; }

        public DbSet<ServiceVersion> ServiceVersions { get; set; }

        public DbSet<ServiceAuthority> ServiceAuthorities { get; set; }

        public DbSet<ServiceDelegate> ServiceDelegates { get; set; }

        public DbSet<ServiceClassfication> ServiceClassfications { get; set; }

        public DbSet<ServiceInvokeRecord> ServiceInvokeRecords { get; set; }

        public DbSet<VersionHistory> VersionHistories { get; set; }

        public JwellDbContext(): base("Default")
        {

        }

        public JwellDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        protected JwellDbContext(DbCompiledModel model) : base(model)
        {
        }      

        #region 配置信息
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<DecimalPropertyConvention>();
            modelBuilder.Conventions.Add(new DecimalPropertyConvention(18, 4));
            modelBuilder.HasDefaultSchema("JWELL_ARCHI");

            //定义Service和ServiceVersion之间的一对多关系
            //注：You can not have a foreign key property in a one-to-one relationship, unless it is also the primary key property.
            //This is essentially because EF6 doesn't support alternate keys/unique indexes, so you can't enforce that a non-primary key property is unique.
            //modelBuilder.Entity<ServiceVersion>()
            //    .HasRequired(v => v.Service)
            //    .WithMany(s => s.Versions)
            //    .HasForeignKey(v => v.No);

            //定义级联删除
            //modelBuilder.Entity<Service>()
            //    .HasMany(s => s.Versions)
            //    .WithRequired(v => v.Service)
            //    .WillCascadeOnDelete();

            SetDefaultSchema(GetDatabaseType(), modelBuilder);
        }
      
        //private void InitializeContext()
        //{
        //    Configuration.UseDatabaseNullSemantics = true;
        //    Configuration.ValidateOnSaveEnabled = false;
        //}

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
                            var schema = text.ToUpper().Replace("USER ID", string.Empty).Replace("=", string.Empty).Trim();
                            modelBuilder.HasDefaultSchema(schema);
                        }
                        break;
                    }
            }
        }

        protected virtual Dasebase GetDatabaseType()
        {
            var name = Database.Connection.GetType().Name;
            if (name == "SqlConnection") return Dasebase.SqlServer;
            return name == "OracleConnection" ? Dasebase.Oracle : Dasebase.SqlServer;
        }
        #endregion
    }
}
