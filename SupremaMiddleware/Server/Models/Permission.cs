﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Models
{
    public class Permission
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public List<Operator> operators { get; set; }
    }
}
