using WebAdaptive.Models;

namespace WebAdaptive.Services.CommentService
{
    public interface ICommentService
    {
        Task<List<CommentModel>> GetAllComments();
        Task<CommentModel> GetCommentById(int id); 
        Task AddComment(CommentModel comment);
        Task UpdateComment(int id, CommentModel comment);
        Task DeleteComment(int id);
    }
}
