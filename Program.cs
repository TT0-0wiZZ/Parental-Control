using System.Runtime.InteropServices;
using System.Diagnostics;

const string URL = "https://ttwizz.su/ParentalControl.txt";

[DllImport("kernel32.dll")]
static extern IntPtr GetConsoleWindow();

[DllImport("user32.dll")]
static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

const int SW_HIDE = 0;

async static Task<string> GetResponse(string address)
{
    HttpClient client = new();
    string response = await client.GetStringAsync(address);
    return response;
}

async static Task<bool> CheckConnection()
{
    try
    {
        await GetResponse(URL);
        return true;
    }
    catch (HttpRequestException)
    {
        return false;
    }
}

async static void main()
{
    var handle = GetConsoleWindow();
    ShowWindow(handle, SW_HIDE);
    while (true)
    {
        if (await CheckConnection() && await GetResponse(URL) == "1")
        {
            Process.Start("shutdown", "/s /f /t 00");
            break;
        }
        await Task.Delay(1000);
    }
}

main();
Console.ReadLine();