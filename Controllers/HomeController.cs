﻿namespace statement_analyzer.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.AspNetCore.Mvc;
    using NPOI.SS.UserModel;
    using statement_analyzer.ModelClasses;
    using NPOI.HSSF.UserModel;
    using statement_analyzer.Repositary.Interface;
    using System.Net;

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
            var sheetName = "july-2020";
            HSSFWorkbook hssfwb;
            using (FileStream file = new FileStream(Path.Combine(Environment.CurrentDirectory, $"{sheetName}.xls"), FileMode.Open, FileAccess.Read))
            {
                hssfwb = new HSSFWorkbook(file);
            }
            List<Statement> respList = new List<Statement>();
            ISheet sheet = hssfwb.GetSheetAt(0);
            for (int row = sheet.FirstRowNum; row <= 19; row++)
            {
                sheet.RemoveRow(sheet.GetRow(row));
            }

            for (int row = sheet.FirstRowNum; row <= sheet.LastRowNum - 2; row++)
            {
                if (sheet.GetRow(row) != null) //null is when the row only contains empty cells 
                {
                    try
                    {
                        var a = new SbiStatement(sheet.GetRow(row));
                        respList.Add(a);
                    }
                    catch (Exception e)
                    {
                        // to do
                    }
                }
            }
            var resp = statementRowRepositary.GetExpenditureSummary(respList);
            return resp;
        }

        [HttpGet]
        [Route("getSummaryHDFC")]
        public dynamic GetSummaryH()
        {
            var sheetName = "july";
            HSSFWorkbook hssfwb;
            using (FileStream file = new FileStream(Path.Combine(Environment.CurrentDirectory, $"{sheetName}.xls"), FileMode.Open, FileAccess.Read))
            {
                hssfwb = new HSSFWorkbook(file);
            }
            List<Statement> respList = new List<Statement>();
            ISheet sheet = hssfwb.GetSheetAt(0);
            for (int row = sheet.FirstRowNum; row <= 20; row++)
            {
                sheet.RemoveRow(sheet.GetRow(row));
            }
            sheet.RemoveRow(sheet.GetRow(21));

            for (int row = sheet.FirstRowNum; row <= sheet.LastRowNum - 17; row++)
            {
                if (sheet.GetRow(row) != null) //null is when the row only contains empty cells 
                {
                    try
                    {
                        var a = new HdfcStatement(sheet.GetRow(row));
                        respList.Add(a);
                    }
                    catch (Exception e)
                    {
                        // to do
                    }
                }
            }
            var resp = statementRowRepositary.GetExpenditureSummary(respList);
            return resp;
        }

        [HttpPost("upload")]
        public HttpStatusCode PostFile(StatementRequest request)
        {

            return HttpStatusCode.Created;
        }
    }
}