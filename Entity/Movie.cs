using System;
using System.Collections.Generic;
using VoteMovie.Entity.Abstract;

namespace VoteMovie.Entity
{
    public partial class Movie : IEntityBase
    {
        public Movie()
        {
            Favorites = new HashSet<Favorite>();
        }

        public int Id { get; set; }
        public bool? IsEnabled { get; set; }
        public string? Title { get; set; }
        public string? ImageContent { get; set; }

        public virtual ICollection<Favorite> Favorites { get; set; }
    }
}
