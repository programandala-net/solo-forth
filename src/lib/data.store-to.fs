  \ data.store-to.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201703302006
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words that change the value of constants.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( !> 2!> c!> )

  \ Credit:
  \
  \ Words inspired by IsForth's `!>`.

need >body

[unneeded] !> ?(

: !>
  \   Interpretation: ( x "name" -- )
  \   Compilation:    ( "name" -- )
  \   Run-time:       ( x -- )
  ' >body compiling? if    postpone literal postpone ! exit
                     then  ! ; immediate ?)

  \ doc{
  \
  \ !>
  \   Interpretation: ( x "name" -- )
  \   Compilation:    ( "name" -- )
  \   Run-time:       ( x -- )
  \ "store-to"

  \
  \ A simpler and faster alternative to standard `to` and
  \ `value`.
  \
  \ ``!>`` is an `immediate` word.
  \
  \ Interpretation:
  \
  \ Parse _name_, which is the name of a word created by
  \ `constant` or `const`, and make _x_ its value.
  \
  \ Compilation:
  \
  \ Parse _name_, which is a word created by `constant` or
  \ `const`, and append the run-time semantics given below to
  \ the current definition.
  \
  \ Run-time:
  \
  \ Make _x_ the current value of constant _name_.
  \
  \ Origin: IsForth.
  \
  \ See: `c!>`, `2!>`.
  \
  \ }doc

[unneeded] 2!> ?(

: 2!>
  \   Interpretation: ( xd "name" -- )
  \   Compilation:    ( "name" -- )
  \   Run-time:       ( xd -- )
  ' >body compiling? if    postpone literal postpone 2! exit
                     then  2! ; immediate ?)

  \ doc{
  \
  \ 2!>
  \   Interpretation: ( xd "name" -- )
  \   Compilation:    ( "name" -- )
  \   Run-time:       ( xd -- )
  \ "two-store-to"

  \
  \ A simpler and faster alternative to standard `to` and
  \ `2value`.
  \
  \ ``2!>`` is an `immediate` word.
  \
  \ Interpretation:
  \
  \ Parse _name_, which is the name of a word created by
  \ `2constant` or `2const`, and make _xd_ its value.
  \
  \ Compilation:
  \
  \ Parse _name_, which is a word created by `2constant` or
  \ `2const`, and append the run-time semantics given below to
  \ the current definition.
  \
  \ Run-time:
  \
  \ Make _xd_ the current value of double-cell constant _name_.
  \
  \ Origin: IsForth's ``!>``.
  \
  \ See: `!>`, `c!>`.
  \
  \ }doc

[unneeded] c!> ?(

: c!>
  \   Interpretation: ( c "name" -- )
  \   Compilation:    ( "name" -- )
  \   Run-time:       ( c -- )
  ' >body compiling? if    postpone literal postpone c! exit
                     then  c! ; immediate ?)

  \ doc{
  \
  \ c!>
  \   Interpretation: ( c "name" -- )
  \   Compilation:    ( "name" -- )
  \   Run-time:       ( c -- )
  \ "c-store-to"

  \
  \ A simpler and faster alternative to standard `to` and
  \ `value`.
  \
  \ ``c!>`` is an `immediate` word.
  \
  \ Interpretation:
  \
  \ Parse _name_, which is the name of a word created by
  \ `cconstant` or `cconst`, and make _c_ its value.
  \
  \ Compilation:
  \
  \ Parse _name_, which is a word created by `cconstant` or
  \ `cconst`, and append the run-time semantics given below to
  \ the current definition.
  \
  \ Run-time:
  \
  \ Make _c_ the current value of the character constant
  \ _name_.
  \
  \ Origin: IsForth's ``!>``.
  \
  \ See: `!>`, `2!>`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-05-10: First version.
  \
  \ 2016-05-17: Need `>body`, which has been moved to the
  \ library.
  \
  \ 2016-11-17: Update documentation with `const`, `cconst` and
  \ `2const`.
  \
  \ 2016-11-18: Add missing references to `const`, `cconst` and
  \ `2const` in documentation.
  \
  \ 2017-02-17: Update cross references and improve
  \ documentation.
  \
  \ 2017-02-27: Improve documentation.
  \
  \ 2017-03-30: Improve documentation.

  \ vim: filetype=soloforth
