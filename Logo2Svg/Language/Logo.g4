grammar Logo;

// Parser

program : command+ EOF
        ;

command : Right Value
        | Forward Value
        ;

// Lexer

Right   : 'RIGHT' | 'RT' ;
Forward : 'FORWARD' | 'FD' ;

Value   : [0-9]+ ;           
            
White   : [ \n\t\r] -> skip;
    