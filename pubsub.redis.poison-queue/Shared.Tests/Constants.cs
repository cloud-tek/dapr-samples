using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Tests
{
    internal class Constants
    {
        public const string PubSubCloudEvent = @"
            {
                ""id"": ""49029da4-a1b2-4e7b-96fd-90c218d3ab43"",
                ""source"": ""service1"",
                ""type"": ""com.dapr.event.sent"",
                ""specversion"": ""1.0"",
                ""datacontenttype"": ""application/json"",
                ""data"": { ""id"": ""49029da4-a1b2-4e7b-96fd-90c218d3ab43"" },
                ""subject"": ""00-326af52d8421c644b58cfd229192c90e-11c21a2905dfe7bf-00"",
                ""topic"": ""ordertopic"",
                ""pubsubname"": ""pubsub""
            }
            ";
    }
}
