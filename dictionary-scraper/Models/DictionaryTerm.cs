using System;
using System.Collections.Generic;
using System.Text;

namespace dictionary_scraper.Models
{
    public class DictionaryTerm
    {
        public string BaseTerm { get; set; }
        public string Type { get; set; }
        public List<string> Translation { get; set; }

    }
}
