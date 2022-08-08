﻿using Microsoft.Extensions.Hosting;

// Setup services outside of ConsoleAppFramework.
var hostBuilder = Host.CreateDefaultBuilder()
    .ConfigureServices(services =>
    {

    });

var app = ConsoleApp.CreateFromHostBuilder(hostBuilder, args);
await app.RunAsync();