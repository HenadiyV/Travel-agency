namespace TurOperator
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Contry")]
    public partial class Contry
    {
        [Key]
        public int Id_contry { get; set; }

        [Column("contry")]
        [Required]
        [StringLength(50)]
        public string contry1 { get; set; }
    }
}
