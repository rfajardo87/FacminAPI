using System;

namespace Models
{
    public class Concepto : Registro
    {
        public long ID { get; set; }
        public string UUID { get; set; }
        public string concepto { get; set; }
        public decimal pu { get; set; }
        public decimal cantidad { get; set; }
        public decimal sub { get; set; }
    }
}
