namespace Toarnbeike.AppLibrary.Extensions;

public class LinqExtensionTests
{
    [Test]
    public void ForEach_ThrowsArgumentNullException_WhenSourceIsNull()
    {
        IEnumerable<int> source = null!;
        Action<int> action = x => { };
        var exception = Should.Throw<ArgumentNullException>(() => source.ForEach(action));
        exception.ParamName.ShouldBe("source");
    }

    [Test]
    public void ForEach_ThrowsArgumentNullException_WhenActionIsNull()
    {
        IEnumerable<int> source = [1, 2, 3];
        Action<int> action = null!;
        var exception = Should.Throw<ArgumentNullException>(() => source.ForEach(action));
        exception.ParamName.ShouldBe("action");
    }

    [Test]
    public void ForEach_AppliesActionToEachElement()
    {
        IEnumerable<int> source = [1, 2, 3];
        var result = new List<int>();

        source.ForEach(x => result.Add(x * 2));

        result.ShouldBe([2, 4, 6]);
    }
}
