  \ meta.tester.hayes.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202006081616
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Tester needed to run the Hayes test.

  \ ===========================================================
  \ Authors

  \ John Hayes S1I, 1995-11-27.

  \ Marcos Cruz (programandala.net) adapted it to Solo Forth,
  \ 2015, 2016, 2017, 2018, 2020.

  \ ===========================================================
  \ License

  \ Original version:

  \ (C) 1995 JOHNS HOPKINS UNIVERSITY / APPLIED PHYSICS
  \ LABORATORY MAY BE DISTRIBUTED FREELY AS LONG AS THIS
  \ COPYRIGHT NOTICE REMAINS.  VERSION 1.1

  \ This version:

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( hayes-tester )

need where need do need depth need ?

variable verbose  verbose on
  \ Set this flag to true for more verbose output;
  \ this may allow you to tell which test caused your system to
  \ hang.

: testing( ( "ccc<paren>" -- )
  verbose @ if cr ." Testing " postpone .( exit then
            postpone ( ;
  \ Talking comment.

: empty-stack ( i*x -- )
  depth ?dup if dup 0< if   negate 0 do 0 loop
                       else 0 do drop loop
                       then then ;
  \ Empty stack. Handle underflowed stack too.

: test-error ( -- )
  cr ." Use WHERE to see the error line." empty-stack abort ;
  \ Complete an error message and abort.

variable actual-depth \ stack record
create actual-results $20 cells allot

: { ( -- ) ;

  \ doc{
  \
  \ { ( -- )
  \
  \ Part of `hayes-tester`: Start a Hayes test.
  \
  \ See: `->`, `}`.
  \
  \ }doc

-->

( hayes-tester )

: -> ( i*x -- )
  depth dup actual-depth ! \ record depth
  ?dup if 0 do actual-results i cells + ! loop then ;

  \ doc{
  \
  \ -> ( i*x -- )
  \
  \ Part of the `hayes-test`: Record depth and content of
  \ stack.
  \
  \ See: `{`, `}`.
  \
  \ }doc

: } ( i*x -- )
  depth actual-depth @ = if \ depths match
    depth ?dup if \ there is something on the stack
      0 do actual-results i cells + @
           <> if cr ." Incorrect result" test-error leave then
           \ Compare actual stack item with expected.
      loop
    then
  else cr ." Wrong number of results:"
       cr ." Expected=" actual-depth ? cr ." Actual=" depth .
       test-error then ;

  \ doc{
  \
  \ } ( i*x -- )
  \
  \ Part of `hayes-tester`: End a Hayes test by comparing stack
  \ (expected) contents with saved (actual) contents.
  \
  \ See: `{`, `->`.
  \
  \ }doc

: hayes-tester ( -- ) ;

  \ doc{
  \
  \ hayes-tester ( -- )
  \
  \ Do nothing. This word is used just for doing ``need
  \ hayes-tester`` in order to load `{`, `->`, and `}`, which
  \ are used by `hayes-test`.
  \
  \ Usage example:

  \ ....
  \ { 1 2 3 swap -> 1 3 2 } ok
  \ { 1 2 3 swap -> 1 2 2 } Incorrect result
  \ Use WHERE to see the error line.
  \ { 1 2 3 swap -> 1 2 } Wrong number of results:
  \ Expected=3
  \ Actual=2
  \ Use WHERE to see the error line.
  \ ....

  \ See: `ttester`, `forth2012-test-suite`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-05-09: First version.
  \
  \ 2017-02-19: Need `do`, which has been moved to the library.
  \
  \ 2018-03-09: Update source style (spaces). Improve
  \ documentation.
  \
  \ 2018-03-10: Fix `}`: the depths in the error message were
  \ exchanged. Update stack notation and documentation.
  \
  \ 2018-03-11: Activate `verbose` by default.
  \
  \ 2020-05-09: Update requirements: `depth` has been moved to
  \ the library.
  \
  \ 2020-06-03: Improve documentation.
  \
  \ 2020-06-08: Need `?`, which has been moved to the library.

  \ vim: filetype=soloforth
