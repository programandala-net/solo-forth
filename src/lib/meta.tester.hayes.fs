  \ meta.tester.hayes.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702220020

  \ -----------------------------------------------------------
  \ Description

  \ Development tests.

  \ -----------------------------------------------------------
  \ Authors

  \ John Hayes S1I, 1995-11-27.

  \ Marcos Cruz (programandala.net) adapted it to Solo Forth,
  \ 2015, 2016.

  \ -----------------------------------------------------------
  \ License

  \ Original version:

  \ (C) 1995 JOHNS HOPKINS UNIVERSITY / APPLIED PHYSICS
  \ LABORATORY MAY BE DISTRIBUTED FREELY AS LONG AS THIS
  \ COPYRIGHT NOTICE REMAINS.  VERSION 1.1

  \ This version:

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ History

  \ 2016-05-09: First version.
  \
  \ 2017-02-19: Need `do`, which has been moved to the library.

( hayes-tester )

need where need do

variable verbose  verbose off
  \ Set this flag to true for more verbose output;
  \ this may allow you to tell which test caused your system to
  \ hang.

: testing( ( "ccc<paren>" -- )
  verbose @ if    cr ." Testing " postpone .(  exit  then
            postpone ( ;
  \ Talking comment.

: empty-stack ( i*x -- )
  depth ?dup if  dup 0< if    negate 0 do  0  loop
                        else  0 do  drop  loop  then  then ;
  \ Empty stack. Handle underflowed stack too.

: test-error ( -- )
  cr ." Use WHERE to see the error line." empty-stack  abort ;
  \ Complete an error message and abort.

variable actual-depth  \ stack record
create actual-results $20 cells allot

: { ( -- ) ;  \ syntactic sugar.

-->

( hayes-tester )

: -> ( i*x -- )
  depth dup actual-depth !  \ record depth
  ?dup if  0 do actual-results i cells + ! loop  then ;
  \ Record depth and content of stack.

: } ( ... -- )
  depth actual-depth @ = if  \ depths match
    depth ?dup if  \ there is something on the stack
      0 do  \ for each stack item
        actual-results i cells + @
          \ compare actual with expected
        <> if  cr ." Incorrect result" test-error leave  then
      loop
    then
  else  cr ." Wrong number of results:"
        cr ." Expected=" depth . cr ." Actual=" actual-depth ?
        test-error
  then ;
  \ Compare stack (expected) contents with saved (actual)
  \ contents.

  \ vim: filetype=soloforth
