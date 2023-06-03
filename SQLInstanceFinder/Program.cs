using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLInstanceFinder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> dbs;
            SQLServerInstanceFinder findr = new SQLServerInstanceFinder();

            List<string> connStrs = findr.GetDatabaseConnectionStrings();

            foreach(string connStr in connStrs)
            {
                Console.WriteLine("======================================================================");

                Console.WriteLine(connStr);

                dbs = findr.GetDatabases(connStr);

                foreach(string theDB in dbs)
                {
                    Console.WriteLine("   "+theDB);
                }
            }
            Console.WriteLine("======================================================================");
            Console.WriteLine("");
            Console.WriteLine("That's it! Press any key to close.");
            Console.ReadLine();
        }
    }
}
