using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Autodesk.ADN.Toolkit.ReCap.DataContracts
{
    /////////////////////////////////////////////////////////////////////////////////
    // Private classes for deserialization
    //
    /////////////////////////////////////////////////////////////////////////////////
    class ReCapStringConverter : JsonConverter
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

                    var obj = serializer.Deserialize<object>(reader);
                    return string.Empty;

                default:

                    try
                    {
                        var result = serializer.Deserialize<string>(reader);

                        return result;
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
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

    class ReCapDoubleConverter : JsonConverter
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

                    var obj = serializer.Deserialize<object>(reader);
                    return 0.0;

                default:

                    try
                    {
                        var result = serializer.Deserialize<double>(reader);

                        return result;
                    }
                    catch //(Exception ex)
                    {
                        return 0.0;
                    }
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

    class ReCapIntConverter : JsonConverter
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

                    var obj = serializer.Deserialize<object>(reader);
                    return 0;

                default:

                    try
                    {
                        var result = serializer.Deserialize<int>(reader);

                        return result;
                    }
                    catch //(Exception ex)
                    {
                        return 0;
                    }
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

    class ReCapUriConverter : JsonConverter
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

                    var obj = serializer.Deserialize<object>(reader);
                    return null;

                default:

                    try
                    {
                        var result = serializer.Deserialize<Uri>(reader);

                        return result;
                    }
                    catch //(Exception ex)
                    {
                        return null;
                    }
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

    class ReCapPhotoscenesConverter : JsonConverter
    {
        class ReCapPhotosceneCollection
        {
            [JsonConverter(typeof(ReCapPhotosceneCollectionConverter))]
            public List<ReCapPhotoscene> Photoscene
            {
                get;
                set;
            }
        }

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
            var result = serializer.Deserialize<ReCapPhotosceneCollection>(reader);

            return result.Photoscene;
        }

        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    class ReCapPhotosceneCollectionConverter : JsonConverter
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

                    var result = new List<ReCapPhotoscene>();

                    var scene = serializer.Deserialize<ReCapPhotoscene>(reader);

                    result.Add(scene);

                    return result;

                case JsonToken.StartArray:

                    return serializer.Deserialize<List<ReCapPhotoscene>>(reader);

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

    class ReCapPhotosceneContainerConverter : JsonConverter
    {
        class ReCapPhotosceneContainer
        {
            public ReCapPhotoscene Photoscene
            {
                get;
                set;
            }
        }

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
            var result = serializer.Deserialize<ReCapPhotosceneContainer>(reader);

            return result.Photoscene;
        }

        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    class ReCapDeletedResourceConverter : JsonConverter
    {
        class ReCapPhotoSceneDeletedResource
        {
            public int Deleted
            {
                get;
                set;
            }
        }

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
            var result = serializer.Deserialize<ReCapPhotoSceneDeletedResource>(reader);

            return result.Deleted;
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
