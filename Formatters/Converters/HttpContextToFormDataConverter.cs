using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GenomixDataManager.Formatters.MultipartDataMediaFormatter.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace GenomixDataManager.Formatters.MultipartDataMediaFormatter.Converters
{
    public class HttpContextToFormDataConverter
    {
        public async Task<FormData> Convert(IFormCollection form)
        {
            var multipartFormData = new FormData();
            
            foreach (IFormFile file in form.Files)
            {
                var name = UnquoteToken(file.Name);
                string fileName = FixFilename(file.FileName);
                string mediaType = file.ContentType;

                var stream = file.OpenReadStream();
                using (StreamReader fileReader = new StreamReader(stream))
                {
                    //@Todo : In the future make the encoding as a setting
                    //byte[] buffer = Encoding.UTF8.GetBytes(await fileReader.ReadToEndAsync());
                    byte[] buffer = await ReadAllBytes(stream);
                    if (buffer.Length >= 0)
                    {
                        multipartFormData.Add(name, new HttpFile(fileName, mediaType, buffer));
                    }
                }
            }

            foreach (var key in form.Keys)
            {
                var name = UnquoteToken(key);
                var data = form[key];
                multipartFormData.Add(name, data);
            }

            return multipartFormData;
        }

        private bool IsFile(ContentDispositionHeaderValue disposition)
        {
            return !string.IsNullOrEmpty(disposition.FileName);
        }

        /// <summary>
        /// Remove bounding quotes on a token if present
        /// </summary>
        private static string UnquoteToken(string token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return token;
            }

            if (token.StartsWith("\"", StringComparison.Ordinal) && token.EndsWith("\"", StringComparison.Ordinal) && token.Length > 1)
            {
                return token.Substring(1, token.Length - 2);
            }

            return token;
        }

        /// <summary>
        /// Amend filenames to remove surrounding quotes and remove path from IE
        /// </summary>
        private static string FixFilename(string originalFileName)
        {
            if (string.IsNullOrWhiteSpace(originalFileName))
                return string.Empty;

            var result = originalFileName.Trim();
            
            // remove leading and trailing quotes
            result = result.Trim('"');

            // remove full path versions
            if (result.Contains("\\"))
                result = Path.GetFileName(result);

            return result;
        }

        private async Task<byte[]> ReadAllBytes(Stream input)
        {
            using (var stream = new MemoryStream())
            {
                await input.CopyToAsync(stream);
                return stream.ToArray();
            }
        }
    }
}
