﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace vproker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://*:56644")
                .UseStartup<Startup>()
                .Build();
    }
}
