using System;
using System.Collections.Generic;

namespace Models
{
    public class Factura : Registro
    {
        public Factura() : base()
        {
            this.conceptos = new List<Concepto>();
        }

        public string UUID { get; set; }
        public string folio { get; set; }
        public string serie { get; set; }
        public string emisor { get; set; }
        public string receptor { get; set; }
        public DateTime fechaTimbrado { get; set; }
        public string statusID { get; set; }
        public string UUIDRelacionado { get; set; }
        public string residencia { get; set; }
        public decimal subtotal { get; set; }
        public decimal iva { get; set; }
        public decimal total { get; set; }
        public decimal original { get; set; }
        public string moneda { get; set; }
        public string forma { get; set; }
        public string metodo { get; set; }
        public DateTime fechaPago { get; set; }

        public List<Concepto> conceptos { get; set; }
    }
}
