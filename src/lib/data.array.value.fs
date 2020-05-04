  \ data.array.value.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202005042226
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words to create and manage 1-dimension single-cell,
  \ double-cell and character values arrays, which behave like
  \ the standard word `value`.

  \ Usage example of a single-cell values array:

    \ 4 avalue bar
    \ 10 0 ato bar  20 1 ato bar  30 2 ato bar  40 3 ato bar
    \ 3 bar .
    \ 0 bar .
    \ 123 3 ato bar
    \ 3 bar .
    \ 1 3 +ato bar
    \ 3 bar .

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018,
  \ 2020.

  \ ===========================================================
  \ Credit

  \ Originally based on code written by Leo Wong:
  \
  \ http://forth.sourceforge.net/techniques/
  \ http://forth.sourceforge.net/techniques/arrays-lw/
  \ http://forth.sourceforge.net/techniques/arrays-lw/index-v.txt

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( avalue 2avalue cavalue )

unneeding avalue ?( need array>

: avalue ( n "name" -- )
  create  cells allot
  does> ( n -- x ) ( n dfa ) array> @ ; ?)

  \ doc{
  \
  \ avalue ( n "name" -- ) "a-value"
  \
  \ Create a 1-dimension single-cell values array _name_
  \ with _n_ elements and the execution semantics defined
  \ below.
  \
  \ _name_ execution:
  \
  \ name ( n -- x )
  \
  \ Return contents _x_ of element _n_.
  \
  \ See: `ato`, `+ato`.
  \
  \ }doc

unneeding 2avalue ?( need 2array>

: 2avalue ( n "name" -- )
  create  [ 2 cells ] literal * allot
  does> ( n -- xd ) ( n dfa ) 2array> 2@ ; ?)

  \ doc{
  \
  \ 2avalue ( n "name" -- ) "two-a-value"
  \
  \ Create a 1-dimension double-cell values array _name_
  \ with _n_ elements and the execution semantics defined
  \ below.
  \
  \ _name_ execution:
  \
  \ name ( n -- xd )
  \
  \ Return contents _xd_ of element _n_.
  \
  \ See: `2ato`.
  \
  \ }doc

unneeding cavalue ?( need align

: cavalue ( n "name" -- )
  create  allot align
  does> ( n -- x ) ( n dfa ) + c@ ; ?)

  \ doc{
  \
  \ cavalue ( n "name" -- ) "c-a-value"
  \
  \ Create a 1-dimension character values array _name_ with
  \ _n_ elements and the execution semantics defined below.
  \
  \ _name_ execution:
  \
  \ name ( n -- c )
  \
  \ Return contents _c_ of element _n_.
  \
  \ See: `cato`, `+cato`.
  \
  \ }doc

( ato +ato 2ato )

