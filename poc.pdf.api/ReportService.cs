using DinkToPdf;
using DinkToPdf.Contracts;

namespace poc.pdf.api
{
    public interface IReportService
    {
        byte[] GeneratePdf(string html);
    }

    public class ReportService : IReportService
    {

        private readonly IConverter _converter;

        public ReportService(IConverter converter)
        {
            _converter = converter;
        }

        public byte[] GeneratePdf(string htmlView)
        {
            var objectSettings = new ObjectSettings
            {
                HtmlContent = htmlView,
                WebSettings = new WebSettings
                {
                    DefaultEncoding = "utf-8",
                    MinimumFontSize = 10,
                }
            };

            var htmlToPdfDocument = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings { Top = 15, Bottom = 25 }
                },
                Objects = { objectSettings },
            };

            return _converter.Convert(htmlToPdfDocument);
        }
    }
}

