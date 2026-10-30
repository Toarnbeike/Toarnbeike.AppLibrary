using Toarnbeike.AppLibrary.Failures;
using Toarnbeike.Results;
using Toarnbeike.Results.Failures;
using Toarnbeike.Results.TestExtensions;

namespace Toarnbeike.AppLibrary.Results;

public class EnsureModifiedTests
{
    [Test]
    public void EnsureModified_OnEntityWithNoChanges_ShouldReturnUnmodifiedFailure()
    {
        var original = new TestEntity(TestEntityId.New(), "Test Name");
        Result<TestEntity> modifiedResult = original;

        var result = modifiedResult.EnsureModified(original);

        result.ShouldBeFailureOfType<UnmodifiedFailure>().Property.ShouldBe(nameof(TestEntity));
    }

    [Test]
    public void EnsureModified_OnEntityWithChanges_ShouldReturnSuccess()
    {
        var original = new TestEntity(TestEntityId.New(), "Test Name");
        var modified = original with { Name = "Modified Name" };
        Result<TestEntity> modifiedResult = modified;

        var result = modifiedResult.EnsureModified(original);

        result.ShouldBeSuccess().ShouldBe(modified);
    }

    [Test]
    public void EnsureModified_OnFailingResult_ShouldReturnSameFailure()
    {
        var original = new TestEntity(TestEntityId.New(), "Test Name");
        Result<TestEntity> modifiedResult = new TestFailure("original failure");

        var result = modifiedResult.EnsureModified(original);

        result.ShouldBeFailureOfType<TestFailure>().Identifier.ShouldBe("original failure");
    }

    [Test]
    public async Task EnsureModified_OnAsyncEntityWithNoChanges_ShouldReturnUnmodifiedFailure()
    {
        var original = new TestEntity(TestEntityId.New(), "Test Name");
        Task<Result<TestEntity>> modifiedResult = Task.FromResult<Result<TestEntity>>(original);

        var result = await modifiedResult.EnsureModified(original);

        result.ShouldBeFailureOfType<UnmodifiedFailure>().Property.ShouldBe(nameof(TestEntity));
    }

    [Test]
    public async Task EnsureModified_OnAsyncEntityWithChanges_ShouldReturnSucces()
    {
        var original = new TestEntity(TestEntityId.New(), "Test Name");
        var modified = original with { Name = "Modified Name" };
        Task<Result<TestEntity>> modifiedResult = Task.FromResult<Result<TestEntity>>(modified);

        var result = await modifiedResult.EnsureModified(original);
        
        result.ShouldBeSuccess().ShouldBe(modified);
    }

    [Test]
    public async Task EnsureModified_OnAsyncFailingResult_ShouldReturnSameFailure()
    {
        var original = new TestEntity(TestEntityId.New(), "Test Name");
        Task<Result<TestEntity>> modifiedResult = Task.FromResult<Result<TestEntity>>(new TestFailure("original failure"));

        var result = await modifiedResult.EnsureModified(original);

        result.ShouldBeFailureOfType<TestFailure>().Identifier.ShouldBe("original failure");
    }

    [Test]
    public void EnsureModified_OnUnchangedProperty_ShouldReturnUnmodifiedFailure()
    {
        var name = "Original";
        var newName = name;

        var result = Result.Success().EnsureModified(name, newName);

        result.ShouldBeFailureOfType<UnmodifiedFailure>().Property.ShouldBe(nameof(name));
    }

    [Test]
    public void EnsureModified_OnChangedProperty_ShouldReturnSuccess()
    {
        var name = "Original";
        var newName = "Modified";
        var result = Result.Success().EnsureModified(name, newName);
        result.ShouldBeSuccess().ShouldBe(newName);
    }

    [Test]
    public void EnsureModified_ForPropertyOnFailingResult_ShouldReturnSameFailure()
    {
        var name = "Original";
        var newName = "Modified";
        Result resultPipeline = new TestFailure("original failure");

        var result = resultPipeline.EnsureModified(name, newName);
        result.ShouldBeFailureOfType<TestFailure>().Identifier.ShouldBe("original failure");
    }

    [Test]
    public void EnsureModified_OnUnchangedPropertyWithExplicitName_ShouldReturnUnmodifiedFailure()
    {
        var name = "Original";
        var newName = name;
        var propertyName = "CustomProperty";
        var result = Result.Success().EnsureModified(name, newName, propertyName);
        result.ShouldBeFailureOfType<UnmodifiedFailure>().Property.ShouldBe(propertyName);
    }

    [Test]
    public void EnsureModified_OnUnchangedPropertyWithExplicitNullName_ShouldReturnUnmodifiedFailure()
    {
        var name = "Original";
        var newName = name;
        var result = Result.Success().EnsureModified(name, newName, null);
        result.ShouldBeFailureOfType<UnmodifiedFailure>().Property.ShouldBe("Property");
    }

    [Test]
    public void EnsureModified_OnUnchangedPropertyResultT_ShouldResultUnmodifiedFailure()
    {
        var name = "Original";
        var newName = name;
        Result<bool> resultPipeline = true;

        var result = resultPipeline.EnsureModified(name, newName);
        result.ShouldBeFailureOfType<UnmodifiedFailure>().Property.ShouldBe(nameof(name));
    }

    [Test]
    public void EnsureModified_OnChangedPropertyResultT_ShouldReturnSuccess()
    {
        var name = "Original";
        var newName = "Modified";
        Result<bool> resultPipeline = true;

        var result = resultPipeline.EnsureModified(name, newName);

        result.ShouldBeSuccess().ShouldBe(newName);
    }

