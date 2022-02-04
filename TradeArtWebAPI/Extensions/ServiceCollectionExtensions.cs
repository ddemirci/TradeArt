﻿using TradeArt.Interfaces;
using TradeArt.TaskService.Impl;

namespace TradeArtWebAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<ITaskService, TaskService>();
        }
    }
}
