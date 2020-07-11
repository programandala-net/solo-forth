  \ display.window.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202007112231
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ A basic implementation of text windows.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2016, 2017, 2018, 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( window )

need +field-opt-0124 need cfield:

0 cfield: ~wx         \ x cursor coordinate
  cfield: ~wy         \ y cursor coordinate
  cfield: ~wx0        \ window left x coordinate on screen
  cfield: ~wy0        \ window top y coordinate on screen
  cfield: ~wcolumns   \ width
  cfield: ~wrows      \ heigth
cconstant /window

  \ doc{
  \
  \ /window ( -- n ) "slash-window"
  \
  \ A `cconstant`. _n_ is the size in bytes of a `window` data
  \ structure.
  \
  \ See: `current-window`.
  \
  \ }doc

variable current-window

  \ doc{
  \
  \ current-window ( -- a )
  \
  \ A `variable`. _a_ is the address of a cell containing the
  \ address of the `current-window`.
  \
  \ See: `wx`, `wy`, `wx0`, `wy0`, `wcolums`, `wrows`.
  \
  \ }doc

: wx ( -- ca ) current-window @ ~wx ;

  \ doc{
  \
  \ wx ( -- ca ) "w-x"
  \
  \ _ca_ is the address of a byte containing the x cursor
  \ coordinate of the `current-window`.
  \
  \ See: `wy`, `wx0`, `wy0`, `wcolumns`, `wrows`.
  \
  \ }doc

: wy ( -- ca ) current-window @ ~wy ;

  \ doc{
  \
  \ wy ( -- ca ) "w-y"
  \
  \ _ca_ is the address of a byte containing the y cursor
  \ coordinate of the `current-window`.
  \
  \ See: `wx`, `wx0`, `wy0`, `wcolumns`, `wrows`.
  \
  \ }doc

: wx0 ( -- ca ) current-window @ ~wx0 ;

  \ doc{
  \
  \ wx0 ( -- ca ) "w-x-zero"
  \
  \ _ca_ is the address of a byte containing the left x
  \ coordinate on screen of the `current-window`.
  \
  \ See: `wx`, `wy`, `wy0`, `wcolumns`, `wrows`.
  \
  \ }doc

: wy0 ( -- ca ) current-window @ ~wy0 ;

  \ doc{
  \
  \ wy0 ( -- ca ) "w-y-zero"
  \
  \ _ca_ is the address of a byte containing the top y
  \ coordinate on screen of the `current-window`.
  \
  \ See: `wx`, `wy`, `wx0`, `wcolumns`, `wrows`.
  \
  \ }doc

: wcolumns ( -- ca ) current-window @ ~wcolumns ;

  \ doc{
  \
  \ wcolumns ( -- ca ) "w-columns"
  \
  \ _ca_ is the address of a byte containing the width
  \ in characters of the `current-window`.
  \
  \ See: `wx`, `wy`, `wx0`, `wy0`, `wrows`.
  \
  \ }doc

: wrows ( -- ca ) current-window @ ~wrows ;  -->

  \ doc{
  \
  \ wrows ( -- ca ) "w-rows"
  \
  \ _ca_ is the address of a byte containing the heigth in rows
  \ of the `current-window`.
  \
  \ See: `wx`, `wy`, `wx0`, `wy0`, `wcolumns`.
  \
  \ }doc

( window )

: window ( x0 y0 columns rows -- a )
  here >r 0 c, 0 c, 2swap swap c, c, swap c, c, r> ;

  \ XXX TODO -- Remove all the `swap` and reorder the data
  \ structure accordingly? Three cells will be saved, but the
  \ order of the fields will look strange.

  \ doc{
  \
  \ window ( col row columns rows -- a )
  \
  \ Create a window definition with top left corner at _col
  \ row_, with a width _columns_ and a height _rows_ (both in
  \ characters).  The internal cursor position of the window in
  \ set to its top left corner.  _a_ is the address of the
  \ window data structure, which is `/window` bytes long and
  \ has the following structure:

  \ .Data structure created by `window`:
  \ |===
  \ | Byte offset | Description
  \
  \ | +0          | x cursor coordinate
  \ | +1          | y cursor coordinate
  \ | +2          | window left x coordinate on screen
  \ | +3          | window top y coordinate on screen
  \ | +4          | width in columns
  \ | +5          | heigth in rows
  \ |===

  \ Windows do not use standard output words like `emit` and
  \ `type`. Instead, they use specific words named with the "w"
  \ prefix: `wemit`, `wtype`, `wcls`, etc.
  \
  \ NOTE: At the moment there's no word to display numbers in a
  \ window. Therefore numbers must be converted to strings
  \ first and displayed with `wemit`.
  \
  \ WARNING: At the moment windows are not aware of display
  \ modes that dont't use 32 characters per line (e.g.
  \ `mode-64ao`, `mode-42pw`). If windows are used when such
  \ mode is active, the layout of the output will be wrong.
  \
  \ See: `current-window`, `wx`, `wy`, `wx0`, `wy0`,
  \ `wcolumns`, `wrows`.
  \
  \ }doc

