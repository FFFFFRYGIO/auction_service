using System.IO.Pipes;

namespace AuctionHouseServer;

public class PipeServer
{
    private NamedPipeServerStream underlyingPipe;
    private StreamWriter sw;
    private readonly string pipeName;

    public PipeServer(string pipeName)
    {
        this.pipeName = pipeName;
        InitPipe();
    }

    private async void InitPipe()
    {
        Console.WriteLine(pipeName);
        underlyingPipe = new(pipeName, PipeDirection.InOut);
        await underlyingPipe.WaitForConnectionAsync();
        sw = new StreamWriter(underlyingPipe);
        
        //insta sending data
        sw.AutoFlush = true;
    }

    public async void WriteIfConnected(string message)
    {
        try
        {
            if (underlyingPipe.IsConnected)
            {
                await sw.WriteLineAsync(message);
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
            Dispose();
            InitPipe();
        }
    }

    private void Dispose()
    {
        try
        {
            sw.Dispose();
            underlyingPipe.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}