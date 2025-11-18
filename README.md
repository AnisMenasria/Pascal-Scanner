# Lexical Analysis Scanner

This module performs tokenization for a subset of a Pascal-like language.  
It recognizes identifiers, numeric literals, keywords, and operators through a state-based DFA structure.

Usage:
- Create a new `Scanner` instance with the input string.
- Call `Scan()` repeatedly to retrieve the next token on each call.
- When an invalid sequence is encountered, the scanner returns an error token.
- Reaching the end of the input does not produce a special token, `Scan()` simply returns `null` once no characters remain.

Note: How you integrate the scanner into the rest of your project depends on your own implementation of subsequent phases.

