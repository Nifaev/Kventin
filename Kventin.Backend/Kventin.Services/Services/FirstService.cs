using Kventin.DataAccess;
using Kventin.DataAccess.Domain;
using Kventin.Services.Dtos;
using Kventin.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kventin.Services.Services
{
    public class FirstService : IFirstService
    {
        private readonly KventinContext _db;

        public FirstService(KventinContext db)
        {
            _db = db;
        }

        public async Task CreateSubject(CreateSubjectDto subjectDto)
        {
            throw new NotImplementedException();
        }

        public async Task<GetSubjectsDto> GetSubjects()
        {
            return new GetSubjectsDto();
        }
    }
}
