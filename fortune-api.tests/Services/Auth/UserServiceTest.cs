using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using fortune_api.App_Start;
using Moq;
using fortune_api.Persistence;
using fortune_api.Models.Auth;
using System.Collections.Generic;
using AutoMapper;
using fortune_api.Dtos.Auth;
using System.Linq;
using System.Linq.Expressions;
using fortune_api.Services.Auth;
using fortune_api.Tests.Test_Start;
using fortune_api.Exceptions;

namespace load_board_api.Tests.Services.Auth
{
    [TestClass]
    public class UserServiceTest
    {
        #region Get (All)

        [TestMethod]
        public void GetAllUsers()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock repos
            Mock<IRepo<UserProfile>> mockUserProfileRepo = new Mock<IRepo<UserProfile>>();

            //Test user
            UserProfile[] testUsers = new UserProfile[] {
                new UserProfile
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Doe",
                    Permissions = new List<Permission>()
                },
                new UserProfile {
                    Id = Guid.NewGuid(),
                    FirstName = "Jane",
                    LastName = "Doe",
                    Permissions = new List<Permission>()
                }
            };
            UserDto[] testUserDtos = Mapper.Map<UserDto[]>(testUsers);

            //Mock call
            mockUserProfileRepo.Setup(x => x.Get(
                It.IsAny<Expression<Func<UserProfile, bool>>>(),
                -1,
                -1,
                It.IsAny<Func<IQueryable<UserProfile>, IOrderedQueryable<UserProfile>>>(),
                ""    
            )).Returns(testUsers);

            //Mock unit of work
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.UserProfileRepo).Returns(mockUserProfileRepo.Object);

            //User service
            UserService userService = new UserService(mockUnitOfWork.Object);

            //Test
            UserDto[] users = userService.Get();
            TestUtil.Compare(testUserDtos, users);
        }

        #endregion

        #region Get

        [TestMethod]
        public void GetUser()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock repos
            Mock<IRepo<UserProfile>> mockUserProfileRepo = new Mock<IRepo<UserProfile>>();

            //Test user
            UserProfile testUser = new UserProfile
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Permissions = new List<Permission>()
            };
            UserDto testUserDto = Mapper.Map<UserDto>(testUser);

            //Mock call
            mockUserProfileRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testUser.Id))).Returns(testUser);

            //Mock unit of work
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.UserProfileRepo).Returns(mockUserProfileRepo.Object);

            //User service
            UserService userService = new UserService(mockUnitOfWork.Object);

            //Test
            UserDto user = userService.Get(testUser.Id);
            TestUtil.Compare(testUserDto, user);
        }

        [TestMethod]
        [ExpectedException(typeof(DoesNotExistException))]
        public void GetNonexistentUser()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock repos
            Mock<IRepo<UserProfile>> mockUserProfileRepo = new Mock<IRepo<UserProfile>>();

            //Mock call
            mockUserProfileRepo.Setup(x => x.Get(It.IsAny<Guid>())).Returns<UserProfile>(null);

            //Mock unit of work
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.UserProfileRepo).Returns(mockUserProfileRepo.Object);

            //User service
            UserService userService = new UserService(mockUnitOfWork.Object);

            //Test
            userService.Get(Guid.NewGuid());
        }

        #endregion

        #region Add

        [TestMethod]
        public void AddUser()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock repos
            Mock<IRepo<UserProfile>> mockUserProfileRepo = new Mock<IRepo<UserProfile>>();

            //Test user
            UserProfile testUser = new UserProfile
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Permissions = new List<Permission>()
            };
            UserDto testUserDto = Mapper.Map<UserDto>(testUser);

            //Mock call
            mockUserProfileRepo.Setup(x => x.Exists(It.IsAny<Guid>())).Returns(false);

            //Mock unit of work
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.UserProfileRepo).Returns(mockUserProfileRepo.Object);

            //User service
            UserService userService = new UserService(mockUnitOfWork.Object);

            //Test
            UserDto user = userService.Add(testUserDto);
            TestUtil.Compare(testUserDto, user, false);
        }

        #endregion

        #region Update

        [TestMethod]
        public void UpdateUser()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock repos
            Mock<IRepo<UserProfile>> mockUserProfileRepo = new Mock<IRepo<UserProfile>>();

            //Test user
            UserProfile testUser = new UserProfile
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Permissions = new List<Permission>()
            };
            UserDto testUserDto = Mapper.Map<UserDto>(testUser);

            //Mock call
            mockUserProfileRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testUser.Id))).Returns(testUser);

            //Mock unit of work
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.UserProfileRepo).Returns(mockUserProfileRepo.Object);

            //User service
            UserService userService = new UserService(mockUnitOfWork.Object);

            //Test
            UserDto user = userService.Update(testUserDto);
            TestUtil.Compare(testUserDto, user);
        }

        #endregion

        #region Delete

        [TestMethod]
        public void DeleteUser()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock repos
            Mock<IRepo<UserProfile>> mockUserProfileRepo = new Mock<IRepo<UserProfile>>();

            //Test user
            UserProfile testUser = new UserProfile
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Permissions = new List<Permission>()
            };
            UserDto testUserDto = Mapper.Map<UserDto>(testUser);

            //Mock call
            mockUserProfileRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testUser.Id))).Returns(testUser);

            //Mock unit of work
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.UserProfileRepo).Returns(mockUserProfileRepo.Object);

            //User service
            UserService userService = new UserService(mockUnitOfWork.Object);

            //Test
            userService.Delete(testUser.Id);
        }

        [TestMethod]
        public void DeleteNonexistentUser()
        {
            //Mock repos
            Mock<IRepo<UserProfile>> mockUserProfileRepo = new Mock<IRepo<UserProfile>>();

            //Mock call
            mockUserProfileRepo.Setup(x => x.Get(It.IsAny<Guid>())).Returns<UserProfile>(null);

            //Mock unit of work
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.UserProfileRepo).Returns(mockUserProfileRepo.Object);

            //User service
            UserService userService = new UserService(mockUnitOfWork.Object);

            //Test
            userService.Delete(Guid.NewGuid());
        }

        #endregion
    }
}
