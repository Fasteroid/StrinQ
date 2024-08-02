namespace Fasteroid {

    /// <summary>
    /// A collection of methods like those from LINQ, but designed specifically for common string querying tasks.
    /// </summary>
    public static class StrinQ {

        /// <summary>
        /// Finds the first line ending from left to right, which could be the end of the string.<br></br>
        /// </summary>
        /// <returns>
        /// 0-based index of the line ending and how many characters it spans
        /// </returns>
        public static (int, int) FirstLineEnding(this ReadOnlySpan<char> self) {
            int idx = self.IndexOf('\n');
            if( idx == -1 ) return (self.Length, 0); // EOL
            if( idx == 0 )  return (0, 1);           // cannot possibly be a crlf, otherwise the \r is out of bounds
            if( self[idx - 1] == '\r' ) return (idx - 1, 2); // crlf
            return (idx, 1); // lf
        }

        /// <inheritdoc cref="FirstLineEnding(ReadOnlySpan{char})"/>/>
        public static (int, int) FirstLineEnding(this string self) {
            return FirstLineEnding(self.AsSpan());
        }

        /// <summary>
        /// (opposite of <see cref="FirstLineEnding(ReadOnlySpan{char})"/>)<br></br>
        /// <br></br>
        /// Finds the first line "beginning" from right to left, which could be the beginning of the string.
        /// </summary>
        /// <returns>
        /// 0-based index of the line ending and how many characters it spans
        /// </returns>
        public static (int, int) LastLineBeginning(this ReadOnlySpan<char> self) {
            int idx = self.LastIndexOf('\n');
            if( idx == -1 ) return (0, 0); // EOL but assume beginning
            if( idx == 0 )  return (0, 1); // again, cannot possibly be a crlf
            if( self[idx - 1] == '\r' ) return (idx - 1, 2); // crlf
            return (idx, 1); // lf
        }

        /// <inheritdoc cref="LastLineBeginning(ReadOnlySpan{char})"/>
        public static (int, int) LastLineBeginning(this string self) {
            return LastLineBeginning(self.AsSpan());
        }

        /// <summary>
        /// Reads the first <paramref name="count"/> lines into the output parameter <paramref name="taken"/> and returns the rest.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="count">How many lines to take</param>
        /// <param name="taken">Output param for taken lines</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static ReadOnlySpan<char> TakeLines(this ReadOnlySpan<char> self, int count, out string taken) {
            int linesAcc = 1;
            int splitIdx = 0;
            ReadOnlySpan<char> remaining = self;

            while(linesAcc <= count) { // could also be while(true) but that makes me uncomfortable
                var (idx, stride) = remaining.FirstLineEnding();

                splitIdx += idx;
                remaining = remaining[(idx + stride)..];

                if( linesAcc == count || remaining.Length == 0 ) {
                    break;
                }

                linesAcc++;
                splitIdx += stride;
            }

            if( linesAcc == count ) {
                taken = self[..splitIdx].ToString();
                return remaining;
            }

            throw new ArgumentOutOfRangeException("count", $"Not enough lines to take (expected {count} but there were only {linesAcc})");
        }

        /// <inheritdoc cref="TakeLines(ReadOnlySpan{char}, int, out string)"/>
        public static ReadOnlySpan<char> TakeLines(this string self, int count, out string taken) {
            return TakeLines(self.AsSpan(), count, out taken);
        }


        /// <summary>
        /// (opposite of <see cref="TakeLines(ReadOnlySpan{char}, int, out string)"/>)<br></br>
        /// <br></br>
        /// Reads the last <paramref name="count"/> lines into the output parameter <paramref name="taken"/> and returns the rest.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="count">How many lines to take</param>
        /// <param name="taken">Output param for taken lines</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static ReadOnlySpan<char> TakeLinesFromEnd(this ReadOnlySpan<char> self, int count, out string taken) {
            int linesAcc = 1;
            int splitIdx = self.Length;
            ReadOnlySpan<char> remaining = self;

            while( linesAcc <= count ) {
                var (idx, stride) = remaining.LastLineBeginning();

                splitIdx  -= (remaining.Length - idx);
                remaining = remaining[..(idx)];

                if( linesAcc == count || remaining.Length == 0 ) {
                    splitIdx += stride; // skip the newline
                    break;
                }

                linesAcc++;
            }

            if( linesAcc == count ) {
                taken = self[splitIdx..].ToString();
                return remaining;
            }

            throw new ArgumentOutOfRangeException("count", $"Not enough lines to take (expected {count} but there were only {linesAcc})");
        }

        /// <inheritdoc cref="TakeLinesFromEnd(ReadOnlySpan{char}, int, out string)"/>
        public static ReadOnlySpan<char> TakeLinesFromEnd(this string self, int count, out string taken) {
            return TakeLinesFromEnd(self.AsSpan(), count, out taken);
        }
    }
}
