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

        public IEncodedString Partial(string templateName)
        {
            var template = this.templateService.Resolve(templateName, "");

            return new RawString(template.Run(new ExecuteContext()));
        }
    }
}