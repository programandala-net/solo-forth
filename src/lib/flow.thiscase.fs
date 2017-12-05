  \ flow.thiscase.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201704262010
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ `thiscase`, an alternative `case` structure that makes any
  \ calculation easier.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ ===========================================================
  \ Credit

  \ Adapted and modified from code written by Wil Baden,
  \ published on Forth Dimensions (volume 8, number 5, page 29,
  \ 1987-01).

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( thiscase )

need alias

: ifcase ( x f -- )
  postpone if  postpone drop ; immediate compile-only

  \ doc{
  \
  \ ifcase ( x f -- )
  \
  \ Part of a `thiscase` structure that checks _x_. If _f_ is
  \ true, discard _x_ and continue execution; else skip the
  \ code compiled until the next `exitcase`.
  \
  \ ``ifcase`` is an `immediate` and `compile-only` word.
  \
  \ See: `othercase`.
  \
  \ }doc

: exitcase ( -- )
  postpone exit  postpone then ; immediate compile-only

  \ doc{
  \
  \ exitcase ( -- )
  \
  \ End part of a `thiscase` structure: exit the current
  \ definition.
  \
  \ ``exitcase`` is an `immediate` and `compile-only` word.
  \
  \ See: `ifcase`, `othercase`.
  \
  \ }doc

' dup alias thiscase ( x -- x x )

  \ doc{
  \
  \ thiscase ( x -- x x )
  \
  \ Mark the start of an alternative `case` structure.
  \
  \ Usage example:

  \ ----
  \ : say0      ( -- ) ." nul" ;
  \ : say1      ( -- ) ." unu" ;
  \ : say2      ( -- ) ." du" ;
  \ : say-other ( -- ) ." alia" ;
  \
  \ : test ( x -- )
  \   thiscase  0 = ifcase  say0  exitcase
  \   thiscase  1 = ifcase  say1  exitcase
  \   thiscase  2 = ifcase  say2  exitcase
  \   othercase say-other ;
  \ ----
  \
  \ See: `ifcase`, `exitcase`, `othercase`.
  \
  \ }doc

' drop alias othercase ( n -- )

  \ doc{
  \
  \ othercase ( x -- )
  \
  \ Mark the start of the default option of a `thiscase`
  \ structure that checked _x_.
  \
  \ See: `thiscase`, `ifcase`, `exitcase`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-11-14: First version.
  \
  \ 2016-03-24: Rename the words to avoid standard names
  \ `case`, `of` and `endof`.
  \
  \ 2016-04-27: Improve documentation and file header.
  \
  \ 2016-12-20: Improve documentation.
  \
  \ 2017-02-17: Update cross references and improve
  \ documentation.
  \
  \ 2017-02-27: Improve documentation.
  \
  \ 2017-04-26: Fix typo.

  \ vim: filetype=soloforth
