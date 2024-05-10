
using CustomersApi.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace CustomersApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
         
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToList();

                    var response = new
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
