using System;
using Models.Tipos;

namespace Models
{
    public class Persona : Registro
    {
        public Persona()
        {
        }

        public string ID { get; set; }
        public char Tipo { get; set; }
        public string Descripcion { get; set; }
        public char statusID { get; set; }

        public TipoPersona TipoPersona { get; set; }
    }
}
