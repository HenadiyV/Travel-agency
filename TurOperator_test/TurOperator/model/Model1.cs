namespace TurOperator
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1(string conct)
            : base(conct)
        {
        }

        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Contry> Contries { get; set; }
        public virtual DbSet<Hotel> Hotels { get; set; }
        public virtual DbSet<Tur> Turs { get; set; }
        public virtual DbSet<usery> useries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
