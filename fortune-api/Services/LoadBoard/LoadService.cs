using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using fortune_api.Dtos.LoadBoard;
using fortune_api.Models.LoadBoard;
using fortune_api.Persistence;
using fortune_api.Exceptions;
using AutoMapper;
using fortune_api.Enums.LoadBoard;

namespace fortune_api.Services.LoadBoard
{
    public class LoadService : ILoadService
    {
        IUnitOfWork unitOfWork;

        public LoadService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public LoadDto Get(Guid id)
        {
            //Repo dependencies
            IRepo<Location> locationRepo = this.unitOfWork.LocationRepo;
            IRepo<Trailer> trailerRepo = this.unitOfWork.TrailerRepo;
            IRepo<Load> loadRepo = this.unitOfWork.LoadRepo;

            //Get load from repo
            Load load = loadRepo.Get(id);
            if (load == null)
            {
                throw new DoesNotExistException();
            }

            //Get trailer from repo
            Trailer trailer = null;
            if (load.TrailerId > 0)
            {
                trailer = trailerRepo.Get(load.TrailerId);
            }

            //Get trailer location from repo
            Location trailerLocation = null;
            if(trailer != null) {
                trailerLocation = locationRepo.Get(trailer.LocationId);
            }

            //Get origin from repo
            Location origin = null;
            if (load.OriginId != null)
            {
                origin = locationRepo.Get(load.OriginId);
            }

            //Get destination from repo
            Location destination = null;
            if (load.DestinationId != null)
            {
                destination = locationRepo.Get(load.DestinationId);
            }

            //Create dto
            LoadDto dto = Mapper.Map<LoadDto>(load);
            if (trailer != null)
            {
                dto.Trailer = Mapper.Map<TrailerDto>(trailer);
            }
            if (trailerLocation != null)
            {
                dto.Trailer.Location = Mapper.Map<LocationDto>(trailerLocation);
            }
            if (origin != null)
            {
                dto.Origin = Mapper.Map<LocationDto>(origin);
            }
            if (destination != null)
            {
                dto.Destination = Mapper.Map<LocationDto>(destination);
            }

            //Return dto
            return dto;

        }

        public LoadDto[] Get(bool includeDeleted = false, int skip = -1, int num = -1)
        {
            //Repo dependencies
            IRepo<Location> locationRepo = this.unitOfWork.LocationRepo;
            IRepo<Trailer> trailerRepo = this.unitOfWork.TrailerRepo;
            IRepo<Load> loadRepo = this.unitOfWork.LoadRepo;

            //Get loads
            IEnumerable<Load> loads;
            if (includeDeleted)
            {
                loads = loadRepo.Get(
                    orderBy: x => x.OrderBy(y => y.Id),
                    skip: skip,
                    num: num
                );
            } 
            else
            {
                loads = loadRepo.Get(
                    orderBy: x => x.OrderBy(y => y.Id),
                    filter: x => x.Status != LoadStatus.Complete,
                    skip: skip,
                    num: num
                );
            }

            //Create dtos
            LoadDto[] dtos = Mapper.Map<LoadDto[]>(loads);
            
            //Get subclasses
            int numLoads = dtos.Length;
            Load load = null;
            LoadDto dto = null;
            Location location = null;
            Trailer trailer = null;
            for (int i = 0; i < numLoads; i++)
            {
                load = loads.ElementAt(i);
                dto = dtos[i];

                //Get trailer
                if (load.TrailerId != null)
                {
                    trailer = trailerRepo.Get(load.TrailerId);
                }
                if (trailer != null)
                {
                    dto.Trailer = Mapper.Map<TrailerDto>(trailer);
                }

                //Get trailer location
                if (trailer != null)
                {
                    location = locationRepo.Get(trailer.LocationId);
                }
                if (location != null)
                {
                    dto.Trailer.Location = Mapper.Map<LocationDto>(location);
                    location = null;
                }

                //Get origin
                if (load.OriginId != null)
                {
                    location = locationRepo.Get(load.OriginId);
                }
                if (location != null)
                {
                    dto.Origin = Mapper.Map<LocationDto>(location);
                    location = null;
                }

                //Get destination
                if (load.DestinationId != null)
                {
                    location = locationRepo.Get(load.DestinationId);
                }
                if (location != null)
                {
                    dto.Destination = Mapper.Map<LocationDto>(location);
                }
            }

            //Return dtos
            return dtos;
        }

