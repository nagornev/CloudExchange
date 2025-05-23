﻿using FluentValidation;
using FluentValidation.AspNetCore;

namespace CloudExchange.API.Extensions.Startup
{
    public static class ValidatorsStartupExtensions
    {
        public static IServiceCollection AddValidators(this IServiceCollection services) =>
            services.AddFluentValidationAutoValidation(options => options.DisableDataAnnotationsValidation = true)
                    .AddFluentValidators();

        private static IServiceCollection AddFluentValidators(this IServiceCollection services) =>
            services.AddValidatorsFromAssemblyContaining<Program>();
    }
}
