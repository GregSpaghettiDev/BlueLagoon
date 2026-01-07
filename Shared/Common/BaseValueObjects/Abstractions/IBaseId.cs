using System;

namespace Common.BaseValueObjects.Abstractions
{
    public interface IBaseId
    {
        Guid Value { get; }
    }
}
