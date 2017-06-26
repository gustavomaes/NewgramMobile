using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NewgramMobile.Util
{
    public class APIHelper : IDisposable
    {
        private RestClient _client;

        public APIHelper()
        {
            _client = new RestClient(App.URL);
        }

        //GET
        public async Task<T> GET<T>(string url)
        {
            IRestRequest request = new RestRequest(url, Method.GET);
            SetHeadersAllRequest(request);

            var response = await _client.Execute<T>(request);
            ValidateReturn(response);
            GetHeadersLastRequest(response.Headers);
            return response.Data;
        }

        public async Task<string> GET(string URL)
        {
            IRestRequest Request = new RestRequest(URL, Method.GET);

            SetHeadersAllRequest(Request);

            IRestResponse Response = await _client.Execute(Request);

            ValidateReturn(Response);

            GetHeadersLastRequest(Response.Headers);

            return Response.Content;
        }


        //POST
        public async Task<T> POST<T>(string URL, object Data)
        {
            IRestRequest Request = new RestRequest(URL, Method.POST);

            SetHeadersAllRequest(Request);

            Request.AddJsonBody(Data);

            IRestResponse<T> Response = await _client.Execute<T>(Request);

            ValidateReturn(Response);

            GetHeadersLastRequest(Response.Headers);

            return Response.Data;
        }

        public async Task POST(string URL, object Data)
        {
            IRestRequest Request = new RestRequest(URL, Method.POST);

            SetHeadersAllRequest(Request);

            Request.AddJsonBody(Data);

            IRestResponse Response = await _client.Execute(Request);

            ValidateReturn(Response);

            GetHeadersLastRequest(Response.Headers);
        }

        //PUT
        public async Task<T> PUT<T>(string URL, object Data)
        {
            IRestRequest Request = new RestRequest(URL, Method.PUT);

            SetHeadersAllRequest(Request);

            Request.AddJsonBody(Data);

            IRestResponse<T> Response = await _client.Execute<T>(Request);

            ValidateReturn(Response);

            GetHeadersLastRequest(Response.Headers);

            return Response.Data;
        }

        public async Task PUT(string URL, object Data)
        {
            IRestRequest Request = new RestRequest(URL, Method.PUT);

            SetHeadersAllRequest(Request);

            Request.AddJsonBody(Data);

            IRestResponse Response = await _client.Execute(Request);

            ValidateReturn(Response);

            GetHeadersLastRequest(Response.Headers);
        }

        //DELETE
        public async Task DELETE(string URL)
        {
            IRestRequest Request = new RestRequest(URL, Method.DELETE);

            SetHeadersAllRequest(Request);

            IRestResponse Response = await _client.Execute(Request);

            ValidateReturn(Response);

            GetHeadersLastRequest(Response.Headers);
        }

        #region HEADERS

        public Dictionary<string, string> HeadersAllRequests
        {
            get
            {
                Dictionary<string, string> RET = App.PreferenceGet<Dictionary<string, string>>("HeadersAllRequests");

                if (RET == null)
                    RET = new Dictionary<string, string>();

                return RET;
            }
            set
            {
                App.PreferenceAdd("HeadersAllRequests", value);
            }
        }


        Dictionary<string, string> _HeadersRequest;
        public Dictionary<string, string> HeadersRequest
        {
            get
            {
                if (_HeadersRequest == null)
                    _HeadersRequest = new Dictionary<string, string>();

                return _HeadersRequest;
            }
            set
            {
                _HeadersRequest = value;
            }
        }

        Dictionary<string, string> _HeadersLastRequest { get; set; }

        Dictionary<string, string> _HeadersLastResponse { get; set; }

        /// <summary>
        /// ON Request
        /// </summary>
        public void HeaderAdd(String Nane, String Value)
        {
            if (_HeadersLastRequest == null)
                _HeadersLastRequest = new Dictionary<string, string>();

            _HeadersLastRequest.Add(Nane, Value);
        }

        /// <summary>
        /// FROM Response
        /// </summary>
        public String HeaderGet(String Nane)
        {
            if (_HeadersLastResponse != null && _HeadersLastResponse.ContainsKey(Nane))
            {
                return _HeadersLastResponse[Nane];
            }

            return "";
        }


        public Dictionary<string, string> HeadersLastRequest { get { return _HeadersLastRequest; } }
        public Dictionary<string, string> HeadersLastResponse { get { return _HeadersLastResponse; } }

        //Get Headers AUTO
        //Get Headers AUTO
        void SetHeadersAllRequest(IRestRequest Request)
        {
            foreach (var H in HeadersAllRequests)
                Request.AddHeader(H.Key, H.Value);

            if (HeadersRequest != null && HeadersRequest.Count > 0)
                foreach (var H in HeadersRequest)
                    Request.AddHeader(H.Key, H.Value);

            _HeadersLastRequest = HeadersRequest;
            HeadersRequest = null;
        }

        void GetHeadersLastRequest(IHttpHeaders Headers)
        {
            _HeadersLastResponse = new Dictionary<string, string>();

            foreach (var H in Headers)
                _HeadersLastResponse.Add(H.Key, H.Value.FirstOrDefault());
        }

        #endregion

        #region Auxiliars

        private static void ValidateReturn(IRestResponse response)
        {
            if (response.StatusCode == HttpStatusCode.Gone ||
                response.StatusCode == HttpStatusCode.NoContent ||
                response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.Created ||
                response.StatusCode == HttpStatusCode.PartialContent ||
                response.StatusCode == HttpStatusCode.Accepted)
            {
                return;
            }

            throw new HTTPException(response.StatusCode, response.Content);
        }

        #endregion

        public void Dispose()
        {
            _client.Dispose();
            _client = null;
        }
    }

    public class HTTPException : Exception
    {
        public HttpStatusCode Status { get; set; }

        public HTTPException(HttpStatusCode status, String message) : base(message)
        {
            Status = status;
        }
    }
}
