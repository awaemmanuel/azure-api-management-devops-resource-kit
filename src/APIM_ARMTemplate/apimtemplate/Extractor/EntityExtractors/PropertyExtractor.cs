﻿using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Extract
{
    public class PropertyExtractor
    {
        static string baseUrl = "https://management.azure.com";
        internal Authentication auth = new Authentication();

        public async Task<string> GetProperties(string ApiManagementName, string ResourceGroupName)
        {
            (string azToken, string azSubId) = await auth.GetAccessToken();

            string requestUrl = string.Format("{0}/subscriptions/{1}/resourceGroups/{2}/providers/Microsoft.ApiManagement/service/{3}/properties?api-version={4}",
               baseUrl, azSubId, ResourceGroupName, ApiManagementName, Constants.APIVersion);

            return await CallApiManagement(azToken, requestUrl);
        }

        public async Task<string> GetProperty(string ApiManagementName, string ResourceGroupName, string propertyName)
        {
            (string azToken, string azSubId) = await auth.GetAccessToken();

            string requestUrl = string.Format("{0}/subscriptions/{1}/resourceGroups/{2}/providers/Microsoft.ApiManagement/service/{3}/properties/{4}?api-version={5}",
               baseUrl, azSubId, ResourceGroupName, ApiManagementName, propertyName, Constants.APIVersion);

            return await CallApiManagement(azToken, requestUrl);
        }

        private static async Task<string> CallApiManagement(string azToken, string requestUrl)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", azToken);

                HttpResponseMessage response = await httpClient.SendAsync(request);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
        }
    }


}
