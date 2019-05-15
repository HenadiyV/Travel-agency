namespace TurOperator
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Hotel
    {
        [Key]
        public int Id_hotel { get; set; }

        public int id_contry { get; set; }

        public int id_city { get; set; }

        public string hotel1 { get; set; }

        public string category { get; set; }

        public string coment { get; set; }

        public string price { get; set; }

        public byte[] img { get; set; }

        public string linck { get; set; }

        public string photo { get; set; }
    }
}
