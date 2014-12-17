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

namespace Autodesk.ADN.Toolkit.ReCap.DataContracts
{
    /////////////////////////////////////////////////////////////////////////////////
    // ReCap Photoscene list Response
    //
    /////////////////////////////////////////////////////////////////////////////////
    public class ReCapPhotosceneListResponse : ReCapResponseBase
    {
        [JsonConverter(typeof(ReCapPhotoscenesConverter))]
        public List<ReCapPhotoscene> Photoscenes
        {
            private set;
            get;
        }

        [JsonConstructor]
        public ReCapPhotosceneListResponse(
            List<ReCapPhotoscene> photoscenes)
        {
            Photoscenes = (photoscenes != null ? 
                photoscenes : 
                new List<ReCapPhotoscene>());
        }

        public ReCapPhotosceneListResponse()
        {
            Photoscenes = new List<ReCapPhotoscene>();
        }
    }
}
