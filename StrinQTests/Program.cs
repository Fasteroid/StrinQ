using Fasteroid;
using System.Diagnostics;

static void Main(string[] args) {

    string test = "1\n2\r\n3\n4\n\r\n6\n";

    {
        var remain = test.TakeLines(1, out string take);
        Debug.Assert(take == "1");
        Debug.Assert(remain.ToString() == "2\r\n3\n4\n\r\n6\n");
    }

    {
        var remain = test.TakeLines(2, out string take);
        Debug.Assert(take == "1\n2");
        Debug.Assert(remain.ToString() == "3\n4\n\r\n6\n");
    }

    {
        var remain = test
                    .TakeLines(3, out string take1)
                    .TakeLines(2, out string take2);
        Debug.Assert(take1 == "1\n2\r\n3");
        Debug.Assert(take2 == "4\n");
        Debug.Assert(remain.ToString() == "6\n");
    }

    {
        // should be fine
        var remain = test.TakeLines(6, out string take);
        Debug.Assert(take == "1\n2\r\n3\n4\n\r\n6");
    }

    {
        bool pass = false;
        try {
            var remain = test.TakeLines(7, out string take);
        } catch (ArgumentOutOfRangeException) {
            pass = true;
        }
        Debug.Assert(pass);
    }

    {
        var remain = test.TakeLinesFromEnd(1, out string take);
        Debug.Assert(take == "6");
    }

    {
        var remain = test.TakeLinesFromEnd(2, out string take);
        Debug.Assert(take == "4\n\r\n6");
    }

    {
        bool pass = false;
        try {
            var remain = test.TakeLinesFromEnd(7, out string take);
        } catch (ArgumentOutOfRangeException) {
            pass = true;
        }
        Debug.Assert(pass);
    }

    {
        var remain = test.TakeLinesFromEnd(1, out string take);
        Debug.Assert(take == "6");
    }

    Console.WriteLine("all tests passed??");

}