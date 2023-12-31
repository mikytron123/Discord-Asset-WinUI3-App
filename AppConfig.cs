﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace DiscordEmoteApp
{
    public class AppConfig
    {
        private readonly IConfigurationRoot _configurationRoot;

        public AppConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Package.Current.InstalledLocation.Path)
                .AddJsonFile("appsettings.json", optional: false);

            _configurationRoot = builder.Build();
        }

        public IConfigurationRoot ConfigurationRoot { get { return _configurationRoot; } }

        //public TokenConfig tokenConfig => GetSection<TokenConfig>(nameof(TokenConfig));

        //private T GetSection<T>(string key) => _configurationRoot.GetSection(key).Get<T>();
    }
}
