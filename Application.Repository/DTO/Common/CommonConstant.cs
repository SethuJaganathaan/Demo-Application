namespace Application.Service.Models
{
    public static class CommonConstant
    {
        public static class Policies
        {
            public const string UserPolicy = "UserPolicy";
            public const string AdminPolicy = "AdminPolicy";
            public const string UserOrAdminPolicy = "UserOrAdminPolicy";
        }

        public static class Role
        {
            public const string User = "User";
            public const string SuperAdmin = "SuperAdmin";
        }

        public static class Context
        {
            public const string ContentType = "ContentType";    
        }
    }

}
