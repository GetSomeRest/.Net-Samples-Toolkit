/////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Autodesk, Inc. All rights reserved 
// Written by Philippe Leefsma 2013 - ADN/Developer Technical Services
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
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace Autodesk.ADN.Toolkit.OAuth
{
    partial class LoginForm : Form
    {
        AdnLoginControl _loginControl;

        public LoginForm(Uri loginUri, Uri targetUri)
        {
            InitializeComponent();

            DialogResult = DialogResult.Cancel;

            _loginControl = new AdnLoginControl(
                loginUri,
                targetUri);

            _loginControl.OnLoginSuccess += OnLoginSuccess;

            ElementHost elementHost = new ElementHost();

            elementHost.Dock = DockStyle.Fill;

            elementHost.Child = _loginControl;

            this.Controls.Add(elementHost);
        }

        void OnLoginSuccess(Uri resultUri)
        {
            DialogResult = DialogResult.OK;

            Close();
        }
    }
}
