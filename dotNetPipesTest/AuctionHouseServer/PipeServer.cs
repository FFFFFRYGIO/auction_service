using System.IO.Pipes;
using System.Text.Json;
using AuctionHouseClient;

namespace AuctionHouseServer;

public class PipeServer : IDisposable
{
    private NamedPipeServerStream _underlyingPipe;
    private readonly string _pipeName;
    public bool ToDelete;

    public PipeServer(string pipeName)
    {
        this._pipeName = pipeName;
        ToDelete = false;
        InitPipe();
    }

    private async void InitPipe()
    {
        _underlyingPipe = new NamedPipeServerStream(_pipeName, PipeDirection.InOut);
        await _underlyingPipe.WaitForConnectionAsync();
    }

    public void WaitConnection()
    {
        try
        {
            if (!_underlyingPipe.IsConnected)
            {
                _underlyingPipe.WaitForConnection();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void WriteIfConnected(string message)
    {
        try
        {
            if (_underlyingPipe.IsConnected)
            {
                StreamString sw = new StreamString(_underlyingPipe);
                sw.WriteString(message);
                _underlyingPipe.Flush();
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
            Dispose();
            InitPipe();
        }
    }
    
    public void WriteIfConnected(CommandJSON message)
    {
        try
        {
            if (_underlyingPipe.IsConnected)
            {
                var sw = new StreamString(_underlyingPipe);
                sw.WriteString(JsonSerializer.Serialize(message));
                _underlyingPipe.Flush();
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
            var sr = new StreamString(_underlyingPipe);

            return sr.ReadString();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public bool IsConnected()
    {
        return _underlyingPipe.IsConnected;
    }

    public string GetName()
    {
        return _pipeName;
    }

    public void Close()
    {
        _underlyingPipe.Close();
    }

    public void Dispose()
    {
        try
        {
            _underlyingPipe.Close();
            _underlyingPipe.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}