  \ meta.tester.ttester.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202006152108
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ This file contains the code for ttester, a utility for
  \ testing Forth words, as developed by several authors (see
  \ below), together with some explanations of its use.

  \ ===========================================================
  \ Authors

  \ ttester is based on the original tester suite by Hayes:
  \ From: John Hayes S1I
  \ Subject: tester.fr
  \ Date: Mon, 27 Nov 95 13:10:09 PST
  \ (C) 1995 JOHNS HOPKINS UNIVERSITY / APPLIED PHYSICS
  \ LABORATORY MAY BE DISTRIBUTED FREELY AS LONG AS THIS
  \ COPYRIGHT NOTICE REMAINS.
  \ VERSION 1.1

  \ All the subsequent changes have been placed in the public
  \ domain.  The primary changes from the original are the
  \ replacement of "{" by "T{" and "}" by "}T" (to avoid
  \ conflicts with the uses of { for locals and } for FSL
  \ arrays), modifications so that the stack is allowed to be
  \ non-empty before T{, and extensions for the handling of
  \ floating point tests.
  \
  \ Code for testing equality of floating point values comes
  \ from ftester.fs written by David N. Williams, based on the
  \ idea of approximate equality in Dirk Zoller's float.4th.
  \
  \ Further revisions were provided by Anton Ertl, including
  \ the ability to handle either integrated or separate
  \ floating point stacks.
  \
  \ Revision history and possibly newer versions can be found
  \ at
  \ http://www.complang.tuwien.ac.at/cvsweb/cgi-bin/cvsweb/gforth/test/ttester.fs
  \
  \ Explanatory material and minor reformatting (no code
  \ changes) by C. G. Montgomery March 2009, with helpful
  \ comments from David Williams and Krishna Myneni.
  \
  \ 25/4/2015  Variable #ERRORS added to accumulate count of
  \ errors for error report at end of tests
  \
  \ Adapted to Solo Forth by Marcos Cruz (programandala.net),
  \ 2018, 2020.

  \ ===========================================================
  \ Credit

  \ Adapted from Gerry Jackson's forth2012-test-suite version
  \ 0.13.0
  \ (https://github.com/gerryjackson/forth2012-test-suite).

  \ ===========================================================
  \ Usage

  \ The basic usage takes the form  `T{ <code> -> <expected
  \ stack> }T`.  This executes  `<code>`  and compares the
  \ resulting stack contents with the  `<expected stack>`
  \ values, and reports any discrepancy between the two sets of
  \ values.

  \ For example:

  \ T{ 1 2 3 swap -> 1 3 2 }T  ok
  \ T{ 1 2 3 swap -> 1 2 2 }T
  \ Incorrect result:
  \ T{ 1 2 3 swap -> 1 2 2 }T ok
  \ T{ 1 2 3 swap -> 1 2 }T
  \ Wrong number of results:
  \ T{ 1 2 3 swap -> 1 2 }T ok

  \ Floating point testing can involve further complications.
  \ The code attempts to determine whether floating-point
  \ support is present, and if so, whether there is a separate
  \ floating-point stack, and behave accordingly.  The
  \ constants `HAS-FLOATING` and `HAS-FLOATING-STACK` contain
  \ the results of its efforts, so the behavior of the code can
  \ be modified by the user if necessary.

  \ Then there are the perennial issues of floating point value
  \ comparisons.  Exact equality is specified by `SET-EXACT`
  \ (the default).  If approximate equality tests are desired,
  \ execute `SET-NEAR`.  Then the floating-point variables
  \ `REL-NEAR` (default 1E-12) and `ABS-NEAR` (default 0E)
  \ contain the values to be used in comparisons by the
  \ (internal) word `FNEARLY=`.

  \ When there is not a separate floating point stack and you
  \ want to use approximate equality for FP values, it is
  \ necessary to identify which stack items are floating point
  \ quantities.  This can be done by replacing the closing }T
  \ with a version that specifies this, such as RRXR}T which
  \ identifies the stack picture ( r r x r ).  The code
  \ provides such words for all combinations of R and X with up
  \ to four stack items.  They can be used with either an
  \ integrated or separate floating point stacks. Adding more
  \ if you need them is straightforward; see the examples in
  \ the source.  Here is an example which also illustrates
  \ controlling the precision of comparisons:

  \   SET-NEAR
  \   1E-6 REL-NEAR F!
  \   T{ S" 3.14159E" >FLOAT -> -1E FACOS TRUE RX}T

  \ The word ERROR is now vectored, so that its action can be
  \ changed by the user (for example, to improve the basic
  \ error counter for the number of errors). The default action
  \ ERROR1 can be used as a factor in the display of error
  \ reports.

  \ Loading ttester does not change `BASE`.  Remember that
  \ floating point input is ambiguous if the base is not
  \ decimal.

  \ The file defines some 70 words in all, but in most cases
  \ only the ones mentioned above will be needed for successful
  \ testing.

  \ ===========================================================

