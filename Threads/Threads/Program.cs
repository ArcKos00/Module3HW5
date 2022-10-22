using System.Text;
using Threads;

int countSeconds = 2;
new FileGenerator().GeneratorFiles();
await AsyncReadFiles();

Console.WriteLine($"Wait {countSeconds} seconds while the files are deleted");
await new FileGenerator().DeleterFiles();
Thread.Sleep(countSeconds * 1000);

async Task<string> FileHello()
{
    using (var sr = new StreamReader(FileGenerator.HelloFile))
    {
        return await sr.ReadToEndAsync();
    }
}

async Task<string> FileWorld()
{
    using (var sr = new StreamReader(FileGenerator.WordFile))
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
        list.Add(Task.Run(() => FileHello()));
        list.Add(Task.Run(() => FileWorld()));
    });
    Task.WhenAll(list).GetAwaiter().GetResult();
    foreach (var someString in list)
    {
        sb.Append(someString.Result + "\n");
    }

    Console.WriteLine(sb.ToString());
}