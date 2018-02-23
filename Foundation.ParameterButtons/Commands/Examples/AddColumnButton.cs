
using Sitecore.Shell.Framework.Commands;

namespace Foundation.ParameterButtons.Commands.Examples
{
    public class AddColumnButton : UpdateRenderingParameterCommand
    {
        public AddColumnButton()
        {
            SetKey("columns");
        }

        public override void Execute(CommandContext context)
        {
            var columns = this.GetParameterValue(context);
            int columnCount;
            if (!int.TryParse(columns, out columnCount))
            {
                columnCount = 1;
            }
            else
            {
                columnCount++;
            }

            SetValue(columnCount);
            base.Execute(context);
        }
    }
}