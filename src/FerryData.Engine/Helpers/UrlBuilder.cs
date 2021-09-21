using FerryData.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FerryData.Engine.Helpers
{
    public class UrlBuilder
    {
        public static Dictionary<string, string> ParseParameters(string url)
        {
            var parameters = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(url))
            {
                return parameters;
            }

            var ind = url.IndexOf('?');

            if (ind < 0)
            {
                return parameters;
            }

            var parametersSubstring = url.Substring(ind + 1);

            char[] separator = { '&' };
            char[] separatorNameValue = { '=' };
            var parts = parametersSubstring.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            foreach (var nameValueString in parts)
            {
                if (string.IsNullOrEmpty(nameValueString))
                {
                    continue;
                }

                var nameValueParts = nameValueString.Split(separatorNameValue, StringSplitOptions.RemoveEmptyEntries);

                if (nameValueParts.Length > 1)
                {
                    var name = nameValueParts[0];
                    var value = nameValueParts[1];

                    if (!parameters.ContainsKey(name))
                    {
                        parameters.Add(name, value);
                    }
                }
            }

            return parameters;
        }

        public static string GetUrlWithouParameters(string url)
        {
            string resultUrl = url;

            if (string.IsNullOrEmpty(resultUrl))
            {
                return resultUrl;
            }

            var ind = url.IndexOf('?');

            if (ind < 0)
            {
                return resultUrl;
            }

            return resultUrl.Substring(0, ind);

        }

        public static string UpdateUrl(string url, IEnumerable<NameValueDescriptionRow> parameters)
        {
            var urlStart = GetUrlWithouParameters(url);

            var sb = new StringBuilder();

            sb.Append(urlStart);

            if (parameters.Any())
            {
                sb.Append("?");

                var n = 1;
                foreach (var row in parameters)
                {

                    if (n > 1)
                    {
                        sb.Append("&");
                    }

                    sb.Append(row.Name);
                    sb.Append("=");
                    sb.Append(row.Value);

                    n++;
                }
            }

            return sb.ToString();
        }
    }
}
