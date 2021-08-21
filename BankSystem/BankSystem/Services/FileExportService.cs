using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Services
{
    class FileExportService
    {
        string propertyInfo = "../../../Resources/PropertyInfo.txt";
        public void PropertyExport<T>(T obj)
        {
            var myType = obj.GetType();
            var properties = myType.GetProperties();

            string clientInfo = "";

            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(obj);

                clientInfo += $"{propertyName}\t{propertyValue}\n";
            }

            using (StreamWriter streamWriter = new StreamWriter(propertyInfo, false))
            {
                streamWriter.Write(clientInfo);
            }
        }
    }
}
