  \ assembler.MISC.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201705091336

  \ ===========================================================
  \ Description

  \ Z80 assembler misc words, independent from the actual
  \ assembler.

  \ ===========================================================
  \ Authors

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( << >> )

[unneeded] << [unneeded] >> and (?

  \ XXX UNDER DEVELOPMENT

  \ Tool to dump assembled code to screen.

  \ Credit:
  \
  \ Original code by Frank Sergeant, for Pygmy Forth.
  \ Code adapted from Pygmy Forth.

need @c+ need for

: << ( -- a depth ) here depth ;

: >> ( a depth -- )
  depth 1- - #-258 ?throw cr base @ >r hex
  dup 4 u.r space  here over - for  c@+ 3 u.r  step drop
  r> base !  space ; ?)

  \ ===========================================================--
  \ Change log

  \ 2015, 2016: Unfinished conversion of Pygmy Forth's `>>`.
  \
  \ 2017-05-09: Rename the file.

  \ vim: filetype=soloforth

