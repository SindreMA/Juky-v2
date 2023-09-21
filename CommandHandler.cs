using System;
using Discord;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using Discord.Commands;
using System.Reflection;
using System.Threading.Tasks;
using NAudio.Wave;
using System.Speech.Synthesis;
using System.IO;

namespace TemplateBot
{

    class CommandHandler
    {
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        private DiscordSocketClient _client;
       
        public CommandHandler(DiscordSocketClient client)
        {
           
            _client = client;
            _commands = new CommandService();
            _commands.AddModulesAsync(Assembly.GetEntryAssembly(),_services);
            _client.MessageReceived += _client_MessageReceived;
        }

        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public int GetDeviceFromName(string name)
        {
            int rtn = 0;
            for (int n = -1; n < WaveOut.DeviceCount; n++)
            {
                var caps = WaveOut.GetCapabilities(n);
                if (caps.ProductName == name)
                {
                    rtn = n;
                }
            }
            return rtn;
        }

        public async Task _client_MessageReceived(SocketMessage arg)
        {
            var msg = arg as SocketUserMessage;
            if (msg == null) return;

            var context = new SocketCommandContext(_client, msg);
            int argPost = 0;

            if (context.User.Username == "Mirtai" || context.User.Username == "SindreMA" && !context.Message.Content.StartsWith("."))
            {
                string text = " " + context.Message.Content;
                foreach (var word in context.Message.Content.Split(' '))
                {
                    if (word.ToLower().Contains("http") || word.ToLower().Contains("www."))
                    {
                        text = text.Replace(word, "");
                    }
                }

                try
                {
                    SpeechSynthesizer synthesizer = new SpeechSynthesizer();
                    synthesizer.Volume = 85;  // 0...100
                    
                    synthesizer.SelectVoice("IVONA 2 Salli");
                    using (MemoryStream stream = new MemoryStream())
                    {
                        MemoryStream streamAudio = new MemoryStream();
                        synthesizer.SetOutputToWaveStream(streamAudio);
                        synthesizer.Speak(text);
                        streamAudio.Position = 0;

                        var rs = new RawSourceWaveStream(streamAudio, new WaveFormat(21000, 16, 1));

                        var outputDevice = new WaveOutEvent() { DeviceNumber =  3};

                        outputDevice.Init(rs);
                        outputDevice.Play();
                    }
                }
                catch (Exception)
                {
                }
                try
                {
                    SpeechSynthesizer synthesizer = new SpeechSynthesizer();
                    synthesizer.Volume = 85;  // 0...100

                    synthesizer.SelectVoice("IVONA 2 Salli");
                    using (MemoryStream stream = new MemoryStream())
                    {
                        MemoryStream streamAudio = new MemoryStream();
                        synthesizer.SetOutputToWaveStream(streamAudio);
                        synthesizer.Speak(text);
                        streamAudio.Position = 0;

                        var rs = new RawSourceWaveStream(streamAudio, new WaveFormat(21000, 16, 1));

                        var outputDevice = new WaveOutEvent();

                        outputDevice.Init(rs);
                        outputDevice.Play();
                    }
                }
                catch (Exception)
                {
                }

            }
           
        }
    }
    
   
}

