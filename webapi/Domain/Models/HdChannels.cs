using System.ComponentModel.DataAnnotations;

namespace webapi.Domain.Models
{
    public class HdChannels
    {
        [Key]
        public int ChannelID { get; set; }

        public string Description { get; set; }

    }
}

