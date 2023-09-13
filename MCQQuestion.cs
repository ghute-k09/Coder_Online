using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoderOnline
{
    public class MCQQuestion
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public int CorrectOptionIndex { get; set; }

        public MCQQuestion() {
            Options = new List<string>();
        }
    }
}
