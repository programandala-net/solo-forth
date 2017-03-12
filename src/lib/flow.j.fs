  \ flow.j.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702240011
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ `j` and `k`.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( j k )

[unneeded] j ?(

code j ( -- n|u ) ( R: do-sys1 do-sys2 -- do-sys1 do-sys2 )
  2A c, rp ,  11 c, 3 cells ,  19 c,  C3 c, ' i 3 + , end-code
    \ ld hl,(return_stack_pointer)
    \ ld de,3*cell
    \ add hl,de
    \ jp i.hl ; secondary entry point in `i`

?)

  \ doc{
  \
  \ j ( -- n|u ) ( R: loop-sys1 loop-sys2 -- loop-sys1 loop-sys2 )
  \
  \ Return a copy _n|u_ of the next-outer `loop` index.
  \
  \ Origin: Forth-83 (Required Word Set), Forth-94 (CORE),
  \ Forth-2012 (CORE).
  \
  \ See also: `i`, `k`.
  \
  \ }doc

[unneeded] k ?(

code k ( -- n|u )
  ( R: loop-sys1..loop-sys3 -- loop-sys1..loop-sys3 )
  2A c, rp ,  11 c, 6 cells ,  19 c,  C3 c, ' i 3 + , end-code
    \ ld hl,(return_stack_pointer)
    \ ld de,6*cell
    \ add hl,de
    \ jp i.hl ; secondary entry point in `i`

?)

  \ doc{
  \
  \ k ( -- n|u ) ( R: loop-sys1..loop-sys3 -- loop-sys1..loop-sys3 )
  \
  \ Return a copy _n|u_ of the second outer `loop` index.
  \
  \ Origin: Forth-83 (Controlled reference words).
  \
  \ See also: `i`, `j`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-04-15: Written.
  \
  \ 2016-04-28: Calculate the secondary entry point of `i`,
  \ instead of using the constant `(i)`, which has been removed
  \ from the kernel.
  \
  \ 2016-05-04: Compact two blocks into one.
  \
  \ 2016-11-17: Use `?(` instead of `[if]`.
  \
  \ 2017-02-20: Update notation of word sets.
  \
  \ 2017-02-24: Improve documentation.

  \ vim: filetype=soloforth
