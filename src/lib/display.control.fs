  \ display.control.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201805131337
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words related to printing control characters.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( printer tabulate newline )

unneeding printer ?\ : printer ( -- ) 3 channel printing on ;

  \ doc{
  \
  \ printer ( -- )
  \
  \ Select the printer as output.
  \
  \ See: `display`, `printing`.
  \
  \ }doc

unneeding tabulate ?( need column

create /tabulate 8 c,

  \ doc{
  \
  \ /tabulate ( -- ca ) "slash-tabulate"
  \
  \ _ca_ is the address of a byte containing the number of
  \ spaces that `tabulate` counts for.  Its default value is 8.
  \
  \ See `tabulate`.
  \
  \ }doc

: tabulate ( -- ) column 1+ /tabulate c@ tuck mod - spaces ; ?)

  \ doc{
  \
  \ tabulate ( -- )
  \
  \ Display the appropriate number of spaces to tabulate to the
  \ next position, using the value of `/tabulate`.
  \
  \ Note `tabulate` does not uses the "tab" control code, whose
  \ behaviour depends on the screen mode (in the default screen
  \ mode, it moves the cursor 16 positions to the right).
  \ `tabulate` prints spaces and is independent from the screen
  \ mode.
  \
  \ See `/tabulate`, `tab`.
  \
  \ }doc

unneeding newline ?( need 'cr'

create newline> 1 c, 'cr' c, 0 c,

  \ doc{
  \
  \ newline> ( -- ca ) "new-line-to"
  \
  \ _ca_ is the address of a counted string containing the
  \ character(s) (maximum 2) used to mark the start of a new
  \ line of text in file operations.
  \
  \ The string can be configured by the application. By default
  \ it contains only  the character `'cr'`.
  \
  \ The string is returned by `newline`.
  \
  \ See: `'lf'`.
  \
  \ }doc

: newline ( -- ca len ) newline> count ; ?)

  \ doc{
  \
  \ newline ( -- ca len )
  \
  \ _ca len_ is a character string containing the character(s)
  \ used to mark the start of a new line of text in file
  \ operations.
  \
  \ The string is stored at `newline>` as a counted string,
  \ which can be configured by the application.
  \
  \ Origin: Gforth.
  \
  \ See: `'cr'`, `'lf'`.
  \
  \ }doc

( 'cr' 'lf' 'tab' 'bs' crs tab tabs backspace backspaces eol? )

unneeding 'tab' ?\ 6 cconstant 'tab'  exit

  \ doc{
  \
  \ 'tab' ( -- c ) "tick-tab-tick"
  \
  \ A character constant that returns the caracter code used as
  \ tabulator (6).
  \
  \ See: `tab`, `'cr'`, `'bs'`.
  \
  \ }doc

unneeding 'bs' ?\ 8 cconstant 'bs'  exit

  \ doc{
  \
  \ 'bs' ( -- c ) "tick-b-s-tick"
  \
  \ A character constant that returns the caracter code used as
  \ backspace (8).
  \
  \ See: `'cr'`, `'tab'`.
  \
  \ }doc

unneeding 'cr' ?\ 13 cconstant 'cr'  exit

  \ doc{
  \
  \ 'cr' ( -- c ) "tick-c-r-tick"
  \
  \ A character constant that returns the caracter code used as
  \ carriage return (13).
  \
  \ See: `cr`, `crs`, `newline`, `'lf'`.
  \
  \ }doc

unneeding 'lf' ?\ 10 cconstant 'lf'  exit

  \ doc{
  \
  \ 'lf' ( -- c ) "tick-l-f-tick"
  \
  \ A character constant that returns the caracter code used as
  \ line feed (10).
  \
  \ NOTE: In the ZX Spectrum's character set, control character
  \ code 10 is not called "line feed" but "cursor down", which
  \ is analogous. ``lf`` is provided for making `read-line` and
  \ other words clearer.
  \
  \ See: `cr`, `newline`.
  \
  \ }doc

unneeding tab ?\ need 'tab'  : tab ( -- ) 'tab' emit ;

  \ doc{
  \
  \ tab ( -- )
  \
  \ `emit` a `'tab'` (character code 6), so that the next
  \ character displayed will appear at the next 16-character
  \ column.
  \
  \ See: `tabulate`.
  \
  \ }doc

unneeding backspace

?\ need 'bs'  : backspace ( -- ) 'bs'  emit ;

  \ doc{
  \
  \ backspace ( -- )
  \
  \ Emit a backspace character (character code 8).
  \
  \ See: `'bs'`.
  \
  \ }doc

unneeding crs ?\ need 'cr'  : crs   ( n -- ) 'cr'  emits ;

  \ doc{
  \
  \ crs ( n -- ) "c-r-s"
  \
  \ Emit _n_ number of cr characters (character code 13).
  \
  \ See: `cr`, `'cr'`.
  \
  \ }doc

unneeding tabs ?\ need 'tab'  : tabs ( n -- ) 'tab' emits ;

  \ doc{
  \
  \ tabs ( n -- )
  \
  \ Emit _n_ number of tab characters (character code 6).
  \
  \ See: `tab`, `'tab'`.
  \
  \ }doc

unneeding backspaces

?\ need 'bs'  : backspaces    ( n -- ) 'bs'  emits ;

  \ doc{
  \
  \ backspaces ( n -- )
  \
  \ Emit _n_ number of backspace characters (character code 8).
  \
  \ See: `backspace`, `'bs'`.
  \
  \ }doc

unneeding eol? ?( need newline need char-in-string?

: eol? ( c -- f ) newline char-in-string? ; ?)

  \ doc{
  \
  \ eol? ( c -- f ) "e-o-l-question"
  \
  \ If _c_ is one of the characters of `newline`
  \ return _true_; otherwise return _false_.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015: Add words to print cr, bs, backspaces.
  \
  \ 2016: Add `tabulate`.
  \
  \ 2016-05-01: Test and document `tabulate`.
  \
  \ 2016-05-04: Move `printer` from the kernel.
  \
  \ 2016-05-06: Fix and add conditional compilation.
  \
  \ 2016-11-26: Improve documentation.
  \
  \ 2016-12-25: Improve needing of word set `'cr' 'tab' 'bs'
  \ crs tab tabs backspace backspaces`.
  \
  \ 2017-01-07: Fix typo.
  \
  \ 2017-01-10: Complete and improve documentation.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-03-29: Improve documentation.
  \
  \ 2017-05-07: Improve documentation.
  \
  \ 2017-05-07: Improve documentation of variables.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-08: Add words' pronunciaton.
  \
  \ 2018-03-26: Add `eol?`, `newline>`, `/newline`, `newline`.
  \
  \ 2018-03-27: Make `/tabulate` a byte variable. Fix `eol?`.
  \
  \ 2018-03-28: Add `'lf'`. Improve documentation. Remove
  \ `/newline`, making `newline>` the address of a counted
  \ string. Make `eol?` check `newline`.
  \
  \ 2018-05-13: Update pronunciation.

  \ vim: filetype=soloforth
