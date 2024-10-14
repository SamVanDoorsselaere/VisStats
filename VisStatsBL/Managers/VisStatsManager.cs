using System;
using System.Collections;
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
        private IVisStatsRepository VisStatsRepository;

        public VisStatsManager(IFileProcessor fileProcessor, IVisStatsRepository visStatsRepository)
        {
            this.fileProcessor = fileProcessor;
            this.VisStatsRepository = visStatsRepository;
        }
        public void UploadVissoorten(string fileName)
        {
            try
            {
                List<string> soorten = fileProcessor.LeesSoorten(fileName);
                List<Vissoort> vissoorten = MaakVissoorten(soorten);
                foreach (Vissoort vis in vissoorten)
                {
                    if (!VisStatsRepository.HeeftVissoort(vis)) // Dit moet nu werken
                    {
                        VisStatsRepository.SchrijfSoort(vis);
                    }
                }
            }
            catch (ManagerException ex)
            {
                throw new ManagerException("UploadVissoorten", ex);
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
                    if (!VisStatsRepository.HeeftHaven(h)) // Dit moet nu werken
                    {
                        VisStatsRepository.SchrijfHaven(h);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ManagerException("UploadHavens", ex);
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
                    catch (ManagerException)
                    {

                    }
                }
            }
            return Haven.Values.ToList();
        }

        public void UploadStatistieken(string fileName)
        {
            try
            {
                // is reed opgeladen
                if (!VisStatsRepository.IsOpgeladen(fileName))
                {
                    //lezen gegevens
                    List<Vissoort> soorten = VisStatsRepository.LeesVissoorten();
                    List<Haven> havens = VisStatsRepository.LeesHavens();
                    //schrijf naar DB
                    var data = fileProcessor.LeesStatistieken(fileName, soorten, havens);
                    VisStatsRepository.SchrijfStatistieken(data, fileName);
                }
                
            }
            catch (Exception ex)
            {
                throw new ManagerException("UploadStatistieken", ex);
            }
        }

        public List<Haven> GeefHavens()
        {
            try
            {
                return VisStatsRepository.LeesHavens();
            }
            catch (Exception ex)
            {
                throw new ManagerException("GeeftHavens");
            }
        }

        public List<int> GeefJaartallen()
        {
            try
            {
                return VisStatsRepository.LeesJaartallen();
            }
            catch (Exception ex)
            {
                throw new ManagerException("GeefJaartallen", ex);
            }
        }

        public List<Vissoort> GeefVissoorten()
        {
            try
            {
                return VisStatsRepository.LeesVissoorten();
            }
            catch (Exception ex)
            {
                throw new ManagerException("GeefVissoorten", ex);
            }
        }

        public List<JaarVangst> GeefVangst(int jaar, Haven haven, List<Vissoort> vissoorten, Eenheid eenheid)
        {
            try
            {
                return VisStatsRepository.LeesStatistieken(jaar, haven, vissoorten, eenheid);
            }
            catch (Exception ex)
            {
                throw new ManagerException("Geefvissoorten", ex);
            }
        }
    }
}