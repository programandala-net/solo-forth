  \ chars.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201709091049
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Misc words related to characters.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ XXX TODO -- move to <strings.misc.fsb>.

( ascii-char? control-char? )

[unneeded] ascii-char? ?\ : ascii-char? ( c -- f ) 127 < ;

  \ doc{
  \
  \ ascii-char? ( c -- f )
  \
  \ Is character _c_ an ASCII character, i.e. in the range
  \ 0..126?
  \
  \ See also: `printable-ascii-char?`, `control-char?`.
  \
  \ }doc

[unneeded] control-char? ?\ : control-char? ( c -- f ) bl < ;

  \ doc{
  \
  \ control-char? ( c -- f )
  \
  \ Is character _c_ a control character, i.e. in the range
  \ 0..31?
  \
  \ See also: `ascii-char?`.
  \
  \ }doc

( printable-ascii-char? >printable-ascii-char )

[unneeded] printable-ascii-char? ?(  need within

: printable-ascii-char? ( c -- f ) bl 127 within ; ?)

  \ doc{
  \
  \ printable-ascii-char? ( c -- f )
  \
  \ Is _c_ a printable ASCII character, i.e. in the range
  \ 32..126?
  \
  \ See also: `ascii-char?`, `>printable-ascii-char`.
  \
  \ }doc

[unneeded] >printable-ascii-char ?(

need printable-ascii-char?

'.' cconstant default-printable-ascii-char

  \ doc{
  \
  \ default-printable-ascii-char ( -- c )
  \
  \ Return the default ASCII character _c_ used by
  \ `>printable-ascii-char`.
  \
  \ }doc

: >printable-ascii-char ( c1 -- c1 | c2 )
  dup printable-ascii-char? ?exit
  drop default-printable-ascii-char ; ?)

  \ doc{
  \
  \ >printable-ascii-char ( c1 -- c1 | c2 )
  \
  \ If character _c1_ is a printable ASCII character, return
  \ it, else return the character returned by
  \ `default-printable-ascii-char`.
  \
  \ See also: `printable-ascii-char?`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

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
  \
  \ 2017-03-28: Improve documentation. Fix `ascii-char?` and
  \ `printable-ascii-char?`: the higher ASCII character is 126,
  \ not 127.
  \
  \ 2017-09-09: Improve and fix documentation.

  \ vim: filetype=soloforth
