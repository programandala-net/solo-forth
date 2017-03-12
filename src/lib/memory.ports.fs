  \ memory.ports.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702281734
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words for ports input and output.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( @p !p )

[unneeded] @p ?(

code @p ( a -- b )
  D9 c, C1 c, ED c, 68 c, 26 c, 00 c, E5 c, D9 c,
    \           ; T  B
    \           ; -- --
    \ exx       ; 04 01
    \ pop bc    ; 10 01
    \ in l,(c)  ; 12 02
    \ ld h,0x00 ; 07 02
    \ push hl   ; 11 01
    \ exx       ; 04 01
  jpnext, end-code ?)
    \ _jp_next  ; 08 02 ; jp (ix)
    \           ; -- --
    \           ; 56 10 Total

  \ XXX OLD -- First version:

    \           ; T  B
    \           ; -- --
    \ pop hl    ; 10 01
    \ push bc   ; 11 01
    \ ld c,l    ; 04 01
    \ ld b,h    ; 04 01
    \ in l,(c)  ; 12 02
    \ pop bc    ; 10 01
    \ ld h,0x00 ; 07 02
    \ jp pushhl ; 10 03
    \           ; 11 01 push hl
    \           ; -- --
    \           ; 79 13 Total

  \ doc{
  \
  \ @p ( a -- b )
  \
  \ Input byte _b_ from port _a_.
  \
  \ See also: `!p`, `@`, `c@`.
  \
  \ }doc

[unneeded] !p ?(

code !p ( b a -- ) D9 c, C1 c, E1 c, ED c, 69 c, D9 c,
    \           ; T  B
    \           ; -- --
    \ exx       ; 04 01
    \ pop bc    ; 10 01
    \ pop hl    ; 10 01
    \ out (c),l ; 12 02
    \ exx       ; 04 01
  jpnext, end-code ?)
    \ _jp_next  ; 08 02 ; jp (ix)
    \           ; -- --
    \           ; 48 08 Total

  \ XXX OLD -- First version:

    \           ; T  B
    \           ; -- --
    \ pop hl    ; 10 01
    \ pop de    ; 10 01
    \ push bc   ; 11 01
    \ ld c,l    ; 04 01
    \ ld b,h    ; 04 01
    \ out (c),e ; 12 02
    \ pop bc    ; 10 01
    \ jp next   ; 10 03
    \           ; -- --
    \           ; 72 11

  \ doc{
  \
  \ !p ( b a -- )
  \
  \ Output byte _b_ to port _a_.
  \
  \ See also: `@p`, `!`, `c!`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-05-07: Improve documentation. Compact the blocks.
  \
  \ 2016-12-20: Rename `jppushhl` to `jppushhl,` and `jpnext`
  \ to `jpnext,`, after the change in the kernel.
  \
  \ 2017-02-28: Improve `@p` and `!p`: faster and smaller.
  \ Improve documentation.

  \ vim: filetype=soloforth