    [Test]
    public void EnsureModified_ForPropertyOnFailingResultT_ShouldReturnSameFailure()
    {
        var name = "Original";
        var newName = "Modified";
        Result<bool> resultPipeline = new TestFailure("original failure");

        var result = resultPipeline.EnsureModified(name, newName);

        result.ShouldBeFailureOfType<TestFailure>().Identifier.ShouldBe("original failure");
    }

    [Test]
    public void EnsureModified_OnUnchangedPropertyResultTWithExplicitName_ShouldReturnUnmodifiedFailure()
    {
        var name = "Original";
        var newName = name;
        var propertyName = "CustomProperty";
        Result<bool> resultPipeline = true;

        var result = resultPipeline.EnsureModified(name, newName, propertyName);
        
        result.ShouldBeFailureOfType<UnmodifiedFailure>().Property.ShouldBe(propertyName);
    }

    [Test]
    public void EnsureModified_OnUnchangedPropertyResultTWithExplicitNullName_ShouldReturnUnmodifiedFailure()
    {
        var name = "Original";
        var newName = name;
        Result<bool> resultPipeline = true;

        var result = resultPipeline.EnsureModified(name, newName, null);

        result.ShouldBeFailureOfType<UnmodifiedFailure>().Property.ShouldBe("Property");
    }

    [Test]
    public async Task EnsureModified_OnUnchangedPropertyAsyncResult_ShouldReturnUnmodifiedFailure()
    {
        var name = "Original";
        var newName = name;
        Task<Result> resultPipeline = Task.FromResult(Result.Success());

        var result = await resultPipeline.EnsureModified(name, newName);

        result.ShouldBeFailureOfType<UnmodifiedFailure>().Property.ShouldBe(nameof(name));
    }

    [Test]
    public async Task EnsureModified_OnChangedPropertyAsyncResult_ShouldReturnSuccess()
    {
        var name = "Original";
        var newName = "Modified";
        Task<Result> resultPipeline = Task.FromResult(Result.Success());

        var result = await resultPipeline.EnsureModified(name, newName);

        result.ShouldBeSuccess().ShouldBe(newName);
    }

    [Test]
    public async Task EnsureModified_ForPropertyOnFailureAsyncResult_ShouldReturnSameFailure()
    {
        var name = "Original";
        var newName = "Modified";
        Task<Result> resultPipeline = Task.FromResult<Result>(new TestFailure("original failure"));

        var result = await resultPipeline.EnsureModified(name, newName);

        result.ShouldBeFailureOfType<TestFailure>().Identifier.ShouldBe("original failure");
    }

    [Test]
    public async Task EnsureModified_OnUnchagendPropertyAsyncResultWithExplicitName_ShouldReutrnUnmodifiedFailure()
    {
        var name = "Original";
        var newName = name;
        var propertyName = "CustomProperty";
        Task<Result> resultPipeline = Task.FromResult<Result>(Result.Success());

        var result = await resultPipeline.EnsureModified(name, newName, propertyName);

        result.ShouldBeFailureOfType<UnmodifiedFailure>().Property.ShouldBe(propertyName);
    }

    [Test]
    public async Task EnsureModified_OnUnchagendPropertyAsyncResultWithExplicitNullName_ShouldReutrnUnmodifiedFailure()
    {
        var name = "Original";
        var newName = name;
        Task<Result> resultPipeline = Task.FromResult<Result>(Result.Success());

        var result = await resultPipeline.EnsureModified(name, newName, null);

        result.ShouldBeFailureOfType<UnmodifiedFailure>().Property.ShouldBe("Property");
    }

    [Test]
    public async Task EnsureModified_OnUnchangedPropertyAsyncResultT_ShouldReturnUnmodifiedFailure()
    {
        var name = "Original";
        var newName = name;
        Task<Result<bool>> resultPipeline = Task.FromResult<Result<bool>>(true);

        var result = await resultPipeline.EnsureModified(name, newName);

        result.ShouldBeFailureOfType<UnmodifiedFailure>().Property.ShouldBe(nameof(name));
    }

    [Test]
    public async Task EnsureModified_OnChangedPropertyAsyncResultT_ShouldReturnSuccess()
    {
        var name = "Original";
        var newName = "Modified";
        Task<Result<bool>> resultPipeline = Task.FromResult<Result<bool>>(true);

        var result = await resultPipeline.EnsureModified(name, newName);

        result.ShouldBeSuccess().ShouldBe(newName);
    }

    [Test]
    public async Task EnsureModified_ForPropertyOnFailureAsyncResultT_ShouldReturnSameFailure()
    {
        var name = "Original";
        var newName = "Modified";
        Task<Result<bool>> resultPipeline = Task.FromResult<Result<bool>>(new TestFailure("original failure"));

        var result = await resultPipeline.EnsureModified(name, newName);

        result.ShouldBeFailureOfType<TestFailure>().Identifier.ShouldBe("original failure");
    }

    [Test]
    public async Task EnsureModified_OnUnchagendPropertyAsyncResultTWithExplicitName_ShouldReutrnUnmodifiedFailure()
    {
        var name = "Original";
        var newName = name;
        var propertyName = "CustomProperty";
        Task<Result<bool>> resultPipeline = Task.FromResult<Result<bool>>(true);

        var result = await resultPipeline.EnsureModified(name, newName, propertyName);

        result.ShouldBeFailureOfType<UnmodifiedFailure>().Property.ShouldBe(propertyName);
    }

    [Test]
    public async Task EnsureModified_OnUnchagendPropertyAsyncResultTWithExplicitNullName_ShouldReutrnUnmodifiedFailure()
    {
        var name = "Original";
        var newName = name;
        Task<Result<bool>> resultPipeline = Task.FromResult<Result<bool>>(true);

        var result = await resultPipeline.EnsureModified(name, newName, null);

        result.ShouldBeFailureOfType<UnmodifiedFailure>().Property.ShouldBe("Property");
    }
}