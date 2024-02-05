using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class QuestionResponseDto
    {
        public string Content { get; set; } = string.Empty;
        public IEnumerable<AnswerResponseDto> Answers { get; set; } = [];
    }
}
