using Microsoft.Extensions.Options;
using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightdataRequestConsoleApp
{
    internal class Service
    {
        private readonly BrightDataSettings _settings;

        public Service(IOptions<BrightDataSettings> settings)
        {
            _settings = settings.Value;
        }
    }
}
