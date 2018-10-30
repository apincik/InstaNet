using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Bot.Instagram.Model;
using BotAction = Bot.Instagram.Model.Action;
using BotLog = Bot.Instagram.Model.Log;
using Bot.Interface;

namespace User.Clients
{
    /// <summary>
    /// Custom HTTP client, possible REST API implementation.
    /// </summary>
    public class Http : Client
    {
        private HttpClient _httpClient;
        private string _baseUrl;
        private string _endpointLog;
        private string _endpointJob;

        public Http(string baseUrl, string endpointJob, string endpointLog, int timeout = 3000)
        {
            _httpClient = new HttpClient(){
                Timeout = TimeSpan.FromMilliseconds(timeout)
            };

            _baseUrl = baseUrl;
            _endpointJob = endpointJob;
            _endpointLog = endpointLog;
        }

        public override async Task<Schedule> FindJobs()
        {
            throw new NotImplementedException();
        }

        public override async Task<SimpleResult> SetJobProcessed(int jobId)
        {
            throw new NotImplementedException();
        }

        public override async Task<SimpleResult> UpdateJobProcess(int jobId, int count)
        {
            throw new NotImplementedException();
        }

        public override async Task<SimpleResult> LogWork(InstaLog jobLog)
        {
            throw new NotImplementedException();
        }

        public override async Task<SimpleResult> LogMessage(string message, int type)
        {
            throw new NotImplementedException();
        }
    }
}
