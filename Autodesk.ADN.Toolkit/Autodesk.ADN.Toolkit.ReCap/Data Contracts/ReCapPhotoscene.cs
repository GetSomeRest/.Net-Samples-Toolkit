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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp.Contrib;

namespace Autodesk.ADN.Toolkit.ReCap.DataContracts
{
    /////////////////////////////////////////////////////////////////////////////////
    // ReCap Photoscene
    //
    /////////////////////////////////////////////////////////////////////////////////
    public class ReCapPhotoscene
    {
        private string Name;

        [DisplayName("Scene Name")]
        public string SceneName
        {
            get
            {
                return (Name != null ? HttpUtility.UrlDecode(Name) : string.Empty);
            }
        }

        [DisplayName("Photoscene Id")]
        public string PhotosceneId
        {
            get;
            private set;
        }

        [JsonConverter(typeof(ReCapStringConverter))]
        [DisplayName("Progress Message")]
        public string ProgressMsg
        {
            get;
            private set;
        }

        [JsonConverter(typeof(ReCapDoubleConverter))]
        [DisplayName("Progress")]
        public double Progress
        {
            get;
            private set;
        }

        [JsonConverter(typeof(ReCapUriConverter))]
        [DisplayName("Scene Link")]
        public Uri SceneLink
        {
            get;
            private set;
        }

        [JsonConverter(typeof(ReCapIntConverter))]
        [DisplayName("File Size")]
        public int FileSize
        {
            get;
            private set;
        }

        [JsonConverter(typeof(ReCapStringConverter))]
        [DisplayName("User Id")]
        public string UserId
        {
            get;
            private set;
        }

        [JsonConverter(typeof(ReCapIntConverter))]
        [DisplayName("Mesh Quality")]
        public int MeshQuality
        {
            get;
            private set;
        }

        [JsonConverter(typeof(ReCapIntConverter))]
        [DisplayName("3d Points")]
        public int Nb3dPoints
        {
            get;
            private set;
        }

        [JsonConverter(typeof(ReCapIntConverter))]
        [DisplayName("Faces")]
        public int NbFaces
        {
            get;
            private set;
        }

        [JsonConverter(typeof(ReCapIntConverter))]
        [DisplayName("Shots")]
        public int NbShots
        {
            get;
            private set;
        }

        [JsonConverter(typeof(ReCapIntConverter))]
        [DisplayName("Stitched Shots")]
        public int NbStitchedShots
        {
            get;
            private set;
        }

        [JsonConverter(typeof(ReCapIntConverter))]
        [DisplayName("Vertices")]
        public int NbVertices
        {
            get;
            private set;
        }

        [JsonConverter(typeof(ReCapStringConverter))]
        [DisplayName("Convert Format")]
        public string ConvertFormat
        {
            get;
            private set;
        }

        [JsonConverter(typeof(ReCapStringConverter))]
        [DisplayName("Convert Status")]
        public string ConvertStatus
        {
            get;
            private set;
        }

        [JsonConverter(typeof(ReCapDoubleConverter))]
        [DisplayName("Processing Time")]
        public double ProcessingTime
        {
            get;
            private set;
        }

        [DisplayName("Deleted")]
        public bool Deleted
        {
            get;
            private set;
        }

        [JsonConverter(typeof(FilesConverter))]
        [DisplayName("Files")]
        public List<ReCapFile> Files
        {
            get;
            private set;
        }

        [DisplayName("Number of Resources")]
        public int NumberOfResources
        {
            get
            {
                return (Files != null ? Files.Count : 0);
            }
        }

        [JsonConstructor]
        public ReCapPhotoscene(
            string name,
            string photosceneId,
            string progressMsg,
            double progress,
            Uri sceneLink,
            int fileSize,
            string userId,
            int meshQuality,
            string convertFormat,
            string convertStatus,
            double processingTime,
            bool deleted,
            List<ReCapFile> files,
            int nb3dPoints,
            int nbFaces,
            int nbShots,
            int nbStitchedShots,
            int nbVertices)
        { 
            Name = name;
            PhotosceneId = photosceneId;
            ProgressMsg = progressMsg;
            Progress = progress;
            SceneLink = sceneLink;
            FileSize = fileSize;
            UserId = userId;
            MeshQuality = meshQuality;
            ConvertFormat = convertFormat;
            ConvertStatus = convertStatus;
            ProcessingTime = processingTime;
            Deleted = deleted;
            Files = files;   
            Nb3dPoints = nb3dPoints;
            NbFaces = nbFaces;
            NbShots = nbShots;
            NbStitchedShots = nbStitchedShots;
            NbVertices = nbVertices;
        }

        public ReCapPhotoscene()
        { 
        
        }
    }
}

