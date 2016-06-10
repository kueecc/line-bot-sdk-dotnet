using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LineBot
{
    public partial class Client
    {
        private Action<ReceivedResults> _callback;
        private HttpListener _listener;

        public void On(string uri, Action<ReceivedResults> callback)
        {
            _listener?.Stop();

            _listener = new HttpListener();
            _listener.Prefixes.Add(uri);

            _callback = callback;

            //Console.WriteLine($"Listening.. {uri}");
            _listener.Start();

            RunWorkerThread(_listener, _callback);
        }


        private void RunWorkerThread(HttpListener listener, Action<ReceivedResults> callback)
        {
            Task.Run(() =>
            {
                while (listener.IsListening)
                {
                    ReceivedResults results;

                    try
                    {
                        //Console.WriteLine($"Waiting next callback..");

                        // blocking
                        var context = _listener.GetContext();

                        byte[] rawBody = new byte[context.Request.ContentLength64];
                        context.Request.InputStream.Read(rawBody, 0, rawBody.Length);

                        var signature = context.Request.Headers[HeaderName.ChannelSignature];

                        //Console.WriteLine(Encoding.UTF8.GetString(rawBody));
                        //Console.WriteLine(signature);

                        if (!ValidateSignature(rawBody, signature))
                        {
                            context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                            context.Response.Close();

                            throw new InvalidSignatureException();
                        }
                        else
                        {
                            context.Response.StatusCode = (int) HttpStatusCode.OK;
                            context.Response.Close();
                        }

                        var utfBody = Encoding.UTF8.GetString(rawBody);
                        results = JsonConvert.DeserializeObject<ReceivedResults>(utfBody);
                    }
                    catch (Exception e)
                    {
                        results = new ReceivedResults {Exception = e};
                    }
                    
                    // spawn another thread to call callback
                    Task.Run(() => callback(results));
                }
            });
        }
    }
}