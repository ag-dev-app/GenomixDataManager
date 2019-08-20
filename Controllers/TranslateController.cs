using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GenomixDataManager.Models;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace GenomixDataManager.Controllers
{
    [ApiController]
    public class TranslateController : ControllerBase
    {
        // @TODO : Ajouter les membres privés/publics de la classe
        // ==> JSON
        private string JSONinput;
        // ==> JSON Config
        private string JSONField;
        // @TODO : Ajouter constructeur

        public JObject jsonObj { get; set; }
        public JsonFieldsCollector dataColl { get; set; }

        // GET api/translate
        // @Param : jsonFile=<fichier>
        // @Param : fields=table1[1].summary
        [Route("api/[controller]")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<JToken>> Get(Data data)
        {
            string jsonString = "";

            Translate translator = new Translate();
            Stream stream = new MemoryStream(data.jsonFile.Buffer);
            StreamReader jsonReader = new StreamReader(stream);

            jsonString = await jsonReader.ReadToEndAsync();

            JsonFieldsCollector dataCollector = new JsonFieldsCollector(jsonString);

            foreach (var field in data.fields)
            {

                dataCollector.jsonObject[field] = translator.TranslateText(dataCollector.jsonObject[field].ToString());

            }
            jsonObj = dataCollector.jsonObject;
            dataColl = dataCollector;
            return jsonObj;
        }

        // GET api/translate/fields
        [Route("api/[controller]/fields")]
        [HttpGet]
        public ActionResult<List<string>> GetFields()
        {
            return dataColl.GetFieldsKeys();
        }

    }
}
