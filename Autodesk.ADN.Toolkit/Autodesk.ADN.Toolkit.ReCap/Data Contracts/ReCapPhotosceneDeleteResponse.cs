using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Autodesk.ADN.Toolkit.ReCap.DataContracts
{
    /////////////////////////////////////////////////////////////////////////////////
    // ReCap Photoscene delete Response
    //
    /////////////////////////////////////////////////////////////////////////////////
    public class ReCapPhotosceneDeleteResponse : ReCapResponseBase
    {
        [JsonProperty(PropertyName = "Photoscene")]
        [JsonConverter(typeof(ReCapDeletedResourceConverter))]
        public int NumberOfDeletedResources
        {
            get;
            private set;
        }

        [JsonConstructor]
        public ReCapPhotosceneDeleteResponse(
            int deleted)
        {
            NumberOfDeletedResources = deleted;
        }

        public ReCapPhotosceneDeleteResponse()
        {

        }
    }
}
