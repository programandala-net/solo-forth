  \ display.window.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201705111535
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Basic implementation of text windows, which use specific
  \ printing words and share the global color attributes.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( window set-window )

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
  \ /window ( -- n )
  \
  \ _n_ is the data size in bytes of a `window`.
  \
  \ }doc

variable current-window

  \ doc{
  \
  \ current-window ( -- a )
  \
  \ A variable. _a_ is the address of a cell containing the
  \ address of the `current-window`.
  \
  \ See also: `set-window`.
  \
  \ }doc

: wx ( -- ca ) current-window @ ~wx ;

  \ doc{
  \
  \ wx ( -- ca )
  \
  \ _ca_ is the address of a byte containing the x cursor
  \ coordinate of the `current-window`.
  \
  \ See also: `wy`, `wx0`, `wy0`, `wcolumns`, `wrows`.
  \
  \ }doc

: wy ( -- ca ) current-window @ ~wy ;

  \ doc{
  \
  \ wy ( -- ca )
  \
  \ _ca_ is the address of a byte containing the y cursor
  \ coordinate of the `current-window`.
  \
  \ See also: `wx`, `wx0`, `wy0`, `wcolumns`, `wrows`.
  \
  \ }doc

: wx0 ( -- ca ) current-window @ ~wx0 ;

  \ doc{
  \
  \ wx0 ( -- ca )
  \
  \ _ca_ is the address of a byte containing the left x
  \ coordinate on screen of the `current-window`.
  \
  \ See also: `wx`, `wy`, `wy0`, `wcolumns`, `wrows`.
  \
  \ }doc

: wy0 ( -- ca ) current-window @ ~wy0 ;

  \ doc{
  \
  \ wy0 ( -- ca )
  \
  \ _ca_ is the address of a byte containing the top y
  \ coordinate on screen of the `current-window`.
  \
  \ See also: `wx`, `wy`, `wx0`, `wcolumns`, `wrows`.
  \
  \ }doc

: wcolumns ( -- ca ) current-window @ ~wcolumns ;

  \ doc{
  \
  \ wcolumns ( -- ca )
  \
  \ _ca_ is the address of a byte containing the width
  \ in characters of the `current-window`.
  \
  \ See also: `wx`, `wy`, `wx0`, `wy0`, `wrows`.
  \
  \ }doc

: wrows ( -- ca ) current-window @ ~wrows ;  -->

  \ doc{
  \
  \ wrows ( -- ca )
  \
  \ _ca_ is the address of a byte containing the heigth in rows
  \ of the `current-window`.
  \
  \ See also: `wx`, `wy`, `wx0`, `wy0`, `wcolumns`.
  \
  \ }doc

( window set-window )

