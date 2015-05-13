using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using fortune_api.App_Start;
using Moq;
using fortune_api.Persistence;
using fortune_api.Models.Auth;
using System.Collections.Generic;
using AutoMapper;
using System.Linq.Expressions;
using System.Linq;
using fortune_api.Dtos.Auth;
using fortune_api.Services.Auth;
using fortune_api.Tests.Test_Start;
using fortune_api.Exceptions;

namespace load_board_api.Tests.Services.Auth
{
    [TestClass]
    public class PermissionServiceTest
    {
        #region Get (All)

        [TestMethod]
        public void GetPermissions()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock repos
            Mock<IRepo<Permission>> mockPermissionRepo = new Mock<IRepo<Permission>>();

            //Test permissions
            Permission[] testPermissions = new Permission[] {
                new Permission
                {
                    Id = Guid.NewGuid(),
                    Name = "Test 1"
                },
                new Permission {
                    Id = Guid.NewGuid(),
                    Name = "Test 2"
                }
            };
            PermissionDto[] testPermissionDtos = Mapper.Map<PermissionDto[]>(testPermissions);

            //Mock call
            mockPermissionRepo.Setup(x => x.Get(
                It.IsAny<Expression<Func<Permission, bool>>>(),
                -1,
                -1,
                It.IsAny<Func<IQueryable<Permission>, IOrderedQueryable<Permission>>>(),
                ""
            )).Returns(testPermissions);

            //Mock unit of work
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.PermissionRepo).Returns(mockPermissionRepo.Object);

            //Permission service
            PermissionService permissionService = new PermissionService(mockUnitOfWork.Object);

            //Test
            PermissionDto[] permissions = permissionService.Get();
            TestUtil.Compare(testPermissionDtos, permissions);
        }

        #endregion

        #region Get

        [TestMethod]
        public void GetPermission()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock repos
            Mock<IRepo<Permission>> mockPermissionRepo = new Mock<IRepo<Permission>>();

            //Test permissions
            Permission testPermission = new Permission {
                Id = Guid.NewGuid(),
                Name = "Test 1"
            };
            PermissionDto testPermissionDto = Mapper.Map<PermissionDto>(testPermission);

            //Mock call
            mockPermissionRepo.Setup(x => x.Get(It.Is<Guid>(y => y == testPermission.Id))).Returns(testPermission);

            //Mock unit of work
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.PermissionRepo).Returns(mockPermissionRepo.Object);

            //Permission service
            PermissionService permissionService = new PermissionService(mockUnitOfWork.Object);

            //Test
            PermissionDto permission = permissionService.Get(testPermission.Id);
            TestUtil.Compare(testPermissionDto, permission);
        }

        [TestMethod]
        [ExpectedException(typeof(DoesNotExistException))]
        public void GetNonexistentPermission()
        {
            //Mock repos
            Mock<IRepo<Permission>> mockPermissionRepo = new Mock<IRepo<Permission>>();

            //Mock call
            mockPermissionRepo.Setup(x => x.Get(It.IsAny<Guid>())).Returns<Permission>(null);

            //Mock unit of work
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.PermissionRepo).Returns(mockPermissionRepo.Object);

            //Permission service
            PermissionService permissionService = new PermissionService(mockUnitOfWork.Object);

            //Test
            permissionService.Get(Guid.NewGuid());
        }

        #endregion

        #region Add

        [TestMethod]
        public void AddPermission()
        {
            //Automapper
            AutoMapperConfig.RegisterMappings();

            //Mock repos
            Mock<IRepo<Permission>> mockPermissionRepo = new Mock<IRepo<Permission>>();

            //Test permissions
            Permission testPermission = new Permission
            {
                Id = Guid.NewGuid(),
                Name = "Test 1"
            };
            PermissionDto testPermissionDto = Mapper.Map<PermissionDto>(testPermission);

            //Mock call
            mockPermissionRepo.Setup(x => x.Exists(It.IsAny<Guid>())).Returns(false);

            //Mock unit of work
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.PermissionRepo).Returns(mockPermissionRepo.Object);

            //Permission service
            PermissionService permissionService = new PermissionService(mockUnitOfWork.Object);

            //Test
            PermissionDto permission = permissionService.Add(testPermissionDto);
            TestUtil.Compare(testPermissionDto, permission, false);
        }

        #endregion

        #region Delete

        [TestMethod]
        public void DeletePermission()
        {
            //Mock repos
            Mock<IRepo<Permission>> mockPermissionRepo = new Mock<IRepo<Permission>>();

            //Mock unit of work
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(x => x.PermissionRepo).Returns(mockPermissionRepo.Object);

            //Permission service
            PermissionService permissionService = new PermissionService(mockUnitOfWork.Object);

            //Test
            permissionService.Delete(Guid.NewGuid());
        }

        #endregion
    }
}
