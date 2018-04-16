  \ flow.stack.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201804162004
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words to manipulate the control flow stack.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( cs-pick cs-roll cs-drop cs-dup cs-mark cs-test )

unneeding cs-pick ?( need pick

: cs-pick pick ; compile-only ?)
  \ ( u -- ) ( C: x#u ... x#1 x#0 -- x#u ... x#1 x#0 x#u )

  \ doc{
  \
  \ cs-pick "c-s-pick"
  \   ( S: u -- )
  \   ( C: x#u ... x#1 x#0 -- x#u ... x#1 x#0 x#u )

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
  \ See: `cs-roll`, `cs-swap`, `cs-drop`, `cs-dup`, `cs-mark`,
  \ `cs-test`.
  \
  \ }doc

unneeding cs-roll ?( need roll

: cs-roll ( u -- ) ( C: x#u x#n ... x#0 -- x#n ... x#0 x#u )
  roll ; compile-only ?)

  \ doc{
  \
  \ cs-roll "c-s-roll"
  \   ( S: u -- )
  \   ( C: x#u x#u-1 ... x#0 -- x#u-1 ... x#0 x#u )

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
  \ See: `cs-pick`, `cs-swap`, `cs-drop`, `cs-dup`, `cs-mark`,
  \ `cs-test`.
  \
  \ }doc

unneeding cs-drop ?\ : cs-drop ( C: x -- ) drop ; compile-only

  \ doc{
  \
  \ cs-drop ( C: x -- ) "c-s-drop"

  \
  \ Remove _x_ from the control-flow stack.
  \
  \ ``cs-drop`` is a `compile-only` word.
  \
  \ NOTE: In Solo Forth the control-flow stack is implemented
  \ using the data stack.
  \
  \ See: `cs-pick`, `cs-roll`, `cs-swap`, `cs-dup`, `cs-mark`,
  \ `cs-test`.
  \
  \ }doc

unneeding cs-dup

?\ : cs-dup ( C: x -- x x ) dup ; compile-only

  \ doc{
  \
  \ cs-dup ( C: x -- x x ) "c-s-dup"

  \
  \ Duplicate _x_ on the control-flow stack.
  \
  \ ``cs-dup`` is a `compile-only` word.
  \
  \ NOTE: In Solo Forth the control-flow stack is implemented
  \ using the data stack.
  \
  \ See: `cs-pick`, `cs-roll`, `cs-swap`, `cs-drop`, `cs-mark`,
  \ `cs-test`.
  \
  \ }doc

unneeding cs-mark ?\ 0 cconstant cs-mark ( C: -- cs-mark )

  \ Credit:
  \ Control-Flow Stack Extensions
  \ http://dxforth.netbay.com.au/cfsext.html

  \ doc{
  \
  \ cs-mark ( C: -- cs-mark ) "c-s-mark"
  \
  \ Place a marker _cs-mark_ on the control-flow stack.  The
  \ marker ocuppies the same width as an _orig|dest_ but is
  \ distinguishable using `cs-test`.
  \
  \ See: `cs-pick`, `cs-roll`, `cs-swap`, `cs-dup`, `cs-drop`.
  \
  \ }doc

unneeding cs-test ?( need cs-mark

: cs-test ( -- f ) ( C: x -- x ) dup cs-mark <> ; ?)

  \ Credit:
  \ Control-Flow Stack Extensions
  \ http://dxforth.netbay.com.au/cfsext.html

  \ doc{
  \
  \ cs-test "c-s-test"
  \   Compilation: ( -- f ) ( C: x -- x )

  \ Return a true flag if _x_ is an _orig|dest_, and false if a
  \ marker placed by `cs-mark`.
  \
  \ See: `cs-pick`, `cs-roll`, `cs-swap`, `cs-dup`, `cs-drop`.
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
  \ Improve documentation. Add `cs-mark`, `cs-test`, `cs-dup`.
  \
  \ 2018-02-15: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-04-15: Update notation ".." to "...".
  \
  \ 2018-04-16: Fix line too long.

  \ vim: filetype=soloforth
