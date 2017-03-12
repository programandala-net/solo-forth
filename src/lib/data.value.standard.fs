  \ data.value.standard.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702280017
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ A standard implemention of `value`, `2value` and `to`.
  \
  \ This is provided as an alternative, when compatibility is
  \ required, but the code is bigger and slower than the
  \ default version provided by module
  \ "data.value.default.fsb", which uses the non-standard word
  \ `2to`.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( value 2value to )

need >body

: value ( n "name"  -- ) create  0 c, ,  does> 1+ @ ;

  \ doc{
  \
  \ value ( x "name" -- )
  \
  \ Create a definition "name" with the following execution
  \ semantics: place _x_ on the stack.
  \
  \ See `to`.
  \
  \ Origin: Forth-94 (CORE EXT), Forth-2012 (CORE EXT).
  \
  \ }doc

: 2value ( n "name"  -- ) create  1 c, 2,  does> 1+ 2@ ;

  \ doc{
  \
  \ 2value ( xd "name" -- )
  \
  \ Create a definition "name" with the following execution
  \ semantics: place _xd_ on the stack.
  \
  \ See `to`.
  \
  \ Origin: Forth-94 (CORE EXT), Forth-2012 (CORE EXT).
  \
  \ }doc

: to ( Int: i*x "name" -- ( Comp: "name" -- ( Exe: i*x -- )
  ' >body dup 1+ swap c@
  compiling? if  swap postpone literal
                 if  postpone 2!  else  postpone !  then  exit
             then
  if  2!  else  !  then
  ; immediate

  \ doc{
  \
  \ to
  \   Interpretation: ( i*x "name" -- )
  \   Compilation: ( "name" -- )
  \   Execution: ( i*x -- )
  \
  \ ``to`` is an `immediate` word.
  \
  \ Interpretation:
  \
  \ Parse "name", which is a word created by `value` or
  \ `2value`, and make _i*x_ its value.
  \
  \ Compilation:
  \
  \ Parse _name_, which is a word created by `value` or
  \ `2value`, and append the execution execution semantics
  \ given below to the current definition.
  \
  \ Execution:
  \
  \ Make _i*x_ the value of "name".
  \
  \ Origin: Forth-94 (CORE EXT), Forth-2012 (CORE EXT).
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-09-25: Benchmark.
  \
  \ 2016-05-10: Improve `2value`.
  \
  \ 2016-05-11: Document.
  \
  \ 2016-05-17: Need `>body`, which has been moved to the
  \ library.
  \
  \ 2017-02-27: Improve documentation.

  \ vim: filetype=soloforth
