/////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Autodesk, Inc. All rights reserved 
// Written by Philippe Leefsma 2014 - ADN/Developer Technical Services
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted, 
// provided that the above copyright notice appears in all copies and 
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting 
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS. 
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC. 
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
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
using Autodesk.ADN.Toolkit.ReCap;
using Autodesk.ADN.Toolkit.ReCap.DataContracts;
using Autodesk.ADN.Toolkit.UI;

namespace Autodesk.ADN.ReCapWPF
{
    public delegate void OnLogReCapErrorHandler(
       ReCapError error);

    public delegate void OnLogMessageHandler(
        string msg);

    /// <summary>
    /// Interaction logic for TreeViewCtrl.xaml
    /// </summary>
    public partial class TreeViewCtrl : UserControl
    {
        ReCapTreeItem _rootNode = null;

        AdnReCapClient _reCapClient;

        private Dictionary<string, SceneProgressDlg>
          _progressMap;

        public ReCapTreeItem RootNode
        {
            get
            {
                return _rootNode;
            }
        }

        private ReCapTreeItem SelectedItem
        {
            get
            {
                return _treeView.SelectedItem 
                    as ReCapTreeItem;
            }
        }

        public TreeViewCtrl()
        {
            InitializeComponent();

            _propertyGrid.ShowSearchBox = false;

            _progressMap = new Dictionary<string, SceneProgressDlg>();
        }

        ReCapTreeItem AddRootNode(string name)
        {
            _rootNode = new ReCapTreeItem(
                name,
                Properties.Resources.folder_open);

            _rootNode.PropertyChanged +=
                new PropertyChangedEventHandler(
                    rootNode_PropertyChanged);

            ObservableCollection<ReCapTreeItem> nodes =
                new ObservableCollection<ReCapTreeItem>();

            nodes.Add(_rootNode);

            _treeView.ItemsSource = nodes;

            return _rootNode;
        }

