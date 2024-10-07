using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL.Model;

namespace VisStatsBL.Interfaces
{
    public interface IVisStatsRepository
    {
        bool HeeftVissoort(Vissoort vis);
        void SchrijfSoort(Vissoort vis);
        bool HeeftHaven(Haven haven);
        void SchrijfHaven(Haven haven);
        List<Haven> LeesHavens();
        List<Vissoort> LeesVissoorten();
        bool IsOpgeladen(string fileName);
        void SchrijfStatistieken(List<VisStatsDataRecord> data, string fileName);

    }
}
