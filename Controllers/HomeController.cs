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
        [Route("getSummary")]
        public dynamic GetSummary()
        {
            HSSFWorkbook hssfwb;
            using (FileStream file = new FileStream(Path.Combine(Environment.CurrentDirectory, "1601675837424Ko3KrO6VYnJMpFhB.xls"), FileMode.Open, FileAccess.Read))
            {
                hssfwb = new HSSFWorkbook(file);
            }
            List<SbiStatement> respList = new List<SbiStatement>();
            ISheet sheet = hssfwb.GetSheet("1601675837424Ko3KrO6VYnJMpFhB");
            for (int row = sheet.FirstRowNum; row <= 19; row++)
            {
                sheet.RemoveRow(sheet.GetRow(row));
            }

            for (int row = sheet.FirstRowNum; row <= sheet.LastRowNum-2; row++)
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
            var resp = statementRowRepositary.GetExpenditureSummary(respList);
            return resp;
        }

        [HttpGet]
        [Route("getKnownBusinesses")]
        public dynamic GetKnownBusinesses()
        {
            return statementRowRepositary.getKnownBusinessess();
        }

        [HttpPost]
        [Route("setKnownBusinesses")]
        public dynamic SetKnownBusinesses(Business business)
        {
            return statementRowRepositary.getKnownBusinessess();
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
