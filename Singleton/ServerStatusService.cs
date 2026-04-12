public interface IServerStatusService
{
    ServerStatus CurrentStatus { get; }
    void ChangeStatus(ServerStatus status);
}

public class ServerStatusService : IServerStatusService
{
    private volatile ServerStatus _status = ServerStatus.Starting;
    public ServerStatus CurrentStatus => _status;

    public void ChangeStatus(ServerStatus status) => _status = status;
}

public enum ServerStatus
{
    Starting,
    Running,
    Stopped,
    Maintenance
}