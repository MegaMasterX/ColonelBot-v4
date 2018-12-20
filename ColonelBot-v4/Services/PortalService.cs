using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using Discord;

//This service handles interaction with the N1GP Online Portal.

namespace ColonelBot_v4.Services
{
    public class PortalService
    {
        private readonly HttpClient _http;

        public PortalService(HttpClient http)
               => _http = http;


    }
}
