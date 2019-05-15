namespace Tur_Admin
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("City")]
    public partial class City
    {
        [Key]
        public int Id_city { get; set; }

        public int id_contry { get; set; }

        [Column("city")]
        [StringLength(50)]
        public string city1 { get; set; }
    }
}
