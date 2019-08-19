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

        // GET api/translate
        // @Param : jsonFile=<fichier>
        // @Param : fields=table1[1].summary
        [Route("api/[controller]")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<JToken>> Get(Data data)
        {
            string jsonString = "";
            string response = "";

            Translate translator = new Translate();
            Stream stream = new MemoryStream(data.jsonFile.Buffer);
            StreamReader jsonReader = new StreamReader(stream);

            jsonString = await jsonReader.ReadToEndAsync();

            JsonFieldsCollector dataCollector = new JsonFieldsCollector(jsonString);

            string translatedTxt = "";

            foreach (var field in data.fields)
            {
                dataCollector.jsonObject[field] = translator.TranslateText(dataCollector.jsonObject[field].ToString());

                //dataCollector.SetTranslatedDictionnary(field, translatedTxt);


                //response += "Field " + field + " : " + translatedTxt + "\n\n";
                //@Todo : Ajouter un setter pour les fields du dictionnaire 
                response += "Field " + field + " : " + dataCollector.jsonObject[field];
            }

            return dataCollector.jsonObject;
        }

        // GET api/translate/fields
        [Route("api/[controller]/fields")]
        [HttpGet]
        public async Task<ActionResult<List<string>>> GetFields()
        {
            string jsonString = "";
            StreamReader bodyReader = new StreamReader(Request.Body);

            jsonString = await bodyReader.ReadToEndAsync();

            JsonFieldsCollector dataCollector = new JsonFieldsCollector(jsonString);
            return dataCollector.GetFieldsKeys();
        }
    }
}
