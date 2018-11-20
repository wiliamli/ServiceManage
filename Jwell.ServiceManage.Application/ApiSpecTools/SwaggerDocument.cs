using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Jwell.Framework.Extensions;
using Jwell.ServiceManage.Application.Services.Dtos;
using Jwell.ServiceManage.Domain.Entities.ServiceMgr;

namespace Jwell.ServiceManage.Application.ApiSpecTools
{
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

        public IDictionary<Operation, IList<ApiParameter>> GetOperationDic(IDictionary<string, Schema> definitions)
        {
            var dict = new Dictionary<Operation,IList<ApiParameter>>();
            if (get != null) {get.name = "get";dict.Add(get, get.GetParameters(definitions));}
            if (post != null) {post.name = "post"; dict.Add(post, post.GetParameters(definitions));}
            if (put != null) {put.name="put";dict.Add(put, put.GetParameters(definitions));}
            if (delete != null) {delete.name="delete";dict.Add(delete, delete.GetParameters(definitions));}
            if (options != null) {options.name="options";dict.Add(options, options.GetParameters(definitions));}
            if (head != null) {head.name="head";dict.Add(head, head.GetParameters(definitions));}
            if (patch != null) {patch.name = "patch"; dict.Add(patch, patch.GetParameters(definitions));}
            return dict;
        }
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

        public string name;

        public IList<ApiParameter> GetParameters(IDictionary<string, Schema> definitions)
        {
            return parameters?.Select(parameter => parameter.GetApiParameter(definitions)).ToList();
        }
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


        public string GetExampleValue(IDictionary<string, Schema> definitions)
        {
            if (@ref != null)
            {
                var objName = @ref.Split('/').Last();
                var exampleValue = definitions[objName].properties.Select(property =>
                    $"\"{property.Key}\":" + property.Value.GetExampleValue(definitions)).ToList();
                return "{" + $"{exampleValue.JoinString(",")}" + "}";
            }         

            switch (type)
            {
                case "array":
                    return $"[{items.GetExampleValue(definitions)}]";
                case "object":
                    if (properties == null) return "object";
                    var exampleValue = properties.Select(property =>
                        $"\"{property.Key}\":" + property.Value.GetExampleValue(definitions)).ToList();
                    return "{" + $"{exampleValue.JoinString(",")}" + "}";
                case "string":
                    return format == "date-time" ? DateTime.Now.ToString("yyyy:MM:dd,HH:mm:ss") : "\"string\"";
                case "integer":
                    return "1";
                case "boolean":
                    return "false";
                case "number":
                    return "1";
                default:
                    return "";
            }
        }    
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

    public class Parameter : PartialSchema
    {
        [JsonProperty("$ref")]
        public string @ref;

        public string name;

        public string @in;

        public string description;

        public bool? required;

        public Schema schema;

        public ApiParameter GetApiParameter(IDictionary<string, Schema> definitions)
        {
            var apiParameter = new ApiParameter
            {
                Name = name,
                Description = description,
                ParameterType = @in,
                DataType = type ?? (schema.type ?? "object"),
                Format = format,
                ExampleValue = schema == null ? GetPrimitiveValue() : schema.GetExampleValue(definitions)
            };
            return apiParameter;
        }

        private string GetPrimitiveValue()
        {
            switch (type)
            {
                case "string":
                    return format == "date-time" ? DateTime.Now.ToString("yyyy:MM:dd,HH:mm:ss") : "\"string\"";
                case "integer":
                    return "1";
                case "boolean":
                    return "false";
                case "number":
                    return "1";
                default:
                    return "";
            }
        }
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
