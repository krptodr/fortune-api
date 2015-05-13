using AutoMapper;
using fortune_api.Dtos.Auth;
using fortune_api.Exceptions;
using fortune_api.Models.Auth;
using fortune_api.Persistence;
using fortune_api.Services.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace fortune_api.Services.Auth
{
    public class AuthService : IAuthService
    {
        private static readonly int JWT_LIFETIME_IN_HOURS = Convert.ToInt32(ConfigurationManager.AppSettings["JWT_LIFETIME_IN_HOURS"]);

        private IHasher hasher;
        private IUnitOfWork unitOfWork;
        private IJwtService jwtService;

        public AuthService(IHasher hasher, IUnitOfWork unitOfWork, IJwtService jwtService)
        {
            Contract.Assert(hasher != null);
            Contract.Assert(unitOfWork != null);
            Contract.Assert(jwtService != null);

            this.hasher = hasher;
            this.unitOfWork = unitOfWork;
            this.jwtService = jwtService;
        }

        public LoginRes LogInViaEmail(string email, string password)
        {
            //Required repos
            IRepo<EmailAuth> emailAuthRepo = this.unitOfWork.EmailAuthRepo;
            IRepo<UserProfile> userProfileRepo = this.unitOfWork.UserProfileRepo;

            //Get auth with email
            IEnumerable<EmailAuth> auths = emailAuthRepo.Get(x => x.Email == email);
            if (auths.Count() == 0)
            {
                throw new UnauthorizedException();
            }
            EmailAuth auth = auths.ElementAt(0);

            //Check password
            if (!this.hasher.CompareWithHash(password, auth.HashedPassword))
            {
                throw new UnauthorizedException();
            }

            //Get user
            UserProfile user = userProfileRepo.Get(auth.UserId);
            UserDto userDto = Mapper.Map<UserDto>(user);

            //Create jwt
            string jwt = this.jwtService.CreateToken(
                user.Id.ToString(),
                JwtService.DEFAULT_ISSUER,
                JwtService.DEFAULT_AUDIENCE,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(JWT_LIFETIME_IN_HOURS),
                new Dictionary<string, string> { }
            );

            //Return login response
            LoginRes loginRes = new LoginRes
            {
                JWT = jwt,
                User = userDto
            };
            return loginRes;
        }

        public LoginRes RegisterEmail(Guid userId, string email, string password)
        {
            //Required repos
            IRepo<EmailAuth> emailAuthRepo = this.unitOfWork.EmailAuthRepo;
            IRepo<UserProfile> userProfileRepo = this.unitOfWork.UserProfileRepo;

            //Ensure user exists
            UserProfile user = userProfileRepo.Get(userId);
            if (user == null)
            {
                throw new DoesNotExistException();
            }

            //Check if email is already registered
            IEnumerable<EmailAuth> auths = emailAuthRepo.Get(x => x.Email == email);
            if (auths.Count() > 0)
            {
                throw new AlreadyExistsException();
            }

            //Hash password
            password = this.hasher.Hash(password);

            //Register email
            EmailAuth auth = new EmailAuth
            {
                UserId = userId,
                Email = email,
                HashedPassword = password
            };
            do
            {
                auth.Id = Guid.NewGuid();
            } while (emailAuthRepo.Exists(auth.Id));
            emailAuthRepo.Insert(auth);

            //Create UserDto
            UserDto userDto = Mapper.Map<UserDto>(user);

            //Create jwt
            string jwt = this.jwtService.CreateToken(
                user.Id.ToString(),
                JwtService.DEFAULT_ISSUER,
                JwtService.DEFAULT_AUDIENCE,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(JWT_LIFETIME_IN_HOURS),
                new Dictionary<string, string> { }
            );

            //Return login response
            LoginRes loginRes = new LoginRes
            {
                JWT = jwt,
                User = userDto
            };
            return loginRes;
        }

        public void RemoveUsersAuthMethods(Guid userId)
        {
            //Required repos
            IRepo<EmailAuth> emailAuthRepo = this.unitOfWork.EmailAuthRepo;

            //Hard delete email auths
            IEnumerable<EmailAuth> auths = emailAuthRepo.Get(x => x.UserId == userId);
            foreach (EmailAuth auth in auths)
            {
                emailAuthRepo.Delete(auth);
            }
        }
    }
}