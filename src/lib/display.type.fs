  \ display.type.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201705071833
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Versions of `type`.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( fartype type-ascii fartype-ascii )

[unneeded] fartype

?\ : fartype ( ca len -- ) bounds ?do  i farc@ emit  loop ;

  \ doc{
  \
  \ fartype ( ca len -- )
  \
  \ If _len_ is greater than zero, display the character string
  \ _ca len_, which is stored in the far memory.
  \
  \ See: `far-banks`, `type`.
  \
  \ }doc

[unneeded] type-ascii ?( need >printable-ascii-char

: type-ascii ( ca len -- )
  bounds ?do  i c@ >printable-ascii-char emit  loop ; ?)

  \ doc{
  \
  \ type-ascii ( ca len -- )
  \
  \ If _len_ is greater than zero, display the string _ca len_,
  \ replacing non-ASCII and control chars with a dot.
  \
  \ }doc

[unneeded] fartype-ascii ?( need >printable-ascii-char

: fartype-ascii ( ca len -- )
  bounds ?do  i farc@ >printable-ascii-char emit  loop ; ?)

  \ doc{
  \
  \ fartype-ascii ( ca len -- )
  \
  \ If _len_ is greater than zero, display the string _ca len_,
  \ which is stored in far memory, replacing non-ASCII and
  \ control chars with a dot.
  \
  \ See: `fartype`, `type-ascii`.
  \
  \ }doc

( drop-type padding-spaces type-left-field )

[unneeded] drop-type ?\ : drop-type ( ca len x -- ) drop type ;

  \ doc{
  \
  \ drop-type ( ca len x -- )
  \
  \ Remove _x_ from the stack and display the string _ca len_.
  \
  \ ``drop-type`` is one of the possible actions of
  \ `type-right-field` and `type-center-field`.
  \
  \ }doc

[unneeded] padding-spaces
?\ : padding-spaces ( len1 len2 -- ) swap - 0 max spaces ;

  \ doc{
  \
  \ padding-spaces ( len1 len2 -- )
  \
  \ If _len2_ minus _len1_ is a positive number, display that
  \ number of spaces; else do nothing.
  \
  \ See: `type-left-field`.
  \
  \ }doc

[unneeded] type-left-field ?( need padding-spaces

: type-left-field ( ca1 len1 len2 -- )
  2dup 2>r min type 2r> padding-spaces ; ?)

  \ doc{
  \
  \ type-left-field ( ca1 len1 len2 -- )
  \
  \ If _len1_ is greater than zero, display the character
  \ string _ca1 len1_ at the left of a field of _len2_
  \ characters.
  \
  \ See: `padding-spaces`, `type-right-field`,
  \ `type-center-field`.
  \
  \ }doc

( type-right-field )

need drop-type need <=> need array>

: type-right-field-crop ( ca1 len1 len2 -- )
  over swap - /string type ;

  \ doc{
  \
  \ type-right-field-crop ( ca1 len1 len2 -- )
  \
  \ Type string _ca1 len1_ at the right of a field of _len2_
  \ characters, which is shorter than the string.
  \
  \ See: `type-right-field`, `type-right-field-fit`.
  \
  \ }doc

: type-right-field-fit ( ca1 len1 len2 -- )
  over - spaces type ;

  \ doc{
  \
  \ type-right-field-fit ( ca1 len1 len2 -- )
  \
  \ Type string _ca1 len1_ at the right of a field of _len2_
  \ characters, which is longer than the string.
  \
  \ See: `type-right-field`, `type-right-field-crop`.
  \
  \ }doc

      ' type-right-field-fit ,
here  ' drop-type ,
      ' type-right-field-crop ,
constant type-right-field-cases
  \ Execution table of `type-right-field`.

: type-right-field ( ca1 len1 len2 -- )
  2dup <=> type-right-field-cases array> perform ;

  \ doc{
  \
  \ type-right-field ( ca1 len1 len2 -- )
  \
  \ If _len1_ is greater than zero, display the character
  \ string _ca1 len1_ at the right of a field of _len2_
  \ characters.
  \
  \ See: `type-right-field-fit`, `type-right-field-crop`,
  \ `drop-type`, `type-left-field`, `type-center-field`.
  \
  \ }doc

( type-center-field )

need drop-type need <=> need array>

: type-center-field-fit ( ca1 len1 len2 -- )
  over - 2 /mod dup >r + spaces type r> spaces ;

  \ doc{
  \
  \ type-center-field-fit ( ca1 len1 len2 -- )
  \
  \ If _len1_ is greater than zero, display the character
  \ string _ca1 len1_ at the center of a field of _len2_
  \ characters, which is longer than the string.
  \
  \ See: `type-center-field-crop`, `type-center-field`.
  \
  \ }doc

: type-center-field-crop ( ca1 len1 len2 -- )
  over swap - 2 /mod dup >r + /string r> - type ;

  \ doc{
  \
  \ type-center-field-crop ( ca1 len1 len2 -- )
  \
  \ If _len1_ is greater than zero, display the character
  \ string _ca1 len1_ at the center of a field of _len2_
  \ characters, which is shorter than the string.
  \
  \ See: `type-center-field-fit`, `type-center-field`.
  \
  \ }doc

      ' type-center-field-fit ,
here  ' drop-type ,
      ' type-center-field-crop ,
constant type-center-field-cases
  \ Execution table of `type-center-field`.

: type-center-field ( ca1 len1 len2 -- )
  2dup <=> type-center-field-cases array> perform ;

  \ doc{
  \
  \ type-center-field ( ca1 len1 len2 -- )
  \
  \ If _len1_ is greater than zero, display the character
  \ string _ca1 len1_ at the center of a field of _len2_
  \ characters.
  \
  \ See: `type-center-field-fit`,
  \ `type-center-field-crop`, `drop-type`, `type-left-field`,
  \ `type-right-field`, `gigatype-title`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-04-27: Move `ascii-type` from module "tool.dump.fsb"
  \ and rename it to `type-ascii`.
  \
  \ 2016-04-27: Start `type-center`, `type-left` and
  \ `type-right`.
  \
  \ 2016-04-28: First working versions of `type-center`,
  \ `type-left` and `type-right`.
  \
  \ 2016-11-26: Move `fartype` from the kernel. Move
  \ `fartype-ascii` from the far memory module.
  \
  \ 2016-12-03: Factor `>printable-ascii-char` from
  \ `type-ascii` and `fartype-ascii`.
  \
  \ 2017-01-10: Fix `fartype`.
  \
  \ 2017-01-18: Improve `type-center` and `type-right` with
  \ `array>`, which makes them a bit faster. Improve
  \ documentation.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-02-04: Rename `type-left`, `type-right` and
  \ `type-center` with the "field" suffix, because they erase
  \ the field with padding spaces, and there will be a parallel
  \ set of words that don't. Improve documentation.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-02-25: Improve documentation.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-03-17: Improve documentation, after the implementation
  \ of `gigatype-title`.
  \
  \ 2017-05-07: Improve documentation.

  \ vim: filetype=soloforth
