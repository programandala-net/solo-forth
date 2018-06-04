  \ meta.benchmark.flow.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201806041117
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Flow control benchmarks written during the development of
  \ Solo Forth in order to choose from different implementation
  \ options.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( case-benchs )

  \ Comparison of case-like structures.
  \
  \ 2015-11-14: `case` (4 versions), `case:`, `options[` and
  \ `cases:`.
  \ 2015-12-14: Updated the comments: `case:` has been renamed
  \ to `positional-case:`.

need bench{

: .used ( u -- ) unused - cr u. ." B used " ;

32767 constant iterations

defer (case-bench ( -- )

: case-bench ( n xt -- )
  cr ." ..."  ['] (case-bench defer!
  bench{ iterations 0 ?do  i %11 and (case-bench  loop
  }bench. ;

cr .( default)  unused need case .used  unused
: case-example ( n -- )
  case  0 of  noop  endof  1 of  noop  endof  2 of  noop  endof
  noop endcase ;
.used .( by its example)  ' case-example case-bench  -->

( case-benchs )

cr .( eForth)  unused need eforth-case .used  unused
: case-example ( n -- )
  case  0 of  noop  endof  1 of  noop  endof  2 of  noop  endof
  noop endcase ;
.used .( by its example)  ' case-example case-bench

cr .( Forth-94 docs)  unused need eforth-case .used  unused
: case-example ( n -- )
  case  0 of  noop  endof  1 of  noop  endof  2 of  noop  endof
  noop endcase ;
.used .( by its example)  ' case-example case-bench

cr .( Abersoft Forth) unused need eforth-case .used  unused
: case-example ( n -- )
  case  0 of  noop  endof  1 of  noop  endof  2 of  noop  endof
  noop endcase ;
.used .( by its example)  ' case-example case-bench -->

( case-benchs )

  \ Note: the `positional-case:` structure is more specific
  \ than the other structures: it lacks a default option and
  \ its argument is positional.

cr .( positional-case:)
unused need positional-case: .used  unused
positional-case:  positional-case:-example ( n -- )
  noop  noop  noop  noop ;
.used .( by its example)  ' positional-case:-example case-bench

cr .( options[)
unused need options[ .used  unused
: options[-example ( n -- )
  options[
    0 option noop  1 option noop  2 option noop
      default-option noop
  ]options ;
.used .( by its example)  ' options[-example case-bench  -->

( case-benchs )

cr .( cases:)
unused need cases: .used  unused
cases: cases:-example ( n -- )
  0 case> noop  1 case> noop  2 case> noop  other> noop
.used .( by its example)  ' cases:-example case-bench

cr .( baden-case)
unused need baden-case .used  unused
: baden-case-example ( n -- )
  case 0 = of  noop  endof
  case 1 = of  noop  endof
  case 2 = of  noop  endof
           othercase noop ;
.used .( by its example)  ' baden-case-example case-bench

-->

( case-benchs )

cr .( baden-case-like)
unused .used  unused
: baden-case-like-example ( n -- )
  dup 0 = if drop  noop  exit then
  dup 1 = if drop  noop  exit then
      2 = if       noop  exit then
  noop ;
.used .( by its example)  ' baden-case-like-example case-bench

cr .( vannorman-switch)
unused need [switch .used  unused
[switch vannorman-switch-example drop
  0 runs noop  1 runs noop  2 runs noop
switch]
.used .( by its example)  ' vannorman-switch-example case-bench


  \                        Bytes used            Speed (3)
  \                        --------------------- --------------
  \ Structure              Code (1)  Example (2) Frames Seconds
  \ ---------              --------- ----------- ------ -------
  \ default case (7)         48       62          1365   27
  \ eforth-case (8)          54       62          1366   27
  \ 94-doc-case (6)          54       62          1365   27
  \ abersoft-case (5)        64       62          1365   27
  \ positional-case: (4)     21       12           823   16
  \ options[ (9)            166       24          3627   72
  \ cases: (10)             109       18          3155   63
  \ baden-case (11)          18       56          1472   29
  \ baden-case (12)          36       56          1472   29
  \ baden-case (13)           0       50          1353   27
  \ vannorman-switch (14)   124       24          3573   71

  \ (1) Bytes used by the compilation of the structure's code.
  \
  \ (2) Bytes used by the tested example: a structure with
  \ three options plus default, that execute a `noop`.
  \
  \ (3) For 32767 iterations with parameter 0..3. One system
  \ frame is 20 ms.

  \ (4) A port of F83's `case:`. It is more specific than the
  \ other structures: it lacks a default option and its
  \ argument is positional.
  \
  \ (5) Eaker/Forth-94 `case` of Abersoft Forth, but with
  \ compiler security removed.
  \
  \ (6) Eaker/Forth-94 `case` copied from the Forth-94
  \ documentation.
  \
  \ (7) Eaker/Forth-94 `case` of eForth, with a little
  \ simplification. This is the default `case` used in Solo
  \ Forth.
  \
  \ (8) Eaker/Forth-94 `case` of eForth.
  \
  \ (9) A port of IsForth's `case:`.
  \
  \ (10) A port of a structure written by Dan Lerner, published
  \ on Forth Dimensions (volume 3, number 6, page 189,
  \ 1982-03).
  \
  \ (11) "Ultimate CASE Statement", written by Wil Baden,
  \ published on Forth Dimensions (volume 8, number 5, page 29,
  \ 1987-01).
  \
  \ (12) The same "Ultimate CASE Statement", by Wil Baden, with
  \ two syntactic sugar words added: `endof` and `othercase`.
  \
  \ (13) The same "Ultimate CASE Statement", by Wil Baden,
  \ emulated with standard words. This is a bit faster because,
  \ without the syntactic sugar definitions, one `dup` and two
  \ `drop` are saved.
  \
  \ (14) Code by Rick VanNorman, published on Forth Dimensions
  \ (volume 20, number 3, pages 19..22, 1998-09).

( do-bench )

  \ 2015-12-17

need bench{

32767 0 2constant range

: forth-83-do ( -- ) bench{  range do83  loop83  }bench. ;

: forth-79-do ( -- ) bench{  range ?do  loop  }bench. ;

: forth-83-i ( -- )
  bench{  range do83  i83 drop  loop83  }bench. ;

: forth-79-i ( -- )
  bench{  range ?do  i drop  loop  }bench. ;

: forth-83-+loop ( -- )
  bench{  range do83  2 +loop83  }bench. ;

: forth-79-+loop ( -- ) bench{  range ?do  2 +loop  }bench. ;

: do-bench ( -- ) forth-83-do forth-79-do
                    forth-83-i forth-79-i
                    forth-83-+loop forth-79-+loop ;

  \           Frames by 32767 iterations
  \           --------------------------
  \ Bench     Forth-79  Forth-83
  \ --------  --------  --------
  \ loop           143       109
  \ i              264       258
  \ +loop          108        97

  \ Note: 1 frame = 50th of second

  \ ===========================================================
  \ Change log

  \ 2016-08-05: Compact the code to save one block.
  \
  \ 2016-10-12: Add `exec-bench`.
  \
  \ 2016-11-26: Remove `warnings off`, because now warnings are
  \ deactivated by default.
  \
  \ 2017-02-19: Replace `do`, which has been moved to the
  \ library, with `?do`.
  \
  \ 2018-06-04: Update: remove trailing closing paren from
  \ word names.

  \ vim: filetype=soloforth

( exec-bench )

  \ 2016-10-12
  \
  \ This bench compares the execution time of executing two
  \ pieces of code depending on a flag, using three methods:
  \
  \ - A conditional structure with `else`
  \ - A conditional structure with `exit` instead of `else`
  \ - An execution table that uses the flag as offset

need bench{

: 7border ( -- ) 7 border ;  : 2border ( -- ) 2 border ;

' 7border ,  here  ' 2border ,  constant execution-table

: do-table ( f -- ) cells execution-table + perform ;

: do-cond-else ( f -- ) if  7 border  else  2 border  then ;

: do-cond-exit ( f -- ) if  7 border exit  then  2 border ;

: exec-bench ( n -- )
  dup  cr ." Conditional structure with else:" cr bench{  0 ?do
            true do-cond-else false do-cond-else
          loop  }bench.
  dup  cr ." Conditional structure with exit:" cr bench{  0 ?do
            true do-cond-exit false do-cond-exit
          loop  }bench.
       cr ." Execution table:" cr bench{  0 ?do
            true do-table false do-table  loop  }bench. cr ;

  \         Frames
  \         -------------------------------------------------
  \  Times  Conditional+else Conditional+exit Execution table
  \  -----  ---------------- ---------------- ---------------
  \    10                  1                0               0
  \   100                  2                2               4
  \  1000                 26               24              40
  \ 10000                252              239             395
