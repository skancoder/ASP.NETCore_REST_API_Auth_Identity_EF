## FluentValidation for CreateTagRequest Model

Nuget> FluentValidation.AspNetCore


#### instead of doing Validation like below, Fluent validation is much cleaner
1. if(string.IsNullOrEmpty(request.TagName){

return BadRequest(new  {error="empty name"})

}

or 2. ------property----------

[EmailAddress]

public string Email{get;set;}

-------------controller------------------------

if(!ModelState.Valid){

var errors = ModelState.Select(x => x.Value.Errors)

                           .Where(y=>y.Count>0)
                           
                           .ToList();

var message = string.Join(" | ", ModelState.Values

        .SelectMany(v => v.Errors)
        
        .Select(e => e.ErrorMessage));

return BadRequest(errors);

}
