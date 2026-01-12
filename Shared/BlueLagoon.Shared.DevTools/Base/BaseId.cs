namespace BlueLagoon.Shared.DevTools.Base;

public record BaseId(Guid Id)
{
    public virtual Guid Value { get; } = Id;

    public static implicit operator BaseId(Guid value) => new(value);

    public static implicit operator BaseId(string value) => new(Guid.Parse(value));

    public static implicit operator Guid(BaseId value) => value?.Value ?? Guid.Empty;

    public static implicit operator string(BaseId value) => value.Value.ToString();

    public static bool operator ==(BaseId id1, Guid id2) => id1.Value == id2;

    public static bool operator !=(BaseId id1, Guid id2) => id1.Value != id2;
}