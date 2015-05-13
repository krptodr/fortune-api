using AutoMapper;
using fortune_api.Dtos.LoadBoard;
using fortune_api.Exceptions;
using fortune_api.Models.LoadBoard;
using fortune_api.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fortune_api.Services.LoadBoard
{
    public class LocationService : ILocationService
    {
        private IUnitOfWork unitOfWork;

        public LocationService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public LocationDto Get(Guid id)
        {
            //Repo dependencies
            IRepo<Location> locationRepo = this.unitOfWork.LocationRepo;

            //Get location from repo
            Location location = locationRepo.Get(id);

            //Ensure location exists
            if (location == null)
            {
                throw new DoesNotExistException();
            }

            //Create dto
            LocationDto dto = Mapper.Map<LocationDto>(location);

            //Return dto
            return dto;
        }

        public LocationDto[] Get(bool includeDeleted = false)
        {
            //Repo dependencies
            IRepo<Location> locationRepo = this.unitOfWork.LocationRepo;

            //Get locations
            IEnumerable<Location> locations;
            if (includeDeleted)
            {
                locations = locationRepo.Get();
            }
            else
            {
                locations = locationRepo.Get(
                    filter: x => x.Deleted == false
                );
            }

            //Create dtos
            LocationDto[] dtos = Mapper.Map<LocationDto[]>(locations);

            //Return dtos
            return dtos;
        }

        public LocationDto Add(LocationDto dto)
        {
            //Repo dependencies
            IRepo<Location> locationRepo = this.unitOfWork.LocationRepo;

            //Check if a location with the same name already exists
            //  If one does, check if it has been soft-deleted.
            //      If so, restore it.
            //      Else throw an exception
            //  Else create new location
            Location location = null;
            IEnumerable<Location> locations = locationRepo.Get(
                filter: x => x.Name == dto.Name
            );
            if (locations.Count() == 0)
            {
                location = Mapper.Map<Location>(dto);
                do
                {
                    location.Id = Guid.NewGuid();
                } while (locationRepo.Exists(location.Id));
                locationRepo.Insert(location);
            }
            else
            {
                location = locations.ElementAt(0);
                if (location.Deleted)
                {
                    location.Deleted = false;
                    location.Name = dto.Name;
                    locationRepo.Update(location);
                }
                else
                {
                    throw new AlreadyExistsException();
                }
            }

            //Create dto
            dto = Mapper.Map<LocationDto>(location);

            //Return dto
            return dto;
        }

        public LocationDto Update(LocationDto dto)
        {
            //Repo dependencies
            IRepo<Location> locationRepo = this.unitOfWork.LocationRepo;

            //Get location from repo
            Location location = locationRepo.Get(dto.Id);

            //Ensure location exists
            if (location == null)
            {
                throw new DoesNotExistException();
            }

            //Update location
            location.Deleted = dto.Deleted;
            location.Name = dto.Name;
            location.RowVersion = dto.RowVersion;
            locationRepo.Update(location);

            //Create dto
            dto = Mapper.Map<LocationDto>(location);

            //Return dto
            return dto;
        }

        public void Delete(Guid id)
        {
            //Repo dependencies
            IRepo<Location> locationRepo = this.unitOfWork.LocationRepo;

            //Get location from repo
            Location location = locationRepo.Get(id);

            //Ensure location exists
            if (location != null)
            {
                //Soft-delete location
                location.Deleted = true;
                locationRepo.Update(location);
            }
        }
    }
}