( ttester )

need environment? need [if] need do need blk-line need s+
need depth

BASE @ DECIMAL

VARIABLE ACTUAL-DEPTH \ stack record
CREATE ACTUAL-RESULTS 32 CELLS ALLOT
VARIABLE START-DEPTH
VARIABLE XCURSOR \ for ...}T
VARIABLE ERROR-XT
VARIABLE #ERRORS 0 #ERRORS ! \ for counting errors

: ERROR ERROR-XT @ EXECUTE ;
  \ For vectoring of error reporting.

: "FLOATING" S" FLOATING" ;
  \ Only compiled `S"` in CORE.

: "FLOATING-STACK" S" FLOATING-STACK" ; -->

( ttester )

"FLOATING" ENVIRONMENT? [IF]
    [IF] TRUE [ELSE] FALSE [THEN]
[ELSE]
    FALSE
[THEN] CONSTANT HAS-FLOATING

"FLOATING-STACK" ENVIRONMENT? [IF]
    [IF] TRUE [ELSE] FALSE [THEN]
[ELSE] HAS-FLOATING
  \ We don't know whether the FP stack is separate.
  \ If we have FLOATING, we assume it is.
[THEN] CONSTANT HAS-FLOATING-STACK -->

( ttester )

HAS-FLOATING [IF]

  \ Set the following to the relative and absolute tolerances
  \ you want for approximate float equality, to be used with
  \ F~ in FNEARLY=.  Keep the signs, because F~ needs them.

  FVARIABLE REL-NEAR 1E-12 REL-NEAR F!
  FVARIABLE ABS-NEAR 0E    ABS-NEAR F!

  \ When EXACT? is TRUE, }F uses FEXACTLY=, otherwise
  \ FNEARLY=.

  TRUE VALUE EXACT?  : SET-EXACT ( -- )  TRUE TO EXACT? ;
                     : SET-NEAR  ( -- ) FALSE TO EXACT? ;

  : FEXACTLY=  ( F: X Y -- S: FLAG ) 0E F~ ;
    \ Leave TRUE if the two floats are identical.

  : FABS=  ( F: X Y -- S: FLAG ) ABS-NEAR F@ F~ ;
    \  Leave TRUE if the two floats are equal within the
    \  tolerance stored in ABS-NEAR.

  : FREL=  ( F: X Y -- S: FLAG ) REL-NEAR F@ FNEGATE F~ ;
    \ Leave TRUE if the two floats are relatively equal based
    \ on the tolerance stored in ABS-NEAR.

  : F2DUP  FOVER FOVER ;  : F2DROP FDROP FDROP ;

  : FNEARLY=  ( F: X Y -- S: FLAG )
    F2DUP FEXACTLY= IF F2DROP TRUE EXIT THEN
    F2DUP FREL=     IF F2DROP TRUE EXIT THEN FABS= ;
    \ Leave TRUE if the two floats are nearly equal.  This is
    \ a refinement of Dirk Zoller's FEQ to also allow X = Y,
    \ including both zero, or to allow approximately equality
    \ when X and Y are too small to satisfy the relative
    \ approximation mode in the F~ specification.

  : FCONF= ( R1 R2 -- F )
    EXACT? IF FEXACTLY= ELSE FNEARLY= THEN ;

