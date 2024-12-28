using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Interfaces
{
    public interface IBlogApi
    {
        Task<int> GetBlogPostCountAsync();
        Task<List<BlogPost>> GetBlogPostsAsync(int numberofposts, int startindex);
        Task<List<Catagory>> GetCatagoriesAsync();
        Task<List<Comment>> GetCommentsAsync(string blogPostId);
        Task<BlogPost?> GetBlogPostAsync(string Id);
        Task<Catagory?> GetCatagoryAsync(string Id);
        Task<Tag?> GetTagAsync(string Id);
        Task<BlogPost?> SaveBlogPostAsync(BlogPost item);
        Task<Catagory?> SaveCatagoryAsync(Catagory item);
        Task<Tag?> SaveTagAsync(Tag item);
        Task<Comment?> SaveCommentAsync(Comment item);

        Task DeleteBlogPostAsync(string id);
        Task DeleteCatagoryAsync(string id);

        Task DeleteTagAsync(string id);
        Task DeleteCommentAsync(string id);

    }
}
