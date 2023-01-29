namespace AuctionHouseServer;

public class privPipe
{
    public PipeServer pipe;
    public Task task;
    public bool isCreated;

    public privPipe(PipeServer p)
    {
        pipe = p;
        isCreated = false;
    }
    
}