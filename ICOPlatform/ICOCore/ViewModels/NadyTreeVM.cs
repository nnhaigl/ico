using ICOCore.Entities.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOCore.ViewModels
{
    public class NadyTreeVM
    {
        public List<NadyTree> Trees { set; get; }
        public NadyTree PreviousLevel { set; get; }
    }
}
