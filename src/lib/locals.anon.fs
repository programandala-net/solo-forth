  \ locals.anon.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702220020

  \ -----------------------------------------------------------
  \ Description

  \ An implementation of locals using an array of anonymous
  \ variables.

  \ -----------------------------------------------------------
  \ Authors

  \ Original code written by Leonard Morgenstern, published on
  \ Forth Dimensions (volume 6, number 1, page 33, 1984-05).
  \
  \ Adapted, modified, improved and commented by Marcos Cruz
  \ (programandala.net), 2015, 2016.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ History

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

( create-anon anon +anon n>anon )

need body>

variable (anon) ( -- a )
  \ xt of the latest anonymous variable.

: create-anon ( -- )
  here (anon) !
  [ (anon) body> @ ] literal compile, 0 , ;
  \ Create a new anonymous variable.  `(anon)` is used to get
  \ and compile the xt executed by all variables.

: anon ( Compilation: -- ) ( Run-time: -- a )
  (anon) @
  compiling? if  compile,  else  execute  then ; immediate
  \ Current anonymous variable (first cell),
  \ equivalent to ``-0 +anon``.

: +anon ( Compilation:  n -- ) ( Run-time: -- )
  cells (anon) @ execute +
  compiling? if  postpone literal  then ; immediate
  \ Current anonymous variable (cell _n_, first is 0).

: n>anon ( x1..xn n -- )
  cells postpone anon swap bounds ?do  i !  cell +loop ;
  \ Store the given _n_ cells into the current anonymous
  \ variable.

  \ Usage example:

  \ create-anon 5 cells allot
  \
  \ : test
  \   400 300 200 100 000  5 n>anon
  \   anon ?          \ prints 0
  \   123 anon !
  \   anon ?          \ prints 123
  \   [ 2 ] +anon ?   \ prints 200
  \   555 [ 2 ] +anon !
  \   [ 2 ] +anon ?   \ prints 555
  \ ;

  \ vim: filetype=soloforth