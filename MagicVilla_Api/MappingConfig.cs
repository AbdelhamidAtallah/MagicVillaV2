﻿using AutoMapper;
using MagicVilla_Api.Models;
using System.Runtime;

namespace MagicVilla_Api
{
    public class MappingConfig : Profile 
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDTO>().ReverseMap();
            CreateMap<Villa, VillaCreateDTO>().ReverseMap();
            CreateMap<Villa, VillaUpdateDTO>().ReverseMap();
        }
    }
}
