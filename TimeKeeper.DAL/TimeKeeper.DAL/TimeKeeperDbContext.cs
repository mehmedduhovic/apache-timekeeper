using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeKeeper.DAL.Entities;

namespace TimeKeeper.DAL
{
    public class TimeKeeperDbContext : DbContext
    {
        public TimeKeeperDbContext() : base("name=TimeKeeper")
        {
            if (Database.Connection.Database == "Testera")
            {
                Database.SetInitializer(new TimeKeeperDbInitializer<TimeKeeperDbContext>());
            }
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Engagement> Engagements { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Entities.Task> Tasks { get; set; }
        public DbSet<Team> Teams { get; set; }
        public IQueryable<Customer> Customer { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<BaseClass<int>>();
            modelBuilder.Ignore<BaseClass<string>>();
            modelBuilder.Entity<Customer>().Map<Customer>(x => { x.Requires("Deleted").HasValue(false); }).Ignore(x => x.Deleted);
            modelBuilder.Entity<Day>().Map<Day>(x => { x.Requires("Deleted").HasValue(false); }).Ignore(x => x.Deleted);
            modelBuilder.Entity<Employee>().Map<Employee>(x => { x.Requires("Deleted").HasValue(false); }).Ignore(x => x.Deleted);
            modelBuilder.Entity<Engagement>().Map<Engagement>(x => { x.Requires("Deleted").HasValue(false); }).Ignore(x => x.Deleted);
            modelBuilder.Entity<Project>().Map<Project>(x => { x.Requires("Deleted").HasValue(false); }).Ignore(x => x.Deleted);
            modelBuilder.Entity<Role>().Map<Role>(x => { x.Requires("Deleted").HasValue(false); }).Ignore(x => x.Deleted);
            modelBuilder.Entity<Entities.Task>().Map<Entities.Task>(x => { x.Requires("Deleted").HasValue(false); }).Ignore(x => x.Deleted);
            modelBuilder.Entity<Team>().Map<Team>(x => { x.Requires("Deleted").HasValue(false); }).Ignore(x => x.Deleted);

            modelBuilder.Entity<Entities.Task>().Property(p => p.Hours).HasPrecision(4, 2);
            modelBuilder.Entity<Role>().Property(p => p.Hrate).HasPrecision(5, 2);
            modelBuilder.Entity<Role>().Property(p => p.Mrate).HasPrecision(8, 2);
            modelBuilder.Entity<Project>().Property(p => p.Amount).HasPrecision(10, 2);
            modelBuilder.Entity<Engagement>().Property(p => p.Hours).HasPrecision(4, 2);
            modelBuilder.Entity<Employee>().Property(p => p.Salary).HasPrecision(8, 2);
            modelBuilder.Entity<Day>().Property(p => p.Hours).HasPrecision(4, 2);


        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries().Where(p => p.State == EntityState.Deleted)) SoftDelete(entry);
            return base.SaveChanges();
        }

        public void SoftDelete(DbEntityEntry entry)
        {
            Type entryEntityType = entry.Entity.GetType();
            string tableName = GetTableName(entryEntityType);
            string sql = String.Format("UPDATE {0} SET Deleted = 1 WHERE id=@id", tableName);
            Database.ExecuteSqlCommand(sql, new SqlParameter("@id", entry.OriginalValues["Id"]));
            //u drugokm slucaju entry.State = EntityState.Detached;
            entry.State = EntityState.Unchanged;
        }

        private static Dictionary<Type, EntitySetBase> _mappingCache = new Dictionary<Type, EntitySetBase>();

        private string GetTableName(Type type)
        {
            EntitySetBase es = GetEntitySet(type);
            return string.Format("[{0}].[{1}]", es.MetadataProperties["Schema"].Value,
                es.MetadataProperties["Table"].Value);
        }

        private EntitySetBase GetEntitySet(Type type)
        {
            if (!_mappingCache.ContainsKey(type))
            {
                ObjectContext cctx = ((IObjectContextAdapter)this).ObjectContext;
                string typeName = ObjectContext.GetObjectType(type).Name;
                var es = cctx.MetadataWorkspace.GetItemCollection(DataSpace.SSpace).GetItems<EntityContainer>().
                    SelectMany(c => c.BaseEntitySets.Where(e => e.Name == typeName)).FirstOrDefault();
                if (es == null)
                    throw new ArgumentException("Entity type not found in GetTableName", typeName);
                _mappingCache.Add(type, es);
            }

            return _mappingCache[type];
        }
    }
}
