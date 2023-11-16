grammar Logo;

// Parser

program : command+ EOF
        ;

command : simpleCommand
        | SetPos squarePoint
        | SetXY simplePoint
        | Home
        | Arc value value
        ;

simpleCommand : cmd=(Right|Left|Forward|Back|SetX|SetY|SetH) value ;

simplePoint : value value
            ;

squarePoint : '[' value value ']'
            ;

value : IntegerValue
      | RealValue
      ;

// Lexer

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