  \ display.window.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702221550
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
  \ A constant that holds the size in bytes _n_ occupied by the
  \ data of one window.
  \
  \ }doc

variable current-window

  \ doc{
  \
  \ current-window ( -- a )
  \
  \ A variable. _a_ holds the data address of the current
  \ window.
  \
  \ See also: `window`, `set-window`.
  \
  \ }doc

: wx       ( -- ca ) current-window @ ~wx ;
: wy       ( -- ca ) current-window @ ~wy ;
: wx0      ( -- ca ) current-window @ ~wx0 ;
: wy0      ( -- ca ) current-window @ ~wy0 ;
: wcolumns ( -- ca ) current-window @ ~wcolumns ;
: wrows    ( -- ca ) current-window @ ~wrows ;  -->

( window set-window )

: window ( x0 y0 columns rows "name" -- )
  create  0 c, 0 c, 2swap swap c, c, swap c, c, ;

  \ doc{
  \
  \ window ( x0 y0 columns rows "name" -- )
  \
  \ Create a window called _name_: _x0 y0_ is the position of
  \ its top left corner on the screen, and _columns rows_ is
  \ its size.  Its cursor position is set to home.
  \
  \ Later execution of _name_ will leave the address of its
  \ data on the stack, which is the following structure:
  \
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
  \
  \ See also: `set-window`, `current-window`.
  \
  \ }doc

: set-window ( a -- ) current-window ! ;

  \ doc{
  \
  \ set-window ( -- )
  \
  \ Make _a_ the current window.
  \
  \ See also: `window`, `current-window`.
  \
  \ }doc

( wspace wemit wfreecolumns (wat-xy wat-xy at-wxy )

[unneeded] wspace
?\ need wemit  : wspace ( -- ) bl wemit ;

[unneeded] wemit ?( need char>string need wtype
: wemit ( c -- ) char>string wtype ; ?)

[unneeded] wfreecolumns ?( need window
: wfreecolumns ( -- n ) wcolumns c@ wx c@ - ; ?)
  \ Return the number _n_ of free columns in the current
  \ line of the current window.

[unneeded] (wat-xy ?( need window need under+
: (wat-xy ( x y -- ) wx0 c@ under+ wy0 c@ + at-xy ; ?)
  \ Set the cursor at current window coordinates _x y_.

[unneeded] wat-xy ?( need window need (wat-xy
: wat-xy ( x y -- ) 2dup wy c! wx c!  (wat-xy ; ?)
  \ Set the current window coordinates to _x y_ and set the
  \ cursor there.

[unneeded] at-wxy ?( need window need (wat-xy
: at-wxy ( -- ) wx c@ wy c@ (wat-xy ; ?)
  \ Set the cursor at the current window coordinates.

( whome wcr ?wcr reset-window wcls )

[unneeded] whome
?\ need wat-xy  : whome ( -- ) 0 0 wat-xy ;
  \ Set the current window coordinates to the first column and
  \ the first row.

[unneeded] wcr ?( need window need whome

: wcr ( -- )
  wy c@ dup wrows c@ 1- =
  if  drop whome exit  then  1+ wy c! 0 wx c! ; ?)
  \ Cause subsequent output to the current window appear at the
  \ beginning of the next line.
  \
  \ XXX TODO -- scroll instead of `whome`

[unneeded] ?wcr ?( need window need wcr
: ?wcr ( -- ) wx c@ 0= ?exit wcr ; ?)
  \ If the x cursor coordinate of the current window is not
  \ zero, cause subsequent output to the current window appear
  \ at the beginning of the next line.

[unneeded] reset-window ?(

need columns need rows need set-window

: reset-window ( -- ) 0 0 columns rows set-window ; ?)
  \ Set the current window to use the full screen.

[unneeded] wcls ?( need window need whome need ruler

: wcls ( -- ) bl wcolumns c@ ruler ( ca len )
  wy0 c@ wrows c@ bounds ?do   2dup wx0 c@ i at-xy type
                         loop  2drop  whome ; ?)
  \ Clear the current window.

( wtype )

need window need at-wxy need wfreecolumns need wcr need ?wcr

: +wc ( n -- )
  wx c@ + dup wx c! wcolumns c@ = if  wcr  then ;
  \ Add _n_ character positions to the x cursor coordinate of
  \ the current window.

variable wtyped
  \ A flag that indicates if a space-delimited substring was
  \ found and printed in the current window. Otherwise, the
  \ string must be broken in order to fit the current line of
  \ the window.

: wtype+ ( ca len -- ) tuck type +wc  wtyped on ;
  \ Type string _ca len_ in the current window and update
  \ the window coordinates accordingly.

: /wtype ( ca len len1 n -- ca' len' )
  >r >r over r> at-wxy wtype+ r> /string ;
  \ Type the first  _len1_ characters of string _ca len_ in the
  \ current window, then remove the first _n_ characters from
  \ the string, returning the result string _ca' len'_.

: wtype ( ca len -- ) wtyped off
  begin  dup wfreecolumns >  while
    0 wfreecolumns do  over i + c@ bl =
                       if  i dup 1+ /wtype leave  then
                   -1 +loop
    wtyped @ if    ?wcr wtyped off
             else  wfreecolumns dup /wtype  then
  repeat  at-wxy wtype+ ;
  \ Type string _ca len_ in the current window, left justified.

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

  \ vim: filetype=soloforth
