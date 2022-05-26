using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCheck.Services
{
    public class TestesBackends
    {
        public string HelloWorld(string nome)
        {
            return $"Hello World {nome}!";
        }
    }
}
