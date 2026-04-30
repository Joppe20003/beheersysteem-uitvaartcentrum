namespace beheersysteem_uitvaartcentrum.backend.application.Security
{
    public static class RolePermissions
    {
        private static readonly Dictionary<string, List<string>> _permissions = new()
        {
            { "Admin", new List<string> { "dossier:create", "dossier:overview", "dossier:view, dossier:invite"} },
            { "UitvaartOndernemer", new List<string> { "dossier:create", "dossier:overview", "dossier:view, dossier:invite" } },
            { "Externe", new List<string> { "dossier:overview", "dossier:view" } }
        };

        public static List<string> GetActionsForRoles(List<string> roles)
        {
            return roles.SelectMany(role => _permissions.ContainsKey(role) ? _permissions[role] : new List<string>()).Distinct().ToList();
        }
    }
}
