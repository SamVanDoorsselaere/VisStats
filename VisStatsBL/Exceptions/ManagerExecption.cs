using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisStatsBL.Exceptions
{
    public class ManagerExecption : Exception
    {
        public ManagerExecption(string? message) : base(message)
        {
        }

        public ManagerExecption(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
