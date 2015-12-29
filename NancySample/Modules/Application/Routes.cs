namespace NancySample.Modules.Application
{
    //A class of constant route names, for use with Nancy.Linker
    //TODO: Is there a better way to do this?
    public static class RouteNames
    {
        public const string GetRoot = "root";

        //Auth Module Names
        public const string GetLogin = "login";
        public const string PostLogin = "postlogin";
        public const string GetLogout = "logout";

        //Home Module names
        public const string GetHome = "home";

        //User Module names
        public const string GetUserProfile = "userprofile";
    }
}