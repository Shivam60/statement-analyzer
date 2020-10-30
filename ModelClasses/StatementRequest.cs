using Microsoft.AspNetCore.Http;

namespace statement_analyzer.ModelClasses
{
    public class StatementRequest
    {
        public string file { get; set; }
        public string bank { get; set; }
    }
}
