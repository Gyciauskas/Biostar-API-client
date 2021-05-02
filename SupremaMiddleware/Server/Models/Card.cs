using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Models
{
    public class Card
    {
        public CardType card_type { get; set; }
        public string card_id { get; set; }
        public string display_card_id { get; set; }
        public string id { get; set; }
        public string cardId { get; set; }
        public WiegandFormatId wiegand_format_id { get; set; }
        public string wiegand_format_name { get; set; }
    }
}
