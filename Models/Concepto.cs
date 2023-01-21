using System;

namespace Models
{
    public class Concepto : Registro
    {
        public string UUID { get; set; }
        public string ClaveProdServ { get; set; }
        public string Descripcion { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Importe { get; set; }
    }
}
