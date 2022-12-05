using System;
using System.Collections.Generic;
using VoteMovie.Entity.Abstract;

namespace VoteMovie.Entity
{
    public partial class User: IEntityBase
    {
        public User()
        {
            Favorites = new HashSet<Favorite>();
        }

        public int Id { get; set; }
        public bool? IsEnabled { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? SecurityStamp { get; set; }
        public string? PasswordHash { get; set; }

        public virtual ICollection<Favorite> Favorites { get; set; }
    }
}
