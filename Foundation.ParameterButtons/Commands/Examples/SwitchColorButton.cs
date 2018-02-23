using Sitecore.Shell.Framework.Commands;

namespace Foundation.ParameterButtons.Commands.Examples
{
    public class SwitchColorButton : UpdateRenderingParameterCommand
    {
        public SwitchColorButton()
        {
            base.SetKey("color");
        }

        public override void Execute(CommandContext context)
        {
            var currentvalue = this.GetParameterValue(context);

            string newColor;
            switch (currentvalue)
            {
                case "blue":
                    newColor = "red";
                    break;
                case "red":
                    newColor = "green";
                    break;
                default:
                    newColor = "blue";
                    break;
            }
            base.SetValue(newColor);

            base.Execute(context);
        }
    }
}