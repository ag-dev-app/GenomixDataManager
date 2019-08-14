using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenomixDataManager.Models
{
    public class AllFilesDatas
    {
        public List<FileDatas> AllFilesDatasList { get; set; }

        public AllFilesDatas()
        {
            AllFilesDatasList = new List<FileDatas>();
        }

        public void AddFileDatas (FileDatas fileDatas)
        {
            AllFilesDatasList.Add(fileDatas);
        }

        public string ShowAllFilesDatas()
        {
            string txt = "";
            foreach (FileDatas datas in AllFilesDatasList)
            {
                txt += datas.ShowJsonDatas() + "\n\n";
            }
            return txt;
        }
    }
}
