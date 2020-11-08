namespace Todo.Services
{
    public static class UserDisplayNameFormatter
    {
        public static string GetDisplayName(string email, string username)
        {
            if (string.IsNullOrEmpty(email))
                return email;

            var displayName = email;

            if (!string.IsNullOrEmpty(username))
            {
                displayName += $" - {username}";
            }

            return displayName;
        }
    }
}
