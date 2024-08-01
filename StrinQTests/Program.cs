using Fasteroid;
using System.Diagnostics;

public class Program {

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

        Console.WriteLine("all tests passed??");

    }
}