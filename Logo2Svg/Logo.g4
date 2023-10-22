grammar Logo;

// Parser

program : White* command ( White+ command)* White* EOF
        ;

command : Right White+ Value
        | Forward White+ Value
        ;

// Lexer

Right   : 'RIGHT' | 'RT' ;
Forward : 'FORWARD' | 'FD' ;

Value   : [0-9]+ ;           
            
White   : [ \n\t\r] ;    
