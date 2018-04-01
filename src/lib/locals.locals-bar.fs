  \ locals.locals-bar.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201804012032
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

need user

: lstackfix ( n1 -- -n2 ) cells negate ;
  \ return stack builds down

  \ : lstackfix ( n1 -- n2 ) 1- cells ;
  \ return stack builds up

user lp ( -- a ) \ locals pointer

  \ Locals runtime
  \ XXX TODO -- All these need to be in code.

: l@ ( -- x ) r@ @ lp @ + @ r> cell+ >r ;

: l! ( x -- ) r@ @ lp @ + ! r> cell+ >r ;

: l{ ( i*x -- ) ( r: -- a i*x )
  r>  lp @ >r  rp@ lp !  dup @
  begin ?dup while rot >r 1- repeat cell+ >r ;
  \ Build locals frame.

: }l ( -- ) ( r: a i*x -- ) r> lp @ rp! r> lp ! >r ;
  \ Remove locals frame.

-->

\ locals| (local) \

  \ locals compiler internals

8 constant #locals

create lv$ ( -- a ) 31 1 + chars #locals * allot
  \ Room for counted strings.
  \ XXX TODO -- smaller?

: lv? ( ca len -- index | 0 )
  lv$  1 >r ( init index )
  begin count ?dup
  while 2over 2over str=
    if 2drop 2drop  r> exit then +  r> 1+ >r
  repeat r> 2drop  2drop  0 ; \ not a local
  \ Find requested locals index.

create lchar 0 c,

variable locals?

: lvrev ( n i -- i )
  nip lchar c@ '}' = if locals? @ 1+ swap - then lstackfix ;
  \ Reverse args for `{ }` form.

-->

\ locals| (local) \

: lvfind ( ca -- ca 0 | xt -1 )
  dup count lv? ?dup \ try locals first
  if lvrev postpone l@ -1 exit \ pass index to be 'compiled'
    \ XXX TODO -- Problem?
  then ;
  \ Patch for compiler `find`.

: setlvfind ( -- ) ['] lvfind is find ;
  \ XXX FIXME -- `find` is not used, and not deferred

: (local) ( ca len -- )
  locals? @  over 1+ locals? +!  2dup c! char+ swap move ;
  \ Save counted string.

: xlocals| ( c '<spaces>i*name<spaces"c">' -- )
  >r locals? @ dup abort" second locals|"  lv$ locals? !
  begin parse-name  over c@ r@ -  over 1-  or
  while (local) 1+  #locals over u< abort" too many locals"
  repeat 2drop  0 dup (local) \ add null string
  r> lchar c!  dup locals? !  ?dup if postpone l{ , then ;

-->

\ locals| (local) \

: ?locals ( -- ) locals? @ if  postpone }l  then ;

: : ( 'name' -- ) 0  dup locals? !  lv$ !  :  ;

: exit ( -- ) ?locals postpone exit ; immediate

: ; ( -- ) ?locals postpone ; ; immediate

setlvfind

  \ Locals user interface:

: locals| ( '<spaces>i*name<spaces|spaces>' -- )
  '|' xlocals| ; immediate

: { ( '<spaces>i*name<spaces}>' -- ) '}' xlocals| ; immediate

: to ( "name" -- )
  >in @ parse-name lv? ?dup \ try locals first
  if lvrev  postpone l! , exit
  then >in !  postpone to \ chain for values
  ; immediate

( locals|-test )

: j1 ( n n -- ) locals| green red | red . green . ;
: j2 ( n n -- ) locals| red green | red . green . ;
: j3 ( n n -- ) locals|         | . . ;

: j4 ( n n n -- )
  locals| red green spot | red . green . spot . ;

: j5 ( -- ) locals| a b c d  e f g h | ;

: j6 ( -- ) locals| a b c d  e f g h |
            a . b . c . d .  e . f . g . h . ;

: j7 ( -- ) locals| a b c d  e f g h  j | ;
  \ XXX REMARK -- Fails: to many locals.

-->

( locals|-test )

: k1 ( -- ) locals| a | a . ;
: k2 ( -- ) locals| a b | a . b . ;
: k3 ( -- ) locals| a b c | a . b . c . ;
: k4 ( -- ) locals| a b c d | a . b . c . d . ;
: k5 ( -- ) locals| a b c d  e | a . b . c . d .  e . ;
: k6 ( -- ) locals| a b c d  e f | a . b . c . d .  e . f . ;

: k7 ( -- )
  locals| a b c d  e f g | a . b . c . d .  e . f . g . ;

: k8 ( -- )
  locals| a b c d  e f g h | a . b . c . d .  e . f . g . h . ;

: kk ( -- 1 2 3 4 5 6 7 8 9 ) 1 2 3 4 5 6 7 8 9 ;

: k9 ( -- )
  { a b c d  e f g h } a . b . c . d .  e . f . g . h . ;

-->

( locals|-test )

: qq ( -- )
  cr lv$ begin  count ?dup
         while  2dup type + 2 spaces
         repeat drop ;
  \ Display the local names.

: q1 ( -- ) locals| a b peach green c d | ; QQ
: q2 ( -- ) locals| peach green c d spot | ; QQ

0 value v0

: t1 ( -- )
  ." should display 60 50 10" cr
  10 20 30 locals| red green spot |
  0 if red drop exit then \ test `exit`
  red green +  dup to green
  spot + to red
  red . green . spot to v0  v0 . ;

variable x  0 x !  -->

( locals|-test )

: t2 ( -- 444 ) 444 555 666 locals| red green |
                cr red . rp@ . x @ throw ; \ restore `lp`

: t3 ( -- 111 )
  111 222 333 locals| red green |
  cr red . rp@ . lp @ >r [ ' t2 ] literal catch \ save `lp`
  if r> lp ! cr ." error " rp@ .
  else r> drop cr ." ok red " rp@ .
  then red . ;

: t6 ( n n n -- n )
  locals| red green |  red green + >r  red green -  r> . . . ;
  \ XXX REMARK -- This is considered a violation in Forth-94,
  \ but it works.

: t7 ( -- ) 1 2 7 t6 ;

: t8 ( u green red -- )
  locals| red green | 0 ?do cr red . green . loop ;
  \ `DO` works.

: t9 ( n n n -- n )
  >r  locals| red green |  red green +  red green -  r> . . . ;
  \ XXX REMARK -- This is a violation, and it fails.

  \ ===========================================================
  \ Change log

  \ 2018-03-31: Start adaption of the original code: Make it
  \ fit in blocks.
  \
  \ 2018-04-01: Fix index lines. Compact the code, saving two
  \ blocks. Update the layout and the source style; convert
  \ code to lowercase. Make `lchar` a byte variable.

  \ vim: filetype=soloforth
