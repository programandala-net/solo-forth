  \ flow.dijkstra.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201712111500

  \ ===========================================================
  \ Description

  \ An implementation of the
  \ Dijkstra Guarded Command Control Structures.

  \ ===========================================================
  \ Authors

  \ M. Edward Borasky, 1996-08-03, "Towards a Discipline of ANS
  \ Forth Programming", published on Forth Dimensions (volume
  \ 18, number 4, pp 5-14), 1996-12.
  \
  \ Modified and adapted to Solo Forth by Marcos Cruz
  \ (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( {if if} if> |if| )

need cs-swap

: {if \ Compilation: ( -- 0 )
  0 ; immediate compile-only

  \ doc{
  \
  \ {if
  \   Compilation: ( -- 0 )
  \
  \ Start a ``{if`` control structure.
  \
  \ See: `if}`, `if>`, `|if|`.
  \
  \ }doc

: if> \ Compilation: ( count -- count+1 ) ( C: -- orig1 )
  1+ >r postpone if r> ; immediate compile-only

  \ doc{
  \
  \ if>
  \   Compilation: ( count -- count+1 )
  \                ( C: -- orig1 )
  \
  \ Part of the `{if` control structure.
  \
  \ }doc

: |if|
  \ ( count -- count )
  \ ( C: orig ... orig1 -- orig ... orig2 )
  >r postpone ahead       \ new _orig_
  cs-swap postpone then \ resolve old _orig_
  r> ; immediate compile-only

  \ doc{
  \
  \ |if|
  \   Compilation: ( count -- count )
  \                ( C: orig .. orig1 -- orig .. orig2 )
  \
  \ Part of the `{if` control structure.
  \
  \ }doc

: if}
  \ Compilation: ( count -- ) ( C: orig#1 .. orig#n -- )
  >r postpone ahead
  cs-swap postpone then \ resolve old _orig_
  -22 postpone literal postpone throw
    \ -22 = "control structure mismatch"
  r> 0 ?do postpone then loop ; immediate compile-only

  \ XXX TODO -- use `thens`

  \ doc{
  \
  \ if}
  \   Compilation: ( count -- )
  \                ( C: orig#1 .. orig#n -- )
  \
  \ Terminate  a `{if` control structure.
  \
  \ }doc

( {do do} do> |do| )

need cs-swap need cs-dup

: {do
  \ Compilation: ( C: -- dest )
  \ Run-time:    ( -- )
  postpone begin ; immediate compile-only

  \ doc{
  \
  \ {do
  \   Compilation: ( C: -- dest )
  \   Run-time:    ( -- )
  \
  \ Start a ``{do`` control structure.
  \
  \ See: `do}`, `|do|`, `do>`.
  \
  \ }doc

: do>
  \ Compilation: ( C: dest -- orig dest )
  postpone if cs-swap ; immediate compile-only

  \ doc{
  \
  \ do>
  \   Compilation: ( C: dest -- orig dest )
  \
  \ Part of the `{do` control structure.
  \
  \ }doc

: |do|
  \ Compilation: ( C: orig dest -- dest )
  cs-dup  postpone again \ resolve a copy of _dest_
  cs-swap postpone then  \ resolve old _orig_
  ; immediate compile-only

  \ doc{
  \
  \ |do|
  \   Compilation: ( C: orig dest -- dest )
  \
  \ Part of the `{do` control structure.
  \
  \ }doc

: do}
  \ Compilation: ( C: orig dest -- )
  \ Run-time:    ( -- )
  postpone again \ resolve _dest_
  postpone then  \ resolve _orig_
  ; immediate compile-only

  \ doc{
  \
  \ do}
  \   Compilation: ( C: orig dest -- )
  \   Run-time:    ( -- )
  \
  \ Terminate a `{do` control structure.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2017-12-11: Improve file header. Prepare documentation.
  \ Replace `1 cs-roll` and `0 cs-pick` with `cs-swap` and
  \ `cs-dup`.

  \ vim: filetype=soloforth
