using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Scanner
    {
        private string text;
        private int i;
        private string toSend = "";

        public (string Type, string Token)? CurrentToken { get; private set; }
        public List<(string Type, string Token)> Tokens { get; private set; } = new List<(string, string)>();

        private string[] keywords = {
            "program", "var", "begin", "end", "if", "then", "else",
            "while", "do", "for", "to", "repeat", "until",
            "procedure", "function", "const", "type", "record",
            "case", "of", "array", "integer", "real", "boolean", "char",
            "read", "readln", "write", "writeln", "and", "or", "not", "div", "mod"
        };


        public Scanner(string input)
        {
            text = input;
            i = 0;
        }

        public (string Type, string Token)? Scan()
        {
            CurrentToken = null;
            toSend = "";

            if (i >= text.Length)
                return null;

            state0();
            return CurrentToken;
        }

        // ---------------- STATES ----------------

        private void state0()
        {
            if (i >= text.Length)
            {
                syntaxError();
                return;
            }

            toSend += text[i];

            if (char.IsLetter(text[i])) { i++; state1(); }
            else if (char.IsDigit(text[i])) { i++; state2(); }
            else if ("+-*/".Contains(text[i])) { i++; state3(); }
            else if (",;.\"".Contains(text[i])) { i++; state4(); }
            else if (text[i] == ':') { i++; state5(); }
            else if (text[i] == '=') { i++; state6(); }
            else if (text[i] == '<') { i++; state8(); }
            else if (text[i] == '>') { i++; state9(); }
            else if (char.IsWhiteSpace(text[i])) { i++; state0(); }
            else syntaxError();
        }

        private void state1()
        {
            if (i >= text.Length) { validate(1); return; }

            if (char.IsLetter(text[i]) || char.IsDigit(text[i]) || text[i] == '_')
            {
                toSend += text[i];
                i++;
                state1();
            }
            else if (char.IsWhiteSpace(text[i]))
            {
                i++;
                validate(1);
            }
            else syntaxError();
        }

        private void state2()
        {
            if (i >= text.Length) { validate(2); return; }

            if (char.IsDigit(text[i]))
            {
                toSend += text[i];
                i++;
                state2();
            }
            else if (char.IsWhiteSpace(text[i]))
            {
                i++;
                validate(2);
            }
            else syntaxError();
        }

        private void state3()
        {
            if (i >= text.Length) { validate(3); return; }

            if (char.IsWhiteSpace(text[i]))
            {
                i++;
                validate(3);
            }
            else syntaxError();
        }

        private void state4()
        {
            if (i >= text.Length) { validate(4); return; }

            if (char.IsWhiteSpace(text[i]))
            {
                i++;
                validate(4);
            }
            else syntaxError();
        }

        private void state5()
        {

            Console.WriteLine("Test");
            if (i >= text.Length) { validate(5); return; }

            if (text[i] == '=')
            {
                toSend += text[i];
                i++;
                state7();
            }
            else if (char.IsWhiteSpace(text[i]))
            {
                i++;
                validate(5);
            }
            else syntaxError();
        }

        private void state6()
        {
            if (i >= text.Length) { validate(6); return; }

            if (char.IsWhiteSpace(text[i]))
            {

                i++;
                validate(6);
            }
            else syntaxError();

        }


        private void state7()
        {
            if (i >= text.Length) { validate(7); return; }

            if (char.IsWhiteSpace(text[i]))
            {
                i++;
                validate(7);
            }
            else syntaxError();
        }
        private void state8()
        {
            if (i >= text.Length) { validate(8); return; }

            if (text[i] == '=')
            {
                toSend += text[i];
                i++;
                state7();
            }
            else if (text[i] == '>')
            {
                toSend += text[i];
                i++;
                state7();
            }
            else if (char.IsWhiteSpace(text[i]))
            {
                i++;
                validate(8);
            }
            else syntaxError();
        }

        private void state9()
        {
            if (i >= text.Length) { validate(8); return; }

            if (text[i] == '=')
            {
                toSend += text[i];
                i++;
                state7();
            }
            else if (char.IsWhiteSpace(text[i]))
            {
                i++;
                validate(8);
            }
            else syntaxError();
        }

        // ---------------- VALIDATE ----------------

        private void validate(int type)
        {
            string token = toSend.Trim();
            string tokenType;

            switch (type)
            {
                case 1:
                    if (IsKeyword(token))
                    {
                        if (token == "and" || token == "or" || token == "div" || token == "mod") tokenType = "Operator";
                        else if (token == "not") tokenType = "Binary";
                        else tokenType = "Keyword";
                    }  
                    else
                        tokenType = "Identifier";
                    break;

                case 2: tokenType = "Number"; break;
                case 3: tokenType = "Operator"; break;
                case 4: tokenType = "Separator"; break;
                case 5: tokenType = "Colon"; break;
                case 6: tokenType = "Equal"; break;
                case 7: tokenType = "Operator"; break;
                case 8: tokenType = "Operator"; break;
                default: tokenType = "Unknown"; break;
            }

            CurrentToken = (tokenType, token);
            Tokens.Add((tokenType, token));
        }

        private bool IsKeyword(string token)
        {
            for (int i = 0; i < keywords.Length; i++)
            {
                if (keywords[i].Equals(token, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        private void syntaxError()
        {
            CurrentToken = ("Error", $"Syntax error near: {toSend.Trim()}");
            Tokens.Add(CurrentToken.Value);
        }
    }

}
