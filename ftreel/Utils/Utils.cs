using Microsoft.AspNetCore.StaticFiles;

namespace ftreel.Utils

{
    public static class UtilsClass
    {
        public static string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(path, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
    }
}