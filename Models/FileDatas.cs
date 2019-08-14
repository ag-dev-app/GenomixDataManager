using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenomixDataManager.Models
{
    public class FileDatas
    {
        public string JsonTitle { get; set; }
        public List<Data> JsonDatasList { get; set; }

        public FileDatas(string title)
        {
            JsonTitle = title;
            JsonDatasList = new List<Data>();
        }

        public void AddData (Data data)
        {
            JsonDatasList.Add(data);
        }

        public string ShowJsonDatas()
        {
            string txt = JsonTitle.ToUpper() + "\n\n";
            foreach (Data data in JsonDatasList)
            {
                txt += data.Key + " : " + data.Value + "\n";
            }
            return txt;
        }
    }
}
