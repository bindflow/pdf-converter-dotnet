using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

namespace poc.pdf.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PdfController : ControllerBase
    {
        private readonly IReportService _report;

        public PdfController(IReportService report)
        {
            _report = report;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateSamplePdfAsync([FromBody] PdfRequest request)
        {
            string contentType = "application/octect-stream";
            string html = string.Empty;

            if (!string.IsNullOrEmpty(request.HtmlUrl))
            {
                var web = new HtmlWeb();
                var doc = await web.LoadFromWebAsync(request.HtmlUrl);

                html = doc.DocumentNode.InnerHtml;

                foreach (var parametro in request.Chaves)
                    html = html.Replace($"@@{parametro.Chave}@@", parametro.Valor);
            }

            return File(_report.GeneratePdf(html), contentType, fileDownloadName: $"{request.FileName}.pdf");
        }
    }

    public class PdfRequest
    {
        public string FileName { get; set; }
        public string HtmlUrl { get; set; }
        public List<PdfKeyValues> Chaves { get; set; }
    }

    public class PdfKeyValues
    {
        public string Chave { get; set; }
        public string Valor { get; set; }
    }
}