( wspace wemit wfreecolumns (wat-xy wat-xy at-wxy whome )

unneeding wspace ?\ need wemit : wspace ( -- ) bl wemit ;

  \ doc{
  \
  \ wspace ( -- ) "w-space"
  \
  \ Display one space in the `current-window`.
  \
  \ See: `space`.
  \
  \ }doc

unneeding wemit ?( need char>string need wtype

: wemit ( c -- ) char>string wtype ; ?)

  \ doc{
  \
  \ wemit ( c -- ) "w-emit"
  \
  \ Display character _c_ in the `current-window`.
  \
  \ See: `wtype`, `wspace`, `emit`.
  \
  \ }doc

unneeding wfreecolumns ?( need window

: wfreecolumns ( -- n ) wcolumns c@ wx c@ - ; ?)

  \ doc{
  \
  \ wfreecolumns ( -- n ) "w-free-columns"
  \
  \ _n_ is the number of free columns in the current line of
  \ the `current-window`.
  \
  \ See: `wcolumns`.
  \
  \ }doc

unneeding (wat-xy ?( need window need under+

: (wat-xy ( col row -- ) wx0 c@ under+ wy0 c@ + at-xy ; ?)

  \ doc{
  \
  \ (wat-xy ( col row -- ) "paren-w-at-x-y"
  \
  \ Set the cursor coordinates to `current-window` cursor
  \ coordinates _col row_.  The upper left corner of the
  \ `window` is column zero, row zero.
  \
  \ See: `wat-xy`.
  \
  \ }doc

unneeding wat-xy ?( need window need (wat-xy

: wat-xy ( col row -- ) 2dup wy c! wx c! (wat-xy ; ?)

  \ doc{
  \
  \ wat-xy ( col row -- ) "w-at-x-y"
  \
  \ Store _col row_ as the `current-window` cursor coordinates
  \ and set the cursor coordinates  accordingly.  The upper
  \ left corner of the `window` is column zero, row zero.
  \
  \ See: `at-wxy`, `at-xy`.
  \
  \ }doc

unneeding at-wxy ?( need window need (wat-xy

: at-wxy ( -- ) wx c@ wy c@ (wat-xy ; ?)

  \ doc{
  \
  \ at-wxy ( -- ) "at-w-x-y"
  \
  \ Set the cursor coordinates to the `current-window` cursor
  \ coordinates.
  \
  \ See: `wat-xy`, `at-xy`.
  \
  \ }doc

unneeding whome ?\ need wat-xy  : whome ( -- ) 0 0 wat-xy ;

  \ doc{
  \
  \ whome ( -- ) "w-home"
  \
  \ Set the `current-window` cursor coordinates to its top left
  \ corner: column zero, row zero.
  \
  \ See: `wat-xy`.
  \
  \ }doc

( wcr ?wcr wstamp wblank )


unneeding wcr ?( need window need whome

: wcr ( -- ) wy c@ dup wrows c@ 1- =
             if drop whome exit then 1+ wy c! 0 wx c! ; ?)

  \ XXX TODO -- scroll instead of `whome`

  \ doc{
  \
  \ wcr ( -- ) "w-c-r"
  \
  \ Cause subsequent output to the `current-window` appear at
  \ the beginning of the next line.
  \
  \ WARNING: When the end of the `window` is reached, the
  \ cursor is set to the top left corner with `whome`. In a
  \ future version of the code, the window will be scrolled.
  \
  \ See: `?wcr`, `wcr`.
  \
  \ }doc

unneeding ?wcr ?( need window need wcr

: ?wcr ( -- ) wx c@ 0= ?exit wcr ; ?)

  \ doc{
  \
  \ ?wcr ( -- ) "question-w-c-r"
  \
  \ If the column cursor coordinate of the `current-window` is not
  \ zero, cause subsequent output to the current window appear
  \ at the beginning of the next line.
  \
  \ WARNING: When the end of the `window` is reached, the
  \ cursor is set to the top left corner with `whome`. In a
  \ future version of the code, the window will be scrolled.
  \
  \ See: `wcr`.
  \
  \ }doc

unneeding wstamp ?( need window need ruler

: wstamp ( c -- ) wcolumns c@ ruler ( ca len )
  wy0 c@ wrows c@ bounds ?do  2dup wx0 c@ i at-xy type
                         loop 2drop ; ?)

  \ doc{
  \
  \ wstamp ( c -- ) "w-stamp"
  \
  \ Fill the `current-window` by displaying as many characters
  \ _c_ as needed, starting from the top left corner.
  \ The cursor position of the window is not changed.
  \
  \ See: `wblank`, `wcls`, `wemit`.
  \
  \ }doc

unneeding wblank

?\ need wstamp need whome : wblank ( -- ) bl wstamp whome ;

  \ doc{
  \
  \ wblank ( -- ) "w-blank"
  \
  \ Fill the `current-window` by displaying as many blanks
  \ (character `bl`) as needed, starting from the top left
  \ corner of the `window`. Finally, reset the cursor position
  \ of the window at the upper left corner (column 0, row 0).
  \
  \ ``wblank`` is a slower but lighter alternative to `wcls`.
  \
  \ See: `wstamp`, `whome`, `wspace`.
  \
  \ }doc

( wcls attr-wcls wcolor )

unneeding wcls ?( need attr@ need attr-wcls

: wcls ( -- ) attr@ attr-wcls ; ?)

  \ doc{
  \
  \ wcls ( -- ) "w-c-l-s-"
  \
  \ Clear the `current-window` with the current attribute and
  \ reset its cursor position at the upper left corner (column
  \ 0, row 0).
  \
  \ See: `attr-wcls`, `wblank`, `attr@`, `whome`,
  \ `clear-rectangle`, `cls`.
  \
  \ }doc

unneeding attr-wcls ?(

need window need clear-rectangle need whome

: attr-wcls ( c -- ) >r wx0 c@ wy0 c@ wcolumns c@ wrows c@
                     r> clear-rectangle whome ; ?)

  \ XXX TODO -- Factor out the fetching.

  \ doc{
  \
  \ attr-wcls ( b -- ) "attr-w-c-l-s"
  \
  \ Clear the `current-window` with color attribute _b_ and
  \ reset its cursor position at the upper left corner (column
  \ 0, row 0).
  \
  \ See: `wcolor`, `wcls`, `wblank`, `whome`,
  \ `clear-rectangle`, `cls`.
  \
  \ }doc

unneeding wcolor ?( need window need color-rectangle

: wcolor ( c -- ) >r wx0 c@ wy0 c@ wcolumns c@ wrows c@
                    r> color-rectangle ; ?)

  \ XXX TODO -- Factor out the fetching.

  \ doc{
  \
  \ wcolor ( b -- ) "w-color"
  \
  \ Color the `current-window` with color attribute _b_.
  \
  \ See: `attr-wcls`, `color-rectangle`.
  \
  \ }doc

( wx+! wtyped wtype+ /wtype free/wtype )

unneeding wx+! ?( need window need wcr

: wx+! ( n -- )
  wx c@ + dup wx c! wcolumns c@ = if wcr then ; ?)

  \ doc{
  \
  \ wx+! ( n -- ) "w-x-plus-store"
  \
  \ Add _n_ character positions to the column cursor coordinate
  \ of the current `window`. ``wx+!`` is a factor of `wtype+`.
  \
  \ }doc

unneeding wtyped ?\ variable wtyped

  \ doc{
  \
  \ wtyped ( -- a ) "w-typed"
  \
  \ A `variable`. _a_ is the address o a cell containing a flag
  \ indicating if a space-delimited substring was found and
  \ displayed in the `current-window`. Otherwise, the string
  \ must be broken in order to fit the current line of the
  \ `window`.
  \
  \ ``wtyped`` is used by `wtype+` and `wltype`.
  \
  \ }doc

unneeding wtype+ ?( need wx+! need wtyped

: wtype+ ( ca len -- ) tuck type wx+! wtyped on ; ?)

  \ doc{
  \
  \ wtype+ ( ca len -- ) "w-type-plus"
  \
  \ Display string _ca len_ in the `current-window` and update
  \ the `window` coordinates accordingly.
  \
  \ }doc

unneeding /wtype ?( need at-wxy need wtype+

: /wtype ( ca len len1 n -- ca' len' )
  >r >r over r> at-wxy wtype+ r> /string ; ?)

  \ doc{
  \
  \ /wtype ( ca len len1 n -- ca' len' ) "slash-w-type"
  \
  \ Display the first  _len1_ characters of string _ca len_ in
  \ the `current-window`, then remove the first _n_ characters
  \ from the string, returning the result string _ca' len'_.
  \
  \ ``/wtype`` is a factor of `wltype`.
  \
  \ See: `free/wtype`.
  \
  \ }doc

unneeding free/wtype ?( need wfreecolumns need at-wxy
                         need wtype+

: free/wtype ( ca len -- ca' len' )
  2dup wfreecolumns min dup >r at-wxy wtype+ r> /string ; ?)

  \ doc{
  \
  \ free/wtype ( ca len -- ca' len' ) "free-slash-w-type"
  \
  \ Display in the `current-window` as many characters of
  \ string _ca len_ as fit in the current line, then remove
  \ them from the string, returning the result string _ca'
  \ len'_.
  \
  \ ``free/wtype`` is a factor of `wltype` and `wtype`.
  \
  \ See: `/wtype`.
  \
  \ }doc

( wtype wltype )

unneeding wtype ?( need free/wtype

: wtype ( ca len -- )
  begin dup while free/wtype repeat 2drop ; ?)

  \ doc{
  \
  \ wtype ( ca len -- ) "w-type"
  \
  \ Display string _ca len_ in the `current-window`.
  \
  \ See: `wltype`, `wemit`, `ltype`.
  \
  \ }doc

unneeding wltype ?( need wtyped need wfreecolumns
                     need ?wcr need at-wxy need wtype+
                     need /wtype need free/wtype need +loop

: wltype ( ca len -- )
  wtyped off begin dup wfreecolumns > while
               0 wfreecolumns ?do  over i + c@ bl =
                                if i dup 1+ /wtype leave then
                            -1 +loop
               wtyped @ if ?wcr wtyped off else free/wtype then
             repeat at-wxy wtype+ ; ?)

  \ XXX TODO -- Simpler and faster. Use `scan`.

  \ doc{
  \
  \ wltype ( ca len -- ) "w-l-type"
  \
  \ Display string _ca len_ in the `current-window`, left
  \ justified.
  \
  \ See: `wtype`, `wemit`, `ltype`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-12-23: Start.
  \
  \ 2016-12-24: First working version.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-19: Remove remaining `exit` at the end of
  \ conditional interpretation.
  \
  \ 2017-01-20: Change the behaviour of `window` and
  \ `set-window` to make them more versatile; document them.
  \
  \ 2017-02-17: Fix markup in documentation.  Update cross
  \ references.
  \
  \ 2017-05-11: Improve documentation.  Rename `+wc` to `wx+!`.
  \ Rename `wtype` to `wltype`, to be consistent `with `ltype`.
  \ Rename `/wtype` to `/wltype`.
  \
  \ 2017-05-12: Convert the slow `wcls` to `wblank` and factor
  \ `wstamp` from it. Rewrite `wcls` as a wrapper of
  \ `clear-rectangle`. Remove `reset-window` and `set-window`.
  \ Remove `create` from `window`: return the data address
  \ instead. Add `wspace`. Add `wtype`. Rename ` Rename
  \ `/wltype` back to `/wtype`. Write simpler alternative to
  \ `wtype`. Add `free/wtype` and tests.
  \
  \ 2017-05-13: Fix `free/wtype`. Try the tests. Move them to
  \ the tests module.
  \
  \ 2017-05-21: Remove unnecessary requirements from `wtype`.
  \
  \ 2018-01-02: Add `attr-wcls` and rewrite `wcls` after it`.
  \ Add `wcolor`.
  \
  \ 2018-02-04: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-04-11: Remove duplicated definition.
  \
  \ 2018-06-04: Link `variable` in documentation.
  \
  \ 2020-05-04: Fix cross reference: `mode-64` -> `mode-64ao`.
  \ `mode-42` -> `mode-42pw`.
  \
  \ 2020-05-18: Update: `+loop` was moved to the library.
  \
  \ 2020-07-11: Add title to the window structure table and
  \ move it from `/window` to `window`. Add new
  \ cross-references.

\ vim: filetype=soloforth
