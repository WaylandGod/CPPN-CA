﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CACPPN.CA.Interfaces
{
    interface ICell
    {
        INeighbourhood neighbourhood { get; set; }
    }
}
