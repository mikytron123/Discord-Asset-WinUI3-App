using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEmoteApp
{
    public class ImageDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BasicImageDataTemplate { get; set; }

        public DataTemplate ApngDataTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            var model = (DiscordAsset)item;

            if (model.type == "apng")
            {
                return ApngDataTemplate;
            }
            else
            {
                return BasicImageDataTemplate;
            }
        }
        
    }
    
}
