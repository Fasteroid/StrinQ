namespace Fasteroid {

    /// <summary>
    /// A collection of methods like those from LINQ, but designed specifically for common string querying tasks.
    /// </summary>
    public static class StrinQ {

        /// <summary>
        /// Finds the first newline-like thing in the sequence.<br></br>
        /// Returns where it was and how many characters wide it was.
        /// </summary>
        public static (int, int) FirstNewline(this ReadOnlySpan<char> self) {
            int idx = self.IndexOf('\n');
            if( idx == -1 ) return (self.Length, 0); // EOL
            if( idx == 0 )  return (0, 1);           // cannot possibly be a crlf, otherwise the \r is out of bounds
            if( self[idx - 1] == '\r' ) return (idx - 1, 2); // crlf
            return (idx, 1); // lf
        }

        /// <inheritdoc cref="FirstNewline(ReadOnlySpan{char})"/>/>
        public static (int, int) FirstNewline(this string self) {
            return FirstNewline(self.AsSpan());
        }

        /// <summary>
        /// Does the same thing as <see cref="FirstNewline(string)(ReadOnlySpan{char})"/>, but searches from the end rather than the beginning.
        /// </summary>
        public static (int, int) LastNewline(this ReadOnlySpan<char> self) {
            int idx = self.LastIndexOf('\n');
            if( idx == -1 ) return (self.Length, 0); // EOL
            if( idx == 0 )  return (0, 1);           // again, cannot possibly be a crlf
            if( self[idx - 1] == '\r' ) return (idx - 1, 2); // crlf
            return (idx, 1); // lf
        }

        /// <inheritdoc cref="LastNewline(ReadOnlySpan{char})"/>
        public static (int, int) LastNewline(this string self) {
            return LastNewline(self.AsSpan());
        }

        /// <summary>
        /// Pushes the first <paramref name="count"/> lines into the output parameter <paramref name="taken"/>, and returns the rest.<br></br>
        /// </summary>
        /// <param name="self"></param>
        /// <param name="count"></param>
        /// <param name="taken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static ReadOnlySpan<char> TakeLines(this ReadOnlySpan<char> self, int count, out string taken) {
            int linesAcc = 1;
            int splitIdx = 0;
            ReadOnlySpan<char> remaining = self;

            while( linesAcc <= count ) {
                var (idx, stride) = remaining.FirstNewline();

                splitIdx  += idx; // don't add stride yet, if we return 'taken' this iteration we don't want the newline
                remaining = remaining.Slice(idx + stride);

                if( linesAcc == count || remaining.Length == 0 ) {
                    break;
                }

                splitIdx += stride;
                linesAcc++;
            }

            if( linesAcc == count ) {
                taken = self.Slice(0, splitIdx).ToString();
                return remaining;
            }

            throw new ArgumentOutOfRangeException("count", $"Not enough lines to take (expected {count} but there were only {linesAcc})");
        }

        /// <inheritdoc cref="TakeLines(ReadOnlySpan{char}, int, out string)"/>
        public static ReadOnlySpan<char> TakeLines(this string self, int count, out string taken) {
            return TakeLines(self.AsSpan(), count, out taken);
        }


        /// <summary>
        /// Like <see cref="TakeLines(ReadOnlySpan{char}, int, out string)"/>, but takes from the end of the string.
        /// </summary>
        public static ReadOnlySpan<char> TakeLinesFromEnd(this ReadOnlySpan<char> self, int count, out string taken) {
            int linesAcc = 1;
            int splitIdx = self.Length;
            ReadOnlySpan<char> remaining = self;

            while( linesAcc <= count ) {
                var (idx, stride) = remaining.LastNewline();

                splitIdx  -= idx; // don't add stride yet, if we return 'taken' this iteration we don't want the newline
                remaining = remaining.Slice(0, idx);

                if( linesAcc == count || remaining.Length == 0 ) {
                    break;
                }

                splitIdx -= stride;
                linesAcc++;
            }

            if( linesAcc == count ) {
                taken = self.Slice(0, splitIdx).ToString();
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
