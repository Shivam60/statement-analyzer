namespace statement_analyzer.ModelClasses
{
    using NPOI.SS.UserModel;
    using statement_analyzer.Utils;
    public class SbiStatement : Statement
    {
        public SbiStatement(dynamic TxnDate, dynamic ValueDate, dynamic Description, dynamic RefNo, dynamic Debit, dynamic Credit, dynamic Balance)
        {
            this.TxnDate = TxnDate;
            this.ValueDate = ValueDate;
            this.Description = Description;
            this.RefNo = RefNo;
            this.Debit = Statement.GetNumberSafe(Debit);
            this.Credit = Statement.GetNumberSafe(Credit);
            this.Balance = Statement.GetNumberSafe(Balance);
        }

        public SbiStatement(IRow sbiStatementRow)
        {
            this.TxnDate = NpoiUtils.GetCellValueSafe(sbiStatementRow.GetCell(0));
            this.ValueDate = NpoiUtils.GetCellValueSafe(sbiStatementRow.GetCell(1));
            this.Description = NpoiUtils.GetCellValueSafe(sbiStatementRow.GetCell(2));
            this.RefNo = NpoiUtils.GetCellValueSafe(sbiStatementRow.GetCell(3));
            this.Debit = GetNumberSafe(NpoiUtils.GetCellValueSafe(sbiStatementRow.GetCell(4)));
            this.Credit = GetNumberSafe(NpoiUtils.GetCellValueSafe(sbiStatementRow.GetCell(5)));
            this.Balance = GetNumberSafe(NpoiUtils.GetCellValueSafe(sbiStatementRow.GetCell(6)));
        }
    }
}
