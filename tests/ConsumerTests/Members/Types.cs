namespace ConsumerTests.Members;

[Intellenum(typeof(int))]
[Member(name: "Invalid", value: -1)]
[Member(name: "Unspecified", value: -2)]
public partial class MyIntWithMembersInvalidAndUnspecified
{
}

[Intellenum(typeof(decimal))]
[Member(name: "Invalid", value: "-1.23")]
[Member(name: "Unspecified", value: "-2.34")]
public partial class MyDecimalWithMembersInvalidAndUnspecified
{
}

[Intellenum(typeof(int))]
[Member(name: "Invalid", value: -1)]
[Member(name: "Unspecified", value: -2)]
public partial class MyStructVoIntWithMembersInvalidAndUnspecified
{
}

[Intellenum(typeof(int))]
[Member(name: "Invalid", value: -1)]
[Member(name: "Unspecified", value: -2)]
public partial class MyRecordClassVoIntWithMembersInvalidAndUnspecified
{
}