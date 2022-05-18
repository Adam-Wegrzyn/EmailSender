using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.Exceptions
{
    public class RecipientNotFoundException : Exception
    {
        public RecipientNotFoundException(string? message) : base(message)
        {
        }
    }
}
