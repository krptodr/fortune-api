using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using fortune_api.Dtos.Auth;
using fortune_api.Models.Auth;
using fortune_api.Persistence;
using fortune_api.Exceptions;
using AutoMapper;
using System.Diagnostics.Contracts;

namespace fortune_api.Services.Auth
{
    public class UserService : IUserService
    {
        private IUnitOfWork unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            Contract.Assert(unitOfWork != null);

            this.unitOfWork = unitOfWork;
        }

        public UserDto[] Get()
        {
            //Required repos
            IRepo<UserProfile> userProfileRepo = this.unitOfWork.UserProfileRepo;

            //Get users
            UserProfile[] users = userProfileRepo.Get().ToArray();

            //Return users
            UserDto[] userDtos = Mapper.Map<UserDto[]>(users);
            return userDtos;
        }

        public UserDto Get(Guid id)
        {
            //Required repos
            IRepo<UserProfile> userProfileRepo = this.unitOfWork.UserProfileRepo;

            //Get user
            UserProfile user = userProfileRepo.Get(id);
            if (user == null)
            {
                throw new DoesNotExistException();
            }

            //Return user
            UserDto userDto = Mapper.Map<UserDto>(user);
            return userDto;
        }

        public UserDto Add(UserDto userDto)
        {
            //Required repos
            IRepo<UserProfile> userProfileRepo = this.unitOfWork.UserProfileRepo;

            //Map UserDto to UserProfile
            UserProfile user = Mapper.Map<UserProfile>(userDto);

            //Generate Id
            do
            {
                user.Id = Guid.NewGuid();
            } while (userProfileRepo.Exists(user.Id));
            

            //Add user
            userProfileRepo.Insert(user);

            //Return user
            userDto = Mapper.Map<UserDto>(user);
            return userDto;
        }

        public UserDto Update(UserDto userDto)
        {
            //Required repos
            IRepo<UserProfile> userProfileRepo = this.unitOfWork.UserProfileRepo;

            //Get user
            UserProfile user = userProfileRepo.Get(userDto.Id);

            //Update user
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Deleted = userDto.Deleted;
            user.RowVersion = userDto.RowVersion;
            user.Permissions.Clear();
            user.Permissions.AddRange(Mapper.Map<List<Permission>>(userDto.Permissions));

            //Return user
            userDto = Mapper.Map<UserDto>(user);
            return userDto;
        }

        public void Delete(Guid id)
        {
            //Required repos
            IRepo<UserProfile> userProfileRepo = this.unitOfWork.UserProfileRepo;

            //Soft delete user
            UserProfile user = userProfileRepo.Get(id);
            user.Deleted = true;
        }
    }
}