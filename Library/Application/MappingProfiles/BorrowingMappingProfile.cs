using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings;

public class BorrowingMappingProfile : Profile
{
    public BorrowingMappingProfile()
    {
        CreateMap<Borrowing, BorrowingDto>()
            .ForMember(
                dest => dest.OverdueDays,
                opt => opt.MapFrom(src => src.IsOverdue ? (DateTime.UtcNow - src.DueDate).Days : 0)
            )
            .ForMember(dest => dest.LateFee, opt => opt.MapFrom(src => src.CalculateLateFee(1.0m)))
            .ForMember(
                dest => dest.BorrowDurationDays,
                opt =>
                    opt.MapFrom(src =>
                        src.IsReturned
                            ? (src.ReturnDate!.Value - src.BorrowDate).Days
                            : (DateTime.UtcNow - src.BorrowDate).Days
                    )
            )
            .ForMember(
                dest => dest.ReadingRate,
                opt =>
                    opt.MapFrom(src =>
                        src.Book != null ? src.CalculateReadingRate(src.Book.PageCount) : 0
                    )
            )
            .ForMember(dest => dest.Book, opt => opt.MapFrom(src => src.Book))
            .ForMember(dest => dest.Borrower, opt => opt.MapFrom(src => src.Borrower));

        CreateMap<Borrowing, BorrowingHistoryDto>()
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Book.Author))
            .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.Book.Isbn.Value))
            .ForMember(
                dest => dest.BorrowDurationDays,
                opt =>
                    opt.MapFrom(src =>
                        src.IsReturned
                            ? (src.ReturnDate!.Value - src.BorrowDate).Days
                            : (DateTime.UtcNow - src.BorrowDate).Days
                    )
            )
            .ForMember(
                dest => dest.ReadingRate,
                opt =>
                    opt.MapFrom(src =>
                        src.Book != null ? src.CalculateReadingRate(src.Book.PageCount) : 0
                    )
            );
    }
}
