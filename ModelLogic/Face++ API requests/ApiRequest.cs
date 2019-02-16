using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FlickerDbModel;
using Microsoft.Extensions.Logging;
using ModelLogic.HelpClasses;
using ModelLogic.ModelLogicInterfaces;
using Newtonsoft.Json;

namespace ModelLogic
{
    public class ApiRequest:IApiRequest
    {
        private readonly HttpClient _client;
        private readonly ILogger _logger;
        private readonly IPhotoModelLogic _photoLogic;
        private readonly IFacesModelLogic _facesModel;

        public ApiRequest(HttpClient client, ILogger<ApiRequest> logger, IPhotoModelLogic photoLogic, IFacesModelLogic facesModel)
        {
            _client = client;
            _logger = logger;
            _photoLogic = photoLogic;
            _facesModel = facesModel;
        }
        public async Task<string> GetJsonSchemaResponse(string imageUrl)
        {
            string jsonSchema;
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api-us.faceplusplus.com/facepp/v3/detect?" +
                                     "api_key=NkLrrYOcXPasOvCeEqaF1IPJHuTBW3OX&" +
                                     "api_secret=Hn13FmiaFD2oFvzHUMz-GY0hxHFHyfmA&" +
                                     "return_attributes=emotion&" +
                                     $"image_url={imageUrl}")
            };
            var response = await _client.SendAsync(request);
            jsonSchema = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                var result = JsonConvert.DeserializeObject<Response>(jsonSchema);
                if (result.Eror_message == "CONCURRENCY_LIMIT_EXCEEDED")
                {
                    _logger.LogWarning($"{response.StatusCode}\n Trying to send request again\n " +
                                       $"You may trapped in recursion ");
                    jsonSchema = await GetJsonSchemaResponse(imageUrl);
                    _logger.LogInformation($"The object is successful accepted\n You out of recursion  ");
                }
            }            
            return jsonSchema;
        }
        public EmotionsFromResponse GetEmotionsObjectFromJson(string jsonSchema)
        {
            if (jsonSchema != null)
            {
                var result = JsonConvert.DeserializeObject<Response>(jsonSchema);
                if (result.Faces != null)
                {
                    List<FaceObjectFromResponse> facesFromResponse;
                    if (result.Faces.Count > 5)
                    {
                        facesFromResponse = result.Faces.GetRange(0, 5);
                    }
                    else
                    {
                        facesFromResponse = result.Faces;
                    }

                    if (facesFromResponse.Count > 1)
                    {
                        return new EmotionsFromResponse()
                        {
                            Anger = facesFromResponse.Max(emotions => emotions.Attributes.Emotion.Anger),
                            Surprise = facesFromResponse.Max(emotions => emotions.Attributes.Emotion.Surprise),
                            Fear = facesFromResponse.Max(emotions => emotions.Attributes.Emotion.Fear),
                            Disgust = facesFromResponse.Max(emotions => emotions.Attributes.Emotion.Disgust),
                            Happiness = facesFromResponse.Max(emotions => emotions.Attributes.Emotion.Happiness),
                            Neutral = facesFromResponse.Max(emotions => emotions.Attributes.Emotion.Neutral),
                            Sadness = facesFromResponse.Max(emotions => emotions.Attributes.Emotion.Sadness)
                        };
                    }
                    else if (facesFromResponse.Count == 0)
                    {
                        return null;
                    }
                    else
                    {
                        return facesFromResponse.FirstOrDefault()?.Attributes.Emotion;
                    }
                }
                else
                {
                    _logger.LogWarning($"{result.Eror_message}\n" +
                                       $"something with that image in json\n{jsonSchema}");
                    return new EmotionsFromResponse();

                }

            }
            else
            {
                throw new Exception("there is no json schema");
            }
        }
        public async Task FindFacesOnPhotos(IEnumerable<Photo> photos)
        {
            foreach (var photo in photos.OrderBy(photo => photo.Id))
            {
                _logger.LogInformation($"{photo.Id} is on the way");
                var jsonSchema = await GetJsonSchemaResponse(photo.Url);
                var emotions = GetEmotionsObjectFromJson(jsonSchema);
                if (emotions == null)
                {
                    await _photoLogic.SetPhotoAsWithOutEnabledFaces(photo.Id);
                }
                else
                {
                    _facesModel.AddFaceEmotionsOnPhoto(photo.Id, emotions);
                    await _photoLogic.SetPhotoAsWithEnabledFaces(photo.Id);
                }
            }
        }
    }
}