using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender
{
    public static class EmptyInputValidator
    {
        public static string Validate(string input)
        {
            while (String.IsNullOrEmpty(input))
            {
                Console.WriteLine("Input cannot be empty!");
                input = Console.ReadLine();
            }
            return input;
        }
    }
}
