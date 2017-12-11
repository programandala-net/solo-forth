  \ flow.stack.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201712111302
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

[unneeded] cs-pick ?( need pick

: cs-pick ( u -- ) ( C: x#u .. x#1 x#0 -- x#u .. x#1 x#0 x#u )
  pick ; compile-only ?)

  \ doc{
  \
  \ cs-pick
  \   \ ( S: u -- )
  \   \ ( C: x#u .. x#1 x#0 -- x#u .. x#1 x#0 x#u )

  \
  \ Remove _u_. Copy _x#u_ to the top of the control-flow
  \ stack.
  \
  \ ``cs-pick`` is a `compile-only` word.
  \
  \ NOTE: In Solo Forth the control-flow stack is implemented
  \ using the data stack.
  \
  \ Origin: Forth-94 (TOOLS EXT), Forth-2012 (TOOLS EXT).
  \
  \ See: `cs-roll`, `cs-swap`, `cs-drop`.
  \
  \ }doc

[unneeded] cs-roll ?( need roll

: cs-roll ( u -- ) ( C: x#u x#n .. x#0 -- x#n .. x#0 x#u )
  roll ; compile-only ?)

  \ doc{
  \
  \ cs-roll
  \   \ ( S: u -- )
  \   \ ( C: x#u x#u-1 .. x#0 -- x#u-1 .. x#0 x#u )

  \
  \ Remove _u_.  Rotate _u+1_ items on top of the control-flow
  \ stack so that _x#u_ is on top of the control-flow stack.
  \
  \ ``cs-roll`` is a `compile-only` word.
  \
  \ NOTE: In Solo Forth the control-flow stack is implemented
  \ using the data stack. Therefore ``cs-roll`` is an `alias`
  \ of `roll`.
  \
  \ Origin: Forth-94 (TOOLS EXT), Forth-2012 (TOOLS EXT).
  \
  \ See: `cs-pick`, `cs-swap`, `cs-drop`.
  \
  \ }doc

[unneeded] cs-drop ?\ : cs-drop ( C: x -- ) drop ; compile-only

  \ doc{
  \
  \ cs-drop ( C: x -- )

  \
  \ Remove _x_ from the control-flow stack.
  \
  \ ``cs-drop`` is a `compile-only` word.
  \
  \ NOTE: In Solo Forth the control-flow stack is implemented
  \ using the data stack.
  \
  \ See: `cs-pick`, `cs-roll`, `cs-swap`.
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
  \
  \ 2017-12-11: Unalias `cs-pick`, `cs-roll` and `cs-drop`.
  \ Improve documentation.

  \ vim: filetype=soloforth
