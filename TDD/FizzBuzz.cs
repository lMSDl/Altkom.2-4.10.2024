namespace TDD
{
    public class FizzBuzz
    {
        public string Print(int n)
        {
            List<string> items =

            Enumerable.Range(1, n).Select(x => (IsFizz(x) ? "Fizz" : "") + (IsBuzz(x) ? "Buzz" : ""))
            .Select((x, i) => string.IsNullOrWhiteSpace(x) ? (i + 1).ToString() : x)
            .ToList();

            return string.Join(" ", items);
        }

        private static bool IsFizz(int x)
        {
            return x % 3 == 0;
        }

        private static bool IsBuzz(int x)
        {
            return x % 5 == 0;
        }

    }
}
