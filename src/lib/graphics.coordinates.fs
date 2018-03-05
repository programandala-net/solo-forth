  \ graphics.coordinates.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803052149
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words to manipulate the graphic coordinates.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( g-xy g-x g-y g-at-xy g-at-x g-at-y g-home )


unneeding g-xy ?( need os-coordx need os-coordy
: g-xy ( -- gx gy ) os-coordx c@ os-coordy c@ ; ?)

  \ doc{
  \
  \ g-xy ( -- gx gy ) "g-x-y"
  \
  \ Return the current graphic coordinates _gx gy_.
  \
  \ See: `g-x`, `g-y`, `g-at-xy`.
  \
  \ }doc

unneeding g-x
?\ need os-coordx : g-x ( -- gx ) os-coordx c@ ;

  \ doc{
  \
  \ g-x ( -- gx )
  \
  \ Return the current graphic x coordinate _gx_.
  \
  \ See: `g-xy`, `g-y`, `g-at-xy`.
  \
  \ }doc

unneeding g-y
?\ need os-coordy : g-y ( -- gy ) os-coordy c@ ;

  \ doc{
  \
  \ g-y ( -- gy )
  \
  \ Return the current graphic y coordinate _gy_.
  \
  \ See: `g-xy`, `g-x`, `g-at-xy`.
  \
  \ }doc

unneeding g-at-xy dup ?\ need os-coordx need os-coordy
?\ : g-at-xy ( gx gy -- ) os-coordy c! os-coordx c! ;

  \ doc{
  \
  \ g-at-xy ( gx gy -- ) "g-at-x-y"
  \
  \ Set the current graphic coordinates _gx gy_.
  \
  \ See: `g-xy`, `g-at-y`, `g-at-x`, `g-home`.
  \
  \ }doc

unneeding g-at-x
?\ need os-coordx : g-at-x ( gx -- ) os-coordx c! ;

  \ doc{
  \
  \ g-at-x ( gx -- )
  \
  \ Set the current graphic x coordinate _gx_, without changing
  \ the current graphic y coordinate.
  \
  \ See: `g-at-xy`, `g-at-y`.
  \
  \ }doc

unneeding g-at-y
?\ need os-coordy : g-at-y ( gy -- ) os-coordy c! ;

  \ doc{
  \
  \ g-at-y ( gy -- )
  \
  \ Set the current graphic y coordinate _gy_, without changing
  \ the current graphic x coordinate.
  \
  \ See: `g-at-xy`, `g-at-x`.
  \
  \ }doc

unneeding g-home
?\ need os-coords  : g-home ( -- ) os-coords off ;

  \ doc{
  \
  \ g-home ( -- )
  \
  \ Set the graphic coordinates to 0, 0.
  \
  \ See: `g-at-xy`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-04-23: First version: `g-x`, `g-y`, `g-xy`, `g-at-x`,
  \ `g-at-y`, `g-at-xy`, `g-home`.
  \
  \ 2017-02-01: Rewrite with a different approach. Document.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-04-20: Improve needing of `g-xy`.
  \
  \ 2018-02-15: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.

  \ vim: filetype=soloforth

