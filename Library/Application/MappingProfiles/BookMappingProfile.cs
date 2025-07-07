using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings;

public class BookMappingProfile : Profile
{
    public BookMappingProfile()
    {
        CreateMap<Book, BookDto>()
            .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.Isbn.Value))
            .ForMember(
                dest => dest.BorrowedCopies,
                opt => opt.MapFrom(src => src.GetBorrowedCopies())
            )
            .ForMember(
                dest => dest.AverageReadingRate,
                opt => opt.MapFrom(src => src.CalculateAverageReadingRate())
            );

        CreateMap<Book, BookSummaryDto>()
            .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.Isbn.Value));
    }
}
