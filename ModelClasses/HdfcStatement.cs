namespace statement_analyzer.ModelClasses
{
    using NPOI.SS.UserModel;
    using statement_analyzer.Utils;

    public class HdfcStatement: Statement
    {
        public HdfcStatement(IRow statementRow)
        {
            this.TxnDate = NpoiUtils.GetCellValueSafe(statementRow.GetCell(0));
            this.Description = NpoiUtils.GetCellValueSafe(statementRow.GetCell(1));
            this.RefNo = NpoiUtils.GetCellValueSafe(statementRow.GetCell(2));
            this.ValueDate = NpoiUtils.GetCellValueSafe(statementRow.GetCell(3));
            this.Debit = GetNumberSafe(NpoiUtils.GetCellValueSafe(statementRow.GetCell(4)));
            this.Credit = GetNumberSafe(NpoiUtils.GetCellValueSafe(statementRow.GetCell(5)));
            this.Balance = GetNumberSafe(NpoiUtils.GetCellValueSafe(statementRow.GetCell(6)));
        }
    }
}
