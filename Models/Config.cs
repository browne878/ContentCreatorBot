using CCbot.Models;
using System.Collections.Generic;

namespace CCbot.Models
{
    public class Config
    {
        public DiscordOptions DiscordOptions { get; set; }
        public List<Servers> Servers { get; set; }
        public MySql MySql { get; set; }

    }
}
