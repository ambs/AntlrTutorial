lexer grammar LogoLexer;

RightSqBracket   : ']' ;
LeftSqBracket    : '[' ;
MinusSign        : '-' ;
PlusSign         : '+' ;
AsteriskSign     : '*' ;
SlashSigh        : '/' ;
LeftParen        : '(' ;
RightParen       : ')' ;
CircunflexSign   : '^' ;
PercentSign      : '%' ;
LessSign         : '<' ;
GreaterSign      : '>' ;
LessEqualSign    : '<=' ;
GreaterEqualSign : '>=' ;

Variable    : '"' [#a-zA-Z0-9_]+ { Text = Text.Substring(1); };
VariableRef : ':' [#a-zA-Z0-9_]+ { Text = Text.Substring(1); };
 
To           : T O ;
End          : E N D ;
 
PenDown      : P D | P E N D O W N ;
PenUp        : P U | P E N U P ;
SetPenColor  : S E T P E N C O L O R ;
SetPalette   : S E T P A L E T T E ;
SetPenSize   : S E T P E N S I Z E ;

Less         : L E S S (P | '?');
Greater      : G R E A T E R (P | '?');
LessEqual    : L E S S E Q U A L (P | '?');
GreaterEqual : G R E A T E R E Q U A L (P | '?');
IfElse       : I F E L S E ;
If           : I F ;
Bye          : B Y E ;
Repeat       : R E P E A T ;
Forever      : F O R E V E R ;
And          : A N D ;
Xor          : X O R ;
Or           : O R ;
True         : T R U E ;
False        : F A L S E ;
Make       : M A K E ;
Name       : N A M E ;
Thing      : T H I N G ;
Show       : S H O W ;
Minus      : M I N U S ;
Power      : P O W E R ;
Quotient   : Q U O T I E N T ;
Product    : P R O D U C T ;
Difference : D I F F E R E N C E ;
Sum        : S U M ;
Remainder  : R E M A I N D E R ;
Modulo     : M O D U L O ;
Abs        : A B S ;
Int        : I N T ;
Round      : R O U N D ;
Sqrt       : S Q R T ;
Exp        : E X P ;
Log10      : L O G '10' ;
Ln         : L N ;
Arctan     : A R C T A N ;
Sin        : S I N ;
Cos        : C O S ;
Tan        : T A N ;
Radarctan  : R A D A R C T A N ;
Radsin     : R A D S I N ;
Radcos     : R A D C O S ;
Radtan     : R A D T A N ;

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

StopTk  : S T O P ;

Literal : [a-zA-Z_]+ ;

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