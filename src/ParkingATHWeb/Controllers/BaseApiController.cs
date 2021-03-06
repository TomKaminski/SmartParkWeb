﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ModelBinding;

namespace ParkingATHWeb.Controllers
{
    //[Authorize("Bearer")]
    public abstract class BaseApiController : Controller
    {
        private const string HeaderAuthorizeName = "HashHeader";

        protected IEnumerable<string> GetModelStateErrors(ModelStateDictionary modelState)
        {
            var modelStateErrors = new List<string>();
            foreach (var mdl in modelState)
            {
                modelStateErrors.AddRange(mdl.Value.Errors.Select(error => error.ErrorMessage));
            }
            return modelStateErrors;
        }

        protected string GetHashFromHeader()
        {
            return Request.Headers[HeaderAuthorizeName];
        }

        protected string GetAppBaseUrl()
        {
            return Url.Action("Index", "Home", new { area = "Portal" }, "http");
        }

        #region TokenAuth
        //private readonly TokenAuthOptions _tokenOptions;

        //protected BaseApiController(TokenAuthOptions tokenOptions, IUserService userService)
        //{
        //    _tokenOptions = tokenOptions;
        //    _userService = userService;
        //}

        //protected async Task<string> GetTokenAsync(string user, DateTime? expires)
        //{
        //    var handler = new JwtSecurityTokenHandler();

        //    var authenticatedUser = await _userService.GetByEmailAsync(user);

        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, authenticatedUser.Result.Email),
        //        new Claim(ClaimTypes.Name, authenticatedUser.Result.Name),
        //        new Claim("isAdmin", authenticatedUser.Result.IsAdmin.ToString()),
        //        new Claim("LastName", authenticatedUser.Result.LastName)
        //    };

        //    var identity = new ClaimsIdentity(claims, "TokenAuth");

        //    var securityToken = handler.CreateToken(
        //        _tokenOptions.Issuer, 
        //        _tokenOptions.Audience,
        //        signingCredentials: _tokenOptions.SigningCredentials,
        //        subject: identity,
        //        expires: expires
        //        );

        //    return handler.WriteToken(securityToken);
        //}
        #endregion
    }
}
