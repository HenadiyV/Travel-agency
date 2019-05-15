namespace TurOperator
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class usery
    {
        public int Id { get; set; }

        public string fam { get; set; }

        public string name { get; set; }

        public string surname { get; set; }

        public string tel { get; set; }

        public string pass { get; set; }

        public string emaill { get; set; }
    }
}
