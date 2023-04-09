namespace ConsumerTests.Instances;

[Intellenum(typeof(int))]
[Member(name: "Invalid", value: -1)]
[Member(name: "Unspecified", value: -2)]
public partial class MyIntWithTwoInstanceOfInvalidAndUnspecified
{
}

[Intellenum(typeof(decimal))]
[Member(name: "Invalid", value: "-1.23")]
[Member(name: "Unspecified", value: "-2.34")]
public partial class MyDecimalWithTwoInstanceOfInvalidAndUnspecified
{
}

[Intellenum(typeof(int))]
[Member(name: "Invalid", value: -1)]
[Member(name: "Unspecified", value: -2)]
public partial class MyStructVoIntWithTwoInstanceOfInvalidAndUnspecified
{
}

[Intellenum(typeof(int))]
[Member(name: "Invalid", value: -1)]
[Member(name: "Unspecified", value: -2)]
public partial class MyRecordClassVoIntWithTwoInstanceOfInvalidAndUnspecified
{
}