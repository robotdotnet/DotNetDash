﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDash
{
    public interface IDashboardTypeMetadata
    {
        bool Builtin { get; }

        string Type { get; }
    }
}