unneeding ato ?( need >body

: (ato ( x n xt -- ) >body array> ! ;

  \ doc{
  \
  \ (ato ( x n xt -- ) "paren-a-to"
  \
  \ Store _x_ into element _n_ of 1-dimension single-cell
  \ values array _xt_.
  \
  \ See: `ato`.
  \
  \ }doc

: ato ( x n "name" -- )
  compiling?  if    postpone ['] postpone (ato
              else  ' (ato  then ; immediate ?)

  \ doc{
  \
  \ ato ( x n "name" -- ) "a-to"
  \
  \ Store _x_ into element _n_ of 1-dimension single-cell
  \ values array _name_.
  \
  \ ``ato`` is an `immediate` word.
  \
  \ See: `avalue`, `(ato`.
  \
  \ }doc

unneeding +ato ?( need >body

: (+ato ( n1 n2 xt -- ) >body array> +! ;

  \ doc{
  \
  \ (+ato ( n1 n2 xt -- ) "paren-plus-a-to"
  \
  \ Add _n1_ to element _n2_ of 1-dimension single-cell
  \ values array _xt_.
  \
  \ See: `avalue`, `+ato`.
  \
  \ }doc

: +ato ( n1 n2 "name" -- )
  compiling? if    postpone ['] postpone (+ato
             else  ' (+ato  then ; immediate ?)

  \ doc{
  \
  \ +ato ( n1 n2 "name" -- ) "plus-a-to"
  \
  \ Add _n1_ to element _n2_ of 1-dimension single-cell
  \ values array _name_.
  \
  \ ``+ato`` is an `immediate` word.
  \
  \ See: `avalue`, `(+ato`.
  \
  \ }doc

unneeding 2ato ?( need >body

: (2ato ( xd n xt -- ) >body 2array> 2! ;

  \ doc{
  \
  \ (2ato ( xd n xt -- ) "paren-two-a-to"
  \
  \ Store _xd_ into element _n_ of 1-dimension double-cell
  \ values array _xt_.
  \
  \ See: `2ato`.
  \
  \ }doc

: 2ato ( xd n "name" -- )
  compiling?  if    postpone ['] postpone (2ato
              else  ' (2ato  then ; immediate ?)

  \ doc{
  \
  \ 2ato ( xd n "name" -- ) "two-a-to"
  \
  \ Store _xd_ into element _n_ of 1-dimension double-cell
  \ values array _name_.
  \
  \ ``2ato`` is an `immediate` word.
  \
  \ See: `2avalue`, `(2ato`.
  \
  \ }doc

( cato +cato )

unneeding cato ?( need >body

: (cato ( c n xt -- ) >body + c! ;

  \ doc{
  \
  \ (cato ( c n xt -- ) "paren-c-a-to"
  \
  \ Store _c_ into element _n_ of 1-dimension character
  \ values array _xt_.
  \
  \ See: `cato`.
  \
  \ }doc

: cato ( x n "name" -- )
  compiling?  if    postpone ['] postpone (cato
              else  ' (cato  then ; immediate ?)

  \ doc{
  \
  \ cato ( c n "name" -- ) "c-a-to"
  \
  \ Store _c_ into element _n_ of 1-dimension character
  \ values array _name_.
  \
  \ ``cato`` is an `immediate` word.
  \
  \ See: `cavalue`, `(cato`.
  \
  \ }doc

unneeding +cato ?(

: (+cato ( c n xt -- ) >body + c+! ;

  \ doc{
  \
  \ (+cato ( c n xt -- ) "paren-plus-c-a-to"
  \
  \ Add _c_ to element _n_ of 1-dimension character values
  \ array _xt_.
  \
  \ See: `cavalue`, `+cato`.
  \
  \ }doc

: +cato ( n1 n2 "name" -- )
  compiling? if    postpone ['] postpone (+cato
             else  ' (+cato  then ; immediate ?)

  \ doc{
  \
  \ +cato ( c n "name" -- ) "plus-c-a-to"
  \
  \ Add _c_ to element _n_ of 1-dimension character values
  \ array _name_.
  \
  \ ``+cato`` is an `immediate` word.
  \
  \ See: `cavalue`, `(+cato`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-11-15: Adapted the Wong arrays to Solo Forth.
  \
  \ 2016-04-02: Reorganized the description.
  \
  \ 2016-05-17: Need `>body`, which has been moved to the
  \ library.
  \
  \ 2016-11-22: Fix and complete documentation of all words.
  \ Rename `array` to `avalue`, to be consistent with `ato`; in
  \ fact it's an array of values, after the behaviour of the
  \ standard `value`. Rename the module accordingly.  Remove
  \ bounds checking: it's inefficient, usually not needed and
  \ easy to add at application-level.  Extend the module with
  \ double-cell and character values arrays.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-19: Remove remaining `exit` at the end of
  \ conditional interpretation, after `immediate` or
  \ `compile-only`.
  \
  \ 2017-02-17: Fix markup in documentation.  Update cross
  \ references.
  \
  \ 2017-02-27: Improve documentation.
  \
  \ 2017-09-09: Update notation "pfa" to the standard "dfa".
  \
  \ 2018-02-05: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-06-04: Update: remove trailing closing paren from
  \ word names.
  \
  \ 2020-05-04: Remove obsolete cross reference. Fix cross
  \ reference.

  \ vim: filetype=soloforth
