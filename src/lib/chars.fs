  \ chars.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202007282031
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Misc words related to characters.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ XXX TODO -- move to <strings.misc.fsb>.

( ascii-char? control-char? )

unneeding ascii-char? ?\ : ascii-char? ( c -- f ) 127 < ;

  \ doc{
  \
  \ ascii-char? ( c -- f ) "ascii-char-question"
  \
  \ Is character _c_ an ASCII character, i.e. in the range
  \ 0..126?
  \
  \ See also: `graphic-ascii-char?`, `control-char?`.
  \
  \ }doc

unneeding control-char? ?\ : control-char? ( c -- f ) bl < ;

  \ doc{
  \
  \ control-char? ( c -- f ) "control-char-question"
  \
  \ Is character _c_ a control character, i.e. in the range
  \ 0..31?
  \
  \ See also: `ascii-char?`.
  \
  \ }doc

( graphic-ascii-char? >graphic-ascii-char )

unneeding graphic-ascii-char? ?( need within

: graphic-ascii-char? ( c -- f ) bl 127 within ; ?)

  \ doc{
  \
  \ graphic-ascii-char? ( c -- f ) "graphic-ascii-char-question"
  \
  \ Is _c_ a printable ASCII character, i.e. in the range
  \ 32..126?
  \
  \ See also: `ascii-char?`, `>graphic-ascii-char`.
  \
  \ }doc

unneeding >graphic-ascii-char ?( need graphic-ascii-char?

'.' cconstant default-graphic-ascii-char

  \ doc{
  \
  \ default-graphic-ascii-char ( -- c )
  \
  \ A character constant. _c_ is the default ASCII graphic
  \ character used by `>graphic-ascii-char`. The value can
  \ be changed with `c!>`.
  \
  \ }doc

: >graphic-ascii-char ( c1 -- c1 | c2 )
  dup graphic-ascii-char? ?exit
  drop default-graphic-ascii-char ; ?)

  \ doc{
  \
  \ >graphic-ascii-char ( c1 -- c1 | c2 )
  \
  \ If character _c1_ is a printable ASCII character, return
  \ it, else return the character returned by
  \ `default-graphic-ascii-char`.
  \
  \ See also: `graphic-ascii-char?`.
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
  \
  \ 2018-02-05: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-23: Rename "printable-ascii-char" words
  \ "graphic-ascii-char", after the standard notation.  Old
  \ words affected: `printable-ascii-char?`
  \ `default-printable-ascii-char?`, `>printable-ascii-char`.

  \ vim: filetype=soloforth
