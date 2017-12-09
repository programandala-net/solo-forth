  \ display.control.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201705071830
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words related to printing control characters.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( printer tabulate )

[unneeded] printer
?\ : printer ( -- ) 3 channel printing on ;

  \ doc{
  \
  \ printer ( -- )
  \
  \ Select the printer as output.
  \
  \ See: `display`, `printing`.
  \
  \ }doc

[unneeded] tabulate ?(

need column

variable /tabulate  8 /tabulate !
  \ doc{
  \
  \ /tabulate ( -- a )
  \
  \ A variable. _a_ is the address of a cell containing the
  \ number of spaces that `tabulate` counts for. Its default
  \ value is 8.
  \
  \ See `tabulate`.
  \
  \ }doc

: tabulate ( -- ) column 1+ /tabulate @ tuck mod - spaces ;

?)

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

( 'cr' 'tab' 'bs' crs tab tabs backspace backspaces )

[unneeded] 'tab' ?\ 6 cconstant 'tab'  exit

  \ doc{
  \
  \ 'tab' ( -- c )
  \
  \ A character constant that returns the caracter code used as
  \ tabulator (6).
  \
  \ See: `tab`, `'cr'`, `'bs'`.
  \
  \ }doc

[unneeded] 'bs' ?\ 8 cconstant 'bs'  exit

  \ doc{
  \
  \ 'bs' ( -- c )
  \
  \ A character constant that returns the caracter code used as
  \ backspace (8).
  \
  \ See: `'cr'`, `'tab'`.
  \
  \ }doc

[unneeded] 'cr' ?\ 13 cconstant 'cr'  exit

  \ doc{
  \
  \ 'cr' ( -- c )
  \
  \ A character constant that returns the caracter code used as
  \ carriage return (13).
  \
  \ See: `cr`, `crs`.
  \
  \ }doc

[unneeded] tab
?\ need 'tab'  : tab ( -- ) 'tab' emit ;

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

[unneeded] backspace
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

[unneeded] crs
?\ need 'cr'  : crs   ( n -- ) 'cr'  emits ;

  \ doc{
  \
  \ crs ( n -- )
  \
  \ Emit _n_ number of cr characters (character code 13).
  \
  \ See: `cr`, `'cr'`.
  \
  \ }doc

[unneeded] tabs
?\ need 'tab'  : tabs ( n -- ) 'tab' emits ;

  \ doc{
  \
  \ tabs ( n -- )
  \
  \ Emit _n_ number of tab characters (character code 6).
  \
  \ See: `tab`, `'tab'`.
  \
  \ }doc

[unneeded] backspaces
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

  \ vim: filetype=soloforth
