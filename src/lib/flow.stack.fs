  \ flow.stack.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201712022236
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words to manipulate the control flow stack.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( cs-pick cs-roll cs-drop )

[unneeded] cs-pick
?\ need alias need pick ' pick alias cs-pick compile-only

  \ doc{
  \
  \ cs-pick
  \   \ ( S: u -- )
  \   \ ( C: orig#u|dest#u .. orig#0|dest#0 --
  \   \      orig#u|dest#u .. orig#0|dest#0 orig#u|dest#u )

  \
  \ Remove _u_. Copy _orig[u]|dest[u]_ to the top of the
  \ control-flow stack.
  \
  \ ``cs-pick`` is a `compile-only` word.
  \
  \ NOTE: In Solo Forth the control-flow stack is implemented
  \ using the data stack. Therefore ``cs-pick`` is an `alias`
  \ of `pick`.
  \
  \ Origin: Forth-94 (TOOLS EXT), Forth-2012 (TOOLS EXT).
  \
  \ See also: `cs-roll`, `cs-swap`, `cs-drop`.
  \
  \ }doc

[unneeded] cs-roll
?\ need alias need roll ' roll alias cs-roll compile-only

  \ doc{
  \
  \ cs-roll
  \   \ ( S: u -- )
  \   \ ( C: orig#u|dest#u orig#u-1|dest#u-1 .. orig#0|dest#0 --
  \   \      orig#u-1|dest#u-1 .. orig#0|dest#0 orig#u|dest#u )

  \
  \ Remove _u_.  Rotate _u+1_ items on top of the control-flow
  \ stack so that _orig[u]|dest[u]_ is on top of the
  \ control-flow stack.
  \
  \ ``cs-roll`` is a `compile-only` word.
  \
  \ NOTE: In Solo Forth the control-flow stack is implemented
  \ using the data stack. Therefore ``cs-roll`` is an `alias`
  \ of `roll`.
  \
  \ Origin: Forth-94 (TOOLS EXT), Forth-2012 (TOOLS EXT).
  \
  \ See also: `cs-pick`, `cs-swap`, `cs-drop`.
  \
  \ }doc

[unneeded] cs-drop
?\ need alias ' drop alias cs-drop compile-only

  \ doc{
  \
  \ cs-drop ( C: orig|dest -- )

  \
  \ Remove _orig|dest_ from the control-flow stack.
  \
  \ ``cs-drop`` is a `compile-only` word.
  \
  \ NOTE: In Solo Forth the control-flow stack is implemented
  \ using the data stack. Therefore ``cs-drop`` is an `alias`
  \ of `drop`.
  \
  \ See also: `cs-pick`, `cs-roll`, `cs-swap`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2017-01-19: Make `cs-pick`, `cs-roll`, `cs-swap` and
  \ `cs-drop` individually accessible to `need`.  Remove
  \ alternative unfinished implementations, ported from
  \ DX-Forth and hForth.
  \
  \ 2017-05-05: Make `cs-pick`, `cs-roll`, `cs-swap` and
  \ `cs-drop` compile-only. Document them.
  \
  \ 2017-12-02: Move `cs-swap` to the kernel. Improve stack
  \ notation.

  \ vim: filetype=soloforth
