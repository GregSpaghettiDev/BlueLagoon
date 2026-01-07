using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace Common.MongoDb.Base;

public class MongoBaseEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public virtual string Id { get; private set; }

    [BsonElement(nameof(CreatedAt))]
    public virtual DateTime CreatedAt { get; private set; }

    protected void SetCreatedAt(DateTime createdAt)
        => CreatedAt = createdAt;
}

