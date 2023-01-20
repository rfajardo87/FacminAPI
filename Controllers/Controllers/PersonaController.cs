using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Rsp;
using Hlp;
using App;

namespace Controllers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonaController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<PersonaController> _logger;
        private PersonaApp _persona;

        public PersonaController(ILogger<PersonaController> logger)
        {
            _logger = logger;
            this._persona = new PersonaApp();
        }

        [HttpGet]
        public IActionResult Get()
        {
            Rsp<List<PersonaRsp>> rsp = new Rsp<List<PersonaRsp>>();

            try
            {
                rsp = this._persona.getAll();
            }
            catch (Exception ex)
            {
                rsp.data = null;
                rsp.mensajes.Add(ex.Message);
                rsp.tipo = Tipo.Fail;
            }

            return Ok(rsp);
        }
    }
}
