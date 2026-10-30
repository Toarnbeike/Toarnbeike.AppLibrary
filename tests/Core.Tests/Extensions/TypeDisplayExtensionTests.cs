namespace Toarnbeike.AppLibrary.Extensions;

public class TypeDisplayExtensionTests
{
    [Test]
    public void ReadableName_ForPrimitiveTypes_ReturnsAlias()
    {
        typeof(bool).ReadableName().ShouldBe("bool");
        typeof(byte).ReadableName().ShouldBe("byte");
        typeof(sbyte).ReadableName().ShouldBe("sbyte");
        typeof(short).ReadableName().ShouldBe("short");
        typeof(ushort).ReadableName().ShouldBe("ushort");
        typeof(int).ReadableName().ShouldBe("int");
        typeof(uint).ReadableName().ShouldBe("uint");
        typeof(long).ReadableName().ShouldBe("long");
        typeof(ulong).ReadableName().ShouldBe("ulong");
        typeof(float).ReadableName().ShouldBe("float");
        typeof(double).ReadableName().ShouldBe("double");
        typeof(decimal).ReadableName().ShouldBe("decimal");
        typeof(char).ReadableName().ShouldBe("char");
        typeof(string).ReadableName().ShouldBe("string");
        typeof(object).ReadableName().ShouldBe("object");
        typeof(void).ReadableName().ShouldBe("void");
        typeof(nint).ReadableName().ShouldBe("nint");
        typeof(nuint).ReadableName().ShouldBe("nuint");
    }

    [Test]
    public void ReadableName_ForArrayTypes_ReturnsElementTypeWithBrackets()
    {
        typeof(int[]).ReadableName().ShouldBe("int[]");
        typeof(string[,]).ReadableName().ShouldBe("string[,]");
        typeof(string[,,]).ReadableName().ShouldBe("string[,,]");
        typeof(int[][]).ReadableName().ShouldBe("int[][]");
        typeof(string[][,]).ReadableName().ShouldBe("string[][,]");
        typeof(string[,][]).ReadableName().ShouldBe("string[,][]");
        typeof(List<string[,]>[]).ReadableName().ShouldBe("List<string[,]>[]");
    }

    [Test]
    public void ReadableName_ForNullableTypes_ReturnsUnderlyingTypeWithQuestionMark()
    {
        typeof(int?).ReadableName().ShouldBe("int?");
        typeof(DateTime?).ReadableName().ShouldBe("DateTime?");
        typeof(Nullable<DateTime>).ReadableName().ShouldBe("DateTime?");
    }

    [Test]
    public void ReadableName_ForGenericTypes_ReturnsTypeNameWithAngleBracketedArguments()
    {
        typeof(List<int>).ReadableName().ShouldBe("List<int>");
        typeof(Dictionary<string, int>).ReadableName().ShouldBe("Dictionary<string, int>");
    }
}