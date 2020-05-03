using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TweetBookAPI.Domain
{
    //[Table("some table name")]//change table name here else it will be 'Posts' as mentioned in DBSet
    public class Post
    {
        [Key]//primary key
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }//this UserId is a foreign to the identityUser

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }

        public virtual List<PostTag> Tags { get; set; }
    }
}
