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

        public FacesModelLogic(d5h6stb0hfhccqContext context)
        {
            _context = context;
        }

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