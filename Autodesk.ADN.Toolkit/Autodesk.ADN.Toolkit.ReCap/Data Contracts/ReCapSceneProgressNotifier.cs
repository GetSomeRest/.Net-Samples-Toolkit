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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autodesk.ADN.Toolkit.ReCap.DataContracts
{
    /////////////////////////////////////////////////////////////////////////////////
    // Delegates
    //
    /////////////////////////////////////////////////////////////////////////////////
    public delegate void OnSceneProgressChangedHandler(
        ReCapPhotosceneResponse response);

    public delegate void OnSceneProgressCompletedHandler(
        ReCapPhotosceneResponse response);

    public delegate void OnSceneProgressErrorHandler(
        ReCapError error);

    /////////////////////////////////////////////////////////////////////////////////
    // ReCap Scene progress notifier
    //
    /////////////////////////////////////////////////////////////////////////////////
    public class ReCapSceneProgressNotifier
    {
        private AdnReCapClient _client;
        private string _photosceneId;
        private int _pollingPeriod;
        private Control _syncCtrl;
        private Thread _worker;
             
        public ReCapSceneProgressNotifier(
            AdnReCapClient client, 
            string photosceneId,
            int pollingPeriod = 1000)
        {
            _worker = null;
            _client = client;       
            _photosceneId = photosceneId;
            _pollingPeriod = pollingPeriod;

            _syncCtrl = new Control();
            _syncCtrl.CreateControl();
        }

        public void Activate()
        {
            if (_worker == null)
            {
                _worker = new Thread(new ThreadStart(this.DoWork));
                _worker.Start();
            }
        }

        private async void DoWork()
        {
            double lastProgress = -1.0;

            ReCapPhotosceneResponse progressResult;

            while(true)
            {
                progressResult = await _client.GetPhotosceneProgressAsync(
                    _photosceneId);

                if (!progressResult.IsOk())
                {
                    if (OnSceneProgressError != null)
                    {
                        _syncCtrl.Invoke(OnSceneProgressError, new object[]
                        {                      
                            progressResult.Error
                        });
                    }

                    return;
                }

                if (OnSceneProgressChanged != null)
                {
                    if (lastProgress != progressResult.Photoscene.Progress)
                    {
                        _syncCtrl.Invoke(OnSceneProgressChanged, new object[]
                        {                      
                            progressResult
                        });
                    }
                }

                if(progressResult.Photoscene.ProgressMsg == "DONE")
                {
                    if (OnSceneProgressCompleted != null)
                    {
                        _syncCtrl.Invoke(OnSceneProgressCompleted, new object[]
                        {                      
                            progressResult
                        });
                    }

                    return;
                }
                else if (progressResult.Photoscene.ProgressMsg == "ERROR")
                {
                    return;
                }

                lastProgress = progressResult.Photoscene.Progress;

                Thread.Sleep(_pollingPeriod);
            }
        }

        public event OnSceneProgressChangedHandler 
            OnSceneProgressChanged = null;

        public event OnSceneProgressCompletedHandler 
            OnSceneProgressCompleted = null;

        public event OnSceneProgressErrorHandler 
            OnSceneProgressError = null;
    }
}
