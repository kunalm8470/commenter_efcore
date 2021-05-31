using Api.Models.Requests;
using Api.Models.Responses;
using AutoMapper;
using Core.Entities;
using Core.Extensions;
using Core.Services;
using System;
using System.Linq;

namespace Api.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile(IPasswordService passwordService)
        {
            string indianTimezoneId = "India Standard Time";

            // Users mapping
            CreateMap<RegisterUserDto, User>()
                .ForMember(dto => dto.FirstName, options => options.MapFrom(src => src.FirstName))
                .ForMember(dto => dto.LastName, options => options.MapFrom(src => src.LastName))
                .ForMember(dto => dto.GenderAbbreviation, options => options.MapFrom(src => src.GenderAbbreviation))
                .ForMember(dto => dto.DateOfBirth, options => options.MapFrom(src => src.DateOfBirth.ToUniversalTime()))
                .ForMember(dto => dto.Phone, options => options.MapFrom(src => src.Phone))
                .ForMember(dto => dto.Email, options => options.MapFrom(src => src.Email))
                .ForMember(dto => dto.Password, options => options.MapFrom(src => passwordService.HashPassword(src.Password)))
                .ForMember(dto => dto.CreatedAt, options => options.MapFrom(_ => DateTime.UtcNow));

            CreateMap<User, UserResponseDto>()
                .ForMember(dto => dto.FirstName, options => options.MapFrom(src => src.FirstName))
                .ForMember(dto => dto.LastName, options => options.MapFrom(src => src.LastName))
                .ForMember(dto => dto.GenderAbbreviation, options => options.MapFrom(src => src.GenderAbbreviation))
                .ForMember(dto => dto.DateOfBirth, options => options.MapFrom(src => src.DateOfBirth.ConvertToLocalFromUTC(indianTimezoneId)))
                .ForMember(dto => dto.Phone, options => options.MapFrom(src => src.Phone))
                .ForMember(dto => dto.Username, options => options.MapFrom(src => src.Username))
                .ForMember(dto => dto.Email, options => options.MapFrom(src => src.Email))
                .ForMember(dto => dto.Posts, options => options.MapFrom(src => src.Posts.Select(p => new UserPost
                {
                    PostId = p.Id,
                    Body = p.Body,
                    Title = p.Title,
                    CreatedAt = p.CreatedAt.ConvertToLocalFromUTC(indianTimezoneId),
                    UpdatedAt = p.UpdatedAt.HasValue
                        ? p.UpdatedAt.Value.ConvertToLocalFromUTC(indianTimezoneId)
                        : p.UpdatedAt.GetValueOrDefault()
                })))
                .ForMember(dto => dto.Comments, options => options.MapFrom(src => src.Posts.Select(c => new UserComment
                {
                    CommentId = c.Id,
                    Body = c.Body,
                    CreatedAt = c.CreatedAt.ConvertToLocalFromUTC(indianTimezoneId),
                    UpdatedAt = c.UpdatedAt.HasValue
                        ? c.UpdatedAt.Value.ConvertToLocalFromUTC(indianTimezoneId)
                        : c.UpdatedAt.GetValueOrDefault()
                })))
                .ForMember(dto => dto.CreatedAt, options => options.MapFrom(src => src.CreatedAt.ConvertToLocalFromUTC(indianTimezoneId)))
                .ForMember(dto => dto.UpdatedAt, options => options.MapFrom(src => src.UpdatedAt.HasValue
                        ? src.UpdatedAt.Value.ConvertToLocalFromUTC(indianTimezoneId)
                        : src.UpdatedAt.GetValueOrDefault()));

            // Posts mapping
            CreateMap<AddPostDto, Post>()
                .ForMember(dto => dto.Title, options => options.MapFrom(src => src.Title))
                .ForMember(dto => dto.Body, options => options.MapFrom(src => src.Body))
                .ForMember(dto => dto.UserId, options => options.MapFrom(src => src.UserId))
                .ForMember(dto => dto.CreatedAt, options => options.MapFrom(_ => DateTime.UtcNow));

            CreateMap<UpdatePostDto, Post>()
                .ForMember(dto => dto.Id, options => options.MapFrom(src => src.Id))
                .ForMember(dto => dto.Title, options => options.MapFrom(src => src.Title))
                .ForMember(dto => dto.Body, options => options.MapFrom(src => src.Body))
                .ForMember(dto => dto.UserId, options => options.MapFrom(src => src.UserId))
                .ForMember(dto => dto.UpdatedAt, options => options.MapFrom(_ => DateTime.UtcNow));

            CreateMap<Post, PostResponseDto>()
                .ForMember(dto => dto.Id, options => options.MapFrom(src => src.Id))
                .ForMember(dto => dto.Title, options => options.MapFrom(src => src.Title))
                .ForMember(dto => dto.Body, options => options.MapFrom(src => src.Body))
                .ForMember(dto => dto.User, options => options.MapFrom(src => new UserResponseBase
                {
                    Id = src.User.Id,
                    FirstName = src.User.FirstName,
                    LastName = src.User.LastName,
                    GenderAbbreviation = src.User.GenderAbbreviation,
                    DateOfBirth = src.User.DateOfBirth.ConvertToLocalFromUTC(indianTimezoneId),
                    Phone = src.User.Phone,
                    Username = src.User.Username,
                    Email = src.User.Email,
                    CreatedAt = src.User.CreatedAt.ConvertToLocalFromUTC(indianTimezoneId),
                    UpdatedAt = src.User.UpdatedAt.HasValue
                        ? src.User.UpdatedAt.Value.ConvertToLocalFromUTC(indianTimezoneId)
                        : src.User.UpdatedAt.GetValueOrDefault()
                }))
                .ForMember(dto => dto.Comments, options => options.MapFrom(src => src.Comments.Select(c => new PostComment
                {
                    CommentId = c.Id,
                    Body = c.Body,
                    Commenter = new UserResponseBase
                    {
                        Id = c.User.Id,
                        FirstName = c.User.FirstName,
                        LastName = c.User.LastName,
                        GenderAbbreviation = c.User.GenderAbbreviation,
                        DateOfBirth = c.User.DateOfBirth.ConvertToLocalFromUTC(indianTimezoneId),
                        Phone = c.User.Phone,
                        Username = c.User.Username,
                        Email = c.User.Email,
                        CreatedAt = c.User.CreatedAt.ConvertToLocalFromUTC(indianTimezoneId),
                        UpdatedAt = c.User.UpdatedAt.HasValue
                        ? c.User.UpdatedAt.Value.ConvertToLocalFromUTC(indianTimezoneId)
                        : c.User.UpdatedAt.GetValueOrDefault()
                    },
                    CreatedAt = c.CreatedAt.ConvertToLocalFromUTC(indianTimezoneId),
                    UpdatedAt = c.UpdatedAt.HasValue
                        ? c.UpdatedAt.Value.ConvertToLocalFromUTC(indianTimezoneId)
                        : c.UpdatedAt.GetValueOrDefault()
                })))
                .ForMember(dto => dto.CreatedAt, options => options.MapFrom(src => src.CreatedAt.ConvertToLocalFromUTC(indianTimezoneId)))
                .ForMember(dto => dto.UpdatedAt, options => options.MapFrom(src => src.UpdatedAt.HasValue
                        ? src.UpdatedAt.Value.ConvertToLocalFromUTC(indianTimezoneId)
                        : src.UpdatedAt.GetValueOrDefault()));

            // Comments mapping
            CreateMap<AddCommentDto, Comment>()
                .ForMember(dto => dto.Body, options => options.MapFrom(src => src.Body))
                .ForMember(dto => dto.UserId, options => options.MapFrom(src => src.UserId))
                .ForMember(dto => dto.PostId, options => options.MapFrom(src => src.PostId))
                .ForMember(dto => dto.CreatedAt, options => options.MapFrom(_ => DateTime.UtcNow));

            CreateMap<UpdateCommentDto, Comment>()
                .ForMember(dto => dto.Id, options => options.MapFrom(src => src.Id))
                .ForMember(dto => dto.Body, options => options.MapFrom(src => src.Body))
                .ForMember(dto => dto.UserId, options => options.MapFrom(src => src.UserId))
                .ForMember(dto => dto.PostId, options => options.MapFrom(src => src.PostId))
                .ForMember(dto => dto.UpdatedAt, options => options.MapFrom(_ => DateTime.UtcNow));

            CreateMap<Comment, CommentResponseDto>()
                .ForMember(dto => dto.Id, options => options.MapFrom(src => src.Id))
                .ForMember(dto => dto.Body, options => options.MapFrom(src => src.Body))
                .ForMember(dto => dto.Commenter, options => options.MapFrom(src => new UserResponseBase
                {
                    Id = src.User.Id,
                    FirstName = src.User.FirstName,
                    LastName = src.User.LastName,
                    GenderAbbreviation = src.User.GenderAbbreviation,
                    DateOfBirth = src.User.DateOfBirth.ConvertToLocalFromUTC(indianTimezoneId),
                    Phone = src.User.Phone,
                    Username = src.User.Username,
                    Email = src.User.Email,
                    CreatedAt = src.User.CreatedAt.ConvertToLocalFromUTC(indianTimezoneId),
                    UpdatedAt = src.User.UpdatedAt.HasValue
                        ? src.User.UpdatedAt.Value.ConvertToLocalFromUTC(indianTimezoneId)
                        : src.User.UpdatedAt.GetValueOrDefault()
                }))
                .ForMember(dto => dto.Post, options => options.MapFrom(src => new UserPost
                {
                    PostId = src.Post.Id,
                    Title = src.Post.Title,
                    Body = src.Post.Body,
                    CreatedAt = src.Post.CreatedAt.ConvertToLocalFromUTC(indianTimezoneId),
                    UpdatedAt = src.Post.UpdatedAt.HasValue
                        ? src.User.UpdatedAt.Value.ConvertToLocalFromUTC(indianTimezoneId)
                        : src.User.UpdatedAt.GetValueOrDefault()
                }))
                .ForMember(dto => dto.CreatedAt, options => options.MapFrom(src => src.CreatedAt.ConvertToLocalFromUTC(indianTimezoneId)))
                .ForMember(dto => dto.UpdatedAt, options => options.MapFrom(src => src.UpdatedAt.HasValue
                        ? src.UpdatedAt.Value.ConvertToLocalFromUTC(indianTimezoneId)
                        : src.UpdatedAt.GetValueOrDefault()));
        }
    }
}
