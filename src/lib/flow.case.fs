  \ flow.case.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803052149
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ An implementation of the standard Eaker's `case` structure
  \ and some variants of `of`.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

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

need cs-mark need thens

cs-mark cconstant case immediate compile-only

  \ doc{
  \
  \ case
  \   Compilation: ( C: -- case-sys )
  \   Run-time:    ( -- )

  \
  \ Compilation: Mark the start of a ``case`` .. `endcase`
  \ structure.
  \
  \ Run-time: Continue execution.
  \
  \ ``case`` is an `immediate` and `compile-only` word.
  \
  \ Origin: Forth-94 (CORE EXT), Forth-2012 (CORE EXT).
  \
  \ See: `of`, `endof`, `default-of`, `less-of`,
  \ `greater-of`, `between-of`, `within-of`, `or-of`, `any-of`,
  \ `cond`, `thens`.
  \
  \ }doc

: of
  \ Compilation: ( C: -- orig )
  \ Run-time: ( x1 x2 -- )
  postpone over  postpone =  postpone if  postpone drop ;
  immediate compile-only

  \ doc{
  \
  \ of
  \   Compilation: ( C: -- orig )
  \   Run-time:    ( x1 x2 -- )

  \
  \ ``of`` is an `immediate` and `compile-only` word.
  \
  \ Compilation: Put _orig_ onto the control flow stack.
  \ Append the run-time semantics given below to the current
  \ definition. The semantics are incomplete until resolved by
  \ a consumer of _orig_ such as `endof`.
  \
  \ Run-time: If _x1_ and _x2_ are not equal, discard _x2_ and
  \ continue execution at the location specified by the
  \ consumer of _orig_, e.g. following the next `endof`.
  \ Otherwise discard _x1 x2_ and continue execution in line.
  \
  \ ``of`` is an `immediate` and `compile-only` word.
  \
  \ Origin: Forth-94 (CORE EXT), Forth-2012 (CORE EXT).
  \
  \ See: `default-of`, `less-of`, `greater-of`, `between-of`,
  \ `within-of`, `or-of`, `any-of`.
  \
  \ }doc

: endof ( orig1 -- orig2 )
  postpone else ; immediate compile-only

  \ doc{
  \
  \ endof
  \   Compilation: ( C: orig1 -- orig2 )
  \   Run-time:    ( -- )

  \
  \ Compilation: Mark the end of an `of` clause (or any of its
  \ variants) of the `case` structure.  Resolve the forward
  \ reference _orig1_, usually left by `of`.  Put the location
  \ of a new unresolved forward reference _orig2_ onto the
  \ control-flow stack, usually to be resolved by `endcase`.
  \
  \ Run-time: Continue execution at the location specified by
  \ the consumer of _orig2_.
  \
  \ ``endof`` is equivalent to `else`.
  \
  \ ``endof`` is an `immediate` and `compile-only` word.
  \
  \ Origin: Forth-94 (CORE EXT), Forth-2012 (CORE EXT).
  \
  \ }doc

