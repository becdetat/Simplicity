using Shouldly;
using Xunit;

namespace Simplicity.Tests
{
    public class Examples
    {
        [Fact]
        public void FirstExample()
        {
            const string name = "Ben";

            var result = name.Match()
                .With(x => x == "Fiona", "It's Fiona!")
                .With(x => x == "Ben", "Hey it's me!")
                .With(x => x == "Steve", "Steve you rascal!")
                .Else("I don't know this person")
                .Do();

            result.ShouldBe("Hey it's me!");
        }

        [Fact]
        public void IDontKnowGeorge()
        {
            var name = "George";
            var result = name.Match()
                .With(x => x == "Fiona", "It's Fiona!")
                .With(x => x == "Ben", x => string.Format("Hey it's {0}!", x))
                .With(x => x == "Steve", "Steve you rascal!")
                .Else(x => string.Format("I don't know {0}", x))
                .Do();
            result.ShouldBe("I don't know George");
        }

        [Fact]
        public void NoMatchForYou()
        {
            var name = "Elton";
            Should.Throw<IncompletePatternMatchException>(() => name.Match()
                .With(x => x == "John", "matched")
                .With(x => x == "Paul", "matched")
                .Do());
        }

        [Fact]
        public void YourFunctionalFortuneTeller()
        {
            var question = "meaning of life";
            var result = question.Match().WithOutputType<dynamic>()
                .With(x => x.Contains("roads"), "Blowing in the wind")
                .With(x => x.Contains("life"), 42)
                .Else("Ask again later")
                .Do();

            ((int) result).ShouldBe(42);
        }

        [Fact]
        public void AddGst()
        {
            var country = "NZ";
            var gstRate = country.Match()
                .With("AU", 0.1m)
                .With("NZ", 0.15m)
                .Else(0.0m);

            var total = 2300.0m*(1.0m + gstRate);

            total.ShouldBe(2645.0m);
        }

        [Fact]
        public void EggsInBasket()
        {
            var eggs = 2;
            var basket = PatternMatch.Match()
                .With(() => eggs == 0, "No eggs")
                .With(() => eggs == 1, "One egg")
                .With(() => eggs > 1, string.Format("{0} eggs", eggs))
                .Else("Invalid number of eggs");

            var twoEggs = basket.Do();
            eggs = 0;
            var zeroEggs = basket.Do();
            eggs = int.MinValue;
            var invalidEggs = basket.Do();

            twoEggs.ShouldBe("2 eggs");
            zeroEggs.ShouldBe("No eggs");
            invalidEggs.ShouldBe("Invalid number of eggs");
        }

        [Fact]
        public void FirstLettersOfThings()
        {
            var processName = PatternMatch.Match<string, string>()
                .With(x => x.StartsWith("A"), "Starts with A")
                .With(x => x.StartsWith("B"), "Starts with B")
                .With(x => x.StartsWith("C"), "Starts with C")
                .With(x => x.StartsWith("D"), "Starts with D")
                .With(x => x.StartsWith("E"), "Starts with E")
                .With(x => x.StartsWith("F"), x => string.Format("{0} starts with F", x))
                .Else("Unknown")
                .ToFunc();

            var alfred = processName("Alfred");
            var fiona = processName("Fiona");
            var ben = processName("Ben");
            var xerces = processName("Xerces");

            alfred.ShouldBe("Starts with A");
            fiona.ShouldBe("Fiona starts with F");
            ben.ShouldBe("Starts with B");
            xerces.ShouldBe("Unknown");
        }
    }
}