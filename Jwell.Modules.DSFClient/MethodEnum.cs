﻿
using System.ComponentModel;

namespace Jwell.Modules.DSFClient
{
    public enum MethodEnum
    {
        [Description("GET")]
        GET,

        [Description("POST")]
        POST,

        [Description("PUT")]
        PUT,

        [Description("DELETE")]
        DELETE
    }
}
