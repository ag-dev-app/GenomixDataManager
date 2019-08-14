using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenomixDataManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranslateController : ControllerBase
    {
        // @TODO : Ajouter les membres privés/publics de la classe
        // ==> JSON
        private string JSONinput;
        // ==> JSON Config
        private string JSONField;
        // @TODO : Ajouter constructeur

        // GET api/translate?field=table1[1].summary
        [HttpGet]
        public ActionResult<string> Get([FromQuery(Name = "field")] string field)
        {
            return Post();
            //return new string[] { field, "Le champ demandé est " + field };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/translate
        [HttpPost]
        // Pas besoin du consume : par défaut c'est du application/json
        public ActionResult<string> Post()
        {
            string path = @"C:\Users\rcmd\Desktop\Aternum DEV\json\ag-SB20879-file all.json";

            //// le plus volumineux des JSON, dans lequel on n'a aucune trad a faire
            //string path = @"C:\Users\rcmd\Desktop\Aternum DEV\json\ag-SB20879-file carrier.json"; 

            JsonFieldsCollector dataCollector = new JsonFieldsCollector(path);

            //string response = dataCollector.GetValue("table1[2].summary");
            //string response = dataCollect.GetValue("table2[2497].trait");
            string response = dataCollector.GetValueIfKeyContains("table1","summary");
            //// envoi de la réponse reçue à l'API de traduction Google Cloud Translator

            //Translate translator = new Translate();
            //string translatedResponse = translator.TranslateText(response);

            //return response + "\r\n\r\n" + translatedResponse;

            return response;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
