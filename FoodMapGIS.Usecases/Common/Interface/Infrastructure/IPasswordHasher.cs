using System;
using System.Collections.Generic;
using System.Text;

namespace FoodMapGIS.Application.Common.Interface.Infrastructure
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string password, string passwordHash);
    }
}
