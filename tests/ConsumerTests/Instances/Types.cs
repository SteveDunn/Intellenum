namespace ConsumerTests.Instances;

[Intellenum(typeof(int))]
[Instance(name: "Invalid", value: -1)]
[Instance(name: "Unspecified", value: -2)]
public partial class MyIntWithTwoInstanceOfInvalidAndUnspecified
{
}

[Intellenum(typeof(decimal))]
[Instance(name: "Invalid", value: "-1.23")]
[Instance(name: "Unspecified", value: "-2.34")]
public partial class MyDecimalWithTwoInstanceOfInvalidAndUnspecified
{
}

[Intellenum(typeof(int))]
[Instance(name: "Invalid", value: -1)]
[Instance(name: "Unspecified", value: -2)]
public partial class MyStructVoIntWithTwoInstanceOfInvalidAndUnspecified
{
}

[Intellenum(typeof(int))]
[Instance(name: "Invalid", value: -1)]
[Instance(name: "Unspecified", value: -2)]
public partial class MyRecordClassVoIntWithTwoInstanceOfInvalidAndUnspecified
{
}