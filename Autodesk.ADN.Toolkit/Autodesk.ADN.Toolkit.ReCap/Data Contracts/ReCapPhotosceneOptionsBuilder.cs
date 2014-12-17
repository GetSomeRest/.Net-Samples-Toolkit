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

namespace Autodesk.ADN.Toolkit.ReCap.DataContracts
{
    /////////////////////////////////////////////////////////////////////////////////
    // ReCap Photoscene options builder
    //
    /////////////////////////////////////////////////////////////////////////////////
    public class ReCapPhotosceneOptionsBuilder
    {
        private string _meshQuality;
        private string _meshFormat;
        private string _meshBBox;

        private Dictionary<string, string> _metaDataMap;

        public ReCapPhotosceneOptionsBuilder(
            MeshQualityEnum meshQuality,
            MeshFormatEnum meshFormat)
        {
            _metaDataMap = new Dictionary<string, string>();

            _meshQuality = meshQuality.ToReCapString();

            _meshFormat = meshFormat.ToReCapString();
        }

        public string Callback
        {
            set;
            private get;
        }

        public string ReferenceId
        {
            set;
            private get;
        }

        public void SetMeshBox(
            double minX, double minY, double minZ,
            double maxX, double maxY, double maxZ)
        {
            _meshBBox = string.Format(
                "{0},{1},{2},{3},{4},{5},{6},",
                minX, minY, minZ,
                maxX, maxY, maxZ);
        }

        public void AddMetadata(string name, string value)
        {
            if (!_metaDataMap.ContainsKey(name))
            {
                _metaDataMap.Add(name, value);
            }
            else
            {
                _metaDataMap[name] = value;
            }
        }

        public Dictionary<string, string> ToPhotosceneOptions()
        {
            var options = new Dictionary<string, string>();

            options.Add("format", _meshFormat);
            options.Add("meshquality", _meshQuality);

            if (Callback != null)
                options.Add("callback", Callback);

            if (ReferenceId != null)
                options.Add("refPID", ReferenceId);

            if (_meshBBox != null)
                options.Add("meshbbox", _meshBBox);

            foreach (var entry in _metaDataMap)
            {
                options.Add(entry.Key, entry.Value);
            }

            return options;
        }
    }
}