        void rootNode_PropertyChanged(
            object sender, PropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Expand()
        {
            var generator = _treeView.ItemContainerGenerator;

            var item = generator.ContainerFromItem(_treeView.Items[0])
                as TreeViewItem;

            item.IsExpanded = true;
        }

        private ReCapTreeItem GetNodeById(string photosceneId)
        {
            var node = RootNode.Children.Where(
                item => item.Photoscene.PhotosceneId == photosceneId).
                    FirstOrDefault();

            return node;
        }

        private void OnTreeViewSelectedItemChanged(
            object sender, 
            RoutedPropertyChangedEventArgs<object> e)
        {
            var item = (ReCapTreeItem)((TreeView)sender).SelectedItem;

            if (item != null)
            {
                _propertyGrid.SelectedObject = item.Photoscene;

                if (item.Photoscene != null)
                {
                    _treeView.ContextMenu = _treeView.Resources["PhotosceneCtx"]
                        as System.Windows.Controls.ContextMenu;
                }
                else
                {
                    _treeView.ContextMenu = _treeView.Resources["RootCtx"]
                       as System.Windows.Controls.ContextMenu;
                }
            }
        }

        public async Task<bool> LoadScenes(
            AdnReCapClient reCapClient)
        {
            _reCapClient = reCapClient;

            var rootNode = AddRootNode(
                "Photoscenes");

            var sceneListResponse = await
                _reCapClient.GetPhotosceneListAsync();

            if (!sceneListResponse.IsOk())
            {
                // We get Deserialization.Exception
                // if deleted scenes exist as Name 
                // is left empty

                if (sceneListResponse.Error.Msg !=
                    "Deserialization.Exception")
                {
                    OnLogReCapError(sceneListResponse.Error);

                    return false;
                }
            }

            foreach (var scene in sceneListResponse.Photoscenes)
            {
                if (!scene.Deleted)
                {
                    var node = new ReCapTreeItem(
                        scene,
                        Properties.Resources.file);

                    rootNode.AddNode(node);

                    Expand();
                }
            }

            List<Task> taskList = new List<Task>();

            foreach(var node in RootNode.Children)
            {
                taskList.Add(RetrieveSceneInfoAndUpdateNode(
                    node.Photoscene.PhotosceneId));
            }

            //await Task.WhenAll(taskList);

            return true;
        }

        private async Task RetrieveSceneInfoAndUpdateNode(string photosceneId)
        { 
            var scene = await RetrieveSceneInfo(photosceneId);

            if (scene != null)
            {
                var node = GetNodeById(photosceneId);

                node.Photoscene = scene;

                if (SelectedItem != null && 
                    SelectedItem.Photoscene != null &&
                    SelectedItem.Photoscene.PhotosceneId == photosceneId)
                {
                    _propertyGrid.SelectedObject = scene;
                }
            }
        }

        private async Task<ReCapPhotoscene> RetrieveSceneInfo(string photosceneId)
        {
            try
            {
                var scenePropsResponse = await _reCapClient.GetPhotoscenePropertiesAsync(
                    photosceneId);

                if (!scenePropsResponse.IsOk())
                {
                    OnLogReCapError(scenePropsResponse.Error);
                    return null;
                }

                ReCapPhotoscene scene = scenePropsResponse.Photoscene;

                var sceneProgResponse = await _reCapClient.GetPhotosceneProgressAsync(
                    photosceneId);

                if (!sceneProgResponse.IsOk())
                {
                    OnLogReCapError(sceneProgResponse.Error);
                    return null;
                }

                double progress = sceneProgResponse.Photoscene.Progress;
                string progressMsg = sceneProgResponse.Photoscene.ProgressMsg;

                Uri link = null;

                if (progress == 100.0)
                {
                    var sceneLinkResponse = await _reCapClient.GetPhotosceneLinkAsync(
                        scene.PhotosceneId,
                        MeshFormatEnumExtensions.FromString(
                            scene.ConvertFormat));

                    if (!sceneLinkResponse.IsOk())
                    {
                        OnLogReCapError(sceneLinkResponse.Error);
                    }
                    else
                    {
                        link = sceneLinkResponse.Photoscene.SceneLink;
                    }
                }

                return new ReCapPhotoscene(
                    scene.SceneName,
                    scene.PhotosceneId,
                    progressMsg,
                    progress,
                    link,
                    scene.FileSize,
                    scene.UserId,
                    scene.MeshQuality,
                    scene.ConvertFormat,
                    scene.ConvertStatus,
                    scene.ProcessingTime,
                    scene.Deleted,
                    scene.Files,
                    scene.Nb3dPoints,
                    scene.NbFaces,
                    scene.NbShots,
                    scene.NbStitchedShots,
                    scene.NbVertices);
            }
            catch
            {
                return null;
            }
        }

        void DownloadSceneResult(ReCapPhotoscene scene)
        {
            if (scene.SceneLink == null)
            {
                OnLogError("Scene Link unavailable ...");
                return;
            }

            string fileName = scene.SceneName + ".zip";

            fileName = UIHelper.GetValidFileName(fileName, '-');

            fileName = UIHelper.GetSaveFileName(fileName);

            if (fileName != string.Empty)
            {
                AdnFileDownloader fd = new AdnFileDownloader(
                    scene.SceneLink,
                    fileName);

                fd.OnDownloadFileCompleted +=
                    OnDownloadFileCompleted;

                fd.Download();
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        // Context Menu Handlers
        //
        /////////////////////////////////////////////////////////////////////////////////
        void DownloadSceneResult_Click(object sender, RoutedEventArgs e)
        {
            DownloadSceneResult(SelectedItem.Photoscene);
        }

        async void DownloadSceneResultAs_Click(object sender, RoutedEventArgs e)
        {
            var scene = SelectedItem.Photoscene;

            SceneSettingsDlg settingsDlg = new SceneSettingsDlg(
                scene.SceneName,
                scene.MeshQuality,
                MeshFormatEnumExtensions.FromString(scene.ConvertFormat));

            settingsDlg.ShowDialog();

            if (!settingsDlg.DialogResult.HasValue || !settingsDlg.DialogResult.Value)
                return;

            var linkResult = await _reCapClient.GetPhotosceneLinkAsync(
                scene.PhotosceneId,
                settingsDlg.MeshFormat);

            if (!linkResult.IsOk())
            {
                OnLogReCapError(linkResult.Error);
                return;
            }

            scene = linkResult.Photoscene;

            if (scene.Progress != 100.0)
            {
                OnLogMessage("Start processing for scene: " + scene.SceneName);

                ShowProgressDlg(scene.SceneName, scene.PhotosceneId);
            }
            else
            {
                DownloadSceneResult(scene);
            }
        }

        async void DownloadSceneImages_Click(object sender, RoutedEventArgs e)
        {
            var folderPath = UIHelper.FolderSelect("Select folder");

            if (folderPath == null)
                return;

            var scene = SelectedItem.Photoscene;

            var propsResult = await _reCapClient.GetPhotoscenePropertiesAsync(
               scene.PhotosceneId);

            if (!propsResult.IsOk())
            {
                OnLogReCapError(propsResult.Error);
                return;
            }

            var files = propsResult.Photoscene.Files;

            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileLinkResponse = await _reCapClient.GetFileLinkAsync(
                        file.FileId);

                    if (!fileLinkResponse.IsOk())
                    {
                        OnLogReCapError(fileLinkResponse.Error);
                        continue;
                    }

                    AdnFileDownloader fd = new AdnFileDownloader(
                        fileLinkResponse.Files[0].FileLink,
                        System.IO.Path.Combine(folderPath, file.Filename));

                    fd.OnDownloadFileCompleted +=
                        OnDownloadFileCompleted;

                    var res = await fd.DownloadAsync();
                }
            }
        }

        void OnDownloadFileCompleted(
            Uri url,
            string location,
            AsyncCompletedEventArgs e,
            TimeSpan elapsed)
        {
            string dt = elapsed.ToString(@"hh\:mm\:ss");

            OnLogMessage("File download completed in " + dt + "\n" + location);
        }

        async void AddImages_Click(object sender, RoutedEventArgs e)
        {
            var scene = SelectedItem.Photoscene;

            string[] files = UIHelper.FileSelect(
                  "Select Pictures",
                  "(*.jpg)|*.jpg",
                  true);

            if (files == null)
                return;

            var uploadResultArray = await _reCapClient.UploadFilesAsync(
                scene.PhotosceneId,
                files);

            foreach (var uploadResult in uploadResultArray)
            {
                if (!uploadResult.IsOk())
                {
                    OnLogReCapError(uploadResult.Error);               
                }            
            }

            OnLogMessage("Files uploaded for scene: " + scene.SceneName);
        }

        async void ReprocessScene_Click(object sender, RoutedEventArgs e)
        {
            var scene = SelectedItem.Photoscene;

            var processResult = await _reCapClient.ProcessPhotosceneAsync(
               scene.PhotosceneId);

            if (!processResult.IsOk())
            {
                OnLogReCapError(processResult.Error);
                return;
            }

            OnLogMessage("Start processing for scene: " + scene.SceneName);

            ShowProgressDlg(scene.SceneName, scene.PhotosceneId);
        }

        async void RefreshScene_Click(object sender, RoutedEventArgs e)
        {
            var scene = SelectedItem.Photoscene;

            var sceneWithInfo = await RetrieveSceneInfo(
                scene.PhotosceneId);

            if (sceneWithInfo != null)
            {
                if (_propertyGrid.SelectedObject == scene)
                {
                    _propertyGrid.SelectedObject = GetNodeById(
                        scene.PhotosceneId).Photoscene;
                }

                OnLogMessage("Refreshed data for scene: " +
                    scene.SceneName);
            }
            else
            {
                OnLogError("Failed to refresh data for scene: " +
                    scene.SceneName);
            }
        }

        async void DeleteScene_Click(object sender, RoutedEventArgs e)
        {
            var scene = SelectedItem.Photoscene;

            var deleteResult = await _reCapClient.DeletePhotosceneAsync(
                scene.PhotosceneId);

            if (!deleteResult.IsOk())
            {
                OnLogReCapError(deleteResult.Error);
                return;
            }
            else
            {
                OnLogMessage("Deleted Scene: " + scene.SceneName + "\n" +
                    "Number of deleted resources: " +
                    deleteResult.NumberOfDeletedResources);

                RootNode.RemoveNode(SelectedItem);
            }
        }

        async void AddScene_Click(object sender, RoutedEventArgs e)
        {
            SceneSettingsDlg settingsDlg = new SceneSettingsDlg();

            settingsDlg.Owner = this.Parent as Window;

            settingsDlg.ShowDialog();

            if (!settingsDlg.DialogResult.HasValue || !settingsDlg.DialogResult.Value)
                return;

            ReCapPhotosceneOptionsBuilder options =
                new ReCapPhotosceneOptionsBuilder(
                    settingsDlg.MeshQuality,
                    settingsDlg.MeshFormat);

            var id = await CreateNewPhotoscene(
                settingsDlg.SceneName,
                options);

            if (id != string.Empty)
            {
                ShowProgressDlg(settingsDlg.SceneName, id);
            }
        }

        async void OnSceneCompleted(ReCapPhotosceneResponse response)
        {
            var sceneWithInfo = await RetrieveSceneInfo(
                response.Photoscene.PhotosceneId);

            string id = response.Photoscene.PhotosceneId;

            if (_progressMap.ContainsKey(id))
            {
                _progressMap[id].Close();

                _progressMap.Remove(id);
            }

            if (sceneWithInfo != null)
            {
                OnLogMessage("Scene completed: " +
                    sceneWithInfo.SceneName);

                var node = GetNodeById(id);

                if (node != null)
                {
                    node.Photoscene = sceneWithInfo;
                }
                else
                {
                    RootNode.AddNode(
                        new ReCapTreeItem(
                            sceneWithInfo,
                            Properties.Resources.file));
                }
            }
        }

        async Task<string> CreateNewPhotoscene(
            string sceneName,
            ReCapPhotosceneOptionsBuilder options)
        {
            string[] files = UIHelper.FileSelect(
                   "Select Pictures",
                   "(*.jpg)|*.jpg",
                   true);

            if (files == null)
                return string.Empty;

            // Step 1 - Create a new Photoscene

            var createResult = await _reCapClient.CreatePhotosceneAsync(
                sceneName,
                options);

            if (!createResult.IsOk())
            {
                OnLogReCapError(createResult.Error);

                return string.Empty;
            }

            OnLogMessage("New scene created: " + sceneName +
                " [Id: " + createResult.Photoscene.PhotosceneId + "]");

            var sceneWithInfo = await RetrieveSceneInfo(
                createResult.Photoscene.PhotosceneId);

            if (sceneWithInfo != null)
            {
                RootNode.AddNode(
                   new ReCapTreeItem(
                       sceneWithInfo,
                       Properties.Resources.file));
            }

            // Step 2 - Upload pictures

            OnLogMessage("Uploading " + files.Length + 
                " images for scene: " + sceneName);

            string photosceneId = createResult.Photoscene.PhotosceneId;

            var uploadResultArray = await _reCapClient.UploadFilesAsync(
                photosceneId,
                files);

            foreach (var uploadResult in uploadResultArray)
            {
                if (!uploadResult.IsOk())
                {
                    OnLogReCapError(uploadResult.Error);

                    //return;
                }
            }

            OnLogMessage("Files uploaded for scene: " + sceneName);

            // Step 3 - start processing the Photoscene

            var processResult = await _reCapClient.ProcessPhotosceneAsync(
                photosceneId);

            if (!processResult.IsOk())
            {
                OnLogReCapError(processResult.Error);

                return photosceneId;
            }

            OnLogMessage("Start processing for scene: " + sceneName);

            return photosceneId;
        }

        void ShowProgressDlg(string sceneName, string photoSceneId)
        {
            ReCapSceneProgressNotifier notifier =
                  new ReCapSceneProgressNotifier(
                      _reCapClient,
                      photoSceneId,
                      5000);

            SceneProgressDlg progressDlg =
                new SceneProgressDlg(notifier, sceneName);

            progressDlg.Owner = this.Parent as Window;

            progressDlg.Show(); 

            notifier.OnSceneProgressCompleted +=
                new OnSceneProgressCompletedHandler(
                    OnSceneCompleted);

            notifier.Activate();

            _progressMap.Add(photoSceneId, progressDlg);
        }

        public event OnLogReCapErrorHandler
           OnLogReCapError = null;

        public event OnLogMessageHandler
            OnLogError = null;

        public event OnLogMessageHandler
            OnLogMessage = null;
    }

