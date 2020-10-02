namespace statement_analyzer.Repositary.Interface
{
    using System.Collections.Generic;
    using statement_analyzer.ModelClasses;
    using static statement_analyzer.Repositary.StatementRowRepositary;

    public interface IStatementRowRepositary
    {
        bool DoesDescriptionMatchInIdentifiers(string description, string[] upiIdentifiers);
        string findBusinessFromUpi(string description);
        Dictionary<string, Dictionary<string, double>> GetExpenditureSummary(List<SbiStatement> respList);
        Business[] getKnownBusinessess();
    }
}
