using AutoMapper;
using load_board_api.Dtos;
using load_board_api.Exceptions;
using load_board_api.Models;
using load_board_api.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace load_board_api.Services
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
            IRepo<Location> locationRepo = unitOfWork.LocationRepo;

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

        public LocationDto[] Get()
        {
            return Get(false);
        }

        public LocationDto[] Get(bool includeDeleted)
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
                location.Id = Guid.NewGuid();
                location.LastUpdated = DateTime.UtcNow;
                locationRepo.Insert(location);
            }
            else
            {
                location = locations.ElementAt(0);
                if (location.Deleted)
                {
                    location.Deleted = false;
                    location.Name = dto.Name;
                    location.LastUpdated = DateTime.UtcNow;
                    locationRepo.Update(location);
                }
                else
                {
                    throw new AlreadyExistsException();
                }
            }

            //Save changes
            this.unitOfWork.Save();

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
            
            //Ensure data is up-to-date
            if (location.LastUpdated > dto.LastUpdated)
            {
                throw new OutdatedDataException();
            }

            //Update location
            location.Deleted = dto.Deleted;
            location.LastUpdated = DateTime.UtcNow;
            location.Name = dto.Name;
            locationRepo.Update(location);

            //Save changes
            this.unitOfWork.Save();

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
                location.LastUpdated = DateTime.UtcNow;
                locationRepo.Update(location);

                //Save changes
                this.unitOfWork.Save();
            }
        }
    }
}