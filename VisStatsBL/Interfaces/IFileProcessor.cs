using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisStatsBL.Interfaces
{
    public interface IFileProcessor
    {
        List<string> LeesSoorten(string fileName);
        List<string> LeesHavens(string fileName);
    }
}
