using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Description;
using Jwell.Modules.WebApi.Attributes;
using Swashbuckle.Swagger;

namespace Jwell.ServiceManage.Web
{
    /// <summary>
    /// Swagger扩展
    /// </summary>
    public class ApiIgnoreDocumentExtensions : IDocumentFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="schemaRegistry"></param>
        /// <param name="apiExplorer"></param>
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            List<string> keys = new List<string>();
            foreach (var item in apiExplorer.ApiDescriptions)
            {
                var apiIgnores = item.GetControllerAndActionAttributes<ApiIgnoreAttribute>().ToList();

                if (apiIgnores != null && apiIgnores.Count > 0)
                {
                    string itemID = $"{item.ActionDescriptor.ActionName}{item.RelativePath}".ToLower();
                    foreach (var path in swaggerDoc.paths)
                    {
                        string pathKey = $"{item.ActionDescriptor.ActionName}{path.Key.Remove(0, 1)}".ToLower();
                       
                        if (itemID == pathKey)
                        {
                            keys.Add(path.Key);
                        }
                    }
                }
            }
            keys.ForEach(m => { swaggerDoc.paths.Remove(m); });
        }

        
    }
}