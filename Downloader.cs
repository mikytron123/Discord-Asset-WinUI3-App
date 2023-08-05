using ABI.System;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Protection.PlayReady;
using Windows.Storage;

namespace DiscordEmoteApp
{
    public class Downloader
    {
        private readonly string baseurl = "https://discord.com/api/v10";
        static readonly HttpClient client = new();

        public async Task<List<Guild>> Getservers(string token)
        {
            string url = $"{this.baseurl}/users/@me/guilds";
            using (HttpRequestMessage request = new(HttpMethod.Get, url))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(token);
                using (var response = await client.SendAsync(request))
                {
                    var responsestr = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Guild>>(responsestr);

                }
            }
        }


        public async Task<List<Emoji>> Getemojis(string token, string id)
        {

            string url = $"{this.baseurl}/guilds/{id}/emojis";
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(token);
                using (var response = await client.SendAsync(request))
                {
                    var responsestr = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Emoji>>(responsestr);

                }
            }
        }

        public async Task<List<Sticker>> Getstickers(string token, string id)
        {
            string url = $"{this.baseurl}/guilds/{id}/stickers";
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(token);
                using (var response = await client.SendAsync(request))
                {
                    var responsestr = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Sticker>>(responsestr);

                }
            }
        }


        public async Task DownloadfilesAsync(string token)
        {
            var serverlist = await Getservers(token);
            var assetlist = new List<Dictionary<string, string>>();
            var emojibaseurl = "https://cdn.discordapp.com/emojis/";
            var stickerbaseurl = "https://cdn.discordapp.com/stickers/";

            foreach (var server in serverlist)
            {
                var emojilist = await Getemojis(token, server.id);

                foreach (var emoji in emojilist)
                {
                    var name = emoji.name;
                    string ext;
                    if (emoji.animated)
                    {
                        ext = ".gif";
                    }
                    else
                    {
                        ext = ".png";
                    }

                    var url = emojibaseurl + emoji.id + ext;
                    Dictionary<string, string> tempDictionary = new Dictionary<string, string>()
                    {
                        { "name", name },
                        { "url", url },
                        { "tags", "" },
                        {"type","emote" }
                    };

                    assetlist.Add(tempDictionary);

                }

                await Task.Delay(1000);

                var stickerlist = await Getstickers(token, server.id);

                foreach (var sticker in stickerlist)
                {
                    string url;
                    string stickertype;
                    if (sticker.format_type == 1)
                    {
                        url = stickerbaseurl + sticker.id + ".png";
                        stickertype = "sticker";
                    }
                    else if (sticker.format_type == 2)
                    {
                        url = stickerbaseurl + sticker.id + ".png";
                        stickertype = "apng";
                    }
                    else if (sticker.format_type == 4)
                    {
                        url = stickerbaseurl + sticker.id + ".gif";
                        stickertype = "sticker";
                    }
                    else
                    {
                        continue;
                    }
                    var tempDictionary = new Dictionary<string, string>()
                    {
                        {"name",sticker.name },
                        {"url",url },
                        {"tags",sticker.tags },
                        {"type",stickertype}
                    };

                    assetlist.Add(tempDictionary);


                }

            }

            var jsonstr = JsonConvert.SerializeObject(assetlist);
            var fileName = "test.json";

            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, jsonstr);

        }

        public async Task<List<DiscordAsset>> ReademotesAsync()
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            var storageFile = await storageFolder.GetFileAsync("test.json");
            var jsonstr = await FileIO.ReadTextAsync(storageFile);
            return JsonConvert.DeserializeObject<List<DiscordAsset>>(jsonstr);

        }
    }
}

