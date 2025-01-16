using Kventin.DataAccess.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kventin.Services.Dtos
{
    public class GetSubjectsDto
    {
        public List<CreateSubjectDto> Subjects { get; set; } = [];
    }
}
