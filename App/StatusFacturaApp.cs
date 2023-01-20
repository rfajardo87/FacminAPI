using System;
using Models;
using Hlp;
using System.Collections.Generic;
using System.Linq;

namespace App
{
    public class StatusFacturaApp : Application
    {
        public StatusFacturaApp() : base() { }

        public Rsp<List<StatusFactura>> getAll()
        {
            Rsp<List<StatusFactura>> rsp = new Rsp<List<StatusFactura>>();
            try
            {
                rsp.data = this._ctx.StatusFactura.ToList();
                rsp.tipo = Tipo.Success;
            }
            catch (Exception ex)
            {
                rsp.data = null;
                rsp.tipo = Tipo.Fail;
                rsp.mensajes.Add($"{ex.Message}");
            }
            return rsp;
        }
    }
}
