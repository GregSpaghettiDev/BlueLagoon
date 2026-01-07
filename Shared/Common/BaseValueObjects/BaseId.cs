using Common.BaseValueObjects.Abstractions;
using System;

namespace Common.BaseValueObjects;

public record BaseId(Guid Id) : IBaseId
{
    public virtual Guid Value { get; } = Id;

    public static implicit operator BaseId(Guid value) => new(value);

    public static implicit operator BaseId(string value) => new(Guid.Parse(value));

    public static implicit operator Guid(BaseId value) => value?.Value ?? Guid.Empty;

    public static implicit operator string(BaseId value) => value.Value.ToString();

    public static bool operator ==(BaseId id1, Guid id2) => id1.Value == id2;

    public static bool operator !=(BaseId id1, Guid id2) => id1.Value != id2;

}