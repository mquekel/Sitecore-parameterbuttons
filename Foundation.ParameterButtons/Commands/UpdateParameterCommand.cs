using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Layouts;
using Sitecore.Shell.Applications.WebEdit.Commands;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.UI.Sheer;

namespace Foundation.ParameterButtons.Commands
{
    public abstract class UpdateRenderingParameterCommand : WebEditCommand
    {
        private string ParamKey { get; set; }

        private string ParamValue { get; set; }

        protected void SetKey(string key)
        {
            this.ParamKey = key;
        }

        protected void SetValue(string value)
        {
            this.ParamValue = value;
        }

        protected void SetValue(int value)
        {
            this.ParamValue = value.ToString();
        }

        public override void Execute(CommandContext context)
        {
            var id = ShortID.Decode(WebUtil.GetFormValue("scDeviceID"));
            var layoutDefinition = GetLayoutDefinition();
            if (layoutDefinition == null)
            {
                this.ReturnLayout();
            }
            else
            {
                var device = layoutDefinition.GetDevice(id);
                if (device == null)
                {
                    this.ReturnLayout();
                }
                else
                {
                    var renderingByUniqueId = device.GetRenderingByUniqueId(context.Parameters["referenceId"]);
                    if (renderingByUniqueId == null)
                    {
                        this.ReturnLayout();
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(renderingByUniqueId.Parameters))
                        {
                            if (!string.IsNullOrEmpty(renderingByUniqueId.ItemID))
                            {
                                var renderingItem = (RenderingItem)Sitecore.Client.ContentDatabase.GetItem(renderingByUniqueId.ItemID);
                                renderingByUniqueId.Parameters = renderingItem != null ? renderingItem.Parameters : string.Empty;
                            }
                            else
                            {
                                renderingByUniqueId.Parameters = string.Empty;
                            }
                        }

                        var parameters = WebUtil.ParseUrlParameters(renderingByUniqueId.Parameters);

                        parameters[this.ParamKey] = this.ParamValue as string;

                        renderingByUniqueId.Parameters = new UrlString(parameters).GetUrl();
                        this.ReturnLayout(WebEditUtil.ConvertXMLLayoutToJSON(layoutDefinition.ToXml()));
                    }
                }
            }
        }

        private LayoutDefinition GetLayoutDefinition()
        {
            var xml = WebEditUtil.ConvertJSONLayoutToXML(WebUtil.GetFormValue("scLayout"));
            var layoutDefinition = LayoutDefinition.Parse(xml);
            return layoutDefinition;
        }

        protected string GetParameterValue(CommandContext context)
        {
            var id = ShortID.Decode(WebUtil.GetFormValue("scDeviceID"));
            var layoutDefinition = GetLayoutDefinition();
            if (layoutDefinition == null)
            {
                return string.Empty;
            }

            var device = layoutDefinition.GetDevice(id);
            var renderingByUniqueId = device?.GetRenderingByUniqueId(context.Parameters["referenceId"]);
            if (renderingByUniqueId == null)
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(renderingByUniqueId.Parameters))
            {
                return string.Empty;
            }

            var parameters = WebUtil.ParseUrlParameters(renderingByUniqueId.Parameters);

            return parameters[this.ParamKey];
        }

        protected virtual void ReturnLayout(string layout = null)
        {
            SheerResponse.SetAttribute("scLayoutDefinition", "value", layout ?? string.Empty);
            if (string.IsNullOrEmpty(layout))
            {
                return;
            }

            SheerResponse.Eval("window.parent.Sitecore.PageModes.ChromeManager.handleMessage('chrome:rendering:propertiescompleted');");
        }
    }
}