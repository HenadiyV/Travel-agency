namespace Tur_Admin
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Tur")]
    public partial class Tur
    {
        public int Id { get; set; }

        public int id_user { get; set; }

        public int id_contry { get; set; }

        public int id_city { get; set; }

        public int id_hotel { get; set; }

        public DateTime? dateStart { get; set; }

        public DateTime? dateEnd { get; set; }

        [Column(TypeName = "image")]
        public byte[] img { get; set; }

        public DateTime? reg_data { get; set; }
    }
}
