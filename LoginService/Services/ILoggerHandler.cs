using LoginService.Models;
namespace LoginService.Services;

public interface ILoggerHandler
{
    public void Log(string message);
}