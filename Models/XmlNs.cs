using System;

namespace Models
{
    public class XmlNs : Registro
    {
        public long id { get; set; }
        public string ns { get; set; }
        public string valor { get; set; }
        public char statusID { get; set; }
    }
}
