using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace lab_2_1
{
    public class Program
    {
        public static DateTime time;
        static async Task Main()
        {
            Console.WriteLine("Enter username:");
            string username = Console.ReadLine();
            Console.Clear();
            time = DateTime.UtcNow;
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    if (Console.CursorLeft == 0) await ShowMessages(username);
                    await Task.Delay(1000);
                }
            });
            while (true)
            {
                string text = Console.ReadLine();
                if (text != "")
                {
                    using FileStream fs = new FileStream(@"Messages\" + DateTime.Now.ToFileTimeUtc() + ".json", FileMode.Create);
                    var options = new JsonSerializerOptions
                    {
                        
                    };
                     fs.Write(JsonSerializer.SerializeToUtf8Bytes<Message>(new Message
                    {
                        Username = username,
                        Text = text,
                    }));
                }
                int currLine = Console.CursorTop - 1;
                Console.SetCursorPosition(0, currLine);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, currLine);
                Console.CursorLeft = 0;
                await ShowMessages(username);
            }
        }

        static async Task ShowMessages(string username)
        {
            string[] filenames = Directory.GetFiles("Messages");
            foreach (string filename in filenames)
            {
                if (time < DateTime.FromFileTimeUtc(Convert.ToInt64(filename.Split('.', '\\')[1])))
                {
                    FileInfo file = new FileInfo(filename);
                    using FileStream fs = new FileStream(filename, FileMode.OpenOrCreate);
                    Message message = await JsonSerializer.DeserializeAsync<Message>(fs);
                    Console.WriteLine(message.Username == username ? message.Text : message.ToString());
                }
            }
            time = DateTime.UtcNow;
        }
    }
}