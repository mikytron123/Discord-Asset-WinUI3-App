// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

namespace DiscordEmoteApp
{
    public class DiscordAsset
    {
        public string name { get; set; }

        public string url { get; set; }

        public string tags { get; set; }

        public string type { get; set; }


        public string ClipboardText
        {
            get
            {
                if (this.type == "emote")
                {
                    return $"{this.url}?size=48";
                }
                else
                {
                    return $"{this.url}";
                }
            }
        }
    }
}