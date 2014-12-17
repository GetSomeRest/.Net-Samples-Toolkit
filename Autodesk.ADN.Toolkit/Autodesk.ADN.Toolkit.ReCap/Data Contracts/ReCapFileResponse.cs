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
    // ReCap File
    //
    /////////////////////////////////////////////////////////////////////////////////
    public class ReCapFile
    {
        public string Filename
        {
            get;
            private set;
        }

        public string FileId
        {
            get;
            private set;
        }

        public int FileSize
        {
            get;
            private set;
        }

        public string FileType
        {
            get;
            private set;
        }

        [JsonConverter(typeof(ReCapStringConverter))]
        public string Msg
        {
            get;
            private set;
        }

        public Uri FileLink
        {
            get;
            private set;
        }

        [JsonConstructor]
        public ReCapFile(
            string filename,
            string fileId,
            int fileSize,
            string fileType,
            string msg,
            Uri fileLink)
        { 
            Filename = filename;
            FileId = fileId;
            FileSize = fileSize;
            FileType = fileType;
            Msg = msg;
            FileLink = fileLink;
        }

        public ReCapFile()
        { 
        
        }
    }

    /////////////////////////////////////////////////////////////////////////////////
    // ReCap File Response
    //
    /////////////////////////////////////////////////////////////////////////////////
    public class ReCapFileResponse : ReCapResponseBase
    {
        public string PhotosceneId
        {
            get;
            private set;
        }

        [JsonConverter(typeof(FilesConverter))]
        public List<ReCapFile> Files
        {
            get;
            private set;        
        }

        [JsonConstructor]
        public ReCapFileResponse(
            string photosceneId,
            List<ReCapFile> files)
        {
            PhotosceneId = photosceneId;

            Files = (files != null ?
                files : 
                new List<ReCapFile>());
        }

        public ReCapFileResponse()
        {

        }
    }

    /////////////////////////////////////////////////////////////////////////////////
    // Private classes for deserialization
    //
    /////////////////////////////////////////////////////////////////////////////////
    class FilesConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            var result = serializer.Deserialize<ReCapFileCollection>(reader);

            if(result != null)
                return result.File;

            return null;
        }

        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    class ReCapFileCollection
    {
        [JsonConverter(typeof(FileCollectionConverter))]
        public List<ReCapFile> File
        {
            get;
            set;
        }

        public ReCapFileCollection()
        {
            File = new List<ReCapFile>();
        }

        [JsonConstructor]
        public ReCapFileCollection(
            List<ReCapFile> file)
        {
            File = (file != null ?
                file :
                new List<ReCapFile>());
        }
    }

    class FileCollectionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.StartObject:

                    var result = new List<ReCapFile>();

                    var file = serializer.Deserialize<ReCapFile>(reader);

                    result.Add(file);

                    return result;

                case JsonToken.StartArray:

                    return serializer.Deserialize<List<ReCapFile>>(reader);

                default:
                    return null;
            }
        }

        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
