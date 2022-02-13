using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiketing
{
    [Serializable]
    public class Tiket
    {
        public long id { get; set; }

        public string tiketNr { get; set; }
        public Programmer programmer { get; set; }
        public Project project { get; set; }

        public Tiket(string tiketNr, Project project, Programmer programmer)
        {
            this.tiketNr = tiketNr;
            this.programmer = programmer;
            this.project = project;
        }

        public Tiket(long id, string tiketNr, Programmer programmer, Project project)
        {
            this.id = id;
            this.tiketNr = tiketNr;
            this.programmer = programmer;
            this.project = project;
        }


    }
}
