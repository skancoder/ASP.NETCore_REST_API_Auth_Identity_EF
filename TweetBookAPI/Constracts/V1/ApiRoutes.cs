﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweetBookAPI.Constracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root+"/"+Version;
        public static class Posts
        {
            public const string GetAll = Base+"/posts";
            public const string Get = Base + "/posts/{postId}";//to make {postId} specific type> {postId:Guid}
            public const string Create = Base + "/posts";
            public const string Update = Base + "/posts/{postId}";
            public const string Delete = Base + "/posts/{postId}";
        }
        public static class Identity//Identity and its controller should be in separate server.(but ignore naming inconsistance of route for simplicity)
        {
            public const string Login = Base + "/identity/login";
            public const string Resgister = Base + "/identity/resgister";
        }
    }
}
