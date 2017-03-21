  \ flow.case.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201703190106
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Several implementations of the standard Eaker's `case`
  \ structure and some variants of `of`.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( case )

  \ Credit:
  \
  \ Code adapted and modified from eForth.

  \ When `alias` is already defined,
  \ this version uses 40 bytes; else it uses 51 bytes.

[defined] alias dup 0= ?\   ' 0 alias case
                       ?\ 0 cconstant case
                       immediate compile-only
  \ doc{
  \
  \ case  ( -- 0 )
  \
  \ ``case`` is an `immediate` and `compile-only` word.
  \
  \ Origin: Forth-94 (CORE EXT), Forth-2012 (CORE EXT).
  \
  \ }doc

: of
  \ Compilation: ( -- orig )
  \ Run-time: ( x1 x2 -- )
  postpone over  postpone =  postpone if  postpone drop ;
  immediate compile-only

  \ doc{
  \
  \ of
  \   Compilation: ( -- orig )
  \   Run-time: ( x1 x2 -- )
  \
  \ ``of`` is an `immediate` and `compile-only` word.
  \
  \ Origin: Forth-94 (CORE EXT), Forth-2012 (CORE EXT).
  \
  \ }doc

[defined] alias dup 0=
?\ ' else alias endof ( orig1 -- orig2 )
?\ : endof ( orig1 -- orig2 ) postpone else ;
immediate compile-only

  \ doc{
  \
  \ endof ( orig1 -- orig2 )
  \
  \ Mark the end of an `of` clause of the `case` structure.
  \
  \ ``endof`` is an `immediate` and `compile-only` word.
  \
  \ Origin: Forth-94 (CORE EXT), Forth-2012 (CORE EXT).
  \
  \ }doc

: endcase
  \ Compilation: ( 0 orig[1]..orig[n] -- )
  \ Run-time: ( x -- )
  postpone drop  begin ?dup while postpone then repeat ;
  immediate compile-only

  \ doc{
  \
  \ endcase
  \   Compilation: ( 0 orig1..orign -- )
  \   Run-time: ( x -- )
  \
  \ ``endcase`` is an `immediate` and `compile-only` word.
  \
  \ Origin: Forth-94 (CORE EXT), Forth-2012 (CORE EXT).
  \
  \ }doc

( between-of )

  \ Credit:
  \
  \ Code from Galope.

need between

