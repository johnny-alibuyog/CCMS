using System;
using CCMS.Core.Entities;

namespace CCMS.Core.Services
{
    public interface IComputationService
    {
        Money Compute<T>(decimal amount) where T : ComputationSetting;
        Money Compute<T>(Money amount) where T : ComputationSetting;
    }
}
