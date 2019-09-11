using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ItemInventoryApp.Classes
{
    class Validations
    {
        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        public bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        public bool isNumber(string text)
        {
            bool valid = false;

            string accepted = "0123456789.";

            if (accepted.Contains(text))
            {
                valid = true;
            }

            return valid;
        }
    }
}
