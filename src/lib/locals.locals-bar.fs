  \ locals.locals-bar.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201804011546
  \ See change log at the end of the file

  \ XXX UNDER DEVELOPMENT

  \ ===========================================================
  \ Description

  \ An implementation of Forth-94 locals, adapted from Bill
  \ Muench's eForth.

  \ ===========================================================
  \ Authors

  \ Bill Muench (OntoLogic, forth(at)calcentral(dot)com) wrote
  \ the original code for eForth, Win32Forth and bForth, 1995,
  \ 1996.
  \
  \ Marcos Cruz (programandala.net) adapted the code to Solo
  \ Forth, 2018.

  \ ===========================================================
  \ License

  \ License of the original version for eForth:

  \ Copyright Bill Muench All rights reserved.
  \
  \ Permission is granted for non-commercial use, provided this
  \ notice is included.

  \ License of this version for Solo Forth:

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ ===========================================================

\ locals| (local) \

DECIMAL

10 CONSTANT [W32F?] IMMEDIATE

: DELIMIT ( 'name< >' -- a u ) BL WORD COUNT ;

[W32F?] [IF]

  \ Win32Forth version

: LSTACKFIX ( n -- -n ) CELLS NEGATE ;
  \ w32f return stack builds down
  \ ABS>REL is defined
  \ LP is defined as the Local Pointer

[ELSE]

  \ bForth version === must be re-defined for other systems ===

: ABS>REL ( a -- a ) ; IMMEDIATE ( bForth )

  : LSTACKFIX ( n -- n ) 1- CELLS ;
  \ bForth return stack builds up

  \ 40 USER LP ( -- a ) ( locals pointer ) ( bForth )
  \ XXX TODO -- multitasking?

[THEN]

-->

\ locals| (local) \

  \ locals runtime, all these need to be in code

: L@ ( -- x ) R@  ABS>REL  @ LP @ + @  R> CELL+ >R ;

: L! ( -- x ) R@  ABS>REL  @ LP @ + !  R> CELL+ >R ;

: L{ ( i*x -- ) ( R: -- a i*x )
  R>  LP @ >R  RP@ LP !  DUP  ABS>REL  @
  BEGIN ?DUP WHILE ROT >R 1- REPEAT  CELL+ >R ;
  \ Build locals frame.

: }L ( -- ) ( R: a i*x -- ) R> LP @ RP! R> LP ! >R ;
  \ Remove locals frame.

-->

\ locals| (local) \

  \ locals compiler internals

8 CONSTANT #LOCALS

CREATE LV$ ( -- a ) \ ???smaller
  31 1 + CHARS #LOCALS * ALLOT ( room for counted strings )

: LV? ( a u -- index | 0 ) ( find requested locals index )
  LV$  1 >R ( init index )
  BEGIN COUNT ?DUP
  WHILE 2OVER 2OVER COMPARE 0= ( *** case sensative *** )
    IF 2DROP 2DROP  R> EXIT THEN +  R> 1+ >R
  REPEAT R> 2DROP  2DROP  0 ; ( not a local )

VARIABLE LCHAR
VARIABLE LOCALS?

: LVREV ( n i -- i ) ( reverse args for { } form )
  NIP LCHAR @ '}' = IF LOCALS? @ 1+ SWAP - THEN LSTACKFIX ;

-->

\ locals| (local) \

: LVFIND ( a -- a 0 | xt -1 ) ( patch for compiler FIND )
  DUP COUNT LV? ?DUP ( try locals first )
  IF LVREV POSTPONE L@ -1 EXIT \ pass index to be 'compiled'
    \ ???problem
  THEN CAPS-FIND ;

