using Bogus;
using WebAdaptive.Models;

namespace WebAdaptive.Services.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly List<CommentModel> _comments = new List<CommentModel>();
        private static int id = 1;
        public CommentService()
        {
            var faker = new Faker<CommentModel>()
                .RuleFor(c => c.Id, f => id++)
                .RuleFor(c => c.Content, f => f.Lorem.Sentence());

            _comments.AddRange(faker.Generate(10));
        }

        public async Task<List<CommentModel>> GetAllComments()
        {
            try
            {
                return _comments;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllComments method: {ex.Message}");
                return null;
            }
        }

        public async Task<CommentModel> GetCommentById(int id)
        {
            try
            {
                return _comments.FirstOrDefault(c => c.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCommentById method: {ex.Message}");
                return null;
            }
        }

        public async Task AddComment(CommentModel comment)
        {
            try
            {
                if (comment == null)
                    throw new ArgumentNullException(nameof(comment));

                comment.Id = id++;
                _comments.Add(comment);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddComment method: {ex.Message}");
            }
        }

        public async Task UpdateComment(int id, CommentModel comment)
        {
            try
            {
                if (comment == null)
                    throw new ArgumentNullException(nameof(comment));

                var existingComment = _comments.FirstOrDefault(c => c.Id == id);
                if (existingComment != null)
                {
                    existingComment.Content = comment.Content;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateComment method: {ex.Message}");
            }
        }

        public async Task DeleteComment(int id)
        {
            try
            {
                var comment = _comments.FirstOrDefault(c => c.Id == id);
                if (comment != null)
                {
                    _comments.Remove(comment);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteComment method: {ex.Message}");
            }
        }
    }
}
