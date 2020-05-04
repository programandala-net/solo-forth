  \ data.storer.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202005042148
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Definer words to create "storers": words that store values.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018,
  \ 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( storer cstorer 2storer )

unneeding storer ?(

: storer ( x a "name" -- )
  create  2,  does>   ( -- ) ( dfa ) 2@ ! ; ?)

  \ doc{
  \
  \ storer ( x a "name" -- )
  \
  \ Define a word _name_ which, when executed, will cause that
  \ _x_ be stored at _a_.
  \
  \ Origin: word ``set`` found in Forth-79 (Reference Word Set)
  \ and Forth-83 (Appendix B.  Uncontrolled Reference Words).
  \
  \ }doc

unneeding cstorer ?(

: cstorer ( c ca "name" -- )
  create  2,  does>   ( -- ) ( dfa ) 2@ c! ; ?)

  \ doc{
  \
  \ cstorer ( c ca "name" -- ) "c-storer"
  \
  \ Define a word _name_ which, when executed, will cause that
  \ _c_ be stored at _ca_.
  \
  \ Origin: variant of the word ``set`` found in Forth-79
  \ (Reference Word Set) and Forth-83 (Appendix B. Uncontrolled
  \ Reference Words).
  \
  \ }doc

unneeding 2storer ?(

: 2storer ( xd a "name" -- )
  create  , 2,
  does>   ( -- ) ( dfa ) dup cell+ 2@ rot @ 2! ; ?)

  \ doc{
  \
  \ 2storer ( xd a "name" -- ) `two-storer"
  \
  \ Define a word _name_ which, when executed, will cause that
  \ _xd_ be stored at _a_.
  \
  \ Origin: variant of the word ``set`` found in Forth-79
  \ (Reference Word Set) and Forth-83 (Appendix B. Uncontrolled
  \ Reference Words).
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
  \ 2017-09-09: Update notation "pfa" to the standard "dfa".
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-07: Add words' pronunciaton.
  \
  \ 2020-05-04: Fix markup in documentation.

  \ vim: filetype=soloforth

