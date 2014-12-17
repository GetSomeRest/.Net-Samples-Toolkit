using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Autodesk.ADN.Toolkit.OAuth
{
    public delegate void OnLoginSuccessHandler(
        Uri resultUri);

    /// <summary>
    /// Interaction logic for AdnLoginControl.xaml
    /// </summary>
    public partial class AdnLoginControl : UserControl
    {
        public event OnLoginSuccessHandler
           OnLoginSuccess = null;

        public AdnLoginControl(Uri loginUri, Uri targetUri)
        {
            InitializeComponent();

            LoginUri = loginUri;

            TargetUri = targetUri;

            _webBrowser.Navigated += 
                OnFirstNavigated;

            this.Loaded += OnLoad;
        }

        void OnLoad(object sender, RoutedEventArgs e)
        {
            _webBrowser.Source = LoginUri;
        }

        public Uri LoginUri
        {
            get;
            private set;
        }

        public Uri ResultUri
        {
            get;
            private set;
        }

        public Uri TargetUri
        {
            get;
            private set;
        }

        void OnFirstNavigated(
            object sender, 
            NavigationEventArgs e)
        {
            _webBrowser.Navigated -= 
                OnFirstNavigated;

            _webBrowser.Navigated += 
                OnNavigated;
        }

        void OnNavigated(
            object sender, 
            NavigationEventArgs e)
        {
            if (e.Uri.AbsoluteUri.StartsWith(TargetUri.AbsoluteUri))
            {
                ResultUri = e.Uri;

                if (OnLoginSuccess != null)
                {
                    OnLoginSuccess(e.Uri);
                }
            }
        }
    }
}
