using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiketing
{
    [Serializable]
   public class Programmer
    {
        public string name { get; set; }
        public string language { get; set; }

        public Programmer(string name, string language)
        {
            this.name = name;
            this.language = language;
        }
    }
}
