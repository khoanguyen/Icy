﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icy
{
    public interface IJob
    {
        void Execute(object param);
    }
}