: SETLVFIND ( -- ) ['] LVFIND IS FIND ;

: (LOCAL) ( a u -- ) ( save counted string )
  LOCALS? @  OVER 1+ LOCALS? +!  2DUP C! CHAR+ SWAP MOVE ;

: XLOCALS| ( c '<spaces>i*name<spaces"c">' -- )
  >R  LOCALS? @ DUP ABORT" LOCALS| only once"  LV$ LOCALS? !
  BEGIN DELIMIT  OVER C@ R@ -  OVER 1-  OR
  WHILE (LOCAL) 1+  #LOCALS OVER U< ABORT" too many locals"
  REPEAT 2DROP  0 DUP (LOCAL) ( add null string )
  R> LCHAR !  DUP LOCALS? !  ?DUP IF POSTPONE L{ , THEN ;

-->

\ locals| (local) \

: ?LOCALS ( -- ) LOCALS? @ IF  POSTPONE }L  THEN ;

: : ( 'name' -- ) 0  DUP LOCALS? !  LV$ !  :  ;

: EXIT ( -- ) ?LOCALS  POSTPONE EXIT  ; IMMEDIATE

: ; ( -- ) ?LOCALS  POSTPONE ;  ; IMMEDIATE

SETLVFIND

  \ locals user interface

: LOCALS| ( '<spaces>i*name<spaces|spaces>' -- )
  '|' XLOCALS| ; IMMEDIATE

: { ( '<spaces>i*name<spaces}>' -- ) '}' XLOCALS| ; IMMEDIATE

: TO ( 'local' -- )
  >IN @  DELIMIT LV? ?DUP ( try locals first )
  IF LVREV  POSTPONE L! , EXIT
  THEN >IN !  POSTPONE TO ( chain for VALUEs )
  ; IMMEDIATE

( locals|-test )

: J1 ( n n -- ) LOCALS| GREEN RED | RED . GREEN . ;
: J2 ( n n -- ) LOCALS| RED GREEN | RED . GREEN . ;
: J3 ( n n -- ) LOCALS|         | . . ;

: J4 ( n n n -- )
  LOCALS| RED GREEN SPOT | RED . GREEN . SPOT . ;

: J5 ( -- ) LOCALS| A B C D  E F G H | ;

: J6 ( -- )
  LOCALS| A B C D  E F G H | A . B . C . D .  E . F . G . H . ;

: J7 ( -- ) LOCALS| A B C D  E F G H  J | ;
  \ FAILS, to many locals

-->

( locals|-test )

: K1 ( -- ) LOCALS| A | A . ;
: K2 ( -- ) LOCALS| A B | A . B . ;
: K3 ( -- ) LOCALS| A B C | A . B . C . ;
: K4 ( -- ) LOCALS| A B C D | A . B . C . D . ;
: K5 ( -- ) LOCALS| A B C D  E | A . B . C . D .  E . ;
: K6 ( -- ) LOCALS| A B C D  E F | A . B . C . D .  E . F . ;

: K7 ( -- )
  LOCALS| A B C D  E F G | A . B . C . D .  E . F . G . ;

: K8 ( -- )
  LOCALS| A B C D  E F G H | A . B . C . D .  E . F . G . H . ;

: KK ( -- 1 2 3 4 5 6 7 8 9 ) 1 2 3 4 5 6 7 8 9 ;

: K9 ( -- )
  { A B C D  E F G H } A . B . C . D .  E . F . G . H . ;

-->

( locals|-test )

: QQ ( -- ) ( display the local names )
  CR LV$ BEGIN  COUNT ?DUP
         WHILE  2DUP TYPE + 2 SPACES
         REPEAT DROP ;

: Q1 ( -- ) LOCALS| A B PEACH GREEN C D | ; QQ
: Q2 ( -- ) LOCALS| PEACH GREEN C D SPOT | ; QQ

-->

( locals|-test )

0 VALUE V0
: T1 ( -- )
  ." should display 60 50 10" CR
  10 20 30 LOCALS| RED GREEN SPOT |
  0 IF RED DROP EXIT THEN \ test EXIT
  RED GREEN +  DUP TO GREEN
  SPOT + TO RED
  RED . GREEN . SPOT TO V0  V0 . ;

VARIABLE X  0 X !

: T2 ( -- 444 )
  444 555 666 LOCALS| RED GREEN |
  CR RED . RP@ . X @ THROW ; \ restore LP

-->

( locals|-test )

: T3 ( -- 111 )
  111 222 333 LOCALS| RED GREEN |
  CR RED . RP@ . LP @ >R [ ' T2 ] LITERAL CATCH \ save LP
  IF R> LP ! CR ." error " RP@ .
  ELSE R> DROP CR ." ok red " RP@ .
  THEN RED . ;

: T6 ( n n n -- n )
  LOCALS| RED GREEN |  RED GREEN + >R  RED GREEN -  R> . . . ;
  \ XXX REMARK -- ANS considered a violation, but works.

: T7 ( -- ) 1 2 7 T6 ;

: T8 ( u GREEN RED -- ) ( DO-LOOP works )
  LOCALS| RED GREEN |  0 ?DO CR RED . GREEN . LOOP ;

: T9 ( n n n -- n ) ( this is a VIOLATION, and it FAILS )
  >R  LOCALS| RED GREEN |  RED GREEN +  RED GREEN -  R> . . . ;

  \ ===========================================================
  \ Change log

  \ 2018-03-31: Start adaption of the original code: Make it
  \ fit in blocks.
  \
  \ 2018-04-01: Fix index lines.
