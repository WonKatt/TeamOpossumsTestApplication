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

namespace ModelLogic.ModelLogicInterfacesRealization
{
    public class FacesModelLogic: IFacesModelLogic
    {        
        private readonly d5h6stb0hfhccqContext  _context;       
        private readonly ILogger _logger;
        public FacesModelLogic(d5h6stb0hfhccqContext context, 
                               ILogger<FacesModelLogic> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        
        
        public static string GetJsonSchemaResponse(string imageUrl)
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
                var response =  client.SendAsync(request).Result;                    
                    jsonSchema = response.Content.ReadAsStringAsync().Result;
                    if (response.StatusCode==HttpStatusCode.Forbidden)
                    {
                      jsonSchema = GetJsonSchemaResponse(imageUrl);
                    }    
            }            
            return jsonSchema;
        }

        private  EmotionsFromResponse GetEmotionsObjectFromJson(string jsonSchema)
        {
            if (jsonSchema != null)
            {
               var result = JsonConvert.DeserializeObject<Response>(jsonSchema);
               if (result.Faces!= null)
               {
                   List<FaceObjectFromResponse> facesFromResponse;
                   if (result.Faces.Count>5)               
                   {
                       facesFromResponse = result.Faces.GetRange(0,5);
                   }
                   else
                   {
                       facesFromResponse = result.Faces;
                   }

                   if (facesFromResponse.Count>1)
                   {                       
                       return new EmotionsFromResponse()
                       {
                           Anger = facesFromResponse.Max(emotions=>emotions.Attributes.Emotion.Anger),
                           Surprise = facesFromResponse.Max(emotions=>emotions.Attributes.Emotion.Surprise),
                           Fear = facesFromResponse.Max(emotions=>emotions.Attributes.Emotion.Fear),
                           Disgust = facesFromResponse.Max(emotions=>emotions.Attributes.Emotion.Disgust),
                           Happiness = facesFromResponse.Max(emotions=>emotions.Attributes.Emotion.Happiness),
                           Neutral = facesFromResponse.Max(emotions=>emotions.Attributes.Emotion.Neutral),
                           Sadness = facesFromResponse.Max(emotions=>emotions.Attributes.Emotion.Sadness)                           
                       };
                   }
                   else if(facesFromResponse.Count==0)
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
                   _logger.LogWarning($"{result.Eror_message }\n" +
                                      $"something with that image in json\n{jsonSchema}");
                   return new EmotionsFromResponse();
                   
               }

            }
            else
            {
                throw new Exception("there is no json schema");                
            }
        }
        
        public void FindFacesOnPhotos(IEnumerable<Photo> photos)
        {
            foreach (var photo in photos.OrderBy(photo=>photo.Id))
            {
                _logger.LogInformation($"{photo.Id} is on the way");
                var jsonSchema = GetJsonSchemaResponse(photo.Url);
                var emotions =  GetEmotionsObjectFromJson(jsonSchema);
                if (emotions==null)
                {
                    SetPhotoAsWithOutEnabledFaces(photo.Id);
                }
                else
                {
                    AddFaceEmotionsOnPhoto(photo.Id, emotions);
                    SetPhotoAsWithEnabledFaces(photo.Id);
                }
            }
        }
        

        public  void FindFacesOnNewPhotos() =>
             FindFacesOnPhotos(GetPhotosWithNoInformationAboutFaces());

        public  void FindFacesOnAllPhotos() =>
             FindFacesOnPhotos(GetAllPhotos());

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
                PhotoId = photoId
            });  
        
        public IEnumerable<Photo> GetAllPhotos() =>
            _context.Photo;

        public IEnumerable<Photo> GetPhotosWithNoInformationAboutFaces() =>
            GetAllPhotos().Where(photo => photo.IsFace == null);

        public  void SetPhotoAsWithEnabledFaces(int photoId) 
        {            
            var photo = GetPhotoById(photoId);
            photo.IsFace = true;
            _context.Entry(GetPhotoById(photoId)).CurrentValues.SetValues(photo);
            _context.SaveChanges();
        }

        public void  SetPhotoAsWithOutEnabledFaces(int photoId)
        {
            var photo = GetPhotoById(photoId);
            photo.IsFace = false;
            _context.Entry(GetPhotoById(photoId)).CurrentValues.SetValues(photo);
            _context.SaveChanges();
        }

        public Photo GetPhotoById(int id) =>
            GetAllPhotos().FirstOrDefault(photo => photo.Id == id);

        public string GetPhotoUrl(int photoId) =>
            GetPhotoById(photoId).Url;
    }
}