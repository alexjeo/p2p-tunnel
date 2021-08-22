using System;
using System.Collections.Generic;
using System.Text;

namespace ozeki
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface)]
    internal sealed class DoNotObfuscateTypeAttribute : Attribute
    {
    }
}
