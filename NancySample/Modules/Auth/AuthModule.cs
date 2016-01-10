using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Nancy.Authentication.Forms;
using Nancy.Linker;
using Nancy.ModelBinding;
using NancySample.Models;
using NancySample.Modules.Application;

namespace NancySample.Modules.Auth
{
    public class AuthModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class SaltRawSaltPasswordEncoder
    {
        private readonly SHA1 _hasher;
        public SaltRawSaltPasswordEncoder()
        {
            _hasher = SHA1.Create();
        }

        public string EncodePassword(string password, string salt)
        {
            return ShaHashFromString(salt + password + salt);
        }
        public bool IsPasswordValid(string encodedPass, string rawPass, string salt)
        {
            var hash = EncodePassword(rawPass, salt);
            return hash == encodedPass;
        }

        private string ShaHashFromString(string utf8)
        {
            var bytes = Encoding.UTF8.GetBytes(utf8);
            var hashBytes = _hasher.ComputeHash(bytes);
            return HexStringFromBytes(hashBytes);
        }

        private static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (var b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }
    }

    public class AuthModule : ApplicationModule
    {
        public AuthModule(IAppDbContext dbCtx, IResourceLinker linker) : base(dbCtx, linker)
        {
            var encoder = new SaltRawSaltPasswordEncoder();
            
            Get[RouteNames.GetLogin, "/auth/login"] = arg =>
            {
                AppModel.PageTitle = "Login";
                return View["login", new { linker }];
            };

            Get[RouteNames.GetLogout, "/auth/logout"] = arg =>
            {
                var rootPath = linker.BuildRelativeUri(Context, RouteNames.GetRoot).ToString();
                return this.LogoutAndRedirect(rootPath);
            };

            Post[RouteNames.PostLogin, "/auth/login"] = parameters =>
            {
                var loginParams = this.Bind<AuthModel>();
                var member = dbCtx.Users.FirstOrDefault(x => x.UserName == loginParams.Username);
                if (member == null || !encoder.IsPasswordValid(member.PassHash, loginParams.Password, member.PassSalt))
                {
                    return "username and/or password was incorrect";
                }
                var expiry = loginParams.RememberMe ? DateTime.MaxValue : DateTime.Now.AddDays(14);
                var rootPath = linker.BuildRelativeUri(Context, RouteNames.GetRoot).ToString();
                return this.LoginAndRedirect(member.Uuid, expiry, rootPath);
            };
        }
    }
}
