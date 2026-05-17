using BlueLagoon.Modules.Iam.Core.Exceptions;

namespace BlueLagoon.Modules.Iam.Core.Dictionaries;

public static class Scope
{
    public readonly static string Resource = "blue-lagoon-api";  

    public readonly static (string Name, string Description) Iam = new("iam", "Moduł uwierzytelnienia i autoryzacji");

    public readonly static (string Name, string Description) Notifications = new("notifications", "Powiadomienia");
}