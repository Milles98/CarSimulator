using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services.Interfaces
{
    public interface ICarService
    {
        void Drive(string direction);
        void Turn(string direction);
        void Refuel();
        void Rest();
        CarStatus GetStatus();
        void CheckStatus();
    }
}
