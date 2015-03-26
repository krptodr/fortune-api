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

            //Get trailer from repo
            Trailer trailer = trailerRepo.Get(id);

            //Ensure trailer exists
            if (trailer == null)
            {
                throw new DoesNotExistException();
            }

            //Create dto
            TrailerDto dto = Mapper.Map<TrailerDto>(trailer);

            //Return dto
            return dto;
        }

        public TrailerDto[] Get(bool includeDeleted = false, int skip = -1, int num = -1)
        {
            //Repo dependencies
            IRepo<Trailer> trailerRepo = this.unitOfWork.TrailerRepo;

            //Get trailers
            IEnumerable<Trailer> trailers;
            if (includeDeleted)
            {
                trailers = trailerRepo.Get(
                    skip: skip,
                    num: num
                );
            }
            else
            {
                trailers = trailerRepo.Get(
                    filter: x => x.Deleted == false,
                    skip: skip,
                    num: num
                );
            }

            //Create dtos
            TrailerDto[] dtos = Mapper.Map<TrailerDto[]>(trailers);

            //Return dtos
            return dtos;
        }

        public TrailerDto Add(TrailerDto dto)
        {
            //Repo dependencies
            IRepo<Location> locationRepo = this.unitOfWork.LocationRepo;
            IRepo<Trailer> trailerRepo = this.unitOfWork.TrailerRepo;

            //Ensure location exists
            if (!locationRepo.Exists(dto.Location.Id))
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

            //Save changes
            this.unitOfWork.Save();

            //Create dto
            dto = Mapper.Map<TrailerDto>(trailer);

            //Return dto
            return dto;
        }

        public TrailerDto Update(TrailerDto dto)
        {
            //Repo dependencies
            IRepo<Location> locationRepo = this.unitOfWork.LocationRepo;
            IRepo<Trailer> trailerRepo = this.unitOfWork.TrailerRepo;

            //Get trailer from repo
            Trailer trailer = trailerRepo.Get(dto.Id);

            //Ensure trailer and location exist
            if (trailer == null || !locationRepo.Exists(dto.Location.Id))
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

            //Save changes
            this.unitOfWork.Save();

            //Create dto
            dto = Mapper.Map<TrailerDto>(trailer);

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

                //Save changes
                this.unitOfWork.Save();
            }
        }
    }
}