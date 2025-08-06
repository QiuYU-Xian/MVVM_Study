using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWpfMvvmApp.Models
{
    public class SampleData
    {
        public string Name { get; set; } = "Default Name";
        public int Value { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
