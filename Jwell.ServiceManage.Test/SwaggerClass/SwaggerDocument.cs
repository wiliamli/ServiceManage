using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Jwell.ServiceManage.Test.SwaggerClass
{
    //public class SwaggerDocument
    //{
    //    public string Swagger { get; set; }

    //    public Info Info { get; set; }

    //    public string Host { get; set; }

    //    public string[] Schemes { get; set; }

    //    public IDictionary<string, PathItem> Paths { get; set; }

    //    public IDictionary<string, object> Definetions { get; set; }
    //}

    //public class Schema
    //{
    //    [JsonProperty("$ref")]
    //    public string @Ref { get; set; }
    //}

    //public class Info
    //{
    //    public string Title { get; set; }

    //    public string Version { get; set; }
    //}

    //public class PathItem
    //{
    //    [JsonProperty("get")]
    //    public Action GetAction { get; set; }

    //    [JsonProperty("post")]
    //    public Action PostAction { get; set; }
    //}

    //public class Action
    //{
    //    public string[] Tags { get; set; }

    //    public string Summary { get; set; }

    //    public string OperationId { get; set; }

    //    public string[] Consumes { get; set; }

    //    public string[] Produces { get; set; }

    //    public Parameter[] Parameters { get; set; }
    //}

    //public class Parameter
    //{
    //    public string Name { get; set; }

    //    public string In { get; set; }

    //    public string Description { get; set; }

    //    public bool Required { get; set; }

    //    [JsonProperty("schema", IsReference = false)]
    //    public Schema Schema { get; set; }

    //    [JsonProperty("type")]
    //    public string Type { get; set; }
    //}

    public class SwaggerDocument
    {
        public readonly string swagger = "2.0";

        public Info info;

        public string host;

        public string basePath;

        public IList<string> schemes;

        public IList<string> consumes;

        public IList<string> produces;

        public IDictionary<string, PathItem> paths;

        public IDictionary<string, Schema> definitions;

        public IDictionary<string, Parameter> parameters;

        public IDictionary<string, Response> responses;

        public IDictionary<string, SecurityScheme> securityDefinitions;

        public IList<IDictionary<string, IEnumerable<string>>> security;

        public IList<Tag> tags;

        public ExternalDocs externalDocs;

        public Dictionary<string, object> vendorExtensions = new Dictionary<string, object>();
    }

    public class Info
    {
        public string version;

        public string title;

        public string description;

        public string termsOfService;

        public Contact contact;

        public License license;

        public Dictionary<string, object> vendorExtensions = new Dictionary<string, object>();
    }

    public class Contact
    {
        public string name;

        public string url;

        public string email;
    }

    public class License
    {
        public string name;

        public string url;
    }

    public class PathItem
    {
        [JsonProperty("$ref")]
        public string @ref;

        public Operation get;

        public Operation put;

        public Operation post;

        public Operation delete;

        public Operation options;

        public Operation head;

        public Operation patch;

        public IList<Parameter> parameters;

        public Dictionary<string, object> vendorExtensions = new Dictionary<string, object>();
    }

    public class Operation
    {
        public IList<string> tags;

        public string summary;

        public string description;

        public ExternalDocs externalDocs;

        public string operationId;

        public IList<string> consumes;

        public IList<string> produces;

        public IList<Parameter> parameters;

        public IDictionary<string, Response> responses;

        public IList<string> schemes;

        public bool? deprecated;

        public IList<IDictionary<string, IEnumerable<string>>> security;

        public Dictionary<string, object> vendorExtensions = new Dictionary<string, object>();
    }

    public class Tag
    {
        public string name;

        public string description;

        public ExternalDocs externalDocs;

        public Dictionary<string, object> vendorExtensions = new Dictionary<string, object>();
    }

    public class ExternalDocs
    {
        public string description;

        public string url;
    }

    public class Parameter : PartialSchema
    {
        [JsonProperty("$ref")]
        public string @ref;

        public string name;

        public string @in;

        public string description;

        public bool? required;

        public Schema schema;
    }

    public class Schema
    {
        [JsonProperty("$ref")]
        public string @ref;

        public string format;

        public string title;

        public string description;

        public object @default;

        public int? multipleOf;

        public int? maximum;

        public bool? exclusiveMaximum;

        public int? minimum;

        public bool? exclusiveMinimum;

        public int? maxLength;

        public int? minLength;

        public string pattern;

        public int? maxItems;

        public int? minItems;

        public bool? uniqueItems;

        public int? maxProperties;

        public int? minProperties;

        public IList<string> required;

        public IList<object> @enum;

        public string type;

        public Schema items;

        public IList<Schema> allOf;

        public IDictionary<string, Schema> properties;

        public Schema additionalProperties;

        public string discriminator;

        public bool? readOnly;

        public Xml xml;

        public ExternalDocs externalDocs;

        public object example;

        public Dictionary<string, object> vendorExtensions = new Dictionary<string, object>();
    }

    public class PartialSchema
    {
        public string type;

        public string format;

        public PartialSchema items;

        public string collectionFormat;

        public object @default;

        public int? maximum;

        public bool? exclusiveMaximum;

        public int? minimum;

        public bool? exclusiveMinimum;

        public int? maxLength;

        public int? minLength;

        public string pattern;

        public int? maxItems;

        public int? minItems;

        public bool? uniqueItems;

        public IList<object> @enum;

        public int? multipleOf;

        public Dictionary<string, object> vendorExtensions = new Dictionary<string, object>();
    }

    public class Response
    {
        public string description;

        public Schema schema;

        public IDictionary<string, Header> headers;

        public object examples;

        public Dictionary<string, object> vendorExtensions = new Dictionary<string, object>();
    }

    public class Header : PartialSchema
    {
        public string description;
    }

    public class Xml
    {
        public string name;

        public string @namespace;

        public string prefix;

        public bool? attribute;

        public bool? wrapped;
    }

    public class SecurityScheme
    {
        public string type;

        public string description;

        public string name;

        public string @in;

        public string flow;

        public string authorizationUrl;

        public string tokenUrl;

        public IDictionary<string, string> scopes;

        public Dictionary<string, object> vendorExtensions = new Dictionary<string, object>();
    }
}