[THEN] -->

( ttester )

HAS-FLOATING-STACK [IF]

  VARIABLE ACTUAL-FDEPTH
  CREATE ACTUAL-FRESULTS 32 FLOATS ALLOT
  VARIABLE START-FDEPTH
  VARIABLE FCURSOR

  : EMPTY-FSTACK ( F: i*x -- j*x )
    FDEPTH START-FDEPTH @ < IF
        FDEPTH START-FDEPTH @ SWAP DO 0E LOOP
    THEN
    FDEPTH START-FDEPTH @ > IF
        FDEPTH START-FDEPTH @ DO FDROP LOOP
    THEN ;

  : F{ ( -- )
    FDEPTH START-FDEPTH ! 0 FCURSOR ! ; -->

( ttester )

  : F-> ( i*x -- j*x )
    FDEPTH DUP ACTUAL-FDEPTH !
    START-FDEPTH @ > IF
      FDEPTH START-FDEPTH @ - 0
      DO ACTUAL-FRESULTS I FLOATS + F! LOOP
    THEN ;

  : F} ( i*x -- j*x )
    FDEPTH ACTUAL-FDEPTH @ = IF
      FDEPTH START-FDEPTH @ > IF
        FDEPTH START-FDEPTH @ - 0 DO
          ACTUAL-FRESULTS I FLOATS + F@ FCONF= INVERT
          IF S" incorrect FP result:" ERROR LEAVE THEN
        LOOP
      THEN
    ELSE S" Wrong number of FP results:" ERROR THEN ; -->

( ttester )

  : F...}T ( -- )
    FCURSOR @ START-FDEPTH @ + ACTUAL-FDEPTH @ <> IF
      S" Number of float results before '->' "
      S" does not match '...}t' specification:" s+ ERROR
    ELSE FDEPTH START-FDEPTH @ = 0= IF
      S" Number of float results before and after '->'"
      S" does not match:" s+ ERROR THEN THEN ;

  : FTESTER ( R -- )
    FDEPTH 0= ACTUAL-FDEPTH @
    FCURSOR @ START-FDEPTH @ + 1+ < OR IF
      S" Number of float results after '->' "
      S" below '...}t' specification: " s+ ERROR
    ELSE ACTUAL-FRESULTS FCURSOR @ FLOATS + F@ FCONF= 0= IF
      S" Incorrect FP result:" ERROR THEN THEN 1 FCURSOR +! ;

[ELSE] -->

( ttester )

  : EMPTY-FSTACK ;  : F{ ;  : F-> ;  : F} ;  : F...}T ;

  HAS-FLOATING [IF]

    : COMPUTE-CELLS-PER-FP ( -- U )
      DEPTH 0E DEPTH 1- >R FDROP R> SWAP - ;

    COMPUTE-CELLS-PER-FP CONSTANT CELLS-PER-FP

    : FTESTER ( R -- )
      DEPTH CELLS-PER-FP < ACTUAL-DEPTH @
      XCURSOR @ START-DEPTH @ + CELLS-PER-FP + < OR IF
        S" Number of results after '->' "
        S" below '...}t' specification:" s+ ERROR EXIT
      ELSE ACTUAL-RESULTS XCURSOR @ CELLS + F@ FCONF= 0= IF
        S" Incorrect FP result:" ERROR
      THEN THEN CELLS-PER-FP XCURSOR +! ;

  [THEN]

[THEN] -->

( ttester )

