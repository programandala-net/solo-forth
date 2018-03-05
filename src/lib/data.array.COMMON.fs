  \ data.array.COMMON.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803052149
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words common to several implementations of arrays.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( array> array< 2array> 2array< )

unneeding array> ?(

code array> ( n a1 -- a2 )
  D1 c, E1 c, 29 c, 19 c, E5 c, jpnext, end-code ?)
  \ pop de
  \ pop hl
  \ add hl,hl
  \ add hl,de
  \ push hl
  \ _jp_next

  \ doc{
  \
  \ array> ( n a1 -- a2 ) "array-to"
  \
  \ Return address _a2_ of element _n_ of a 1-dimension
  \ single-cell array _a1_. ``array>`` is a common factor of
  \ `avalue` and `avariable`.
  \
  \ ``array>`` is written in Z80. Its equivalent definition in
  \ Forth is the following:

  \ ----
  \ : array> ( n a1 -- a2 ) swap cells + ;
  \ ----

  \ See: `2array>`, `array<`, `+perform`.
  \
  \ }doc

unneeding array< ?(

code array< ( a1 n -- a2 )
  E1 c, D1 c, 29 c, 19 c, E5 c, jpnext, end-code ?)
  \ pop hl
  \ pop de
  \ add hl,hl
  \ add hl,de
  \ push hl
  \ _jp_next

  \ doc{
  \
  \ array< ( a1 n -- a2 ) "array-from"
  \
  \ Return address _a2_ of element _n_ of a 1-dimension
  \ single-cell array _a1_.
  \
  \ ``array<`` is written in Z80. Its equivalent definition in
  \ Forth is the following:

  \ ----
  \ : array< ( a1 n -- a2 ) cells + ;
  \ ----

  \ See: `array>`, `+perform`.
  \
  \ }doc

unneeding 2array> ?(

code 2array> ( n a1 -- a2 )
  D1 c, E1 c, 29 c, 29 c, 19 c, E5 c, jpnext, end-code ?)
  \ pop de
  \ pop hl
  \ add hl,hl
  \ add hl,hl
  \ add hl,de
  \ push hl
  \ _jp_next

  \ doc{
  \
  \ 2array> ( n a1 -- a2 ) "two-array-to"
  \
  \ Return address _a2_ of element _n_ of a 1-dimension
  \ double-cell array _a1_.  ``2array>`` is a common factor of
  \ `2avalue` and `2avariable`.
  \
  \ ``2array>`` is written in Z80. Its equivalent definition in
  \ Forth is the following:

  \ ----
  \ : 2array> ( n a1 -- a2 ) swap [ 2 cells ] literal * + ;
  \ ----

  \ See: `2array<`, `array>`.
  \
  \ }doc

unneeding 2array< ?(

code 2array< ( a1 n -- a2 )
  E1 c, D1 c, 29 c, 29 c, 19 c, E5 c, jpnext, end-code ?)
  \ pop hl
  \ pop de
  \ add hl,hl
  \ add hl,hl
  \ add hl,de
  \ push hl
  \ _jp_next

  \ doc{
  \
  \ 2array< ( a1 n -- a2 ) "two-array-from"
  \
  \ Return address _a2_ of element _n_ of a 1-dimension
  \ double-cell array _a1_.
  \
  \ ``2array<`` is written in Z80. Its equivalent definition in
  \ Forth is the following:

  \ ----
  \ : 2array> ( a1 n -- a2 ) [ 2 cells ] literal * + ;
  \ ----

  \ See: `2array>,``array<`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-11-23: Add `array>`, `2array>`.
  \
  \ 2016-12-08: Rename the module filename with uppercase
  \ "COMMON", after the new convention.
  \
  \ 2016-12-20: Rename `jppushhl` to `jppushhl,`, after the
  \ change in the kernel.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-03-21: Improve documentation.
  \
  \ 2017-05-09: Remove `jppushhl,`.
  \
  \ 2018-01-04: Improve documentation.
  \
  \ 2018-02-05: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-02-20: Add `array<` and `2array<`.  Fix, update and
  \ improve documentation.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.

  \ vim: filetype=soloforth

