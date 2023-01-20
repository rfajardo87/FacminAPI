using System;
using System.Linq;
using Models;
using System.Xml.Linq;
using System.Collections.Generic;
using System.IO;
using LiteDB;
using Hlp;

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

        public bool nuevasFacturas()
        {
            bool status = true;
            try
            {
                this.checkOrCreatePath(dest);
                foreach (string factura in this.getFacturaName())
                {
                    readXML(factura);
                    Directory.Move($"{pathFolder}{factura}", $"{dest}{factura}");
                }
            }
            catch (System.Exception ex)
            {
                status = false;
                Console.WriteLine(ex.Message);
            }
            return status;
        }

        private void readXML(string file)
        {
            try
            {
                XDocument doc = XDocument.Load(file);
                this.facturaRecord(doc);

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private void facturaRecord(XDocument node)
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
                string[] decimalStrings = new string[] { "subtotal", "iva", "total", "original" };
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

                using (LiteDatabase db = new LiteDatabase(this._fctx.getDB()))
                {
                    ILiteCollection<Factura> facturas = db.GetCollection<Factura>("Factura");

                    facturas.Insert(new Factura
                    {
                        UUID = valores.GetValueOrDefault("UUID"),
                        serie = valores.GetValueOrDefault("serie"),
                        folio = valores.GetValueOrDefault("folio"),
                        subtotal = decimalValues.GetValueOrDefault("subtotal"),
                        iva = decimalValues.GetValueOrDefault("iva"),
                        total = decimalValues.GetValueOrDefault("total"),
                        original = decimalValues.GetValueOrDefault("original"),
                        moneda = valores.GetValueOrDefault("moneda"),
                        metodo = valores.GetValueOrDefault("metodo"),
                        fechaPago = DateTime.Parse(valores.GetValueOrDefault("Fecha")),
                        forma = valores.GetValueOrDefault("FormaPago"),

                    });

                    ILiteCollection<Concepto> conceptos = db.GetCollection<Concepto>("Concepto");
                    IEnumerable<XElement> concepts = root.Descendants(ns + "Concepto");

                    foreach (XElement concepto in concepts)
                    {
                        decimalValues = new Dictionary<string, decimal>();
                        foreach (XAttribute att in concepto.Attributes())
                        {
                            valores.Add($"{att.Name}", att.Value);
                        }

                        conceptos.Insert(new Concepto
                        {
                            UUID = valores.GetValueOrDefault("UUID"),
                            ID = conceptos.Count() + 1,
                            concepto = concepto.Attribute("concepto").Value,
                            pu = decimalValues.GetValueOrDefault("ValorUnitario"),
                            cantidad = decimalValues.GetValueOrDefault("Cantidad"),
                            sub = decimalValues.GetValueOrDefault("Importe")
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


    }
}
