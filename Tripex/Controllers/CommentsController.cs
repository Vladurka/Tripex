﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tripex.Application.DTOs.Comments;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;
using Tripex.Core.Domain.Interfaces.Services.Security;

namespace Tripex.Controllers
{
    [Authorize]
    public class CommentsController(ICommentsService service, ICrudRepository<Comment> repo,
        ITokenService tokenService) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult> AddComment(CommentAdd commentAdd)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n",
                ModelState.Values.SelectMany(value => value.Errors)
                    .Select(err => err.ErrorMessage));

                return BadRequest(errors);
            }

            var id = tokenService.GetUserIdByToken();

            var comment = new Comment(id, commentAdd.PostId, commentAdd.Content);

            var response = await service.AddCommentAsync(comment);
            return CheckResponse(response);
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<CommentGet>> GetLike(Guid id)
        {
            var comment = await service.GetCommentAsync(id);
            var commentGet = new CommentGet(comment);
            return Ok(commentGet);
        }

        [HttpGet("more/post/{postId:Guid}")]
        public async Task<ActionResult<IEnumerable<CommentGet>>> GetCommentsByPost(Guid postId)
        {
            var comments = await service.GetCommentsByPostIdAsync(postId);
            var commentsGet = comments.Select(comments => new CommentGet(comments))
                .ToList();

            return Ok(commentsGet);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteComment(Guid id)
        {
            return CheckResponse(await repo.RemoveAsync(id));
        }
    }
}
