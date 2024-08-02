# StrinQ

StrinQ is an open-source collection of extension methods similar to those from LINQ, but designed specifically for common string querying tasks.<br>
I somehow have severe doubts this will be useful anywhere aside from my one project where I need it, but I'm in too deep to not finish this package now.

If you have an idea for something useful to add, please open a PR!<br>
The repository is available [here](https://github.com/Fasteroid/StrinQ).
## Methods

### FirstLineEnding

Finds the first line ending from left to right, which could be the end of the string.

```csharp
public static (int, int) FirstLineEnding(this ReadOnlySpan<char> self)
public static (int, int) FirstLineEnding(this string self)
```

### LastLineBeginning

Finds the first line \"beginning\" from right to left, which could be the beginning of the string.

```csharp
public static (int, int) LastLineBeginning(this ReadOnlySpan<char> self)
public static (int, int) LastLineBeginning(this string self)
```

### TakeLines

Reads `count` lines into the output parameter `taken` and returns the rest.

```csharp
public static ReadOnlySpan<char> TakeLines(this ReadOnlySpan<char> self, int count, out string taken)
public static ReadOnlySpan<char> TakeLines(this string self, int count, out string taken)
```

Parameters:
- `count`: How many lines to take
- `taken`: Output parameter for taken lines

Throws: `ArgumentOutOfRangeException` if there aren't enough lines to take.

### TakeLinesFromEnd

Reads `count` lines from the end of the string into the output parameter `taken` and returns the rest.

```csharp
public static ReadOnlySpan<char> TakeLinesFromEnd(this ReadOnlySpan<char> self, int count, out string taken)
public static ReadOnlySpan<char> TakeLinesFromEnd(this string self, int count, out string taken)
```

Parameters:
- `count`: How many lines to take
- `taken`: Output parameter for taken lines

Throws: `ArgumentOutOfRangeException` if there aren't enough lines to skip.

### SkipLines

Skips `count` lines and returns the rest.

```csharp
public static ReadOnlySpan<char> SkipLines(this ReadOnlySpan<char> self, int count)
public static ReadOnlySpan<char> SkipLines(this string self, int count)
```

Parameters:
- `count`: How many lines to skip

Throws: `ArgumentOutOfRangeException` if there aren't enough lines to take.

### SkipLinesFromEnd

Skips `count` lines from the end of the string and returns the rest.

```csharp
public static ReadOnlySpan<char> SkipLinesFromEnd(this ReadOnlySpan<char> self, int count)
public static ReadOnlySpan<char> SkipLinesFromEnd(this string self, int count)
```

Parameters:
- `count`: How many lines to skip

Throws: `ArgumentOutOfRangeException` if there aren't enough lines to skip.
