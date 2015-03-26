using load_board_api.Dtos;
using load_board_api.Exceptions;
using load_board_api.Models;
using load_board_api.Persistence;
using load_board_api.Services;
using load_board_api.Tests.Test_Start;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace load_board_api.Tests.Services
{
    [TestClass]
    public class LocationServiceTest
    {
        private ILocationService locationService;

        public LocationServiceTest()
        {
            this.locationService = new LocationService(TestUtil.UNIT_OF_WORK);
        }

        [TestMethod]
        public void GetLocation()
        {
            LocationDto testDto = TestUtil.LOCATION_DTOS.ElementAt(0);
            LocationDto dto = this.locationService.Get(testDto.Id);
            TestUtil.AreEqual(testDto, dto);
        }

        [TestMethod]
        [ExpectedException(typeof(DoesNotExistException))]
        public void GetNonexistentLocation()
        {
            this.locationService.Get(Guid.Empty);
        }

        [TestMethod]
        public void GetLocations()
        {
            LocationDto[] dtos = this.locationService.Get();
            Assert.AreEqual(1, dtos.Length);
            TestUtil.AreEqual(TestUtil.LOCATION_DTOS.ElementAt(0), dtos[0]);
        }

        [TestMethod]
        public void GetLocations2()
        {
            LocationDto[] dtos = this.locationService.Get(false);
            Assert.AreEqual(1, dtos.Length);
            TestUtil.AreEqual(TestUtil.LOCATION_DTOS.ElementAt(0), dtos[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void AddExistingLocation()
        {
            this.locationService.Add(TestUtil.LOCATION_DTOS.ElementAt(0));
        }

        [TestMethod]
        public void UpdateLocation()
        {
            LocationDto testDto = TestUtil.LOCATION_DTOS.ElementAt(0);
            LocationDto dto = this.locationService.Update(testDto);
            Assert.AreNotEqual(testDto.LastUpdated, dto.LastUpdated);
            dto.LastUpdated = testDto.LastUpdated;
            TestUtil.AreEqual(testDto, dto);
        }

        [TestMethod]
        [ExpectedException(typeof(ConflictException))]
        public void UpdateLocationOutdated()
        {
            LocationDto testDto = TestUtil.LOCATION_DTOS.ElementAt(0);
            testDto.LastUpdated = testDto.LastUpdated.AddSeconds(-1);
            this.locationService.Update(testDto);
        }

        [TestMethod]
        [ExpectedException(typeof(DoesNotExistException))]
        public void UpdateNonexistentLocation()
        {
            LocationDto testDto = TestUtil.LOCATION_DTOS.ElementAt(0);
            testDto.Id = Guid.Empty;
            this.locationService.Update(testDto);
        }

        [TestMethod]
        public void DeleteLocation()
        {
            LocationDto testDto = TestUtil.LOCATION_DTOS.ElementAt(0);
            this.locationService.Delete(testDto.Id);
        }

        [TestMethod]
        public void DeleteNonexistentLocation()
        {
            this.locationService.Delete(Guid.Empty);
        }
    }
}
