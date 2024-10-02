using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL.Exceptions;
using VisStatsBL.Interfaces;
using VisStatsBL.Model;

namespace VisStatsBL.Managers
{
    public class VisStatsManager
    {
        private IFileProcessor fileProcessor;
        private IVisStatsRepository visStatsRepository;

        public VisStatsManager(IFileProcessor fileProcessor, IVisStatsRepository visStatsRepository)
        {
            this.fileProcessor = fileProcessor;
            this.visStatsRepository = visStatsRepository;
        }
        public void UploadVissoorten(string fileName)
        {
            try
            {
                List<string> soorten = fileProcessor.LeesSoorten(fileName);
                List<Vissoort> vissoorten = MaakVissoorten(soorten);
                foreach (Vissoort vis in vissoorten)
                {
                    if (!visStatsRepository.HeeftVissoort(vis)) // Dit moet nu werken
                    {
                        visStatsRepository.SchrijfSoort(vis);
                    }
                }
            }
            catch (Exception ex)
            {
                // Foutafhandeling
            }
        }

        private List<Vissoort> MaakVissoorten(List<string> soorten)
        {
            Dictionary<string, Vissoort> visSoorten = new();
            foreach (string soort in soorten)
            {
                if (!visSoorten.ContainsKey(soort))
                {
                    try
                    {
                        visSoorten.Add(soort, new Vissoort(soort));
                    }
                    catch (DomeinException)
                    {

                    }
                }
            }
            return visSoorten.Values.ToList();
        }

        public void UploadHavens(string fileName)
        {
            try
            {
                List<string> haven = fileProcessor.LeesHavens(fileName);
                List<Haven> havens = MaakHavens(haven);
                foreach (Haven h in havens)
                {
                    if (!visStatsRepository.HeeftHaven(h)) // Dit moet nu werken
                    {
                        visStatsRepository.SchrijfHaven(h);
                    }
                }
            }
            catch (Exception ex)
            {
                // Foutafhandeling
            }
        }

        private List<Haven> MaakHavens(List<string> haven)
        {
            Dictionary<string, Haven> Haven = new();
            foreach (string h in haven)
            {
                if (!Haven.ContainsKey(h))
                {
                    try
                    {
                        Haven.Add(h, new Haven(h));
                    }
                    catch (DomeinException)
                    {

                    }
                }
            }
            return Haven.Values.ToList();
        }
    }
}