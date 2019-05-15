namespace Tur_Admin
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("_User")]
    public partial class C_User
    {
        [Column("_UserId")]
        public int C_UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string fam { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }

        [Required]
        [StringLength(50)]
        public string surname { get; set; }

        [Required]
        [StringLength(20)]
        public string tel { get; set; }

        [StringLength(30)]
        public string pass { get; set; }
    }
}
