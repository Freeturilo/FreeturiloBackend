﻿using FreeturiloWebApi.DTO;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.HttpMethods
{
    public static class UserMethods
    {
        public static string Authenticate(string serverPath, AuthDTO auth)
        {
            var client = new RestClient(serverPath + "user")
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            if (auth != null)
            {
                string body;
                if (auth.Password != null && auth.Email != null)
                    body = @"{" + @"""email"": """ + auth.Email + @""", ""password"": """ + auth.Password + @""" }";
                else if (auth.Password == null)
                    body = @"{" + @"""email"": """ + auth.Email + @""" }";
                else if (auth.Email == null)
                    body = @"{" + @"""password"": """ + auth.Password + @""" }";
                else
                    body = @"{}";


                request.AddParameter("application/json", body, ParameterType.RequestBody);
            }
            IRestResponse response = client.Execute(request);

            string token = response.Content[1..^1];
            return token;
        }
    }
}
