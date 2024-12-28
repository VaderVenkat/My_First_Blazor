using Data.Models;
using Data.Models.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Data
{
    public class BlogApiJsonDirectAccess : IBlogApi
    {
        BlogApiJsonDirectAccessSetting _settings;
        public BlogApiJsonDirectAccess(IOptions<BlogApiJsonDirectAccessSetting> options)
        {
            _settings = options.Value;
            ManageDataPath();

        }

        private void ManageDataPath()
        {
            CreateDirectoryIfNotExists(_settings.DataPath);
            CreateDirectoryIfNotExists($@"{_settings.DataPath}\{_settings.BlogPostsFolder}");
            CreateDirectoryIfNotExists($@"{_settings.DataPath}\{_settings.CategoriesFolder}");
            CreateDirectoryIfNotExists($@"{_settings.DataPath}\{_settings.TagsFolder}");
            CreateDirectoryIfNotExists($@"{_settings.DataPath}\{_settings.CommentsFolder}");
        }

        private void CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

        }

        private async Task<List<T>> LoadAsync<T>(string folder)
        {
            var list = new List<T>();
            foreach (var f in Directory.GetFiles($@"{_settings.DataPath}\{folder}"))
            {
                var json = await File.ReadAllTextAsync(f);
                var blogpost = JsonSerializer.Deserialize<T>(json);
                if (blogpost is not null)
                {
                    list.Add(blogpost);
                }

            }
            return list;
        }

        private async Task SaveAsync<T>(string folder, string filename, T item)
        {
           var filepath = $@"{_settings.DataPath}\{folder}\{filename}.json";
            await File.WriteAllTextAsync(filepath, JsonSerializer.Serialize<T>(item));
        }
        private Task DeleteAsync(string folder, string filename)
        {
            var filepath = $@"{_settings.DataPath}\{folder}\{filename}.json";
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            return Task.CompletedTask;
        }

        public async Task<int> GetBlogPostCountAsync()
        {
            var list = await LoadAsync<BlogPost>(_settings.BlogPostsFolder);
            return list.Count;
        }
        public async Task<List<BlogPost>> GetBlogPostsAsync(int numberofposts, int startindex)
        {
            var list = await LoadAsync<BlogPost>(_settings.BlogPostsFolder);
            return list.Skip(startindex).Take(numberofposts).ToList();


        }

        public async Task<BlogPost?> GetBlogPostAsync(string id)
        {
            var list = await LoadAsync<BlogPost>(_settings.BlogPostsFolder);
            return list.FirstOrDefault(bp => bp.Id == id);
        }
        public async Task<List<Catagory>> GetCategoriesAsync()
        {
            return await LoadAsync<Catagory>(_settings.CategoriesFolder);

        }
        public async Task<Catagory?> GetCatagoryAsync(string id)
        {
            var list = await LoadAsync<Catagory>(_settings.CategoriesFolder);
            return list.FirstOrDefault(c => c.Id == id);
        }

        public async Task<List<Tag>> GetTagsAsync()
        {
            return await LoadAsync<Tag>(_settings.TagsFolder);
        }
        public async Task<Tag?> GetTagAsync(string id)
        {
            var list = await LoadAsync<Tag>(_settings.TagsFolder);
            return list.FirstOrDefault(t => t.Id == id);
        }
        public async Task<List<Comment>> GetCommentsAsync(string blogpostid)
        {
            var list = await LoadAsync<Comment>(_settings.CommentsFolder);
            return list.Where(t => t.BlogPostId == blogpostid).ToList();
        }

        public async Task<BlogPost?> SaveBlogPostAsync(BlogPost item)
        {
            item.Id ??= Guid.NewGuid().ToString();
            await SaveAsync(_settings.BlogPostsFolder, item.Id, item);
            return item;
        }

        public async Task<Catagory?> SaveCatagoryAsync(Catagory item)
        {
            item.Id ??= Guid.NewGuid().ToString();
            await SaveAsync(_settings.CategoriesFolder, item.Id, item);
            return item;
        }
        public async Task<Tag?> SaveTagAsync(Tag item)
        {
            item.Id ??= Guid.NewGuid().ToString() ; 
            await SaveAsync(_settings.TagsFolder, item.Id, item);
            return item;

        }
        public async Task<Comment?> SaveCommentAsync(Comment item)
        {
            item.Id ??= Guid.NewGuid().ToString();
            await SaveAsync(_settings.CommentsFolder,item.Id, item);
            return item;
        }

        public async Task DeleteBlogPostAsync(string id)
        {
            await DeleteAsync(_settings.BlogPostsFolder, id);

            var comments = await GetCommentsAsync(id);
            foreach (var comment in comments)
            {
                if (comment.Id != null)
                {
                    await DeleteAsync(_settings.CommentsFolder, comment.Id);
                }

            }
        }

        public async Task DeleteCatagoryAsync(string id)
        {
            await DeleteAsync(_settings.CategoriesFolder , id);
        }

        public async Task DeleteTagAsync(string id)
        {
            await DeleteAsync(_settings.TagsFolder , id);
        }

        public async Task DeleteCommentAsync(string id)
        {
            await DeleteAsync(_settings.CommentsFolder , id);
        }
    }
}
