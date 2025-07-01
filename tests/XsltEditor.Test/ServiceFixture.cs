using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XsltEditor.Test;
internal class ServiceFixture
{
    public ServiceFixture()
    {
        Services = new ServiceCollection();

        // add services
    }

    public IServiceCollection Services { get; set; }
}
