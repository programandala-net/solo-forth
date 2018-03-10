  \ meta.test.forth2012-test-suite.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803101607
  \ See change log at the end of the file

  \ XXX UNDER DEVELOPMENT

  \ ===========================================================

  \ From: John Hayes S1I
  \ Subject: core.fr
  \ Date: Mon, 27 Nov 95 13:10

  \ (C) 1995 JOHNS HOPKINS UNIVERSITY / APPLIED PHYSICS
  \ LABORATORY MAY BE DISTRIBUTED FREELY AS LONG AS THIS
  \ COPYRIGHT NOTICE REMAINS.  VERSION 1.2
  \
  \ THIS PROGRAM TESTS THE CORE WORDS OF AN ANS FORTH SYSTEM.
  \ THE PROGRAM ASSUMES A TWO'S COMPLEMENT IMPLEMENTATION WHERE
  \ THE RANGE OF SIGNED NUMBERS IS -2^(N-1) ... 2^(N-1)-1 AND
  \ THE RANGE OF UNSIGNED NUMBERS IS 0 ... 2^(N)-1.
  \
  \ I HAVEN'T FIGURED OUT HOW TO TEST KEY, QUIT, ABORT, OR
  \ ABORT"...  I ALSO HAVEN'T THOUGHT OF A WAY TO TEST
  \ ENVIRONMENT?...

  \ ===========================================================

