using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CapitalOne.CodingExcercise.Summary.Connectors;
using CapitalOne.CodingExcercise.SummaryApi.Services;
using CapitalOne.CodingExcercise.Summary.Domain;
using Newtonsoft.Json;

namespace CapitalOne.CodingExcercise.SummaryApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// Called by the runtime to add services to the container.
        /// </summary>
        /// <param name="services">The collection to add the services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.Formatting = Formatting.Indented;
            });

            // Add connection the endpoint with transactions.
            services.AddTransient<ITransactionsConnector, TransactionsHttpClientConnector>();

            // Plug-ins to summarize transactions.
            services.AddTransient<IMonthsRangeIdentifier, MonthsRangeIdentifier>();
            services.AddTransient<ISummaryByTimeCategorizer, SummaryByTimeCategorizer>();
            services.AddTransient<IAverageMonthCalculator, AverageMonthCalculator>();

            // Plug-in to ignore transactions that match certain values. I.e. donuts.
            services.AddTransient<IExcludeByFieldValueCategorizer<string>, ExcludeStringFieldValueCategorizer>();

            // Plug-in to exclude credit card payments.
            services.AddTransient<ICreditCardPaymentsIdentifier, CreditCardPaymentsIdentifierByOppositeAmounts>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
