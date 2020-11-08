using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Todo.Services.Gravatar;
using Xunit;

namespace Todo.Tests.Gravatar
{
    public class GravatarClientTests
    {
        private IGravatarClient _client;
        private Mock<IGravatarDataFetcher> _fetcherMock;
        private Mock<IMemoryCache> _memoryCacheMock;

        private const string DisplayName = "Name";
        private const string Email = "test@test.com";

        private readonly GravatarResult _gravatarResult = new GravatarResult
        {
            Entry = new Entry[]
            {
                new Entry
                {
                    DisplayName = DisplayName
                }
            }
        };

        public GravatarClientTests()
        {
            _fetcherMock = new Mock<IGravatarDataFetcher>();
            _memoryCacheMock = new Mock<IMemoryCache>();

            object mock = _gravatarResult;
            _memoryCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out mock)).Returns(true);

            _client = new GravatarClient(_fetcherMock.Object, _memoryCacheMock.Object);
        }

        [Fact]
        public async Task GetUserNameAsync_FetcherReturnsData_ShoulReturnDisplayName()
        {
            //Arrange
            _fetcherMock.Setup(x => x.GetGravatarDataFromServiceAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(_gravatarResult));

            //Act
            var result = await _client.GetUserNameAsync(Email);

            //Assert
            Assert.Equal(DisplayName, result);
        }

        [Fact]
        public async Task GetUserNameAsync_FetcherReturnsNull_ShoulReturnNull()
        {
            //Arrange
            object mock = null;
            _memoryCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out mock)).Returns(true);

            _fetcherMock.Setup(x => x.GetGravatarDataFromServiceAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<GravatarResult>(null));

            //Act
            var result = await _client.GetUserNameAsync(Email);

            //Assert
            Assert.Equal(null, result);
        }
    }
}
