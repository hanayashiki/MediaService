using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.DBManager;
using Core.Storage;
using MediaServicePlatform.ServiceFactory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Serilog;

namespace MediaServicePlatform
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Log.Logger = new LoggerConfiguration()
              .Enrich.FromLogContext()
              .WriteTo.File("./media.log")
              .CreateLogger();

            var builder = new ConfigurationBuilder();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            // services.Configure<MediaServiceConfiguration>(Configuration);
            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));

            BlobStorageConfig blobStorageConfig = BlobStorageConfig.GetConfig("./Resource/blobstorageconfig.json");
            FileStorageConfig fileStorageConfig = FileStorageConfig.GetConfig("./Resource/filestorageconfig.json");
            MongoDbConfig mongoDbConfig = MongoDbConfig.GetMongoDbConfig("./Resource/mongodbconfig.json");
            WebConfig webConfig = WebConfig.GetWebConfig("./webconfig.json");
            services.AddSingleton<IImageService, ImageService>(
                    s => ImageServiceFactory.GetImageServiceCached(blobStorageConfig, fileStorageConfig, mongoDbConfig)
                );
            services.AddSingleton<WebConfig, WebConfig>(c => webConfig);


            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
