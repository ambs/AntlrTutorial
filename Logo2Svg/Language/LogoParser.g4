parser grammar LogoParser;

options { tokenVocab=LogoLexer; }

program : command+ EOF
        ;

command : simpleCommand          #basicCommand
        | SetPos squarePoint     #setPosition
        | SetXY simplePoint      #setPosition
        | cmd=(Home|Bye)         #atomicCmd
        | Arc expr expr          #arc
        | Make Variable expr     #setVariable
        | Name expr Variable     #setVariable
        | Show expr              #show
        | controlStmt            #controlStatement      
        ;
        
cmdBlock : '[' command+ ']'
         ;        
        
controlStmt : Repeat expr cmdBlock                           #repeatStmt
            | Forever cmdBlock                               #foreverStmt
            | If ('[' expr ']' | expr) cmdBlock              #ifStmt
            | IfElse ('[' expr ']' | expr) cmdBlock cmdBlock #ifElseStmt
            ;  

simpleCommand : cmd=(Right|Left|Forward|Back|SetX|SetY|SetH) expr ;

simplePoint : expr expr
            ;

squarePoint : '[' expr expr ']'
            ;

expr : cmd=(Sum|Difference|Product|Quotient|Power|Remainder|Modulo
               |Less|Greater|LessEqual|GreaterEqual) expr expr             #prefixBinaryOp 
     | op=(Minus|MinusSign) expr                                           #unaryMinus
     | fun=(Abs|Int|Round|Sqrt|Exp|Log10|Ln|Arctan|Sin| Cos|Tan
               |Radarctan|Radsin|Radcos|Radtan) expr                       #arithFuncs
     | '(' fun=(Arctan|Radarctan) expr expr ')'                            #arithFuncs
     | '(' Sum expr+ ')'                                                   #summation
     | '(' Product expr+ ')'                                               #product
     | '(' Quotient expr ')'                                               #quotient
     | '(' cmd=(And|Or|Xor) expr+ ')'                                      #boolean
     | <assoc=right> expr op='^' expr                                      #binaryOp
     | <assoc=left> expr op=('*'|'/'|'%') expr                             #binaryOp
     | <assoc=left> expr op=('+'|'-') expr                                 #binaryOp
     | <assoc=left> expr op=('<'|'>'|LessEqualSign|GreaterEqualSign) expr  #binaryOp
     | <assoc=left> expr op=(And|Or|Xor) expr                              #binaryOp
     | value                                                               #scalar
     | '(' expr ')'                                                        #scalar
     | VariableRef                                                         #variable
     | Thing Variable                                                      #variable
     ; 

value : IntegerValue
      | RealValue
      | True
      | False
      ;
