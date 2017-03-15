  \ printing.cursor.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201703151951
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words related to the cursor position.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( column last-column row last-row at-x at-y xy>r r>xy )

[unneeded] column
?\ : column ( -- col ) xy drop ;

  \ doc{
  \
  \ column ( -- col )
  \
  \ Current column (x coordinate).
  \
  \ See also: `row`, `last-column`, `columns`.
  \
  \ }doc

[unneeded] last-column
?\ need columns : last-column ( -- row  ) columns 1- ;

  \ doc{
  \
  \ last-column ( -- col )
  \
  \ Last column (x coordinate) in the current screen mode.
  \
  \ See also: `last-row`, `columns`, `column`.
  \
  \ }doc

[unneeded] row
?\ : row ( -- row ) xy nip ;

  \ doc{
  \
  \ row ( -- row )
  \
  \ Current row (y coordinate).
  \
  \ See also: `column`, `last-row`, `rows`.
  \
  \ }doc

[unneeded] last-row
?\ need rows  : last-row ( -- row  ) rows 1- ;

  \ doc{
  \
  \ last-row ( -- row )
  \
  \ Last row (y coordinate) in the current screen mode.
  \
  \ See also: `last-column`, `row`, `rows`.
  \
  \ }doc

[unneeded] at-x
?\ need row  : at-x ( col -- ) row at-xy ;

  \ doc{
  \
  \ at-x ( col -- )
  \
  \ Set the cursor at the given column (x coordinate) _col_ and
  \ the current row (y coordinate).
  \
  \ See also: `at-y`, `at-xy`, `row`, `column`.
  \
  \ }doc

[unneeded] at-y
?\ need column  : at-y ( row -- ) column swap at-xy ;

  \ doc{
  \
  \ at-y ( row -- )
  \
  \ Set the cursor at the current column (x coordinate) and the
  \ given row (y coordinate) _row_.
  \
  \ See also: `at-x`, `at-xy`, `row`, `column`.
  \
  \ }doc


[unneeded] xy>r ?\ : xy>r ( R: -- col row ) r>    xy 2>r >r ;

  \ doc{
  \
  \ xy>r ( -- ) ( R: -- col row )
  \
  \ Save the current cursor coordinates to the return stack.
  \
  \ See also: `r>xy`, `save-mode`.
  \
  \ }doc

[unneeded] r>xy ?\ : r>xy ( R: col row -- ) r> 2r> at-xy >r ;

  \ doc{
  \
  \ r>xy ( -- ) ( R: col row -- )
  \
  \ Restore the current cursor coordinates from the return
  \ stack.
  \
  \ See also: `xy>r`, `restore-mode`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-10-14: Add `column`, `row`, `at-x`, `at-y`, adapted
  \ from Galope.
  \
  \ 2016-05-01: Add conditional compilation and documentation.
  \
  \ 2016-05-07: Fix typos and conditional compilation.
  \
  \ 2016-11-26: Improve needing and documentation.
  \
  \ 2017-01-18: Fix `last-column` and `last-row`. Improve
  \ documentation.  Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-03-15: Add `xy>r` and `r>xy`.

  \ vim: filetype=soloforth
