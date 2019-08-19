using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using GenomixDataManager.Formatters.MultipartDataMediaFormatter.Converters;
using GenomixDataManager.Formatters.MultipartDataMediaFormatter.Infrastructure;
using GenomixDataManager.Formatters.MultipartDataMediaFormatter.Infrastructure.Logger;

namespace GenomixDataManager.Formatters.MultipartDataMediaFormatter
{
    public class FormMultipartEncodedMediaTypeFormatter : MediaTypeFormatter, IInputFormatter
    {
        private const string SupportedMediaType = "multipart/form-data";
        
        private readonly MultipartFormatterSettings Settings;

        public FormMultipartEncodedMediaTypeFormatter(MultipartFormatterSettings settings = null)
        {
            Settings = settings ?? new MultipartFormatterSettings();
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(SupportedMediaType));
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);

            //need add boundary
            //(if add when fill SupportedMediaTypes collection in class constructor then receive post with another boundary will not work - Unsupported Media Type exception will thrown)
            if (headers.ContentType == null)
            {
                headers.ContentType = new MediaTypeHeaderValue(SupportedMediaType);
            }
            if (!String.Equals(headers.ContentType.MediaType, SupportedMediaType, StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Not a Multipart Content");
            }
            if (headers.ContentType.Parameters.All(m => m.Name != "boundary"))
            {
                headers.ContentType.Parameters.Add(new NameValueHeaderValue("boundary", "MultipartDataMediaFormatterBoundary1q2w3e"));
            }
        }

        public Boolean CanRead(InputFormatterContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            //var contentType = context.HttpContext.Request.ContentType;
            //if (string.IsNullOrEmpty(contentType) || contentType == SupportedMediaType)
            //    return true;

            //return false;
            return true;
        }

        public async Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
        {
            Type type;
            IFormatterLogger formatterLogger;

            type = context.ModelType;
            formatterLogger = null;

            HttpContextToFormDataConverter httpContextToFormDataConverter = new HttpContextToFormDataConverter();
            FormData multipartFormData = await httpContextToFormDataConverter.Convert(context.HttpContext.Request.Form);

            IFormDataConverterLogger logger;
            if (formatterLogger != null)
                logger = new FormatterLoggerAdapter(formatterLogger);
            else 
                logger = new FormDataConverterLogger();

            var dataToObjectConverter = new FormDataToObjectConverter(multipartFormData, logger, Settings);
            object result = dataToObjectConverter.Convert(type);

            logger.EnsureNoErrors();

            return await InputFormatterResult.SuccessAsync(result);
        }
    }
}