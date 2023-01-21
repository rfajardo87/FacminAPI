using System;
using System.Linq;
using Models;
using System.Xml.Linq;
using System.Collections.Generic;
using System.IO;
using LiteDB;
using Hlp;
using System.Threading.Tasks;

namespace App
{
    public class FacturaApp : Application
    {
        private const string root = "../Dts/";
        private const string pathFolder = $"{root}auto/";
        private const string dest = $"{root}facturas/";

        public FacturaApp() : base()
        {

        }

        public async Task<bool> nuevasFacturas()
        {
            bool status = true;
            try
            {
                this.checkOrCreatePath(dest);
                foreach (string factura in this.getFacturaName())
                {
                    await readXML(factura);
                    Directory.Move(factura, $"{dest}{factura.Split('/').Last()}");
                }
            }
            catch (System.Exception ex)
            {
                status = false;
                Console.WriteLine(ex.Message);
            }
            return status;
        }

        private async Task readXML(string file)
        {
            try
            {
                XDocument doc = XDocument.Load(file);
                await this.facturaRecord(doc);

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private async Task facturaRecord(XDocument node)
        {
            try
            {
                XNamespace ns = this._ctx.XmlNs.FirstOrDefault(x => x.statusID == 'A').valor;
                if (ns.Equals(XNamespace.None))
                {
                    throw new Exception("No existe el namespace");
                }

                const string main = "Comprobante";
                Dictionary<string, string> valores = new Dictionary<string, string>();
                Dictionary<string, decimal> decimalValues = new Dictionary<string, decimal>();
                string[] decimalStrings = new string[] { "SubTotal", "iva", "Total", "original" };
                string attKey = "";
                decimal swapDecimal = 0;
                IEnumerable<XElement> root = node.Descendants(ns + main);
                foreach (XElement subNode in root)
                {
                    foreach (XAttribute att in subNode.Attributes())
                    {
                        attKey = $"{att.Name}";
                        if (decimalStrings.ToList().Contains(attKey))
                        {
                            Decimal.TryParse(att.Value, out swapDecimal);
                            decimalValues.Add(attKey, swapDecimal);
                            continue;
                        }
                        valores.Add(attKey, att.Value);
                    }
                }

                Persona emisorFactura = await this.personaFactura("Emisor", root, ns);
                Persona receptorFactura = await this.personaFactura("Receptor", root, ns);

                #region UUID
                XNamespace tfd = "http://www.sat.gob.mx/TimbreFiscalDigital";
                IEnumerable<XElement> timbre = root.Descendants(tfd + "TimbreFiscalDigital");
                if (timbre.Count() != 1)
                {
                    throw new Exception("Error con el timbre del documento, verificar");
                }

                string UUID = timbre.FirstOrDefault().Attribute("UUID").Value;
                #endregion

                using (LiteDatabase db = new LiteDatabase(this._fctx.getDB()))
                {
                    ILiteCollection<Factura> facturas = db.GetCollection<Factura>("Factura");

                    facturas.Insert(new Factura
                    {
                        UUID = UUID,
                        Serie = valores.GetValueOrDefault("Serie"),
                        Folio = valores.GetValueOrDefault("Folio"),
                        SubTotal = decimalValues.GetValueOrDefault("SubTotal"),
                        iva = decimalValues.GetValueOrDefault("iva"),
                        Total = decimalValues.GetValueOrDefault("Total"),
                        original = decimalValues.GetValueOrDefault("original"),
                        moneda = valores.GetValueOrDefault("Moneda"),
                        metodo = valores.GetValueOrDefault("MetodoPago"),
                        fechaPago = DateTime.Parse(valores.GetValueOrDefault("Fecha")),
                        forma = valores.GetValueOrDefault("FormaPago"),
                        emisor = emisorFactura.ID,
                        receptor = receptorFactura.ID
                    });

                    ILiteCollection<Concepto> conceptos = db.GetCollection<Concepto>("Concepto");
                    IEnumerable<XElement> concepts = root.Descendants(ns + "Concepto");


                    foreach (XElement concepto in concepts)
                    {
                        valores = new Dictionary<string, string>();
                        foreach (XAttribute att in concepto.Attributes())
                        {
                            valores.Add($"{att.Name}", att.Value);
                        }

                        conceptos.Insert(new Concepto
                        {
                            UUID = UUID,
                            ClaveProdServ = valores.GetValueOrDefault("ClaveProdServ"),
                            Descripcion = valores.GetValueOrDefault("Descripcion"),
                            ValorUnitario = decimalValues.GetValueOrDefault("ValorUnitario"),
                            Cantidad = decimalValues.GetValueOrDefault("Cantidad"),
                            Importe = decimalValues.GetValueOrDefault("Importe")
                        });
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public Rsp<List<Factura>> getAll()
        {
            Rsp<List<Factura>> rsp = new Rsp<List<Factura>>();
            try
            {
                using (LiteDatabase db = new LiteDatabase(this._fctx.getDB()))
                {
                    List<Factura> facturas = db.GetCollection<Factura>("Factura").FindAll().ToList();
                    ILiteCollection<Concepto> conceptos = db.GetCollection<Concepto>();

                    foreach (Factura f in facturas)
                    {
                        f.conceptos.AddRange(conceptos.Query().Where(x => x.UUID == f.UUID).ToEnumerable());
                    }

                    rsp.data = facturas;
                }
                rsp.tipo = Tipo.Success;
            }
            catch (System.Exception ex)
            {
                rsp.data = null;
                rsp.tipo = Tipo.Fail;
                rsp.mensajes.Add(ex.Message);
            }
            return rsp;
        }

        private List<string> getFacturaName()
        {
            List<string> listaFacturas = new List<string>();

            try
            {
                if (!this.checkOrCreatePath(pathFolder))
                {
                    throw new Exception("Folder no encontrado");
                }

                listaFacturas = Directory.GetFiles(pathFolder).ToList();
            }
            catch (System.Exception)
            {

                throw;
            }

            return listaFacturas;
        }

        private bool checkOrCreatePath(string path)
        {
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
                return false;
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return false;
            }

            return true;
        }

        private async Task<Persona> personaFactura(string tipo, IEnumerable<XElement> root, XNamespace ns)
        {
            Persona persona = new Persona();
            IEnumerable<XElement> personasFactura = root.Descendants(ns + tipo);

            if (personasFactura.Count() != 1)
            {
                throw new Exception($"Hay más de un {tipo} en la factura, revisar archivo");
            }

            XElement personaFactura = personasFactura.FirstOrDefault();
            string rfc = personaFactura.Attribute("Rfc").Value;
            persona = this._ctx.Persona.FirstOrDefault(x => x.ID == rfc);

            if (persona == null)
            {
                persona = new Persona
                {
                    ID = rfc,
                    Nombre = personaFactura.Attribute("Nombre").Value,
                    Tipo = rfc.Count() < 13 ? 'M' : 'F',
                    statusID = 'A'
                };
                this._ctx.Persona.Add(persona);

                await this._ctx.SaveChangesAsync();
            }
            return persona;
        }
    }
}