: EMPTY-STACK ( i*x -- j*x ) ( F: i*x -- j*x )
  DEPTH START-DEPTH @ <
  IF START-DEPTH @ DEPTH DO 0 LOOP THEN
  DEPTH START-DEPTH @ >
  IF DEPTH START-DEPTH @ DO DROP LOOP THEN EMPTY-FSTACK ;
  \ Empty stack; handles underflowed stack too.

VARIABLE ERROR-PAUSE ERROR-PAUSE ON

: ERROR1 ( ca len -- )
  CR TYPE CR BLK-LINE TYPE CR
  ERROR-PAUSE @ IF   CR ." Press any key to continue" KEY DROP
                THEN EMPTY-STACK  #ERRORS @ 1 + #ERRORS ! ;
  \ The default error manager. Display an error message
  \ followed by the line that had the error. Empty the stack
  \ and update the error count.

' ERROR1 ERROR-XT !

: T{ ( -- ) DEPTH START-DEPTH ! 0 XCURSOR ! F{ ; -->

  \ doc{
  \
  \ t{ ( -- )
  \
  \ Part of `ttester`: Start a test.
  \
  \ See: `->`, `}t`.
  \
  \ }doc

( ttester )

: -> ( i*x -- )
  DEPTH DUP ACTUAL-DEPTH ! START-DEPTH @ > IF
    \ There is something on the stack.
    DEPTH START-DEPTH @ - 0 DO ACTUAL-RESULTS I CELLS + ! LOOP
      \ Save it.
  THEN F-> ;

  \ doc{
  \
  \ -> ( i*x -- )
  \
  \ Part of `ttester`: Record depth and contents of
  \ stack.
  \
  \ See: `t{`, `}t`.
  \
  \ }doc

: }T ( i*x -- )
  DEPTH ACTUAL-DEPTH @ = IF \ depths match
    DEPTH START-DEPTH @ > IF \ there is something on the stack
      DEPTH START-DEPTH @ - 0 DO \ for each stack item
        ACTUAL-RESULTS I CELLS + @ <>
          \ Compare actual with expected.
        IF S" Incorrect result:" ERROR LEAVE THEN
      LOOP
    THEN
  ELSE \ depth mismatch
    S" Wrong number of results:" ERROR
  THEN F} ; -->

  \ doc{
  \
  \ }t ( i*x -- )
  \
  \ Part of `ttester`: End a test by comparing stack
  \ (expected) contents with saved (actual) contents.
  \
  \ See: `t{`, `->`.
  \
  \ }doc

( ttester )

: ...}T ( -- )
  XCURSOR @ START-DEPTH @ + ACTUAL-DEPTH @ <> IF
    S" Number of cell results before '->' "
    S" does not match '...}t' specification:" s+ ERROR
  ELSE DEPTH START-DEPTH @ = 0= IF
    S" Number of cell results before and after '->' "
    S" does not match:" s+ ERROR
  THEN THEN F...}T ;

: XTESTER ( X -- )
  DEPTH 0= ACTUAL-DEPTH @ XCURSOR @ START-DEPTH @ + 1+ < OR IF
    S" Number of cell results after '->' "
    S" below '...}t' specification:" s+ ERROR EXIT
  ELSE ACTUAL-RESULTS XCURSOR @ CELLS + @ <> IF
    S" Incorrect cell result:" ERROR
  THEN THEN 1 XCURSOR +! ; -->

( ttester )

: X}T XTESTER ...}T ;
: XX}T XTESTER XTESTER ...}T ;
: XXX}T XTESTER XTESTER XTESTER ...}T ;
: XXXX}T XTESTER XTESTER XTESTER XTESTER ...}T ;

HAS-FLOATING [IF]

  : R}T FTESTER ...}T ;  : XR}T FTESTER XTESTER ...}T ;
  : RX}T XTESTER FTESTER ...}T ;
  : RR}T FTESTER FTESTER ...}T ;
  : XXR}T FTESTER XTESTER XTESTER ...}T ;
  : XRX}T XTESTER FTESTER XTESTER ...}T ;
  : XRR}T FTESTER FTESTER XTESTER ...}T ;
  : RXX}T XTESTER XTESTER FTESTER ...}T ;
  : RXR}T FTESTER XTESTER FTESTER ...}T ;
  : RRX}T XTESTER FTESTER FTESTER ...}T ;
  : RRR}T FTESTER FTESTER FTESTER ...}T ; -->

