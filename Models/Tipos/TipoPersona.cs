using System;
using Models;
using System.Collections.Generic;

namespace Models.Tipos
{
    public class TipoPersona : Registro
    {
        public TipoPersona()
        {
            this.Personas = new List<Persona>();
        }

        public char ID { get; set; }
        public string Tipo { get; set; }
        public char statusId { get; set; }

        public virtual List<Persona> Personas { get; set; }
    }
}
