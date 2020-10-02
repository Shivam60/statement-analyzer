namespace statement_analyzer.Repositary
{
    using Newtonsoft.Json;
    using Npoi.Mapper;
    using statement_analyzer.ModelClasses;
    using statement_analyzer.Repositary.Interface;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class StatementRowRepositary : IStatementRowRepositary
    {
        private Database database;
        public StatementRowRepositary()
        {
            using StreamReader r = new StreamReader(Path.Combine(Environment.CurrentDirectory, "data", "database.json"));
            string json = r.ReadToEnd();
            database = JsonConvert.DeserializeObject<Database>(json);
        }

        public bool DoesDescriptionMatchUpiIdentifiers(string description, string[] upiIdentifiers)
        {
            foreach(string upi in upiIdentifiers)
            {
                if(Regex.IsMatch(description, upi))
                {
                    return true;
                }
            }
            return false;
        }

        public string findBusiness(string description)
        {
            string upi = StatementRowRepositary.findUpiIdentifier(description);
            var business = database.knownBusinesses.FirstOrDefault(business => this.DoesDescriptionMatchUpiIdentifiers(upi, business.upiIdentifier));
            return business?.name ?? null;
        }

        public static string findUpiIdentifier(string description)
        {
            return string.Join("/", description.Split("/").Skip(3).Take(3));
        }
    }
}
