using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kventin.Services.Infrastructure.Exceptions
{
    public class NoAccessException(string message) : Exception(message)
    {
    }
}
