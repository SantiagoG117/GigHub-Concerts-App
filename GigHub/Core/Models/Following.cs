using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GigHub.Core.Models
{
    public class Following
    {
        [Key]
        [Column(Order = 1)]
        public string FollowerId { get; set; }
        public ApplicationUser Follower { get; set; }

        [Key]
        [Column(Order = 2)]
        public string ArtistId { get; set; }
        public ApplicationUser Artist { get; set; }

    }
}