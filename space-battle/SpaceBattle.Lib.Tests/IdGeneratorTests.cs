using App;
using App.Scopes;
using SpaceBattle.lib;

public class GuidIdGeneratorTests
{
    [Fact]
    public void Generate_ReturnsNonEmptyId()
    {
        var generator = new GuidIdGenerator();

        var id = generator.Generate();

        Assert.False(string.IsNullOrWhiteSpace(id));
    }

    [Fact]
    public void Generate_ReturnsDifferentIdsOnSequentialCalls()
    {
        var generator = new GuidIdGenerator();

        var first = generator.Generate();
        var second = generator.Generate();

        Assert.NotEqual(first, second);
    }
}

public class RegisterIoCDependencyIdGeneratorTests
{
    public RegisterIoCDependencyIdGeneratorTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void Execute_RegistersIdGenerator_AndResolveWorks()
    {
        new RegisterIoCDependencyIdGenerator().Execute();

        var generator = Ioc.Resolve<IIdGenerator>("Generators.Id");
        var id = generator.Generate();

        Assert.NotNull(generator);
        Assert.False(string.IsNullOrWhiteSpace(id));
    }
}
