using Easysave.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easysave.Logger
{
    public class DailyLogger : Logger
    {
        public DailyLogger (string folderPath,string filename) : base(folderPath,filename)
        {
        }

        public DailyLogger(string folderPath) : base(folderPath)
        {
        }

        public void WriteDailyLog(BackupModel model,long fileSize,long fileTransferTime)
        {
            DailyData data = new DailyData(model.Name,model.SourceDirectory,model.DestinationDirectory,fileSize,fileTransferTime,DateTime.Now);
            using (StreamWriter sw = new StreamWriter(this.FilePath, true))
            {
                string JsonOutput = JsonConvert.SerializeObject(data);
                sw.WriteLine($"{JsonOutput}");
            }
        }


    }
}
