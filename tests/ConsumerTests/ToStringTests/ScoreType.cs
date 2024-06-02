namespace ConsumerTests.ToStringTests;

using Intellenum;

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
        Member("Cherry", 100);
        Member("Strawberry", 300);
        Member("Orange", 500);
        Member("Apple", 700);
        Member("Melon", 1000);
        Member("Galaxian", 2000);
        Member("Bell", 3000);
        Member("Key", 5000);
    }

    public override string ToString() => Value.ToString("D5");
}