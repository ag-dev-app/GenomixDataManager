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

        public readonly Dictionary<string, JValue> fields;
        public Dictionary<string, JValue> translatedFields;

        public dynamic jsonObject;

        private List<string> fieldsKeys;


        public JsonFieldsCollector(string content)
        {
            //string JsonString = Deserialize(content).ToString();

            var json = JToken.Parse(content);
            
            // initialisation du dictionnaire de données (fields)
            fields = new Dictionary<string, JValue>();

            // initialisation du dictionnaire de données (translatedFields)
            translatedFields = new Dictionary<string, JValue>();

            // initialisation liste de keys fieldKays
            fieldsKeys = new List<string>();

            // "remplissage" de fields
            CollectFields(json);

            // remplissage fieldsKeys
            foreach (var field in fields)
                fieldsKeys.Add($"{field.Key}");

            translatedFields = fields;

            jsonObject = JToken.Parse(content);
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
                    txt += field.Value + "\n\n";
                }
            }
            return txt;
        }

        public string GetValueIfKeyContains(string containedTxt1, string containedTxt2)
        {
            string txt = "";
            foreach (var field in fields)
            {
                if (field.Key.Contains(containedTxt1) && field.Key.Contains(containedTxt2))
                {
                    txt += field.Value + "\n\n";
                }
            }
            return txt;
        }

        public Dictionary<string,JValue> SetTranslatedDictionnary(string key, string value)
        {
            translatedFields[key].Value = value;
            return translatedFields;
        }
    }
}
