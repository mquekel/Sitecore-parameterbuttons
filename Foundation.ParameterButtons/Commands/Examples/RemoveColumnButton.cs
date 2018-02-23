using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Sitecore.Shell.Framework.Commands;

namespace Foundation.ParameterButtons.Commands.Examples
{
    public class RemoveColumnButton : UpdateRenderingParameterCommand
    {
        public RemoveColumnButton()
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
                columnCount--;
            }

            SetValue(columnCount);
            base.Execute(context);
        }
    }
}