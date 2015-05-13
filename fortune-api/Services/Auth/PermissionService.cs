using AutoMapper;
using fortune_api.Dtos.Auth;
using fortune_api.Exceptions;
using fortune_api.Models.Auth;
using fortune_api.Persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace fortune_api.Services.Auth
{
    public class PermissionService : IPermissionService
    {
        private IUnitOfWork unitOfWork;

        public PermissionService(IUnitOfWork unitOfWork)
        {
            Contract.Assert(unitOfWork != null);

            this.unitOfWork = unitOfWork;
        }

        public PermissionDto[] Get()
        {
            //Required repos
            IRepo<Permission> permissionRepo = this.unitOfWork.PermissionRepo;

            //Get permissions
            Permission[] permissions = permissionRepo.Get().ToArray(); 

            //Convert to dtos
            PermissionDto[] dtos = Mapper.Map<PermissionDto[]>(permissions);

            //Return dtos
            return dtos;
        }

        public PermissionDto Get(Guid id)
        {
            //Required repos
            IRepo<Permission> permissionRepo = this.unitOfWork.PermissionRepo;

            //Get permission
            Permission permission = permissionRepo.Get(id);
            if (permission == null)
            {
                throw new DoesNotExistException();
            }

            //Convert to dto
            PermissionDto dto = Mapper.Map<PermissionDto>(permission);

            //Return dto
            return dto;
        }

        public PermissionDto Add(PermissionDto dto)
        {
            //Required repos
            IRepo<Permission> permissionRepo = this.unitOfWork.PermissionRepo;

            //Convert to permission
            Permission permission = Mapper.Map<Permission>(dto);
            do
            {
                permission.Id = Guid.NewGuid();
            } while (permissionRepo.Exists(permission.Id));
            

            //Add permission
            permissionRepo.Insert(permission);

            //Convert to dto
            dto = Mapper.Map<PermissionDto>(permission);

            //Return dto
            return dto;
        }

        public void Delete(Guid id)
        {
            //Required repos
            IRepo<Permission> permissionRepo = this.unitOfWork.PermissionRepo;

            //Delete permission
            permissionRepo.Delete(id);
        }
    }
}