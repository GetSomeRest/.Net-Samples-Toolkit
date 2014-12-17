using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for SceneSettingsDlg.xaml
    /// </summary>
    public partial class SceneSettingsDlg : Window
    {
        class MeshQualityItem
        {
            string _text;

            public MeshQualityEnum Value
            {
                get;
                private set;
            }

            public MeshQualityItem(
                string text, 
                MeshQualityEnum value)
            {
                _text = text;
                Value = value;
            }

            public override string ToString()
            {
                return _text;
            }
        }

        class MeshFormatItem
        {
            string _text;

            public MeshFormatEnum Value
            {
                get;
                private set;
            }

            public MeshFormatItem(
                string text,
                MeshFormatEnum value)
            {
                _text = text;
                Value = value;
            }

            public override string ToString()
            {
                return _text;
            }
        }

        private static int _qualityIndex = 1;

        private static int _formatIndex = 5;

        private bool _saveSettings = true;


        public SceneSettingsDlg()
        {
            InitializeComponent();

            string sceneName = "ADN - " + DateTime.Now.ToString(
                "dd/MM/yyyy - HH:mm:ss",
                CultureInfo.InvariantCulture);

            _tbSceneName.Text = sceneName;

            // Mesh Quality
            _cbMeshQuality.Items.Add(
                new MeshQualityItem(
                    "Draft", 
                    MeshQualityEnum.kDraft));

            _cbMeshQuality.Items.Add(
               new MeshQualityItem(
                   "Standard",
                   MeshQualityEnum.kStandard));

            _cbMeshQuality.Items.Add(
               new MeshQualityItem(
                   "High",
                   MeshQualityEnum.kHigh));

            // Mesh Format
            _cbMeshFormat.Items.Add(
                new MeshFormatItem(
                    "3dp", 
                    MeshFormatEnum.k3dp));

            _cbMeshFormat.Items.Add(
                new MeshFormatItem(
                    "Fbx",
                    MeshFormatEnum.kFbx));

            _cbMeshFormat.Items.Add(
                new MeshFormatItem(
                    "Fysc",
                    MeshFormatEnum.kFysc));

            _cbMeshFormat.Items.Add(
                new MeshFormatItem(
                    "Ipm",
                    MeshFormatEnum.kIpm));

            _cbMeshFormat.Items.Add(
                new MeshFormatItem(
                    "Las",
                    MeshFormatEnum.kLas));

            _cbMeshFormat.Items.Add(
                new MeshFormatItem(
                    "Obj",
                    MeshFormatEnum.kObj));

            _cbMeshFormat.Items.Add(
                new MeshFormatItem(
                    "Rcm",
                    MeshFormatEnum.kRcm));

            _cbMeshFormat.Items.Add(
                new MeshFormatItem(
                    "Rcs",
                    MeshFormatEnum.kRcs));


            _cbMeshQuality.SelectedIndex = _qualityIndex;
            _cbMeshFormat.SelectedIndex = _formatIndex;
        }

        public SceneSettingsDlg(
            string sceneName, 
            int quality,
            MeshFormatEnum format)
        {
            InitializeComponent();

            _saveSettings = false;

            _tbSceneName.Text = sceneName;
            _tbSceneName.IsEnabled = false;

            // Mesh Quality
            switch (quality)
            {
                case 7:

                    _cbMeshQuality.Items.Add(
                        new MeshQualityItem(
                            "Draft",
                            MeshQualityEnum.kDraft));
                    break;

                case 9:

                    _cbMeshQuality.Items.Add(
                       new MeshQualityItem(
                           "High",
                           MeshQualityEnum.kHigh));

                    break;

                case 8:
                default:

                    _cbMeshQuality.Items.Add(
                      new MeshQualityItem(
                          "Standard",
                          MeshQualityEnum.kStandard));

                    break;
            }

            // Mesh Format
            _cbMeshFormat.Items.Add(
                new MeshFormatItem(
                    "3dp",
                    MeshFormatEnum.k3dp));

            _cbMeshFormat.Items.Add(
                new MeshFormatItem(
                    "Fbx",
                    MeshFormatEnum.kFbx));

            _cbMeshFormat.Items.Add(
                new MeshFormatItem(
                    "Fysc",
                    MeshFormatEnum.kFysc));

            _cbMeshFormat.Items.Add(
                new MeshFormatItem(
                    "Ipm",
                    MeshFormatEnum.kIpm));

            _cbMeshFormat.Items.Add(
                new MeshFormatItem(
                    "Las",
                    MeshFormatEnum.kLas));

            _cbMeshFormat.Items.Add(
                new MeshFormatItem(
                    "Obj",
                    MeshFormatEnum.kObj));

            _cbMeshFormat.Items.Add(
                new MeshFormatItem(
                    "Rcm",
                    MeshFormatEnum.kRcm));

            _cbMeshFormat.Items.Add(
                new MeshFormatItem(
                    "Rcs",
                    MeshFormatEnum.kRcs));

            _cbMeshQuality.SelectedIndex = 0;
            _cbMeshQuality.IsEnabled = false;

            switch (format)
            {
                case MeshFormatEnum.k3dp:
                    _cbMeshFormat.SelectedIndex = 0;
                    break;

                case MeshFormatEnum.kFbx:
                    _cbMeshFormat.SelectedIndex = 1;
                    break;

                case MeshFormatEnum.kFysc:
                    _cbMeshFormat.SelectedIndex = 2;
                    break;

                case MeshFormatEnum.kIpm:
                    _cbMeshFormat.SelectedIndex = 3;
                    break;

                case MeshFormatEnum.kLas:
                    _cbMeshFormat.SelectedIndex = 4;
                    break;

                case MeshFormatEnum.kObj:
                    _cbMeshFormat.SelectedIndex = 5;
                    break;

                case MeshFormatEnum.kRcm:
                    _cbMeshFormat.SelectedIndex = 6;
                    break;

                case MeshFormatEnum.kRcs:
                    _cbMeshFormat.SelectedIndex = 7;
                    break;
            }
        }

        public string SceneName
        {
            get
            {
                return _tbSceneName.Text;
            }
        }

        public MeshQualityEnum MeshQuality
        {
            get
            { 
                var item = _cbMeshQuality.SelectedItem
                    as MeshQualityItem;

                return item.Value;
            }
        }

        public MeshFormatEnum MeshFormat
        {
            get
            {
                var item = _cbMeshFormat.SelectedItem
                    as MeshFormatItem;

                return item.Value;
            }
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            DialogResult = true;

            if (_saveSettings)
            {
                _qualityIndex = _cbMeshQuality.SelectedIndex;

                _formatIndex = _cbMeshFormat.SelectedIndex;
            }

            Close();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tbSceneName_TextChanged(object sender, EventArgs e)
        {
            if (_tbSceneName.Text.Length == 0)
            {
                bOK.IsEnabled = false;
            }
            else
            {
                bOK.IsEnabled = true;
            }
        }
    }
}