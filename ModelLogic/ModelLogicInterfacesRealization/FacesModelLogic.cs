using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FlickerDbModel;
using Microsoft.Extensions.Logging;
using ModelLogic.HelpClasses;
using ModelLogic.HelpClasses.Pagination_helped_classe;
using ModelLogic.ModelLogicInterfaces;
using Newtonsoft.Json;

namespace ModelLogic.ModelLogicInterfacesRealization
{
    public class FacesModelLogic : IFacesModelLogic
    {
        private readonly d5h6stb0hfhccqContext _context;
        private readonly ILogger _logger;
        private readonly IPhotoModelLogic _photoLogic;

        public FacesModelLogic(d5h6stb0hfhccqContext context,
            ILogger<FacesModelLogic> logger,
            IPhotoModelLogic photoLogic)
        {
            _photoLogic = photoLogic;
            _context = context;
            _logger = logger;
        }
        public static async Task<string> GetJsonSchemaResponse(string imageUrl)
        {
            string jsonSchema;
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://api-us.faceplusplus.com/facepp/v3/detect?" +
                                         "api_key=NkLrrYOcXPasOvCeEqaF1IPJHuTBW3OX&" +
                                         "api_secret=Hn13FmiaFD2oFvzHUMz-GY0hxHFHyfmA&" +
                                         "return_attributes=emotion&" +
                                         $"image_url={imageUrl}")
                };
                var response = await client.SendAsync(request);
                jsonSchema = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    jsonSchema =  await GetJsonSchemaResponse(imageUrl);
                }
            }

            return jsonSchema;
        }

        private EmotionsFromResponse GetEmotionsObjectFromJson(string jsonSchema)
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
                    AddFaceEmotionsOnPhoto(photo.Id, emotions);
                    await _photoLogic.SetPhotoAsWithEnabledFaces(photo.Id);
                }
            }
        }


        public async Task FindFacesOnNewPhotos() =>
             await FindFacesOnPhotos(_photoLogic.GetPhotosWithNoInformationAboutFaces());

        public async Task FindFacesOnAllPhotos() =>
            await FindFacesOnPhotos(_photoLogic.GetAllPhotos());

        public bool IsEmotionParamIsLegit(string emotion) =>
            emotion == "Fear" || emotion == "Sadness" || emotion == "Neutral"
            || emotion == "Disgust" || emotion == "Anger" || emotion == "Surprise"
            || emotion == "Happiness";
        
        public void AddFaceEmotionsOnPhoto(int photoId, EmotionsFromResponse emotions) =>        
            _context.Faces.Add(new Faces()
            {
                Surprise = emotions.Surprise,
                Anger = emotions.Anger,
                Disgust = emotions.Disgust,
                Fear = emotions.Fear,
                Happiness = emotions.Happiness,
                Neutral = emotions.Neutral,
                Sadness = emotions.Sadness,
                PhotoId=photoId
            });
    }
}