: window ( x0 y0 columns rows "name" -- )
  create  0 c, 0 c, 2swap swap c, c, swap c, c, ;

  \ doc{
  \
  \ window ( x0 y0 columns rows "name" -- )
  \
  \ Create a window called _name_: _x0 y0_ is the position of
  \ its top left corner on the screen, and _columns rows_ is
  \ its size in characters.  Its cursor position is set to its
  \ top left corner.
  \
  \ Later execution of _name_ will leave the address of its
  \ data structure on the stack:

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

  \ See also: `set-window`, `reset-window`, `current-window`,
  \ `wx`, `wy`, `wx0`, `wy0`, `wcolumns`, `wrows.
  \
  \ }doc

: set-window ( a -- ) current-window ! ;

  \ doc{
  \
  \ set-window ( a -- )
  \
  \ Make `window` _a_ the `current-window`.
  \
  \ }doc

( wspace wemit wfreecolumns (wat-xy wat-xy at-wxy )

[unneeded] wspace
?\ need wemit  : wspace ( -- ) bl wemit ;

  \ doc{
  \
  \ wspace ( -- )
  \
  \ Display one space in the `current-window`.
  \
  \ See also: `space`.
  \
  \ }doc

[unneeded] wemit ?( need char>string need wtype
: wemit ( c -- ) char>string wtype ; ?)

  \ doc{
  \
  \ wemit ( c -- )
  \
  \ Display character _c_ in the `current-window`.
  \
  \ See also: `emit`.
  \
  \ }doc

[unneeded] wfreecolumns ?( need window
: wfreecolumns ( -- n ) wcolumns c@ wx c@ - ; ?)

  \ doc{
  \
  \ wfreecolumns ( -- n )
  \
  \ Return the number _n_ of free columns in the current
  \ line of the `current-window`.
  \
  \ }doc


[unneeded] (wat-xy ?( need window need under+
: (wat-xy ( col row -- ) wx0 c@ under+ wy0 c@ + at-xy ; ?)

  \ doc{
  \
  \ (wat-xy ( col row -- )
  \
  \ Set the cursor coordinates to `current-window` cursor
  \ coordinates _col row_.  The upper left corner of the
  \ `window` is column zero, row zero.
  \
  \ See also: `wat-xy`.
  \
  \ }doc

[unneeded] wat-xy ?( need window need (wat-xy
: wat-xy ( col row -- ) 2dup wy c! wx c!  (wat-xy ; ?)

  \ doc{
  \
  \ wat-xy ( col row -- )
  \
  \ Store _col row_ as the `current-window` cursor coordinates
  \ and set the cursor coordinates  accordingly.  The upper
  \ left corner of the `window` is column zero, row zero.
  \
  \ See also: `at-wxy`, `at-xy`.
  \
  \ }doc

[unneeded] at-wxy ?( need window need (wat-xy
: at-wxy ( -- ) wx c@ wy c@ (wat-xy ; ?)

  \ doc{
  \
  \ at-wxy ( -- )
  \
  \ Set the cursor coordinates to the `current-window` cursor
  \ coordinates.
  \
  \ See also: `wat-xy`.
  \
  \ }doc

( whome wcr ?wcr reset-window wcls )

[unneeded] whome
?\ need wat-xy  : whome ( -- ) 0 0 wat-xy ;

  \ doc{
  \
  \ whome ( -- )
  \
  \ Set the `current-window` cursor coordinates to its top left
  \ corner: column zero, row zero.
  \
  \ See also: `wat-xy`.
  \
  \ }doc

[unneeded] wcr ?( need window need whome

: wcr ( -- )
  wy c@ dup wrows c@ 1- =
  if  drop whome exit  then  1+ wy c! 0 wx c! ; ?)

  \ XXX TODO -- scroll instead of `whome`

  \ doc{
  \
  \ wcr ( -- )
  \
  \ Cause subsequent output to the `current-window` appear at
  \ the beginning of the next line.
  \
  \ WARNING: When the end of the `window` is reached, the
  \ cursor is set to the top left corner with `whome`. In a
  \ future version of the code, the window will be scrolled.
  \
  \ See also: `?wcr`.
  \
  \ }doc

[unneeded] ?wcr ?( need window need wcr
: ?wcr ( -- ) wx c@ 0= ?exit wcr ; ?)

  \ doc{
  \
  \ wcr ( -- )
  \
  \ If the column cursor coordinate of the `current-window` is not
  \ zero, cause subsequent output to the current window appear
  \ at the beginning of the next line.
  \
  \ WARNING: When the end of the `window` is reached, the
  \ cursor is set to the top left corner with `whome`. In a
  \ future version of the code, the window will be scrolled.
  \
  \ See also: `wcr`.
  \
  \ }doc

[unneeded] reset-window ?(

need columns need rows need set-window

: reset-window ( -- ) 0 0 columns rows set-window ; ?)

  \ doc{
  \
  \ reset-window ( -- )
  \
  \ Set the `current-window` to use the full screen.
  \
  \ }doc

[unneeded] wcls ?( need window need whome need ruler

: wcls ( -- ) bl wcolumns c@ ruler ( ca len )
  wy0 c@ wrows c@ bounds ?do   2dup wx0 c@ i at-xy type
                         loop  2drop  whome ; ?)

  \ doc{
  \
  \ wcls ( -- )
  \
  \ Clear the `current-window`.
  \
  \ See also: `cls`.
  \
  \ }doc

( wltype )

need window need at-wxy need wfreecolumns need wcr need ?wcr

: +wx ( n -- )
  wx c@ + dup wx c! wcolumns c@ = if  wcr  then ;

  \ doc{
  \
  \ +wx ( n -- )
  \
  \ Add _n_ character positions to the column cursor coordinate
  \ of the current window.
  \
  \ }doc

variable wtyped

  \ doc{
  \
  \ wtyped ( -- a )
  \
  \ A variable. _a_ is the address o a cell containing a flag
  \ indicating if a space-delimited substring was found and
  \ printed in the `current-window`. Otherwise, the string must
  \ be broken in order to fit the current line of the `window`.
  \ 
  \ ``wtyped`` is used by `wtype+` and `wltype`.
  \
  \ }doc

: wtype+ ( ca len -- ) tuck type +wx wtyped on ;

  \ doc{
  \
  \ wtype+ ( ca len -- )
  \
  \ Type string _ca len_ in the `current-window` and update the
  \ `window` coordinates accordingly.
  \
  \ }doc

: /wltype ( ca len len1 n -- ca' len' )
  >r >r over r> at-wxy wtype+ r> /string ;

  \ doc{
  \
  \ /wltype ( ca len len1 n -- ca' len' )
  \
  \ Type the first  _len1_ characters of string _ca len_ in the
  \ `current-window`, then remove the first _n_ characters from
  \ the string, returning the result string _ca' len'_.
  \
  \ ``/wltype`` is a factor of `wltype`.
  \
  \ }doc

: wltype ( ca len -- ) wtyped off
  begin  dup wfreecolumns >  while
    0 wfreecolumns do  over i + c@ bl =
                       if  i dup 1+ /wltype leave  then
                   -1 +loop
    wtyped @ if    ?wcr wtyped off
             else  wfreecolumns dup /wltype  then
  repeat  at-wxy wtype+ ;

  \ doc{
  \
  \ wltype ( ca len -- )
  \
  \ Type string _ca len_ in the `current-window`, left justified.
  \
  \ See also: `wemit`, `ltype`.
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
  \ 2017-05-11: Improve documentation.  Rename `+wc` to `+wx`.
  \ Rename `wtype` to `wltype`, to be consistent `with `ltype`.
  \ Rename `/wtype` to `/wltype`.

  \ vim: filetype=soloforth
