using CodeModel.Symbols;
using RazorEngine.Templating;
using RazorEngine.Text;

namespace RuleRunner.Reports.Html
{
    public class ReportTemplateBase<TModel> : TemplateBase<TModel>
    {
        public ReportHelper Helper
        {
            get { return new ReportHelper(this._context, this.TemplateService); }
        }

        public ReportTemplateBase()
        {
            
        }
    }

    public class ReportHelper
    {
        private readonly ExecuteContext executeContext;
        private readonly ITemplateService templateService;

        public ReportHelper(ExecuteContext executeContext, ITemplateService templateService)
        {
            this.executeContext = executeContext;
            this.templateService = templateService;
        }

        public IEncodedString Partial<TModel>(string templateName, TModel model)
        {
            var template = this.templateService.Resolve(templateName, model);

            return new RawString(template.Run(new ExecuteContext()));
        }

        public IEncodedString SafePercent(double percent)
        {
            if (double.IsNaN(percent))
            {
                return new HtmlEncodedString("N/A");                     
            }
            else
            {
                return new HtmlEncodedString(percent.ToString("P"));
            }
        }

        public IEncodedString SourceLocation(SourceLocation location)
        {
            var s = string.Format("{0}({1},{2})", location.FileName, location.StartLine, location.StartColumn);

            return new HtmlEncodedString(s);
        }
    }
}