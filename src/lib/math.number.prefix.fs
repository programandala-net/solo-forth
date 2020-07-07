  \ math.number.prefix.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202007080018
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Numeric prefix words. Solo Forth recognizes the standard
  \ notations, but these words may be useful in some cases.

  \ ===========================================================
  \ Author

  \ XXX TODO -- update

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018,
  \ 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( base# b# d# h# )

  \ Credit:
  \
  \ Based on code from eForth and code written by Wil Baden
  \ (published on Forth Dimensions 20-3, p. 27).

need evaluate need catch

: base# ( -- ) ( "name" -- )
  create c, immediate
  does> c@
  base c@ >r  base !    \ save and set radix
  parse-name            \ get string
  ['] evaluate catch    \ convert to number, set trap
  r> base !  throw ;   \ restore radix before error control

 2 base# b#
10 base# d#
16 base# h#

( x# b# o# d# h# t# )

  \ Credit:
  \
  \ Code from eForth.

need evaluate need catch

: x# ( -- ) ( "name" -- n | d )
  does> c@              \ new radix
  base @ >r  base !     \ save and set radix
  parse-name            \ get string
  ['] evaluate catch    \ convert to number, set trap
  r> base !  throw ;   \ restore radix before error control

create b# ( "name" -- n | d ) 2 c, x# immediate
create o# ( "name" -- n | d ) 2 c, x# immediate
create d# ( "name" -- n | d ) 10 c, x# immediate
create h# ( "name" -- n | d ) 16 c, x# immediate
create t# ( "name" -- n | d ) 36 c, x# immediate

( c# )

  \ Credit:
  \
  \ Code inspired by eForth.

: c# ( "name" -- c )
  parse-name drop c@
  compiling? if postpone literal then ; immediate

  \ doc{
  \
  \ c# ( "name" -- c ) "c-number-sign"
  \
  \ Parse _name_ and return the code _c_ of the its first
  \ character.
  \
  \ ``c#`` is a short and state-smart alternative to the
  \ standard words `char` and `[char]`.
  \
  \ ``c#`` is an `immediate` word.
  \
  \ WARNING: ``c#`` is a state-smart word (see: `state`).
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-05-14: Update: `evaluate` has been moved to the
  \ library.
  \
  \ 2016-11-26: Need `catch`, which has been moved to the
  \ library.
  \
  \ 2017-02-27: Improve documentation.
  \
  \ 2018-03-07: Add words' pronunciaton.
  \
  \ 2020-05-24: Replace "hash" notation with "number sign".
  \
  \ 2020-06-15: Improve documentation.
  \
  \ 2020-07-08: Improve documentation.

  \ vim: filetype=soloforth
