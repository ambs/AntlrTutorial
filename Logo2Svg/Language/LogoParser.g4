parser grammar LogoParser;

options { tokenVocab=LogoLexer; }

program : command+ EOF
        ;

command : simpleCommand
        | SetPos squarePoint
        | SetXY simplePoint
        | Home
        | Arc expr expr
        | Make Variable expr
        | Name expr Variable
        | Show expr
        ;

simpleCommand : cmd=(Right|Left|Forward|Back|SetX|SetY|SetH) expr ;

simplePoint : expr expr
            ;

squarePoint : '[' expr expr ']'
            ;

expr : cmd=(Sum|Difference|Product|
                  Quotient|Power|Remainder|Modulo) expr expr  #prefixBinaryOp 
     | op=(Minus|MinusSign) expr                              #unaryMinus
     | fun=(Abs|Int|Round|Sqrt|Exp|Log10|Ln|Arctan|Sin|
              Cos|Tan|Radarctan|Radsin|Radcos|Radtan) expr    #arithFuncs
     | '(' fun=(Arctan|Radarctan) expr expr ')'               #arithFuncs
     | '(' Sum expr+ ')'                                      #summation
     | '(' Product expr+ ')'                                  #product
     | '(' Quotient expr ')'                                  #quotient
     | <assoc=right> expr op='^' expr                         #binaryOp
     | <assoc=left> expr op=('*'|'/'|'%') expr                #binaryOp
     | <assoc=left> expr op=('+'|'-') expr                    #binaryOp
     | value                                                  #scalar
     | '(' expr ')'                                           #scalar
     | VariableRef                                            #variable
     | Thing Variable                                         #variable
     ;

value : IntegerValue
      | RealValue
      ;
