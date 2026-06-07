using Domain.Repositories;
using Domain.Services;
using Infrastructure.Repositories;
using WebApi.Infrastructure.DomainExceptionHandler;

namespace WebApi;

public class Program
{
    public static void Main( string[] args )
    {
        WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder( args );

        webApplicationBuilder.Services.AddControllers();
        webApplicationBuilder.Services.AddEndpointsApiExplorer();
        webApplicationBuilder.Services.AddSwaggerGen();

        webApplicationBuilder.Services.AddSingleton<IPropertyRepository, InMemoryPropertyRepository>();
        webApplicationBuilder.Services.AddSingleton<IRoomTypeRepository, InMemoryRoomTypeRepository>();
        webApplicationBuilder.Services.AddSingleton<IReservationRepository, InMemoryReservationRepository>();

        webApplicationBuilder.Services.AddSingleton<IAvailabilityChecker, AvailabilityChecker>();
        webApplicationBuilder.Services.AddSingleton<IBookingService, BookingService>();
        webApplicationBuilder.Services.AddSingleton<ISearchService, SearchService>();

        webApplicationBuilder.Services.AddExceptionHandler<DomainExceptionHandler>();
        webApplicationBuilder.Services.AddProblemDetails();

        WebApplication webApplication = webApplicationBuilder.Build();

        webApplication.UseExceptionHandler();

        if ( webApplication.Environment.IsDevelopment() )
        {
            webApplication.UseSwagger();
            webApplication.UseSwaggerUI();
        }

        webApplication.UseHttpsRedirection();
        webApplication.MapControllers();
        webApplication.Run();
    }
}