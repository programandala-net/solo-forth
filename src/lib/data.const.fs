  \ data.const.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202007282031
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Definers of so called "fast constants", which work like
  \ ordinary constants, except their value is compiled as a
  \ literal. A literal is placed on the stack faster than a
  \ constant.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ Credit

  \ Words inspired by IsForth's `const`.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( const cconst 2const )

unneeding const ?(

: const ( x "name" -- )
  create immediate ,
  does>  @ executing? ?exit  postpone literal ; ?)

  \ doc{
  \
  \ const ( x "name" -- )
  \
  \ Create a fast constant _name_, with value _x_.
  \
  \ A fast constant works like an ordinary `constant`, except
  \ its value is compiled as a literal.
  \
  \ Origin: IsForth.
  \
  \ See also: `[const]`, `cconst`, `2const`.
  \
  \ }doc

unneeding cconst ?(

: cconst ( c "name" -- )
  create immediate c,
  does>  c@ executing? ?exit  postpone cliteral ; ?)

  \ doc{
  \
  \ cconst ( c "name" -- ) "c-const"
  \
  \ Create a character fast constant _name_, with value _c_.
  \
  \ A character fast constant works like an ordinary
  \ `cconstant`, except its value is compiled as a literal.
  \
  \ Origin: IsForth's `const`.
  \
  \ See also: `[cconst]`, `const`, `2const`.
  \
  \ }doc

unneeding 2const ?(

: 2const ( xd "name" -- )
  create immediate 2,
  does>  2@ executing? ?exit  postpone 2literal ; ?)

  \ doc{
  \
  \ 2const ( x1 x2 "name" -- ) "two-const"
  \
  \ Create a double fast constant _name_, with value _x1 x2_.
  \
  \ A double fast constant works like an ordinary `2constant`,
  \ except its value is compiled as a literal.
  \
  \ Origin: IsForth's `const`.
  \
  \ See also: `[2const]`, `const`, `cconst`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-11-25: Extract code from <data.misc.fsb>. Improve
  \ documentation.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-02-27: Improve documentation.
  \
  \ 2017-03-30: Improve documentation.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-07: Add words' pronunciaton.

  \ vim: filetype=soloforth
