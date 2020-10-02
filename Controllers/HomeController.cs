using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NPOI.SS.UserModel;
using statement_analyzer.ModelClasses;
using NPOI.HSSF.UserModel;
using statement_analyzer.Repositary;
using statement_analyzer.Repositary.Interface;

namespace statement_analyzer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IStatementRowRepositary statementRowRepositary;

        public HomeController(IStatementRowRepositary _statementRowRepositary)
        {
            statementRowRepositary = _statementRowRepositary ?? throw new ArgumentNullException();
        }

        [HttpGet]
        public dynamic Get()
        {
            HSSFWorkbook hssfwb;
            using (FileStream file = new FileStream(Path.Combine(Environment.CurrentDirectory, "1601643806906lAGkzP0qRHbsL0T8.xls"), FileMode.Open, FileAccess.Read))
            {
                hssfwb = new HSSFWorkbook(file);
            }
            List<SbiStatement> respList = new List<SbiStatement>();
            ISheet sheet = hssfwb.GetSheet("Sheet1");
            for (int row = 1; row <= sheet.LastRowNum; row++)
            {
                if (sheet.GetRow(row) != null) //null is when the row only contains empty cells 
                {
                    var a = new SbiStatement(
                        this.getValue(sheet.GetRow(row).GetCell(0)),
                        this.getValue(sheet.GetRow(row).GetCell(1)),
                        this.getValue(sheet.GetRow(row).GetCell(2)),
                        this.getValue(sheet.GetRow(row).GetCell(3)),
                        this.getValue(sheet.GetRow(row).GetCell(4)),
                        this.getValue(sheet.GetRow(row).GetCell(5)),
                        this.getValue(sheet.GetRow(row).GetCell(6)));
                    respList.Add(a);
                }
            }
            var resp = new Dictionary<string, double>();
            var resp2 = new Dictionary<string, double>();
            respList.ForEach((row) =>
            {
                string businessName = statementRowRepositary.findBusiness(row.Description);
                if (businessName != null)
                {
                    resp[businessName] = resp.ContainsKey(businessName) ? resp[businessName] + row.Debit : row.Debit;
                }
                else
                {
                    resp2[StatementRowRepositary.findUpiIdentifier(row.Description)] = row.Debit;
                }
            });
            return resp2;
        }

        private dynamic getValue(ICell cell)
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
