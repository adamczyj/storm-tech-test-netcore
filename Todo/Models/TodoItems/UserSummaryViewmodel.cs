using Todo.Services;

namespace Todo.Models.TodoItems
{
    public class UserSummaryViewmodel
    {
        //I would probably do it in different way.
        //I would get rid of static factories and make them injectable. Then I would just use GravatarClient to get UserName from Gravatar.
        public string UserName { get; set; }
        public string Email { get; }

        public string DisplayName => UserDisplayNameFormatter.GetDisplayName(Email, UserName);

        public UserSummaryViewmodel(string userName, string email)
        {
            UserName = userName;
            Email = email;
        }
    }
}