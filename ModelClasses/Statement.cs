using System;
namespace statement_analyzer.ModelClasses
{
    public class Statement
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

        protected static double GetNumberSafe(dynamic value)
        {
            return value.GetType() == "".GetType() ? 0 : Convert.ToDouble(value);
        }
    }
}
