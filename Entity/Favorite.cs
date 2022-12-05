using System;
using System.Collections.Generic;
using VoteMovie.Entity.Abstract;

namespace VoteMovie.Entity
{
    public partial class Favorite : IEntityBase
    {
        public int Id { get; set; }
        public bool? IsEnabled { get; set; }
        public int? UserId { get; set; }
        public int? MovieId { get; set; }
        public string? TypeOfFavorite { get; set; }

        public virtual Movie? Movie { get; set; }
        public virtual User? User { get; set; }
    }
}
