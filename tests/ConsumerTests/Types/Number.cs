namespace Intellenum.Tests.Types;

[Intellenum(typeof(int))]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class Number
{

}

// /// <summary>
// /// A Value Object that is not supported
// /// </summary>
// [Intellenum(typeof(List<Dave>))]
// public partial class Daves
// {
//     private static Validation Validate(List<Dave> value) => value.Count > 0 ? Validation.Ok : Validation.Invalid("no dave's found");
// }