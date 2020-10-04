namespace statement_analyzer.Utils
{
    using NPOI.SS.UserModel;
    public class NpoiUtils
    {
        public static dynamic GetCellValueSafe(ICell cell)
        {
            switch (cell.CellType)
            {
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Numeric:
                    return cell.NumericCellValue;
                case CellType.Boolean:
                    return cell.BooleanCellValue;
            }
            return "";

        }
    }
}
