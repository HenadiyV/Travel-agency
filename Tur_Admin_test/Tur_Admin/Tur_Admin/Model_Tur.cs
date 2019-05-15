namespace Tur_Admin
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model_Tur : DbContext
    {
        public Model_Tur()
            : base("name=Model_Tur")
        {
        }

        public virtual DbSet<C_User> C_User { get; set; }
        public virtual DbSet<InfoTur> InfoTur { get; set; }
        public virtual DbSet<Table> Table { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
