namespace Tur_Admin
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Hotel")]
    public partial class Hotel
    {
        [Key]
        public int Id_hotel { get; set; }

        public int id_contry { get; set; }

        public int id_city { get; set; }

        [Column("hotel")]
        [Required]
        public string hotel1 { get; set; }

        [StringLength(50)]
        public string category { get; set; }

        public string coment { get; set; }

        [StringLength(50)]
        public string price { get; set; }

        [Column(TypeName = "image")]
        public byte[] img { get; set; }

        public string linck { get; set; }
    }
}
