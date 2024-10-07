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

                    //schrijf in Upload / korte versie
                    cmd.CommandText = SQLupload;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@fileName", fileName.Substring(fileName.LastIndexOf("\\") + 1));
                    cmd.Parameters.AddWithValue("@pad", fileName.Substring(0, fileName.LastIndexOf("\\") + 1));
                    cmd.Parameters.AddWithValue("@datum", DateTime.Now);
                    cmd.ExecuteNonQuery();
                    cmd.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    cmd.Transaction.Rollback();
                    throw new Exception("SchrijfStatistieken", ex);
                }
            }
        }
    }
}
