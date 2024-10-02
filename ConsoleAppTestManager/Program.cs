using VisStatsBL.Interfaces;
using VisStatsBL.Managers;
using VisStatsDL_File;
using VisStatsDL_SQL;

namespace ConsoleAppTestManager
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            string conn = "Data Source=LAPTOP-6EGCK7EE\\SQLEXPRESS;Initial Catalog=VisStats1F;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
            string filePath = @"C:\data\Vis\vissoorten1.txt";
            IFileProcessor fp = new FileProcessor();
            IVisStatsRepository repo = new VisStatsRepository(conn);

            VisStatsManager vm = new VisStatsManager(fp, repo);
            vm.UploadVissoorten(filePath);
        }
    }
}
