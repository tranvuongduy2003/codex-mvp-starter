using Solution.Application.Abstractions.Auth;
using Solution.Application.Abstractions.Persistence;
using Solution.Application.Abstractions.Storage;
using Solution.Domain.Users;
using FluentAssertions;

namespace Solution.Domain.UnitTests;

public class SolutionSkeletonTests
{
    [Fact]
    public void UserAggregate_IsPresent()
    {
        typeof(User).Assembly.Should().BeSameAs(Solution.Domain.AssemblyReference.Assembly);
    }

    [Fact]
    public void ApplicationPorts_AreDefinedForBoilerplateInfrastructure()
    {
        typeof(IUserRepository).Assembly.Should().BeSameAs(Solution.Application.AssemblyReference.Assembly);
        typeof(ISessionStore).Assembly.Should().BeSameAs(Solution.Application.AssemblyReference.Assembly);
        typeof(IObjectStorage).Assembly.Should().BeSameAs(Solution.Application.AssemblyReference.Assembly);
    }
}
