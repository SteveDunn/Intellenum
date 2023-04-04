// ReSharper disable RedundantNameQualifier
// ReSharper disable ArrangeConstructorOrDestructorBody
// ReSharper disable PartialTypeWithSinglePart

namespace Intellenum.Examples.SyntaxExamples
{
    /*
     * Can be in nested namespaces, but cannot be in a nested class.
     * This example below is OK as it's just a nested namespace.
     */
    namespace Namespace1
    {
        namespace Namespace2
        {
            [Intellenum]
            [Instance("Standard", 1)]
            [Instance("Gold", 2)]
            public partial class CustomerType
            {
            }
        }

        /*
         * This example below is not OK as it's a nested class.
         */
        internal class TopLevelClass
        {
            internal class AnotherClass
            {
                internal class AndAnother
                {
                    // uncomment to get error VOG001: Type 'NestedType' cannot be nested - remove it from inside AndAnother
                    // [Intellenum(typeof(int))]
                    // [Instance("Item1", 1)]
                    public partial class NestedType
                    {
                    }
                }
            }
        }
    }
}

