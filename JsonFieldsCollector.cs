using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GenomixDataManager
{
    public class JsonFieldsCollector
    {
        //public string path = @"C:\Users\rcmd\Desktop\Aternum DEV\json\ag-SB20879-file all.json";


        private readonly Dictionary<string, JValue> fields;
        private List<string> fieldsKeys;


        public JsonFieldsCollector(string path)
        {
            StreamReader stream = new StreamReader(path);
            string JsonString = stream.ReadToEnd();
            var json = JToken.Parse(JsonString);

            // création du dictionnaire de données (fields)
            fields = new Dictionary<string, JValue>();
            // création d'une liste répertoriant toutes les clés (fieldKeys)
            fieldsKeys = new List<string>();

            // "remplissage" de fields
            CollectFields(json);

            // "remplissage" de fieldsKeys
            foreach (var field in fields)
                fieldsKeys.Add($"{field.Key}");
        }

        private void CollectFields(JToken jToken)
        {
            switch (jToken.Type)
            {
                case JTokenType.Object:
                    foreach (var child in jToken.Children<JProperty>())
                        CollectFields(child);
                    break;
                case JTokenType.Array:
                    foreach (var child in jToken.Children())
                        CollectFields(child);
                    break;
                case JTokenType.Property:
                    CollectFields(((JProperty)jToken).Value);
                    break;
                default:
                    fields.Add(jToken.Path, (JValue)jToken);
                    break;
            }
        }

        public IEnumerable<KeyValuePair<string, JValue>> GetAllFields() => fields;


        public List<string> GetFieldsKeys()
        {
            return fieldsKeys;
        }

        public string GetValue(string key)
        {
            return fields[key].ToString();
        }

        public string GetValueIfKeyContains(string containedTxt)
        {
            string txt = "";
            foreach (var field in fields)
            {
                if (field.Key.Contains(containedTxt))
                {
                    txt += field.Key + " :\n" + field.Value + "\n";
                }
            }
            return txt;
        }
    }
}
