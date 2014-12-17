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
using System.Windows.Shapes;
using Autodesk.ADN.Toolkit.ReCap.DataContracts;

namespace Autodesk.ADN.ReCapWPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class SceneProgressDlg: Window
    {
        public SceneProgressDlg(
            ReCapSceneProgressNotifier notifier,
            string sceneName)
        {
            InitializeComponent();

            _lbSceneName.Content = sceneName;

            notifier.OnSceneProgressChanged +=
                new OnSceneProgressChangedHandler(
                    OnProgressChange);

            notifier.OnSceneProgressError += 
                new OnSceneProgressErrorHandler(
                    OnError);
        }

        void OnProgressChange(ReCapPhotosceneResponse response)
        {
            string progress = response.Photoscene.Progress.ToString("F2") + " %";

            this.Title = "Photoscene Progress - " + progress;

            _lbProgress.Content = response.Photoscene.ProgressMsg;

            _progressBar.Value = response.Photoscene.Progress;
            //_progressBar.LabelText = progress;
        }

        void OnError(ReCapError error)
        {
            this.Title = "Photoscene Progress - Error";

            _lbProgress.Content = error.Msg;
        }
    }
}
