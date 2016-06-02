using System;
using System.Collections.Generic;

namespace Headmaster
{
    public interface IControllerTypesResolver
    {
        IReadOnlyCollection<Type> GetControllerTypes();
    }
}