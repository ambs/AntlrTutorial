grammar Logo;

// Parser

program : command+ EOF
        ;

command : simpleCommand
        | SetPos squarePoint
        | SetXY simplePoint
        | Home
        | Arc expr expr
        ;

simpleCommand : cmd=(Right|Left|Forward|Back|SetX|SetY|SetH) expr ;

simplePoint : expr expr
            ;

squarePoint : '[' expr expr ']'
            ;

expr : cmd=(Sum|Difference|Product|Quotient|Power) expr expr  #prefixBinaryOp 
     | op=(Minus|'-') expr                                    #unaryMinus
     | '(' Sum expr+ ')'                                      #summation
     | '(' Product expr+ ')'                                  #product
     | '(' Quotient expr ')'                                  #quotient
     | <assoc=right> expr op='^' expr                         #binaryOp
     | <assoc=left> expr op=('*'|'/') expr                    #binaryOp
     | <assoc=left> expr op=('+'|'-') expr                    #binaryOp
     | value                                                  #scalar
     | '(' expr ')'                                           #scalar
     ;

value : IntegerValue
      | RealValue
      ;

// Lexer

Minus      : M I N U S ;
Power      : P O W E R ;
Quotient   : Q U O T I E N T ;
Product    : P R O D U C T ;
Difference : D I F F E R E N C E ;
Sum        : S U M ;

Right   : R I G H T | R T ;
Forward : F O R W A R D | F D ;
Arc     : A R C ;
Home    : H O M E ;
Back    : B A C K | B K ;
Left    : L E F T | L T ;
SetX    : S E T X ;
SetY    : S E T Y ;
SetXY   : S E T X Y ;
SetPos  : S E T P O S ;
SetH    : S E T H ( E A D I N G )? ;

IntegerValue   : [0-9]+ ;
RealValue      : [0-9]* '.' [0-9]+ ;

White   : [ \n\t\r] -> skip;
    
fragment A : [Aa] ;
fragment B : [Bb] ;
fragment C : [Cc] ;
fragment D : [Dd] ;
fragment E : [Ee] ;
fragment F : [Ff] ;
fragment G : [Gg] ;
fragment H : [Hh] ;
fragment I : [Ii] ;
fragment J : [Jj] ;
fragment K : [Kk] ;
fragment L : [Ll] ;
fragment M : [Mm] ;
fragment N : [Nn] ;
fragment O : [Oo] ;
fragment P : [Pp] ;
fragment Q : [Qq] ;
fragment R : [Rr] ;
fragment S : [Ss] ;
fragment T : [Tt] ;
fragment U : [Uu] ;
fragment V : [Vv] ;
fragment W : [Ww] ;
fragment X : [Xx] ;
fragment Y : [Yy] ;
fragment Z : [Zz] ;