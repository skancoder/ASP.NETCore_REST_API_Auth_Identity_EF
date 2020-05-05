using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBookAPI.Constracts.V1.Requests;

namespace TweetBookAPI.Validators
{
    public class CreateTagRequestValidator : AbstractValidator<CreateTagRequest>
    {
        public CreateTagRequestValidator()
        {
            RuleFor(x => x.TagName)
                .NotEmpty()
                .Matches("^[a-zA-z0-9 ]*$");

            //RuleFor(x => x.TagName)
            //    .Must(s => s.Contains("special text"));
        }
    }
}
