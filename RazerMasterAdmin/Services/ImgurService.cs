using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Imgur.API.Authentication;
using Imgur.API.Endpoints;
using System.Threading.Tasks;
using System.Net.Http;


namespace RazerMasterAdmin.Services
{
    public static class ImgurService
    {
        public static async Task<string> Upload(HttpPostedFileBase file)
        {
            var apiClient = new ApiClient("bfc6307f79426e0", "a6c37b991fd4200ac703eecbc0c4bf8f27bc31dc");
            var httpClient = new HttpClient();
            var imageEndpoint = new ImageEndpoint(apiClient, httpClient);
            var imageUpload = await imageEndpoint.UploadImageAsync(file.InputStream);

            return imageUpload.Link;
        }

    }
}