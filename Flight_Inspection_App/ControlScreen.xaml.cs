using System.Windows;

namespace Flight_Inspection_App
{
    public partial class ControlScreen : Window
    {
        public ControlScreen()
        {
            InitializeComponent();
        }

        internal void setConnect(Connect c)
        {
            videoControl.setConnect(c);
            showGraph.setConnect(c);
            joystick.setConnect(c);
            dashboard.setConnect(c);
            erorcontrol.setConnect(c);

        }
    }
}