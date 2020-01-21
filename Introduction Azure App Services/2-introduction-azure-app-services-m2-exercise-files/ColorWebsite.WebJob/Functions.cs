using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using ColorWebsite.Data.Services;

namespace ColorWebsite.WebJob
{
    public class Functions
    {
        [NoAutomaticTrigger]
        public static void ChangeColors()
        {
            ColorService service = new ColorService();

            service.DeleteColors();

            service.InsertRandomColors();
        }
    }
}
