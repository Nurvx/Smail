using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Smail
{
    public class Arguments
    {
        private StringDictionary _Parameters;

        public string this[string param]
        {
            get { return (_Parameters[param]); }
        }

        public Arguments(string[] args)
        {
            _Parameters = new StringDictionary();
            Regex splitter = new Regex(@"^-{1}|^/|=|:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Regex remover = new Regex(@"^['""]?(.*?)['""]?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            string parameter = null;
            string[] parts;

            foreach (string txt in args)
            {
                parts = splitter.Split(txt, 3);

                switch (parts.Length)
                {
                    case 1:
                        if (parameter != null)
                        {
                            if (!_Parameters.ContainsKey(parameter))
                            {
                                parts[0] = remover.Replace(parts[0], "$1");
                                _Parameters.Add(parameter, parts[0]);
                            }
                            parameter = null;
                        }
                        break;
                    case 2:
                        if (parameter != null)
                        {
                            if (!_Parameters.ContainsKey(parameter))
                                _Parameters.Add(parameter, "true");
                        }
                        parameter = parts[1];
                        break;
                    case 3:
                        if (parameter != null)
                        {
                            if (!_Parameters.ContainsKey(parameter))
                                _Parameters.Add(parameter, "true");
                        }

                        parameter = parts[1];

                        if (!_Parameters.ContainsKey(parameter))
                        {
                            parts[2] = remover.Replace(parts[2], "$1");
                            _Parameters.Add(parameter, parts[2]);
                        }

                        parameter = null;
                        break;
                }
            }

            if (parameter != null)
            {
                if (!_Parameters.ContainsKey(parameter))
                    _Parameters.Add(parameter, "true");
            }
        }
    }
}
