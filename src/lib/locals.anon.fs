  \ locals.anon.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201705051618
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ An implementation of locals using an array of anonymous
  \ variables.

  \ ===========================================================
  \ Authors

  \ Original code written by Leonard Morgenstern, published on
  \ Forth Dimensions (volume 6, number 1, page 33, 1984-05).
  \
  \ Adapted, modified, improved and commented by Marcos Cruz
  \ (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( anon )

need array>

variable anon> ( -- a )

  \ doc{
  \
  \ anon>  ( -- a )
  \
  \ A variable. _a_ contains the address of the buffer used by
  \ local variables defined by `set-anon` and accessed by
  \ `anon`.
  \
  \ ``anon>`` must be set by the application before compiling a
  \ word that uses `set-anon` and `anon`.  One single buffer
  \ pointed by ``anon>`` can be shared by several words,
  \ provided they dont't need to use it at the same time, e.g.
  \ because of nesting.
  \
  \ }doc

: anon \ Compilation: ( n -- ) Run-time: ( -- a )
  anon> @ array> postpone literal ; immediate compile-only

  \ doc{
  \
  \ anon
  \   Compilation:  ( n -- )
  \   Run-time:     ( -- a )

  \
  \ Compilation: Compile a reference to cell _n_ (0 index) of
  \ the buffer pointed by `anon>`.
  \
  \ Run-time: Return address _a_ of cell _n_ (0 index) of the
  \ buffer that was pointed by `anon>` during the compilation.
  \
  \ ``anon`` is an `immediate` and `compile-only` word.
  \
  \ See also: `arguments`, `local`.
  \
  \ }doc

: set-anon ( x[n-1]..x[0] n -- )
  cells anon> @ swap bounds ?do i ! cell +loop ;

  \ doc{
  \
  \ set-anon ( x[n-1]..x[0] n a -- )
  \
  \ Store the given _n_ cells into the buffer pointed by
  \ `anon>`, which will be accessed by `anon`.
  \
  \ Usage example:

  \ ----
  \ here anon> ! 5 cells allot
  \
  \ : test ( x4 x3 x2 x1 x0 -- )
  \   5 set-anon
  \   [ 0 ] anon ?     \ print _x5_
  \   123 [ 0 ] anon !
  \   [ 0 ] anon ?     \ print 123
  \   [ 2 ] anon ?     \ print _x3_
  \   555 [ 2 ] anon !
  \   [ 2 ] anon ?     \ print 555
  \   ;
  \ ----
  
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-11-14: Adapted from the original code.
  \
  \ 2016-04-09: Fixed the file header.
  \
  \ 2016-05-17: Need `body>`, which has been moved to the
  \ library.
  \
  \ 2017-02-17: Change markup of inline code that is not a
  \ cross reference.
  \
  \ 2017-02-19: Replace `do`, which has been moved to the
  \ library, with `?do`.
  \
  \ 2017-03-18: Document. Rewrite: Simplify and make more
  \ versatile.
  \
  \ 2017-03-19: Finish the new version. Update the
  \ documentation.
  \
  \ 2017-05-05: Improve documentation.

  \ vim: filetype=soloforth
