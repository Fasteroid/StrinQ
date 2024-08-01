using Fasteroid;
using System.Diagnostics;
using System.Text;

public static class StopwatchExtensions {
    public static long ElapsedMicroseconds(this Stopwatch sw) => sw.ElapsedTicks / TimeSpan.TicksPerMicrosecond;
    public static long ElapsedNanoseconds(this Stopwatch sw) => sw.ElapsedTicks * 1000L / TimeSpan.TicksPerMicrosecond;
}

public class Program {

    public static IEnumerable<string> GenerateMixedLineEndings(int numberOfLines, int minLineLength = 5, int maxLineLength = 50)
    {
        Random random = new Random();

        for (int i = 0; i < numberOfLines; i++)
        {
            int lineLength = random.Next(minLineLength, maxLineLength + 1);
            StringBuilder line = new StringBuilder(lineLength);

            // Generate random content for the line
            for (int j = 0; j < lineLength; j++)
            {
                line.Append((char)random.Next('A', 'Z' + 1));
            }

            // Randomly choose line ending
            string lineEnding = random.Next(2) == 0 ? "\n" : "\r\n";

            yield return line.ToString() + lineEnding;
        }
    }

    static void Assert(bool condition) {
        if( !condition ) throw new Exception("Assertion Failed... :(");
    }

    static void Main(string[] args) {
        //              *  *    *  * *
        string test = "1\n2\r\n3\n4\n\r\n6";

        {
            var remain = test.TakeLines(1, out string take);
            Assert(take == "1");
            Assert(remain.ToString() == "2\r\n3\n4\n\r\n6");
        }

        {
            var remain = test.TakeLines(2, out string take);
            Assert(take == "1\n2");
            Assert(remain.ToString() == "3\n4\n\r\n6");
        }

        {
            var remain = test
                        .TakeLines(3, out string take1)
                        .TakeLines(2, out string take2);
            Assert(take1 == "1\n2\r\n3");
            Assert(take2 == "4\n");
            Assert(remain.ToString() == "6");
        }

        {
            // should be fine
            var remain = test.TakeLines(6, out string take);
            Assert(take == "1\n2\r\n3\n4\n\r\n6");
            Assert(remain.ToString() == "");
        }

        {
            var remain = test.TakeLines(5, out string take1)
                             .TakeLines(1, out string take2);
            Assert(take1 == "1\n2\r\n3\n4\n");
            Assert(take2 == "6");
            Assert(remain.ToString() == "");
        }

        {
            bool pass = false;
            try {
                var remain = test.TakeLines(7, out string take);
            } catch (ArgumentOutOfRangeException) {
                pass = true;
            }
            Assert(pass);
        }

        {
            var remain = test.TakeLinesFromEnd(1, out string take);
            Assert(take == "6");
            Assert(remain.ToString() == "1\n2\r\n3\n4\n");
        }

        {
            var remain = test.TakeLinesFromEnd(3, out string take);
            Assert(take == "4\n\r\n6");
            Assert(remain.ToString() == "1\n2\r\n3");
        }

        {
            var remain = test
                        .TakeLinesFromEnd(2, out string take1)
                        .TakeLinesFromEnd(1, out string take2);
            Assert(take1 == "\r\n6");
            Assert(take2 == "4");
            Assert(remain.ToString() == "1\n2\r\n3");
        }

        {
            bool pass = false;
            try {
                var remain = test.TakeLinesFromEnd(7, out string take);
            } catch (ArgumentOutOfRangeException) {
                pass = true;
            }
            Assert(pass);
        }


        {
            Console.WriteLine("sanity tests passed, generating evil data for stress tests...");
            string veryLong = GenerateMixedLineEndings(2<<20, 5, 10).Aggregate(new StringBuilder(), (sb, line) => sb.Append(line)).ToString();
            Console.WriteLine("generated");

            var sw = Stopwatch.StartNew();
            var remaining = veryLong.TakeLines(4096, out string _4096lines);
            sw.Stop();

            int _4095 = _4096lines.Count(c => c == '\n');
            Assert(_4095 == 4095);

            Console.WriteLine($"cold reading 4096 lines took {sw.ElapsedMicroseconds()}us");

            sw.Restart();
            remaining.TakeLines(16384, out string _16384lines);
            sw.Stop();

            int _16383 = _16384lines.Count(c => c == '\n');
            Assert(_16383 == 16383);

            Console.WriteLine($"cold reading 16384 lines took {sw.ElapsedMicroseconds()}us");

            long avgElapsed = 0;
            for(int i = 0; i < 64; i++) {
                sw.Restart();
                remaining = remaining.TakeLines(4096, out string _2048lines);
                sw.Stop();
                avgElapsed += sw.ElapsedMicroseconds();
            }
            avgElapsed /= 1000;
            Console.WriteLine($"warm reading 4096 lines took {avgElapsed}us / op");

            avgElapsed = 0;
            for(int i = 0; i < 64; i++) {
                sw.Restart();
                remaining = remaining.TakeLines(16384, out string _2048lines);
                sw.Stop();
                avgElapsed += sw.ElapsedMicroseconds();
            }
            avgElapsed /= 1000;
            Console.WriteLine($"warm reading 16384 lines took {avgElapsed}us / op");

        }


    }
}