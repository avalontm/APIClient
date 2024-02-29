using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginAPI
{
    public class ConsoleMenuItem
    {
        public string? Title { get; set; }
        public Func<Task>? Action { get; set; }
    }
}
