using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka_Stateless_Demo.Messages
{
    public class StartRequest
    {
        public StartRequest(string tag)
        {
            Tag = tag;
        }

        public string Tag { get; private set; }

        public override string ToString()
        {
            return $"Tag: {Tag}";
        }
    }
}
