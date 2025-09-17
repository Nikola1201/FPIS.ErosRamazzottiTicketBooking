using FPIS.Domain.ViewModels;

namespace FPIS.ErosRamazzottiTicketBooking.Api.Services;

public interface IHomeService
{
    HomePageViewModel? GetHomePage();
}
public class HomeService : IHomeService
{
    public HomePageViewModel? GetHomePage()
    {
        throw new NotImplementedException();
    }
}
