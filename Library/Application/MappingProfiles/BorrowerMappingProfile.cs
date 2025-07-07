using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings;

public class BorrowerMappingProfile : Profile
{
    public BorrowerMappingProfile()
    {
        CreateMap<Borrower, BorrowerDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
            .ForMember(
                dest => dest.CurrentBorrowedCount,
                opt => opt.MapFrom(src => src.CurrentBorrowedCount)
            )
            .ForMember(
                dest => dest.TotalBooksRead,
                opt => opt.MapFrom(src => src.GetTotalBooksRead())
            )
            .ForMember(
                dest => dest.AverageReadingRate,
                opt => opt.MapFrom(src => src.GetAverageReadingRate())
            );

        CreateMap<Borrower, BorrowerSummaryDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value));
    }
}
