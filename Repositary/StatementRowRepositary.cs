namespace statement_analyzer.Repositary
{
    using Newtonsoft.Json;
    using Npoi.Mapper;
    using statement_analyzer.ModelClasses;
    using statement_analyzer.Repositary.Interface;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class StatementRowRepositary : IStatementRowRepositary
    {
        private Database database;
        public enum TransactionType
        {
            UPI,
            INB,
            WITHDRAWALTRANSFER,
            UNKNOWN
        }
        public StatementRowRepositary()
        {
            using StreamReader r = new StreamReader(Path.Combine(Environment.CurrentDirectory, "data", "database.json"));
            string json = r.ReadToEnd();
            database = JsonConvert.DeserializeObject<Database>(json);
        }

        public bool DoesDescriptionMatchInIdentifiers(string description, string[] identifiers)
        {
            foreach(string identifier in identifiers)
            {
                if(Regex.IsMatch(description, identifier))
                {
                    return true;
                }
            }
            return false;
        }

        public string findBusinessFromUpi(string description)
        {
            string upi = FindUpiIdentifier(description);
            var business = database.knownBusinesses.FirstOrDefault(business => this.DoesDescriptionMatchInIdentifiers(upi, business.upiIdentifier));
            return business?.name ?? null;
        }

        public string findBusinessFromInb(string description)
        {
            string inb = FindInbIdentifier(description);
            var business = database.knownBusinesses.FirstOrDefault(business => this.DoesDescriptionMatchInIdentifiers(inb, business.inbIdentifier));
            return business?.name ?? null;
        }

        public static string FindUpiIdentifier(string description)
        {
            return string.Join("/", description.Split("/").Skip(3).Take(3));
        }

        public static string FindInbIdentifier(string description)
        {
            return string.Join(" ", description.Split(" ").Skip(2).Take(4));
        }

        public Dictionary<string, Dictionary<string, double>> GetExpenditureSummary(List<SbiStatement> respList)
        {
            var resp = new Dictionary<string, Dictionary<string, double>>();
            var expenditureSummary = new Dictionary<string, double>();
            var unProcessedExpenditureSummary = new Dictionary<string, double>();
            respList.ForEach((row) =>
            {
                string businessName = null;
                switch(GetTransactionType(row.Description))
                {
                    case TransactionType.UPI:
                        businessName = findBusinessFromUpi(row.Description);
                        break;
                    case TransactionType.INB:
                        businessName = findBusinessFromInb(row.Description);
                        break;
                }
                if (businessName != null)
                {
                    double summary = row.Debit - row.Credit;
                    expenditureSummary[businessName] = expenditureSummary.ContainsKey(businessName) ? expenditureSummary[businessName] + summary : summary;
                }
                else
                {
                    double summary = row.Debit - row.Credit;
                    unProcessedExpenditureSummary[row.Description] = unProcessedExpenditureSummary.ContainsKey(row.Description) ? unProcessedExpenditureSummary[row.Description] + summary : summary;
                }
            });
            resp.Add("expenditureSummary", expenditureSummary);
            resp.Add("unProcessedExpenditureSummary", unProcessedExpenditureSummary);
            return resp;
        }

        public Dictionary<string, double> GetUnProcessedExpenditureSummary(List<SbiStatement> respList)
        {
            var resp = new Dictionary<string, double>();
            respList.ForEach((row) =>
            {
                    string businessName = findBusinessFromUpi(row.Description);
                    if (businessName == null)
                    {
                        double summary = row.Debit - row.Credit;
                        resp[row.Description] = resp.ContainsKey(row.Description) ? resp[row.Description] + summary : summary;
                    }
            });
            return resp;
        }

        public Business[] getKnownBusinessess()
        {
            return database.knownBusinesses;
        }

        public static TransactionType GetTransactionType(string description)
        {
            if(string.CompareOrdinal(description, "   WITHDRAWAL TRANSFER---")==0)
            {
                return TransactionType.WITHDRAWALTRANSFER;
            }
            if(description.Contains("TO TRANSFER-UPI") || description.Contains("BY TRANSFER-UPI"))
            {
                return TransactionType.UPI;
            }
            else if(description.Contains("TO TRANSFER-INB") || description.Contains("BY TRANSFER-INB"))
            {
                return TransactionType.INB;
            }
            return TransactionType.UNKNOWN;
        }
    }
}
