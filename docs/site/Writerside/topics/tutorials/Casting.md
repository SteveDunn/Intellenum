# Cast to and from the underlying type

<note>
This topic is copied from Vogen and/or is incomplete. It is being worked on (or is planned
to be worked on). 

If you would like to help with this, please see the list of [open issues](https://github.com/SteveDunn/Intellenum/issues).
</note>

It is recommended not to use casting operators, either explicit or implicit.

It might seem like a handy way to use the underlying primitive natively, but the goal of strong-typing primitives is to differentiate them from the underlying type.

Take, for instance, a `Score` type:

```c#
[ValueObject<int>]
public partial struct Score { }
```

A cast operator, whether implicit or explicit would allow easy access to the `Value`, allowing things such as
`int n = _score + 10;`. But what would be preferable is to be explicit and add a method that describes the operation, 
something like:

```c#
[ValueObject<int>]
public partial struct Score 
{ 
    public Score IncreaseBy(Points points) => From(_value + points.Value);
}
```


