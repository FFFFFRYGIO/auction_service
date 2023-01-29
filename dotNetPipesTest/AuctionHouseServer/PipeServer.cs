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
        //Console.WriteLine(pipeName);
        underlyingPipe = new(pipeName, PipeDirection.InOut);
        await underlyingPipe.WaitForConnectionAsync();
        sw = new StreamWriter(underlyingPipe);
        
        //insta sending data
        sw.AutoFlush = true;
    }

    public void WaitConnection()
    {
        try
        {
            if (!underlyingPipe.IsConnected)
            {
                underlyingPipe.WaitForConnection();
            }
            //underlyingPipe.WaitForConnection();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async void WriteIfConnected(string message)
    {
        try
        {
            if (underlyingPipe.IsConnected)
            {
                StreamString sw = new StreamString(underlyingPipe);
                //await sw.WriteLineAsync(message);
                sw.WriteString(message);
                underlyingPipe.WaitForPipeDrain();
                underlyingPipe.Flush();
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
            Dispose();
            InitPipe();
        }
    }

    public string Read()
    {
        
        try
        {
            StreamString sr = new StreamString(underlyingPipe);

            return sr.ReadString();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        /*
        try
        {
            StreamReader sr = new StreamReader(underlyingPipe);

            return sr.ReadLine();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        */
    }

    public bool isConnected()
    {
        return underlyingPipe.IsConnected;
    }

    public string getName()
    {
        return pipeName;
    }

    public void close()
    {
        underlyingPipe.Close();
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