        public LoadDto Add(LoadDto dto)
        {
            //Repo dependencies
            IRepo<Location> locationRepo = this.unitOfWork.LocationRepo;
            IRepo<Trailer> trailerRepo = this.unitOfWork.TrailerRepo;
            IRepo<Load> loadRepo = this.unitOfWork.LoadRepo;

            //Ensure trailer exists
            Trailer trailer = null;
            if (dto.Trailer != null)
            {
                trailer = trailerRepo.Get(dto.Trailer.Id);
                if (trailer == null)
                {
                    throw new DoesNotExistException();
                }
            }

            //Ensure trailer location exists
            Location trailerLocation = null;
            if (trailer != null)
            {
                trailerLocation = locationRepo.Get(trailer.LocationId);
                if (trailerLocation == null)
                {
                    throw new DoesNotExistException();
                }
            }

            //Ensure origin exists
            Location origin = null;
            if (dto.Origin != null)
            {
                origin = locationRepo.Get(dto.Origin.Id);
                if (origin == null)
                {
                    throw new DoesNotExistException();
                }
            }

            //Ensure destination exists
            Location destination = null;
            if (dto.Destination != null)
            {
                destination = locationRepo.Get(dto.Destination.Id);
                if (destination == null)
                {
                    throw new DoesNotExistException();
                }
            }

            //Check if load exists
            //  If it does, check if it has been soft-deleted
            //      If so, restore it
            //      Else throw exception
            //  Else add new load
            Load load = loadRepo.Get(dto.Id);
            if (load == null)
            {
                load = Mapper.Map<Load>(dto);
                do
                {
                    load.Id = Guid.NewGuid();
                } while (loadRepo.Exists(load.Id));
                loadRepo.Insert(load);
            }
            else if (load.Deleted)
            {
                load.Deleted = false;
                load.Appointment = dto.Appointment;
                load.ArrivalTime = dto.ArrivalTime;
                load.CfNum = dto.CfNum;
                load.DepartureTime = dto.DepartureTime;
                load.DestinationId = dto.Destination.Id;
                load.OriginId = dto.Origin.Id;
                load.PuNum = dto.PuNum;
                load.Status = dto.Status;
                load.TrailerId = dto.Trailer.Id;
                load.Type = dto.Type;
                loadRepo.Update(load);
            }
            else
            {
                throw new AlreadyExistsException();
            }

            //Create dto
            dto = Mapper.Map<LoadDto>(load);
            if (trailer != null)
            {
                dto.Trailer = Mapper.Map<TrailerDto>(trailer);
            }
            if (trailerLocation != null)
            {
                dto.Trailer.Location = Mapper.Map<LocationDto>(trailerLocation);
            }
            if (origin != null)
            {
                dto.Origin = Mapper.Map<LocationDto>(origin);
            }
            if (destination != null)
            {
                dto.Destination = Mapper.Map<LocationDto>(destination);
            }

            //Return dto
            return dto;
        }

        public LoadDto Update(LoadDto dto)
        {
            //Repo dependencies
            IRepo<Location> locationRepo = this.unitOfWork.LocationRepo;
            IRepo<Trailer> trailerRepo = this.unitOfWork.TrailerRepo;
            IRepo<Load> loadRepo = this.unitOfWork.LoadRepo;

            //Ensure trailer exists
            Trailer trailer = null;
            if (dto.Trailer != null)
            {
                trailer = trailerRepo.Get(dto.Trailer.Id);
                if (trailer == null)
                {
                    throw new DoesNotExistException();
                }
            }

            //Ensure trailer location exists
            Location trailerLocation = null;
            if (trailer != null)
            {
                trailerLocation = locationRepo.Get(trailer.LocationId);
                if (trailerLocation == null)
                {
                    throw new DoesNotExistException();
                }
            }

            //Ensure origin exists
            Location origin = null;
            if (dto.Origin != null)
            {
                origin = locationRepo.Get(dto.Origin.Id);
                if (origin == null)
                {
                    throw new DoesNotExistException();
                }
            }

            //Ensure destination exists
            Location destination = null;
            if (dto.Destination != null)
            {
                destination = locationRepo.Get(dto.Destination.Id);
                if (destination == null)
                {
                    throw new DoesNotExistException();
                }
            }

            //Get load from repo
            Load load = loadRepo.Get(dto.Id);

            //Ensure load exists
            if (load == null)
            {
                throw new DoesNotExistException();
            }

            //Update load
            load.Appointment = dto.Appointment;
            load.ArrivalTime = dto.ArrivalTime;
            load.CfNum = dto.CfNum;
            load.Deleted = dto.Deleted;
            load.DepartureTime = dto.DepartureTime;
            load.DestinationId = dto.Destination.Id;
            load.OriginId = dto.Origin.Id;
            load.PuNum = dto.PuNum;
            load.Status = dto.Status;
            load.TrailerId = dto.Trailer.Id;
            load.Type = dto.Type;
            load.RowVersion = dto.RowVersion;
            loadRepo.Update(load);

            //Create dto
            dto = Mapper.Map<LoadDto>(load);
            if (trailer != null)
            {
                dto.Trailer = Mapper.Map<TrailerDto>(trailer);
            }
            if (trailerLocation != null)
            {
                dto.Trailer.Location = Mapper.Map<LocationDto>(trailerLocation);
            }
            if (origin != null)
            {
                dto.Origin = Mapper.Map<LocationDto>(origin);
            }
            if (destination != null)
            {
                dto.Destination = Mapper.Map<LocationDto>(destination);
            }

            //Return dto
            return dto;
        }

        public void Delete(Guid id)
        {
            //Repo dependencies
            IRepo<Load> loadRepo = this.unitOfWork.LoadRepo;

            //Get load from repo 
            Load load = loadRepo.Get(id);

            //Ensure load exists
            if (load != null)
            {
                //Soft-delete load
                load.Deleted = true;
                loadRepo.Update(load);
            }
        }
    }
}