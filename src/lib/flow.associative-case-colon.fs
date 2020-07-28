  \ flow.associative-case-colon.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202007282031
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ `associative-case:`.

  \ ===========================================================
  \ Authors

  \ Original code by Frank Sergeant, for Pygmy Forth.

  \ Adapted by Marcos Cruz (programandala.net), 2015, 2016,
  \ 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( associative-case: )

need create:

: associative-case: ( "name" -- )
  create:
  does> ( n -- ) ( n dfa ) cell+  \ move past `lit`
  begin   2dup @ dup 0= >r ( n a n n')
          =  r> or  0= ( n a f )
  while   ( n a ) [ 3 cells ] literal +  \ no match
  repeat  nip cell+ perform ;

  \ doc{

  \ associative-case: ( "name" -- ) "associative-case-colon"
  \
  \ Create an associative case definition "name":
  \ ``name ( i*x n -- j*x )``.

  \ Usage example:

  \ ----
  \ : red       ." red" ;
  \ : blue      ." blue" ;
  \ : orange    ." orange" ;
  \ : pink      ." pink" ;
  \ : black     ." black" ;
  \
  \ associative-case: color ( n -- )
  \   7 red  12 blue  472 orange  15 pink  0 black ;
  \
  \ 7 color cr  472 color cr  3000 color cr
  \ ----

  \ _n_ for default must be 0 and the default pair must be
  \ last.  Numbers can be in any order except 0 must be last.
  \ An actual zero or a no match causes the default to be
  \ executed.  Numbers can't be constants.
  \
  \ See also: `associative:`, `associative-list`.

  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-04-09: Fixed the file header.
  \
  \ 2017-02-22: Update markup in documentation.
  \
  \ 2017-09-09: Update notation "pfa" to the standard "dfa".
  \
  \ 2018-02-04: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2020-06-15: Improve documentation: add cross-references.

  \ vim: filetype=soloforth
