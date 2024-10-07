using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VisStatsBL.Interfaces;
using VisStatsBL.Model;

namespace VisStatsDL_File
{
    public class FileProcessor : IFileProcessor
    {
        public List<string> LeesSoorten(string fileName)
        {
			try
			{
				List<string> soorten = new List<string>();
				using(StreamReader reader = new StreamReader(fileName))
				{
                    string line;
                    while((line = reader.ReadLine()) != null)
					{
                        soorten.Add(line.Trim());
                    }
                }
                return soorten;
            }
			catch (Exception ex)
			{
				throw new Exception("FileProcessor-LeesSoorten");
			}
        }

        public List<string> LeesHavens(string fileName)
        {
            try
            {
                List<string> havens = new List<string>();
                using (StreamReader reader = new StreamReader(fileName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        havens.Add(line.Trim());
                    }
                }
                return havens;
            }
            catch (Exception ex)
            {
                throw new Exception("FileProcessor-LeesHavens");
            }
        }

        public List<VisStatsDataRecord> LeesStatistieken(string fileName, List<Vissoort> vissoorten, List<Haven> havens)
        {
            try
            {                                                                   //string     object
                Dictionary<string, Vissoort> soortenD = vissoorten.ToDictionary(x => x.Naam, x => x);
                Dictionary<string, Haven> havensD = havens.ToDictionary(x => x.Naam, x => x);
                Dictionary<(string, int, int, string), VisStatsDataRecord> data = new(); // haven, jaar, maand, soort
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string line;
                    int jaar = 0, maand = 0;
                    List<string> havensTXT = new List<string>();
                    while ((line = sr.ReadLine()) != null)
                    {
                        //nieuwe maand
                        if (Regex.IsMatch(line, @"^-+\d{6}-+"))
                        {
                            Console.WriteLine(line);
                            jaar = Int32.Parse(Regex.Match(line, @"\d{4}").Value);
                            maand = Int32.Parse(Regex.Match(line, @"(\d{2})-+").Groups[1].Value);
                            havensTXT.Clear();
                        }
                        //havens lezen
                        else if (line.Contains("Vissoorten|Totaal van de havens"))
                        {
                            string pattern = @"\|([A-Za-z]+)\|";
                            MatchCollection matches = Regex.Matches(line, pattern);
                            foreach (Match match in matches) havensTXT.Add(match.Groups[1].Value);
                        }

                        // statistiekens inlezen
                        else
                        {
                            string[] element = line.Split('|');
                            // Eerste element is de naam van de vissoort
                            if (soortenD.ContainsKey(element[0]))
                            {
                                for (int i = 0; i < havensTXT.Count; i++)
                                {
                                    if (havensD.ContainsKey(havensTXT[i]))
                                    {
                                        if (data.ContainsKey((havensTXT[i], jaar, maand, element[0])))
                                        {
                                            data[(havensTXT[i], jaar, maand, element[0])].Gewicht += ParseValue(element[(i * 2) + 3]);
                                            data[(havensTXT[i], jaar, maand, element[0])].Waarde += ParseValue(element[(i * 2) + 4]);
                                        }
                                        else
                                        {
                                            data.Add((havensTXT[i], jaar, maand, element[0]),
                                                new VisStatsDataRecord(jaar, maand, ParseValue(element[(i * 2) + 3]),
                                                ParseValue(element[(i * 2) + 4]),
                                                havensD[havensTXT[i]],
                                                soortenD[element[0]]));
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                return data.Values.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("FileProcessor-LeesStatistieken", ex);
            }
        }

        private double ParseValue(string value)
        {
            if (double.TryParse(value, out double d)) return d;
            else return 0.0;
        }
    }
}
