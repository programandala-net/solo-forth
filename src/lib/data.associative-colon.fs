  \ data.associative-colon.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202007282108
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ `associative:`.

  \ ===========================================================
  \ Authors

  \ Original code from F83, by Henry Laxen and Michael Perry.

  \ Adapted by Marcos Cruz (programandala.net), 2015, 2016,
  \ 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( associative: )

: associative: ( n "name" -- )
  constant
  does> ( x -- index )
    ( x dfa )
    dup @ ( x dfa n ) -rot dup @ 0 ( n x dfa n 0 )
    do ( n x dfa )
      cell+ 2dup @ = ( n x dfa' flag )
      if  2drop drop i unloop exit  then
    loop 2drop ( n ) ;

  \ doc{

  \ associative: ( n "name" -- ) "associative-colon"

  \ Create a table lookup _name_ with _n_ entries.
  \
  \ An associative memory word.  It must be followed by a set
  \ of values to be looked up.  At runtime, the values stored
  \ in the data field are searched for a match.  If a match is
  \ made, the index to that value is returned.  If no match is
  \ made, then the number of entries is returned.  This is the
  \ inverse of an array.

  \ Usage example:

  \ ----
  \ 1000 constant zx1
  \ 200 constant zx2
  \ 30 constant zx3
  \
  \ 3 associative: unzx ( value -- n ) zx1 , zx2 , zx3 ,
  \
  \ 1000 unzx .  \ prints 0
  \ 200 unzx .   \ prints 1
  \ 30 unzx .    \ prints 2
  \ ----

  \ See also: `associative-list`, `associative-case:`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-08-11: Adapted.
  \
  \ 2016-04-09: Fixed the file header. Improved the
  \ documentation.
  \
  \ 2017-09-09: Update notation "pfa" to the standard "dfa".
  \
  \ 2017-11-06: Improve documentation with cross-reference.
  \
  \ 2018-02-05: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2020-06-15: Improve documentation: add cross-references.
  \
  \ 2020-07-28: Update notation of parsed "name" in word
  \ descriptions.

  \ vim: filetype=soloforth
