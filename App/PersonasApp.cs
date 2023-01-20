using System;
using Models;
using Models.Rsp;
using Hlp;
using System.Collections.Generic;
using System.Linq;


namespace App
{
    public class PersonaApp : Application
    {

        public PersonaApp() : base() { }

        public Rsp<List<PersonaRsp>> getAll()
        {
            Rsp<List<PersonaRsp>> rsp = new Rsp<List<PersonaRsp>>();
            try
            {
                rsp.data = (from p in this._ctx.Persona
                            join t in this._ctx.TipoPersona on p.Tipo equals t.ID
                            select new PersonaRsp
                            {
                                ID = p.ID,
                                statusID = p.statusID,
                                Tipo = p.Tipo,
                                Descripcion = p.Descripcion,
                                creado = p.creado,
                                actualizado = p.actualizado,
                                tipoDescripcion = t.Tipo
                            }).ToList();
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
