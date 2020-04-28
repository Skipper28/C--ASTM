
using System;

class Parser {
        const char Header = 'H';
        const char Patient = 'P';
        const char Order   = 'O';

        const char Result = 'R';

        const char Comment = 'C';
        const char Exit = 'E';
        string astm;


        public Parser (string astm){

                this.astm = astm;
                Console.WriteLine(astm);

        }

        public string ParseResults() {

                return null;

        }

}