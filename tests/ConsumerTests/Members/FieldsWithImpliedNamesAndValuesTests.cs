namespace ConsumerTests.MembersMethodTests.StringFieldDeclarations
{
    namespace With_a_field_constructed_with_just_a_value
    {
        [Intellenum<string>]
        public partial class E
        {
            public static readonly E Member1 = new("One!");
        }
        
        public class W
        {
            [Fact]
            public void The_name_is_inferred_from_the_field_name() => E.Member1.Name.Should().Be("Member1");
        }
    }

    namespace With_a_field_constructed_with_just_new_and_no_parameters
    {
        [Intellenum<string>]
        public partial class E
        {
            public static readonly E Member1 = new();
        }

        public class Tests
        {
            [Fact]
            public void The_name_and_value_is_inferred_from_the_field_name()
            {
                E.Member1.Name.Should().Be("Member1");
                E.Member1.Value.Should().Be("Member1");
            }
        }
    }

    namespace With_a_field_with_just_a_declarator_and_no_creation_expression
    {
        [Intellenum<string>]
        public partial class E
        {
            public static readonly E Member1;
        }

        public class Tests
        {
            [Fact]
            public void The_name_and_value_is_inferred_from_the_field_name()
            {
                E.Member1.Name.Should().Be("Member1");
                E.Member1.Value.Should().Be("Member1");
            }
        }
    }

    namespace With_a_field_declaration_with_multiple_declarators_with_no_creation_expression
    {
        [Intellenum<string>]
        public partial class E
        {
            public static readonly E Member1, Member2, Member3;
        }

        public class Tests
        {
            [Fact]
            public void Each_name_and_value_is_inferred_each_declarator_name()
            {
                E.Member1.Name.Should().Be("Member1");
                E.Member1.Value.Should().Be("Member1");

                E.Member2.Name.Should().Be("Member2");
                E.Member2.Value.Should().Be("Member2");
                
                E.Member3.Name.Should().Be("Member3");
                E.Member3.Value.Should().Be("Member3");
            }
        }
    }

    namespace With_field_combining_different_declarations
    {
        [Intellenum<string>]
        public partial class E
        {
            public static readonly E Member1, Member2, Member3;
            public static readonly E Member4, Member5, Member6;
            public static readonly E Member7 = new(), Member8 = new E("8!"), Member9 = new("9!", "N_I_N_E");
        }

        public class Tests
        {
            [Fact]
            public void Each_name_and_value_is_inferred_each_declarator_name()
            {
                E.Member1.Name.Should().Be("Member1");
                E.Member1.Value.Should().Be("Member1");

                E.Member2.Name.Should().Be("Member2");
                E.Member2.Value.Should().Be("Member2");
                
                E.Member3.Name.Should().Be("Member3");
                E.Member3.Value.Should().Be("Member3");

                E.Member4.Name.Should().Be("Member4");
                E.Member4.Value.Should().Be("Member4");

                E.Member5.Name.Should().Be("Member5");
                E.Member5.Value.Should().Be("Member5");
                
                E.Member6.Name.Should().Be("Member6");
                E.Member6.Value.Should().Be("Member6");

                E.Member7.Name.Should().Be("Member7");
                E.Member7.Value.Should().Be("Member7");

                E.Member8.Name.Should().Be("Member8");
                E.Member8.Value.Should().Be("8!");

                E.Member9.Name.Should().Be("9!");
                E.Member9.Value.Should().Be("N_I_N_E");
            }
        }
    }
}

namespace ConsumerTests.MembersMethodTests.IntFieldDeclarations
{
    namespace With_a_field_constructed_with_just_a_value
    {
        [Intellenum]
        public partial class E
        {
            public static readonly E Member1 = new(1);
        }
        
        public class W
        {
            [Fact]
            public void The_name_is_inferred_from_the_field_name() => E.Member1.Name.Should().Be("Member1");
        }
    }

    namespace With_a_field_constructed_with_just_new_and_no_parameters
    {
        [Intellenum]
        public partial class E
        {
            public static readonly E Member1 = new();
        }

        public class Tests
        {
            [Fact]
            public void The_name_and_value_is_inferred_from_the_field_name()
            {
                E.Member1.Name.Should().Be("Member1");
                E.Member1.Value.Should().Be(0);
            }
        }
    }

    namespace With_a_field_with_just_a_declarator_and_no_creation_expression
    {
        [Intellenum]
        public partial class E
        {
            public static readonly E Member1;
        }

        public class Tests
        {
            [Fact]
            public void The_name_and_value_is_inferred_from_the_field_name()
            {
                E.Member1.Name.Should().Be("Member1");
                E.Member1.Value.Should().Be(0);
            }
        }
    }

    namespace With_a_field_declaration_with_multiple_declarators_with_no_creation_expression
    {
        [Intellenum]
        public partial class E
        {
            public static readonly E Member1, Member2, Member3;
        }

        public class Tests
        {
            [Fact]
            public void Each_name_and_value_is_inferred_each_declarator_name()
            {
                E.Member1.Name.Should().Be("Member1");
                E.Member1.Value.Should().Be(0);

                E.Member2.Name.Should().Be("Member2");
                E.Member2.Value.Should().Be(1);
                
                E.Member3.Name.Should().Be("Member3");
                E.Member3.Value.Should().Be(2);
            }
        }
    }

    namespace With_field_combining_different_declarations
    {
        [Intellenum]
        public partial class E
        {
            public static readonly E Member1, Member2, Member3;
            public static readonly E Member4, Member5, Member6;
            public static readonly E Member7 = new(), Member8 = new E(888), Member9 = new("9!", 999);
        }

        public class Tests
        {
            [Fact]
            public void Each_name_and_value_is_inferred_each_declarator_name()
            {
                E.Member1.Name.Should().Be("Member1");
                E.Member1.Value.Should().Be(0);

                E.Member2.Name.Should().Be("Member2");
                E.Member2.Value.Should().Be(1);
                
                E.Member3.Name.Should().Be("Member3");
                E.Member3.Value.Should().Be(2);

                E.Member4.Name.Should().Be("Member4");
                E.Member4.Value.Should().Be(3);

                E.Member5.Name.Should().Be("Member5");
                E.Member5.Value.Should().Be(4);
                
                E.Member6.Name.Should().Be("Member6");
                E.Member6.Value.Should().Be(5);

                E.Member7.Name.Should().Be("Member7");
                E.Member7.Value.Should().Be(6);

                E.Member8.Name.Should().Be("Member8");
                E.Member8.Value.Should().Be(888);

                E.Member9.Name.Should().Be("9!");
                E.Member9.Value.Should().Be(999);
            }
        }
    }

    namespace With_field_combining_different_declarations2
    {
        [Intellenum]
        public partial class E
        {
            public static readonly E Member1, Member2 = new();
        }

        public class Tests
        {
            [Fact]
            public void Each_name_and_value_is_inferred_each_declarator_name()
            {
                E.Member1.Name.Should().Be("Member1");
                E.Member1.Value.Should().Be(0);

                E.Member2.Name.Should().Be("Member2");
                E.Member2.Value.Should().Be(1);
            }
        }
    }
}