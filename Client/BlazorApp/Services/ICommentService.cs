using System;
using ApiContracts;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Services;

public interface ICommentService
{
    Task<CommentDto> AddComment(CreateCommentDto request);

    Task<CommentDto> UpdateComment(int id, UpdateCommentDto request);

    Task<CommentDto?> GetComment(int id);

    Task<IEnumerable<CommentDto>> GetAllComments(int? PostId = null, string? createdByName = null, int? createdById = null);

    Task Delete(int id);

}
