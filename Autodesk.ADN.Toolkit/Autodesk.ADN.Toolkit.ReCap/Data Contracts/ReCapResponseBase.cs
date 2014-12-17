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
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Autodesk.ADN.Toolkit.ReCap.DataContracts
{
    /////////////////////////////////////////////////////////////////////////////////
    // Base class for ReCap Responses
    //
    /////////////////////////////////////////////////////////////////////////////////
    public class ReCapResponseBase
    {
        public string Usage
        {
            get;
            private set;
        }

        public string Resource
        {
            get;
            private set;
        }

        public ReCapError Error
        {
            get;
            set;
        }

        public bool IsOk()
        {
            return (Error == null);
        }

        [JsonConstructor]
        public ReCapResponseBase(
            string usage,
            string resource,
            ReCapError error)
        {
            Usage = usage;
            Resource = resource;
            Error = error;
        }

        public ReCapResponseBase()
        { 
        
        }
    }

    /////////////////////////////////////////////////////////////////////////////////
    // ReCap Version Response
    //
    /////////////////////////////////////////////////////////////////////////////////
    public class ReCapVersionResponse : ReCapResponseBase
    {
        public string Version
        {
            get;
            private set;
        }

        [JsonConstructor]
        public ReCapVersionResponse(
            string version)
        {
            Version = version;
        }

        public ReCapVersionResponse()
        {

        }
    }

    /////////////////////////////////////////////////////////////////////////////////
    // ReCap Server Time Response
    //
    /////////////////////////////////////////////////////////////////////////////////
    public class ReCapServerTimeResponse : ReCapResponseBase
    {
        public DateTime Date
        {
            get;
            private set;
        }

        [JsonConstructor]
        public ReCapServerTimeResponse(
            DateTime date)
        {
            Date = date;
        }

        public ReCapServerTimeResponse()
        {

        }
    }
}
