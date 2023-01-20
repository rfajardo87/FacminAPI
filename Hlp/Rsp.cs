using System;
using System.Collections.Generic;

namespace Hlp
{
    public class Rsp<T>
    {
        public Rsp()
        {
            this.mensajes = new List<string>();
        }

        public T data { get; set; }
        public List<string> mensajes { get; set; }
        public Tipo tipo { get; set; }
    }
}
