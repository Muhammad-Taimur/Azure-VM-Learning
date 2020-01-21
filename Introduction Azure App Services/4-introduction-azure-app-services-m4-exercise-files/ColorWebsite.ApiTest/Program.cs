using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorWebsite.ApiTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Color c = new Color(new ApiTestClient());
            var result = c.Get();
            

        }
    }
}
