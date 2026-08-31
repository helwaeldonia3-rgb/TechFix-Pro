using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace TechFix.Pro
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.ExtendsContentIntoTitleBar = true;
            SetupNavigation();
        }

        private void SetupNavigation()
        {
            NavView.ItemInvoked += (sender, args) =>
            {
                if (args.IsSettingsInvoked)
                {
                    // Navigate to settings
                }
                else if (args.InvokedItemContainer is NavigationViewItem item)
                {
                    var tag = item.Tag?.ToString();
                    NavigateToPage(tag);
                }
            };
        }

        private void NavigateToPage(string? tag)
        {
            // Navigation logic will be implemented
            switch (tag)
            {
                case "dashboard":
                    // ContentFrame.Navigate(typeof(DashboardPage));
                    break;
                case "device-detection":
                    // ContentFrame.Navigate(typeof(DeviceDetectionPage));
                    break;
                // Add other cases as needed
            }
        }
    }
}