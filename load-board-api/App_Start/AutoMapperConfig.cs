using AutoMapper;
using load_board_api.Models;
using load_board_api.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace load_board_api.App_Start
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
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