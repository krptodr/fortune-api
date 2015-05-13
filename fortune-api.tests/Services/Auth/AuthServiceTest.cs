using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using fortune_api.App_Start;
using Moq;
using fortune_api.Persistence;
using fortune_api.Models.Auth;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using fortune_api.Services.Security;
using fortune_api.Dtos.Auth;
using AutoMapper;
using fortune_api.Services.Auth;
using fortune_api.Tests.Test_Start;
using fortune_api.Exceptions;

namespace load_board_api.Tests.Services.Auth
{
    [TestClass]
    public class AuthServiceTest
    {
        #region LogInViaEmail

        [TestMethod]
        public void LogInViaEmail()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock repos
            Mock<IRepo<EmailAuth>> mockEmailAuthRepo = new Mock<IRepo<EmailAuth>>();
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

            //Test auth
            EmailAuth testAuth = new EmailAuth
            {
                Id = Guid.NewGuid(),
                UserId = testUser.Id,
                Email = "john.doe@test.com",
                HashedPassword = "Password"
            };

            //Test login response
            LoginRes testLoginRes = new LoginRes
            {
                JWT = "JWT",
                User = testUserDto
            };

            //Mock calls
            mockEmailAuthRepo.Setup(x => x.Get(
                It.IsAny<Expression<Func<EmailAuth, bool>>>(),
                -1,
                -1,
                It.IsAny<Func<IQueryable<EmailAuth>, IOrderedQueryable<EmailAuth>>>(),
                ""    
            )).Returns(new List<EmailAuth> {testAuth});
            mockUserProfileRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testUser.Id))).Returns(testUser);

            //Mock unit of work
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.UserProfileRepo).Returns(mockUserProfileRepo.Object);
            mockUnitOfWork.SetupGet(x => x.EmailAuthRepo).Returns(mockEmailAuthRepo.Object);

            //Mock hasher
            Mock<IHasher> mockHasher = new Mock<IHasher>();
            mockHasher.Setup(x => x.CompareWithHash(testAuth.HashedPassword, testAuth.HashedPassword)).Returns(true);

            //Mock jwt service
            Mock<IJwtService> mockJwtService = new Mock<IJwtService>();
            mockJwtService.Setup(x => x.CreateToken(
                testUser.Id.ToString(),
                JwtService.DEFAULT_ISSUER,
                JwtService.DEFAULT_AUDIENCE,
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<Dictionary<string, string>>()
            )).Returns(testLoginRes.JWT);   

            //Auth service
            AuthService authService = new AuthService(mockHasher.Object, mockUnitOfWork.Object, mockJwtService.Object);

            //Test
            LoginRes loginRes = authService.LogInViaEmail(testAuth.Email, testAuth.HashedPassword);
            TestUtil.Compare(loginRes, testLoginRes);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void LogInViaNonexistentEmail()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock repos
            Mock<IRepo<EmailAuth>> mockEmailAuthRepo = new Mock<IRepo<EmailAuth>>();
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

            //Test auth
            EmailAuth testAuth = new EmailAuth
            {
                Id = Guid.NewGuid(),
                UserId = testUser.Id,
                Email = "john.doe@test.com",
                HashedPassword = "Password"
            };

            //Test login response
            LoginRes testLoginRes = new LoginRes
            {
                JWT = "JWT",
                User = testUserDto
            };

            //Mock calls
            mockEmailAuthRepo.Setup(x => x.Get(
                It.IsAny<Expression<Func<EmailAuth, bool>>>(),
                -1,
                -1,
                It.IsAny<Func<IQueryable<EmailAuth>, IOrderedQueryable<EmailAuth>>>(),
                ""    
            )).Returns(new List<EmailAuth> {});
            mockUserProfileRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testUser.Id))).Returns(testUser);

            //Mock unit of work
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.UserProfileRepo).Returns(mockUserProfileRepo.Object);
            mockUnitOfWork.SetupGet(x => x.EmailAuthRepo).Returns(mockEmailAuthRepo.Object);

            //Mock hasher
            Mock<IHasher> mockHasher = new Mock<IHasher>();
            mockHasher.Setup(x => x.CompareWithHash(testAuth.HashedPassword, testAuth.HashedPassword)).Returns(true);

            //Mock jwt service
            Mock<IJwtService> mockJwtService = new Mock<IJwtService>();
            mockJwtService.Setup(x => x.CreateToken(
                testUser.Id.ToString(),
                JwtService.DEFAULT_ISSUER,
                JwtService.DEFAULT_AUDIENCE,
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<Dictionary<string, string>>()
            )).Returns(testLoginRes.JWT);   

            //Auth service
            AuthService authService = new AuthService(mockHasher.Object, mockUnitOfWork.Object, mockJwtService.Object);

            //Test
            authService.LogInViaEmail(testAuth.Email, testAuth.HashedPassword);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void LogInViaEmailInvalidPassword()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock repos
            Mock<IRepo<EmailAuth>> mockEmailAuthRepo = new Mock<IRepo<EmailAuth>>();
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

            //Test auth
            EmailAuth testAuth = new EmailAuth
            {
                Id = Guid.NewGuid(),
                UserId = testUser.Id,
                Email = "john.doe@test.com",
                HashedPassword = "Password"
            };

            //Test login response
            LoginRes testLoginRes = new LoginRes
            {
                JWT = "JWT",
                User = testUserDto
            };

            //Mock calls
            mockEmailAuthRepo.Setup(x => x.Get(
                It.IsAny<Expression<Func<EmailAuth, bool>>>(),
                -1,
                -1,
                It.IsAny<Func<IQueryable<EmailAuth>, IOrderedQueryable<EmailAuth>>>(),
                ""    
            )).Returns(new List<EmailAuth> {testAuth});
            mockUserProfileRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testUser.Id))).Returns(testUser);

            //Mock unit of work
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.UserProfileRepo).Returns(mockUserProfileRepo.Object);
            mockUnitOfWork.SetupGet(x => x.EmailAuthRepo).Returns(mockEmailAuthRepo.Object);

            //Mock hasher
            Mock<IHasher> mockHasher = new Mock<IHasher>();
            mockHasher.Setup(x => x.CompareWithHash(testAuth.HashedPassword, testAuth.HashedPassword)).Returns(false);

            //Mock jwt service
            Mock<IJwtService> mockJwtService = new Mock<IJwtService>();
            mockJwtService.Setup(x => x.CreateToken(
                testUser.Id.ToString(),
                JwtService.DEFAULT_ISSUER,
                JwtService.DEFAULT_AUDIENCE,
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<Dictionary<string, string>>()
            )).Returns(testLoginRes.JWT);   

            //Auth service
            AuthService authService = new AuthService(mockHasher.Object, mockUnitOfWork.Object, mockJwtService.Object);

            //Test
            authService.LogInViaEmail(testAuth.Email, testAuth.HashedPassword);
        }
        #endregion

        #region RegisterEmail

        [TestMethod]
        public void RegisterEmail()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock repos
            Mock<IRepo<EmailAuth>> mockEmailAuthRepo = new Mock<IRepo<EmailAuth>>();
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

            //Test auth
            EmailAuth testAuth = new EmailAuth
            {
                Id = Guid.NewGuid(),
                UserId = testUser.Id,
                Email = "john.doe@test.com",
                HashedPassword = "Password"
            };

            //Test login response
            LoginRes testLoginRes = new LoginRes
            {
                JWT = "JWT",
                User = testUserDto
            };

            //Mock calls
            mockEmailAuthRepo.Setup(x => x.Get(
                It.IsAny<Expression<Func<EmailAuth, bool>>>(),
                -1,
                -1,
                It.IsAny<Func<IQueryable<EmailAuth>, IOrderedQueryable<EmailAuth>>>(),
                ""
            )).Returns(new List<EmailAuth> { });
            mockEmailAuthRepo.Setup(x => x.Exists(It.IsAny<Guid>())).Returns(false);
            mockUserProfileRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testUser.Id))).Returns(testUser);

            //Mock unit of work
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.UserProfileRepo).Returns(mockUserProfileRepo.Object);
            mockUnitOfWork.SetupGet(x => x.EmailAuthRepo).Returns(mockEmailAuthRepo.Object);

            //Mock hasher
            Mock<IHasher> mockHasher = new Mock<IHasher>();
            mockHasher.Setup(x => x.Hash(testAuth.HashedPassword)).Returns(testAuth.HashedPassword);

            //Mock jwt service
            Mock<IJwtService> mockJwtService = new Mock<IJwtService>();
            mockJwtService.Setup(x => x.CreateToken(
                testUser.Id.ToString(),
                JwtService.DEFAULT_ISSUER,
                JwtService.DEFAULT_AUDIENCE,
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<Dictionary<string, string>>()
            )).Returns(testLoginRes.JWT);

            //Auth service
            AuthService authService = new AuthService(mockHasher.Object, mockUnitOfWork.Object, mockJwtService.Object);

            //Test
            LoginRes loginRes = authService.RegisterEmail(testUser.Id, testAuth.Email, testAuth.HashedPassword);
            TestUtil.Compare(loginRes, testLoginRes);
        }

        [TestMethod]
        [ExpectedException(typeof(DoesNotExistException))]
        public void RegisterEmailNonexistentUser()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock repos
            Mock<IRepo<EmailAuth>> mockEmailAuthRepo = new Mock<IRepo<EmailAuth>>();
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

            //Test auth
            EmailAuth testAuth = new EmailAuth
            {
                Id = Guid.NewGuid(),
                UserId = testUser.Id,
                Email = "john.doe@test.com",
                HashedPassword = "Password"
            };

            //Test login response
            LoginRes testLoginRes = new LoginRes
            {
                JWT = "JWT",
                User = testUserDto
            };

            //Mock calls
            mockEmailAuthRepo.Setup(x => x.Get(
                It.IsAny<Expression<Func<EmailAuth, bool>>>(),
                -1,
                -1,
                It.IsAny<Func<IQueryable<EmailAuth>, IOrderedQueryable<EmailAuth>>>(),
                ""
            )).Returns(new List<EmailAuth> {  });
            mockEmailAuthRepo.Setup(x => x.Exists(It.IsAny<Guid>())).Returns(false);
            mockUserProfileRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testUser.Id))).Returns<UserProfile>(null);

            //Mock unit of work
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.UserProfileRepo).Returns(mockUserProfileRepo.Object);
            mockUnitOfWork.SetupGet(x => x.EmailAuthRepo).Returns(mockEmailAuthRepo.Object);

            //Mock hasher
            Mock<IHasher> mockHasher = new Mock<IHasher>();
            mockHasher.Setup(x => x.Hash(testAuth.HashedPassword)).Returns(testAuth.HashedPassword);

            //Mock jwt service
            Mock<IJwtService> mockJwtService = new Mock<IJwtService>();
            mockJwtService.Setup(x => x.CreateToken(
                testUser.Id.ToString(),
                JwtService.DEFAULT_ISSUER,
                JwtService.DEFAULT_AUDIENCE,
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<Dictionary<string, string>>()
            )).Returns(testLoginRes.JWT);

            //Auth service
            AuthService authService = new AuthService(mockHasher.Object, mockUnitOfWork.Object, mockJwtService.Object);

            //Test
            LoginRes loginRes = authService.RegisterEmail(testUser.Id, testAuth.Email, testAuth.HashedPassword);
            TestUtil.Compare(loginRes, testLoginRes);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void RegisterExistingEmail()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock repos
            Mock<IRepo<EmailAuth>> mockEmailAuthRepo = new Mock<IRepo<EmailAuth>>();
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

            //Test auth
            EmailAuth testAuth = new EmailAuth
            {
                Id = Guid.NewGuid(),
                UserId = testUser.Id,
                Email = "john.doe@test.com",
                HashedPassword = "Password"
            };

            //Test login response
            LoginRes testLoginRes = new LoginRes
            {
                JWT = "JWT",
                User = testUserDto
            };

            //Mock calls
            mockEmailAuthRepo.Setup(x => x.Get(
                It.IsAny<Expression<Func<EmailAuth, bool>>>(),
                -1,
                -1,
                It.IsAny<Func<IQueryable<EmailAuth>, IOrderedQueryable<EmailAuth>>>(),
                ""
            )).Returns(new List<EmailAuth> { testAuth });
            mockEmailAuthRepo.Setup(x => x.Exists(It.IsAny<Guid>())).Returns(false);
            mockUserProfileRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testUser.Id))).Returns(testUser);

            //Mock unit of work
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.UserProfileRepo).Returns(mockUserProfileRepo.Object);
            mockUnitOfWork.SetupGet(x => x.EmailAuthRepo).Returns(mockEmailAuthRepo.Object);

            //Mock hasher
            Mock<IHasher> mockHasher = new Mock<IHasher>();
            mockHasher.Setup(x => x.Hash(testAuth.HashedPassword)).Returns(testAuth.HashedPassword);

            //Mock jwt service
            Mock<IJwtService> mockJwtService = new Mock<IJwtService>();
            mockJwtService.Setup(x => x.CreateToken(
                testUser.Id.ToString(),
                JwtService.DEFAULT_ISSUER,
                JwtService.DEFAULT_AUDIENCE,
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<Dictionary<string, string>>()
            )).Returns(testLoginRes.JWT);

            //Auth service
            AuthService authService = new AuthService(mockHasher.Object, mockUnitOfWork.Object, mockJwtService.Object);

            //Test
            LoginRes loginRes = authService.RegisterEmail(testUser.Id, testAuth.Email, testAuth.HashedPassword);
            TestUtil.Compare(loginRes, testLoginRes);
        }

        #endregion

        #region RemoveUsersAuthMethods

        [TestMethod]
        public void RemoveUsersAuthMethods()
        {
            //Mock repos
            Mock<IRepo<EmailAuth>> mockEmailAuthRepo = new Mock<IRepo<EmailAuth>>();

            //Mock calls
            //Mock calls
            mockEmailAuthRepo.Setup(x => x.Get(
                It.IsAny<Expression<Func<EmailAuth, bool>>>(),
                -1,
                -1,
                It.IsAny<Func<IQueryable<EmailAuth>, IOrderedQueryable<EmailAuth>>>(),
                ""
            )).Returns(new List<EmailAuth> { new EmailAuth() });
        }

        #endregion
    }
}
