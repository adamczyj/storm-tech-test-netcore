using Todo.Services;
using Xunit;

namespace Todo.Tests
{
    public class UserDisplayNameFormatterTests
    {
        private const string Email = "test@test.com";
        private const string UserName = "test";

        [Fact]
        public void GetDisplayName_UserNameIsNull_ReturnOnlyEmail()
        {
            var result = UserDisplayNameFormatter.GetDisplayName(Email, null);

            Assert.Equal(Email, result);
        }

        [Fact]
        public void GetDisplayName_EmailAndUserNameAreNull_ReturnOnlyEmailWithUsername()
        {
            var result = UserDisplayNameFormatter.GetDisplayName(Email, UserName);

            Assert.Equal($"{Email} - {UserName}", result);
        }

        [Fact]
        public void GetDisplayName_EmailIsNullAndUsernameIsNot_ReturnNull()
        {
            var result = UserDisplayNameFormatter.GetDisplayName(null, UserName);

            Assert.Null(result);
        }
    }
}
