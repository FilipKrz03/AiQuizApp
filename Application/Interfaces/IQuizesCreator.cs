﻿using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IQuizesCreator
    {
        Task Create(string technologyName , AdvanceNumber advanceNumber);
    }
}
