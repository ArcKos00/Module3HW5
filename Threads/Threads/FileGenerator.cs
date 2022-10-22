using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threads
{
    public class FileGenerator
    {
        private string[] _stringArray = new string[]
        {
            HelloFile.Split(".")[0],
            WordFile.Split(".")[0]
        };
        public static string HelloFile => "Hello.txt";
        public static string WordFile => "Word.txt";
        public void GeneratorFiles()
        {
            var list = new List<Task>(_stringArray.Length);
            for (int i = 0; i < _stringArray.Length; i++)
            {
                list.Add(Task.Run(() => GenerateFile(_stringArray[i] + ".txt", _stringArray[i])));
                Task.Delay(100).Wait();
            }

            Task.WaitAll(list.ToArray());
        }

        public async Task DeleterFiles()
        {
            var list = new List<Task>(_stringArray.Length);
            for (int i = 0; i < _stringArray.Length; i++)
            {
                list.Add(Task.Run(() => DeleteFile(_stringArray[i] + ".txt")));
                Task.Delay(100).Wait();
            }

            await Task.WhenAll(list);
        }

        private void GenerateFile(string fileName, string wordToWrite)
        {
            var rand = new Random().Next(byte.MinValue, byte.MaxValue);
            using (var sw = new StreamWriter(fileName, true))
            {
                var sb = new StringBuilder();
                for (int i = 0; i < rand; i++)
                {
                    sb.Append(wordToWrite + " ");
                }

                sw.WriteAsync(sb.ToString());
            }
        }

        private void DeleteFile(string fileName)
        {
            File.Delete(fileName);
        }
    }
}
