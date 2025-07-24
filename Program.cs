using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using TwitchLib.EventSub.Websockets;

namespace TestConsole
{
    class Program : IDisposable
    {
        public static Process process;

        class Bot
        {
            public TwitchClient client;
            public static string kogkey = Environment.GetEnvironmentVariable("KOGGYBOT_KEY");
            public static string BotName = "BOTNAME";
            public static string ChannelName = "CHANNELNAME";
            public static Process process;
            int currentFont = 1;

            string REWARD_SOUNDFONT = Environment.GetEnvironmentVariable("REWARD_SOUNDFONT");
            string REWARD_TTS = Environment.GetEnvironmentVariable("REWARD_TTS"); //Currently unused

            public Bot(Process _process)
            {
                process = _process;
                ConnectionCredentials credentials = new ConnectionCredentials(BotName, kogkey);
                var clientOptions = new ClientOptions
                {
                    MessagesAllowedInPeriod = 750,
                    ThrottlingPeriod = TimeSpan.FromSeconds(30)
                };
                WebSocketClient customClient = new WebSocketClient(clientOptions);
                client = new TwitchClient(customClient);
                client.Initialize(credentials, ChannelName);

                //client.OnLog += Client_OnLog;
                client.OnJoinedChannel += Client_OnJoinedChannel;
                client.OnMessageReceived += Client_OnMessageReceived;
                client.OnConnected += Client_OnConnected;

                client.Connect();
            }

            

            private void Client_OnConnected(object sender, OnConnectedArgs e)
            {
                Console.WriteLine($"Connected to {e.BotUsername}");
            }

            private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
            {
                Console.WriteLine($"Connected to {e.Channel}.");
                client.SendMessage(e.Channel, "IM WATCHING YOU");
            }

            private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
            {
                Console.WriteLine($"{e.ChatMessage.Username}: {e.ChatMessage.Message}");
                if (e.ChatMessage.CustomRewardId == REWARD_SOUNDFONT)
                {
                    string path = ($"C:\\ProgramData\\soundfonts\\{e.ChatMessage.Message.ToLower()}.sf2");
                    if(File.Exists(path))
                    {
                        Console.WriteLine($"[REWARD]: Soundfont changing to {e.ChatMessage.Message.ToLower()}");
                        SendChangeCommand(path);
                    }
                    else
                    {
                        Console.WriteLine($"[REWARD]: Soundfont not found...");
                        return;
                    }
                }
            }

            private void SendChangeCommand(string input)
            {
                SendInput($"load \"{input}\"");
                Thread.Sleep(2000);
                SendInput($"unload {currentFont}");
                currentFont += 1;
            }
            public static void SendInput(string input)
            {
                process.StandardInput.WriteLine(input);
                process.StandardInput.Flush();
            }
        }

        static Bot bot;

        static void Main(string[] args)
        {
            process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            process.StartInfo.WorkingDirectory = @"C:\Users\\Documents\fluidsynth-2.3.5-win10-x64\bin";
            process.StartInfo.FileName = @"C:\Users\koreandev\Documents\fluidsynth-2.3.5-win10-x64\bin\fluidsynth.exe";
            process.StartInfo.Arguments = "\"C:\\ProgramData\\soundfonts\\touhou.sf2\"";

            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;

            bot = new Bot(process);

            process.Start();

            if (process.Responding) Console.WriteLine("FluidSynth created");

            while (true)
            {
                string input = Console.ReadLine();
                bot.client.SendMessage(Bot.ChannelName, input);
            }
        }

        public void Dispose()
        {
            if (process != null)
            {
                bot.client.Disconnect();
                process.Dispose();
            }
        }
    }
}