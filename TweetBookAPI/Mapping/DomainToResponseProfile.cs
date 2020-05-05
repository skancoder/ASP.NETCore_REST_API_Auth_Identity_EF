using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBookAPI.Constracts.V1.Responses;
using TweetBookAPI.Domain;

namespace TweetBookAPI.Mapping
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Post, PostResponse>()
                //since Tags name and type doest match in source and destination, do it manually
                .ForMember(destinationMember: dest => dest.Tags,
                            memberOptions: opt => opt.MapFrom(
                             mapExpression: src => src.Tags.Select(x => new TagResponse { Name = x.TagName })));

            CreateMap<Tag, TagResponse>();
        }
    }
}
