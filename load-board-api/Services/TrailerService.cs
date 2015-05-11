using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using load_board_api.Dtos;
using load_board_api.Models;
using load_board_api.Persistence;
using load_board_api.Exceptions;
using AutoMapper;

namespace load_board_api.Services
{
    public class TrailerService : ITrailerService
    {
        private IUnitOfWork unitOfWork;

        public TrailerService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public TrailerDto Get(int id)
        {
            //Repo dependencies
            IRepo<Trailer> trailerRepo = this.unitOfWork.TrailerRepo;
            IRepo<Location> locationRepo = this.unitOfWork.LocationRepo;

            //Get trailer from repo
            Trailer trailer = trailerRepo.Get(id);

            //Ensure trailer exists
            if (trailer == null)
            {
                throw new DoesNotExistException();
            }

            //Get location from repo
            Location location = locationRepo.Get(trailer.LocationId);

            //Create dto
            TrailerDto dto = Mapper.Map<TrailerDto>(trailer);
            if (location != null)
            {
                dto.Location = Mapper.Map<LocationDto>(location);
            }

            //Return dto
            return dto;
        }

        public TrailerDto[] Get(bool includeDeleted = false, int skip = -1, int num = -1)
        {
            //Repo dependencies
            IRepo<Trailer> trailerRepo = this.unitOfWork.TrailerRepo;
            IRepo<Location> locationRepo = this.unitOfWork.LocationRepo;

            //Get trailers
            IEnumerable<Trailer> trailers;
            if (includeDeleted)
            {
                trailers = trailerRepo.Get(
                    orderBy: x => x.OrderBy(y => y.Id),
                    skip: skip,
                    num: num
                );
            }
            else
            {
                trailers = trailerRepo.Get(
                    orderBy: x => x.OrderBy(y => y.Id),
                    filter: x => x.Deleted == false,
                    skip: skip,
                    num: num
                );
            }

            //Create dtos
            TrailerDto[] dtos = Mapper.Map<TrailerDto[]>(trailers);

            //Get locations
            int numTrailers = dtos.Length;
            Location location;
            for (int i = 0; i < numTrailers; i++)
            {
                location = locationRepo.Get(trailers.ElementAt(i).LocationId);
                if (location != null)
                {
                    dtos[i].Location = Mapper.Map<LocationDto>(location);
                }
            }

            //Return dtos
            return dtos;
        }

        public TrailerDto Add(TrailerDto dto)
        {
            //Repo dependencies
            IRepo<Location> locationRepo = this.unitOfWork.LocationRepo;
            IRepo<Trailer> trailerRepo = this.unitOfWork.TrailerRepo;

            //Get location
            Location location = locationRepo.Get(dto.Location.Id);

            //Ensure location exists
            if (location == null)
            {
                throw new DoesNotExistException();
            }

            //Check if trailer trailer exists
            //  If it does, check if it has been soft-deleted
            //      If so, restore it
            //      Else throw exception
            //  Else add new trailer
            Trailer trailer = trailerRepo.Get(dto.Id);
            if (trailer == null)
            {
                trailer = Mapper.Map<Trailer>(dto);
                trailer.LastUpdated = DateTime.UtcNow;
                trailerRepo.Insert(trailer);
            }
            else if (trailer.Deleted)
            {
                trailer.Deleted = false;
                trailer.LocationId = dto.Location.Id;
                trailer.LastUpdated = DateTime.UtcNow;
                trailerRepo.Update(trailer);
            }
            else
            {
                throw new AlreadyExistsException();
            }

            //Create dto
            dto = Mapper.Map<TrailerDto>(trailer);
            dto.Location = Mapper.Map<LocationDto>(location);

            //Return dto
            return dto;
        }

        public TrailerDto Update(TrailerDto dto)
        {
            //Repo dependencies
            IRepo<Location> locationRepo = this.unitOfWork.LocationRepo;
            IRepo<Trailer> trailerRepo = this.unitOfWork.TrailerRepo;

            //Get location
            Location location = locationRepo.Get(dto.Location.Id);

            //Ensure location exists
            if (location == null)
            {
                throw new DoesNotExistException();
            }

            //Get trailer from repo
            Trailer trailer = trailerRepo.Get(dto.Id);

            //Ensure trailer exists
            if (trailer == null)
            {
                throw new DoesNotExistException();
            }

            //Prevent conflict
            if (trailer.LastUpdated > dto.LastUpdated)
            {
                throw new ConflictException();
            }

            //Update trailer
            trailer.LocationId = dto.Location.Id;
            trailer.Deleted = dto.Deleted;
            trailer.LastUpdated = DateTime.UtcNow;
            trailerRepo.Update(trailer);

            //Create dto
            dto = Mapper.Map<TrailerDto>(trailer);
            dto.Location = Mapper.Map<LocationDto>(location);

            //Return dto
            return dto;
        }

        public void Delete(int id)
        {
            //Repo dependencies
            IRepo<Trailer> trailerRepo = this.unitOfWork.TrailerRepo;

            //Get trailer from repo
            Trailer trailer = trailerRepo.Get(id);

            //Ensure trailer exists
            if (trailer != null)
            {
                //Soft-delete trailer
                trailer.Deleted = true;
                trailer.LastUpdated = DateTime.UtcNow;
                trailerRepo.Update(trailer);
            }
        }
    }
}