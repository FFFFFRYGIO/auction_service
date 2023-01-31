namespace AuctionHouseServer;

public class privPipe : IDisposable
{
    public PipeServer pipe;
    public Task task;
    public bool isCreated;
    public bool isClosed;

    public privPipe(PipeServer p)
    {
        pipe = p;
        isCreated = false;
        isClosed = false;
    }

    public void Dispose()
    {
        try
        {
            pipe.Dispose();
            task.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    ~privPipe()
    {
        Dispose();
    }
}