: (between-of) ( x1 x2 x3 -- x1 x1 | x1 x1' )
  2>r dup dup 2r> between 0= if  invert  then ;

: between-of
  \ Compilation: ( -- of-sys )
  \ Run-time: ( x1 x2 x3 -- | x1 )
  postpone (between-of) postpone of ;  immediate compile-only

  \ doc{
  \
  \ between-of
  \   Compilation: ( -- of-sys )
  \   Run-time: ( x1 x2 x3 -- | x1 )

  \
  \ ``between-of`` is an `immediate` and `compile-only` word.
  \
  \ Usage example:

  \ ----
  \ : test ( n -- )
  \   case
  \     1 of  ." one"  endof
  \     2 5 between-of  ." between two and five"  endof
  \     6 of  ." six"  endof
  \   endcase ;
  \ ----

  \
  \ }doc

( less-of greater-of )

  \ Credit:
  \
  \ Code from Galope.

[unneeded] less-of ?(

[defined] nup ?\ : nup ( x1 x2 -- x1 x1 x2 ) over swap ;

: (less-of) ( x1 x2 -- x1 x1 | x1 x1' )
  nup nup >= if  invert  then ;

  \ doc{
  \
  \ (less-of) ( x1 x2 -- x1 x1 | x1 x1' )
  \
  \ The run-time factor of `less-of`.
  \
  \ }doc

: less-of
  \ Compilation: ( -- of-sys )
  \ Run-time: ( x1 x2 -- | x1 )
  postpone (less-of) postpone of ;  immediate compile-only ?)

  \ doc{
  \
  \ less-of
  \   Compilation: ( -- of-sys )
  \   Run-time: ( x1 x2 -- | x1 )

  \
  \ ``less-of`` is an `immediate` and `compile-only` word.
  \
  \ Usage example:

  \ ----
  \ : test ( x -- )
  \   case
  \     10 of      ." ten!"         endof
  \     15 less-of ." less than 15" endof
  \     ." greater than 14"
  \   endcase ;
  \ ----

  \ See also: `greater-of`, `(less-of)`.
  \
  \ }doc

: (greater-of) ( x1 x2 -- x1 x1 | x1 x1' )
  nup nup <= if  invert  then ;

  \ doc{
  \
  \ (greater-of) ( x1 x2 -- x1 x1 | x1 x1' )
  \
  \ The run-time factor of `greater-of`.
  \
  \ }doc

: greater-of
  \ Compilation: ( -- of-sys )
  \ Run-time: ( x1 x2 -- | x1 )
  postpone (greater-of) postpone of ; immediate compile-only

  \ Usage example:


  \ doc{
  \
  \ greater-of
  \   Compilation: ( -- of-sys )
  \   Run-time: ( x1 x2 -- | x1 )

  \
  \ ``greater-of`` is an `immediate` and `compile-only` word.
  \
  \ Usage example:

  \ ----
  \ : test ( x -- )
  \   case
  \     10 of         ." ten!"            endof
  \     15 greater-of ." greater than 15" endof
  \     ." less than 10 or 11..15"
  \   endcase ;
  \ ----

  \ See also: `less-of`.
  \
  \ }doc

( any-of default-of )

[unneeded] any-of ?( need any? need pick

: (any-of) ( x0 x1..xn n -- x0 x0 | x0 0 )
  dup 1+ pick >r any? r> tuck and ;

  \ doc{
  \
  \ (any-of) ( x0 x1..xn n -- x0 x0 | x0 0 )
  \
  \ The run-time factor of `any-of`.  If _x0_ equals any of
  \ _x1..xn_, return _x0 x0_; else return _x0 0_.
  \
  \ }doc

: any-of
  \ Compilation: ( -- of-sys )
  \ Run-time: ( x0 x1..xn n -- | x0 )
  postpone (any-of) postpone of ; immediate compile-only ?)

  \ doc{
  \
  \ any-of
  \   Compilation: ( C: -- of-sys )
  \   Run-time: ( x0 x1..xn n -- | x0 )

  \
  \ Compilation:
  \
  \ Put _of-sys_ onto the control flow stack. Append the
  \ run-time semantics given below to to the current
  \ definition. The semantics are incomplete until resolved by
  \ a consumer of _of-sys_, such as `endof`.
  \
  \ Run-time:
  \
  \ A variant of `of`. If _x0_ equals any of _x1..xn_, discard
  \ _x1..xn n_ and continue execution at the location specified
  \ by the consumer of _of-sys_, e.g., following the next
  \ `endof`. Otherwise, consume also _x0_ and continue
  \ execution in line.

  \ Usage example:

  \ ----
  \ : test ( n -- )
  \   case
  \     1 of            ." one"               endof
  \     2 7 10 3 any-of ." two, seven or ten" endof
  \     6 of            ." six"               endof
  \   endcase ;
  \ ----

  \ See also: `case`, `endcase`, `(any-of)`.
  \
  \ }doc

  \ Credit:
  \
  \ Code from Galope.  Originally based on code by Mark Willis
  \ posted to <comp.lang.forth>:
  \ Message-ID: <64b90787-344c-4ee0-a0e4-4e2c12b3dec3@googlegroups.com>
  \ Date: Fri, 24 Jan 2014 02:08:22 -0800 (PST)

: default-of ( -- )
  postpone dup postpone of ; immediate compile-only

  \ Usage example:

  \ : test ( x -- )
  \   case
  \     1 of       ." one"    endof
  \     2 of       ." two"    endof
  \     default-of ." other"  endof
  \   endcase ;

( within-of or-of )

  \ Credit:
  \
  \ Code from Galope.

[unneeded] within-of ?( need within

: (within-of) ( x1 x2 x3 -- x1 x1 | x1 x1' )
  2>r dup dup 2r> within 0= if  invert  then ;

: within-of
  \ Compilation: ( -- of-sys )
  \ Run-time: ( x1 x2 x3 -- | x1 )
  postpone (within-of) postpone of ; immediate compile-only ?)

  \ XXX TODO confirm the ranges in the example:

  \ Usage example:

  \ : test ( x -- )
  \   case
  \     1 of          ." one"                 endof
  \     2 5 within-of ." within two and five" endof
  \     6 of          ." six"                 endof
  \   endcase ;

  \ Credit:
  \
  \ Code from Galope.

: (or-of) ( x1 x2 x3 -- x1 x1 | x1 x1' )
  2>r dup dup dup r> = swap r> = or 0= if  invert  then ;

: or-of
  \ Compilation: ( -- of-sys )
  \ Run-time: ( x1 x2 x3 -- | x1 )
  postpone (or-of) postpone of ; immediate compile-only

  \ Usage example:

  \ : test ( x -- )
  \   case
  \     1 of      ." one"          endof
  \     2 3 or-of ." two or three" endof
  \     4 of      ." four"         endof
  \   endcase ;

  \ ===========================================================
  \ Change log

  \ 2016-04-24: Add `need pick`, because `pick` has been moved
  \ from the kernel to the library.
  \
  \ 2016-05-06: Replace two remaining `[compile]` with
  \ `postpone`.
  \
  \ 2016-08-05: Compact the code to save some blocks.
  \
  \ 2016-11-16: Update the space used by every version. Make
  \ the default version of `case` use `alias` if it's already
  \ defined.
  \
  \ 2017-01-19: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-03-17: Use `cconstant` instead of `constant`. Update
  \ style of stack comments. Remove all alternative
  \ implementations of `case`. Improve documentation.
  \
  \ 2017-03-19: Improve documentation.

  \ vim: filetype=soloforth
