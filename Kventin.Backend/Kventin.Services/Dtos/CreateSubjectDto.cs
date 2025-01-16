using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kventin.Services.Dtos
{
    public class CreateSubjectDto
    {
        public required string Name { get; set; }
        public List<int> StudyGroupIds { get; set; } = [];
    }
}
