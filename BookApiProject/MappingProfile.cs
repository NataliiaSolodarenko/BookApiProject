using AutoMapper;
using BookApiProject.Models;
using BookApiProject.AuthDTOs;
using BookApiProject.AuthorDTOs;
using BookApiProject.BookDTOs;

namespace BookApiProject;

/// <summary>
/// AutoMapper profile for mapping between entities and DTOs.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Book mappings
        CreateMap<BookCreateDto, Book>();
        CreateMap<Book, BookReadDto>();
        CreateMap<Book, BookCreateDto>();

        // Author mappings
        CreateMap<AuthorCreateDto, Author>();
        CreateMap<Author, AuthorReadDto>();
        CreateMap<Author, AuthorCreateDto>();

        // User registration mapping
        CreateMap<RegisterDto, User>();
    }
}
