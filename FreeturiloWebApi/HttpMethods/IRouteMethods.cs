using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Exceptions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace FreeturiloWebApi.HttpMethods
{
    public interface IRouteMethods
    {
        /// <summary>
        /// Return route based on parameters
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        FragmentRouteDTO[] GetRoute(string serverPath, RouteParametersDTO parameters);
    }
}
