using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Domain.Entities
{
    public class Mock
    {

        public string FileName(string archiveFolder, string sourceFilePath)
        {

            DateTime d = DateTime.Now;
            string fileName = Path.GetFileName(sourceFilePath);
            int dotIndex = fileName.LastIndexOf('.');
            string fileExtension = (dotIndex < 0 ? string.Empty : fileName.Substring(dotIndex));
            string currentArchiveFilePath = archiveFolder + @"\" + fileName;
            string newArchiveFilePath = archiveFolder + @"\" + fileName.Replace(fileExtension,
                                                "_" + d.ToString().Replace("-", "_").Replace(" ", "_").Replace(":", "_") + fileExtension);

            return newArchiveFilePath;

        }

    }
}
