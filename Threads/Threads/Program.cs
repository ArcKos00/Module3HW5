using System.Text;
using Threads.Services;

int countSeconds = 2;
new FileService().GeneratorFiles();
await AsyncReadFiles();

Console.ReadLine();
Console.WriteLine($"Wait {countSeconds} seconds while the files are deleted");
await new FileService().DeleterFiles();
Thread.Sleep(countSeconds * 1000);

async Task<string> FileHello()
{
    using (var sr = new StreamReader(FileService.HelloFile))
    {
        return await sr.ReadToEndAsync();
    }
}

async Task<string> FileWorld()
{
    using (var sr = new StreamReader(FileService.WordFile))
    {
        return await sr.ReadToEndAsync();
    }
}

async Task AsyncReadFiles()
{
    var list = new List<Task<string>>();
    var sb = new StringBuilder();

    await Task.Run(() =>
    {
        list.Add(Task.Run(async () => await FileHello()));
        list.Add(Task.Run(async () => await FileWorld()));
    });
    Task.WhenAll(list).GetAwaiter().GetResult();
    foreach (var someString in list)
    {
        sb.Append(someString.Result + "\n");
    }

    Console.WriteLine(sb.ToString());
}