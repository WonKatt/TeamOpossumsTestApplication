using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace TestProject
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            int pageNumber = 0;
            List<int> integers = new List<int>()
            {
                1,2,3,4,5,6,7,9,10,11
            }; 
            int paginationIndex = (pageNumber - 1) * 50;
            int remainingPhotosForPagination = integers.Count - paginationIndex;
            int maxRequiredPhotosForPagination = 50;
             if (paginationIndex + maxRequiredPhotosForPagination > _photoLogic.GetAllPhotosCount())
            {
                return _photoLogic.GetAllPhotos().OrderBy(photo => photo.Faces.Fear).ToList().GetRange(paginationIndex, 
                    remainingPhotosForPagination-1);
            }
            else
            {
                return _photoLogic.GetAllPhotos().OrderBy(photo => photo.Faces.Fear).ToList().GetRange(paginationIndex,
                    maxRequiredPhotosForPagination);
            }
        }
    }
}