CR CR SOURCE TYPE ( Preliminary test ) CR
SOURCE ( These lines test SOURCE, TYPE, ) TYPE CR
SOURCE ( CR and parenthetic comments ) TYPE CR

  \ It is now assumed that SOURCE, TYPE, CR and comments work.
  \ SOURCE and TYPE will be used to report test passes until
  \ something better can be defined to report errors. Until
  \ then reporting failures will depend on the system under
  \ test and will usually be via reporting an unrecognised word
  \ or possibly the system crashing. Tests will be numbered by
  \ #n from now on to assist fault finding. Test failures
  \ successes will be indicated by 'Pass: #n ...' and failures
  \ by 'Error: #n ...'

  \ Initial tests of `>IN +!` and `1+`.  Check that `n >IN +!`
  \ acts as an interpretive `IF`, where n >= 0.

  ( Pass #1: testing 0 >IN +! ) 0 >IN +! SOURCE TYPE CR
  ( Pass #2: testing 1 >IN +! ) 1 >IN +! xSOURCE TYPE CR
  ( Pass #3: testing 1+ ) 1 1+ >IN +! xxSOURCE TYPE CR

  \ Test results can now be reported using the >IN +! trick to
  \ skip 1 or more characters

  \ The value of `BASE` is unknown so it is not safe to use
  \ digits > 1, therefore it will be set it to binary and then
  \ decimal, this also tests `@` and `!`.

  ( Pass #4: testing @ ! BASE )
  0 1+ 1+ BASE ! BASE @ >IN +! xxSOURCE TYPE CR
  ( Set BASE to decimal ) 1010 BASE !
  ( Pass #5: testing decimal BASE )
  BASE @ >IN +! xxxxxxxxxxSOURCE TYPE CR

  \ Now in decimal mode and digits >1 can be used

  \ A better error reporting word is needed, much like `.(`
  \ which can't be used as it is in the Core Extension word
  \ set, similarly `PARSE` can't be used either, only `WORD` is
  \ available to parse a message and must be used in a colon
  \ definition. Therefore a simple colon definition is tested
  \ next.

  ( Pass #6: testing : ; )
: .SRC SOURCE TYPE CR ; 6 >IN +! xxxxxx.SRC

  ( Pass #7: testing number input )
19 >IN +! xxxxxxxxxxxxxxxxxxx.SRC


  \ `VARIABLE` is now tested as one will be used instead of
  \ `DROP` e.g. `Y !`.

  ( Pass #8: testing VARIABLE )
VARIABLE Y 2 Y ! Y @ >IN +! xx.SRC

: MSG 41 WORD COUNT ;
  \ 41 is the ASCII code for right parenthesis.

  \ The next tests MSG leaves 2 itmes on the data stack
  ( Pass #9: testing WORD COUNT )
5 MSG abcdef) Y ! Y ! >IN +! xxxxx.SRC

  ( Pass #10: testing WORD COUNT )
MSG ab) >IN +! xxY ! .SRC


  \ For reporting success .MSG( is now defined
: .MSG( MSG TYPE ; .MSG( Pass #11: testing WORD COUNT .MSG) CR

  \ To define an error reporting word, `= 2* AND` will be
  \ needed, test them first. This assumes 2's complement
  \ arithmetic.

1 1 = 1+ 1+ >IN +!
x.MSG( Pass #12: testing = returns all 1's for true) CR
1 0 = 1+ >IN +!
x.MSG( Pass #13: testing = returns 0 for false) CR
1 1 = -1 = 1+ 1+ >IN +!
x.MSG( Pass #14: testing -1 interpreted correctly) CR

1 2* >IN +! xx.MSG( Pass #15: testing 2*) CR
-1 2* 1+ 1+ 1+ >IN +! x.MSG( Pass #16: testing 2*) CR

-1 -1 AND 1+ 1+ >IN +! x.MSG( Pass #17: testing AND) CR
-1  0 AND 1+ >IN +! x.MSG( Pass #18: testing AND) CR
6  -1 AND >IN +! xxxxxx.MSG( Pass #19: testing AND) CR

  \ Define ~ to use as a 'to end of line' comment. `\` cannot
  \ be used as it a Core Extension word

: ~ ( -- ) SOURCE >IN ! Y ! ;

  \ Rather than relying on a pass message test words can now be
  \ defined to report errors in the event of a failure. For
  \ convenience words `?T~` and `?F~` are defined together with
  \ a helper `?~~` to test for TRUE and FALSE. Usage is:
  \
  \ <test> ?T~ Error #n: <message>
  \
  \ Success makes `>IN` index the `~` in `?T~` or `?F~` to skip
  \ the error message.  Hence it is essential there is only 1
  \ space between `?T~` and "Error".

: ?~~ ( -1 | 0 -- ) 2* >IN +! ;
: ?F~ ( f -- )  0 = ?~~ ;
: ?T~ ( f -- ) -1 = ?~~ ;

  \ Errors will be counted
VARIABLE #ERRS 0 #ERRS !
: Error  1 #ERRS +! -6 >IN +! .MSG( CR ;
: Pass  -1 #ERRS +! 1 >IN +! Error ;
~ Pass is defined solely to test Error

-1 ?F~ Pass #20: testing ?F~ ?~~ Pass Error
-1 ?T~ Error #1: testing ?T~ ?~~ ~

0  0 = 0= ?F~ Error #2: testing 0=
1  0 = 0= ?T~ Error #3: testing 0=
-1 0 = 0= ?T~ Error #4: testing 0=

0  0 = ?T~ Error #5: testing =
0  1 = ?F~ Error #6: testing =
1  0 = ?F~ Error #7: testing =
-1 1 = ?F~ Error #8: testing =
1 -1 = ?F~ Error #9: testing =

-1 0< ?T~ Error #10: testing 0<
0  0< ?F~ Error #11: testing 0<
1  0< ?F~ Error #12: testing 0<

DEPTH 1+ DEPTH = ?~~ Error #13: testing DEPTH

~ Up to now whether the data stack
~ was empty or not~ hasn't mattered
~ as long as it didn't overflow.
~ Now it will be emptied - also
~ removing any unreported underflow

DEPTH 0< 0= 1+ >IN +! ~ 0 0 >IN ! Remove any underflow
DEPTH 0= 1+ >IN +! ~ Y !  0 >IN ! Empty the stack
DEPTH 0= ?T~ Error #14: data stack not emptied

4 -5 SWAP 4 = SWAP -5 = = ?T~ Error #15: testing SWAP
111 222 333 444
DEPTH 4 = ?T~ Error #16: testing DEPTH
444 = SWAP 333 = = DEPTH 3 = = ?T~ Error#17: testing SWAP DEPTH
222 = SWAP 111 = = DEPTH 1 = = ?T~ Error#18: testing SWAP DEPTH
DEPTH 0= ?T~ Error #19: testing DEPTH = 0

~ From now on the stack is expected to be empty after a test so
~ ?~ will be defined to include a check on the stack depth.
~ Note that ?~~ was defined and used earlier instead of ?~ to
~ avoid (irritating) redefinition messages that many systems
~ display had ?~ simply been redefined

: ?~ ( -1 | 0 -- ) DEPTH 1 = AND ?~~ ;

~ -1 test success, 0 test failure

123 -1 ?~ Pass #21: testing ?~
Y !   ~ equivalent to DROP

~ Testing the remaining Core words used in the Hayes tester,
~ with the above definitions these are straightforward

1 DROP DEPTH 0= ?~ Error #20: testing DROP
123 DUP  = ?~ Error #21: testing DUP
123 ?DUP = ?~ Error #22: testing ?DUP
0  ?DUP 0= ?~ Error #23: testing ?DUP
123  111  + 234  = ?~ Error #24: testing +
123  -111 + 12   = ?~ Error #25: testing +
-123 111  + -12  = ?~ Error #26: testing +
-123 -111 + -234 = ?~ Error #27: testing +
-1 NEGATE 1 = ?~ Error #28: testing NEGATE
0  NEGATE 0=  ?~ Error #29: testing NEGATE
987 NEGATE -987 = ?~ Error #30: testing NEGATE
HERE DEPTH SWAP DROP 1 = ?~ Error #31: testing HERE
CREATE TST1 HERE TST1 = ?~ Error #32: testing CREATE HERE
16  ALLOT HERE TST1 NEGATE + 16 = ?~ Error #33: testing ALLOT
-16 ALLOT HERE TST1 = ?~ Error #34: testing ALLOT
0 CELLS 0= ?~ Error #35: testing CELLS
1 CELLS ALLOT HERE TST1 NEGATE + VARIABLE CSZ CSZ !
CSZ @ 0= 0= ?~ Error #36: testing CELLS
3 CELLS CSZ @ DUP 2* + = ?~ Error #37: testing CELLS
-3 CELLS CSZ @ DUP 2* + + 0= ?~ Error #38: testing CELLS
: TST2 ( f -- n ) DUP IF 1+ THEN ;
0 TST2 0=  ?~ Error #39: testing IF THEN
1 TST2 2 = ?~ Error #40: testing IF THEN
: TST3 ( n1 -- n2 ) IF 123 ELSE 234 THEN ;
0 TST3 234 = ?~ Error #41: testing IF ELSE THEN
1 TST3 123 = ?~ Error #42: testing IF ELSE THEN
: TST4 ( -- n ) 0 5 0 DO 1+ LOOP ;
TST4 5 = ?~ Error #43: testing DO LOOP
: TST5 ( -- n ) 0 10 0 DO I + LOOP ;
TST5 45 = ?~ Error #44: testing I
: TST6 ( -- n ) 0 10 0 DO DUP 5 = IF LEAVE ELSE 1+ THEN LOOP ;
TST6 5 = ?~ Error #45: testing LEAVE
: TST7 ( -- n1 n2 ) 123 >R 234 R> ;
TST7 NEGATE + 111 = ?~ Error #46: testing >R R>
: TST8 ( -- ch ) [CHAR] A ;
TST8 65 = ?~ Error #47: testing [CHAR]
: TST9 ( -- )
  [CHAR] s [CHAR] s [CHAR] a [CHAR] P 4 0 DO EMIT LOOP ;
TST9 .MSG(  #22: testing EMIT) CR
: TST10 ( -- ) S" Pass #23: testing S" TYPE [CHAR] " EMIT CR ;
TST10

~ The Hayes tester uses some words from the Core extension word
~ set. These will be conditionally defined following definition
~ of a word called ?DEFINED to determine whether these are
~ already defined

VARIABLE TIMM1 0 TIMM1 !
: TIMM2  123 TIMM1 ! ; IMMEDIATE
: TIMM3 TIMM2 ; TIMM1 @ 123 = ?~ Error #48: testing IMMEDIATE

: ?DEFINED ( "name" -- 0 | -1 ) 32 WORD FIND SWAP DROP 0= 0= ;
?DEFINED SWAP ?~ Error #49: testing FIND ?DEFINED
?DEFINED <<no-such-word-hopefully>> 0=
?~ Error #50 testing FIND ?DEFINED

?DEFINED \ ?~ : \ ~ ; IMMEDIATE
  \ Error #51: testing \
: TIMM4  \ Error #52: testing \ is IMMEDIATE
;

~ TRUE and FALSE are defined as colon definitions
~ to avoid using and having to test CONSTANT

?DEFINED TRUE  ?~ : TRUE 1 NEGATE ;
?DEFINED FALSE ?~ : FALSE 0 ;
?DEFINED HEX   ?~ : HEX 16 BASE ! ;

TRUE -1 = ?~ Error #53: testing TRUE
FALSE 0=  ?~ Error #54: testing FALSE
10 HEX 0A = ?~ Error #55: testing HEX
AB 0A BASE ! 171 = ?~ Error #56: testing hex number

~ Delete the ~ on the next 2 lines to check the final error
~ report
~ Error #998: testing a deliberate failure
~ Error #999: testing a deliberate failure

~ Describe the messages that should be seen. The previously
~ defined .MSG( can be used for text messages

CR .MSG( Results: ) CR
CR .MSG( Pass messages #1 to #23 should be displayed above)
CR .MSG( and no error messages) CR

~ Finally display a message giving the number of tests that
~ failed.  This is complicated by the fact that untested words
~ including .( ." and .  cannot be used. Also more colon
~ definitions shouldn't be defined than are ~ needed. To
~ display a number, note that the number of errors will have
~ one or two digits at most and an interpretive loop can be
~ used to display those.

CR
0 #ERRS @
~ Loop to calculate the 10's digit (if any)
DUP NEGATE 9 + 0< NEGATE >IN +! ( -10 + SWAP 1+ SWAP 0 >IN ! )
~ Display the error count
SWAP ?DUP 0= 1+ >IN +! ( 48 + EMIT ( ) 48 + EMIT

.MSG(  test) #ERRS @ 1 = 1+ >IN +! ~ .MSG( s)
.MSG(  failed out of 56 additional tests) CR

CR CR .MSG( --- End of Preliminary Tests --- ) CR

  \ ===========================================================

CR
TESTING CORE WORDS
HEX

  \ ===========================================================

TESTING BASIC ASSUMPTIONS

T{ -> }T \ START WITH CLEAN SLATE
  \ TEST IF ANY BITS ARE SET; ANSWER IN BASE 1
T{ : BITSSET? IF 0 0 ELSE 0 THEN ; -> }T
T{  0 BITSSET? -> 0 }T  \ ZERO IS ALL BITS CLEAR
T{  1 BITSSET? -> 0 0 }T  \ OTHER NUMBER HAVE AT LEAST ONE BIT
T{ -1 BITSSET? -> 0 0 }T

  \ ===========================================================

TESTING BOOLEANS: INVERT AND OR XOR

T{ 0 0 AND -> 0 }T
T{ 0 1 AND -> 0 }T
T{ 1 0 AND -> 0 }T
T{ 1 1 AND -> 1 }T

T{ 0 INVERT 1 AND -> 1 }T
T{ 1 INVERT 1 AND -> 0 }T

0    CONSTANT 0S
0 INVERT CONSTANT 1S

T{ 0S INVERT -> 1S }T
T{ 1S INVERT -> 0S }T

T{ 0S 0S AND -> 0S }T
T{ 0S 1S AND -> 0S }T
T{ 1S 0S AND -> 0S }T
T{ 1S 1S AND -> 1S }T

T{ 0S 0S OR -> 0S }T
T{ 0S 1S OR -> 1S }T
T{ 1S 0S OR -> 1S }T
T{ 1S 1S OR -> 1S }T

T{ 0S 0S XOR -> 0S }T
T{ 0S 1S XOR -> 1S }T
T{ 1S 0S XOR -> 1S }T
T{ 1S 1S XOR -> 0S }T

  \ ===========================================================

TESTING 2* 2/ LSHIFT RSHIFT

  \ WE TRUST 1S, INVERT, AND BITSSET?; WE WILL CONFIRM RSHIFT
  \ LATER

1S 1 RSHIFT INVERT CONSTANT MSB
T{ MSB BITSSET? -> 0 0 }T

T{ 0S 2* -> 0S }T
T{ 1 2* -> 2 }T
T{ 4000 2* -> 8000 }T
T{ 1S 2* 1 XOR -> 1S }T
T{ MSB 2* -> 0S }T

T{ 0S 2/ -> 0S }T
T{ 1 2/ -> 0 }T
T{ 4000 2/ -> 2000 }T
T{ 1S 2/ -> 1S }T            \ MSB PROPOGATED
T{ 1S 1 XOR 2/ -> 1S }T
T{ MSB 2/ MSB AND -> MSB }T

T{ 1 0 LSHIFT -> 1 }T
T{ 1 1 LSHIFT -> 2 }T
T{ 1 2 LSHIFT -> 4 }T
T{ 1 F LSHIFT -> 8000 }T         \ BIGGEST GUARANTEED SHIFT
T{ 1S 1 LSHIFT 1 XOR -> 1S }T
T{ MSB 1 LSHIFT -> 0 }T

T{ 1 0 RSHIFT -> 1 }T
T{ 1 1 RSHIFT -> 0 }T
T{ 2 1 RSHIFT -> 1 }T
T{ 4 2 RSHIFT -> 1 }T
T{ 8000 F RSHIFT -> 1 }T         \ BIGGEST
T{ MSB 1 RSHIFT MSB AND -> 0 }T      \ RSHIFT ZERO FILLS MSBS
T{ MSB 1 RSHIFT 2* -> MSB }T

  \ ===========================================================

TESTING COMPARISONS: 0= = 0< < > U< MIN MAX

0 INVERT         CONSTANT MAX-UINT
0 INVERT 1 RSHIFT      CONSTANT MAX-INT
0 INVERT 1 RSHIFT INVERT   CONSTANT MIN-INT
0 INVERT 1 RSHIFT      CONSTANT MID-UINT
0 INVERT 1 RSHIFT INVERT   CONSTANT MID-UINT+1

0S CONSTANT <FALSE>
1S CONSTANT <TRUE>

T{ 0 0= -> <TRUE> }T
T{ 1 0= -> <FALSE> }T
T{ 2 0= -> <FALSE> }T
T{ -1 0= -> <FALSE> }T
T{ MAX-UINT 0= -> <FALSE> }T
T{ MIN-INT 0= -> <FALSE> }T
T{ MAX-INT 0= -> <FALSE> }T

T{ 0 0 = -> <TRUE> }T
T{ 1 1 = -> <TRUE> }T
T{ -1 -1 = -> <TRUE> }T
T{ 1 0 = -> <FALSE> }T
T{ -1 0 = -> <FALSE> }T
T{ 0 1 = -> <FALSE> }T
T{ 0 -1 = -> <FALSE> }T

T{ 0 0< -> <FALSE> }T
T{ -1 0< -> <TRUE> }T
T{ MIN-INT 0< -> <TRUE> }T
T{ 1 0< -> <FALSE> }T
T{ MAX-INT 0< -> <FALSE> }T

T{ 0 1 < -> <TRUE> }T
T{ 1 2 < -> <TRUE> }T
T{ -1 0 < -> <TRUE> }T
T{ -1 1 < -> <TRUE> }T
T{ MIN-INT 0 < -> <TRUE> }T
T{ MIN-INT MAX-INT < -> <TRUE> }T
T{ 0 MAX-INT < -> <TRUE> }T
T{ 0 0 < -> <FALSE> }T
T{ 1 1 < -> <FALSE> }T
T{ 1 0 < -> <FALSE> }T
T{ 2 1 < -> <FALSE> }T
T{ 0 -1 < -> <FALSE> }T
T{ 1 -1 < -> <FALSE> }T
T{ 0 MIN-INT < -> <FALSE> }T
T{ MAX-INT MIN-INT < -> <FALSE> }T
T{ MAX-INT 0 < -> <FALSE> }T

T{ 0 1 > -> <FALSE> }T
T{ 1 2 > -> <FALSE> }T
T{ -1 0 > -> <FALSE> }T
T{ -1 1 > -> <FALSE> }T
T{ MIN-INT 0 > -> <FALSE> }T
T{ MIN-INT MAX-INT > -> <FALSE> }T
T{ 0 MAX-INT > -> <FALSE> }T
T{ 0 0 > -> <FALSE> }T
T{ 1 1 > -> <FALSE> }T
T{ 1 0 > -> <TRUE> }T
T{ 2 1 > -> <TRUE> }T
T{ 0 -1 > -> <TRUE> }T
T{ 1 -1 > -> <TRUE> }T
T{ 0 MIN-INT > -> <TRUE> }T
T{ MAX-INT MIN-INT > -> <TRUE> }T
T{ MAX-INT 0 > -> <TRUE> }T

T{ 0 1 U< -> <TRUE> }T
T{ 1 2 U< -> <TRUE> }T
T{ 0 MID-UINT U< -> <TRUE> }T
T{ 0 MAX-UINT U< -> <TRUE> }T
T{ MID-UINT MAX-UINT U< -> <TRUE> }T
T{ 0 0 U< -> <FALSE> }T
T{ 1 1 U< -> <FALSE> }T
T{ 1 0 U< -> <FALSE> }T
T{ 2 1 U< -> <FALSE> }T
T{ MID-UINT 0 U< -> <FALSE> }T
T{ MAX-UINT 0 U< -> <FALSE> }T
T{ MAX-UINT MID-UINT U< -> <FALSE> }T

T{ 0 1 MIN -> 0 }T
T{ 1 2 MIN -> 1 }T
T{ -1 0 MIN -> -1 }T
T{ -1 1 MIN -> -1 }T
T{ MIN-INT 0 MIN -> MIN-INT }T
T{ MIN-INT MAX-INT MIN -> MIN-INT }T
T{ 0 MAX-INT MIN -> 0 }T
T{ 0 0 MIN -> 0 }T
T{ 1 1 MIN -> 1 }T
T{ 1 0 MIN -> 0 }T
T{ 2 1 MIN -> 1 }T
T{ 0 -1 MIN -> -1 }T
T{ 1 -1 MIN -> -1 }T
T{ 0 MIN-INT MIN -> MIN-INT }T
T{ MAX-INT MIN-INT MIN -> MIN-INT }T
T{ MAX-INT 0 MIN -> 0 }T

T{ 0 1 MAX -> 1 }T
T{ 1 2 MAX -> 2 }T
T{ -1 0 MAX -> 0 }T
T{ -1 1 MAX -> 1 }T
T{ MIN-INT 0 MAX -> 0 }T
T{ MIN-INT MAX-INT MAX -> MAX-INT }T
T{ 0 MAX-INT MAX -> MAX-INT }T
T{ 0 0 MAX -> 0 }T
T{ 1 1 MAX -> 1 }T
T{ 1 0 MAX -> 1 }T
T{ 2 1 MAX -> 2 }T
T{ 0 -1 MAX -> 0 }T
T{ 1 -1 MAX -> 1 }T
T{ 0 MIN-INT MAX -> 0 }T
T{ MAX-INT MIN-INT MAX -> MAX-INT }T
T{ MAX-INT 0 MAX -> MAX-INT }T

  \ ===========================================================

TESTING STACK OPS: 2DROP 2DUP 2OVER 2SWAP ?DUP DEPTH DROP DUP
TESTING OVER ROT SWAP

T{ 1 2 2DROP -> }T
T{ 1 2 2DUP -> 1 2 1 2 }T
T{ 1 2 3 4 2OVER -> 1 2 3 4 1 2 }T
T{ 1 2 3 4 2SWAP -> 3 4 1 2 }T
T{ 0 ?DUP -> 0 }T
T{ 1 ?DUP -> 1 1 }T
T{ -1 ?DUP -> -1 -1 }T
T{ DEPTH -> 0 }T
T{ 0 DEPTH -> 0 1 }T
T{ 0 1 DEPTH -> 0 1 2 }T
T{ 0 DROP -> }T
T{ 1 2 DROP -> 1 }T
T{ 1 DUP -> 1 1 }T
T{ 1 2 OVER -> 1 2 1 }T
T{ 1 2 3 ROT -> 2 3 1 }T
T{ 1 2 SWAP -> 2 1 }T

  \ ===========================================================

TESTING >R R> R@

T{ : GR1 >R R> ; -> }T
T{ : GR2 >R R@ R> DROP ; -> }T
T{ 123 GR1 -> 123 }T
T{ 123 GR2 -> 123 }T
T{ 1S GR1 -> 1S }T   ( RETURN STACK HOLDS CELLS )

  \ ===========================================================

TESTING ADD/SUBTRACT: + - 1+ 1- ABS NEGATE

T{ 0 5 + -> 5 }T
T{ 5 0 + -> 5 }T
T{ 0 -5 + -> -5 }T
T{ -5 0 + -> -5 }T
T{ 1 2 + -> 3 }T
T{ 1 -2 + -> -1 }T
T{ -1 2 + -> 1 }T
T{ -1 -2 + -> -3 }T
T{ -1 1 + -> 0 }T
T{ MID-UINT 1 + -> MID-UINT+1 }T

T{ 0 5 - -> -5 }T
T{ 5 0 - -> 5 }T
T{ 0 -5 - -> 5 }T
T{ -5 0 - -> -5 }T
T{ 1 2 - -> -1 }T
T{ 1 -2 - -> 3 }T
T{ -1 2 - -> -3 }T
T{ -1 -2 - -> 1 }T
T{ 0 1 - -> -1 }T
T{ MID-UINT+1 1 - -> MID-UINT }T

T{ 0 1+ -> 1 }T
T{ -1 1+ -> 0 }T
T{ 1 1+ -> 2 }T
T{ MID-UINT 1+ -> MID-UINT+1 }T

T{ 2 1- -> 1 }T
T{ 1 1- -> 0 }T
T{ 0 1- -> -1 }T
T{ MID-UINT+1 1- -> MID-UINT }T

T{ 0 NEGATE -> 0 }T
T{ 1 NEGATE -> -1 }T
T{ -1 NEGATE -> 1 }T
T{ 2 NEGATE -> -2 }T
T{ -2 NEGATE -> 2 }T

T{ 0 ABS -> 0 }T
T{ 1 ABS -> 1 }T
T{ -1 ABS -> 1 }T
T{ MIN-INT ABS -> MID-UINT+1 }T

  \ ===========================================================

TESTING MULTIPLY: S>D * M* UM*

T{ 0 S>D -> 0 0 }T
T{ 1 S>D -> 1 0 }T
T{ 2 S>D -> 2 0 }T
T{ -1 S>D -> -1 -1 }T
T{ -2 S>D -> -2 -1 }T
T{ MIN-INT S>D -> MIN-INT -1 }T
T{ MAX-INT S>D -> MAX-INT 0 }T

T{ 0 0 M* -> 0 S>D }T
T{ 0 1 M* -> 0 S>D }T
T{ 1 0 M* -> 0 S>D }T
T{ 1 2 M* -> 2 S>D }T
T{ 2 1 M* -> 2 S>D }T
T{ 3 3 M* -> 9 S>D }T
T{ -3 3 M* -> -9 S>D }T
T{ 3 -3 M* -> -9 S>D }T
T{ -3 -3 M* -> 9 S>D }T
T{ 0 MIN-INT M* -> 0 S>D }T
T{ 1 MIN-INT M* -> MIN-INT S>D }T
T{ 2 MIN-INT M* -> 0 1S }T
T{ 0 MAX-INT M* -> 0 S>D }T
T{ 1 MAX-INT M* -> MAX-INT S>D }T
T{ 2 MAX-INT M* -> MAX-INT 1 LSHIFT 0 }T
T{ MIN-INT MIN-INT M* -> 0 MSB 1 RSHIFT }T
T{ MAX-INT MIN-INT M* -> MSB MSB 2/ }T
T{ MAX-INT MAX-INT M* -> 1 MSB 2/ INVERT }T

T{ 0 0 * -> 0 }T            \ TEST IDENTITIES
T{ 0 1 * -> 0 }T
T{ 1 0 * -> 0 }T
T{ 1 2 * -> 2 }T
T{ 2 1 * -> 2 }T
T{ 3 3 * -> 9 }T
T{ -3 3 * -> -9 }T
T{ 3 -3 * -> -9 }T
T{ -3 -3 * -> 9 }T

T{ MID-UINT+1 1 RSHIFT 2 * -> MID-UINT+1 }T
T{ MID-UINT+1 2 RSHIFT 4 * -> MID-UINT+1 }T
T{ MID-UINT+1 1 RSHIFT MID-UINT+1 OR 2 * -> MID-UINT+1 }T

T{ 0 0 UM* -> 0 0 }T
T{ 0 1 UM* -> 0 0 }T
T{ 1 0 UM* -> 0 0 }T
T{ 1 2 UM* -> 2 0 }T
T{ 2 1 UM* -> 2 0 }T
T{ 3 3 UM* -> 9 0 }T

T{ MID-UINT+1 1 RSHIFT 2 UM* -> MID-UINT+1 0 }T
T{ MID-UINT+1 2 UM* -> 0 1 }T
T{ MID-UINT+1 4 UM* -> 0 2 }T
T{ 1S 2 UM* -> 1S 1 LSHIFT 1 }T
T{ MAX-UINT MAX-UINT UM* -> 1 1 INVERT }T

  \ ===========================================================

TESTING DIVIDE: FM/MOD SM/REM UM/MOD */ */MOD / /MOD MOD

T{ 0 S>D 1 FM/MOD -> 0 0 }T
T{ 1 S>D 1 FM/MOD -> 0 1 }T
T{ 2 S>D 1 FM/MOD -> 0 2 }T
T{ -1 S>D 1 FM/MOD -> 0 -1 }T
T{ -2 S>D 1 FM/MOD -> 0 -2 }T
T{ 0 S>D -1 FM/MOD -> 0 0 }T
T{ 1 S>D -1 FM/MOD -> 0 -1 }T
T{ 2 S>D -1 FM/MOD -> 0 -2 }T
T{ -1 S>D -1 FM/MOD -> 0 1 }T
T{ -2 S>D -1 FM/MOD -> 0 2 }T
T{ 2 S>D 2 FM/MOD -> 0 1 }T
T{ -1 S>D -1 FM/MOD -> 0 1 }T
T{ -2 S>D -2 FM/MOD -> 0 1 }T
T{  7 S>D  3 FM/MOD -> 1 2 }T
T{  7 S>D -3 FM/MOD -> -2 -3 }T
T{ -7 S>D  3 FM/MOD -> 2 -3 }T
T{ -7 S>D -3 FM/MOD -> -1 2 }T
T{ MAX-INT S>D 1 FM/MOD -> 0 MAX-INT }T
T{ MIN-INT S>D 1 FM/MOD -> 0 MIN-INT }T
T{ MAX-INT S>D MAX-INT FM/MOD -> 0 1 }T
T{ MIN-INT S>D MIN-INT FM/MOD -> 0 1 }T
T{ 1S 1 4 FM/MOD -> 3 MAX-INT }T
T{ 1 MIN-INT M* 1 FM/MOD -> 0 MIN-INT }T
T{ 1 MIN-INT M* MIN-INT FM/MOD -> 0 1 }T
T{ 2 MIN-INT M* 2 FM/MOD -> 0 MIN-INT }T
T{ 2 MIN-INT M* MIN-INT FM/MOD -> 0 2 }T
T{ 1 MAX-INT M* 1 FM/MOD -> 0 MAX-INT }T
T{ 1 MAX-INT M* MAX-INT FM/MOD -> 0 1 }T
T{ 2 MAX-INT M* 2 FM/MOD -> 0 MAX-INT }T
T{ 2 MAX-INT M* MAX-INT FM/MOD -> 0 2 }T
T{ MIN-INT MIN-INT M* MIN-INT FM/MOD -> 0 MIN-INT }T
T{ MIN-INT MAX-INT M* MIN-INT FM/MOD -> 0 MAX-INT }T
T{ MIN-INT MAX-INT M* MAX-INT FM/MOD -> 0 MIN-INT }T
T{ MAX-INT MAX-INT M* MAX-INT FM/MOD -> 0 MAX-INT }T

T{ 0 S>D 1 SM/REM -> 0 0 }T
T{ 1 S>D 1 SM/REM -> 0 1 }T
T{ 2 S>D 1 SM/REM -> 0 2 }T
T{ -1 S>D 1 SM/REM -> 0 -1 }T
T{ -2 S>D 1 SM/REM -> 0 -2 }T
T{ 0 S>D -1 SM/REM -> 0 0 }T
T{ 1 S>D -1 SM/REM -> 0 -1 }T
T{ 2 S>D -1 SM/REM -> 0 -2 }T
T{ -1 S>D -1 SM/REM -> 0 1 }T
T{ -2 S>D -1 SM/REM -> 0 2 }T
T{ 2 S>D 2 SM/REM -> 0 1 }T
T{ -1 S>D -1 SM/REM -> 0 1 }T
T{ -2 S>D -2 SM/REM -> 0 1 }T
T{  7 S>D  3 SM/REM -> 1 2 }T
T{  7 S>D -3 SM/REM -> 1 -2 }T
T{ -7 S>D  3 SM/REM -> -1 -2 }T
T{ -7 S>D -3 SM/REM -> -1 2 }T
T{ MAX-INT S>D 1 SM/REM -> 0 MAX-INT }T
T{ MIN-INT S>D 1 SM/REM -> 0 MIN-INT }T
T{ MAX-INT S>D MAX-INT SM/REM -> 0 1 }T
T{ MIN-INT S>D MIN-INT SM/REM -> 0 1 }T
T{ 1S 1 4 SM/REM -> 3 MAX-INT }T
T{ 2 MIN-INT M* 2 SM/REM -> 0 MIN-INT }T
T{ 2 MIN-INT M* MIN-INT SM/REM -> 0 2 }T
T{ 2 MAX-INT M* 2 SM/REM -> 0 MAX-INT }T
T{ 2 MAX-INT M* MAX-INT SM/REM -> 0 2 }T
T{ MIN-INT MIN-INT M* MIN-INT SM/REM -> 0 MIN-INT }T
T{ MIN-INT MAX-INT M* MIN-INT SM/REM -> 0 MAX-INT }T
T{ MIN-INT MAX-INT M* MAX-INT SM/REM -> 0 MIN-INT }T
T{ MAX-INT MAX-INT M* MAX-INT SM/REM -> 0 MAX-INT }T

T{ 0 0 1 UM/MOD -> 0 0 }T
T{ 1 0 1 UM/MOD -> 0 1 }T
T{ 1 0 2 UM/MOD -> 1 0 }T
T{ 3 0 2 UM/MOD -> 1 1 }T
T{ MAX-UINT 2 UM* 2 UM/MOD -> 0 MAX-UINT }T
T{ MAX-UINT 2 UM* MAX-UINT UM/MOD -> 0 2 }T
T{ MAX-UINT MAX-UINT UM* MAX-UINT UM/MOD -> 0 MAX-UINT }T

: IFFLOORED
   [ -3 2 / -2 = INVERT ] LITERAL IF POSTPONE \ THEN ;

: IFSYM
   [ -3 2 / -1 = INVERT ] LITERAL IF POSTPONE \ THEN ;

  \ The system might do either floored or symmetric division.
  \ since we have already tested `M*`, `FM/MOD`, and `SM/REM`
  \ we can use them in test.

IFFLOORED : T/MOD  >R S>D R> FM/MOD ;
IFFLOORED : T/     T/MOD SWAP DROP ;
IFFLOORED : TMOD   T/MOD DROP ;
IFFLOORED : T*/MOD >R M* R> FM/MOD ;
IFFLOORED : T*/    T*/MOD SWAP DROP ;
IFSYM     : T/MOD  >R S>D R> SM/REM ;
IFSYM     : T/     T/MOD SWAP DROP ;
IFSYM     : TMOD   T/MOD DROP ;
IFSYM     : T*/MOD >R M* R> SM/REM ;
IFSYM     : T*/    T*/MOD SWAP DROP ;

T{ 0 1 /MOD -> 0 1 T/MOD }T
T{ 1 1 /MOD -> 1 1 T/MOD }T
T{ 2 1 /MOD -> 2 1 T/MOD }T
T{ -1 1 /MOD -> -1 1 T/MOD }T
T{ -2 1 /MOD -> -2 1 T/MOD }T
T{ 0 -1 /MOD -> 0 -1 T/MOD }T
T{ 1 -1 /MOD -> 1 -1 T/MOD }T
T{ 2 -1 /MOD -> 2 -1 T/MOD }T
T{ -1 -1 /MOD -> -1 -1 T/MOD }T
T{ -2 -1 /MOD -> -2 -1 T/MOD }T
T{ 2 2 /MOD -> 2 2 T/MOD }T
T{ -1 -1 /MOD -> -1 -1 T/MOD }T
T{ -2 -2 /MOD -> -2 -2 T/MOD }T
T{ 7 3 /MOD -> 7 3 T/MOD }T
T{ 7 -3 /MOD -> 7 -3 T/MOD }T
T{ -7 3 /MOD -> -7 3 T/MOD }T
T{ -7 -3 /MOD -> -7 -3 T/MOD }T
T{ MAX-INT 1 /MOD -> MAX-INT 1 T/MOD }T
T{ MIN-INT 1 /MOD -> MIN-INT 1 T/MOD }T
T{ MAX-INT MAX-INT /MOD -> MAX-INT MAX-INT T/MOD }T
T{ MIN-INT MIN-INT /MOD -> MIN-INT MIN-INT T/MOD }T

T{ 0 1 / -> 0 1 T/ }T
T{ 1 1 / -> 1 1 T/ }T
T{ 2 1 / -> 2 1 T/ }T
T{ -1 1 / -> -1 1 T/ }T
T{ -2 1 / -> -2 1 T/ }T
T{ 0 -1 / -> 0 -1 T/ }T
T{ 1 -1 / -> 1 -1 T/ }T
T{ 2 -1 / -> 2 -1 T/ }T
T{ -1 -1 / -> -1 -1 T/ }T
T{ -2 -1 / -> -2 -1 T/ }T
T{ 2 2 / -> 2 2 T/ }T
T{ -1 -1 / -> -1 -1 T/ }T
T{ -2 -2 / -> -2 -2 T/ }T
T{ 7 3 / -> 7 3 T/ }T
T{ 7 -3 / -> 7 -3 T/ }T
T{ -7 3 / -> -7 3 T/ }T
T{ -7 -3 / -> -7 -3 T/ }T
T{ MAX-INT 1 / -> MAX-INT 1 T/ }T
T{ MIN-INT 1 / -> MIN-INT 1 T/ }T
T{ MAX-INT MAX-INT / -> MAX-INT MAX-INT T/ }T
T{ MIN-INT MIN-INT / -> MIN-INT MIN-INT T/ }T

T{ 0 1 MOD -> 0 1 TMOD }T
T{ 1 1 MOD -> 1 1 TMOD }T
T{ 2 1 MOD -> 2 1 TMOD }T
T{ -1 1 MOD -> -1 1 TMOD }T
T{ -2 1 MOD -> -2 1 TMOD }T
T{ 0 -1 MOD -> 0 -1 TMOD }T
T{ 1 -1 MOD -> 1 -1 TMOD }T
T{ 2 -1 MOD -> 2 -1 TMOD }T
T{ -1 -1 MOD -> -1 -1 TMOD }T
T{ -2 -1 MOD -> -2 -1 TMOD }T
T{ 2 2 MOD -> 2 2 TMOD }T
T{ -1 -1 MOD -> -1 -1 TMOD }T
T{ -2 -2 MOD -> -2 -2 TMOD }T
T{ 7 3 MOD -> 7 3 TMOD }T
T{ 7 -3 MOD -> 7 -3 TMOD }T
T{ -7 3 MOD -> -7 3 TMOD }T
T{ -7 -3 MOD -> -7 -3 TMOD }T
T{ MAX-INT 1 MOD -> MAX-INT 1 TMOD }T
T{ MIN-INT 1 MOD -> MIN-INT 1 TMOD }T
T{ MAX-INT MAX-INT MOD -> MAX-INT MAX-INT TMOD }T
T{ MIN-INT MIN-INT MOD -> MIN-INT MIN-INT TMOD }T

T{ 0 2 1 */ -> 0 2 1 T*/ }T
T{ 1 2 1 */ -> 1 2 1 T*/ }T
T{ 2 2 1 */ -> 2 2 1 T*/ }T
T{ -1 2 1 */ -> -1 2 1 T*/ }T
T{ -2 2 1 */ -> -2 2 1 T*/ }T
T{ 0 2 -1 */ -> 0 2 -1 T*/ }T
T{ 1 2 -1 */ -> 1 2 -1 T*/ }T
T{ 2 2 -1 */ -> 2 2 -1 T*/ }T
T{ -1 2 -1 */ -> -1 2 -1 T*/ }T
T{ -2 2 -1 */ -> -2 2 -1 T*/ }T
T{ 2 2 2 */ -> 2 2 2 T*/ }T
T{ -1 2 -1 */ -> -1 2 -1 T*/ }T
T{ -2 2 -2 */ -> -2 2 -2 T*/ }T
T{ 7 2 3 */ -> 7 2 3 T*/ }T
T{ 7 2 -3 */ -> 7 2 -3 T*/ }T
T{ -7 2 3 */ -> -7 2 3 T*/ }T
T{ -7 2 -3 */ -> -7 2 -3 T*/ }T
T{ MAX-INT 2 MAX-INT */ -> MAX-INT 2 MAX-INT T*/ }T
T{ MIN-INT 2 MIN-INT */ -> MIN-INT 2 MIN-INT T*/ }T

T{ 0 2 1 */MOD -> 0 2 1 T*/MOD }T
T{ 1 2 1 */MOD -> 1 2 1 T*/MOD }T
T{ 2 2 1 */MOD -> 2 2 1 T*/MOD }T
T{ -1 2 1 */MOD -> -1 2 1 T*/MOD }T
T{ -2 2 1 */MOD -> -2 2 1 T*/MOD }T
T{ 0 2 -1 */MOD -> 0 2 -1 T*/MOD }T
T{ 1 2 -1 */MOD -> 1 2 -1 T*/MOD }T
T{ 2 2 -1 */MOD -> 2 2 -1 T*/MOD }T
T{ -1 2 -1 */MOD -> -1 2 -1 T*/MOD }T
T{ -2 2 -1 */MOD -> -2 2 -1 T*/MOD }T
T{ 2 2 2 */MOD -> 2 2 2 T*/MOD }T
T{ -1 2 -1 */MOD -> -1 2 -1 T*/MOD }T
T{ -2 2 -2 */MOD -> -2 2 -2 T*/MOD }T
T{ 7 2 3 */MOD -> 7 2 3 T*/MOD }T
T{ 7 2 -3 */MOD -> 7 2 -3 T*/MOD }T
T{ -7 2 3 */MOD -> -7 2 3 T*/MOD }T
T{ -7 2 -3 */MOD -> -7 2 -3 T*/MOD }T
T{ MAX-INT 2 MAX-INT */MOD -> MAX-INT 2 MAX-INT T*/MOD }T
T{ MIN-INT 2 MIN-INT */MOD -> MIN-INT 2 MIN-INT T*/MOD }T

  \ ===========================================================

TESTING HERE , @ ! CELL+ CELLS C, C@ C! CHARS 2@ 2! ALIGN
TESTING ALIGN ALIGNED +! ALLOT

HERE 1 ALLOT
HERE
CONSTANT 2NDA
CONSTANT 1STA
T{ 1STA 2NDA U< -> <TRUE> }T      \ HERE MUST GROW WITH ALLOT
T{ 1STA 1+ -> 2NDA }T         \ ... BY ONE ADDRESS UNIT

  \ MISSING TEST: NEGATIVE ALLOT

HERE 1 ,
HERE 2 ,
CONSTANT 2ND
CONSTANT 1ST
T{ 1ST 2ND U< -> <TRUE> }T         \ HERE MUST GROW WITH ALLOT
T{ 1ST CELL+ -> 2ND }T         \ ... BY ONE CELL
T{ 1ST 1 CELLS + -> 2ND }T
T{ 1ST @ 2ND @ -> 1 2 }T
T{ 5 1ST ! -> }T
T{ 1ST @ 2ND @ -> 5 2 }T
T{ 6 2ND ! -> }T
T{ 1ST @ 2ND @ -> 5 6 }T
T{ 1ST 2@ -> 6 5 }T
T{ 2 1 1ST 2! -> }T
T{ 1ST 2@ -> 2 1 }T
T{ 1S 1ST !  1ST @ -> 1S }T      \ CAN STORE CELL-WIDE VALUE

HERE 1 C,
HERE 2 C,
CONSTANT 2NDC
CONSTANT 1STC
T{ 1STC 2NDC U< -> <TRUE> }T      \ HERE MUST GROW WITH ALLOT
T{ 1STC CHAR+ -> 2NDC }T         \ ... BY ONE CHAR
T{ 1STC 1 CHARS + -> 2NDC }T
T{ 1STC C@ 2NDC C@ -> 1 2 }T
T{ 3 1STC C! -> }T
T{ 1STC C@ 2NDC C@ -> 3 2 }T
T{ 4 2NDC C! -> }T
T{ 1STC C@ 2NDC C@ -> 3 4 }T

ALIGN 1 ALLOT HERE ALIGN HERE 3 CELLS ALLOT
CONSTANT A-ADDR  CONSTANT UA-ADDR
T{ UA-ADDR ALIGNED -> A-ADDR }T
T{    1 A-ADDR C!  A-ADDR C@ ->    1 }T
T{ 1234 A-ADDR  !  A-ADDR  @ -> 1234 }T
T{ 123 456 A-ADDR 2!  A-ADDR 2@ -> 123 456 }T
T{ 2 A-ADDR CHAR+ C!  A-ADDR CHAR+ C@ -> 2 }T
T{ 3 A-ADDR CELL+ C!  A-ADDR CELL+ C@ -> 3 }T
T{ 1234 A-ADDR CELL+ !  A-ADDR CELL+ @ -> 1234 }T
T{ 123 456 A-ADDR CELL+ 2!  A-ADDR CELL+ 2@ -> 123 456 }T

: BITS ( X -- U )
   0 SWAP BEGIN  DUP
          WHILE  DUP MSB AND IF >R 1+ R> THEN 2*
          REPEAT DROP ;
  \ CHARACTERS >= 1 AU, <= SIZE OF CELL, >= 8 BITS

T{ 1 CHARS 1 < -> <FALSE> }T
T{ 1 CHARS 1 CELLS > -> <FALSE> }T
  \ TBD: HOW TO FIND NUMBER OF BITS?

  \ CELLS >= 1 AU, INTEGRAL MULTIPLE OF CHAR SIZE, >= 16 BITS

T{ 1 CELLS 1 < -> <FALSE> }T
T{ 1 CELLS 1 CHARS MOD -> 0 }T
T{ 1S BITS 10 < -> <FALSE> }T

T{ 0 1ST ! -> }T
T{ 1 1ST +! -> }T
T{ 1ST @ -> 1 }T
T{ -1 1ST +! 1ST @ -> 0 }T

  \ ===========================================================

TESTING CHAR [CHAR] [ ] BL S"

T{ BL -> 20 }T
T{ CHAR X -> 58 }T
T{ CHAR HELLO -> 48 }T
T{ : GC1 [CHAR] X ; -> }T
T{ : GC2 [CHAR] HELLO ; -> }T
T{ GC1 -> 58 }T
T{ GC2 -> 48 }T
T{ : GC3 [ GC1 ] LITERAL ; -> }T
T{ GC3 -> 58 }T
T{ : GC4 S" XY" ; -> }T
T{ GC4 SWAP DROP -> 2 }T
T{ GC4 DROP DUP C@ SWAP CHAR+ C@ -> 58 59 }T

  \ ===========================================================

TESTING ' ['] FIND EXECUTE IMMEDIATE COUNT LITERAL POSTPONE
TESTING STATE

T{ : GT1 123 ; -> }T
T{ ' GT1 EXECUTE -> 123 }T
T{ : GT2 ['] GT1 ; IMMEDIATE -> }T
T{ GT2 EXECUTE -> 123 }T
HERE 3 C, CHAR G C, CHAR T C, CHAR 1 C, CONSTANT GT1STRING
HERE 3 C, CHAR G C, CHAR T C, CHAR 2 C, CONSTANT GT2STRING
T{ GT1STRING FIND -> ' GT1 -1 }T
T{ GT2STRING FIND -> ' GT2 1 }T
  \ HOW TO SEARCH FOR NON-EXISTENT WORD?
T{ : GT3 GT2 LITERAL ; -> }T
T{ GT3 -> ' GT1 }T
T{ GT1STRING COUNT -> GT1STRING CHAR+ 3 }T

T{ : GT4 POSTPONE GT1 ; IMMEDIATE -> }T
T{ : GT5 GT4 ; -> }T
T{ GT5 -> 123 }T
T{ : GT6 345 ; IMMEDIATE -> }T
T{ : GT7 POSTPONE GT6 ; -> }T
T{ GT7 -> 345 }T

T{ : GT8 STATE @ ; IMMEDIATE -> }T
T{ GT8 -> 0 }T
T{ : GT9 GT8 LITERAL ; -> }T
T{ GT9 0= -> <FALSE> }T

  \ ===========================================================

TESTING IF ELSE THEN BEGIN WHILE REPEAT UNTIL RECURSE

T{ : GI1 IF 123 THEN ; -> }T
T{ : GI2 IF 123 ELSE 234 THEN ; -> }T
T{ 0 GI1 -> }T
T{ 1 GI1 -> 123 }T
T{ -1 GI1 -> 123 }T
T{ 0 GI2 -> 234 }T
T{ 1 GI2 -> 123 }T
T{ -1 GI1 -> 123 }T

T{ : GI3 BEGIN DUP 5 < WHILE DUP 1+ REPEAT ; -> }T
T{ 0 GI3 -> 0 1 2 3 4 5 }T
T{ 4 GI3 -> 4 5 }T
T{ 5 GI3 -> 5 }T
T{ 6 GI3 -> 6 }T

T{ : GI4 BEGIN DUP 1+ DUP 5 > UNTIL ; -> }T
T{ 3 GI4 -> 3 4 5 6 }T
T{ 5 GI4 -> 5 6 }T
T{ 6 GI4 -> 6 7 }T

T{ : GI5 BEGIN DUP 2 >
         WHILE DUP 5 <
         WHILE DUP 1+ REPEAT 123 ELSE 345 THEN ; -> }T
T{ 1 GI5 -> 1 345 }T
T{ 2 GI5 -> 2 345 }T
T{ 3 GI5 -> 3 4 5 123 }T
T{ 4 GI5 -> 4 5 123 }T
T{ 5 GI5 -> 5 123 }T

T{ : GI6 ( N -- 0,1,..N )
     DUP IF DUP >R 1- RECURSE R> THEN ; -> }T
T{ 0 GI6 -> 0 }T
T{ 1 GI6 -> 0 1 }T
T{ 2 GI6 -> 0 1 2 }T
T{ 3 GI6 -> 0 1 2 3 }T
T{ 4 GI6 -> 0 1 2 3 4 }T

  \ ===========================================================

TESTING DO LOOP +LOOP I J UNLOOP LEAVE EXIT

T{ : GD1 DO I LOOP ; -> }T
T{ 4 1 GD1 -> 1 2 3 }T
T{ 2 -1 GD1 -> -1 0 1 }T
T{ MID-UINT+1 MID-UINT GD1 -> MID-UINT }T

T{ : GD2 DO I -1 +LOOP ; -> }T
T{ 1 4 GD2 -> 4 3 2 1 }T
T{ -1 2 GD2 -> 2 1 0 -1 }T
T{ MID-UINT MID-UINT+1 GD2 -> MID-UINT+1 MID-UINT }T

T{ : GD3 DO 1 0 DO J LOOP LOOP ; -> }T
T{ 4 1 GD3 -> 1 2 3 }T
T{ 2 -1 GD3 -> -1 0 1 }T
T{ MID-UINT+1 MID-UINT GD3 -> MID-UINT }T

T{ : GD4 DO 1 0 DO J LOOP -1 +LOOP ; -> }T
T{ 1 4 GD4 -> 4 3 2 1 }T
T{ -1 2 GD4 -> 2 1 0 -1 }T
T{ MID-UINT MID-UINT+1 GD4 -> MID-UINT+1 MID-UINT }T

T{ : GD5 123 SWAP 0 DO I 4 > IF DROP 234 LEAVE THEN LOOP ;
   -> }T
T{ 1 GD5 -> 123 }T
T{ 5 GD5 -> 123 }T
T{ 6 GD5 -> 234 }T

T{
  : GD6
    \ ( PAT: T{0 0},{0 0}{1 0}{1 1},{0 0}{1 0}{1 1}{2 0}{2 1}{2 2} )
    0 SWAP 0 DO
       I 1+ 0 DO
         I J + 3 = IF I UNLOOP I UNLOOP EXIT THEN 1+
       LOOP
    LOOP ;
  -> }T

T{ 1 GD6 -> 1 }T
T{ 2 GD6 -> 3 }T
T{ 3 GD6 -> 4 1 2 }T

  \ ===========================================================

TESTING DEFINING WORDS: : ; CONSTANT VARIABLE CREATE DOES>
TESTING >BODY

T{ 123 CONSTANT X123 -> }T
T{ X123 -> 123 }T
T{ : EQU CONSTANT ; -> }T
T{ X123 EQU Y123 -> }T
T{ Y123 -> 123 }T

T{ VARIABLE V1 -> }T
T{ 123 V1 ! -> }T
T{ V1 @ -> 123 }T

T{ : NOP : POSTPONE ; ; -> }T
T{ NOP NOP1 NOP NOP2 -> }T
T{ NOP1 -> }T
T{ NOP2 -> }T

T{ : DOES1 DOES> @ 1 + ; -> }T
T{ : DOES2 DOES> @ 2 + ; -> }T
T{ CREATE CR1 -> }T
T{ CR1 -> HERE }T
T{ ' CR1 >BODY -> HERE }T
T{ 1 , -> }T
T{ CR1 @ -> 1 }T
T{ DOES1 -> }T
T{ CR1 -> 2 }T
T{ DOES2 -> }T
T{ CR1 -> 3 }T

T{ : WEIRD: CREATE DOES> 1 + DOES> 2 + ; -> }T
T{ WEIRD: W1 -> }T
T{ ' W1 >BODY -> HERE }T
T{ W1 -> HERE 1 + }T
T{ W1 -> HERE 2 + }T

  \ ===========================================================

TESTING EVALUATE

: GE1 S" 123" ; IMMEDIATE
: GE2 S" 123 1+" ; IMMEDIATE
: GE3 S" : GE4 345 ;" ;
: GE5 EVALUATE ; IMMEDIATE

T{ GE1 EVALUATE -> 123 }T \ TEST EVALUATE IN INTERP. STATE
T{ GE2 EVALUATE -> 124 }T
T{ GE3 EVALUATE -> }T
T{ GE4 -> 345 }T

T{ : GE6 GE1 GE5 ; -> }T \ TEST EVALUATE IN COMPILE STATE
T{ GE6 -> 123 }T
T{ : GE7 GE2 GE5 ; -> }T
T{ GE7 -> 124 }T

  \ ===========================================================

TESTING SOURCE >IN WORD

: GS1 S" SOURCE" 2DUP EVALUATE
       >R SWAP >R = R> R> = ;
T{ GS1 -> <TRUE> <TRUE> }T

VARIABLE SCANS
: RESCAN?  -1 SCANS +! SCANS @ IF 0 >IN ! THEN ;

T{ 2 SCANS !
345 RESCAN?
-> 345 345 }T

: GS2  5 SCANS ! S" 123 RESCAN?" EVALUATE ;
T{ GS2 -> 123 123 123 123 123 }T

: GS3 WORD COUNT SWAP C@ ;
T{ BL GS3 HELLO -> 5 CHAR H }T
T{ CHAR " GS3 GOODBYE" -> 7 CHAR G }T
T{ BL GS3
DROP -> 0 }T            \ BLANK LINE RETURN ZERO-LENGTH STRING

: GS4 SOURCE >IN ! DROP ;
T{ GS4 123 456
-> }T

  \ ===========================================================

TESTING <# # #S #> HOLD SIGN BASE >NUMBER HEX DECIMAL

: S=  \ ( ADDR1 C1 ADDR2 C2 -- T/F ) COMPARE TWO STRINGS.
   >R SWAP R@ = IF         \ MAKE SURE STRINGS HAVE SAME LENGTH
      R> ?DUP IF         \ IF NON-EMPTY STRINGS
    0 DO
       OVER C@ OVER C@ - IF 2DROP <FALSE> UNLOOP EXIT THEN
       SWAP CHAR+ SWAP CHAR+
         LOOP
      THEN
      2DROP <TRUE>         \ IF WE GET HERE, STRINGS MATCH
   ELSE
      R> DROP 2DROP <FALSE>      \ LENGTHS MISMATCH
   THEN ;

: GP1  <# 41 HOLD 42 HOLD 0 0 #> S" BA" S= ;
T{ GP1 -> <TRUE> }T

: GP2  <# -1 SIGN 0 SIGN -1 SIGN 0 0 #> S" --" S= ;
T{ GP2 -> <TRUE> }T

: GP3  <# 1 0 # # #> S" 01" S= ;
T{ GP3 -> <TRUE> }T

: GP4  <# 1 0 #S #> S" 1" S= ;
T{ GP4 -> <TRUE> }T

24 CONSTANT MAX-BASE         \ BASE 2 .. 36
: COUNT-BITS
   0 0 INVERT BEGIN DUP WHILE >R 1+ R> 2* REPEAT DROP ;
COUNT-BITS 2* CONSTANT #BITS-UD      \ NUMBER OF BITS IN UD

: GP5
   BASE @ <TRUE>
   MAX-BASE 1+ 2 DO         \ FOR EACH POSSIBLE BASE
      I BASE !            \ TBD: ASSUMES BASE WORKS
      I 0 <# #S #> S" 10" S= AND
   LOOP
   SWAP BASE ! ;
T{ GP5 -> <TRUE> }T

: GP6
   BASE @ >R  2 BASE !
   MAX-UINT MAX-UINT <# #S #>      \ MAXIMUM UD TO BINARY
   R> BASE !            \ S: C-ADDR U
   DUP #BITS-UD = SWAP
   0 DO               \ S: C-ADDR FLAG
      OVER C@ [CHAR] 1 = AND      \ ALL ONES
      >R CHAR+ R>
   LOOP SWAP DROP ;
T{ GP6 -> <TRUE> }T

: GP7
   BASE @ >R    MAX-BASE BASE !
   <TRUE>
   A 0 DO
      I 0 <# #S #>
      1 = SWAP C@ I 30 + = AND AND
   LOOP
   MAX-BASE A DO
      I 0 <# #S #>
      1 = SWAP C@ 41 I A - + = AND AND
   LOOP
   R> BASE ! ;

T{ GP7 -> <TRUE> }T

  \ >NUMBER TESTS
CREATE GN-BUF 0 C,
: GN-STRING   GN-BUF 1 ;
: GN-CONSUMED   GN-BUF CHAR+ 0 ;
: GN'      [CHAR] ' WORD CHAR+ C@ GN-BUF C!  GN-STRING ;

T{ 0 0 GN' 0' >NUMBER -> 0 0 GN-CONSUMED }T
T{ 0 0 GN' 1' >NUMBER -> 1 0 GN-CONSUMED }T
T{ 1 0 GN' 1' >NUMBER -> BASE @ 1+ 0 GN-CONSUMED }T

T{ 0 0 GN' -' >NUMBER -> 0 0 GN-STRING }T
  \ SHOULD FAIL TO CONVERT THESE

T{ 0 0 GN' +' >NUMBER -> 0 0 GN-STRING }T
T{ 0 0 GN' .' >NUMBER -> 0 0 GN-STRING }T

: >NUMBER-BASED
   BASE @ >R BASE ! >NUMBER R> BASE ! ;

T{ 0 0 GN' 2' 10 >NUMBER-BASED -> 2 0 GN-CONSUMED }T
T{ 0 0 GN' 2'  2 >NUMBER-BASED -> 0 0 GN-STRING }T
T{ 0 0 GN' F' 10 >NUMBER-BASED -> F 0 GN-CONSUMED }T
T{ 0 0 GN' G' 10 >NUMBER-BASED -> 0 0 GN-STRING }T
T{ 0 0 GN' G' MAX-BASE >NUMBER-BASED -> 10 0 GN-CONSUMED }T
T{ 0 0 GN' Z' MAX-BASE >NUMBER-BASED -> 23 0 GN-CONSUMED }T

: GN1 ( UD BASE -- UD' LEN )
  \ UD SHOULD EQUAL UD' AND LEN SHOULD BE ZERO.
  BASE @ >R BASE !
  <# #S #>
  0 0 2SWAP >NUMBER SWAP DROP \ RETURN LENGTH ONLY
  R> BASE ! ;

T{ 0 0 2 GN1 -> 0 0 0 }T
T{ MAX-UINT 0 2 GN1 -> MAX-UINT 0 0 }T
T{ MAX-UINT DUP 2 GN1 -> MAX-UINT DUP 0 }T
T{ 0 0 MAX-BASE GN1 -> 0 0 0 }T
T{ MAX-UINT 0 MAX-BASE GN1 -> MAX-UINT 0 0 }T
T{ MAX-UINT DUP MAX-BASE GN1 -> MAX-UINT DUP 0 }T

: GN2   \ ( -- 16 10 )
   BASE @ >R  HEX BASE @  DECIMAL BASE @  R> BASE ! ;
T{ GN2 -> 10 A }T

  \ ===========================================================

TESTING FILL MOVE

CREATE FBUF 00 C, 00 C, 00 C,
CREATE SBUF 12 C, 34 C, 56 C,
: SEEBUF FBUF C@  FBUF CHAR+ C@  FBUF CHAR+ CHAR+ C@ ;

T{ FBUF 0 20 FILL -> }T
T{ SEEBUF -> 00 00 00 }T

T{ FBUF 1 20 FILL -> }T
T{ SEEBUF -> 20 00 00 }T

T{ FBUF 3 20 FILL -> }T
T{ SEEBUF -> 20 20 20 }T

T{ FBUF FBUF 3 CHARS MOVE -> }T      \ BIZARRE SPECIAL CASE
T{ SEEBUF -> 20 20 20 }T

T{ SBUF FBUF 0 CHARS MOVE -> }T
T{ SEEBUF -> 20 20 20 }T

T{ SBUF FBUF 1 CHARS MOVE -> }T
T{ SEEBUF -> 12 20 20 }T

T{ SBUF FBUF 3 CHARS MOVE -> }T
T{ SEEBUF -> 12 34 56 }T

T{ FBUF FBUF CHAR+ 2 CHARS MOVE -> }T
T{ SEEBUF -> 12 12 34 }T

T{ FBUF CHAR+ FBUF 2 CHARS MOVE -> }T
T{ SEEBUF -> 12 34 34 }T

  \ ===========================================================

TESTING OUTPUT: . ." CR EMIT SPACE SPACES TYPE U.

: OUTPUT-TEST
  ." YOU SHOULD SEE THE STANDARD GRAPHIC CHARACTERS:" CR
  41 BL DO I EMIT LOOP CR
  61 41 DO I EMIT LOOP CR
  7F 61 DO I EMIT LOOP CR
  ." YOU SHOULD SEE 0-9 SEPARATED BY A SPACE:" CR
  9 1+ 0 DO I . LOOP CR
  ." YOU SHOULD SEE 0-9 (WITH NO SPACES):" CR
  [CHAR] 9 1+ [CHAR] 0 DO I 0 SPACES EMIT LOOP CR
  ." YOU SHOULD SEE A-G SEPARATED BY A SPACE:" CR
  [CHAR] G 1+ [CHAR] A DO I EMIT SPACE LOOP CR
  ." YOU SHOULD SEE 0-5 SEPARATED BY TWO SPACES:" CR
  5 1+ 0 DO I [CHAR] 0 + EMIT 2 SPACES LOOP CR
  ." YOU SHOULD SEE TWO SEPARATE LINES:" CR
  S" LINE 1" TYPE CR S" LINE 2" TYPE CR
  ." YOU SHOULD SEE THE NUMBER RANGES:" CR
  ."   SIGNED: " MIN-INT . MAX-INT . CR
  ." UNSIGNED: " 0 U. MAX-UINT U. CR ;

T{ OUTPUT-TEST -> }T

  \ ===========================================================

TESTING INPUT: ACCEPT

CREATE ABUF 50 CHARS ALLOT

: ACCEPT-TEST
   CR ." PLEASE TYPE UP TO 80 CHARACTERS:" CR
   ABUF 50 ACCEPT
   CR ." RECEIVED: " [CHAR] " EMIT
   ABUF SWAP TYPE [CHAR] " EMIT CR
;

T{ ACCEPT-TEST -> }T

  \ ===========================================================

TESTING DICTIONARY SEARCH RULES

T{ : GDX   123 ; : GDX   GDX 234 ; -> }T

T{ GDX -> 123 234 }T

CR .( End of Core word set tests) CR

  \ ===========================================================

  \ To test the ANS Forth Block word set and extension words

  \ This program was written by Steve Palmer in 2015, with
  \ contributions from others where indicated, and is in the
  \ public domain - it can be distributed and/or modified in
  \ any way but please retain this notice.

  \ This program is distributed in the hope that it will be
  \ useful, but WITHOUT ANY WARRANTY; without even the implied
  \ warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
  \ PURPOSE.

  \ The tests are not claimed to be comprehensive or correct

  \ ===========================================================
  \ Version 0.1 23 October 2015  First Version
  \ Version 0.2 15 November 2015 Updated after feedback from
  \                Gerry Jackson

  \ ===========================================================
  \ The tests are based on John Hayes test program for the core
  \ word set
  \
  \ Words tested in this file are: BLK BLOCK BUFFER EVALUATE
  \ FLUSH LOAD SAVE-BUFFERS UPDATE EMPTY-BUFFERS LIST SCR THRU
  \ REFILL SAVE-INPUT RESTORE-INPUT \
  \
  \ ===========================================================
  \ Assumptions and dependencies: - tester.fr or ttester.fs has
  \ been loaded prior to this file - errorreport.fth has been
  \ loaded prior to this file - utilities.fth has been loaded
  \ prioir to this file
  \ ===========================================================

TESTING Block word set

DECIMAL

  \ Define these constants from the system documentation
  \ provided.
  \
  \ WARNING: The contents of the test blocks will be destroyed
  \ by this test.
  \
  \ The blocks tested will be in the range
  \
  \    FIRST-TEST-BLOCK <= u < LIMIT-TEST-BLOCK
  \
  \ The tests need at least 2 test blocks in the range to
  \ complete.

20 CONSTANT FIRST-TEST-BLOCK
30 CONSTANT LIMIT-TEST-BLOCK \ one beyond the last

FIRST-TEST-BLOCK LIMIT-TEST-BLOCK U< 0= [?IF]
\?  .( Error: Test Block range not identified ) CR ABORT
[?THEN]

LIMIT-TEST-BLOCK FIRST-TEST-BLOCK - CONSTANT TEST-BLOCK-COUNT
TEST-BLOCK-COUNT 2 U< [?IF]
\? .( Error: 2 Test Blocks are required to run tests ) CR ABORT
[?THEN]

  \ ===========================================================

TESTING Random Number Utilities

  \ The block tests make extensive use of random numbers to
  \ select blocks to test and to set the contents of the block.
  \ It also makes use of a Hash code to ensure the integrity of
  \ the blocks against unexpected changes.

  \ == Memory Walk tools ==

: @++ ( a-addr -- a-addr+4 a-addr@ ) DUP CELL+ SWAP @ ;

: !++ ( x a-addr -- a-addr+4 ) TUCK ! CELL+ ;

: C@++ ( c-addr -- c-addr;char+ c-addr@ ) DUP CHAR+ SWAP C@ ;

: C!++ ( char c-addr -- c-addr+1 ) TUCK ! CHAR+ ;

  \ == Random Numbers ==
  \ Based on "Xorshift" PRNG wikipedia page
  \ reporting on results by George Marsaglia
  \ https://en.wikipedia.org/wiki/Xorshift
  \ Note: THIS IS NOT CRYPTOGRAPHIC QUALITY

: PRNG
    CREATE ( "name" -- )
        4 CELLS ALLOT
    DOES> ( -- prng )
;

: PRNG-ERROR-CODE ( prng -- errcode | 0 )
    0 4 0 DO          \ prng acc
        >R @++ R> OR  \ prng acc'
    LOOP              \ prng xORyORzORw
    NIP 0= ;          \ xORyORzORw=0

: PRNG-COPY ( src-prng dst-prng -- )
    4 CELLS MOVE ;

: PRNG-SET-SEED ( prng w z y x -- )
    4 PICK                 \ prng w z y x prng
    4 0 DO !++ LOOP DROP   \ prng
    DUP PRNG-ERROR-CODE IF \ prng
        1 OVER +!          \ prng
    THEN                   \ prng
    DROP ;                 \

BITS/CELL 64 = [?IF]
\?  : PRNG-RND ( prng -- rnd )
\?      DUP @
\?      DUP 21 LSHIFT XOR
\?      DUP 35 RSHIFT XOR
\?      DUP  4 LSHIFT XOR
\?      TUCK SWAP ! ;
[?THEN]

BITS/CELL 32 = [?IF]
\?  : PRNG-RND ( prng -- rnd )
\?      DUP @                            \ prng x
\?      DUP 11 LSHIFT XOR                \ prng t=x^(x<<11)
\?      DUP 8 RSHIFT XOR                 \ prng t'=t^(t>>8)
\?      OVER DUP CELL+ SWAP 3 CELLS MOVE \ prng t'
\?      OVER 3 CELLS + @                 \ prng t' w
\?      DUP 19 RSHIFT XOR                \ prng t' w'=w^(w>>19)
\?      XOR                              \ prng rnd=w'^t'
\?      TUCK SWAP 3 CELLS + ! ;          \ rnd
[?THEN]

BITS/CELL 16 = [?IF]
\?  .( === NOT TESTED === )
\?  : PRNG-RND ( prng -- rnd )
\?      DUP @                        \ prng x
\?      DUP 5 LSHIFT XOR             \ prng t=x^(x<<5)
\?      DUP 3 RSHIFT XOR             \ prng t'=t^(t>>3)
\?      OVER DUP CELL+ @ TUCK SWAP ! \ prng t' y
\?      DUP 1 RSHIFT XOR             \ prng t' y'=y^(y>>1)
\?      XOR                          \ prng rnd=y'^t'
\?      TUCK SWAP CELL+ ! ;          \ rnd
[?THEN]
  \ From http://b2d-f9r.blogspot.co.uk/2010/08/16-bit-xorshift-rng-now-with-more.html

[?DEF] PRNG-RND
\?  .( You need to add a Psuedo Random Number Generator )
\?  .( for your cell size: )
\?  BITS/CELL U. CR
\?  ABORT
[?THEN]

: PRNG-RANDOM ( lower upper prng -- rnd )
    >R OVER - R> PRNG-RND UM* NIP + ;
  \ PostCondition: T{ lower upper 2DUP 2>R prng PRNG-RANDOM 2R> WITHIN -> TRUE }T

PRNG BLOCK-PRNG
  \ Generated by Random.org
BLOCK-PRNG -1865266521 188896058 -2021545234 -1456609962
PRNG-SET-SEED
: BLOCK-RND    ( -- rnd )             BLOCK-PRNG PRNG-RND ;
: BLOCK-RANDOM ( lower upper -- rnd ) BLOCK-PRNG PRNG-RANDOM ;

: RND-TEST-BLOCK ( -- blk )
    FIRST-TEST-BLOCK LIMIT-TEST-BLOCK BLOCK-RANDOM ;
  \ PostCondition: T{ RND-TEST-BLOCK FIRST-TEST-BLOCK LIMIT-TEST-BLOCK WITHIN -> TRUE }T

  \ Two distinct random test blocks
: 2RND-TEST-BLOCKS ( -- blk1 blk2 )
    RND-TEST-BLOCK BEGIN  \ blk1
        RND-TEST-BLOCK    \ blk1 blk2
        2DUP =            \ blk1 blk2 blk1==blk2
    WHILE                 \ blk1 blk1
        DROP              \ blk1
    REPEAT ;              \ blk1 blk2
  \ PostCondition: T{ 2RND-TEST-BLOCKS = -> FALSE }T

  \ first random test block in a sequence of length u
: RND-TEST-BLOCK-SEQ ( u -- blks )
    FIRST-TEST-BLOCK LIMIT-TEST-BLOCK ROT 1- - BLOCK-RANDOM ;

  \ I'm not sure if this algorithm is correct if " 1 CHARS 1 <> ".
: ELF-HASH-ACCUMULATE ( hash c-addr u -- hash )
  >R SWAP R> 0 DO             \ c-addr h
    4 LSHIFT                  \ c-addr h<<=4
    SWAP C@++ ROT +           \ c-addr' h+=*s
    DUP [ HEX ] F0000000
    [ DECIMAL ] AND           \ c-addr' h high=h&0xF0000000
    DUP IF                    \ c-addr' h high
      DUP >R 24 RSHIFT XOR R> \ c-addr' h^=high>>24 high
    THEN                      \ c-addr' h high
    INVERT AND                \ c-addr' h&=~high
  LOOP NIP ;

: ELF-HASH ( c-addr u -- hash )
    0 ROT ROT ELF-HASH-ACCUMULATE ;

  \ ===========================================================

TESTING BLOCK ( read-only mode )

  \ BLOCK signature
T{ RND-TEST-BLOCK BLOCK DUP ALIGNED = -> TRUE }T

  \ BLOCK accepts all blocks in the test range
: BLOCK-ALL ( blk2 blk1 -- )
    DO
        I BLOCK DROP
    LOOP ;
T{ LIMIT-TEST-BLOCK FIRST-TEST-BLOCK BLOCK-ALL -> }T

  \ BLOCK twice on same block returns the same value
T{ RND-TEST-BLOCK DUP BLOCK SWAP BLOCK = -> TRUE }T

  \ BLOCK twice on distinct block numbers
  \ may or may not return the same value!
  \ Nothing to test

  \ ===========================================================

TESTING BUFFER ( read-only mode )

  \ Although it is not in the spirit of the specification,
  \ a compliant definition of BUFFER would be
  \ : BUFFER BLOCK ;
  \ So we can only repeat the tests for BLOCK ...

  \ BUFFER signature
T{ RND-TEST-BLOCK BUFFER DUP ALIGNED = -> TRUE }T

  \ BUFFER accepts all blocks in the test range
: BUFFER-ALL ( blk2 blk1 -- )
    DO
        I BUFFER DROP
    LOOP ;
T{ LIMIT-TEST-BLOCK FIRST-TEST-BLOCK BUFFER-ALL -> }T

  \ BUFFER twice on the same block returns the same value
T{ RND-TEST-BLOCK DUP BUFFER SWAP BUFFER = -> TRUE }T

  \ BUFFER twice on distinct block numbers
  \ may or may not return the same value!
  \ Nothing to test

  \ Combinations with BUFFER
T{ RND-TEST-BLOCK DUP BLOCK SWAP BUFFER = -> TRUE }T
T{ RND-TEST-BLOCK DUP BUFFER SWAP BLOCK = -> TRUE }T

  \ ===========================================================

TESTING Read and Write access with UPDATE and FLUSH

  \ Ideally, we'd like to be able to test the persistence
  \ across power cycles of the writes, but we can't do that in
  \ a simple test.  The tests below could be fooled by a large
  \ buffers store and a tricky `FLUSH` but what else are you
  \ going to do?

  \ Signatures
T{ RND-TEST-BLOCK BLOCK DROP UPDATE -> }T
T{ FLUSH -> }T

: BLANK-BUFFER ( blk -- blk-addr )
    BUFFER DUP 1024 BL FILL ;

  \ Test R/W of a Simple Blank Random Block
T{ RND-TEST-BLOCK                 \ blk
   DUP BLANK-BUFFER               \ blk blk-addr1
   1024 ELF-HASH                  \ blk hash
   UPDATE FLUSH                   \ blk hash
   SWAP BLOCK                     \ hash blk-addr2
   1024 ELF-HASH = -> TRUE }T

  \ Boundary Test: Modify first character
T{ RND-TEST-BLOCK                  \ blk
   DUP BLANK-BUFFER                \ blk blk-addr1
   CHAR \ OVER C!                  \ blk blk-addr1
   1024 ELF-HASH                   \ blk hash
   UPDATE FLUSH                    \ blk hash
   SWAP BLOCK                      \ hash blk-addr2
   1024 ELF-HASH = -> TRUE }T

  \ Boundary Test: Modify last character
T{ RND-TEST-BLOCK                  \ blk
   DUP BLANK-BUFFER                \ blk blk-addr1
   CHAR \ OVER 1023 CHARS + C!     \ blk blk-addr1
   1024 ELF-HASH                   \ blk hash
   UPDATE FLUSH                    \ blk hash
   SWAP BLOCK                      \ hash blk-addr2
   1024 ELF-HASH = -> TRUE }T

  \ Boundary Test: First and Last (and all other) blocks in the
  \ test range.

1024 8 * BITS/CELL / CONSTANT CELLS/BLOCK

: PREPARE-RND-BLOCK ( hash blk -- hash' )
  BUFFER DUP                 \ hash blk-addr blk-addr
  CELLS/BLOCK 0 DO           \ hash blk-addr blk-addr[i]
    BLOCK-RND OVER ! CELL+   \ hash blk-addr blk-addr[i+1]
  LOOP DROP                  \ hash blk-addr
  1024 ELF-HASH-ACCUMULATE ; \ hash'

: WRITE-RND-BLOCKS-WITH-HASH ( blk2 blk1 -- hash )
    0 ROT ROT DO                   \ hash
        I PREPARE-RND-BLOCK UPDATE \ hash'
    LOOP ;                         \ hash'

: READ-BLOCKS-AND-HASH ( blk2 blk1 -- hash )
    0 ROT ROT DO                         \ hash(i)
        I BLOCK 1024 ELF-HASH-ACCUMULATE \ hash(i+1)
    LOOP ;                               \ hash

T{ LIMIT-TEST-BLOCK FIRST-TEST-BLOCK
   WRITE-RND-BLOCKS-WITH-HASH FLUSH
   LIMIT-TEST-BLOCK FIRST-TEST-BLOCK READ-BLOCKS-AND-HASH =
   -> TRUE }T

: TUF1 ( xt blk -- hash )
    DUP BLANK-BUFFER               \ xt blk blk-addr1
    1024 ELF-HASH                  \ xt blk hash
    ROT EXECUTE                    \ blk hash
    SWAP BLOCK                     \ hash blk-addr2
    1024 ELF-HASH = ;              \ TRUE

  \ Double UPDATE make no difference
: TUF1-1 ( -- ) UPDATE UPDATE FLUSH ;
T{ ' TUF1-1 RND-TEST-BLOCK TUF1 -> TRUE }T

  \ Double FLUSH make no difference
: TUF1-2 ( -- ) UPDATE FLUSH FLUSH ;
T{ ' TUF1-2 RND-TEST-BLOCK TUF1 -> TRUE }T

  \ FLUSH only saves UPDATEd buffers
T{ RND-TEST-BLOCK                      \ blk
   0 OVER PREPARE-RND-BLOCK            \ blk hash
   UPDATE FLUSH                        \ blk hash
   OVER 0 SWAP PREPARE-RND-BLOCK DROP  \ blk hash
   FLUSH ( with no preliminary UPDATE) \ blk hash
   SWAP BLOCK 1024 ELF-HASH = -> TRUE }T

  \ `UPDATE` only marks the current block buffer.  This test
  \ needs at least 2 distinct buffers, though this is not a
  \ requirement of the language specification.  If 2 distinct
  \ buffers are not returned, then the tests quits with a
  \ trivial Pass.

: TUF2
  \ ( xt blk1 blk2 -- hash1'' hash2'' hash1' hash2' hash1 hash2 )
  OVER BUFFER OVER BUFFER = IF \ test needs 2 distinct buffers
    2DROP DROP 0 0 0 0 0 0     \ Dummy result
  ELSE
    OVER 0 SWAP
    PREPARE-RND-BLOCK
    UPDATE                     \ xt blk1 blk2 hash1
    OVER 0 SWAP
    PREPARE-RND-BLOCK
    UPDATE                     \ xt blk1 blk2 hash1 hash2
    2>R                        \ xt blk1 blk2
    FLUSH                      \ xt blk1 blk2
    OVER 0 SWAP
    PREPARE-RND-BLOCK          \ xt blk1 blk2 hash1'
    OVER 0 SWAP
    PREPARE-RND-BLOCK          \ xt blk1 blk2 hash1' hash2'
    2>R                        \ xt blk1 blk2
    ROT EXECUTE                \ blk1 blk2
    FLUSH                      \ blk1 blk2
    SWAP BLOCK 1024 ELF-HASH   \ blk2 hash1''
    SWAP BLOCK 1024 ELF-HASH   \ hash1'' hash2''
    2R> 2R> \ hash1'' hash2'' hash1' hash2' hash1 hash2
  THEN ;

: 2= ( x1 x2 x3 x4 -- flag )
    ROT = ROT ROT = AND ;

: TUF2-0 ( blk1 blk2 -- blk1 blk2 ) ;   \ no updates

T{ ' TUF2-0 2RND-TEST-BLOCKS TUF2 \ run test procedure
   2SWAP 2DROP 2= -> TRUE }T      \ compare expected and actual

: TUF2-1 ( blk1 blk2 -- blk1 blk2 )    \ update blk1 only
    OVER BLOCK DROP UPDATE ;
T{ ' TUF2-1 2RND-TEST-BLOCKS TUF2       \ run test procedure
   SWAP DROP SWAP DROP 2= -> TRUE }T

: TUF2-2 ( blk1 blk2 -- blk1 blk2 )    \ update blk2 only
    DUP BUFFER DROP UPDATE ;
T{ ' TUF2-2 2RND-TEST-BLOCKS TUF2       \ run test procedure
   DROP ROT DROP SWAP 2= -> TRUE }T

: TUF2-3 ( blk1 blk2 -- blk1 blk2 )    \ update blk1 and blk2
    TUF2-1 TUF2-2 ;
T{ ' TUF2-3 2RND-TEST-BLOCKS TUF2       \ run test procedure
   2DROP 2= -> TRUE }T

  \ FLUSH and then UPDATE is ambiguous and untestable

  \ ===========================================================

TESTING SAVE-BUFFERS

  \ In principle, all the tests above can be repeated with
  \ `SAVE-BUFFERS` instead of `FLUSH`.  However, only the full
  \ random test is repeated...

T{ LIMIT-TEST-BLOCK FIRST-TEST-BLOCK WRITE-RND-BLOCKS-WITH-HASH
   SAVE-BUFFERS
   LIMIT-TEST-BLOCK FIRST-TEST-BLOCK READ-BLOCKS-AND-HASH =
   -> TRUE }T

  \ FLUSH and then SAVE-BUFFERS is harmless but undetectable
  \ SAVE-BUFFERS and then FLUSH is undetectable

  \ Unlike FLUSH, SAVE-BUFFERS then BUFFER/BLOCK
  \ returns the original buffer address
T{ RND-TEST-BLOCK DUP BLANK-BUFFER
   SAVE-BUFFERS        SWAP BUFFER = -> TRUE }T
T{ RND-TEST-BLOCK DUP BLANK-BUFFER
   UPDATE SAVE-BUFFERS SWAP BUFFER = -> TRUE }T
T{ RND-TEST-BLOCK DUP BLANK-BUFFER
   SAVE-BUFFERS        SWAP BLOCK  = -> TRUE }T
T{ RND-TEST-BLOCK DUP BLANK-BUFFER
   UPDATE SAVE-BUFFERS SWAP BLOCK  = -> TRUE }T

  \ ===========================================================

TESTING BLK

  \ Signature
T{ BLK DUP ALIGNED = -> TRUE }T

  \ None of the words considered so far effect BLK
T{ BLK @ RND-TEST-BLOCK BUFFER DROP BLK @ = -> TRUE }T
T{ BLK @ RND-TEST-BLOCK BLOCK  DROP BLK @ = -> TRUE }T
T{ BLK @ UPDATE                     BLK @ = -> TRUE }T

T{ BLK @ FLUSH        BLK @ = -> TRUE }T
T{ BLK @ SAVE-BUFFERS BLK @ = -> TRUE }T

  \ ===========================================================

TESTING LOAD and EVALUATE

  \ Signature: n LOAD --> blank screen
T{ RND-TEST-BLOCK DUP BLANK-BUFFER DROP UPDATE FLUSH LOAD -> }T

T{ BLK @ RND-TEST-BLOCK DUP BLANK-BUFFER DROP
   UPDATE FLUSH LOAD BLK @ = -> TRUE }T

: WRITE-BLOCK ( blk c-addr u -- )
    ROT BLANK-BUFFER SWAP CHARS MOVE UPDATE FLUSH ;

  \ blk: u; blk LOAD
: TL1 ( u blk -- )
    SWAP 0 <# #S #> WRITE-BLOCK ;
T{ BLOCK-RND RND-TEST-BLOCK 2DUP TL1 LOAD = -> TRUE }T

  \ Boundary Test: FIRST-TEST-BLOCK
T{ BLOCK-RND FIRST-TEST-BLOCK 2DUP TL1 LOAD = -> TRUE }T

  \ Boundary Test: LIMIT-TEST-BLOCK-1
T{ BLOCK-RND LIMIT-TEST-BLOCK 1- 2DUP TL1 LOAD = -> TRUE }T

: WRITE-AT-END-OF-BLOCK ( blk c-addr u -- )
    ROT BLANK-BUFFER
    OVER 1024 SWAP - CHARS +
    SWAP CHARS MOVE UPDATE FLUSH ;

  \ Boundary Test: End of Buffer
: TL2 ( u blk -- )
    SWAP 0 <# #S #> WRITE-AT-END-OF-BLOCK ;
T{ BLOCK-RND RND-TEST-BLOCK 2DUP TL2 LOAD = -> TRUE }T

  \ LOAD updates BLK
  \ u: "BLK @"; u LOAD
: TL3 ( blk -- )
    S" BLK @" WRITE-BLOCK ;
T{ RND-TEST-BLOCK DUP TL3 DUP LOAD = -> TRUE }T

  \ EVALUATE resets BLK
  \ u: "EVALUATE-BLK@"; u LOAD
: EVALUATE-BLK@ ( -- BLK@ )
    S" BLK @" EVALUATE ;
: TL4 ( blk -- )
    S" EVALUATE-BLK@" WRITE-BLOCK ;
T{ RND-TEST-BLOCK DUP TL4 LOAD -> 0 }T

  \ EVALUTE can nest with LOAD
  \ u: "BLK @"; S" u LOAD" EVALUATE
: TL5 ( blk -- c-addr u )
    0 <#                       \ blk 0
         [CHAR] D HOLD
         [CHAR] A HOLD
         [CHAR] O HOLD
         [CHAR] L HOLD
         BL HOLD
    #S #> ;                    \ c-addr u
T{ RND-TEST-BLOCK DUP TL3 DUP TL5 EVALUATE = -> TRUE }T

  \ Nested LOADs
  \ u2: "BLK @"; u1: "LOAD u2"; u1 LOAD
: TL6 ( blk1 blk2 -- )
    DUP TL3                    \ blk1 blk2
    TL5 WRITE-BLOCK ;
T{ 2RND-TEST-BLOCKS 2DUP TL6 SWAP LOAD = -> TRUE }T

  \ `LOAD` changes the currect block that is effected by
  \ `UPDATE` This test needs at least 2 distinct buffers,
  \ though this is not a requirement of the language
  \ specification.  If 2 distinct buffers are not returned,
  \ then the tests quits with a trivial Pass.

: TL7 ( blk1 blk2 -- u1 u2 rnd2 blk2-addr rnd1' rnd1 )
  OVER BUFFER OVER BUFFER = IF \ test needs 2 distinct buffers
    2DROP 0 0 0 0 0 0          \ Dummy result
  ELSE
    OVER BLOCK-RND DUP ROT TL1 >R   \ blk1 blk2
    DUP S" SOURCE DROP" WRITE-BLOCK \ blk1 blk2
    \ change blk1 to a new rnd, but don't UPDATE
    OVER BLANK-BUFFER        \ blk1 blk2 blk1-addr
    BLOCK-RND DUP >R         \ blk1 blk2 blk1-addr rnd1'
    0 <# #S #>               \ blk1 blk2 blk1-addr c-addr u
    ROT SWAP CHARS MOVE      \ blk1 blk2
    \ Now LOAD blk2
    DUP LOAD DUP >R          \ blk1 blk2 blk2-addr
    \ Write a new blk2
    DUP 1024 BL FILL         \ blk1 blk2 blk2-addr
    BLOCK-RND DUP >R         \ blk1 blk2 blk2-addr rnd2
    0 <# #S #>               \ blk1 blk2 blk2-addr c-addr u
    ROT SWAP CHARS MOVE      \ blk1 blk2
    \ The following UPDATE should refer to the LOADed blk2, not blk1
    UPDATE FLUSH             \ blk1 blk2
    \ Finally, load both blocks then collect all results
    LOAD SWAP LOAD           \ u2 u1
    R> R> R> R>              \ u2 u1 rnd2 blk2-addr rnd1' rnd1
  THEN ;

T{ 2RND-TEST-BLOCKS TL7                 \ run test procedure
   SWAP DROP SWAP DROP                  \ u2 u1 rnd2 rnd1
   2= -> TRUE }T

  \ I would expect LOAD to work on the contents of the buffer
  \ cache and not the block device, but the specification
  \ doesn't say.  Similarly, I would not expect `LOAD` to
  \ `FLUSH` the buffer cache, but the specification doesn't say
  \ so.

  \ ===========================================================

TESTING LIST and SCR

  \ Signatures
T{ SCR DUP ALIGNED = -> TRUE }T
  \ LIST signature is test implicitly in the following tests...

: TLS1 ( blk -- )
    S" Should show a (mostly) blank screen" WRITE-BLOCK ;
T{ RND-TEST-BLOCK DUP TLS1 DUP LIST SCR @ = -> TRUE }T

  \ Boundary Test: FIRST-TEST-BLOCK
: TLS2 ( blk -- )
    S" List of the First test block" WRITE-BLOCK ;
T{ FIRST-TEST-BLOCK DUP TLS2 LIST -> }T

  \ Boundary Test: LIMIT-TEST-BLOCK
: TLS3 ( blk -- )
    S" List of the Last test block" WRITE-BLOCK ;
T{ LIMIT-TEST-BLOCK 1- DUP TLS3 LIST -> }T

  \ Boundary Test: End of Screen
: TLS4 ( blk -- )
    S" End of Screen" WRITE-AT-END-OF-BLOCK ;
T{ RND-TEST-BLOCK DUP TLS4 LIST -> }T

  \ `BLOCK`, `BUFFER`, `UPDATE` et al don't change SCR.

: TLS5 ( blk -- )
  S" Should show another (mostly) blank screen" WRITE-BLOCK ;

  \ The first test below sets the scenario for the subsequent
  \ tests `BLK` is unchanged by `LIST`.

T{ BLK @ RND-TEST-BLOCK DUP TLS5 LIST                BLK @ = ->
   TRUE }T
  \ SCR is unchanged by Earlier words

T{ SCR @ FLUSH                                       SCR @ = ->
    TRUE }T
T{ SCR @ FLUSH DUP 1+ BUFFER DROP                    SCR @ = ->
    TRUE }T
T{ SCR @ FLUSH DUP 1+ BLOCK DROP                     SCR @ = ->
    TRUE }T
T{ SCR @ FLUSH DUP 1+ BLOCK DROP UPDATE              SCR @ = ->
    TRUE }T
T{ SCR @ FLUSH DUP 1+ BLOCK DROP UPDATE SAVE-BUFFERS SCR @ = ->
    TRUE }T

: TLS6 ( blk -- )
    S" SCR @" WRITE-BLOCK ;
T{ SCR @ RND-TEST-BLOCK DUP TLS6 LOAD SCR @ OVER 2= -> TRUE }T

  \ ===========================================================

TESTING EMPTY-BUFFERS

T{ EMPTY-BUFFERS -> }T
T{ BLK @ EMPTY-BUFFERS BLK @ = -> TRUE }T
T{ SCR @ EMPTY-BUFFERS SCR @ = -> TRUE }T

  \ Test R/W, but discarded changes with EMPTY-BUFFERS
T{ RND-TEST-BLOCK                    \ blk
   DUP BLANK-BUFFER                  \ blk blk-addr1
   1024 ELF-HASH                     \ blk hash
   UPDATE FLUSH                      \ blk hash
   OVER BLOCK CHAR \ SWAP C!         \ blk hash
   UPDATE EMPTY-BUFFERS FLUSH        \ blk hash
   SWAP BLOCK                        \ hash blk-addr2
   1024 ELF-HASH = -> TRUE }T

  \ EMPTY-BUFFERS discards all buffers
: TUF2-EB ( blk1 blk2 -- blk1 blk2 )
    TUF2-1 TUF2-2 EMPTY-BUFFERS ;  \ c.f. TUF2-3
T{ ' TUF2-EB 2RND-TEST-BLOCKS TUF2
   2SWAP 2DROP 2= -> TRUE }T

  \ FLUSH and then EMPTY-BUFFERS is acceptable but untestable
  \ EMPTY-BUFFERS and then UPDATE is ambiguous and untestable

  \ ===========================================================

TESTING >IN manipulation from a block source

: TIN ( blk -- )
    S" 1 8 >IN +!     2        3" WRITE-BLOCK ;
T{ RND-TEST-BLOCK DUP TIN LOAD -> 1 3 }T

  \ ===========================================================

TESTING \, SAVE-INPUT, RESTORE-INPUT and REFILL
TESTING from a block source

  \ Try to determine the number of charaters per line
  \ Assumes an even number of characters per line
: | ( u -- u-2 ) 2 - ;
: C/L-CALC ( blk -- c/l )
    DUP BLANK-BUFFER           \ blk blk-addr
    [CHAR] \ OVER C!           \ blk blk-addr  blk:"\"
    511 0 DO                   \ blk c-addr[i]
        CHAR+ CHAR+
        [CHAR] | OVER C!       \ blk c-addr[i+1]
    LOOP DROP                  \ blk   blk:"\ | | | | ... |"
    UPDATE SAVE-BUFFERS FLUSH  \ blk
    1024 SWAP LOAD ;           \ c/l
[?DEF] C/L
[?ELSE]
\? .( Given Characters per Line: ) C/L U. CR
[?ELSE]
\? RND-TEST-BLOCK C/L-CALC CONSTANT C/L
\? C/L 1024 U< [?IF]
\? .( Calculated Characters per Line: ) C/L U. CR
[?THEN]

: WRITE-BLOCK-LINE ( lin-addr[i] c-addr u -- lin-addr[i+1] )
    2>R DUP C/L CHARS + SWAP 2R> ROT SWAP MOVE ;

  \ Discards to the end of the line
: TCSIRIR1 ( blk -- )
    BLANK-BUFFER
    C/L 1024 U< IF
        S" 2222 \ 3333" WRITE-BLOCK-LINE
        S" 4444"        WRITE-BLOCK-LINE
    THEN
    DROP UPDATE SAVE-BUFFERS ;
T{ RND-TEST-BLOCK DUP TCSIRIR1 LOAD -> 2222 4444 }T

VARIABLE T-CNT 0 T-CNT !

: MARK ( "<char>" -- ) \ Use between <# and #>
    CHAR HOLD ; IMMEDIATE

: ?EXECUTE ( xt f -- )
    IF EXECUTE ELSE DROP THEN ;

  \ SAVE-INPUT and RESTORE-INPUT within a single block

: TCSIRIR2-EXPECTED S" EDCBCBA" ;
  \ Remember that the string comes out backwards

: TCSIRIR2 ( blk -- )
    C/L 1024 U< IF
        BLANK-BUFFER
        S" 0 T-CNT !"                   WRITE-BLOCK-LINE
        S" <# MARK A SAVE-INPUT MARK B" WRITE-BLOCK-LINE
        S" 1 T-CNT +! MARK C ' RESTORE-INPUT "
        s" T-CNT @ 2 < ?EXECUTE MARK D" s+
                                        WRITE-BLOCK-LINE
        S" MARK E 0 0 #>"               WRITE-BLOCK-LINE
        UPDATE SAVE-BUFFERS DROP
    ELSE
        S" 0 TCSIRIR2-EXPECTED"         WRITE-BLOCK
    THEN ;
T{ RND-TEST-BLOCK DUP TCSIRIR2 LOAD TCSIRIR2-EXPECTED S=
   -> 0 TRUE }T

  \ REFILL across 2 blocks
: TCSIRIR3 ( blks -- )
    DUP S" 1 2 3 REFILL 4 5 6" WRITE-BLOCK
    1+  S" 10 11 12"           WRITE-BLOCK ;
T{ 2 RND-TEST-BLOCK-SEQ DUP TCSIRIR3 LOAD
   -> 1 2 3 -1 10 11 12 }T

  \ SAVE-INPUT and RESTORE-INPUT across 2 blocks

: TCSIRIR4-EXPECTED S" HGF1ECBF1ECBA" ;
  \ Remember that the string comes out backwards

: TCSIRIR4 ( blks -- )
    C/L 1024 U< IF
        DUP BLANK-BUFFER
        S" 0 T-CNT !"                   WRITE-BLOCK-LINE
        S" <# MARK A SAVE-INPUT MARK B" WRITE-BLOCK-LINE
        S" MARK C REFILL MARK D"        WRITE-BLOCK-LINE
        DROP UPDATE 1+ BLANK-BUFFER
        S" MARK E ABS CHAR 0 + HOLD"    WRITE-BLOCK-LINE
        S" 1 T-CNT +! MARK F ' RESTORE-INPUT "
        S" T-CNT @ 2 < ?EXECUTE MARK G" s+
                                        WRITE-BLOCK-LINE
        S" MARK H 0 0 #>"               WRITE-BLOCK-LINE
        DROP UPDATE SAVE-BUFFERS
    ELSE
        S" 0 TCSIRIR4-EXPECTED"         WRITE-BLOCK
    THEN ;
T{ 2 RND-TEST-BLOCK-SEQ DUP TCSIRIR4 LOAD TCSIRIR4-EXPECTED S=
   -> 0 TRUE }T

  \ ===========================================================

TESTING THRU

: TT1 ( blks -- )
    DUP S" BLK" WRITE-BLOCK
    1+  S" @"   WRITE-BLOCK ;
T{ 2 RND-TEST-BLOCK-SEQ DUP TT1 DUP DUP 1+ THRU 1- = -> TRUE }T

BLOCK-ERRORS SET-ERROR-COUNT

CR .( End of Block word tests) CR

  \ ===========================================================

  \ To test the ANS Forth Core Extension word set

  \ This program was written by Gerry Jackson in 2006, with
  \ contributions from others where indicated, and is in the
  \ public domain - it can be distributed and/or modified in
  \ any way but please retain this notice.

  \ This program is distributed in the hope that it will be
  \ useful, but WITHOUT ANY WARRANTY; without even the implied
  \ warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
  \ PURPOSE.

  \ The tests are not claimed to be comprehensive or correct

  \ ===========================================================
  \ Version 0.13 28 October 2015
  \              Replace <FALSE> and <TRUE> with FALSE and TRUE to avoid
  \              dependence on Core tests
  \              Moved SAVE-INPUT and RESTORE-INPUT tests in a file to filetest.fth
  \              Use of 2VARIABLE (from optional wordset) replaced with CREATE.
  \              Minor lower to upper case conversions.
  \              Calls to COMPARE replaced by S= (in utilities.fth) to avoid use
  \              of a word from an optional word set.
  \              UNUSED tests revised as UNUSED UNUSED = may return FALSE when an
  \              implementation has the data stack sharing unused dataspace.
  \              Double number input dependency removed from the HOLDS tests.
  \              Minor case sensitivities removed in definition names.
  \         0.11 25 April 2015
  \              Added tests for PARSE-NAME HOLDS BUFFER:
  \              S\" tests added
  \              DEFER IS ACTION-OF DEFER! DEFER@ tests added
  \              Empty CASE statement test added
  \              [COMPILE] tests removed because it is obsolescent in Forth 2012
  \         0.10 1 August 2014
  \             Added tests contributed by James Bowman for:
  \                <> U> 0<> 0> NIP TUCK ROLL PICK 2>R 2R@ 2R>
  \                HEX WITHIN UNUSED AGAIN MARKER
  \             Added tests for:
  \                .R U.R ERASE PAD REFILL SOURCE-ID
  \             Removed ABORT from NeverExecuted to enable Win32
  \             to continue after failure of RESTORE-INPUT.
  \             Removed max-intx which is no longer used.
  \         0.7 6 June 2012 Extra CASE test added
  \         0.6 1 April 2012 Tests placed in the public domain.
  \             SAVE-INPUT & RESTORE-INPUT tests, position
  \             of T{ moved so that tests work with ttester.fs
  \             CONVERT test deleted - obsolete word removed from Forth 200X
  \             IMMEDIATE VALUEs tested
  \             RECURSE with :NONAME tested
  \             PARSE and .( tested
  \             Parsing behaviour of C" added
  \         0.5 14 September 2011 Removed the double [ELSE] from the
  \             initial SAVE-INPUT & RESTORE-INPUT test
  \         0.4 30 November 2009  max-int replaced with max-intx to
  \             avoid redefinition warnings.
  \         0.3  6 March 2009 { and } replaced with T{ and }T
  \                           CONVERT test now independent of cell size
  \         0.2  20 April 2007 ANS Forth words changed to upper case
  \                            Tests qd3 to qd6 by Reinhold Straub
  \         0.1  Oct 2006 First version released
  \ ===========================================================
  \ The tests are based on John Hayes test program for the core word set

  \ Words tested in this file are:
  \     .( .R 0<> 0> 2>R 2R> 2R@ :NONAME <> ?DO AGAIN C" CASE COMPILE, ENDCASE
  \     ENDOF ERASE FALSE HEX MARKER NIP OF PAD PARSE PICK REFILL
  \     RESTORE-INPUT ROLL SAVE-INPUT SOURCE-ID TO TRUE TUCK U.R U> UNUSED
  \     VALUE WITHIN [COMPILE]

  \ Words not tested or partially tested:
  \     \ because it has been extensively used already and is, hence, unnecessary
  \     REFILL and SOURCE-ID from the user input device which are not possible
  \     when testing from a file such as this one
  \     UNUSED (partially tested) as the value returned is system dependent
  \     Obsolescent words #TIB CONVERT EXPECT QUERY SPAN TIB as they have been
  \     removed from the Forth 2012 standard

  \ Results from words that output to the user output device have to visually
  \ checked for correctness. These are .R U.R .(

  \ ===========================================================
  \ Assumptions & dependencies:
  \     - tester.fr (or ttester.fs), errorreport.fth and utilities.fth have been
  \       included prior to this file
  \     - the Core word set available
  \ ===========================================================

TESTING Core Extension words

DECIMAL

TESTING TRUE FALSE

T{ TRUE  -> 0 INVERT }T
T{ FALSE -> 0 }T

  \ ===========================================================

TESTING <> U>   (contributed by James Bowman)

T{ 0 0 <> -> FALSE }T
T{ 1 1 <> -> FALSE }T
T{ -1 -1 <> -> FALSE }T
T{ 1 0 <> -> TRUE }T
T{ -1 0 <> -> TRUE }T
T{ 0 1 <> -> TRUE }T
T{ 0 -1 <> -> TRUE }T

T{ 0 1 U> -> FALSE }T
T{ 1 2 U> -> FALSE }T
T{ 0 MID-UINT U> -> FALSE }T
T{ 0 MAX-UINT U> -> FALSE }T
T{ MID-UINT MAX-UINT U> -> FALSE }T
T{ 0 0 U> -> FALSE }T
T{ 1 1 U> -> FALSE }T
T{ 1 0 U> -> TRUE }T
T{ 2 1 U> -> TRUE }T
T{ MID-UINT 0 U> -> TRUE }T
T{ MAX-UINT 0 U> -> TRUE }T
T{ MAX-UINT MID-UINT U> -> TRUE }T

  \ ===========================================================

TESTING 0<> 0>   (contributed by James Bowman)

T{ 0 0<> -> FALSE }T
T{ 1 0<> -> TRUE }T
T{ 2 0<> -> TRUE }T
T{ -1 0<> -> TRUE }T
T{ MAX-UINT 0<> -> TRUE }T
T{ MIN-INT 0<> -> TRUE }T
T{ MAX-INT 0<> -> TRUE }T

T{ 0 0> -> FALSE }T
T{ -1 0> -> FALSE }T
T{ MIN-INT 0> -> FALSE }T
T{ 1 0> -> TRUE }T
T{ MAX-INT 0> -> TRUE }T

  \ ===========================================================

TESTING NIP TUCK ROLL PICK   (contributed by James Bowman)

T{ 1 2 NIP -> 2 }T
T{ 1 2 3 NIP -> 1 3 }T

T{ 1 2 TUCK -> 2 1 2 }T
T{ 1 2 3 TUCK -> 1 3 2 3 }T

T{ : RO5 100 200 300 400 500 ; -> }T
T{ RO5 3 ROLL -> 100 300 400 500 200 }T
T{ RO5 2 ROLL -> RO5 ROT }T
T{ RO5 1 ROLL -> RO5 SWAP }T
T{ RO5 0 ROLL -> RO5 }T

T{ RO5 2 PICK -> 100 200 300 400 500 300 }T
T{ RO5 1 PICK -> RO5 OVER }T
T{ RO5 0 PICK -> RO5 DUP }T

  \ ===========================================================

TESTING 2>R 2R@ 2R>   (contributed by James Bowman)

T{ : RR0 2>R 100 R> R> ; -> }T
T{ 300 400 RR0 -> 100 400 300 }T
T{ 200 300 400 RR0 -> 200 100 400 300 }T

T{ : RR1 2>R 100 2R@ R> R> ; -> }T
T{ 300 400 RR1 -> 100 300 400 400 300 }T
T{ 200 300 400 RR1 -> 200 100 300 400 400 300 }T

T{ : RR2 2>R 100 2R> ; -> }T
T{ 300 400 RR2 -> 100 300 400 }T
T{ 200 300 400 RR2 -> 200 100 300 400 }T

  \ ===========================================================

TESTING HEX   (contributed by James Bowman)

T{ BASE @ HEX BASE @ DECIMAL BASE @ - SWAP BASE ! -> 6 }T

  \ ===========================================================

TESTING WITHIN   (contributed by James Bowman)

T{ 0 0 0 WITHIN -> FALSE }T
T{ 0 0 MID-UINT WITHIN -> TRUE }T
T{ 0 0 MID-UINT+1 WITHIN -> TRUE }T
T{ 0 0 MAX-UINT WITHIN -> TRUE }T
T{ 0 MID-UINT 0 WITHIN -> FALSE }T
T{ 0 MID-UINT MID-UINT WITHIN -> FALSE }T
T{ 0 MID-UINT MID-UINT+1 WITHIN -> FALSE }T
T{ 0 MID-UINT MAX-UINT WITHIN -> FALSE }T
T{ 0 MID-UINT+1 0 WITHIN -> FALSE }T
T{ 0 MID-UINT+1 MID-UINT WITHIN -> TRUE }T
T{ 0 MID-UINT+1 MID-UINT+1 WITHIN -> FALSE }T
T{ 0 MID-UINT+1 MAX-UINT WITHIN -> FALSE }T
T{ 0 MAX-UINT 0 WITHIN -> FALSE }T
T{ 0 MAX-UINT MID-UINT WITHIN -> TRUE }T
T{ 0 MAX-UINT MID-UINT+1 WITHIN -> TRUE }T
T{ 0 MAX-UINT MAX-UINT WITHIN -> FALSE }T
T{ MID-UINT 0 0 WITHIN -> FALSE }T
T{ MID-UINT 0 MID-UINT WITHIN -> FALSE }T
T{ MID-UINT 0 MID-UINT+1 WITHIN -> TRUE }T
T{ MID-UINT 0 MAX-UINT WITHIN -> TRUE }T
T{ MID-UINT MID-UINT 0 WITHIN -> TRUE }T
T{ MID-UINT MID-UINT MID-UINT WITHIN -> FALSE }T
T{ MID-UINT MID-UINT MID-UINT+1 WITHIN -> TRUE }T
T{ MID-UINT MID-UINT MAX-UINT WITHIN -> TRUE }T
T{ MID-UINT MID-UINT+1 0 WITHIN -> FALSE }T
T{ MID-UINT MID-UINT+1 MID-UINT WITHIN -> FALSE }T
T{ MID-UINT MID-UINT+1 MID-UINT+1 WITHIN -> FALSE }T
T{ MID-UINT MID-UINT+1 MAX-UINT WITHIN -> FALSE }T
T{ MID-UINT MAX-UINT 0 WITHIN -> FALSE }T
T{ MID-UINT MAX-UINT MID-UINT WITHIN -> FALSE }T
T{ MID-UINT MAX-UINT MID-UINT+1 WITHIN -> TRUE }T
T{ MID-UINT MAX-UINT MAX-UINT WITHIN -> FALSE }T
T{ MID-UINT+1 0 0 WITHIN -> FALSE }T
T{ MID-UINT+1 0 MID-UINT WITHIN -> FALSE }T
T{ MID-UINT+1 0 MID-UINT+1 WITHIN -> FALSE }T
T{ MID-UINT+1 0 MAX-UINT WITHIN -> TRUE }T
T{ MID-UINT+1 MID-UINT 0 WITHIN -> TRUE }T
T{ MID-UINT+1 MID-UINT MID-UINT WITHIN -> FALSE }T
T{ MID-UINT+1 MID-UINT MID-UINT+1 WITHIN -> FALSE }T
T{ MID-UINT+1 MID-UINT MAX-UINT WITHIN -> TRUE }T
T{ MID-UINT+1 MID-UINT+1 0 WITHIN -> TRUE }T
T{ MID-UINT+1 MID-UINT+1 MID-UINT WITHIN -> TRUE }T
T{ MID-UINT+1 MID-UINT+1 MID-UINT+1 WITHIN -> FALSE }T
T{ MID-UINT+1 MID-UINT+1 MAX-UINT WITHIN -> TRUE }T
T{ MID-UINT+1 MAX-UINT 0 WITHIN -> FALSE }T
T{ MID-UINT+1 MAX-UINT MID-UINT WITHIN -> FALSE }T
T{ MID-UINT+1 MAX-UINT MID-UINT+1 WITHIN -> FALSE }T
T{ MID-UINT+1 MAX-UINT MAX-UINT WITHIN -> FALSE }T
T{ MAX-UINT 0 0 WITHIN -> FALSE }T
T{ MAX-UINT 0 MID-UINT WITHIN -> FALSE }T
T{ MAX-UINT 0 MID-UINT+1 WITHIN -> FALSE }T
T{ MAX-UINT 0 MAX-UINT WITHIN -> FALSE }T
T{ MAX-UINT MID-UINT 0 WITHIN -> TRUE }T
T{ MAX-UINT MID-UINT MID-UINT WITHIN -> FALSE }T
T{ MAX-UINT MID-UINT MID-UINT+1 WITHIN -> FALSE }T
T{ MAX-UINT MID-UINT MAX-UINT WITHIN -> FALSE }T
T{ MAX-UINT MID-UINT+1 0 WITHIN -> TRUE }T
T{ MAX-UINT MID-UINT+1 MID-UINT WITHIN -> TRUE }T
T{ MAX-UINT MID-UINT+1 MID-UINT+1 WITHIN -> FALSE }T
T{ MAX-UINT MID-UINT+1 MAX-UINT WITHIN -> FALSE }T
T{ MAX-UINT MAX-UINT 0 WITHIN -> TRUE }T
T{ MAX-UINT MAX-UINT MID-UINT WITHIN -> TRUE }T
T{ MAX-UINT MAX-UINT MID-UINT+1 WITHIN -> TRUE }T
T{ MAX-UINT MAX-UINT MAX-UINT WITHIN -> FALSE }T

T{ MIN-INT MIN-INT MIN-INT WITHIN -> FALSE }T
T{ MIN-INT MIN-INT 0 WITHIN -> TRUE }T
T{ MIN-INT MIN-INT 1 WITHIN -> TRUE }T
T{ MIN-INT MIN-INT MAX-INT WITHIN -> TRUE }T
T{ MIN-INT 0 MIN-INT WITHIN -> FALSE }T
T{ MIN-INT 0 0 WITHIN -> FALSE }T
T{ MIN-INT 0 1 WITHIN -> FALSE }T
T{ MIN-INT 0 MAX-INT WITHIN -> FALSE }T
T{ MIN-INT 1 MIN-INT WITHIN -> FALSE }T
T{ MIN-INT 1 0 WITHIN -> TRUE }T
T{ MIN-INT 1 1 WITHIN -> FALSE }T
T{ MIN-INT 1 MAX-INT WITHIN -> FALSE }T
T{ MIN-INT MAX-INT MIN-INT WITHIN -> FALSE }T
T{ MIN-INT MAX-INT 0 WITHIN -> TRUE }T
T{ MIN-INT MAX-INT 1 WITHIN -> TRUE }T
T{ MIN-INT MAX-INT MAX-INT WITHIN -> FALSE }T
T{ 0 MIN-INT MIN-INT WITHIN -> FALSE }T
T{ 0 MIN-INT 0 WITHIN -> FALSE }T
T{ 0 MIN-INT 1 WITHIN -> TRUE }T
T{ 0 MIN-INT MAX-INT WITHIN -> TRUE }T
T{ 0 0 MIN-INT WITHIN -> TRUE }T
T{ 0 0 0 WITHIN -> FALSE }T
T{ 0 0 1 WITHIN -> TRUE }T
T{ 0 0 MAX-INT WITHIN -> TRUE }T
T{ 0 1 MIN-INT WITHIN -> FALSE }T
T{ 0 1 0 WITHIN -> FALSE }T
T{ 0 1 1 WITHIN -> FALSE }T
T{ 0 1 MAX-INT WITHIN -> FALSE }T
T{ 0 MAX-INT MIN-INT WITHIN -> FALSE }T
T{ 0 MAX-INT 0 WITHIN -> FALSE }T
T{ 0 MAX-INT 1 WITHIN -> TRUE }T
T{ 0 MAX-INT MAX-INT WITHIN -> FALSE }T
T{ 1 MIN-INT MIN-INT WITHIN -> FALSE }T
T{ 1 MIN-INT 0 WITHIN -> FALSE }T
T{ 1 MIN-INT 1 WITHIN -> FALSE }T
T{ 1 MIN-INT MAX-INT WITHIN -> TRUE }T
T{ 1 0 MIN-INT WITHIN -> TRUE }T
T{ 1 0 0 WITHIN -> FALSE }T
T{ 1 0 1 WITHIN -> FALSE }T
T{ 1 0 MAX-INT WITHIN -> TRUE }T
T{ 1 1 MIN-INT WITHIN -> TRUE }T
T{ 1 1 0 WITHIN -> TRUE }T
T{ 1 1 1 WITHIN -> FALSE }T
T{ 1 1 MAX-INT WITHIN -> TRUE }T
T{ 1 MAX-INT MIN-INT WITHIN -> FALSE }T
T{ 1 MAX-INT 0 WITHIN -> FALSE }T
T{ 1 MAX-INT 1 WITHIN -> FALSE }T
T{ 1 MAX-INT MAX-INT WITHIN -> FALSE }T
T{ MAX-INT MIN-INT MIN-INT WITHIN -> FALSE }T
T{ MAX-INT MIN-INT 0 WITHIN -> FALSE }T
T{ MAX-INT MIN-INT 1 WITHIN -> FALSE }T
T{ MAX-INT MIN-INT MAX-INT WITHIN -> FALSE }T
T{ MAX-INT 0 MIN-INT WITHIN -> TRUE }T
T{ MAX-INT 0 0 WITHIN -> FALSE }T
T{ MAX-INT 0 1 WITHIN -> FALSE }T
T{ MAX-INT 0 MAX-INT WITHIN -> FALSE }T
T{ MAX-INT 1 MIN-INT WITHIN -> TRUE }T
T{ MAX-INT 1 0 WITHIN -> TRUE }T
T{ MAX-INT 1 1 WITHIN -> FALSE }T
T{ MAX-INT 1 MAX-INT WITHIN -> FALSE }T
T{ MAX-INT MAX-INT MIN-INT WITHIN -> TRUE }T
T{ MAX-INT MAX-INT 0 WITHIN -> TRUE }T
T{ MAX-INT MAX-INT 1 WITHIN -> TRUE }T
T{ MAX-INT MAX-INT MAX-INT WITHIN -> FALSE }T

  \ ===========================================================

TESTING UNUSED  (contributed by James Bowman & Peter Knaggs)

VARIABLE UNUSED0

T{ UNUSED DROP -> }T

T{ ALIGN UNUSED UNUSED0 ! 0 , UNUSED CELL+ UNUSED0 @ =
   -> TRUE }T

T{ UNUSED UNUSED0 ! 0 C, UNUSED CHAR+ UNUSED0 @ = -> TRUE }T
  \ aligned -> unaligned

T{ UNUSED UNUSED0 ! 0 C, UNUSED CHAR+ UNUSED0 @ = -> TRUE }T
  \ unaligned -> ?

  \ ===========================================================

TESTING AGAIN   (contributed by James Bowman)

T{ : AG0 701 BEGIN DUP 7 MOD 0= IF EXIT THEN 1+ AGAIN ; -> }T
T{ AG0 -> 707 }T

  \ ===========================================================

TESTING MARKER   (contributed by James Bowman)

T{ : MA? BL WORD FIND NIP 0<> ; -> }T
T{ MARKER MA0 -> }T
T{ : MA1 111 ; -> }T
T{ MARKER MA2 -> }T
T{ : MA1 222 ; -> }T
T{ MA? MA0 MA? MA1 MA? MA2 -> TRUE TRUE TRUE }T
T{ MA1 MA2 MA1 -> 222 111 }T
T{ MA? MA0 MA? MA1 MA? MA2 -> TRUE TRUE FALSE }T
T{ MA0 -> }T
T{ MA? MA0 MA? MA1 MA? MA2 -> FALSE FALSE FALSE }T

  \ ===========================================================

TESTING ?DO

: QD ?DO I LOOP ;
T{ 789 789 QD -> }T
T{ -9876 -9876 QD -> }T
T{ 5 0 QD -> 0 1 2 3 4 }T

: QD1 ?DO I 10 +LOOP ;
T{ 50 1 QD1 -> 1 11 21 31 41 }T
T{ 50 0 QD1 -> 0 10 20 30 40 }T

: QD2 ?DO I 3 > IF LEAVE ELSE I THEN LOOP ;
T{ 5 -1 QD2 -> -1 0 1 2 3 }T

: QD3 ?DO I 1 +LOOP ;
T{ 4  4 QD3 -> }T
T{ 4  1 QD3 -> 1 2 3 }T
T{ 2 -1 QD3 -> -1 0 1 }T

: QD4 ?DO I -1 +LOOP ;
T{  4 4 QD4 -> }T
T{  1 4 QD4 -> 4 3 2 1 }T
T{ -1 2 QD4 -> 2 1 0 -1 }T

: QD5 ?DO I -10 +LOOP ;
T{   1 50 QD5 -> 50 40 30 20 10 }T
T{   0 50 QD5 -> 50 40 30 20 10 0 }T
T{ -25 10 QD5 -> 10 0 -10 -20 }T

VARIABLE ITERS
VARIABLE INCRMNT

: QD6 ( limit start increment -- )
   INCRMNT !
   0 ITERS !
   ?DO
      1 ITERS +!
      I
      ITERS @  6 = IF LEAVE THEN
      INCRMNT @
   +LOOP ITERS @
;

T{  4  4 -1 QD6 -> 0 }T
T{  1  4 -1 QD6 -> 4 3 2 1 4 }T
T{  4  1 -1 QD6 -> 1 0 -1 -2 -3 -4 6 }T
T{  4  1  0 QD6 -> 1 1 1 1 1 1 6 }T
T{  0  0  0 QD6 -> 0 }T
T{  1  4  0 QD6 -> 4 4 4 4 4 4 6 }T
T{  1  4  1 QD6 -> 4 5 6 7 8 9 6 }T
T{  4  1  1 QD6 -> 1 2 3 3 }T
T{  4  4  1 QD6 -> 0 }T
T{  2 -1 -1 QD6 -> -1 -2 -3 -4 -5 -6 6 }T
T{ -1  2 -1 QD6 -> 2 1 0 -1 4 }T
T{  2 -1  0 QD6 -> -1 -1 -1 -1 -1 -1 6 }T
T{ -1  2  0 QD6 -> 2 2 2 2 2 2 6 }T
T{ -1  2  1 QD6 -> 2 3 4 5 6 7 6 }T
T{  2 -1  1 QD6 -> -1 0 1 3 }T

  \ ===========================================================

TESTING BUFFER:

T{ 8 BUFFER: BUF:TEST -> }T
T{ BUF:TEST DUP ALIGNED = -> TRUE }T
T{ 111 BUF:TEST ! 222 BUF:TEST CELL+ ! -> }T
T{ BUF:TEST @ BUF:TEST CELL+ @ -> 111 222 }T

  \ ===========================================================

TESTING VALUE TO

T{ 111 VALUE VAL1 -999 VALUE VAL2 -> }T
T{ VAL1 -> 111 }T
T{ VAL2 -> -999 }T
T{ 222 TO VAL1 -> }T
T{ VAL1 -> 222 }T
T{ : VD1 VAL1 ; -> }T
T{ VD1 -> 222 }T
T{ : VD2 TO VAL2 ; -> }T
T{ VAL2 -> -999 }T
T{ -333 VD2 -> }T
T{ VAL2 -> -333 }T
T{ VAL1 -> 222 }T
T{ 123 VALUE VAL3 IMMEDIATE VAL3 -> 123 }T
T{ : VD3 VAL3 LITERAL ; VD3 -> 123 }T

  \ ===========================================================

TESTING CASE OF ENDOF ENDCASE

: CS1 CASE 1 OF 111 ENDOF
           2 OF 222 ENDOF
           3 OF 333 ENDOF
           >R 999 R>
      ENDCASE
;

T{ 1 CS1 -> 111 }T
T{ 2 CS1 -> 222 }T
T{ 3 CS1 -> 333 }T
T{ 4 CS1 -> 999 }T

  \ Nested CASE's

: CS2 >R CASE -1 OF CASE R@ 1 OF 100 ENDOF
                            2 OF 200 ENDOF
                           >R -300 R>
                    ENDCASE
                 ENDOF
              -2 OF CASE R@ 1 OF -99  ENDOF
                            >R -199 R>
                    ENDCASE
                 ENDOF
                 >R 299 R>
         ENDCASE R> DROP
;

T{ -1 1 CS2 ->  100 }T
T{ -1 2 CS2 ->  200 }T
T{ -1 3 CS2 -> -300 }T
T{ -2 1 CS2 -> -99  }T
T{ -2 2 CS2 -> -199 }T
T{  0 2 CS2 ->  299 }T

  \ Boolean short circuiting using CASE

: CS3 ( N1 -- N2 )
   CASE 1- FALSE OF 11 ENDOF
        1- FALSE OF 22 ENDOF
        1- FALSE OF 33 ENDOF
        44 SWAP
   ENDCASE
;

T{ 1 CS3 -> 11 }T
T{ 2 CS3 -> 22 }T
T{ 3 CS3 -> 33 }T
T{ 9 CS3 -> 44 }T

  \ Empty CASE statements with/without default

T{ : CS4 CASE ENDCASE ; 1 CS4 -> }T
T{ : CS5 CASE 2 SWAP ENDCASE ; 1 CS5 -> 2 }T
T{ : CS6 CASE 1 OF ENDOF 2 ENDCASE ; 1 CS6 -> }T
T{ : CS7 CASE 3 OF ENDOF 2 ENDCASE ; 1 CS7 -> 1 }T

  \ ===========================================================

TESTING :NONAME RECURSE

VARIABLE NN1
VARIABLE NN2
:NONAME 1234 ; NN1 !
:NONAME 9876 ; NN2 !
T{ NN1 @ EXECUTE -> 1234 }T
T{ NN2 @ EXECUTE -> 9876 }T

T{ :NONAME ( n -- 0,1,..n ) DUP IF DUP >R 1- RECURSE R> THEN ;
   CONSTANT RN1 -> }T
T{ 0 RN1 EXECUTE -> 0 }T
T{ 4 RN1 EXECUTE -> 0 1 2 3 4 }T

:NONAME ( n -- n1 ) \ Multiple RECURSEs in one definition
   1- DUP
   CASE 0 OF EXIT ENDOF
        1 OF 11 SWAP RECURSE ENDOF
        2 OF 22 SWAP RECURSE ENDOF
        3 OF 33 SWAP RECURSE ENDOF
        DROP ABS RECURSE EXIT
   ENDCASE
; CONSTANT RN2

T{  1 RN2 EXECUTE -> 0 }T
T{  2 RN2 EXECUTE -> 11 0 }T
T{  4 RN2 EXECUTE -> 33 22 11 0 }T
T{ 25 RN2 EXECUTE -> 33 22 11 0 }T

  \ ===========================================================

TESTING C"

T{ : CQ1 C" 123" ; -> }T
T{ CQ1 COUNT EVALUATE -> 123 }T
T{ : CQ2 C" " ; -> }T
T{ CQ2 COUNT EVALUATE -> }T
T{ : CQ3 C" 2345"COUNT EVALUATE ; CQ3 -> 2345 }T

  \ ===========================================================

TESTING COMPILE,

:NONAME DUP + ; CONSTANT DUP+
T{ : Q DUP+ COMPILE, ; -> }T
T{ : AS1 [ Q ] ; -> }T
T{ 123 AS1 -> 246 }T

  \ ===========================================================

  \ Cannot automatically test `SAVE-INPUT` and `RESTORE-INPUT`,
  \ from a console source.

TESTING SAVE-INPUT and RESTORE-INPUT with a string source

VARIABLE SI_INC 0 SI_INC !

: SI1
   SI_INC @ >IN +!
   15 SI_INC !
;

: S$ S" SAVE-INPUT SI1 RESTORE-INPUT 12345" ;

T{ S$ EVALUATE SI_INC @ -> 0 2345 15 }T

  \ ===========================================================

TESTING .(

CR CR .( Output from .()
T{ CR .( You should see -9876: ) -9876 . -> }T
T{ CR .( and again: ).( -9876)CR -> }T

CR CR .( On the next 2 lines you should see First then Second )
      .( messages:)

T{ : DOTP CR ." Second message via ." [CHAR] " EMIT
     \ Check .( is immediate
     [ CR ] .( First message via .( ) ; DOTP -> }T
CR CR
T{ : IMM? BL WORD FIND NIP ; IMM? .( -> 1 }T

  \ ===========================================================

TESTING .R and U.R - has to handle different cell sizes

  \ Create some large integers just below/above `MAX-INT` and
  \ `MIN-INT`.

MAX-INT 73 79 */ CONSTANT LI1
MIN-INT 71 73 */ CONSTANT LI2

LI1 0 <# #S #> NIP CONSTANT LENLI1

: (.R&U.R) ( u1 u2 -- )
  \ u1 <= string length, u2 is required indentation
  TUCK + >R
  LI1 OVER SPACES  . CR R@    LI1 SWAP  .R CR
  LI2 OVER SPACES  . CR R@ 1+ LI2 SWAP  .R CR
  LI1 OVER SPACES U. CR R@    LI1 SWAP U.R CR
  LI2 SWAP SPACES U. CR R>    LI2 SWAP U.R CR ;

: .R&U.R ( -- )
  CR ." You should see lines duplicated:" CR
  ." indented by 0 spaces" CR 0      0 (.R&U.R) CR
  ." indented by 0 spaces" CR LENLI1 0 (.R&U.R) CR
    \ Just fits required width
  ." indented by 5 spaces" CR LENLI1 5 (.R&U.R) CR ;

CR CR .( Output from .R and U.R)
T{ .R&U.R -> }T

  \ ===========================================================

TESTING PAD ERASE
  \ Must handle different size characters i.e. 1 CHARS >= 1

84 CONSTANT CHARS/PAD      \ Minimum size of PAD in chars
CHARS/PAD CHARS CONSTANT AUS/PAD
: CHECKPAD ( caddr u ch -- f ) \ f = TRUE if u chars = ch
   SWAP 0
   ?DO
      OVER I CHARS + C@ OVER <>
      IF 2DROP UNLOOP FALSE EXIT THEN
   LOOP
   2DROP TRUE
;

T{ PAD DROP -> }T
T{ 0 INVERT PAD C! -> }T
T{ PAD C@ CONSTANT MAXCHAR -> }T
T{ PAD CHARS/PAD 2DUP MAXCHAR FILL MAXCHAR CHECKPAD -> TRUE }T
T{ PAD CHARS/PAD 2DUP CHARS ERASE 0 CHECKPAD -> TRUE }T
T{ PAD CHARS/PAD 2DUP MAXCHAR FILL PAD 0 ERASE MAXCHAR CHECKPAD
   -> TRUE }T
T{ PAD 43 CHARS + 9 CHARS ERASE -> }T
T{ PAD 43 MAXCHAR CHECKPAD -> TRUE }T
T{ PAD 43 CHARS + 9 0 CHECKPAD -> TRUE }T
T{ PAD 52 CHARS + CHARS/PAD 52 - MAXCHAR CHECKPAD -> TRUE }T

  \ Check that use of `WORD` and pictured numeric output do not
  \ corrupt `PAD`. Minimum size of buffers for these are 33
  \ chars and (2*n)+2 chars respectively where n is number of
  \ bits per cell.

PAD CHARS/PAD ERASE
2 BASE !
MAX-UINT MAX-UINT <# #S CHAR 1 DUP HOLD HOLD #> 2DROP
DECIMAL
BL WORD 12345678123456781234567812345678 DROP
T{ PAD CHARS/PAD 0 CHECKPAD -> TRUE }T

  \ ===========================================================

TESTING PARSE

T{ CHAR | PARSE 1234| DUP ROT ROT EVALUATE -> 4 1234 }T
T{ CHAR ^ PARSE  23 45 ^ DUP ROT ROT EVALUATE -> 7 23 45 }T
: PA1 [CHAR] $ PARSE DUP >R PAD SWAP CHARS MOVE PAD R> ;
T{ PA1 3456
   DUP ROT ROT EVALUATE -> 4 3456 }T
T{ CHAR A PARSE A SWAP DROP -> 0 }T
T{ CHAR Z PARSE
   SWAP DROP -> 0 }T
T{ CHAR " PARSE 4567 "DUP ROT ROT EVALUATE -> 5 4567 }T

  \ ===========================================================

TESTING PARSE-NAME  (Forth 2012)
  \ Adapted from the PARSE-NAME RfD tests

T{ PARSE-NAME abcd  STR1  S= -> TRUE }T     \ No leading spaces
T{ PARSE-NAME      abcde STR2 S= -> TRUE }T \ Leading spaces

  \ Test empty parse area, new lines are necessary
T{ PARSE-NAME
  NIP -> 0 }T
  \ Empty parse area with spaces after PARSE-NAME
T{ PARSE-NAME
  NIP -> 0 }T

T{ : PARSE-NAME-TEST ( "name1" "name2" -- n )
    PARSE-NAME PARSE-NAME S= ; -> }T
T{ PARSE-NAME-TEST abcd abcd  -> TRUE }T
T{ PARSE-NAME-TEST abcd   abcd  -> TRUE }T  \ Leading spaces
T{ PARSE-NAME-TEST abcde abcdf -> FALSE }T
T{ PARSE-NAME-TEST abcdf abcde -> FALSE }T
T{ PARSE-NAME-TEST abcde abcde
   -> TRUE }T         \ Parse to end of line
T{ PARSE-NAME-TEST abcde           abcde
   -> TRUE }T         \ Leading and trailing spaces

  \ ===========================================================

TESTING DEFER DEFER@ DEFER! IS ACTION-OF (Forth 2012)
  \ Adapted from the Forth 200X RfD tests

T{ DEFER DEFER1 -> }T
T{ : MY-DEFER DEFER ; -> }T
T{ : IS-DEFER1 IS DEFER1 ; -> }T
T{ : ACTION-DEFER1 ACTION-OF DEFER1 ; -> }T
T{ : DEF! DEFER! ; -> }T
T{ : DEF@ DEFER@ ; -> }T

T{ ' * ' DEFER1 DEFER! -> }T
T{ 2 3 DEFER1 -> 6 }T
T{ ' DEFER1 DEFER@ -> ' * }T
T{ ' DEFER1 DEF@ -> ' * }T
T{ ACTION-OF DEFER1 -> ' * }T
T{ ACTION-DEFER1 -> ' * }T
T{ ' + IS DEFER1 -> }T
T{ 1 2 DEFER1 -> 3 }T
T{ ' DEFER1 DEFER@ -> ' + }T
T{ ' DEFER1 DEF@ -> ' + }T
T{ ACTION-OF DEFER1 -> ' + }T
T{ ACTION-DEFER1 -> ' + }T
T{ ' - IS-DEFER1 -> }T
T{ 1 2 DEFER1 -> -1 }T
T{ ' DEFER1 DEFER@ -> ' - }T
T{ ' DEFER1 DEF@ -> ' - }T
T{ ACTION-OF DEFER1 -> ' - }T
T{ ACTION-DEFER1 -> ' - }T

T{ MY-DEFER DEFER2 -> }T
T{ ' DUP IS DEFER2 -> }T
T{ 1 DEFER2 -> 1 1 }T

  \ ===========================================================

TESTING HOLDS  (Forth 2012)

: HTEST S" Testing HOLDS" ;
: HTEST2 S" works" ;
: HTEST3 S" Testing HOLDS works 123" ;
T{ 0 0 <#  HTEST HOLDS #> HTEST S= -> TRUE }T
T{ 123 0 <# #S BL HOLD HTEST2 HOLDS BL HOLD HTEST HOLDS #>
   HTEST3 S= -> TRUE }T
T{ : HLD HOLDS ; -> }T
T{ 0 0 <#  HTEST HLD #> HTEST S= -> TRUE }T

  \ ===========================================================

TESTING REFILL SOURCE-ID
  \ `REFILL` and `SOURCE-ID` from the user input device can't
  \ be tested from a file, can only be tested from a string via
  \ `EVALUATE`.

T{ : RF1  S" REFILL" EVALUATE ; RF1 -> FALSE }T
T{ : SID1  S" SOURCE-ID" EVALUATE ; SID1 -> -1 }T

  \ ===========================================================

TESTING S\"  (Forth 2012 compilation mode)

  \ Extended the Forth 200X RfD tests.

  \ Note this tests the Core Ext definition of `S\"` which has
  \ undefined interpretation semantics. `S\"` in interpretation
  \ mode is tested in the tests on the File-Access word set.

T{ : SSQ1 S\" abc" S" abc" S= ; -> }T  \ No escapes
T{ SSQ1 -> TRUE }T
T{ : SSQ2 S\" " ; SSQ2 SWAP DROP -> 0 }T    \ Empty string

T{ : SSQ3 S\" \a\b\e\f\l\m\q\r\t\v\x0F0\x1Fa\xaBx\z\"\\" ;
   -> }T
T{ SSQ3 SWAP DROP          ->  20 }T \ String length
T{ SSQ3 DROP            C@ ->   7 }T \ \a   BEL Bell
T{ SSQ3 DROP  1 CHARS + C@ ->   8 }T \ \b   BS  Backspace
T{ SSQ3 DROP  2 CHARS + C@ ->  27 }T \ \e   ESC Escape
T{ SSQ3 DROP  3 CHARS + C@ ->  12 }T \ \f   FF  Form feed
T{ SSQ3 DROP  4 CHARS + C@ ->  10 }T \ \l   LF  Line feed
T{ SSQ3 DROP  5 CHARS + C@ ->  13 }T \ \m       CR of CR/LF
T{ SSQ3 DROP  6 CHARS + C@ ->  10 }T \          LF of CR/LF
T{ SSQ3 DROP  7 CHARS + C@ ->  34 }T \ \q   "   Double Quote
T{ SSQ3 DROP  8 CHARS + C@ ->  13 }T \ \r   CR  Carriage Return
T{ SSQ3 DROP  9 CHARS + C@ ->   9 }T \ \t   TAB Horizontal Tab
T{ SSQ3 DROP 10 CHARS + C@ ->  11 }T \ \v   VT  Vertical Tab
T{ SSQ3 DROP 11 CHARS + C@ ->  15 }T \ \x0F     Given Char
T{ SSQ3 DROP 12 CHARS + C@ ->  48 }T \ 0    0   Digit follow on
T{ SSQ3 DROP 13 CHARS + C@ ->  31 }T \ \x1F     Given Char
T{ SSQ3 DROP 14 CHARS + C@ ->  97 }T \ a    a   Hex follow on
T{ SSQ3 DROP 15 CHARS + C@ -> 171 }T \ \xaB
                                     \ Insensitive Given Char
T{ SSQ3 DROP 16 CHARS + C@ -> 120 }T \ x    x
                                     \ Non hex follow on
T{ SSQ3 DROP 17 CHARS + C@ ->   0 }T \ \z   NUL No Character
T{ SSQ3 DROP 18 CHARS + C@ ->  34 }T \ \"   "   Double Quote
T{ SSQ3 DROP 19 CHARS + C@ ->  92 }T \ \\   \   Back Slash

  \ The above does not test `\n` as this is a system dependent
  \ value.  Check it displays a new line:

CR .( The next test should display:)
CR .( One line...)
CR .( another line)
T{ : SSQ4 S\" \nOne line...\nanotherLine\n" type ; SSQ4 -> }T

  \ Test bare escapable characters appear as themselves:
T{ : SSQ5 S\" abeflmnqrtvxz" S" abeflmnqrtvxz" S= ; SSQ5
   -> TRUE }T

T{ : SSQ6 S\" a\""2DROP 1111 ; SSQ6 -> 1111 }T
  \ Parsing behaviour

T{ : SSQ7
     S\" 111 : SSQ8 s\\\" 222\" EVALUATE ; SSQ8 333" EVALUATE ;
   -> }T
T{ SSQ7 -> 111 222 333 }T
T{ : SSQ9
     S\" 11 : SSQ10 s\\\" \\x32\\x32\" EVALUATE ;
     SSQ10 33" EVALUATE ; -> }T
T{ SSQ9 -> 11 22 33 }T

  \ ===========================================================
CORE-EXT-ERRORS SET-ERROR-COUNT

CR .( End of Core Extension word tests) CR

  \ Additional tests on the the ANS Forth Core word set

  \ This program was written by Gerry Jackson in 2007, with
  \ contributions from others where indicated, and is in the
  \ public domain - it can be distributed and/or modified in
  \ any way but please retain this notice.

  \ This program is distributed in the hope that it will be
  \ useful, but WITHOUT ANY WARRANTY; without even the implied
  \ warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
  \ PURPOSE.

  \ The tests are not claimed to be comprehensive or correct

  \ ===========================================================
  \ Version 0.13 TRUE changed to <TRUE> as defined in core.fr
  \         0.12 22 July 2015, >IN Manipulation test modified to work on 16 bit
  \              Forth systems
  \         0.11 25 April 2015 Number prefixes # $ % and 'c' character input tested
  \         0.10 3 August 2014 Test IMMEDIATE doesn't toggle an immediate flag
  \         0.3  1 April 2012 Tests placed in the public domain.
  \              Testing multiple ELSE's.
  \              Further tests on DO +LOOPs.
  \              Ackermann function added to test RECURSE.
  \              >IN manipulation in interpreter mode
  \              Immediate CONSTANTs, VARIABLEs and CREATEd words tests.
  \              :NONAME with RECURSE moved to core extension tests.
  \              Parsing behaviour of S" ." and ( tested
  \         0.2  6 March 2009 { and } replaced with T{ and }T
  \              Added extra RECURSE tests
  \         0.1  20 April 2007 Created
  \ ===========================================================
  \ The tests are based on John Hayes test program for the core word set
\
  \ This file provides some more tests on Core words where the original Hayes
  \ tests are thought to be incomplete
\
  \ Words tested in this file are:
  \     DO +LOOP RECURSE ELSE >IN IMMEDIATE
  \ ===========================================================
  \ Assumptions and dependencies:
  \     - tester.fr or ttester.fs has been loaded prior to this file
  \     - core.fr has been loaded so that constants <TRUE> MAX-INT, MIN-INT and
  \       MAX-UINT are defined
  \ ===========================================================

DECIMAL

TESTING DO +LOOP with run-time increment, negative increment,
TESTING infinite loop
  \ Contributed by Reinhold Straub

VARIABLE ITERATIONS
VARIABLE INCREMENT
: GD7 ( LIMIT START INCREMENT -- )
   INCREMENT !
   0 ITERATIONS !
   DO
      1 ITERATIONS +!
      I
      ITERATIONS @  6 = IF LEAVE THEN
      INCREMENT @
   +LOOP ITERATIONS @
;

T{  4  4 -1 GD7 -> 4 1 }T
T{  1  4 -1 GD7 -> 4 3 2 1 4 }T
T{  4  1 -1 GD7 -> 1 0 -1 -2 -3 -4 6 }T
T{  4  1  0 GD7 -> 1 1 1 1 1 1 6 }T
T{  0  0  0 GD7 -> 0 0 0 0 0 0 6 }T
T{  1  4  0 GD7 -> 4 4 4 4 4 4 6 }T
T{  1  4  1 GD7 -> 4 5 6 7 8 9 6 }T
T{  4  1  1 GD7 -> 1 2 3 3 }T
T{  4  4  1 GD7 -> 4 5 6 7 8 9 6 }T
T{  2 -1 -1 GD7 -> -1 -2 -3 -4 -5 -6 6 }T
T{ -1  2 -1 GD7 -> 2 1 0 -1 4 }T
T{  2 -1  0 GD7 -> -1 -1 -1 -1 -1 -1 6 }T
T{ -1  2  0 GD7 -> 2 2 2 2 2 2 6 }T
T{ -1  2  1 GD7 -> 2 3 4 5 6 7 6 }T
T{  2 -1  1 GD7 -> -1 0 1 3 }T
T{ -20 30 -10 GD7 -> 30 20 10 0 -10 -20 6 }T
T{ -20 31 -10 GD7 -> 31 21 11 1 -9 -19 6 }T
T{ -20 29 -10 GD7 -> 29 19 9 -1 -11 5 }T

  \ ===========================================================

TESTING DO +LOOP with large and small increments

  \ Contributed by Andrew Haley

MAX-UINT 8 RSHIFT 1+ CONSTANT USTEP
USTEP NEGATE CONSTANT -USTEP
MAX-INT 7 RSHIFT 1+ CONSTANT STEP
STEP NEGATE CONSTANT -STEP

VARIABLE BUMP

T{ : GD8 BUMP ! DO 1+ BUMP @ +LOOP ; -> }T

T{ 0 MAX-UINT 0 USTEP GD8 -> 256 }T
T{ 0 0 MAX-UINT -USTEP GD8 -> 256 }T

T{ 0 MAX-INT MIN-INT STEP GD8 -> 256 }T
T{ 0 MIN-INT MAX-INT -STEP GD8 -> 256 }T

  \ Two's complement arithmetic, wraps around modulo wordsize
  \ Only tested if the Forth system does wrap around, use of
  \ conditional compilation deliberately avoided.

MAX-INT 1+ MIN-INT = CONSTANT +WRAP?
MIN-INT 1- MAX-INT = CONSTANT -WRAP?
MAX-UINT 1+ 0=       CONSTANT +UWRAP?
0 1- MAX-UINT =      CONSTANT -UWRAP?

: GD9 ( n limit start step f result -- )
   >R IF GD8 ELSE 2DROP 2DROP R@ THEN -> R> }T
;

T{ 0 0 0  USTEP +UWRAP? 256 GD9
T{ 0 0 0 -USTEP -UWRAP?   1 GD9
T{ 0 MIN-INT MAX-INT  STEP +WRAP? 1 GD9
T{ 0 MAX-INT MIN-INT -STEP -WRAP? 1 GD9

  \ ===========================================================

TESTING DO +LOOP with maximum and minimum increments

: (-MI)
  MAX-INT DUP NEGATE + 0= IF MAX-INT NEGATE ELSE -32767 THEN ;
(-MI) CONSTANT -MAX-INT

T{ 0 1 0 MAX-INT GD8  -> 1 }T
T{ 0 -MAX-INT NEGATE -MAX-INT OVER GD8  -> 2 }T

T{ 0 MAX-INT  0 MAX-INT GD8  -> 1 }T
T{ 0 MAX-INT  1 MAX-INT GD8  -> 1 }T
T{ 0 MAX-INT -1 MAX-INT GD8  -> 2 }T
T{ 0 MAX-INT DUP 1- MAX-INT GD8  -> 1 }T

T{ 0 MIN-INT 1+   0 MIN-INT GD8  -> 1 }T
T{ 0 MIN-INT 1+  -1 MIN-INT GD8  -> 1 }T
T{ 0 MIN-INT 1+   1 MIN-INT GD8  -> 2 }T
T{ 0 MIN-INT 1+ DUP MIN-INT GD8  -> 1 }T

  \ ===========================================================

TESTING +LOOP setting I to an arbitrary value

  \ The specification for +LOOP permits the loop index I to be
  \ set to any value including a value outside the range given
  \ to the corresponding  DO.

: SET-I ( n1 n2 n3 -- n1-n2 | 1 )
  OVER = IF - ELSE 2DROP 1 THEN ;
  \ SET-I is a helper to set I in a DO...+LOOP to a given value
  \ n2 is the value of I in a DO...+LOOP
  \ n3 is a test value
  \ If n2=n3 then return n1-n2 else return 1

: -SET-I ( n1 n2 n3 -- n1-n2 | -1 )
  SET-I DUP 1 = IF NEGATE THEN ;

: PL1 20 1 DO I 18 I 3 SET-I +LOOP ;
T{ PL1 -> 1 2 3 18 19 }T
: PL2 20 1 DO I 20 I 2 SET-I +LOOP ;
T{ PL2 -> 1 2 }T
: PL3 20 5 DO I 19 I 2 SET-I DUP 1 = IF DROP 0 I 6 SET-I THEN
           +LOOP ;
T{ PL3 -> 5 6 0 1 2 19 }T
: PL4 20 1 DO I MAX-INT I 4 SET-I +LOOP ;
T{ PL4 -> 1 2 3 4 }T
: PL5 -20 -1 DO I -19 I -3 -SET-I +LOOP ;
T{ PL5 -> -1 -2 -3 -19 -20 }T
: PL6 -20 -1 DO I -21 I -4 -SET-I +LOOP ;
T{ PL6 -> -1 -2 -3 -4 }T
: PL7 -20 -1 DO I MIN-INT I -5 -SET-I +LOOP ;
T{ PL7 -> -1 -2 -3 -4 -5 }T
: PL8
  -20 -5 DO I -20 I -2 -SET-I DUP -1 =
            IF DROP 0 I -6 -SET-I THEN
         +LOOP ;
T{ PL8 -> -5 -6 0 -1 -2 -20 }T

  \ ===========================================================

TESTING multiple RECURSEs in one colon definition

: ACK ( m n -- u ) \ Ackermann function, from Rosetta Code
   OVER 0= IF  NIP 1+ EXIT  THEN \ ack(0, n) = n+1
   SWAP 1- SWAP ( -- m-1 n )
   DUP  0= IF  1+  RECURSE EXIT  THEN \ ack(m, 0) = ack(m-1, 1)
   1- OVER 1+ SWAP
   RECURSE RECURSE \ ack(m, n) = ack(m-1, ack(m,n-1))
  ;

T{ 0 0 ACK ->  1 }T
T{ 3 0 ACK ->  5 }T
T{ 2 4 ACK -> 11 }T

  \ ===========================================================

TESTING multiple ELSE's in an IF statement
  \ Discussed on comp.lang.forth and accepted as valid ANS
  \ Forth.

: MELSE IF 1 ELSE 2 ELSE 3 ELSE 4 ELSE 5 THEN ;
T{ 0 MELSE -> 2 4 }T
T{ -1 MELSE -> 1 3 5 }T

  \ ===========================================================

TESTING manipulation of >IN in interpreter mode

T{ 12345 DEPTH OVER 9 < 34 AND + 3 + >IN !
   -> 12345 2345 345 45 5 }T
T{ 14145 8115 ?DUP 0= 34 AND >IN +!
   TUCK MOD 14 >IN ! GCD CALCULATION -> 15 }T

  \ ===========================================================

TESTING IMMEDIATE with CONSTANT VARIABLE & CREATE [ ... DOES> ]

T{ 123 CONSTANT IW1 IMMEDIATE IW1 -> 123 }T
T{ : IW2 IW1 LITERAL ; IW2 -> 123 }T
T{ VARIABLE IW3 IMMEDIATE 234 IW3 ! IW3 @ -> 234 }T
T{ : IW4 IW3 [ @ ] LITERAL ; IW4 -> 234 }T
T{ :NONAME [ 345 ] IW3 [ ! ] ; DROP IW3 @ -> 345 }T
T{ CREATE IW5 456 , IMMEDIATE -> }T
T{ :NONAME IW5 [ @ IW3 ! ] ; DROP IW3 @ -> 456 }T
T{ : IW6 CREATE , IMMEDIATE DOES> @ 1+ ; -> }T
T{ 111 IW6 IW7 IW7 -> 112 }T
T{ : IW8 IW7 LITERAL 1+ ; IW8 -> 113 }T
T{ : IW9 CREATE , DOES> @ 2 + IMMEDIATE ; -> }T
: FIND-IW BL WORD FIND NIP ; ( -- 0 | 1 | -1 )
T{ 222 IW9 IW10 FIND-IW IW10 -> -1 }T \ IW10 is not immediate
T{ IW10 FIND-IW IW10 -> 224 1 }T      \ IW10 becomes immediate

  \ ===========================================================

TESTING that IMMEDIATE doesn't toggle a flag

VARIABLE IT1 0 IT1 !
: IT2 1234 IT1 ! ; IMMEDIATE IMMEDIATE
T{ : IT3 IT2 ; IT1 @ -> 1234 }T

  \ ===========================================================

TESTING parsing behaviour of S" ." and (
  \ which should parse to just beyond the terminating character
  \ no space needed

T{ : GC5 S" A string"2DROP ; GC5 -> }T
T{ ( A comment)1234 -> 1234 }T
T{ : PB1 CR ." You should see 2345: "." 2345"( A comment) CR ;
   PB1 -> }T

  \ ===========================================================

TESTING number prefixes # $ % and 'c' character input
  \ Adapted from the Forth 200X Draft 14.5 document

VARIABLE OLD-BASE
DECIMAL BASE @ OLD-BASE !
T{ #1289 -> 1289 }T
T{ #-1289 -> -1289 }T
T{ $12eF -> 4847 }T
T{ $-12eF -> -4847 }T
T{ %10010110 -> 150 }T
T{ %-10010110 -> -150 }T
T{ 'z' -> 122 }T
T{ 'Z' -> 90 }T
  \ Check BASE is unchanged
T{ BASE @ OLD-BASE @ = -> <TRUE> }T

  \ Repeat in Hex mode
16 OLD-BASE ! 16 BASE !
T{ #1289 -> 509 }T
T{ #-1289 -> -509 }T
T{ $12eF -> 12EF }T
T{ $-12eF -> -12EF }T
T{ %10010110 -> 96 }T
T{ %-10010110 -> -96 }T
T{ 'z' -> 7a }T
T{ 'Z' -> 5a }T
  \ Check BASE is unchanged
T{ BASE @ OLD-BASE @ = -> <TRUE> }T   \ 2

DECIMAL
  \ Check number prefixes in compile mode
T{ : nmp  #8327 $-2cbe %011010111 ''' ;
   nmp -> 8327 -11454 215 39 }T

  \ ===========================================================

TESTING definition names
  \ should support {1..31} graphical characters

: !"#$%&'()*+,-./0123456789:;<=>? 1 ;
T{ !"#$%&'()*+,-./0123456789:;<=>? -> 1 }T
: @ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^ 2 ;
T{ @ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^ -> 2 }T
: _`abcdefghijklmnopqrstuvwxyz{|} 3 ;
T{ _`abcdefghijklmnopqrstuvwxyz{|} -> 3 }T
: _`abcdefghijklmnopqrstuvwxyz{|~ 4 ; \ Last char different
T{ _`abcdefghijklmnopqrstuvwxyz{|~ -> 4 }T
T{ _`abcdefghijklmnopqrstuvwxyz{|} -> 3 }T

  \ ===========================================================

TESTING FIND with a zero length string and a non-existent word

CREATE EMPTYSTRING 0 C,
: EMPTYSTRING-FIND-CHECK ( c-addr 0 | xt 1 | xt -1 -- t|f )
    DUP IF ." FIND returns a TRUE value for an empty string!"
           CR THEN
    0= SWAP EMPTYSTRING = = ;
T{ EMPTYSTRING FIND EMPTYSTRING-FIND-CHECK -> <TRUE> }T

CREATE NON-EXISTENT-WORD   \ Same as in exceptiontest.fth
      15 C, CHAR $ C, CHAR $ C, CHAR Q C, CHAR W C, CHAR E C,
  CHAR Q C,
  CHAR W C, CHAR E C, CHAR Q C, CHAR W C, CHAR E C, CHAR R C,
  CHAR T C,
  CHAR $ C, CHAR $ C,
T{ NON-EXISTENT-WORD FIND -> NON-EXISTENT-WORD 0 }T

  \ ===========================================================

TESTING IF ... BEGIN ... REPEAT (unstructured)

T{ : UNS1
     DUP 0 > IF 9 SWAP BEGIN 1+ DUP 3 > IF EXIT THEN REPEAT ;
   -> }T
T{ -6 UNS1 -> -6 }T
T{  1 UNS1 -> 9 4 }T

  \ ===========================================================

TESTING DOES> doesn't cause a problem with a CREATEd address

: MAKE-2CONST DOES> 2@ ;
T{ CREATE 2K 3 , 2K , MAKE-2CONST 2K -> ' 2K >BODY 3 }T

CR .( End of additional Core tests) CR

  \ ===========================================================

  \ To test the ANS Forth Double-Number word set and double
  \ number extensions

  \ This program was written by Gerry Jackson in 2006, with
  \ contributions from others where indicated, and is in the
  \ public domain - it can be distributed and/or modified in
  \ any way but please retain this notice.

  \ This program is distributed in the hope that it will be
  \ useful, but WITHOUT ANY WARRANTY; without even the implied
  \ warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
  \ PURPOSE.

  \ The tests are not claimed to be comprehensive or correct
  \ ===========================================================
  \ Version 0.13  Assumptions and dependencies changed
  \         0.12  1 August 2015 test D< acts on MS cells of double word
  \         0.11  7 April 2015 2VALUE tested
  \         0.6   1 April 2012 Tests placed in the public domain.
  \               Immediate 2CONSTANTs and 2VARIABLEs tested
  \         0.5   20 November 2009 Various constants renamed to avoid
  \               redefinition warnings. <TRUE> and <FALSE> replaced
  \               with TRUE and FALSE
  \         0.4   6 March 2009 { and } replaced with T{ and }T
  \               Tests rewritten to be independent of word size and
  \               tests re-ordered
  \         0.3   20 April 2007 ANS Forth words changed to upper case
  \         0.2   30 Oct 2006 Updated following GForth test to include
  \               various constants from core.fr
  \         0.1   Oct 2006 First version released
  \ ===========================================================
  \ The tests are based on John Hayes test program for the core word set

  \ Words tested in this file are:
  \     2CONSTANT 2LITERAL 2VARIABLE D+ D- D. D.R D0< D0= D2* D2/
  \     D< D= D>S DABS DMAX DMIN DNEGATE M*/ M+ 2ROT DU<
  \ Also tests the interpreter and compiler reading a double number
  \ ===========================================================
  \ Assumptions and dependencies:
  \     - tester.fr (or ttester.fs), errorreport.fth and utilities.fth have been
  \       included prior to this file
  \     - the Core word set is available and tested
  \ ===========================================================
  \ Constant definitions

DECIMAL
0 INVERT        CONSTANT 1SD
1SD 1 RSHIFT    CONSTANT MAX-INTD   \ 01...1
MAX-INTD INVERT CONSTANT MIN-INTD   \ 10...0
MAX-INTD 2/     CONSTANT HI-INT     \ 001...1
MIN-INTD 2/     CONSTANT LO-INT     \ 110...1

  \ ===========================================================

TESTING interpreter and compiler reading double numbers
TESTING with/without prefixes

T{ 1. -> 1 0 }T
T{ -2. -> -2 -1 }T
T{ : RDL1 3. ; RDL1 -> 3 0 }T
T{ : RDL2 -4. ; RDL2 -> -4 -1 }T

VARIABLE OLD-DBASE
DECIMAL BASE @ OLD-DBASE !
T{ #12346789. -> 12346789. }T
T{ #-12346789. -> -12346789. }T
T{ $12aBcDeF. -> 313249263. }T
T{ $-12AbCdEf. -> -313249263. }T
T{ %10010110. -> 150. }T
T{ %-10010110. -> -150. }T
  \ Check BASE is unchanged
T{ BASE @ OLD-DBASE @ = -> <TRUE> }T

  \ Repeat in Hex mode
16 OLD-DBASE ! 16 BASE !
T{ #12346789. -> BC65A5. }T
T{ #-12346789. -> -BC65A5. }T
T{ $12aBcDeF. -> 12AbCdeF. }T
T{ $-12AbCdEf. -> -12ABCDef. }T
T{ %10010110. -> 96. }T
T{ %-10010110. -> -96. }T
  \ Check BASE is unchanged
T{ BASE @ OLD-DBASE @ = -> <TRUE> }T   \ 2

DECIMAL
  \ Check number prefixes in compile mode
T{ : dnmp  #8327. $-2cbe. %011010111. ; dnmp
   -> 8327. -11454. 215. }T

  \ ===========================================================

TESTING 2CONSTANT

T{ 1 2 2CONSTANT 2C1 -> }T
T{ 2C1 -> 1 2 }T
T{ : CD1 2C1 ; -> }T
T{ CD1 -> 1 2 }T
T{ : CD2 2CONSTANT ; -> }T
T{ -1 -2 CD2 2C2 -> }T
T{ 2C2 -> -1 -2 }T
T{ 4 5 2CONSTANT 2C3 IMMEDIATE 2C3 -> 4 5 }T
T{ : CD6 2C3 2LITERAL ; CD6 -> 4 5 }T

  \ ===========================================================
  \ Some 2CONSTANTs for the following tests

1SD MAX-INTD 2CONSTANT MAX-2INT  \ 01...1
0   MIN-INTD 2CONSTANT MIN-2INT  \ 10...0
MAX-2INT 2/  2CONSTANT HI-2INT   \ 001...1
MIN-2INT 2/  2CONSTANT LO-2INT   \ 110...0

  \ ===========================================================

TESTING DNEGATE

T{ 0. DNEGATE -> 0. }T
T{ 1. DNEGATE -> -1. }T
T{ -1. DNEGATE -> 1. }T
T{ MAX-2INT DNEGATE -> MIN-2INT SWAP 1+ SWAP }T
T{ MIN-2INT SWAP 1+ SWAP DNEGATE -> MAX-2INT }T

  \ ===========================================================

TESTING D+ with small integers

T{  0.  5. D+ ->  5. }T
T{ -5.  0. D+ -> -5. }T
T{  1.  2. D+ ->  3. }T
T{  1. -2. D+ -> -1. }T
T{ -1.  2. D+ ->  1. }T
T{ -1. -2. D+ -> -3. }T
T{ -1.  1. D+ ->  0. }T


TESTING D+ with mid range integers

T{  0  0  0  5 D+ ->  0  5 }T
T{ -1  5  0  0 D+ -> -1  5 }T
T{  0  0  0 -5 D+ ->  0 -5 }T
T{  0 -5 -1  0 D+ -> -1 -5 }T
T{  0  1  0  2 D+ ->  0  3 }T
T{ -1  1  0 -2 D+ -> -1 -1 }T
T{  0 -1  0  2 D+ ->  0  1 }T
T{  0 -1 -1 -2 D+ -> -1 -3 }T
T{ -1 -1  0  1 D+ -> -1  0 }T
T{ MIN-INTD 0 2DUP D+ -> 0 1 }T
T{ MIN-INTD S>D MIN-INTD 0 D+ -> 0 0 }T

TESTING D+ with large double integers

T{ HI-2INT 1. D+ -> 0 HI-INT 1+ }T
T{ HI-2INT 2DUP D+ -> 1SD 1- MAX-INTD }T
T{ MAX-2INT MIN-2INT D+ -> -1. }T
T{ MAX-2INT LO-2INT D+ -> HI-2INT }T
T{ HI-2INT MIN-2INT D+ 1. D+ -> LO-2INT }T
T{ LO-2INT 2DUP D+ -> MIN-2INT }T

  \ ===========================================================

TESTING D- with small integers

T{  0.  5. D- -> -5. }T
T{  5.  0. D- ->  5. }T
T{  0. -5. D- ->  5. }T
T{  1.  2. D- -> -1. }T
T{  1. -2. D- ->  3. }T
T{ -1.  2. D- -> -3. }T
T{ -1. -2. D- ->  1. }T
T{ -1. -1. D- ->  0. }T

TESTING D- with mid-range integers

T{  0  0  0  5 D- ->  0 -5 }T
T{ -1  5  0  0 D- -> -1  5 }T
T{  0  0 -1 -5 D- ->  1  4 }T
T{  0 -5  0  0 D- ->  0 -5 }T
T{ -1  1  0  2 D- -> -1 -1 }T
T{  0  1 -1 -2 D- ->  1  2 }T
T{  0 -1  0  2 D- ->  0 -3 }T
T{  0 -1  0 -2 D- ->  0  1 }T
T{  0  0  0  1 D- ->  0 -1 }T
T{ MIN-INTD 0 2DUP D- -> 0. }T
T{ MIN-INTD S>D MAX-INTD 0 D- -> 1 1SD }T

TESTING D- with large integers

T{ MAX-2INT MAX-2INT D- -> 0. }T
T{ MIN-2INT MIN-2INT D- -> 0. }T
T{ MAX-2INT HI-2INT  D- -> LO-2INT DNEGATE }T
T{ HI-2INT  LO-2INT  D- -> MAX-2INT }T
T{ LO-2INT  HI-2INT  D- -> MIN-2INT 1. D+ }T
T{ MIN-2INT MIN-2INT D- -> 0. }T
T{ MIN-2INT LO-2INT  D- -> LO-2INT }T

  \ ===========================================================

TESTING D0< D0=

T{ 0. D0< -> FALSE }T
T{ 1. D0< -> FALSE }T
T{ MIN-INTD 0 D0< -> FALSE }T
T{ 0 MAX-INTD D0< -> FALSE }T
T{ MAX-2INT  D0< -> FALSE }T
T{ -1. D0< -> TRUE }T
T{ MIN-2INT D0< -> TRUE }T

T{ 1. D0= -> FALSE }T
T{ MIN-INTD 0 D0= -> FALSE }T
T{ MAX-2INT  D0= -> FALSE }T
T{ -1 MAX-INTD D0= -> FALSE }T
T{ 0. D0= -> TRUE }T
T{ -1. D0= -> FALSE }T
T{ 0 MIN-INTD D0= -> FALSE }T

  \ ===========================================================

TESTING D2* D2/

T{ 0. D2* -> 0. D2* }T
T{ MIN-INTD 0 D2* -> 0 1 }T
T{ HI-2INT D2* -> MAX-2INT 1. D- }T
T{ LO-2INT D2* -> MIN-2INT }T

T{ 0. D2/ -> 0. }T
T{ 1. D2/ -> 0. }T
T{ 0 1 D2/ -> MIN-INTD 0 }T
T{ MAX-2INT D2/ -> HI-2INT }T
T{ -1. D2/ -> -1. }T
T{ MIN-2INT D2/ -> LO-2INT }T

  \ ===========================================================

TESTING D< D=

T{  0.  1. D< -> TRUE  }T
T{  0.  0. D< -> FALSE }T
T{  1.  0. D< -> FALSE }T
T{ -1.  1. D< -> TRUE  }T
T{ -1.  0. D< -> TRUE  }T
T{ -2. -1. D< -> TRUE  }T
T{ -1. -2. D< -> FALSE }T
T{ 0 1   1. D< -> FALSE }T  \ Suggested by Helmut Eller
T{ 1.  0 1  D< -> TRUE  }T
T{ 0 -1 1 -2 D< -> FALSE }T
T{ 1 -2 0 -1 D< -> TRUE  }T
T{ -1. MAX-2INT D< -> TRUE }T
T{ MIN-2INT MAX-2INT D< -> TRUE }T
T{ MAX-2INT -1. D< -> FALSE }T
T{ MAX-2INT MIN-2INT D< -> FALSE }T
T{ MAX-2INT 2DUP -1. D+ D< -> FALSE }T
T{ MIN-2INT 2DUP  1. D+ D< -> TRUE  }T

T{ MAX-INTD S>D 2DUP 1. D+ D< -> TRUE }T
  \ Ensure D< acts on MS cells

T{ -1. -1. D= -> TRUE  }T
T{ -1.  0. D= -> FALSE }T
T{ -1.  1. D= -> FALSE }T
T{  0. -1. D= -> FALSE }T
T{  0.  0. D= -> TRUE  }T
T{  0.  1. D= -> FALSE }T
T{  1. -1. D= -> FALSE }T
T{  1.  0. D= -> FALSE }T
T{  1.  1. D= -> TRUE  }T

T{ 0 -1 0 -1 D= -> TRUE  }T
T{ 0 -1 0  0 D= -> FALSE }T
T{ 0 -1 0  1 D= -> FALSE }T
T{ 0  0 0 -1 D= -> FALSE }T
T{ 0  0 0  0 D= -> TRUE  }T
T{ 0  0 0  1 D= -> FALSE }T
T{ 0  1 0 -1 D= -> FALSE }T
T{ 0  1 0  0 D= -> FALSE }T
T{ 0  1 0  1 D= -> TRUE  }T

T{ MAX-2INT MIN-2INT D= -> FALSE }T
T{ MAX-2INT 0. D= -> FALSE }T
T{ MAX-2INT MAX-2INT D= -> TRUE }T
T{ MAX-2INT HI-2INT  D= -> FALSE }T
T{ MAX-2INT MIN-2INT D= -> FALSE }T
T{ MIN-2INT MIN-2INT D= -> TRUE }T
T{ MIN-2INT LO-2INT  D=  -> FALSE }T
T{ MIN-2INT MAX-2INT D= -> FALSE }T

  \ ===========================================================

TESTING 2LITERAL 2VARIABLE

T{ : CD3 [ MAX-2INT ] 2LITERAL ; -> }T
T{ CD3 -> MAX-2INT }T
T{ 2VARIABLE 2V1 -> }T
T{ 0. 2V1 2! -> }T
T{ 2V1 2@ -> 0. }T
T{ -1 -2 2V1 2! -> }T
T{ 2V1 2@ -> -1 -2 }T
T{ : CD4 2VARIABLE ; -> }T
T{ CD4 2V2 -> }T
T{ : CD5 2V2 2! ; -> }T
T{ -2 -1 CD5 -> }T
T{ 2V2 2@ -> -2 -1 }T
T{ 2VARIABLE 2V3 IMMEDIATE 5 6 2V3 2! -> }T
T{ 2V3 2@ -> 5 6 }T
T{ : CD7 2V3 [ 2@ ] 2LITERAL ; CD7 -> 5 6 }T
T{ : CD8 [ 6 7 ] 2V3 [ 2! ] ; 2V3 2@ -> 6 7 }T

  \ ===========================================================

TESTING DMAX DMIN

T{  1.  2. DMAX -> 2. }T
T{  1.  0. DMAX -> 1. }T
T{  1. -1. DMAX -> 1. }T
T{  1.  1. DMAX -> 1. }T
T{  0.  1. DMAX -> 1. }T
T{  0. -1. DMAX -> 0. }T
T{ -1.  1. DMAX -> 1. }T
T{ -1. -2. DMAX -> -1. }T

T{ MAX-2INT HI-2INT  DMAX -> MAX-2INT }T
T{ MAX-2INT MIN-2INT DMAX -> MAX-2INT }T
T{ MIN-2INT MAX-2INT DMAX -> MAX-2INT }T
T{ MIN-2INT LO-2INT  DMAX -> LO-2INT  }T

T{ MAX-2INT  1. DMAX -> MAX-2INT }T
T{ MAX-2INT -1. DMAX -> MAX-2INT }T
T{ MIN-2INT  1. DMAX ->  1. }T
T{ MIN-2INT -1. DMAX -> -1. }T


T{  1.  2. DMIN ->  1. }T
T{  1.  0. DMIN ->  0. }T
T{  1. -1. DMIN -> -1. }T
T{  1.  1. DMIN ->  1. }T
T{  0.  1. DMIN ->  0. }T
T{  0. -1. DMIN -> -1. }T
T{ -1.  1. DMIN -> -1. }T
T{ -1. -2. DMIN -> -2. }T

T{ MAX-2INT HI-2INT  DMIN -> HI-2INT  }T
T{ MAX-2INT MIN-2INT DMIN -> MIN-2INT }T
T{ MIN-2INT MAX-2INT DMIN -> MIN-2INT }T
T{ MIN-2INT LO-2INT  DMIN -> MIN-2INT }T

T{ MAX-2INT  1. DMIN ->  1. }T
T{ MAX-2INT -1. DMIN -> -1. }T
T{ MIN-2INT  1. DMIN -> MIN-2INT }T
T{ MIN-2INT -1. DMIN -> MIN-2INT }T

  \ ===========================================================

TESTING D>S DABS

T{  1234  0 D>S ->  1234 }T
T{ -1234 -1 D>S -> -1234 }T
T{ MAX-INTD  0 D>S -> MAX-INTD }T
T{ MIN-INTD -1 D>S -> MIN-INTD }T

T{  1. DABS -> 1. }T
T{ -1. DABS -> 1. }T
T{ MAX-2INT DABS -> MAX-2INT }T
T{ MIN-2INT 1. D+ DABS -> MAX-2INT }T

  \ ===========================================================

TESTING M+ M*/

T{ HI-2INT   1 M+ -> HI-2INT   1. D+ }T
T{ MAX-2INT -1 M+ -> MAX-2INT -1. D+ }T
T{ MIN-2INT  1 M+ -> MIN-2INT  1. D+ }T
T{ LO-2INT  -1 M+ -> LO-2INT  -1. D+ }T

  \ To correct the result if the division is floored, only used
  \ when necessary i.e. negative quotient and remainder <> 0.

: ?FLOORED [ -3 2 / -2 = ] LITERAL IF 1. D- THEN ;

T{  5.  7 11 M*/ ->  3. }T
T{  5. -7 11 M*/ -> -3. ?FLOORED }T    \ FLOORED -4.
T{ -5.  7 11 M*/ -> -3. ?FLOORED }T    \ FLOORED -4.
T{ -5. -7 11 M*/ ->  3. }T
T{ MAX-2INT  8 16 M*/ -> HI-2INT }T

T{ MAX-2INT -8 16 M*/ -> HI-2INT DNEGATE ?FLOORED }T
  \ FLOORED SUBTRACT 1

T{ MIN-2INT  8 16 M*/ -> LO-2INT }T
T{ MIN-2INT -8 16 M*/ -> LO-2INT DNEGATE }T
T{ MAX-2INT MAX-INTD MAX-INTD M*/ -> MAX-2INT }T
T{ MAX-2INT MAX-INTD 2/ MAX-INTD M*/
   -> MAX-INTD 1- HI-2INT NIP }T
T{ MIN-2INT LO-2INT NIP DUP NEGATE M*/ -> MIN-2INT }T
T{ MIN-2INT LO-2INT NIP 1- MAX-INTD M*/
   -> MIN-INTD 3 + HI-2INT NIP 2 + }T
T{ MAX-2INT LO-2INT NIP DUP NEGATE M*/ -> MAX-2INT DNEGATE }T
T{ MIN-2INT MAX-INTD DUP M*/ -> MIN-2INT }T

  \ ===========================================================

TESTING D. D.R

  \ Create some large double numbers
MAX-2INT 71 73 M*/ 2CONSTANT DBL1
MIN-2INT 73 79 M*/ 2CONSTANT DBL2

: D>ASCII ( D -- CADDR U )
   DUP >R <# DABS #S R> SIGN #>    ( -- CADDR1 U )
   HERE SWAP 2DUP 2>R CHARS DUP ALLOT MOVE 2R>
;

DBL1 D>ASCII 2CONSTANT "DBL1"
DBL2 D>ASCII 2CONSTANT "DBL2"

: DOUBLEOUTPUT
   CR ." You should see lines duplicated:" CR
   5 SPACES "DBL1" TYPE CR
   5 SPACES DBL1 D. CR
   8 SPACES "DBL1" DUP >R TYPE CR
   5 SPACES DBL1 R> 3 + D.R CR
   5 SPACES "DBL2" TYPE CR
   5 SPACES DBL2 D. CR
   10 SPACES "DBL2" DUP >R TYPE CR
   5 SPACES DBL2 R> 5 + D.R CR
;

T{ DOUBLEOUTPUT -> }T

  \ ===========================================================

TESTING 2ROT DU< (Double Number extension words)

T{ 1. 2. 3. 2ROT -> 2. 3. 1. }T
T{ MAX-2INT MIN-2INT 1. 2ROT -> MIN-2INT 1. MAX-2INT }T

T{  1.  1. DU< -> FALSE }T
T{  1. -1. DU< -> TRUE  }T
T{ -1.  1. DU< -> FALSE }T
T{ -1. -2. DU< -> FALSE }T
T{ 0 1   1. DU< -> FALSE }T
T{ 1.  0 1  DU< -> TRUE  }T
T{ 0 -1 1 -2 DU< -> FALSE }T
T{ 1 -2 0 -1 DU< -> TRUE  }T

T{ MAX-2INT HI-2INT  DU< -> FALSE }T
T{ HI-2INT  MAX-2INT DU< -> TRUE  }T
T{ MAX-2INT MIN-2INT DU< -> TRUE }T
T{ MIN-2INT MAX-2INT DU< -> FALSE }T
T{ MIN-2INT LO-2INT  DU< -> TRUE }T

  \ ===========================================================

TESTING 2VALUE

T{ 1111 2222 2VALUE 2VAL -> }T
T{ 2VAL -> 1111 2222 }T
T{ 3333 4444 TO 2VAL -> }T
T{ 2VAL -> 3333 4444 }T
T{ : TO-2VAL TO 2VAL ; 5555 6666 TO-2VAL -> }T
T{ 2VAL -> 5555 6666 }T

  \ ===========================================================

DOUBLE-ERRORS SET-ERROR-COUNT

CR .( End of Double-Number word tests) CR

  \ To collect and report on the number of errors resulting
  \ from running the ANS Forth and Forth 2012 test programs

  \ This program was written by Gerry Jackson in 2015, and is
  \ in the public domain - it can be distributed and/or
  \ modified in any way but please retain this notice.

  \ This program is distributed in the hope that it will be
  \ useful, but WITHOUT ANY WARRANTY; without even the implied
  \ warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
  \ PURPOSE.

  \ ===========================================================

  \ This file is INCLUDED after Core tests are complete and
  \ only uses Core words already tested. The purpose of this
  \ file is to count errors in test results and present them as
  \ a summary at the end of the tests.

DECIMAL

VARIABLE TOTAL-ERRORS

: ERROR-COUNT ( "name" n1 -- n2 ) \ n2 = n1 + 1cell
   CREATE  DUP , CELL+
   DOES> ( -- offset ) @     \ offset in address units
;

0     \ Offset into ERRORS[] array
ERROR-COUNT CORE-ERRORS          ERROR-COUNT CORE-EXT-ERRORS
ERROR-COUNT DOUBLE-ERRORS        ERROR-COUNT EXCEPTION-ERRORS
ERROR-COUNT FACILITY-ERRORS      ERROR-COUNT FILE-ERRORS
ERROR-COUNT LOCALS-ERRORS        ERROR-COUNT MEMORY-ERRORS
ERROR-COUNT SEARCHORDER-ERRORS   ERROR-COUNT STRING-ERRORS
ERROR-COUNT TOOLS-ERRORS         ERROR-COUNT BLOCK-ERRORS
CREATE ERRORS[] DUP ALLOT CONSTANT #ERROR-COUNTS

  \ `SET-ERROR-COUNT` called at the end of each test file with
  \ its own offset into the `ERRORS[]` array. `#ERRORS` is in
  \ files <tester.fr> and <ttester.fs>.

: SET-ERROR-COUNT ( offset -- )
   #ERRORS @ SWAP ERRORS[] + !
   #ERRORS @ TOTAL-ERRORS +!
   0 #ERRORS !
;

: INIT-ERRORS ( -- )
   ERRORS[] #ERROR-COUNTS OVER + SWAP DO -1 I ! 1 CELLS +LOOP
   0 TOTAL-ERRORS !
   CORE-ERRORS SET-ERROR-COUNT
;

INIT-ERRORS

  \ Report summary of errors

25 CONSTANT MARGIN

: SHOW-ERROR-LINE ( n caddr u -- )
   CR SWAP OVER TYPE MARGIN - ABS >R
   DUP -1 = IF DROP R> 1- SPACES ." -" ELSE
   R> .R THEN
;

: SHOW-ERROR-COUNT ( caddr u offset -- )
   ERRORS[] + @ ROT ROT SHOW-ERROR-LINE
;

: HLINE ( -- ) CR ." ---------------------------"  ;

: REPORT-ERRORS
   HLINE
   CR 8 SPACES ." Error Report"
   CR ." Word Set" 13 SPACES ." Errors"
   HLINE
   S" Core" CORE-ERRORS SHOW-ERROR-COUNT
   S" Core extension" CORE-EXT-ERRORS SHOW-ERROR-COUNT
   S" Block" BLOCK-ERRORS SHOW-ERROR-COUNT
   S" Double number" DOUBLE-ERRORS SHOW-ERROR-COUNT
   S" Exception" EXCEPTION-ERRORS SHOW-ERROR-COUNT
   S" Facility" FACILITY-ERRORS SHOW-ERROR-COUNT
   S" File-access" FILE-ERRORS SHOW-ERROR-COUNT
   S" Locals"    LOCALS-ERRORS SHOW-ERROR-COUNT
   S" Memory-allocation" MEMORY-ERRORS SHOW-ERROR-COUNT
   S" Programming-tools" TOOLS-ERRORS SHOW-ERROR-COUNT
   S" Search-order" SEARCHORDER-ERRORS SHOW-ERROR-COUNT
   S" String" STRING-ERRORS SHOW-ERROR-COUNT
   HLINE
   TOTAL-ERRORS @ S" Total" SHOW-ERROR-LINE
   HLINE CR CR ;

  \ To test the ANS Forth Exception word set and extension
  \ words

  \ This program was written by Gerry Jackson in 2006, with
  \ contributions from others where indicated, and is in the
  \ public domain - it can be distributed and/or modified in
  \ any way but please retain this notice.

  \ This program is distributed in the hope that it will be
  \ useful, but WITHOUT ANY WARRANTY; without even the implied
  \ warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
  \ PURPOSE.

  \ The tests are not claimed to be comprehensive or correct

  \ ===========================================================
  \ Version 0.13 13 Nov 2015 C6 rewritten to avoid use of CASE etc and hence
  \              dependence on the Core extension word set.
  \         0.4 1 April 2012  Tests placed in the public domain.
  \         0.3 6 March 2009 { and } replaced with T{ and }T
  \         0.2 20 April 2007 ANS Forth words changed to upper case
  \         0.1 Oct 2006 First version released

  \ ===========================================================
  \ The tests are based on John Hayes test program for the core word set
\
  \ Words tested in this file are:
  \     CATCH THROW ABORT ABORT"
\
  \ ===========================================================
  \ Assumptions and dependencies:
  \     - the forth system under test throws an exception with throw
  \       code -13 for a word not found by the text interpreter. The
  \       undefined word used is $$qweqweqwert$$,  if this happens to be
  \       a valid word in your system change the definition of t7 below
  \     - tester.fr (or ttester.fs), errorreport.fth and utilities.fth have been
  \       included prior to this file
  \     - the Core word set available and tested
  \     - CASE, OF, ENDOF and ENDCASE from the core extension wordset
  \       are present and work correctly
  \ ===========================================================

TESTING CATCH THROW

DECIMAL

: T1 9 ;
: C1 1 2 3 ['] T1 CATCH ;
T{ C1 -> 1 2 3 9 0 }T         \ No THROW executed

: T2 8 0 THROW ;
: C2 1 2 ['] T2 CATCH ;
T{ C2 -> 1 2 8 0 }T            \ 0 THROW does nothing

: T3 7 8 9 99 THROW ;
: C3 1 2 ['] T3 CATCH ;
T{ C3 -> 1 2 99 }T            \ Restores stack to CATCH depth

: T4 1- DUP 0> IF RECURSE ELSE 999 THROW -222 THEN ;
: C4 3 4 5 10 ['] T4 CATCH -111 ;
T{ C4 -> 3 4 5 0 999 -111 }T   \ Test return stack unwinding

: T5 2DROP 2DROP 9999 THROW ;

: C5 1 2 3 4 ['] T5 CATCH DEPTH >R DROP 2DROP 2DROP R> ;
  \ Test depth restored correctly after stack has been emptied.

T{ C5 -> 5 }T

  \ ===========================================================

TESTING ABORT ABORT"

-1  CONSTANT EXC_ABORT
-2  CONSTANT EXC_ABORT"
-13 CONSTANT EXC_UNDEF
: T6 ABORT ;

  \ The 77 in `T10` is necessary for the second `ABORT"` test
  \ as the data stack is restored to a depth of 2 when `THROW`
  \ is executed. The 77 ensures the top of stack value is known
  \ for the results check.

: T10 77 SWAP ABORT" This should not be displayed" ;
: C6 CATCH
   >R   R@ EXC_ABORT  = IF 11
   ELSE R@ EXC_ABORT" = IF 12
   ELSE R@ EXC_UNDEF  = IF 13
   THEN THEN THEN R> DROP
;

T{ 1 2 ' T6 C6  -> 1 2 11 }T     \ Test that ABORT is caught
T{ 3 0 ' T10 C6 -> 3 77 }T       \ ABORT" does nothing
T{ 4 5 ' T10 C6 -> 4 77 12 }T    \ ABORT" caught, no message

  \ ===========================================================

TESTING a system generated exception

: T7 S" 333 $$QWEQWEQWERT$$ 334" EVALUATE 335 ;
: T8 S" 222 T7 223" EVALUATE 224 ;
: T9 S" 111 112 T8 113" EVALUATE 114 ;

T{ 6 7 ' T9 C6 3 -> 6 7 13 3 }T \ Test unlinking of sources

  \ ===========================================================

EXCEPTION-ERRORS SET-ERROR-COUNT

CR .( End of Exception word tests) CR

  \ To test part of the Forth 2012 Facility word set

  \ This program was written by Gerry Jackson in 2015, with
  \ contributions from others where indicated, and is in the
  \ public domain - it can be distributed and/or modified in
  \ any way but please retain this notice.

  \ This program is distributed in the hope that it will be
  \ useful, but WITHOUT ANY WARRANTY; without even the implied
  \ warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
  \ PURPOSE.

  \ The tests are not claimed to be comprehensive or correct

  \ ===========================================================
  \ Version 0.13 Assumptions and dependencies added
  \         0.11 25 April 2015 Added tests for BEGIN-STRUCTURE END-STRUCTURE +FIELD
  \              FIELD: CFIELD:
  \ ===========================================================
  \ The tests are based on John Hayes test program for the core word set

  \ Words tested in this file are: +FIELD BEGIN-STRUCTURE CFIELD: END-STRUCTURE
  \      FIELD:

  \ ===========================================================
  \ Assumptions and dependencies:
  \     - tester.fr (or ttester.fs), errorreport.fth and utilities.fth have been
  \       included prior to this file
  \     - the Core word set is available and tested
  \ ===========================================================

TESTING Facility words

DECIMAL
  \ ===========================================================

TESTING BEGIN-STRUCTURE END-STRUCTURE +FIELD

T{ BEGIN-STRUCTURE STRCT1
   END-STRUCTURE   -> }T
T{ STRCT1 -> 0 }T

T{ BEGIN-STRUCTURE STRCT2
      1 CHARS +FIELD F21
      2 CHARS +FIELD F22
      0 +FIELD F23
      1 CELLS +FIELD F24
   END-STRUCTURE   -> }T

T{ STRCT2 -> 3 chars 1 cells + }T   \ +FIELD doesn't align
T{ 0 F21 -> 0 }T
T{ 0 F22 -> 1 }T
T{ 0 F23 -> 3 }T
T{ 0 F24 -> 3 }T
T{ 5 F23 -> 8 }T

T{ CREATE S21 STRCT2 ALLOT -> }T
T{ 11 S21 F21 C! -> }T
T{ 22 S21 F22 C! -> }T
T{ 33 S21 F23 C! -> }T
T{ S21 F23 C@ -> 33 }T
T{ 44 S21 F24 C! -> }T
T{ S21 F21 C@ -> 11 }T
T{ S21 F22 C@ -> 22 }T
T{ S21 F23 C@ -> 44 }T
T{ S21 F24 C@ -> 44 }T

T{ CREATE S22 STRCT2 ALLOT -> }T
T{ 55 S22 F21 C! -> }T
T{ 66 S22 F22 C! -> }T
T{ S21 F21 C@ -> 11 }T
T{ S21 F22 C@ -> 22 }T
T{ S22 F21 C@ -> 55 }T
T{ S22 F22 C@ -> 66 }T

TESTING FIELD: CFIELD:

T{ BEGIN-STRUCTURE STRCT3
      FIELD:  F31
      FIELD:  F32
      CFIELD: CF31
      CFIELD: CF32
      CFIELD: CF33
      FIELD:  F33
   END-STRUCTURE -> }T

T{ 0 F31  CELL+ -> 0 F32  }T
T{ 0 CF31 CHAR+ -> 0 CF32 }T
T{ 0 CF32 CHAR+ -> 0 CF33 }T
T{ 0 CF33 CHAR+ ALIGNED -> 0 F33 }T
T{ 0 F33 ALIGNED -> 0 F33 }T


T{ CREATE S31 STRCT3 ALLOT -> }T
T{ 1 S31 F31   ! -> }T
T{ 2 S31 F32   ! -> }T
T{ 3 S31 CF31 C! -> }T
T{ 4 S31 CF32 C! -> }T
T{ 5 S31 F33   ! -> }T
T{ S31 F31   @ -> 1 }T
T{ S31 F32   @ -> 2 }T
T{ S31 CF31 C@ -> 3 }T
T{ S31 CF32 C@ -> 4 }T
T{ S31 F33   @ -> 5 }T

TESTING Nested structures

T{ BEGIN-STRUCTURE STRCT4
      STRCT2 +FIELD F41
      ALIGNED STRCT3 +FIELD F42
      3 +FIELD F43
      STRCT2 +FIELD F44
   END-STRUCTURE        -> }T
T{ STRCT4 -> STRCT2 ALIGNED STRCT3 + 3 + STRCT2 + }T

T{ CREATE S41 STRCT4 ALLOT -> }T
T{ 21 S41 F41 F21  C! -> }T
T{ 22 S41 F41 F22  C! -> }T
T{ 23 S41 F41 F23  C! -> }T
T{ 24 S41 F42 F31   ! -> }T
T{ 25 S41 F42 F32   ! -> }T
T{ 26 S41 F42 CF31 C! -> }T
T{ 27 S41 F42 CF32 C! -> }T
T{ 28 S41 F42 CF33 C! -> }T
T{ 29 S41 F42 F33   ! -> }T
T{ 30 S41 F44 F21  C! -> }T
T{ 31 S41 F44 F22  C! -> }T
T{ 32 S41 F44 F23  C! -> }T

T{ S41 F41 F21  C@ -> 21 }T
T{ S41 F41 F22  C@ -> 22 }T
T{ S41 F41 F23  C@ -> 23 }T
T{ S41 F42 F31   @ -> 24 }T
T{ S41 F42 F32   @ -> 25 }T
T{ S41 F42 CF31 C@ -> 26 }T
T{ S41 F42 CF32 C@ -> 27 }T
T{ S41 F42 CF33 C@ -> 28 }T
T{ S41 F42 F33   @ -> 29 }T
T{ S41 F44 F21  C@ -> 30 }T
T{ S41 F44 F22  C@ -> 31 }T
T{ S41 F44 F23  C@ -> 32 }T

FACILITY-ERRORS SET-ERROR-COUNT

CR .( End of Facility word tests) CR

  \ ===========================================================

  \ To test the ANS File Access word set and extension words

  \ This program was written by Gerry Jackson in 2006, with
  \ contributions from others where indicated, and is in the
  \ public domain - it can be distributed and/or modified in
  \ any way but please retain this notice.

  \ This program is distributed in the hope that it will be
  \ useful, but WITHOUT ANY WARRANTY; without even the implied
  \ warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
  \ PURPOSE.

  \ The tests are not claimed to be comprehensive or correct

  \ ===========================================================
  \ Version 0.13 S" in interpretation mode tested.
  \              Added SAVE-INPUT RESTORE-INPUT REFILL in a file, (moved from
  \              coreexttest.fth).
  \              Calls to COMPARE replaced with S= (in utilities.fth)
  \         0.11 25 April 2015 S\" in interpretation mode test added
  \              REQUIRED REQUIRE INCLUDE tests added
  \              Two S" and/or S\" buffers availability tested
  \         0.5  1 April 2012  Tests placed in the public domain.
  \         0.4  22 March 2009 { and } replaced with T{ and }T
  \         0.3  20 April 2007  ANS Forth words changed to upper case.
  \              Removed directory test from the filenames.
  \         0.2  30 Oct 2006 updated following GForth tests to remove
  \              system dependency on file size, to allow for file
  \              buffering and to allow for PAD moving around.
  \         0.1  Oct 2006 First version released.

  \ ===========================================================
  \ The tests are based on John Hayes test program for the core word set
  \ and requires those files to have been loaded

  \ Words tested in this file are:
  \     ( BIN CLOSE-FILE CREATE-FILE DELETE-FILE FILE-POSITION FILE-SIZE
  \     OPEN-FILE R/O R/W READ-FILE READ-LINE REPOSITION-FILE RESIZE-FILE
  \     S" S\" SOURCE-ID W/O WRITE-FILE WRITE-LINE
  \     FILE-STATUS FLUSH-FILE RENAME-FILE SAVE-INPUT RESTORE-INPUT
  \     REFILL

  \ Words not tested:
  \     INCLUDED INCLUDE-FILE (as these will likely have been
  \     tested in the execution of the test files)
  \ ===========================================================
  \ Assumptions, dependencies and notes:
  \     - tester.fr (or ttester.fs), errorreport.fth and utilities.fth have been
  \       included prior to this file
  \     - the Core word set is available and tested
  \     - These tests create files in the current directory, if all goes
  \       well these will be deleted. If something fails they may not be
  \       deleted. If this is a problem ensure you set a suitable
  \       directory before running this test. There is no ANS standard
  \       way of doing this. Also be aware of the file names used below
  \       which are:  fatest1.txt, fatest2.txt and fatest3.txt
  \ ===========================================================

TESTING File Access word set

DECIMAL

  \ ===========================================================

TESTING CREATE-FILE CLOSE-FILE

: FN1 S" fatest1.txt" ;
VARIABLE FID1

T{ FN1 R/W CREATE-FILE SWAP FID1 ! -> 0 }T
T{ FID1 @ CLOSE-FILE -> 0 }T

  \ ===========================================================

TESTING OPEN-FILE W/O WRITE-LINE

: LINE1 S" Line 1" ;

T{ FN1 W/O OPEN-FILE SWAP FID1 ! -> 0 }T
T{ LINE1 FID1 @ WRITE-LINE -> 0 }T
T{ FID1 @ CLOSE-FILE -> 0 }T

  \ ===========================================================

TESTING R/O FILE-POSITION (simple)  READ-LINE

200 CONSTANT BSIZE
CREATE BUF BSIZE ALLOT
VARIABLE #CHARS

T{ FN1 R/O OPEN-FILE SWAP FID1 ! -> 0 }T
T{ FID1 @ FILE-POSITION -> 0. 0 }T
T{ BUF 100 FID1 @ READ-LINE ROT DUP #CHARS !
   -> TRUE 0 LINE1 SWAP DROP }T
T{ BUF #CHARS @ LINE1 S= -> TRUE }T
T{ FID1 @ CLOSE-FILE -> 0 }T

  \ Additional test contributed by Helmut Eller

  \ Test with buffer shorter than the line including zero
  \ length buffer.

T{ FN1 R/O OPEN-FILE SWAP FID1 ! -> 0 }T
T{ FID1 @ FILE-POSITION -> 0. 0 }T
T{ BUF 0 FID1 @ READ-LINE ROT DUP #CHARS ! -> TRUE 0 0 }T
T{ BUF 3 FID1 @ READ-LINE ROT DUP #CHARS ! -> TRUE 0 3 }T
T{ BUF #CHARS @ LINE1 DROP 3 S= -> TRUE }T
T{ BUF 100 FID1 @ READ-LINE ROT DUP #CHARS !
   -> TRUE 0 LINE1 NIP 3 - }T
T{ BUF #CHARS @ LINE1 3 /STRING S= -> TRUE }T
T{ FID1 @ CLOSE-FILE -> 0 }T

  \ Additional test contributed by Helmut Eller
  \ Test with buffer exactly as long as the line.
T{ FN1 R/O OPEN-FILE SWAP FID1 ! -> 0 }T
T{ FID1 @ FILE-POSITION -> 0. 0 }T
T{ BUF LINE1 NIP FID1 @ READ-LINE ROT DUP #CHARS !
   -> TRUE 0 LINE1 NIP }T
T{ BUF #CHARS @ LINE1 S= -> TRUE }T
T{ FID1 @ CLOSE-FILE -> 0 }T

  \ ===========================================================

TESTING S" in interpretation mode
TESTING (compile mode tested in Core tests)

T{ S" abcdef" $" abcdef" S= -> TRUE }T
T{ S" " $" " S= -> TRUE }T
T{ S" ghi"$" ghi" S= -> TRUE }T

  \ ===========================================================

TESTING R/W WRITE-FILE REPOSITION-FILE
TESTING READ-FILE FILE-POSITION S"

: LINE2 S" Line 2 blah blah blah" ;
: RL1 BUF 100 FID1 @ READ-LINE ;
2VARIABLE FP

T{ FN1 R/W OPEN-FILE SWAP FID1 ! -> 0 }T
T{ FID1 @ FILE-SIZE DROP FID1 @ REPOSITION-FILE -> 0 }T
T{ FID1 @ FILE-SIZE -> FID1 @ FILE-POSITION }T
T{ LINE2 FID1 @ WRITE-FILE -> 0 }T
T{ 10. FID1 @ REPOSITION-FILE -> 0 }T
T{ FID1 @ FILE-POSITION -> 10. 0 }T
T{ 0. FID1 @ REPOSITION-FILE -> 0 }T
T{ RL1 -> LINE1 SWAP DROP TRUE 0 }T
T{ RL1 ROT DUP #CHARS ! -> TRUE 0 LINE2 SWAP DROP }T
T{ BUF #CHARS @ LINE2 S= -> TRUE }T
T{ RL1 -> 0 FALSE 0 }T
T{ FID1 @ FILE-POSITION ROT ROT FP 2! -> 0 }T
T{ FP 2@ FID1 @ FILE-SIZE DROP D= -> TRUE }T
T{ S" " FID1 @ WRITE-LINE -> 0 }T
T{ S" " FID1 @ WRITE-LINE -> 0 }T
T{ FP 2@ FID1 @ REPOSITION-FILE -> 0 }T
T{ RL1 -> 0 TRUE 0 }T
T{ RL1 -> 0 TRUE 0 }T
T{ RL1 -> 0 FALSE 0 }T
T{ FID1 @ CLOSE-FILE -> 0 }T

  \ ===========================================================

TESTING BIN READ-FILE FILE-SIZE

: CBUF BUF BSIZE 0 FILL ;
: FN2 S" FATEST2.TXT" ;
VARIABLE FID2
: SETPAD PAD 50 0 DO I OVER C! CHAR+ LOOP DROP ;

SETPAD
  \ If anything else is defined setpad must be called again as
  \ pad may move.

T{ FN2 R/W BIN CREATE-FILE SWAP FID2 ! -> 0 }T
T{ PAD 50 FID2 @ WRITE-FILE FID2 @ FLUSH-FILE -> 0 0 }T
T{ FID2 @ FILE-SIZE -> 50. 0 }T
T{ 0. FID2 @ REPOSITION-FILE -> 0 }T
T{ CBUF BUF 29 FID2 @ READ-FILE -> 29 0 }T
T{ PAD 29 BUF 29 S= -> TRUE }T
T{ PAD 30 BUF 30 S= -> FALSE }T
T{ CBUF BUF 29 FID2 @ READ-FILE -> 21 0 }T
T{ PAD 29 + 21 BUF 21 S= -> TRUE }T
T{ FID2 @ FILE-SIZE DROP FID2 @ FILE-POSITION DROP D=
   -> TRUE }T
T{ BUF 10 FID2 @ READ-FILE -> 0 0 }T
T{ FID2 @ CLOSE-FILE -> 0 }T

  \ ===========================================================

TESTING RESIZE-FILE

T{ FN2 R/W BIN OPEN-FILE SWAP FID2 ! -> 0 }T
T{ 37. FID2 @ RESIZE-FILE -> 0 }T
T{ FID2 @ FILE-SIZE -> 37. 0 }T
T{ 0. FID2 @ REPOSITION-FILE -> 0 }T
T{ CBUF BUF 100 FID2 @ READ-FILE -> 37 0 }T
T{ PAD 37 BUF 37 S= -> TRUE }T
T{ PAD 38 BUF 38 S= -> FALSE }T
T{ 500. FID2 @ RESIZE-FILE -> 0 }T
T{ FID2 @ FILE-SIZE -> 500. 0 }T
T{ 0. FID2 @ REPOSITION-FILE -> 0 }T
T{ CBUF BUF 100 FID2 @ READ-FILE -> 100 0 }T
T{ PAD 37 BUF 37 S= -> TRUE }T
T{ FID2 @ CLOSE-FILE -> 0 }T

  \ ===========================================================

TESTING DELETE-FILE

T{ FN2 DELETE-FILE -> 0 }T
T{ FN2 R/W BIN OPEN-FILE SWAP DROP 0= -> FALSE }T
T{ FN2 DELETE-FILE 0= -> FALSE }T

  \ ===========================================================

TESTING multi-line ( comments

T{ ( 1 2 3
4 5 6
7 8 9 ) 11 22 33 -> 11 22 33 }T

  \ ===========================================================

TESTING SOURCE-ID (can only test it does not return 0 or -1)

T{ SOURCE-ID DUP -1 = SWAP 0= OR -> FALSE }T

  \ ===========================================================

TESTING RENAME-FILE FILE-STATUS FLUSH-FILE

: FN3 S" fatest3.txt" ;
: >END FID1 @ FILE-SIZE DROP FID1 @ REPOSITION-FILE ;


T{ FN3 DELETE-FILE DROP -> }T
T{ FN1 FN3 RENAME-FILE 0= -> TRUE }T
T{ FN1 FILE-STATUS SWAP DROP 0= -> FALSE }T

T{ FN3 FILE-STATUS SWAP DROP 0= -> TRUE }T
  \ Return value is undefined.

T{ FN3 R/W OPEN-FILE SWAP FID1 ! -> 0 }T
T{ >END -> 0 }T
T{ S" Final line" fid1 @ WRITE-LINE -> 0 }T

T{ FID1 @ FLUSH-FILE -> 0 }T
  \ Can only test FLUSH-FILE doesn't fail.

T{ FID1 @ CLOSE-FILE -> 0 }T

  \ Tidy the test folder
T{ fn3 DELETE-FILE DROP -> }T

  \ ===========================================================

TESTING REQUIRED REQUIRE INCLUDED
  \ Tests taken from Forth 2012 RfD

T{ 0
  S" required-helper1.fth" REQUIRED
  REQUIRE required-helper1.fth
  INCLUDE required-helper1.fth
  -> 2 }T

T{ 0
  INCLUDE required-helper2.fth
  S" required-helper2.fth" REQUIRED
  REQUIRE required-helper2.fth
  S" required-helper2.fth" INCLUDED
  -> 2 }T

  \ ===========================================================

TESTING S\" (Forth 2012 interpretation mode)

  \ S\" in compilation mode already tested in Core Extension
  \ tests.

T{ : SSQ11 S\" \a\b\e\f\l\m\q\r\t\v\x0F0\x1Fa\xaBx\z\"\\" ;
   -> }T

T{ S\" \a\b\e\f\l\m\q\r\t\v\x0F0\x1Fa\xaBx\z\"\\" SSQ11  S=
   -> TRUE }T

  \ ===========================================================

TESTING two buffers available for S" and/or S\" (Forth 2012)

: SSQ12 S" abcd" ;   : SSQ13 S" 1234" ;
T{ S" abcd"  S" 1234" SSQ13  S= ROT ROT SSQ12 S=
    -> TRUE TRUE }T
T{ S\" abcd" S\" 1234" SSQ13 S= ROT ROT SSQ12 S=
    -> TRUE TRUE }T
T{ S" abcd"  S\" 1234" SSQ13 S= ROT ROT SSQ12 S=
    -> TRUE TRUE }T
T{ S\" abcd" S" 1234" SSQ13  S= ROT ROT SSQ12 S=
    -> TRUE TRUE }T

  \ ===========================================================

TESTING SAVE-INPUT and RESTORE-INPUT with a file source

VARIABLE SIV -1 SIV !

: NEVEREXECUTED
   CR ." This should never be executed" CR
;

T{ 11111 SAVE-INPUT

SIV @

[?IF]
\?   0 SIV !
\?   RESTORE-INPUT
\?   NEVEREXECUTED
\?   33333
[?ELSE]

\? TESTING the -[ELSE]- part is executed
\? 22222

[?THEN]

   -> 11111 0 22222 }T   \ 0 comes from RESTORE-INPUT

TESTING nested SAVE-INPUT, RESTORE-INPUT and REFILL from a file

: READ_A_LINE
   REFILL 0=
   ABORT" REFILL FAILED"
;

0 SI_INC !

CREATE 2RES -1 , -1 ,
  \ Don't use `2VARIABLE` from Double number word set.

: SI2
   READ_A_LINE
   READ_A_LINE
   SAVE-INPUT
   READ_A_LINE
   READ_A_LINE
   S$ EVALUATE 2RES 2!
   RESTORE-INPUT
;

  \ WARNING: do not delete or insert lines of text after si2 is
  \ called otherwise the next test will fail.

T{ SI2
33333               \ This line should be ignored
2RES 2@ 44444      \ RESTORE-INPUT should return to this line

55555

TESTING the nested results

 -> 0 0 2345 44444 55555 }T

  \ End of warning

  \ ===========================================================

FILE-ERRORS SET-ERROR-COUNT

CR .( End of File-Access word set tests) CR
  \ To test the ANS Forth and Forth 2012 Locals word set

  \ This program was written by Gerry Jackson in 2015 and is in
  \ the public domain - it can be distributed and/or modified
  \ in any way but please retain this notice.

  \ This program is distributed in the hope that it will be useful,
  \ but WITHOUT ANY WARRANTY; without even the implied warranty of
  \ MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

  \ The tests are not claimed to be comprehensive or correct

  \ ===========================================================
  \ Version 0.13 13 Nov 2015 Priority of locals tests made conditional on the
  \              the required search-order words being available
  \         0.11 25 April 2015 Initial release

  \ ===========================================================
  \ The tests are based on John Hayes test program for the core word set
  \ and requires those files to have been loaded

  \ Words tested in this file are:
  \     {: TO (LOCAL)

  \ Words not tested:
  \     LOCALS|  (designated obsolescent in Forth 2012)
  \ ===========================================================
  \ Assumptions, dependencies and notes:
  \     - tester.fr (or ttester.fs), errorreport.fth and utilities.fth have been
  \       included prior to this file
  \     - the Core word set is available and tested
  \     - some tests at the end require the following words from the Search-Order
  \       word set WORDLIST GET-CURRENT SET-CURRENT GET-ORDER SET-ORDER PREVIOUS.
  \       If any these are not available the tests will be ignored.
  \ ===========================================================

TESTING Locals word set

DECIMAL

  \ Syntax is : foo ... {: <args>* [| <vals>*] [-- <out>*] :} ... ;
  \ <arg>s are initialised from the data stack
  \ <val>s are uninitialised
  \ <out>s are ignored (treated as a comment)

TESTING null locals

T{ : LT0 {: :} ; 0 LT0 -> 0 }T
T{ : LT1 {: | :} ; 1 LT1 -> 1 }T
T{ : LT2 {: -- :} ; 2 LT2 -> 2 }T
T{ : LT3 {: | -- :} ; 3 LT3 -> 3 }T

TESTING <arg>s and TO <arg>

T{ : LT4 {: A :} ; 4 LT4 -> }T
T{ : LT5 {: A :} A ; 5 LT5 -> 5 }T
T{ : LT6 DEPTH {: A B :} DEPTH A B ; 6 LT6 -> 0 6 1 }T
T{ : LT7 {: A B :} B A ; 7 8 LT7 -> 8 7 }T
T{ : LT8 {: A B :} B A 11 TO A A B 12 TO B B A ; 9 10 LT8
   -> 10 9 11 10 12 11 }T
T{ : LT9 2DUP + {: A B C :} C B A ; 13 14 LT9 -> 27 14 13 }T

TESTING | <val>s and TO <val>s
T{ : LT10 {: A B | :} B 2* A + ; 15 16 LT10 -> 47 }T
T{ : LT11 {: A | B :} A 2* ; 17 18 LT11 -> 17 36 }T
T{ : LT12 {: A | B C :} 20 TO B A 21 TO A 22 TO C A C B ;
   19 LT12 -> 19 21 22 20 }T
T{ : LT13 {: | A :} ; 23 LT13 -> 23 }T
T{ : LT14 {: | A B :} 24 TO B 25 TO A A B ; 26 LT14
   -> 26 25 24 }T

TESTING -- ignores everything up to :}
T{ : LT15 {: -- DUP SWAP OVER :} DUP 28 SWAP OVER ;
   27 LT15 -> 27 28 27 28 }T
T{ : LT16 {: | A -- this should be ignored :} TO A A + ;
   29 30 LT16 -> 59 }T
T{ : LT17 {: A -- A + 1 :} A 1+ ; 31 LT17 -> 32 }T
T{ : LT18 {: A | B -- 2A+B :} TO B A 2* B + ;
   33 34 LT18 -> 101 }T

TESTING local names supersede global names and numbers
T{ : LT19 {: DUP DROP | SWAP -- OVER :}
   35 TO SWAP SWAP DUP DROP OVER ; -> }T
T{ 36 37 38 LT19 -> 36 35 37 38 37 }T
T{ HEX : LT20 {: BEAD DEAF :} DEAF BEAD ; BEEF DEAD LT20
   -> DEAD BEEF }T DECIMAL

TESTING definition with locals calling another
TESTING with same name locals

T{ : LT21 {: A | B :} 39 TO B A B ; -> }T
T{ : LT22 {: B | A :} 40 TO A A 2* B 2* LT21 A B ; -> }T
T{ 41 LT22 -> 80 82 39 40 41 }T

TESTING locals in :NONAME & DOES>
T{ 42 43 :NONAME {: W X | Y -- DUP :} 44 TO Y X W Y DUP ;
   EXECUTE -> 43 42 44 44 }T
T{ : LT23 {: P Q :}
     CREATE P Q 2* + ,
     DOES> @ ROT ROT {: P Q | R -- DUP :} TO R Q R P ; -> }T
T{ 45 46 LT23 LT24 -> }T
T{ 47 48 LT24 -> 48 137 47 }T

TESTING locals in control structures
T{ : LT25 {: A B :} IF A ELSE B THEN ; -1 50 51 LT25 -> 50 }T
T{ 0 52 53 LT25 -> 53 }T
T{ : LT26 {: A :} 0 BEGIN A WHILE 2 + A 1- TO A REPEAT ; -> }T
T{ 5 LT26 -> 10 }T
T{ : LT27 {: A :} 0 BEGIN A 1- TO A 3 + A 0= UNTIL ; -> }T
T{ 5 LT27 -> 15 }T
T{ : LT28 1+ {: A B :} B A DO I LOOP ; 54 58 LT28
   -> 54 55 56 57 58 }T
T{ : LT29 {: I J :} 2 0 DO 5 3 DO I J LOOP LOOP ; -> }T
T{ 59 60 LT29 -> 59 60 59 60 59 60 59 60 }T


TESTING recursion with locals
T{ : LT30 {: A B :}
     A 0> IF A B * A 1- B 10 * RECURSE A B THEN ; -> }T
T{ 3 10 LT30 -> 30 200 1000 1 1000 2 100 3 10 }T

TESTING system supplies at least 16 locals

: LOC-ENVQ S" #LOCALS" ENVIRONMENT? ;
T{ LOC-ENVQ SWAP 15 > -> TRUE TRUE }T
T{ : LT31 {: A B C D E F G H I J K L M N O P :}
             P O N M L K J I H G F E D C B A ; -> }T
T{ 0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 LT31
          -> 15 14 13 12 11 10 9 8 7 6 5 4 3 2 1 0 }T

TESTING (LOCAL)
T{ : LOCAL BL WORD COUNT (LOCAL) ; IMMEDIATE -> }T
T{ : END-LOCALS 99 0 (LOCAL) ; IMMEDIATE     -> }T
: LT32 LOCAL A LOCAL B LOCAL C END-LOCALS A B C ;
  61 62 63 LT32 -> 63 62 61 }T

  \ ===========================================================

  \ These tests require Search-order words `WORDLIST`,
  \ `GET-CURRENT`, `SET-CURRENT`, `GET-ORDER`, `SET-ORDER`,
  \ `PREVIOUS`. If any of these are not available the following
  \ tests will be ignored except for the simple test.

[?UNDEF] WORDLIST \? [?UNDEF] GET-CURRENT
\? [?UNDEF] SET-CURRENT
\? [?UNDEF] GET-ORDER \? [?UNDEF] SET-ORDER

\? TESTING that local names are always found first & that they
\? TESTING are not available after the end of a definition.

  \ Simple test
: LT36 68 ;
T{ : LT37 {: LT36 :} LT36 ; 69 LT37 LT36 -> 69 68 }T

\? WORDLIST CONSTANT LTWL1
\? WORDLIST CONSTANT LTWL2
\? GET-CURRENT LTWL1 SET-CURRENT
\? : LT33 64 ;       \ Define LT33 in LTWL1 wordlist
\? LTWL2 SET-CURRENT
\? : LT33 65 ;       \ Redefine LT33 in LTWL2 wordlist
\? SET-CURRENT
\? : ALSO-LTWL ( wid -- ) >R GET-ORDER R> SWAP 1+ SET-ORDER ;
\? LTWL1 ALSO-LTWL   \ Add LTWL1 to search-order
\? T{ : LT34 {: LT33 :} LT33 ; 66 LT34 LT33 -> 66 64 }T
\? T{ : LT35 {: LT33 :}
\?    LT33 LTWL2 ALSO-LTWL LT33 PREVIOUS LT33 PREVIOUS LT33 ;
\?    -> }T

  \ If the next test fails the system may be left with `LTWL2`
  \ and/or `LTWL1` in the search order.

\? T{ 67 LT35 -> 67 67 67 67 }T
[?ELSE]
\? CR CR
\? .( Some search-order words not present)
\? .( - priority of Locals not fully tested)
\? CR
[?THEN]

  \ ===========================================================

LOCALS-ERRORS SET-ERROR-COUNT    \ For final error report

CR .( End of Locals word set tests. ) .S
  \ To test the ANS Forth Memory-Allocation word set

  \ This program was written by Gerry Jackson in 2006, with
  \ contributions from others where indicated, and is in the
  \ public domain - it can be distributed and/or modified in
  \ any way but please retain this notice.

  \ This program is distributed in the hope that it will be
  \ useful, but WITHOUT ANY WARRANTY; without even the implied
  \ warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
  \ PURPOSE.

  \ The tests are not claimed to be comprehensive or correct

  \ ===========================================================
  \ Version 0.11 25 April 2015 Now checks memory region is unchanged following a
  \              RESIZE. @ and ! in allocated memory.
  \         0.8 10 January 2013, Added CHARS and CHAR+ where necessary to correct
  \             the assumption that 1 CHARS = 1
  \         0.7 1 April 2012  Tests placed in the public domain.
  \         0.6 30 January 2011 CHECKMEM modified to work with ttester.fs
  \         0.5 30 November 2009 <FALSE> replaced with FALSE
  \         0.4 9 March 2009 Aligned test improved and data space pointer tested
  \         0.3 6 March 2009 { and } replaced with T{ and }T
  \         0.2 20 April 2007  ANS Forth words changed to upper case
  \         0.1 October 2006 First version released

  \ ===========================================================
  \ The tests are based on John Hayes test program for the core word set

  \ Words tested in this file are:
  \     ALLOCATE FREE RESIZE
  \
  \ ===========================================================
  \ Assumptions, dependencies and notes:
  \     - tester.fr (or ttester.fs), errorreport.fth and utilities.fth have been
  \       included prior to this file
  \     - the Core word set is available and tested
  \     - that 'addr -1 ALLOCATE' and 'addr -1 RESIZE' will return an error
  \     - testing FREE failing is not done as it is likely to crash the system
  \ ===========================================================

TESTING Memory-Allocation word set

DECIMAL

  \ ===========================================================

TESTING ALLOCATE FREE RESIZE

VARIABLE ADDR1
VARIABLE DATSP

HERE DATSP !
T{ 100 ALLOCATE SWAP ADDR1 ! -> 0 }T
T{ ADDR1 @ ALIGNED -> ADDR1 @ }T \ Test address is aligned
T{ HERE -> DATSP @ }T \ Check data space pointer is unchanged
T{ ADDR1 @ FREE -> 0 }T

T{ 99 ALLOCATE SWAP ADDR1 ! -> 0 }T
T{ ADDR1 @ ALIGNED -> ADDR1 @ }T
T{ ADDR1 @ FREE -> 0 }T

T{ 50 CHARS ALLOCATE SWAP ADDR1 ! -> 0 }T

: WRITEMEM 0 DO I 1+ OVER C! CHAR+ LOOP DROP ;   ( ad n -- )

  \ `CHECKMEM` is defined this way to maintain compatibility
  \ with both <tester.fr> and <ttester.fs> which differ in
  \ their definitions of `T{`.

: CHECKMEM ( ad n --- )
   0
   DO
      >R
      T{ R@ C@ -> R> I 1+ SWAP >R }T
      R> CHAR+
   LOOP
   DROP
;

ADDR1 @ 50 WRITEMEM ADDR1 @ 50 CHECKMEM

T{ ADDR1 @ 28 CHARS RESIZE SWAP ADDR1 ! -> 0 }T
ADDR1 @ 28 CHECKMEM

T{ ADDR1 @ 200 CHARS RESIZE SWAP ADDR1 ! -> 0 }T
ADDR1 @ 28 CHECKMEM

  \ ===========================================================

TESTING failure of RESIZE and ALLOCATE
TESTING (unlikely to be enough memory)

  \ This test relies on the previous test having passed

VARIABLE RESIZE-OK
T{ ADDR1 @ -1 CHARS RESIZE 0= DUP RESIZE-OK !
   -> ADDR1 @ FALSE }T

  \ Check unRESIZEd allocation is unchanged following RESIZE failure
: MEM?  RESIZE-OK @ 0= IF ADDR1 @ 28 CHECKMEM THEN ;
  \ Avoid using `[IF]`.
MEM?

T{ ADDR1 @ FREE -> 0 }T   \ Tidy up

T{ -1 ALLOCATE SWAP DROP 0= -> FALSE }T
  \ Memory allocate failed.

  \ ===========================================================

TESTING @ and ! work in ALLOCATEd memory
TESTING (provided by Peter Knaggs)

: WRITE-CELL-MEM ( ADDR N -- )
  1+ 1 DO I OVER ! CELL+ LOOP DROP
;

: CHECK-CELL-MEM ( ADDR N -- )
  1+ 1 DO
    I SWAP >R >R
    T{ R> ( I ) -> R@ ( ADDR ) @ }T
    R> CELL+
  LOOP DROP
;

  \ Cell based access to the heap

T{ 50 CELLS ALLOCATE SWAP ADDR1 ! -> 0 }T
ADDR1 @ 50 WRITE-CELL-MEM
ADDR1 @ 50 CHECK-CELL-MEM

MEMORY-ERRORS SET-ERROR-COUNT

CR .( End of Memory-Allocation word tests) CR

  \ ===========================================================

  \ ANS Forth tests - run all tests

  \ Adjust the file paths as appropriate to your system Select
  \ the appropriate test harness, either the simple <tester.fr>
  \ or the more complex <ttester.fs>.

CR .( Running forth-2012-test-suite) CR

S" prelimtest.fth" INCLUDED
S" tester.fr" INCLUDED
  \ S" ttester.fs" INCLUDED

S" core.fr" INCLUDED
S" coreplustest.fth" INCLUDED
S" utilities.fth" INCLUDED
S" errorreport.fth" INCLUDED
S" coreexttest.fth" INCLUDED
S" blocktest.fth" INCLUDED
S" doubletest.fth" INCLUDED
S" exceptiontest.fth" INCLUDED
S" facilitytest.fth" INCLUDED
S" filetest.fth" INCLUDED
S" localstest.fth" INCLUDED
S" memorytest.fth" INCLUDED
S" toolstest.fth" INCLUDED
S" searchordertest.fth" INCLUDED
S" stringtest.fth" INCLUDED
REPORT-ERRORS

CR .( Forth tests completed ) CR CR

  \ ===========================================================

  \ To test the ANS Forth search-order word set and search
  \ order extensions

  \ This program was written by Gerry Jackson in 2006, with
  \ contributions from others where indicated, and is in the
  \ public domain - it can be distributed and/or modified in
  \ any way but please retain this notice.

  \ This program is distributed in the hope that it will be
  \ useful, but WITHOUT ANY WARRANTY; without even the implied
  \ warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
  \ PURPOSE.

  \ The tests are not claimed to be comprehensive or correct

  \ ===========================================================
  \ Version 0.13  Replaced 2 instances of ?DO with DO
  \               Interpretive use of S" replaced by $" from utilities.fth
  \         0.10 3 August 2014 Name changes to remove redefinition messages
  \               "list" changed to "wordlist" in message for ORDER tests
  \         0.5 1 April 2012  Tests placed in the public domain.
  \         0.4 6 March 2009 { and } replaced with T{ and }T
  \         0.3 20 April 2007 ANS Forth words changed to upper case
  \         0.2 30 Oct 2006 updated following GForth tests to get
  \             initial search order into a known state
  \         0.1 Oct 2006 First version released

  \ ===========================================================
  \ The tests are based on John Hayes test program for the core word set

  \ Words tested in this file are:
  \     FORTH-WORDLIST GET-ORDER SET-ORDER ALSO ONLY FORTH GET-CURRENT
  \     SET-CURRENT DEFINITIONS PREVIOUS SEARCH-WORDLIST WORDLIST FIND
  \ Words not fully tested:
  \     ORDER only tests that it executes, display is implementation
  \           dependent and should be visually inspected

  \ ===========================================================
  \ Assumptions, dependencies and notes:
  \     - tester.fr (or ttester.fs), errorreport.fth and utilities.fth have been
  \       included prior to this file
  \     - the Core word set is available and tested
  \     - that ONLY FORTH DEFINITIONS will work at the start of the file
  \       to ensure the search order is in a known state
  \ ===========================================================

ONLY FORTH DEFINITIONS

TESTING Search-order word set

DECIMAL

VARIABLE WID1  VARIABLE WID2

  \ Only execute SAVE-ORDERLIST once
: SAVE-ORDERLIST ( widn ... wid1 n -> )
  DUP , ?DUP IF 0 DO , LOOP THEN ;

  \ ===========================================================

TESTING FORTH-WORDLIST GET-ORDER SET-ORDER

T{ FORTH-WORDLIST WID1 ! -> }T

CREATE ORDER-LIST

T{ GET-ORDER SAVE-ORDERLIST -> }T

: GET-ORDERLIST ( -- widn ... wid1 n )
   ORDER-LIST DUP @ CELLS DUP ( -- ad n )
   IF
      OVER + DO I @ -1 CELLS +LOOP
   ELSE
      2DROP 0
   THEN
;

T{ GET-ORDER OVER -> GET-ORDER WID1 @ }T
  \ Forth wordlist at top

T{ GET-ORDER SET-ORDER -> }T \ Effectively noop
T{ GET-ORDER -> GET-ORDERLIST }T \ Check nothing changed
T{ GET-ORDERLIST DROP GET-ORDERLIST 2* SET-ORDER -> }T
T{ GET-ORDER -> GET-ORDERLIST DROP GET-ORDERLIST 2* }T
T{ GET-ORDERLIST SET-ORDER GET-ORDER -> GET-ORDERLIST }T

  \ ===========================================================

TESTING ALSO ONLY FORTH

T{ ALSO GET-ORDER -> GET-ORDERLIST OVER SWAP 1+ }T

T{ ONLY FORTH GET-ORDER -> GET-ORDERLIST }T
  \ See assumptions above

  \ ===========================================================

TESTING GET-CURRENT SET-CURRENT WORDLIST (simple)

T{ GET-CURRENT -> WID1 @ }T        \ See assumptions above
T{ WORDLIST WID2 ! -> }T
T{ WID2 @ SET-CURRENT -> }T
T{ GET-CURRENT -> WID2 @ }T
T{ WID1 @ SET-CURRENT -> }T

  \ ===========================================================

TESTING minimum search order list
TESTING contains FORTH-WORDLIST and SET-ORDER

: SO1 SET-ORDER ;
  \ In case it is unavailable in the forth wordlist.

T{ ONLY FORTH-WORDLIST 1 SET-ORDER GET-ORDERLIST SO1 -> }T
T{ GET-ORDER -> GET-ORDERLIST }T

  \ ===========================================================

TESTING GET-ORDER SET-ORDER
TESTING with 0 and -1 number of wids argument

: SO2A GET-ORDER GET-ORDERLIST SET-ORDER ;
  \ To recover search order.

: SO2 0 SET-ORDER SO2A ;

T{ SO2 -> 0 }T \ 0 set-order leaves an empty search order

: SO3 -1 SET-ORDER SO2A ;
: SO4 ONLY SO2A ;

T{ SO3 -> SO4 }T       \ -1 SET-ORDER = ONLY

  \ ===========================================================

TESTING DEFINITIONS PREVIOUS

T{ ONLY FORTH DEFINITIONS -> }T
T{ GET-CURRENT -> FORTH-WORDLIST }T
T{ GET-ORDER WID2 @ SWAP 1+ SET-ORDER DEFINITIONS GET-CURRENT
   -> WID2 @ }T
T{ GET-ORDER -> GET-ORDERLIST WID2 @ SWAP 1+ }T
T{ PREVIOUS GET-ORDER -> GET-ORDERLIST }T
T{ DEFINITIONS GET-CURRENT -> FORTH-WORDLIST }T

  \ ===========================================================

TESTING SEARCH-WORDLIST WORDLIST FIND

ONLY FORTH DEFINITIONS
VARIABLE XT  ' DUP XT !
VARIABLE XTI ' ( XTI !    \ Immediate word

  \ $" is an equivalent to S" in interpreter mode. It is defined in the file
  \ utilities.fth and used to avoid relying on a File-Access word set extension

T{ $" DUP" WID1 @ SEARCH-WORDLIST -> XT  @ -1 }T
T{ $" ("   WID1 @ SEARCH-WORDLIST -> XTI @  1 }T
T{ $" DUP" WID2 @ SEARCH-WORDLIST ->        0 }T

: C"DUP" C" DUP" ;
: C"("  C" (" ;
: C"X" C" UNKNOWN WORD"  ;

T{ C"DUP" FIND -> XT  @ -1 }T
T{ C"("  FIND -> XTI @  1 }T
T{ C"X"   FIND -> C"X"   0 }T

  \ ===========================================================

TESTING new definitions are put into the correct wordlist

: ALSOWID2 ALSO GET-ORDER WID2 @ ROT DROP SWAP SET-ORDER ;
ALSOWID2
: W2 1234  ;
DEFINITIONS
: W2 -9876 ; IMMEDIATE

ONLY FORTH
T{ W2 -> 1234 }T
DEFINITIONS
T{ W2 -> 1234 }T
ALSOWID2
T{ W2 -> -9876 }T
DEFINITIONS
T{ W2 -> -9876 }T

ONLY FORTH DEFINITIONS

: SO5  DUP IF SWAP EXECUTE THEN ;

T{ $" W2" WID1 @ SEARCH-WORDLIST SO5 -> -1  1234 }T
T{ $" W2" WID2 @ SEARCH-WORDLIST SO5 ->  1 -9876 }T

: C"W2" C" W2" ;
T{ ALSOWID2 C"W2" FIND SO5 ->  1 -9876 }T
T{ PREVIOUS C"W2" FIND SO5 -> -1  1234 }T

  \ ===========================================================

TESTING ORDER
  \ Should display search order and compilation wordlist.

CR .( ONLY FORTH DEFINITIONS search order)
CR .( and compilation wordlist) CR
T{ ONLY FORTH DEFINITIONS ORDER -> }T

CR .( Plus another unnamed wordlist)
CR .( at the head of the search order) CR
T{ ALSOWID2 DEFINITIONS ORDER -> }T

  \ ===========================================================

SEARCHORDER-ERRORS SET-ERROR-COUNT

CR .( End of Search Order word tests) CR

ONLY FORTH DEFINITIONS
  \ Leave search order in the standard state.

  \ To test the ANS Forth String word set

  \ This program was written by Gerry Jackson in 2006, with
  \ contributions from others where indicated, and is in the
  \ public domain - it can be distributed and/or modified in
  \ any way but please retain this notice.

  \ This program is distributed in the hope that it will be
  \ useful, but WITHOUT ANY WARRANTY; without even the implied
  \ warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
  \ PURPOSE.

  \ The tests are not claimed to be comprehensive or correct

  \ ===========================================================
  \ Version 0.13 13 Nov 2015 Interpretive use of S" replaced by $" from
  \                          utilities.fth
  \         0.11 25 April 2015 Tests for REPLACES SUBSTITUTE UNESCAPE added
  \         0.6 1 April 2012 Tests placed in the public domain.
  \         0.5 29 April 2010 Added tests for SEARCH and COMPARE with
  \             all strings zero length (suggested by Krishna Myneni).
  \             SLITERAL test amended in line with comp.lang.forth
  \             discussion
  \         0.4 30 November 2009 <TRUE> and <FALSE> replaced with TRUE
  \             and FALSE
  \         0.3 6 March 2009 { and } replaced with T{ and }T
  \         0.2 20 April 2007 ANS Forth words changed to upper case
  \         0.1 Oct 2006 First version released

  \ ===========================================================
  \ The tests are based on John Hayes test program for the core word set
  \ and requires those files to have been loaded

  \ Words tested in this file are:
  \     -TRAILING /STRING BLANK CMOVE CMOVE> COMPARE SEARCH SLITERAL
  \     REPLACES SUBSTITUTE UNESCAPE
\
  \ ===========================================================
  \ Assumptions, dependencies and notes:
  \     - tester.fr (or ttester.fs), errorreport.fth and utilities.fth have been
  \       included prior to this file
  \     - the Core word set is available and tested
  \     - COMPARE is case sensitive
  \ ===========================================================

TESTING String word set

DECIMAL

T{ :  S1 S" abcdefghijklmnopqrstuvwxyz" ; -> }T
T{ :  S2 S" abc"   ; -> }T
T{ :  S3 S" jklmn" ; -> }T
T{ :  S4 S" z"     ; -> }T
T{ :  S5 S" mnoq"  ; -> }T
T{ :  S6 S" 12345" ; -> }T
T{ :  S7 S" "      ; -> }T
T{ :  S8 S" abc  " ; -> }T
T{ :  S9 S"      " ; -> }T
T{ : S10 S"    a " ; -> }T

  \ ===========================================================

TESTING -TRAILING

T{  S1 -TRAILING -> S1 }T
T{  S8 -TRAILING -> S8 2 - }T
T{  S7 -TRAILING -> S7 }T
T{  S9 -TRAILING -> S9 DROP 0 }T
T{ S10 -TRAILING -> S10 1- }T

  \ ===========================================================

TESTING /STRING

T{ S1  5 /STRING -> S1 SWAP 5 + SWAP 5 - }T
T{ S1 10 /STRING -4 /STRING -> S1 6 /STRING }T
T{ S1  0 /STRING -> S1 }T

  \ ===========================================================

TESTING SEARCH

T{ S1 S2 SEARCH -> S1 TRUE }T
T{ S1 S3 SEARCH -> S1  9 /STRING TRUE }T
T{ S1 S4 SEARCH -> S1 25 /STRING TRUE }T
T{ S1 S5 SEARCH -> S1 FALSE }T
T{ S1 S6 SEARCH -> S1 FALSE }T
T{ S1 S7 SEARCH -> S1 TRUE }T
T{ S7 PAD 0 SEARCH -> S7 TRUE }T

  \ ===========================================================

TESTING COMPARE

T{ S1 S1 COMPARE -> 0 }T
T{ S1 PAD SWAP CMOVE -> }T
T{ S1 PAD OVER COMPARE -> 0 }T
T{ S1 PAD 6 COMPARE -> 1 }T
T{ PAD 10 S1 COMPARE -> -1 }T
T{ S1 PAD 0 COMPARE -> 1 }T
T{ PAD 0 S1 COMPARE -> -1 }T
T{ S1 S6 COMPARE ->  1 }T
T{ S6 S1 COMPARE -> -1 }T
T{ S7 PAD 0 COMPARE -> 0 }T

T{ S1 $" abdde"  COMPARE -> -1 }T
T{ S1 $" abbde"  COMPARE ->  1 }T
T{ S1 $" abcdf"  COMPARE -> -1 }T
T{ S1 $" abcdee" COMPARE ->  1 }T

: S11 S" 0abc" ;
: S12 S" 0aBc" ;

T{ S11 S12  COMPARE -> 1 }T
T{ S12 S11  COMPARE -> -1 }T

  \ ===========================================================

TESTING CMOVE and CMOVE>

PAD 30 CHARS 0 FILL
T{ S1 PAD SWAP CMOVE -> }T
T{ S1 PAD S1 SWAP DROP COMPARE -> 0 }T
T{ S6 PAD 10 CHARS + SWAP CMOVE -> }T
T{ $" abcdefghij12345pqrstuvwxyz" PAD S1 SWAP DROP COMPARE
   -> 0 }T
T{ PAD 15 CHARS + PAD CHAR+ 6 CMOVE -> }T
T{ $" apqrstuhij12345pqrstuvwxyz" PAD 26 COMPARE -> 0 }T
T{ PAD PAD 3 CHARS + 7 CMOVE -> }T
T{ $" apqapqapqa12345pqrstuvwxyz" PAD 26 COMPARE -> 0 }T
T{ PAD PAD CHAR+ 10 CMOVE -> }T
T{ $" aaaaaaaaaaa2345pqrstuvwxyz" PAD 26 COMPARE -> 0 }T
T{ S7 PAD 14 CHARS + SWAP CMOVE -> }T
T{ $" aaaaaaaaaaa2345pqrstuvwxyz" PAD 26 COMPARE -> 0 }T

PAD 30 CHARS 0 FILL

T{ S1 PAD SWAP CMOVE> -> }T
T{ S1 PAD S1 SWAP DROP COMPARE -> 0 }T
T{ S6 PAD 10 CHARS + SWAP CMOVE> -> }T
T{ $" abcdefghij12345pqrstuvwxyz" PAD S1 SWAP DROP COMPARE
   -> 0 }T
T{ PAD 15 CHARS + PAD CHAR+ 6 CMOVE> -> }T
T{ $" apqrstuhij12345pqrstuvwxyz" PAD 26 COMPARE -> 0 }T
T{ PAD 13 CHARS + PAD 10 CHARS + 7 CMOVE> -> }T
T{ $" apqrstuhijtrstrstrstuvwxyz" PAD 26 COMPARE -> 0 }T
T{ PAD 12 CHARS + PAD 11 CHARS + 10 CMOVE> -> }T
T{ $" apqrstuhijtvvvvvvvvvvvwxyz" PAD 26 COMPARE -> 0 }T
T{ S7 PAD 14 CHARS + SWAP CMOVE> -> }T
T{ $" apqrstuhijtvvvvvvvvvvvwxyz" PAD 26 COMPARE -> 0 }T

  \ ===========================================================

TESTING BLANK

: S13 S" aaaaa      a" ;
  \ Don't move this down as it might corrupt `PAD`.

T{ PAD 25 CHAR a FILL -> }T
T{ PAD 5 CHARS + 6 BLANK -> }T
T{ PAD 12 S13 COMPARE -> 0 }T

  \ ===========================================================

TESTING SLITERAL

T{ HERE DUP S1 DUP ALLOT ROT SWAP CMOVE S1 SWAP DROP 2CONSTANT
   S1A -> }T
T{ : S14 [ S1A ] SLITERAL ; -> }T
T{ S1A S14 COMPARE -> 0 }T
T{ S1A DROP S14 DROP = -> FALSE }T

  \ ===========================================================

TESTING UNESCAPE

CREATE SUBBUF 48 CHARS ALLOT

  \ $CHECK AND $CHECKN return f = 0 if caddr1 = SUBBUF and
  \ string1 = string2

: $CHECK   ( caddr1 u1 caddr2 u2 -- f )
  2SWAP OVER SUBBUF <> >R COMPARE R> or ;
: $CHECKN ( caddr1 u1 n caddr2 u2 -- f n ) ROT >R $CHECK R> ;

T{ 123 SUBBUF C! $" " SUBBUF UNESCAPE SUBBUF 0 $CHECK
   -> FALSE }T
T{ SUBBUF C@ -> 123 }T
T{ $" unchanged" SUBBUF UNESCAPE $" unchanged" $CHECK
   -> FALSE }T
T{ $" %" SUBBUF UNESCAPE $" %%" $CHECK -> FALSE }T
T{ $" %%%" SUBBUF UNESCAPE $" %%%%%%" $CHECK -> FALSE }T
T{ $" abc%def" SUBBUF UNESCAPE $" abc%%def" $CHECK -> FALSE }T

T{ : TEST-UNESCAPE S" %abc%def%%ghi%" SUBBUF UNESCAPE ; -> }T
  \ Compile check.

T{ TEST-UNESCAPE $" %%abc%%def%%%%ghi%%" $CHECK -> FALSE }T

TESTING SUBSTITUTE REPLACES

T{ $" abcdef" SUBBUF 20 SUBSTITUTE $" abcdef" $CHECKN
   -> FALSE 0 }T \ Unchanged

T{ $" " SUBBUF 20 SUBSTITUTE $" " $CHECKN -> FALSE 0 }T
  \ Zero length string.

T{ $" %%" SUBBUF 20 SUBSTITUTE $" %" $CHECKN
   -> FALSE 0 }T \ %% --> %

T{ $" %%%%%%" SUBBUF 25 SUBSTITUTE $" %%%" $CHECKN
   -> FALSE 0 }T

T{ $" %%%%%%%" SUBBUF 25 SUBSTITUTE $" %%%%" $CHECKN
   -> FALSE 0 }T \ Odd no. %'s

: MAC1 S" mac1" ;  : MAC2 S" mac2" ;  : MAC3 S" mac3" ;

T{ $" wxyz" MAC1 REPLACES -> }T
T{ $" %mac1%" SUBBUF 20 SUBSTITUTE $" wxyz" $CHECKN
   -> FALSE 1 }T
T{ $" abc%mac1%d" SUBBUF 20 SUBSTITUTE $" abcwxyzd" $CHECKN
   -> FALSE 1 }T
T{ : SUBST SUBBUF 20 SUBSTITUTE ; -> }T   \ Check it compiles
T{ $" defg%mac1%hi" SUBST $" defgwxyzhi" $CHECKN -> FALSE 1 }T
T{ $" 12" MAC2 REPLACES -> }T
T{ $" %mac1%mac2" SUBBUF 20 SUBSTITUTE $" wxyzmac2" $CHECKN
   -> FALSE 1 }T
T{ $" abc %mac2% def%mac1%gh" SUBBUF 20 SUBSTITUTE
   $" abc 12 defwxyzgh" $CHECKN
      -> FALSE 2 }T
T{ : REPL ( caddr1 u1 "name" -- ) PARSE-NAME REPLACES ; -> }T
T{ $" " REPL MAC3 -> }T    \ Check compiled version
T{ $" abc%mac3%def%mac1%gh" SUBBUF 20 SUBSTITUTE
   $" abcdefwxyzgh" $CHECKN
      -> FALSE 2 }T      \ Zero length string substituted
T{ $" %mac3%" SUBBUF 10 SUBSTITUTE $" " $CHECKN
      -> FALSE 1 }T      \ Zero length string substituted
T{ $" abc%%mac1%%%mac2%" SUBBUF 20 SUBSTITUTE
   $" abc%mac1%12" $CHECKN
      -> FALSE 1 }T   \ Check substitution is single pass
T{ $" %mac3%" MAC3 REPLACES -> }T
T{ $" a%mac3%b" SUBBUF 20 SUBSTITUTE $" a%mac3%b" $CHECKN
      -> FALSE 1 }T    \ Check non-recursive
T{ $" %%" MAC3 REPLACES -> }T
T{ $" abc%mac1%de%mac3%g%mac2%%%%mac1%hij" SUBBUF 30 SUBSTITUTE
      $" abcwxyzde%%g12%wxyzhij" $CHECKN -> FALSE 4 }T
T{ $" ab%mac4%c" SUBBUF 20 SUBSTITUTE $" ab%mac4%c" $CHECKN
      -> FALSE 0 }T   \ Non-substitution name passed unchanged
T{ $" %mac2%%mac5%" SUBBUF 20 SUBSTITUTE $" 12%mac5%" $CHECKN
      -> FALSE 1 }T   \ Non-substitution name passed unchanged
T{ $" %mac5%" SUBBUF 20 SUBSTITUTE $" %mac5%" $CHECKN
      -> FALSE 0 }T   \ Non-substitution name passed unchanged

  \ Check UNESCAPE SUBSTITUTE leaves a string unchanged
T{ $" %mac1%" SUBBUF 30 CHARS + UNESCAPE SUBBUF 10 SUBSTITUTE
   $" %mac1%" $CHECKN
   -> FALSE 0 }T

  \ Check with odd numbers of "%" characters, last is passed
  \ unchanged.

T{ $" %" SUBBUF 10 SUBSTITUTE $" %" $CHECKN -> FALSE 0 }T
T{ $" %abc" SUBBUF 10 SUBSTITUTE $" %abc" $CHECKN -> FALSE 0 }T
T{ $" abc%" SUBBUF 10 SUBSTITUTE $" abc%" $CHECKN -> FALSE 0 }T
T{ $" abc%mac1" SUBBUF 10 SUBSTITUTE $" abc%mac1" $CHECKN
   -> FALSE 0 }T
T{ $" abc%mac1%d%%e%mac2%%mac3" SUBBUF 20 SUBSTITUTE
      $" abcwxyzd%e12%mac3" $CHECKN -> FALSE 2 }T

  \ Check for errors
T{ $" abcd" SUBBUF 4 SUBSTITUTE $" abcd" $CHECKN -> FALSE 0 }T
  \ Just fits

T{ $" abcd" SUBBUF 3 SUBSTITUTE ROT ROT 2DROP 0< -> TRUE }T
  \ Just too long

T{ $" abcd" SUBBUF 0 SUBSTITUTE ROT ROT 2DROP 0< -> TRUE }T
T{ $" zyxwvutsr" MAC3 REPLACES -> }T
T{ $" abc%mac3%d" SUBBUF 10 SUBSTITUTE ROT ROT 2DROP 0<
   -> TRUE }T

  \ Conditional test for overlapping strings, including the
  \ case where caddr1 = caddr2. If a system cannot handle
  \ overlapping strings it should return n < 0 with (caddr2 u2)
  \ undefined. If it can handle them correctly it should return
  \ the usual results for success. The following definition
  \ applies the appropriate tests depending on whether n < 0 or
  \ not.  Return ( f n ) where f = TRUE if: n >= 0 and (bufad =
  \ caddr1) and (string1 = string2) or n < 0

: SUBST-N? ( n1 n2 -- f ) \ True if n1<0 or n1>=0 and n1=n2 )
   OVER 0< IF DROP 0< 0= ELSE = THEN
;

  \ Check the result of overlapped-subst

  \ n2 is expected number of substitutions, caddr2 u2 the
  \ expected result

: CHECK-SUBST ( caddr1 u1 bufad n n2 caddr2 u2 -- f )
   >R >R ROT >R SUBST-N?         ( -- caddr1 u1 f1 )
   IF
      OVER R> =                  \ Check caddr1 = bufad
      IF
         R> R> COMPARE 0= EXIT   \ Check string1 = string2
      THEN
   ELSE
      R> DROP
   THEN
   R> R> 2DROP 2DROP FALSE ;

  \ Copy string to (buf+u2) and expect substitution result at
  \ (buf+u3) u4 is length of result buffer then execute
  \ SUBSTITUTE and check the result

: OVERLAPPED-SUBST ( caddr1 u1 u2 u3 u4 -- caddr5 u5 bufad n )
   >R >R                    ( -- caddr1 u1 u2 ) ( R: -- u4 u3 )
   CHARS SUBBUF + SWAP        ( -- caddr1 buf+u2' u1 )
   DUP >R OVER >R MOVE        ( -- ) ( R: -- u4 u3 u1 buf+u2')
   R> R> SUBBUF R> CHARS + R> ( -- buf+u2 u1 buf+u3' u4 )
   OVER >R SUBSTITUTE R> SWAP ( -- caddr5 u5 buf+u3 n )
;

T{ $" zyxwvut" MAC3 REPLACES -> }T
T{ $" zyx"     MAC2 REPLACES -> }T
T{ $" a%mac3%b" 0 9 20 OVERLAPPED-SUBST 1
   $" azyxwvutb" CHECK-SUBST -> TRUE }T
T{ $" a%mac3%b" 0 3 20 OVERLAPPED-SUBST 1
   $" azyxwvutb" CHECK-SUBST -> TRUE }T
T{ $" a%mac2%b" 0 3 20 OVERLAPPED-SUBST 1
   $" azyxb"     CHECK-SUBST -> TRUE }T
T{ $" abcdefgh" 0 0 20 OVERLAPPED-SUBST 0
   $" abcdefgh"  CHECK-SUBST -> TRUE }T
T{ $" a%mac3%b" 3 0 20 OVERLAPPED-SUBST 1
   $" azyxwvutb" CHECK-SUBST -> TRUE }T
T{ $" a%mac3%b" 9 0 20 OVERLAPPED-SUBST 1
   $" azyxwvutb" CHECK-SUBST -> TRUE }T

  \ Definition using a name on the stack
: $CREATE ( caddr u -- )
   S" name" REPLACES          ( -- )
   S" CREATE %name%" SUBBUF 40 SUBSTITUTE
   0 > IF EVALUATE THEN
;
t{ $" SUBST2" $CREATE 123 , -> }t
t{ SUBST2 @ -> 123 }t

  \ ===========================================================

STRING-ERRORS SET-ERROR-COUNT

CR .( End of String word tests) CR

  \ To test some of the ANS Forth Programming Tools and
  \ extension wordset

  \ This program was written by Gerry Jackson in 2006, with
  \ contributions from others where indicated, and is in the
  \ public domain - it can be distributed and/or modified in
  \ any way but please retain this notice.

  \ This program is distributed in the hope that it will be
  \ useful, but WITHOUT ANY WARRANTY; without even the implied
  \ warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
  \ PURPOSE.

  \ The tests are not claimed to be comprehensive or correct

  \ ===========================================================
  \ Version 0.13 31 October 2015 More tests on [ELSE] and [THEN]
  \              TRAVERSE-WORDLIST etc tests made conditional on the required
  \              search-order words being available
  \              Calls to COMPARE replaced with S= (in utilities.fth)
  \         0.11 25 April Added tests for N>R NR> SYNONYM TRAVERSE-WORDLIST
  \              NAME>COMPILE NAME>INTERPRET NAME>STRING
  \         0.6  1 April 2012 Tests placed in the public domain.
  \              Further tests on [IF] [ELSE] [THEN]
  \         0.5  30 November 2009 <TRUE> and <FALSE> replaced with TRUE and FALSE
  \         0.4  6 March 2009 ENDIF changed to THEN. {...} changed to T{...}T
  \         0.3  20 April 2007 ANS Forth words changed to upper case
  \         0.2  30 Oct 2006 updated following GForth test to avoid
  \              changing stack depth during a colon definition
  \         0.1  Oct 2006 First version released

  \ ===========================================================
  \ The tests are based on John Hayes test program

  \ Words tested in this file are:
  \     AHEAD [IF] [ELSE] [THEN] CS-PICK CS-ROLL [DEFINED] [UNDEFINED]
  \     N>R NR> SYNONYM TRAVERSE-WORDLIST NAME>COMPILE NAME>INTERPRET
  \     NAME>STRING
  \

  \ Words not tested:
  \     .S ? DUMP SEE WORDS
  \     ;CODE ASSEMBLER BYE CODE EDITOR FORGET STATE
  \ ===========================================================
  \ Assumptions, dependencies and notes:
  \     - tester.fr (or ttester.fs), errorreport.fth and utilities.fth have been
  \       included prior to this file
  \     - the Core word set is available and tested
  \     - testing TRAVERSE-WORDLIST uses WORDLIST SEARCH-WORDLIST GET-CURRENT
  \       SET-CURRENT and FORTH-WORDLIST from the Search-order word set. If any
  \       of these are not present these tests will be ignored
  \ ===========================================================

DECIMAL

  \ ===========================================================

TESTING AHEAD

T{ : PT1 AHEAD 1111 2222 THEN 3333 ; -> }T
T{ PT1 -> 3333 }T

  \ ===========================================================

TESTING [IF] [ELSE] [THEN]

T{ TRUE  [IF] 111 [ELSE] 222 [THEN] -> 111 }T
T{ FALSE [IF] 111 [ELSE] 222 [THEN] -> 222 }T

T{ TRUE  [IF] 1     \ Code spread over more than 1 line
             2
          [ELSE]
             3
             4
          [THEN] -> 1 2 }T
T{ FALSE [IF]
             1 2
          [ELSE]
             3 4
          [THEN] -> 3 4 }T

T{ TRUE  [IF] 1 TRUE  [IF] 2 [ELSE] 3 [THEN] [ELSE] 4 [THEN]
   -> 1 2 }T
T{ FALSE [IF] 1 TRUE  [IF] 2 [ELSE] 3 [THEN] [ELSE] 4 [THEN]
   -> 4 }T
T{ TRUE  [IF] 1 FALSE [IF] 2 [ELSE] 3 [THEN] [ELSE] 4 [THEN]
   -> 1 3 }T
T{ FALSE [IF] 1 FALSE [IF] 2 [ELSE] 3 [THEN] [ELSE] 4 [THEN]
   -> 4 }T

  \ ===========================================================

TESTING immediacy of [IF] [ELSE] [THEN]

T{ : PT2 [  0 ] [IF] 1111 [ELSE] 2222 [THEN]  ; PT2 -> 2222 }T
T{ : PT3 [ -1 ] [IF] 3333 [ELSE] 4444 [THEN]  ; PT3 -> 3333 }T
: PT9 BL WORD FIND ;
T{ PT9 [IF]   NIP -> 1 }T
T{ PT9 [ELSE] NIP -> 1 }T
T{ PT9 [THEN] NIP -> 1 }T

  \ ===========================================================

TESTING [IF] and [ELSE] carry out a text scan
TESTING by parsing and discarding words
  \ so that an [ELSE] or [THEN] in a comment or string is recognised

: PT10 REFILL DROP REFILL DROP ;

T{ 0  [IF]            \ Words ignored up to [ELSE] 2
      [THEN] -> 2 }T
T{ -1 [IF] 2 [ELSE] 3 $" [THEN] 4 PT10 IGNORED TO END OF LINE"
      [THEN]
      \ A precaution in case [THEN] in string isn't recognised
   -> 2 4 }T

  \ ===========================================================

TESTING [ELSE] and [THEN] without a preceding [IF]

  \ [ELSE] ... [THEN] acts like a multi-line comment
T{ [ELSE]
11 12 13
[THEN] 14 -> 14 }T

T{ [ELSE] -1 [IF] 15 [ELSE] 16 [THEN] 17 [THEN] 18 -> 18 }T

  \ A lone [THEN] is a noop
T{ 19 [THEN] 20 -> 19 20 }T

  \ ===========================================================

TESTING CS-PICK and CS-ROLL

  \ Test PT5 based on example in ANS document p 176.

: ?REPEAT
   0 CS-PICK POSTPONE UNTIL
; IMMEDIATE

VARIABLE PT4

T{ : PT5 ( N1 -- )
      PT4 !
      BEGIN
         -1 PT4 +!
         PT4 @ 4 > 0= ?REPEAT \ Back TO BEGIN if FALSE
         111
         PT4 @ 3 > 0= ?REPEAT
         222
         PT4 @ 2 > 0= ?REPEAT
         333
         PT4 @ 1 =
      UNTIL
; -> }T

T{ 6 PT5 -> 111 111 222 111 222 333 111 222 333 }T


T{ : ?DONE POSTPONE IF 1 CS-ROLL ; IMMEDIATE
   -> }T  \ Same as WHILE
T{ : PT6
      >R
      BEGIN
         R@
      ?DONE
         R@
         R> 1- >R
      REPEAT
      R> DROP
   ; -> }T

T{ 5 PT6 -> 5 4 3 2 1 }T

: MIX_UP 2 CS-ROLL ; IMMEDIATE  \ CS-ROT

: PT7    ( f3 f2 f1 -- ? )
   IF 1111 ROT ROT ( -- 1111 f3 f2 ) ( cs: -- orig1 )
      IF 2222 SWAP ( -- 1111 2222 f3 ) ( cs: -- orig1 orig2 )
         IF ( cs: -- orig1 orig2 orig3 )
            3333 MIX_UP ( -- 1111 2222 3333 )
                        ( cs: -- orig2 orig3 orig1 )
         THEN ( cs: -- orig2 orig3 )
         4444
         \ Hence failure of first IF comes here and falls through
      THEN ( cs: -- orig2 )
      5555 \ Failure of 3rd IF comes here
   THEN ( cs: -- )
   6666 \ Failure of 2nd IF comes here
;

T{ -1 -1 -1 PT7 -> 1111 2222 3333 4444 5555 6666 }T
T{  0 -1 -1 PT7 -> 1111 2222 5555 6666 }T
T{  0  0 -1 PT7 -> 1111 0    6666 }T
T{  0  0  0 PT7 -> 0    0    4444 5555 6666 }T

: [1CS-ROLL] 1 CS-ROLL ; IMMEDIATE

T{ : PT8
      >R
      AHEAD 111
      BEGIN 222
         [1CS-ROLL]
         THEN
         333
         R> 1- >R
         R@ 0<
      UNTIL
      R> DROP
   ; -> }T

T{ 1 PT8 -> 333 222 333 }T

  \ ===========================================================

TESTING [DEFINED] [UNDEFINED]

CREATE DEF1

T{ [DEFINED]   DEF1 -> TRUE  }T
T{ [UNDEFINED] DEF1 -> FALSE }T
T{ [DEFINED]   12345678901234567890 -> FALSE }T
T{ [UNDEFINED] 12345678901234567890 -> TRUE  }T
T{ : DEF2 [DEFINED]   DEF1 [IF] 1 [ELSE] 2 [THEN] ; -> }T
T{ : DEF3 [UNDEFINED] DEF1 [IF] 3 [ELSE] 4 [THEN] ; -> }T
T{ DEF2 -> 1 }T
T{ DEF3 -> 4 }T

  \ ===========================================================

TESTING N>R NR>

T{ : NTR  N>R -1 NR> ; -> }T
T{ 1 2 3 4 5 6 7 4 NTR -> 1 2 3 -1 4 5 6 7 4 }T
T{ 1 0 NTR -> 1 -1 0 }T
T{ : NTR2 N>R N>R -1 NR> -2 NR> ;
T{ 1 2 2 3 4 5 3 NTR2 -> -1 1 2 2 -2 3 4 5 3 }T
T{ 1 0 0 NTR2 -> 1 -1 0 -2 0 }T

  \ ===========================================================

TESTING SYNONYM

: SYN1 1234 ;
T{ SYNONYM NEW-SYN1 SYN1 -> }T
T{ NEW-SYN1 -> 1234 }T
: SYN2 2345 ; IMMEDIATE
T{ SYNONYM NEW-SYN2 SYN2 -> }T
T{ NEW-SYN2 -> 2345 }T
T{ : SYN3 SYN2 LITERAL ; SYN3 -> 2345 }T

  \ ===========================================================

  \ These tests require `GET-CURRENT`, `SET-CURRENT` and
  \ `WORDLIST` from the optional Search-Order word set. If any
  \ of these are not available the tests will be ignored.

[?UNDEF] WORDLIST \? [?UNDEF] GET-CURRENT
\? [?UNDEF] SET-CURRENT
\? [?UNDEF] FORTH-WORDLIST

\? TESTING TRAVERSE-WORDLIST NAME>COMPILE NAME>INTERPRET
\? NAME>STRING

\? GET-CURRENT CONSTANT CURR-WL
\? WORDLIST CONSTANT TRAV-WL
\? : WDCT ( n nt -- n+1 f ) DROP 1+ TRUE ;
\? T{ 0 ' WDCT TRAV-WL TRAVERSE-WORDLIST -> 0 }T

\? TRAV-WL SET-CURRENT
\? : TRAV1 1 ;
\? T{ 0 ' WDCT TRAV-WL TRAVERSE-WORDLIST -> 1 }T
\? : TRAV2 2 ; : TRAV3 3 ; : TRAV4 4 ; : TRAV5 5 ;
\? : TRAV6 6 ; IMMEDIATE
\? CURR-WL SET-CURRENT
\? T{ 0 ' WDCT TRAV-WL TRAVERSE-WORDLIST -> 6 }T
   \ Traverse whole wordlist

  \ Terminate `TRAVERSE-WORDLIST` after n words & check it
  \ compiles.

\? : (PART-OF-WL) ( ct n nt -- ct+1 n-1 )
\?    DROP DUP IF SWAP 1+ SWAP 1- THEN DUP
\? ;
\? : PART-OF-WL ( n -- ct 0 | ct+1 n-1)
\?    0 SWAP ['] (PART-OF-WL) TRAV-WL TRAVERSE-WORDLIST DROP
\? ;
\? T{ 0 PART-OF-WL -> 0 }T
\? T{ 1 PART-OF-WL -> 1 }T
\? T{ 4 PART-OF-WL -> 4 }T
\? T{ 9 PART-OF-WL -> 6 }T  \ Traverse whole wordlist

  \ Testing `NAME>..` words require a name token. It will be
  \ easier to test them if there is a way of obtaining the name
  \ token of a given word. To get this we need a definition to
  \ compare a given name with the result of `NAME>STRING`.  The
  \ output from `NAME>STRING` has to be copied into a buffer
  \ and converted to a known case as different Forth systems
  \ may store names as lower, upper or mixed case.

\? CREATE UCBUF 32 CHARS ALLOT    \ The buffer

  \ Convert string to upper case and save in the buffer.

\? : >UPPERCASE ( caddr u  -- caddr2 u2 )
\?    32 MIN DUP >R UCBUF DUP 2SWAP
\?    OVER + SWAP 2DUP U>
\?    IF
\?       DO \ ?DO can't be used, as it is a Core Extension word
\?          I C@ DUP [CHAR] a [CHAR] z 1+ WITHIN
\?          IF 32 INVERT AND THEN
\?          OVER C! CHAR+
\?       LOOP
\?    ELSE
\?       2DROP
\?    THEN
\?    DROP R>
\? ;

  \ Compare string (caddr u) with name associated with nt
\? : NAME? ( caddr u nt -- caddr u f )
\?    \ f = true for name = (caddr u) string
\?    NAME>STRING >UPPERCASE 2OVER S= ;

  \ The word to be executed by TRAVERSE-WORDLIST
\? : GET-NT ( caddr u 0 nt -- caddr u nt false | caddr u 0 nt )
\?    \ nt <> 0
\?    2>R R@ NAME? IF R> R> ELSE 2R> THEN ;

  \ Get name token of (caddr u) in wordlist wid, return 0 if not present
\? : GET-NAME-TOKEN ( caddr u wid -- nt | 0 )
\?    0 ['] GET-NT ROT TRAVERSE-WORDLIST >R 2DROP R> ;

  \ Test NAME>STRING via TRAVERSE-WORDLIST
\? T{ $" ABCDE" TRAV-WL GET-NAME-TOKEN 0= -> TRUE  }T
   \ Not in wordlist
\? T{ $" TRAV4" TRAV-WL GET-NAME-TOKEN 0= -> FALSE }T

  \ Test NAME>INTERPRET on a word with interpretation semantics
\? T{ $" TRAV3" TRAV-WL GET-NAME-TOKEN NAME>INTERPRET EXECUTE
\?   -> 3 }T

  \ Test `NAME>INTERPRET` on a word without interpretation
  \ semantics. It is difficult to choose a suitable word
  \ because:

  \    - a user cannot define one in a standard system
  \    - a Forth system may choose to define interpretation semantics for a word
  \      despite the standard stating they are undefined. If so the behaviour
  \      cannot be tested as it is 'undefined' by the standard.
  \ (October 2016) At least one major system, GForth, has defined behaviour for
  \ all words with undefined interpretation semantics. It is not possible in
  \ standard Forth to define a word without interpretation semantics, therefore
  \ it is not possible to have a general test for NAME>INTERPRET returning 0.
  \ So the following word TIF executes NAME>INTERPRET for all words with
  \ undefined interpretation semantics in the Core word set, the first one to
  \ return 0 causes the rest to be skipped. If none return 0 a message is
  \ displayed to that effect. No system can fail this test!

\? VARIABLE TIF-SKIP
\? : TIF ( "name1 ... namen" -- )
\?    \ TIF = TEST-INTERPRETATION-UNDEFINED
\?    BEGIN
\?       TIF-SKIP @ IF SOURCE >IN ! DROP EXIT THEN
\?       BL WORD COUNT DUP 0= IF 2DROP EXIT THEN  \ End of line
\?       FORTH-WORDLIST GET-NAME-TOKEN ?DUP ( -- nt nt | 0 0 )
\?       IF
\?          NAME>INTERPRET 0= TIF-SKIP !
\?            \ Returning 0 skips further tests
\?       THEN
\?       0      \ AGAIN is a Core Ext word
\?    UNTIL ;

\? : TIF? ( -- )
\?   TIF-SKIP @ 0=
\?   IF
\?     CR
\?     ." NAME>INTERPRET returns an execution token for all"
\?     CR
\?     ." core words with undefined interpretation semantics."
\?     CR
\?     ." So NAME>INTERPRET returning 0 is untested." CR
\?   THEN ;

\? 0 TIF-SKIP !
\? TIF DUP SWAP DROP
\? TIF >R R> R@ ." ; EXIT ['] [CHAR] RECURSE ABORT" DOES>
\? TIF LITERAL POSTPONE
\? TIF DO I J LOOP +LOOP UNLOOP LEAVE IF ELSE THEN BEGIN WHILE
\? TIF REPEAT UNTIL
\? TIF?

  \ Test NAME>COMPILE
\? : N>C ( caddr u -- )
\?   TRAV-WL GET-NAME-TOKEN NAME>COMPILE EXECUTE ; IMMEDIATE

\? T{ : N>C1 ( -- n ) [ $" TRAV2" ] N>C ; N>C1 -> 2 }T
        \ Not immediate

\? T{ : N>C2 ( -- n ) [ $" TRAV6" ] N>C LITERAL ; N>C2 -> 6 }T
        \ Immediate word

\? T{ $" TRAV6" TRAV-WL GET-NAME-TOKEN NAME>COMPILE EXECUTE
\?    -> 6 }T

  \ Test the order of finding words with the same name
\? TRAV-WL SET-CURRENT
\? : TRAV3 33 ; : TRAV3 333 ; : TRAV7 7 ; : TRAV3 3333 ;
\? CURR-WL SET-CURRENT

\? : (GET-ALL) ( caddr u nt -- [n] caddr u true )
\?    DUP >R NAME? IF R@ NAME>INTERPRET EXECUTE ROT ROT THEN
\?    R> DROP TRUE
\? ;

\? : GET-ALL ( caddr u -- i*x )
\?    ['] (GET-ALL) TRAV-WL TRAVERSE-WORDLIST 2DROP
\? ;

\? T{ $" TRAV3" GET-ALL -> 3333 333 33 3 }T
[?ELSE]
\? CR CR
\? .( Some search-order words not present) CR
\? .( TRAVERSE-WORDLIST etc not tested) CR
[?THEN]

  \ ===========================================================

TOOLS-ERRORS SET-ERROR-COUNT

CR .( End of Programming Tools word tests) CR

  \ The ANS/Forth 2012 test suite is being modified so that the
  \ test programs for the optional word sets only use standard
  \ words from the Core word set.  This file, which is included
  \ *after* the Core test programs, contains various
  \ definitions for use by the optional word set test programs
  \ to remove any dependencies between word sets.

DECIMAL

  \ First a definition to see if a word is already defined.
  \ Note that `[DEFINED]`, `[IF]`, `[ELSE]` and `[THEN]` are in
  \ the optional Programming Tools word set.

VARIABLE (\?) 0 (\?) !
  \ Flag: Word defined = 0 | word undefined = -1

  \ `[?DEF]` followed by `[?IF]` cannot be used again until
  \ after `[THEN]`.

: [?DEF] ( "name" -- )
   BL WORD FIND SWAP DROP 0= (\?) !
;

  \ Test [?DEF]
T{ 0 (\?) ! [?DEF] ?DEFTEST1 (\?) @ -> -1 }T
: ?DEFTEST1 1 ;
T{ -1 (\?) ! [?DEF] ?DEFTEST1 (\?) @ -> 0 }T

: [?UNDEF] [?DEF] (\?) @ 0= (\?) ! ;

  \ Equivalents of [IF] [ELSE] [THEN], these must not be nested
: [?IF] ( f -- ) (\?) ! ; IMMEDIATE
: [?ELSE] ( -- ) (\?) @ 0= (\?) ! ; IMMEDIATE
: [?THEN] ( -- ) 0 (\?) ! ; IMMEDIATE

  \ A conditional comment and \ will be defined. Note that
  \ these definitions are inadequate for use in Forth blocks.
  \ If needed in the blocks test program they will need to be
  \ modified here or redefined there.

  \ \? is a conditional comment
: \? ( "..." -- ) (\?) @ IF EXIT THEN SOURCE >IN ! DROP ;
  IMMEDIATE

  \ Test \?
T{ [?DEF] ?DEFTEST1 \? : ?DEFTEST1 2 ; \ Shouldn't be redefined
          ?DEFTEST1 -> 1 }T
T{ [?DEF] ?DEFTEST2 \? : ?DEFTEST1 2 ; \ Should be redefined
          ?DEFTEST1 -> 2 }T

[?DEF] TRUE  \? -1 CONSTANT TRUE
[?DEF] FALSE \?  0 CONSTANT FALSE
[?DEF] NIP   \?  : NIP SWAP DROP ;
[?DEF] TUCK  \?  : TUCK SWAP OVER ;

[?DEF] PARSE
\? : BUMP ( caddr u n -- caddr+n u-n )
\?    TUCK - >R CHARS + R>
\? ;

\? : PARSE ( ch "ccc<ch>" -- caddr u )
\?    >R SOURCE >IN @ BUMP
\?    OVER R> SWAP >R >R ( -- start u1 ) ( R: -- start ch )
\?    BEGIN
\?       DUP
\?    WHILE
\?       OVER C@ R@ = 0=
\?    WHILE
\?       1 BUMP
\?    REPEAT
\?       1-                      ( end u2 ) \ delimiter found
\?    THEN
\?    SOURCE NIP SWAP - >IN !    ( -- end )
\?    R> DROP R>                 ( -- end start )
\?    TUCK - 1 CHARS /           ( -- start u )
\? ;

[?DEF] .(  \? : .(  [CHAR] ) PARSE TYPE ; IMMEDIATE

  \ `S=` to compare (case sensitive) two strings to avoid use
  \ of `COMPARE` from the String word set. It is defined in
  \ core.fr and conditionally defined here if core.fr has not
  \ been included by the usera.

[?DEF] S=
\? : S= ( caddr1 u1 caddr2 u2 -- f )
\?   \ f = TRUE if strings are equal
\?   ROT OVER = 0= IF DROP 2DROP FALSE EXIT THEN
\?   DUP 0= IF DROP 2DROP TRUE EXIT THEN
\?   0 DO
\?     OVER C@ OVER C@ = 0= IF 2DROP FALSE UNLOOP EXIT THEN
\?     CHAR+ SWAP CHAR+
\?   LOOP 2DROP TRUE ;

  \ Buffer for strings in interpretive mode since `S"` only
  \ valid in compilation mode when File-Access word set is
  \ defined.

64 CONSTANT SBUF-SIZE
CREATE SBUF1 SBUF-SIZE CHARS ALLOT
CREATE SBUF2 SBUF-SIZE CHARS ALLOT

  \ ($") saves a counted string at (caddr)
: ($") ( caddr "ccc" -- caddr' u )
   [CHAR] " PARSE ROT 2DUP C!       ( -- ca2 u2 ca)
   CHAR+ SWAP 2DUP 2>R CHARS MOVE   ( -- ) ( R: -- ca' u2 )
   2R>
;

: $" ( "ccc" -- caddr u ) SBUF1 ($") ;
: $2" ( "ccc" -- caddr u ) SBUF2 ($") ;
: $CLEAR ( caddr -- ) SBUF-SIZE BL FILL ;
: CLEAR-SBUFS ( -- ) SBUF1 $CLEAR SBUF2 $CLEAR ;

  \ More definitions in core.fr used in other test programs,
  \ conditionally defined here if core.fr has not been loaded.

[?DEF] MAX-UINT   \? 0 INVERT                 CONSTANT MAX-UINT
[?DEF] MAX-INT    \? 0 INVERT 1 RSHIFT        CONSTANT MAX-INT
[?DEF] MIN-INT    \? 0 INVERT 1 RSHIFT INVERT CONSTANT MIN-INT
[?DEF] MID-UINT   \? 0 INVERT 1 RSHIFT        CONSTANT MID-UINT
[?DEF] MID-UINT+1 \? 0 INVERT 1 RSHIFT INVERT
  CONSTANT MID-UINT+1

[?DEF] 2CONSTANT \? : 2CONSTANT  CREATE , , DOES> 2@ ;

BASE @ 2 BASE ! -1 0 <# #S #> SWAP DROP
CONSTANT BITS/CELL BASE !


  \ ===========================================================
  \ Tests

: STR1  S" abcd" ;  : STR2  S" abcde" ;
: STR3  S" abCd" ;  : STR4  S" wbcd"  ;
: S"" S" " ;

T{ STR1 2DUP S= -> TRUE }T
T{ STR2 2DUP S= -> TRUE }T
T{ S""  2DUP S= -> TRUE }T \ ""
T{ STR1 STR2 S= -> FALSE }T
T{ STR1 STR3 S= -> FALSE }T
T{ STR1 STR4 S= -> FALSE }T

T{ CLEAR-SBUFS -> }T
T{ $" abcdefghijklm"  SBUF1 COUNT S= -> TRUE  }T
T{ $" nopqrstuvwxyz"  SBUF2 OVER  S= -> FALSE }T
T{ $2" abcdefghijklm" SBUF1 COUNT S= -> FALSE }T
T{ $2" nopqrstuvwxyz" SBUF1 COUNT S= -> TRUE  }T

  \ ===========================================================

CR $" Test utilities loaded" TYPE CR

  \ ===========================================================
  \ Change log

  \ 2018-03-09: Start the adaptation of the code from Gerry
  \ Jackson's forth2012-test-suite version 0.13.0
  \ (https://github.com/gerryjackson/forth2012-test-suite).
  \
  \ 2018-03-10: Make all lines fit.

  \ vim: filetype=soloforth
