using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Exceptions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FreeturiloWebApi.HttpMethods
{
    public interface IUserMethods
    {
        string Authenticate(string serverPath, AuthDTO auth);
    }
}