: endcase
  \ Compilation: ( C: 0 orig#1 .. orig#n -- )
  \ Run-time: ( x -- )
  postpone drop postpone thens ; immediate compile-only

  \ doc{
  \
  \ endcase
  \   Compilation: ( C: 0 orig#1 .. orig#n -- )
  \   Run-time:    ( x -- )
  \
  \ ``endcase`` is an `immediate` and `compile-only` word.
  \
  \ Origin: Forth-94 (CORE EXT), Forth-2012 (CORE EXT).
  \
  \ See: `thens`.
  \
  \ }doc

( between-of )

  \ Credit:
  \
  \ Code from Galope.

need between

: (between-of) ( x1 x2 x3 -- x1 x1 | x1 x4 )
  2>r dup dup 2r> between 0= if invert then ;

  \ doc{
  \
  \ (between-of) ( x1 x2 x3 -- x1 x1 | x1 x4 ) "paren-between-of"
  \
  \ The run-time factor of `between-of`.  If _x1_ is in range
  \ _x2 x3_, as calculated by `between`, return _x1 x1_;
  \ otherwise return _x1 x4_, being _x4_ not equal to _x1_.
  \
  \ }doc

: between-of
  \ Compilation: ( C: -- of-sys )
  \ Run-time: ( x1 x2 x3 -- | x1 )
  postpone (between-of) postpone of ;  immediate compile-only

  \ doc{
  \
  \ between-of
  \   Compilation: ( C: -- of-sys )
  \   Run-time:    ( x1 x2 x3 -- | x1 )

  \ A variant of `of`.
  \
  \ Compilation:
  \
  \ Put _of-sys_ onto the control flow stack. Append the
  \ run-time semantics given below to the current definition.
  \ The semantics are incomplete until resolved by a consumer
  \ of _of-sys_, such as `endof`.
  \
  \ Run-time:
  \
  \ If _x1_ is not in range _x2 x3_, as calculated by
  \ `between`, discard _x2 x3_ and continue execution at the
  \ location specified by the consumer of _of-sys_, e.g.,
  \ following the next `endof`. Otherwise, consume also _x1_
  \ and continue execution in line.
  \
  \ ``between-of`` is an `immediate` and `compile-only` word.
  \
  \ Usage example:

  \ ----
  \ : test ( n -- )
  \   case
  \     1           of  ." one"                  endof
  \     2 5 between-of  ." between two and five" endof
  \     6           of  ." six"                  endof
  \   endcase ;
  \ ----

  \ See: `case`, `within-of`, `(between-of)`.
  \
  \ }doc

( less-of greater-of )

  \ Credit:
  \
  \ Code from Galope.

unneeding less-of ?( need nup

: (less-of) ( n1 n2 -- n1 n1 | n1 n3 )
  nup nup >= if invert then ;

  \ doc{
  \
  \ (less-of) ( n1 n2 -- n1 n1 | n1 n3 ) "paren-less-of"
  \
  \ The run-time factor of `less-of`.
  \
  \ If _n1_ is less than _n2_, leave _n1 n1_; otherwise leave
  \ _n1 n3_, being _n3_ not equal to _n1_.
  \
  \ See: `(greater-of)`.
  \
  \ }doc

: less-of
  \ Compilation: ( C: -- of-sys )
  \ Run-time: ( x1 x2 -- | x1 )
  postpone (less-of) postpone of ; immediate compile-only ?)

  \ doc{
  \
  \ less-of
  \   Compilation: ( C: -- of-sys )
  \   Run-time:    ( x1 x2 -- | x1 )

  \
  \ ``less-of`` is an `immediate` and `compile-only` word.
  \
  \ Usage example:

  \ ----
  \ : test ( x -- )
  \   case
  \     10      of ." ten!"         endof
  \     15 less-of ." less than 15" endof
  \     ." greater than 14"
  \   endcase ;
  \ ----

  \ See: `case`, `greater-of`, `(less-of)`.
  \
  \ }doc

unneeding greater-of ?( need nup

: (greater-of) ( n1 n2 -- n1 n1 | n1 n3 )
  nup nup <= if invert then ;

  \ doc{
  \
  \ (greater-of) ( n1 n2 -- n1 n1 | n1 n3 ) "paren-greater-of"
  \
  \ The run-time factor of `greater-of`.
  \
  \ If _n1_ is greater than _n2_, leave _n1 n1_; otherwise
  \ leave _n1 n3_, being _n3_ not equal to _n1_.
  \
  \ See: `(less-of)`.
  \
  \ }doc

: greater-of
  \ Compilation: ( C: -- of-sys )
  \ Run-time: ( x1 x2 -- | x1 )
  postpone (greater-of) postpone of ; immediate compile-only ?)

  \ doc{
  \
  \ greater-of
  \   Compilation: ( C: -- of-sys )
  \   Run-time:    ( x1 x2 -- | x1 )

  \
  \ ``greater-of`` is an `immediate` and `compile-only` word.
  \
  \ Usage example:

  \ ----
  \ : test ( x -- )
  \   case
  \     10 of         ." ten!"            endof
  \     15 greater-of ." greater than 15" endof
  \     ." less than 10 or 11 .. 15"
  \   endcase ;
  \ ----

  \ See: `case`, `less-of`, `(greater-of)`.
  \
  \ }doc

( any-of default-of )

unneeding any-of ?( need any? need pick

: (any-of) ( x#0 x#1 .. x#n n -- x#0 x#0 | x#0 0 )
  dup 1+ pick >r any? r> tuck and ;

  \ doc{
  \
  \ (any-of) ( x#0 x#1 .. x#n n -- x#0 x#0 | x#0 0 ) "paren-any-of"
  \
  \ The run-time factor of `any-of`.  If _x#0_ equals any of
  \ _x#1 .. x#n_, return _x#0 x#0_; else return _x#0 0_.
  \
  \ }doc

: any-of
  \ Compilation: ( C: -- of-sys )
  \ Run-time: ( x#0 x#1 .. x#n n -- | x#0 )
  postpone (any-of) postpone of ; immediate compile-only ?)

  \ doc{
  \
  \ any-of
  \   Compilation: ( C: -- of-sys )
  \   Run-time:    ( x#0 x#1 .. x#n n -- | x#0 )

  \ A variant of `of`.
  \
  \ Compilation:
  \
  \ Put _of-sys_ onto the control flow stack. Append the
  \ run-time semantics given below to the current definition.
  \ The semantics are incomplete until resolved by a consumer
  \ of _of-sys_, such as `endof`.
  \
  \ Run-time:
  \
  \ If _x#0_ equals any of _x#1 .. x#n_, discard _x#1 .. x#n n_
  \ and continue execution at the location specified by the
  \ consumer of _of-sys_, e.g., following the next `endof`.
  \ Otherwise, consume also _x0_ and continue execution in
  \ line.

  \ ``any-of`` is an `immediate` and `compile-only` word.
  \
  \ Usage example:

  \ ----
  \ : test ( n -- )
  \   case
  \     1 of            ." one"               endof
  \     2 7 10 3 any-of ." two, seven or ten" endof
  \     6 of            ." six"               endof
  \   endcase ;
  \ ----

  \ See: `case`, `or-of`, `(any-of)`.
  \
  \ }doc

: default-of ( -- )
  \ Compilation: ( C: -- of-sys )
  \ Run-time: ( x -- )
  postpone dup postpone of ; immediate compile-only

  \ Credit:
  \
  \ Code from Galope.  Originally based on code by Mark Willis
  \ posted to <comp.lang.forth>:
  \ Message-ID: <64b90787-344c-4ee0-a0e4-4e2c12b3dec3@googlegroups.com>
  \ Date: Fri, 24 Jan 2014 02:08:22 -0800 (PST)

  \ doc{
  \
  \ default-of
  \   Compilation: ( C: -- of-sys )
  \   Run-time:    ( x -- )

  \ An alternative to mark the default clause of a `case`
  \ structure.
  \
  \ Compilation:
  \
  \ Put _of-sys_ onto the control flow stack. Append the
  \ run-time semantics given below to the current definition.
  \ The semantics are incomplete until resolved by a consumer
  \ of _of-sys_, such as `endof`.
  \
  \ Run-time:
  \
  \ Discard _x_ and continue execution.
  \
  \ ``default-of`` is an `immediate` and `compile-only` word.
  \
  \ Usage example:

  \ ----
  \ : test ( x -- )
  \   case
  \     1 of       ." one"   endof
  \     2 of       ." two"   endof
  \     default-of ." other" endof
  \   endcase ;
  \ ----

  \ }doc

( within-of or-of )

  \ Credit:
  \
  \ Code from Galope.

unneeding within-of ?( need within

: (within-of) ( x1 x2 x3 -- x1 x1 | x1 x4 )
  2>r dup dup 2r> within 0= if invert then ;

  \ doc{
  \
  \ (within-of) ( x1 x2 x3 -- x1 x1 | x1 x4 ) "paren-within-of"
  \
  \ The run-time factor of `within-of`.  If _x1_ is in range
  \ _x2 x3_, as calculated by `within`, return _x1 x1_;
  \ otherwise return _x1 x4_, being _x4_ not equal to _x1_.
  \
  \ }doc

: within-of
  \ Compilation: ( C: -- of-sys )
  \ Run-time: ( x1 x2 x3 -- | x1 )
  postpone (within-of) postpone of ; immediate compile-only ?)

  \ doc{
  \
  \ within-of
  \   Compilation: ( C: -- of-sys )
  \   Run-time:    ( x1 x2 x3 -- | x1 )

  \ A variant of `of`.
  \
  \ Compilation:
  \
  \ Put _of-sys_ onto the control flow stack. Append the
  \ run-time semantics given below to the current definition.
  \ The semantics are incomplete until resolved by a consumer
  \ of _of-sys_, such as `endof`.
  \
  \ Run-time:
  \
  \ If _x1_ is not in range _x2 x3_, as calculated by `within`,
  \ discard _x2 x3_ and continue execution at the location
  \ specified by the consumer of _of-sys_, e.g., following the
  \ next `endof`. Otherwise, consume also _x1_ and continue
  \ execution in line.
  \
  \ ``within-of`` is an `immediate` and `compile-only` word.

  \ Usage example:

  \ ----
  \ : test ( x -- )
  \   case
  \     1          of ." one"                           endof
  \     2 5 within-of ." within two and five; not five" endof
  \     5          of ." five"                          endof
  \   endcase ;
  \ ----

  \ See: `case`, `between-of`, `(within-of)`.
  \
  \ }doc

  \ Credit:
  \
  \ Code from Galope.

: (or-of) ( x1 x2 x3 -- x1 x1 | x1 x4 )
  2>r dup dup dup r> = swap r> = or 0= if invert then ;

  \ doc{
  \
  \ (or-of) ( x1 x2 x3 -- x1 x1 | x1 x4 ) "paren-or-of"
  \
  \ The run-time factor of `less-of`.
  \
  \ }doc

: or-of
  \ Compilation: ( C: -- of-sys )
  \ Run-time: ( x1 x2 x3 -- | x1 )
  postpone (or-of) postpone of ; immediate compile-only

  \ doc{
  \
  \ or-of
  \   Compilation: ( C: -- of-sys )
  \   Run-time:    ( x1 x2 x3 -- | x1 )

  \ A variant of `of`.
  \
  \ Compilation:
  \
  \ Put _of-sys_ onto the control flow stack. Append the
  \ run-time semantics given below to the current definition.
  \ The semantics are incomplete until resolved by a consumer
  \ of _of-sys_, such as `endof`.
  \
  \ Run-time:
  \
  \ If _x1_ is equal to _x2_ or _x1_ is equal to _x3_ discard
  \ _x1 x2 x3_ and continue execution in line; otherwise
  \ discard _x2 x3_ and continue execution at the location
  \ specified by the consumer of _of-sys_, e.g., following the
  \ next `endof`.
  \
  \ ``or-of`` is an `immediate` and `compile-only` word.
  \
  \ Usage example:

  \ ----
  \ : test ( x -- )
  \   case
  \     1      of ." one"          endof
  \     2 3 or-of ." two or three" endof
  \     4      of ." four"         endof
  \   endcase ;
  \ ----

  \ See: `case`, `any-of`, `(or-of)`.
  \
  \ }doc

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
  \
  \ 2017-11-27: Improve documentation. Fix needing of
  \ `greater-of`. Need `nup` instead of define it. Use `thens`
  \ in `endcase`. Test `within-of`.
  \
  \ 2017-12-09: Remove optional usage of `alias` to define
  \ `case` and `endof`, since `[defined]` is moved to the
  \ library.
  \
  \ 2017-12-10: Improve documentation.
  \
  \ 2018-01-04: Use `cs-mark` for `case`, because `thens` is
  \ used in `endcase`.
  \
  \ 2018-02-04: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.

  \ vim: filetype=soloforth
