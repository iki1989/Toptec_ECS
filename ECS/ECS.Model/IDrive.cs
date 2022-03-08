using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model
{
    public interface IDrive
    {
        void Start();

        void Stop();

        Task StartAsync();

        Task StopAsync();
    }
}
