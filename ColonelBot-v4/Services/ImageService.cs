using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

//This service is to be used to prevent the bot from having to lock and upload images from files, saving on IO 

namespace ColonelBot_v4.Services
{
    public class ImageService
    {
        private readonly HttpClient _http;

        public ImageService(HttpClient http)
            => _http = http;

        public async Task<Stream> GetImageFromURL(string URL)
        {
            var resp = await _http.GetAsync(URL);
            return await resp.Content.ReadAsStreamAsync();
        }
    }
}
