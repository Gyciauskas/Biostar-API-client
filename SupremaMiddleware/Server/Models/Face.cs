using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Models
{
    public class Face
    {
        public string raw_image { get; set; }
        public List<Template> templates { get; set; }
        public string flag { get; set; }
        public string useProfile { get; set; }
        public int index { get; set; }
    }
}
