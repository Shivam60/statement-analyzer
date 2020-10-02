using System;
using NPOI.SS.UserModel;

namespace statement_analyzer.ModelClasses
{
    public class SbiStatement
    {
        private dynamic _Description;
        public dynamic TxnDate { get; set; }
        public dynamic ValueDate { get; set; }
        public dynamic Description 
        { 
            get { return _Description; }
            set { _Description = value.Trim(); }
        }
        public string RefNo { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public double Balance { get; set; }

        public SbiStatement(dynamic TxnDate, dynamic ValueDate, dynamic Description, dynamic RefNo, dynamic Debit, dynamic Credit, dynamic Balance)
        {
            this.TxnDate = TxnDate;
            this.ValueDate = ValueDate;
            this.Description = Description;
            this.RefNo = RefNo;
            this.Debit = SbiStatement.GetNumberSafe(Debit);
            this.Credit = SbiStatement.GetNumberSafe(Credit);
            this.Balance = SbiStatement.GetNumberSafe(Balance);
        }

        private static double GetNumberSafe(dynamic value)
        {
            return value.GetType() == "".GetType() ? 0 : Convert.ToDouble(value);
        }
    }
}
