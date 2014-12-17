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
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
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
using Autodesk.ADN.Toolkit.OAuth;
using Autodesk.ADN.Toolkit.ReCap;
using Autodesk.ADN.Toolkit.ReCap.DataContracts;

namespace Autodesk.ADN.ReCapWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool _isLoggedIn = false;

        AdnOAuthConnector _connector;

        AdnReCapClient _reCapClient;

        public MainWindow()
        {
            InitializeComponent();

            TreeViewScenes.OnLogError += 
                OnLogError;

            TreeViewScenes.OnLogReCapError +=
               OnLogReCapError;

            TreeViewScenes.OnLogMessage +=
               OnLogMessage;

            Login();
        }

        void OnLogReCapError(ReCapError error)
        {
            LogReCapError(error);
        }

        void OnLogError(string error)
        {
            LogError(error);
        }

        void OnLogMessage(string msg)
        {
            LogMessage(msg);
        }

        private void OnLoginError(string problem, string msg)
        {
            LogError("Authentification Error \n" +
                problem + ": " + msg);

            _isLoggedIn = false;
        }

        private async void Login()
        {
            bool isNetworkAvailable =
               NetworkInterface.GetIsNetworkAvailable();

            if (!isNetworkAvailable)
            {
                LogError("Network Error \n" +
                    "Check your network connection and try again...");

                return;
            }

            //Already login? -> do a refresh 
            if (_isLoggedIn)
            {
                _isLoggedIn = await _connector.DoRefreshAsync();
            }
            else
            {
                _connector = new AdnOAuthConnector(
                   UserSettings.OAUTH_URL,
                   UserSettings.CONSUMER_KEY,
                   UserSettings.CONSUMER_SECRET);

                _connector.LoginViewMode = LoginViewModeEnum.iFrame;

                _connector.OnError += OnLoginError;

                _isLoggedIn = await _connector.DoLoginAsync();
            }

            if (_isLoggedIn)
            {
                LogMessage("Logged in as " + _connector.UserName, true, string.Empty);

                _reCapClient = new AdnReCapClient(
                    UserSettings.RECAP_URL,
                    UserSettings.RECAP_CLIENTID,
                    _connector.ConsumerKey,
                    _connector.ConsumerSecret,
                    _connector.AccessToken,
                    _connector.AccessTokenSecret);

                var versionResponse = await _reCapClient.GetVersionAsync();

                if (!versionResponse.IsOk())
                {
                    LogReCapError(versionResponse.Error);
                }
                else
                {
                    LogMessage("Service version: " +
                        versionResponse.Version);
                }

                var timeResponse = await _reCapClient.GetServerTimeAsync();

                if (!timeResponse.IsOk())
                {
                    LogReCapError(timeResponse.Error);
                }
                else
                {
                    LogMessage("Server time: " +
                        timeResponse.Date.ToLongDateString() + " - " +
                        timeResponse.Date.ToLongTimeString());
                }

                var res = await TreeViewScenes.LoadScenes(
                    _reCapClient);
            }
            else
            {
                LogError("Authentication failed...");
            }
        }

        private async void LogOut()
        {
            bool res = await _connector.DoLogoutAsync();

            _connector = null;

            _isLoggedIn = false;

            LogMessage("Logged out ...");
        }

        /////////////////////////////////////////////////////////////////////////////////
        // UI Utilities
        //
        /////////////////////////////////////////////////////////////////////////////////
        string GetTimeStamp()
        {
            DateTime now = DateTime.Now;

            return now.ToString(
                "dd/MM/yyyy - HH:mm:ss",
                CultureInfo.InvariantCulture);
        }

        void AppendText(string text, Brush color, bool bold)
        {
            var doc = _logger.Document;

            Paragraph paragraph = new Paragraph();
            paragraph.Foreground = color;
            paragraph.LineHeight = 5;

            Inline content = new Run(text);
                
            if(bold)
            {
                content = new Bold(content);
            }

            paragraph.Inlines.Add(content);

            doc.Blocks.Add(paragraph);
        }

        void LogMessage(
            string msg, 
            bool appendDateTime = true,
            string separator = "\n")
        {
            if (appendDateTime)
            {
                AppendText(separator + GetTimeStamp(), Brushes.Blue, true);
            }

            AppendText(msg, Brushes.Black, false);

            _logger.ScrollToEnd();
        }

        void LogError(
            string msg,
            bool appendDateTime = true,
            string separator = "\n")
        {
            if (appendDateTime)
            {
                AppendText(separator + GetTimeStamp(), Brushes.Blue, true);
            }

            AppendText(msg, Brushes.Red, false);

            _logger.ScrollToEnd();
        }

        void LogReCapError(ReCapError error)
        {
            switch (error.Msg)
            {
                case "System.Net.HttpStatusCode":

                    LogError("Http Error \n" +
                        "Status Code = " +
                       error.StatusCode.ToString());

                    break;

                case "System.Exception":

                    LogError("Exception \n" +
                        error.Exception.Message);

                    break;

                case "Deserialization.Exception":

                    LogError("Deserialization Exception");

                    foreach (var jsonErr in error.JsonErrors)
                    {
                        LogError("\nMember: " + jsonErr.ErrorContext.Member, false);
                        LogError(jsonErr.ErrorContext.Error.Message, false);
                    }

                    break;

                default:

                    LogError("ReCap Server Error \n" +
                        error.Msg);

                    break;
            }
        }
    }
}