( ttester )

  : XXXR}T FTESTER XTESTER XTESTER XTESTER ...}T ;
  : XXRX}T XTESTER FTESTER XTESTER XTESTER ...}T ;
  : XXRR}T FTESTER FTESTER XTESTER XTESTER ...}T ;
  : XRXX}T XTESTER XTESTER FTESTER XTESTER ...}T ;
  : XRXR}T FTESTER XTESTER FTESTER XTESTER ...}T ;
  : XRRX}T XTESTER FTESTER FTESTER XTESTER ...}T ;
  : XRRR}T FTESTER FTESTER FTESTER XTESTER ...}T ;
  : RXXX}T XTESTER XTESTER XTESTER FTESTER ...}T ;
  : RXXR}T FTESTER XTESTER XTESTER FTESTER ...}T ;
  : RXRX}T XTESTER FTESTER XTESTER FTESTER ...}T ;
  : RXRR}T FTESTER FTESTER XTESTER FTESTER ...}T ;
  : RRXX}T XTESTER XTESTER FTESTER FTESTER ...}T ;
  : RRXR}T FTESTER XTESTER FTESTER FTESTER ...}T ;
  : RRRX}T XTESTER FTESTER FTESTER FTESTER ...}T ;
  : RRRR}T FTESTER FTESTER FTESTER FTESTER ...}T ; [THEN] -->

( ttester )

VARIABLE VERBOSE TRUE VERBOSE !
  \ Set flag to TRUE for more verbose output; this may allow
  \ you to tell which test caused your system to hang.

: TESTING ( "ccc" -- )
  ?LOADING
  VERBOSE @ IF   BLK-LINE >IN/L /STRING -TRAILING TYPE CR
            THEN ->IN/L >IN +! ;
  \ Display the rest of the current block line, then skip it.

BASE !

: ttester ( -- ) ;

  \ doc{
  \
  \ ttester ( -- )
  \
  \ Do nothing. ``ttester`` is used just for doing ``need
  \ ttester``, loading `t{`, `->`, `}t` and other words, which
  \ are used by `hayes-test` and `forth2012-test-suite`..
  \
  \ Usage example:

  \ ....
  \ T{ 1 2 3 swap -> 1 3 2 }T  ok
  \ T{ 1 2 3 swap -> 1 2 2 }T
  \ Incorrect result:
  \ T{ 1 2 3 swap -> 1 2 2 }T ok
  \ T{ 1 2 3 swap -> 1 2 }T
  \ Wrong number of results:
  \ T{ 1 2 3 swap -> 1 2 }T ok
  \ ....

  \ See: `hayes-tester`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2018-03-09: Start. Adapted from Gerry Jackson's
  \ forth2012-test-suite version 0.13.0
  \ (https://github.com/gerryjackson/forth2012-test-suite).
  \
  \ 2018-03-10: Update stack comments. Improve source layout.
  \ Compact the code, saving three blocks. Rewrite `testing`
  \ for blocks.
  \
  \ 2018-03-11: Activate `verbose` by default. Factor
  \ `blk-line`, `>in/l` and `->in/l` from `testing`. Modify
  \ `error1` to display the current block line. Remove trailing
  \ spaces from the `testing` messages.
  \
  \ 2018-03-12: Restore the original long messages and convert
  \ them to lowercase. Add a pause to `error1`. Compact the
  \ code, saving one block.
  \
  \ 2020-05-09: Update requirements: `depth` has been moved to
  \ the library.
  \
  \ 2020-06-03: Improve documentation.
  \
  \ 2020-06-15: Improve documentation.

  \ vim: filetype=soloforth
