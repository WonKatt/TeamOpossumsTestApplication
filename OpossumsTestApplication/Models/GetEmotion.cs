using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace OpossumsTestApplication.Models
{
    public class FlickerImg{}
    public class GetEmotion:BackgroundService
    {
        private ILogger _logger;
        private Timer _timer;
        private List<FlickerImg> Images;
        public GetEmotion(ILogger<GetEmotion> logger)
        {
            _logger = logger;
        }
        protected  override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run((() =>
            {
                _logger.LogInformation("Background service start");
                _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(15));
            }), stoppingToken);
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("Timed Background Service is working.");
            
        }

        public static async Task<Emotions> GetEmotions(string imageUrl)
        {
            Emotions emotions;
            using (HttpClient client = new HttpClient())
            {
                using (HttpRequestMessage request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Post;
                    request.RequestUri=
                        new Uri("https://api-us.faceplusplus.com/facepp/v3/detect?" +
                                "api_key=NkLrrYOcXPasOvCeEqaF1IPJHuTBW3OX&" +
                                "api_secret=Hn13FmiaFD2oFvzHUMz-GY0hxHFHyfmA&" +
                                "return_attributes=emotion&" +
                                $"image_url={imageUrl}");
                    var response = await client.SendAsync(request);
                    string schemaJson = await request.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(schemaJson);
                    emotions = result.Faces.FirstOrDefault()?.Attributes.Emotion;
                }
            }
            return emotions;
        }
    }
    
    public class Response
    {
        public string Image_id { get; set; }
        public string Request_id { get; set; }
        public int Time_used { get; set; }
        public string Eror_message { get; set; }
        public List<Face> Faces { get; set; }
    }

    public class Face
    {
        public string Face_token { get; set; }
        public Attributes Attributes { get; set; }
    }

    public class Attributes
    {
        public Emotions Emotion { get; set; }
    }

    public class Emotions
    {
        public double Sadness { get; set; }
        public double Neutral { get; set; }    
        public double Disgust { get; set; }
        public double Anger { get; set; }
        public double Surprise { get; set; }
        public double Happiness { get; set; }    
    }
}