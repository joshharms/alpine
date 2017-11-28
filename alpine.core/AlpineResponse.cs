using System;

using Newtonsoft.Json;

namespace alpine.core
{
    public class AlpineResponse
    {
        [JsonProperty( "meta" )]
        public MetaResponse Meta { get; set; }

        [JsonProperty( "data", NullValueHandling = NullValueHandling.Ignore )]
        public object Data { get; set; }
    }

    public class MetaResponse
    {
        [JsonProperty( "code" )]
        public int Code { get; set; }

        [JsonProperty( "time" )]
        public DateTime Time { get; set; }

        [JsonProperty( "error", NullValueHandling = NullValueHandling.Ignore )]
        public string Error { get; set; }

        [JsonProperty( "description", NullValueHandling = NullValueHandling.Ignore )]
        public string Description { get; set; }

        [JsonProperty( "alpine", NullValueHandling = NullValueHandling.Ignore )]
        public bool? Alpine { get; set; }
    }

    public class AlpineCreateResponse
    {
        public AlpineResponse Success( int code, object data = null )
        {
            AlpineResponse success = new AlpineResponse
            {
                Meta = new MetaResponse
                {
                    Code = code,
                    Time = DateTime.UtcNow
                },

                Data = data
            };

            return success;
        }

        public AlpineResponse Error( int code, string message, string messageExtra, bool alpine )
        {
            AlpineResponse error = new AlpineResponse();

            error.Meta = new MetaResponse
            {
                Code = code,
                Time = DateTime.UtcNow,
                Error = message,
                Description = messageExtra,
                Alpine = alpine
            };

            return error;
        }
    }
}
