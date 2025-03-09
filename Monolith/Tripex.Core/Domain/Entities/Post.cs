namespace Tripex.Core.Domain.Entities
{
    public class Post : BaseEntity, ILikable, ISavable, IWatchable
    {
        public Guid UserId { get; set; }
        public User? User { get; set; }
        
        public string ContentUrl { get; set; } = string.Empty;
        public string? Description { get; set; }

        public IEnumerable<Like<Post>> Likes { get; set; } = new List<Like<Post>>();
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
        public IEnumerable<Watcher<Post>> PostWatchers { get; set; } = new List<Watcher<Post>>();

        public int LikesCount { get; set; } = 0;
        public int CommentsCount { get; set; } = 0;
        public int ViewedCount { get; set; } = 0;

        public DateTime ContentUrlUpdated { get; set; } = DateTime.UtcNow;
        public DateTime ViewedCountUpdated { get; set; } = DateTime.UtcNow;

        private const int UPDATE_CONENT_URL_TIME = 590;

        public Post() { }
        public Post(Guid id, Guid userId, string contentUrl, string? description)
        {
            Id = id;
            UserId = userId;
            ContentUrl = contentUrl;
            Description = description;
        }

        public async Task UpdatePostIfNeededAsync(IS3FileService s3FileService, ICrudRepository<Watcher<Post>> postWatcherRepo,
         Guid userWatched, ICrudRepository<Post> repo)
        {
            bool changes = false;
            if (DateTime.UtcNow - ContentUrlUpdated >= TimeSpan.FromMinutes(UPDATE_CONENT_URL_TIME))
            {
                ContentUrl = await s3FileService.GetPreSignedURL(Id.ToString(), 10);
                ContentUrlUpdated = DateTime.UtcNow;
                changes = true;
            }

            var existingWatcher = await postWatcherRepo.GetQueryable<Watcher<Post>>()
               .AsNoTracking()
               .AnyAsync(w => w.UserId == userWatched && w.EntityId == Id);

            if (!existingWatcher)
            {
                ViewedCount++;
                await postWatcherRepo.AddAsync(new Watcher<Post>(userWatched, Id));
                changes = true;
            }

            if (changes)
                await repo.UpdateAsync(this);
        }

        public async Task UpdatePostUrlIfNeededAsync(IS3FileService s3FileService, ICrudRepository<Post> repo)
        {
            if (DateTime.UtcNow - ContentUrlUpdated >= TimeSpan.FromMinutes(UPDATE_CONENT_URL_TIME))
            {
                ContentUrl = await s3FileService.GetPreSignedURL(Id.ToString(), 10);
                ContentUrlUpdated = DateTime.UtcNow;
                await repo.UpdateAsync(this);
            }
        }
    }
}
