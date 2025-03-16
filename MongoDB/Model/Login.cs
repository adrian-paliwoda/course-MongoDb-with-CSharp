using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class Login
{
    [BsonElement("uuid")]
    public string Uuid { get; set; }

    [BsonElement("username")]
    public string Username { get; set; }

    [BsonElement("password")]
    public string Password { get; set; }

    [BsonElement("salt")]
    public string Salt { get; set; }

    [BsonElement("md5")]
    public string Md5 { get; set; }

    [BsonElement("sha1")]
    public string Sha1 { get; set; }

    [BsonElement("sha256")]
    public string Sha256 { get; set; }
}