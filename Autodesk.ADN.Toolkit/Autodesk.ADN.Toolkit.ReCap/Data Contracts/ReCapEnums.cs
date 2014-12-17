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
    public enum MeshQualityEnum
    {
        kDraft,
        kStandard,
        kHigh
    }

    public static class MeshQualityEnumExtensions
    {
        public static string ToReCapString(this MeshQualityEnum value)
        {
            switch (value)
            {
                case MeshQualityEnum.kDraft:
                    return "7";

                case MeshQualityEnum.kStandard:
                    return "8";

                case MeshQualityEnum.kHigh:
                    return "9";

                default:
                    return "8";
            }
        }
    }

    public enum MeshFormatEnum
    {
        k3dp,
        kFbx,
        kIpm,
        kLas,
        kObj,
        kFysc,
        kRcs,
        kRcm,
        kInvalid
    }

    public static class MeshFormatEnumExtensions
    {
        public static string ToReCapString(this MeshFormatEnum value)
        {
            switch (value)
            {
                case MeshFormatEnum.k3dp:
                    return "3dp";

                case MeshFormatEnum.kFbx:
                    return "fbx";

                case MeshFormatEnum.kFysc:
                    return "fysc";

                case MeshFormatEnum.kIpm:
                    return "ipm";

                case MeshFormatEnum.kLas:
                    return "las";

                case MeshFormatEnum.kObj:
                    return "obj";

                case MeshFormatEnum.kRcm:
                    return "rcm";

                case MeshFormatEnum.kRcs:
                    return "rcs";

                case MeshFormatEnum.kInvalid:
                    return string.Empty;

                default:
                    return "3dp";
            }
        }

        public static MeshFormatEnum FromString(string value)
        {
            switch (value.ToLower())
            {
                case "3dp":
                    return MeshFormatEnum.k3dp;

                case "fbx":
                    return MeshFormatEnum.kFbx;

                case "fysc":
                    return MeshFormatEnum.kFysc;

                case "ipm":
                    return MeshFormatEnum.kIpm;

                case "las":
                    return MeshFormatEnum.kLas;

                case "obj":
                    return MeshFormatEnum.kObj;

                case "rcm":
                    return MeshFormatEnum.kRcm;

                case "rcs":
                    return MeshFormatEnum.kRcs;

                default:
                    return MeshFormatEnum.kInvalid;
            }
        }
    }
}
