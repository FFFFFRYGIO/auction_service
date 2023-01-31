namespace AuctionHouseServer;

public class PrivPipe : IDisposable
{
    public readonly PipeServer Pipe;
    public Task Task;
    public bool IsCreated;
    public bool IsClosed;

    public PrivPipe(PipeServer p)
    {
        Pipe = p;
        IsCreated = false;
        IsClosed = false;
    }

    public void Dispose()
    {
        try
        {
            Pipe.Dispose();
            Task.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    ~PrivPipe()
    {
        Dispose();
    }
}