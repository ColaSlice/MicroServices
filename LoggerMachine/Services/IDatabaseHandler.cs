using LoggerMachine.Models;

namespace LoggerMachine.Services;

public interface IDatabaseHandler
{
    public void Insert(string message, DateTime timestamp);
    public List<Log> Read();
}