using System;
using System.Collections.Generic;
using System.Text;

namespace lab_2_1
{
    class Message
    {
        public string Username { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
            return $"{Username}: {Text}";
        }
    }
}
