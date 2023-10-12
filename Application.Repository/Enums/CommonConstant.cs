namespace Application.Repository.Enums
{
    public static class CommonConstant
    {
        public static class Policies
        {
            public const string UserPolicy = "UserPolicy";
            public const string AdminPolicy = "AdminPolicy";
            public const string UserAndAdminPolicy = "UserOrAdminPolicy";
        }

        public static class Role
        {
            public const string User = "User";
            public const string Admin = "SuperAdmin";
        }
    }

}
