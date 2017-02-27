  \ data.array.COMMON.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702221550

  \ -----------------------------------------------------------
  \ Description

  \ Words common to several implementations of arrays.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2016, 2017.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ History

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

( array> 2array> )

[unneeded] array> ?(
code array> ( n a1 -- a2 )
  D1 c, E1 c, 29 c, 19 c, jppushhl, end-code ?)
  \ pop de
  \ pop hl
  \ add hl,hl
  \ add hl,de
  \ jp pushhl

  \ doc{
  \
  \ array> ( n a1 -- a2 )
  \
  \ Return address _a2_ of element _n_ of a 1-dimension
  \ single-cell array _a1_. This word is a common factor
  \ of `avalue` and `avariable`. It's written in Z80 but
  \ its equivalent definition in Forth is the following:
  \
  \ ----
  \ : array> ( n a1 -- a2 )
  \   swap cells + ;
  \ ----
  \
  \ }doc

[unneeded] 2array> ?(
code 2array> ( n a1 -- a2 )
  D1 c, E1 c, 29 c, 29 c, 19 c, jppushhl, end-code ?)
  \ pop de
  \ pop hl
  \ add hl,hl
  \ add hl,hl
  \ add hl,de
  \ jp pushhl

  \ doc{
  \
  \ 2array> ( n a1 -- a2 )
  \
  \ Return address _a2_ of element _n_ of a 1-dimension
  \ single-cell array _a1_.  This word is a common factor of
  \ `2avalue` and `2avariable`. It's written in Z80 but its
  \ equivalent definition in Forth is the following:
  \
  \ ----
  \ : 2array> ( n a1 -- a2 )
  \   swap [ 2 cells ] literal * + ;
  \ ----
  \
  \ }doc

  \ vim: filetype=soloforth

