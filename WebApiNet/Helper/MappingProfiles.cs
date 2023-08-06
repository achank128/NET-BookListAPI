
using AutoMapper;
using WebApiNet.Dto;
using WebApiNet.Models;

namespace WebApiNet.Helper
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			CreateMap<Book, BookDto>().ReverseMap();
		}
	}
}
