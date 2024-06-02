# Working with logging frameworks

In this tutorial, we'll see how to log instances of value objects.

## Writing to the console

In a new console application, add a NuGet reference to `Intellenum`, and create:

```c#
[Intellenum<int>]
public partial class PacManPoints
{
    static PacManPoints()
    {
        Member("Dot", 10);
        Member("PowerPellet", 50);
        Member("Ghost1", 200);
        Member("Ghost2", 400);
        Member("Ghost3", 800);
        Member("Ghost4", 1600);
    }
}
```

Now, create an instance and write it to the console:

```c#
Console.WriteLine(PacManPoints.PowerPellet.ToString()); 
```

As expected, you'll see:

```Bash
PowerPellet
```

Intellenum generates a default `ToString` method that looks like this:

```C#
public override string ToString() => Name;
```

It is possible to override `ToString`—for example, you might want to write the _value_ out and possibly pad the value with zeroes.
Let's see how to do that. Add the following method to `PacManPoints`:

```C#
[Intellenum<int>]
public partial class PacManPoints
{
    ...
    public override string ToString() => Value.ToString("D5");
}
```

Run it again and it'll now print:

```Bash
00050
```

## Using the default logging framework

Next, we'll log values out using the default .NET logging framework.

Add NuGet references to:
* `Microsoft.Extensions.Logging` 
* `Microsoft.Extensions.Logging.Console`

Next, add a namespace reference to `Microsoft.Extensions.Logging` and create an instance of a logger that writes
to the console:

```c#
using Microsoft.Extensions.Logging;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});

ILogger logger = loggerFactory.CreateLogger<Program>();
```

Now, change the `Console.WriteLine` to:

```C#
var  points = PacManPoints.PowerPellet;
logger.LogInformation("Points awarded! {PointsAwarded}", points);
```

The output is something like this:

```Bash
info: Program[0]
      Points awarded! 00050
```

So far, we've used the standard .NET logger to write to a console, and we've written an Intellenum instance using structured
logging.

Next, we'll look more at structured logging and switch to **Serilog**, a common choice for .NET.

## Structured logging with Serilog

<note>
Structured logging means that you're logging events as data structures rather than formatted text messages. 
With structured logging, you're essentially writing log events as structured data objects rather than plain text. 
The structured data objects can then be stored, queried, and processed in ways that aren’t possible with text logs.
</note>

Structure logging is supported by the default .NET logger.
The structured properties are inside 'index placeholders', for example, `{PointsAwarded}` above.
The default logger supports structured logging.
However, it can't fully use structured data.
For this, we'll use Serilog.

* create a new console app
* add NuGet packages for `Intellenum`, `Serilog`, and `Serilog.Sinks.Console`
* create the `PacManPoints` type from above

Add the following initialization:

```C#
using Serilog;
using Intellenum;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var  points = PacManPoints.PowerPellet;
Log.Information("Points awarded! {PointsAwarded}", points);
```

Run it again, and you'll see something like this:

```Bash
[06:33:12 INF] Points awarded! 00050
```

Let's create a bit more structure. Add a `GameEvent` object:

```C#
public record GameEvent(string Name, PacManPoints Points);
```

Replace what is logged with:

```C#
Log.Information("{GameEvent}", 
    new GameEvent("Fred", PacManPoints.PowerPellet));
```

You'll see:

```c#
[06:38:38 INF] GameEvent { Name = Fred, Points = 00050 }
```

In Serilog and some other .NET logging libraries that support structured logging, the `@` character in a _placeholder_ 
is called the _destructuring operator_.
It tells the logging system to serialize the object completely instead of just calling `ToString()`.
This means the complete state of the object is logged, which can give you much more useful information for complex objects.

In our simple example, changing the logging line to:

```C#
Log.Information("{@GameEvent}",
    new GameEvent("Fred", PacManPoints.PowerPellet));
```

Results in:
```Bash
[06:40:57 INF] {"Name": "Fred", "Points": 
    {"Value": 50, "Name": "PowerPellet", "$type": "PacManPoints"}, 
    "$type": "GameEvent"}
```

You can see that `Name` is written as: `Fred`.
But `Points` is written as: `{"Value": 50, "Name": "PowerPellet", "$type": "PacManPoints"},
"$type": "GameEvent"}`

You might find this output for Intellenums is too detailed and that it pollutes the logs.

We can tell Serilog to use the simpler format for Intellenums.

Add the following `Destructure` line to the initialization:

```C#
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    ++ .Destructure.With(new IntellenumDestructuringPolicy())
    .CreateLogger();
```

And add this type to the project:

```C#
public class IntellenumDestructuringPolicy : IDestructuringPolicy
{
    public bool TryDestructure(object value, 
        ILogEventPropertyValueFactory propertyValueFactory, 
        out LogEventPropertyValue? result)
    {
        bool isIntellenum = value.GetType().GetCustomAttribute(
            typeof(IntellenumAttribute)) is IntellenumAttribute;
        
        result = isIntellenum 
            ? new ScalarValue(value.ToString()) 
            : null;
        
        return isIntellenum;
    }
}
```

Now, run again, and see that enum is written more simply:

```Bash
[06:46:49 INF] 
    {"Name": "Fred", "Points": "00050", "$type": "GameEvent"}
```

In this tutorial, we've seen how to Intellenums to the console and to the default logger.
We've also seen how to use structured logging and how to customize the output of Intellenums written to Serilog.

