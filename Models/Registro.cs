using System;

namespace Models
{
    public class Registro
    {
        public Registro()
        {
            this.creado = DateTime.Now;
            this.actualizado = DateTime.Now;
        }

        public DateTime creado { get; set; }
        public DateTime actualizado { get; set; }
    }
}