    public class ReCapTreeItem : INotifyPropertyChanged
    {
        Bitmap _image;

        ReCapPhotoscene _scene;

        ObservableCollection<ReCapTreeItem> _children =
            new ObservableCollection<ReCapTreeItem>();

        public ReCapTreeItem(string name, Bitmap image)
        {
            Name = name;

            _image = image;
        }

        public ReCapTreeItem(ReCapPhotoscene scene, Bitmap image)
        {
            _scene = scene;

            _image = image;

            Name = scene.SceneName;
        }

        public ReCapPhotoscene Photoscene
        {
            get
            {
                return _scene;
            }
            set
            {
                _scene = value;
            }
        }

        public ImageSource Image
        {
            get
            {
                return BitmapConverter.ToBitmapSource(_image);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(
                    this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }

        private string _name;

        public string Name
        {
            get 
            {
                return "  " + _name; 
            }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public ObservableCollection<ReCapTreeItem> Children
        {
            get 
            {
                return _children; 
            }
            set
            {
                _children = value;

                OnPropertyChanged("Children");
            }
        }

        public void AddNode(ReCapTreeItem node)
        {
            Children.Add(node);

            OnPropertyChanged("Children");
        }

        public void RemoveNode(ReCapTreeItem node)
        {
            Children.Remove(node);

            OnPropertyChanged("Children");
        }
    }
}
