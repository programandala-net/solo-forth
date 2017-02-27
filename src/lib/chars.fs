  \ chars.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702221237

  \ -----------------------------------------------------------
  \ Description

  \ Misc words related to characters.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ History

  \ 2016-04-27: Move `ascii-char?` and `control-char?` from
  \ module "tool.dump.fsb".
  \
  \ 2016-12-03: Add `printable-ascii-char?`,
  \ `default-printable-ascii-char` and `>printable-ascii-char`.
  \ Make all words accessible to `need`.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-26: Fix requirement of `printable-ascii-char?`.

  \ XXX TODO -- move to <strings.misc.fsb>.

( ascii-char? control-char? )

[unneeded] ascii-char? ?\ : ascii-char? ( c -- f ) 128 < ;

  \ doc{
  \
  \ ascii-char?    ( c -- f )
  \
  \ Is character _c_ an ASCII character?
  \
  \ }doc

[unneeded] control-char? ?\ : control-char? ( c -- f ) bl < ;

  \ doc{
  \
  \ control-char?    ( c -- f )
  \
  \ Is character _c_ a control character?
  \
  \ }doc

( printable-ascii-char? >printable-ascii-char )

[unneeded] printable-ascii-char? dup
?\ need within
?\ : printable-ascii-char? ( c -- f ) bl 128 within ;

[unneeded] >printable-ascii-char ?exit

need printable-ascii-char?

'.' cconstant default-printable-ascii-char

: >printable-ascii-char ( c1 -- c2 )
  dup printable-ascii-char? ?exit
  drop default-printable-ascii-char ;

  \ vim: filetype=soloforth
