using AutoMapper;
using fortune_api.LoadBoard.Models;
using fortune_api.LoadBoard.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using fortune_api.Models.Auth;
using fortune_api.Dtos.Auth;

namespace fortune_api.App_Start
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            //User
            Mapper.CreateMap<UserProfile, UserDto>();
            Mapper.CreateMap<UserDto, UserProfile>();

            //Permission
            Mapper.CreateMap<Permission, PermissionDto>();
            Mapper.CreateMap<PermissionDto, Permission>();

            //Location
            Mapper.CreateMap<Location, LocationDto>();
            Mapper.CreateMap<LocationDto, Location>();

            //Trailer
            Mapper.CreateMap<Trailer, TrailerDto>();
            Mapper.CreateMap<TrailerDto, Trailer>();

            //Load
            Mapper.CreateMap<Load, LoadDto>();
            Mapper.CreateMap<LoadDto, Load>();
        }
    }
}