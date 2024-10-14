using Microsoft.Data.SqlClient;
using System.Data;
using VisStatsBL.Interfaces;
using VisStatsBL.Model;

namespace VisStatsDL_SQL
{
    public class VisStatsRepository : IVisStatsRepository
    {
        private string connectionString;

        public VisStatsRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool HeeftVissoort(Vissoort vis)
        {
            string SQL = "SELECT Count(*) FROM Soort WHERE naam=@naam";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                    cmd.Parameters["@naam"].Value = vis.Naam;
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {
                    throw new Exception("HeeftVissoort", ex);
                }
            }
        }

        public void SchrijfSoort(Vissoort vis)
        {
            string SQL = "INSERT INTO Soort(Naam) VALUES (@Naam)";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                    cmd.Parameters["@naam"].Value = vis.Naam;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Schrijfsoort", ex);
                }
            }
        }

        public bool HeeftHaven(Haven haven)
        {
            string SQL = "SELECT Count(*) FROM Haven WHERE naam=@naam";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                    cmd.Parameters["@naam"].Value = haven.Naam;
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {
                    throw new Exception("HeeftHaven", ex);
                }
            }
        }

        public void SchrijfHaven(Haven haven)
        {
            string SQL = "INSERT INTO Haven(Naam) VALUES (@Naam)";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                    cmd.Parameters["@naam"].Value = haven.Naam;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("SchrijfHaven", ex);
                }
            }
        }

        public List<Haven> LeesHavens()
        {
            string SQl = "SELECT * FROM Haven";
            List<Haven> havens = new List<Haven>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQl;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        havens.Add(new Haven((int)reader["id"], (string)reader["naam"]));
                    }
                    return havens;
                }
                catch (Exception ex) { throw new Exception("Leeshavens", ex); }
            }
        }

        public List<Vissoort> LeesVissoorten()
        {
            string SQl = "SELECT * FROM Soort";
            List<Vissoort> soorten = new List<Vissoort>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQl;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        soorten.Add(new Vissoort((int)reader["id"], (string)reader["naam"]));
                    }
                    return soorten;
                }
                catch (Exception ex) { throw new Exception("LeesSoorten", ex); }
            }
        }

        public bool IsOpgeladen(string fileName)
        {
            string SQl = "SELECT Count(*) FROM upload WHERE filename=@filename";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQl;
                    cmd.Parameters.AddWithValue("@filename", SqlDbType.NVarChar);
                    cmd.Parameters["@filename"].Value = fileName.Substring(fileName.LastIndexOf('\\') + 1);
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex) { throw new Exception("IsOpgeladen", ex); }
            }
        }

        public void SchrijfStatistieken(List<VisStatsDataRecord> data, string fileName)
        {
            string SQLdata = "INSERT INTO VisStats(jaar,maand,haven_id,soort_id,gewicht,waarde) VALUES (@jaar,@maand,@haven_id,@soort_id,@gewicht,@waarde)";
            string SQLupload = "INSERT INTO Upload (fileName,datum,pad) VALUES (@fileName,@datum,@pad)";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQLdata;
                    cmd.Transaction = conn.BeginTransaction();

                    //schrijf in VisStats / lange versie
                    cmd.Parameters.Add(new SqlParameter("@jaar", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@maand", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@haven_id", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@soort_id", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@gewicht", SqlDbType.Float));
                    cmd.Parameters.Add(new SqlParameter("@waarde", SqlDbType.Float));
                    foreach (VisStatsDataRecord dataRecord in data)
                    {
                        cmd.Parameters["@jaar"].Value = dataRecord.Jaar;
                        cmd.Parameters["@maand"].Value = dataRecord.Maand;
                        cmd.Parameters["@haven_id"].Value = dataRecord.Haven.ID;
                        cmd.Parameters["@soort_id"].Value = dataRecord.Vissoort.ID;
                        cmd.Parameters["@gewicht"].Value = dataRecord.Gewicht;
                        cmd.Parameters["@waarde"].Value = dataRecord.Waarde;
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    cmd.Transaction.Rollback();
                    throw new Exception("SchrijfStatistieken", ex);
                }
            }
        }

        public List<int> LeesJaartallen()
        {
            string SQL = "SELECT DISTINCT jaar FROM VisStats";
            List<int> jaartallen = new List<int>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        jaartallen.Add((int)reader["jaar"]);
                    }
                    return jaartallen;
                }
                catch (Exception ex) { throw new Exception("LeesJaartallen", ex); }
            }
        }   

        public List<JaarVangst> LeesStatistieken(int jaar, Haven haven, List<Vissoort> vissoorten, Eenheid eenheid)
        {
            string kolom = "";
            switch (eenheid)
            {
                case Eenheid.kg: kolom = "gewicht"; break;
                case Eenheid.euro: kolom = "waarde"; break;
            }
            string paramSoorten = "";
            for (int i = 0; i < vissoorten.Count; i++) paramSoorten += $"@ps{i},";
            paramSoorten = paramSoorten.Remove(paramSoorten.Length - 1);
            string SQL = $"SELECT t2.naam, t1.jaar, SUM({kolom}) totaal, MIN({kolom}) minimum, MAX({kolom}) " +
                $"maximum, AVG({kolom}) gemiddelde FROM VisStats t1 INNER JOIN soort t2 ON " +
                $"t1.soort_id = t2.id WHERE haven_id = @haven_id AND " +
                $"jaar = @jaar AND soort_id IN ({paramSoorten}) GROUP BY t2.naam, t1.jaar";
            List<JaarVangst> vangst = new();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@jaar", jaar);
                    cmd.Parameters.AddWithValue("@haven_id", haven.ID);
                    for (int i = 0; i < vissoorten.Count; i++) cmd.Parameters.AddWithValue($"@ps{i}", vissoorten[i].ID);
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        vangst.Add(new JaarVangst((string)reader["naam"], 
                            (double)reader["totaal"], (double)reader["minimum"], 
                            (double)reader["maximum"], (double)reader["gemiddelde"]));
                    }
                    return vangst;
                }
                catch (Exception ex) { throw new Exception("GeefJaarVangst", ex); }
            }
        }
    }
}
