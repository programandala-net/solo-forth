  \ flow.positional-case-colon.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201802071854
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ `positional-case:`.

  \ ===========================================================
  \ Authors

  \ Original code from F83's `case:`, by Henry Laxen and
  \ Michael Perry.

  \ Adapted by Marcos Cruz (programandala.net), 2015, 2016,
  \ 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( positional-case: )

need create:

: positional-case: ( "name" -- )
  create:
  does>   ( n -- )
          \ Execute the n-th word compiled.
          ( n dfa ) swap cells + perform ;

  \ doc{
  \
  \ positional-case: ( "name" -- ) "positional-case-colon"
  \
  \ Create a positional case word _name_. At runtime, _name_
  \ will execute the n-th word compiled in its definition,
  \ depending upon the value on the stack. No range checking.
  \
  \ Usage example:

  \ ----
  \ : say0 ( -- ) ." nul" ;
  \ : say1 ( -- ) ." unu" ;
  \ : say2 ( -- ) ." du" ;

  \ positional-case: say ( n -- ) say0 say1 say2 ;

  \ 0 say cr 1 say cr 2 say cr
  \ ----

  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-08-11: Adapted.
  \
  \ 2015-11-22: Modified to use `create:`.
  \
  \ 2015-12-14: Renamed to `positional-case:`.
  \
  \ 2016-04-09: Fixed the file header. Documented.
  \
  \ 2017-09-09: Update notation "pfa" to the standard "dfa".
  \
  \ 2018-02-07: Improve documentation: add pronunciation to
  \ words that need it.

  \ vim: filetype=soloforth
