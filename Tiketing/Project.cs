using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiketing
{
    [Serializable]
   public class Project

    {
        public string name { get; set; }
        public string error { get; set; }

        public Project(string name, string error)
        {
            this.name = name;
            this.error = error;
        }
    }
}
