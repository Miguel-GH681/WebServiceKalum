using FinalAdoSoap.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SoapCore;

namespace FinalAdoSoap{
    public class Startup{
        public void ConfigureServices(IServiceCollection services){
            services.TryAddSingleton<IEnrollmentService, EnrollmentService>();
            services.AddMvc();
            services.AddSoapCore();
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env){
            if(env.IsDevelopment()){
                app.UseDeveloperExceptionPage();
            }
            app.UseSoapEndpoint<IEnrollmentService>("/EnrollmentService.asmx", new SoapEncoderOptions(), SoapSerializer.XmlSerializer);
        }
    }
}