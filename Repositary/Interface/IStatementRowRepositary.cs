namespace statement_analyzer.Repositary.Interface
{
    using statement_analyzer.ModelClasses;

    public interface IStatementRowRepositary
    {
        bool DoesDescriptionMatchUpiIdentifiers(string description, string[] upiIdentifiers);
        string findBusiness(string description);
    }
}
