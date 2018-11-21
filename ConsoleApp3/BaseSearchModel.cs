using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp3
{
    public class BaseSearchModel
    {
        public int Size { get; set; }
        public int From { get; set; }
        public Dictionary<string, string> Fields { get; set; }
    }
}
