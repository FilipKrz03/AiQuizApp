using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class QuizBasicResponseDto
    {
        public string Title { get; set; } = string.Empty;
        public string TechnologyName { get; set; } = string.Empty;  
        public int AdvanceNumber {  get; set; } 
    }
}
