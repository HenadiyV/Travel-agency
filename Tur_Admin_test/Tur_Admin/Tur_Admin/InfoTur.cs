namespace Tur_Admin
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InfoTur")]
    public partial class InfoTur
    {
        public int InfoTurId { get; set; }

        [Column("_region")]
        [Required]
        [StringLength(50)]
        public string C_region { get; set; }

        [Column("_city")]
        [Required]
        [StringLength(50)]
        public string C_city { get; set; }

        [Column("_hotel")]
        [Required]
        [StringLength(50)]
        public string C_hotel { get; set; }

        [Column("_coment")]
        public string C_coment { get; set; }

        [Column("_img_hotel")]
        [StringLength(50)]
        public string C_img_hotel { get; set; }

        [Column("_img", TypeName = "image")]
        public byte[] C_img { get; set; }
    }
}
