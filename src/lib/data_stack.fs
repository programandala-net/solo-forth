  \ data_stack.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202006081220
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words that manipulate the data stack.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018,
  \ 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( 2nip pick unpick roll )

unneeding 2nip ?( code 2nip ( x1 x2 x3 x4 -- x3 x4 )
  E1 c, D1 c, F1 c, F1 c, D5 c, E5 c, jpnext, end-code ?)
    \ pop hl
    \ pop de
    \ pop af
    \ pop af
    \ push de
    \ push hl
    \ _jp_next

  \ Credit:
  \
  \ Code from Afera; original code from DZX-Forth.

  \ doc{
  \
  \ 2nip ( x1 x2 x3 x4 -- x3 x4 ) "two-nip"
  \
  \ See: `nip`.
  \
  \ }doc

unneeding pick ?(
code pick ( x#u...x#1 x#0 u -- x#u...x#1 x#0 x#u )
  E1 c,  29 c,  39 c,  C3 c, fetchhl , end-code ?)
    \ pop hl
    \ add hl,hl
    \ add hl,sp
    \ jp fetch.hl

  \ doc{
  \
  \ pick ( x#u...x#1 x#0 u -- x#u...x#1 x#0 x#u )
  \
  \ Remove _u_ copy the _x#u_ to the top of the stack.  ``0
  \ pick`` is equivalent to `dup` and ``1 pick`` is equivalent
  \ to `over`.
  \
  \ Origin: Forth-83 (Required Word Set), Forth-94 (CORE EXT),
  \ Forth-2012 (CORE EXT).
  \
  \ See: `unpick`, `roll`, `rot`.
  \
  \ }doc

unneeding unpick ?\ : unpick ( x u -- ) 2+ cells sp@ + ! ;

  \ doc{
  \
  \ unpick ( x#u...x#1 x#0 x u -- x...x#1 x#0 )
  \
  \ Remove _x_ and _u_. Replace _x#u_ with _x_.  ``0 unpick``
  \ is equivalent to `nip` (but much slower).
  \
  \ See: `pick`.
  \
  \ Origin: LaForth's ``put``.
  \
  \ }doc

unneeding roll ?( need assembler need unresolved need >amark

code roll ( x#u x#u-1...x#0 u -- x#u-1...x#0 x#u )

  h pop, h addp, h d ldp, sp addp,
    \ pop hl
    \ add hl,hl
    \ ld e,l
    \ ld d,h
    \ add hl,sp

  b push, m c ld, h incp, m b ld,
    \ push bc
    \ ld c,(hl)
    \ inc hl
    \ ld b,(hl)

  b push, d b ldp, h d ldp, b tstp,
    \ push bc
    \ ld b,d
    \ ld c,e
    \ ld d,h
    \ ld e,l
    \ ld a,b
    \ or c

  0000 z? ?jp, >amark 0 unresolved ! h decp, h decp, lddr,
    \ jp z,roll.end
    \ dec hl
    \ dec hl
    \ lddr
    \ roll.end:
  0 unresolved @ >resolve h pop, b pop, exsp,
    \ pop hl
    \ pop bc
    \ ex (sp),hl
  jpnext, end-code ?)

  \ Credit:
  \
  \ Code adapted from DZX-Forth.

  \ doc{
  \
  \ roll ( x#u x#u-1...x#0 u -- x#u-1...x#0 x#u )
  \
  \ See: `pick`, `rot`.
  \
  \ }doc

( 3drop 4drop 3dup 4dup )

unneeding 3drop ?(

code 3drop ( x1 x2 x3 -- )
 E1 c, E1 c, E1 c, jpnext, end-code ?)
    \ pop hl
    \ pop hl
    \ pop hl
    \ _jp_next

  \ doc{
  \
  \ 3drop ( x1 x2 x3 -- ) "three-drop"
  \
  \ See: `3dup`, `drop`, `2drop`, `4drop`.
  \
  \ }doc

unneeding 4drop ?(

code 4drop ( x1 x2 x3 x4 -- )
  E1 c,  E1 c,  E1 c,  E1 c,  jpnext, end-code ?)
    \ pop hl
    \ pop hl
    \ pop hl
    \ pop hl
    \ _jp_next

  \ doc{
  \
  \ 4drop ( x1 x2 x3 x4 -- ) "four-drop"
  \
  \ See: `4dup`, `drop`, `2drop`, `3drop`.
  \
  \ }doc

unneeding 3dup ?(

code 3dup ( x1 x2 x3 -- x1 x2 x3 x1 x2 x3 )
  D9 c, C1 c, D1 c, E1 c, E5 c, D5 c, C5 c, E5 c, D5 c, C5 c,
    \ exx
    \ pop bc
    \ pop de
    \ pop hl
    \ push hl
    \ push de
    \ push bc
    \ push hl
    \ push de
    \ push bc
  D9 c, jpnext, end-code ?)
    \ exx
    \ _jp_next

  \ doc{
  \
  \ 3dup ( x1 x2 x3 -- x1 x2 x3 x1 x2 x3 ) "three-dup"
  \
  \ ``3dup`` is written is Z80. An equivalent definition in
  \ Forth is the following:

  \ ----
  \ : 3dup ( x1 x2 x3 -- x1 x2 x3 x1 x2 x3 ) dup 2over rot ;
  \ ----

  \ See: `3drop`, `dup`, `2dup`, `4dup`.
  \
  \ }doc

unneeding 4dup ?(

code 4dup ( x1 x2 x3 x4 -- x1 x2 x3 x4 x1 x2 x3 x4 )
  D9 c, F1 c, C1 c, D1 c, E1 c,
    \ exx
    \ pop af
    \ pop bc
    \ pop de
    \ pop hl
  E5 c, D5 c, C5 c, F5 c, E5 c, D5 c, C5 c, F5 c,
    \ push hl
    \ push de
    \ push bc
    \ push af
    \ push hl
    \ push de
    \ push bc
    \ push af
  D9 c, jpnext, end-code ?)
    \ exx
    \ _jp_next

  \ doc{
  \
  \ 4dup ( x1 x2 x3 x4 -- x1 x2 x3 x4 x1 x2 x3 x4 ) "four-dup"
  \
  \ See: `4drop`, `dup`, `2dup`, `3dup`.
  \
  \ }doc

( 2rot swapped )

unneeding 2rot ?( need roll

: 2rot ( x1 x2 x3 x4 x5 x6 -- x3 x4 x5 x6 x1 x2 )
  5 roll 5 roll ; ?)

  \ doc{
  \
  \ 2rot ( x1 x2 x3 x4 x5 x6 -- x3 x4 x5 x6 x1 x2 ) "two-rot"
  \
  \ }doc

unneeding swapped ?(

: swapped ( i*x n1 n2 -- j*x )
  >r 1+ cells sp@ +     ( i*x a1 ) ( R: n2 )
  r> 2+ cells sp@ +     ( i*x a1 a2 )
  over @ over @         ( i*x a1 a2 x1 x2 )
  >r swap !  r> swap ! ; ?)

  \ Credit:
  \
  \ Adapted from code written by Sam Suan Chen, published
  \ on Forth Dimensions (volume 6, number 6, page 9, 1985-03).

  \ Original code by Sam Suan Chen,
  \ with an equivalent usage example:

  \ : xyswap ( i*x n -- j*x )
  \   16 /mod >r dup + sp@ + sp@ r> dup + +
  \   over @ over @
  \   >r swap !  r> swap ! ;
  \
  \   ( 1 2 3 4 5 ) $25 xyswap ( 4 2 3 1 5 )

  \ doc{
  \
  \ swapped ( i*x n1 n2 -- j*x )
  \
  \ Remove _n1_ and _n2_. Swap elements _n1_ and _n2_ of the
  \ stack, being 0 the top of the stack. `0 1 swapped` is
  \ equivalent to `swap`.
  \
  \ Usage example:

  \ ----
  \   ( 1 2 3 4 5 ) 1 4 swapped ( 4 2 3 1 5 )
  \ ----

  \ }doc

( nup drup dip 0dup -dup )

unneeding nup ?( code nup ( x1 x2 -- x1 x1 x2 )
  E1 c,  D1 c,  D5 c,  D5 c, E5 c, jpnext, end-code ?)
    \ pop hl
    \ pop de
    \ push de
    \ push de
    \ push hl
    \ _jp_next

  \ Also called `under` in other Forth systems.

  \ doc{
  \
  \ nup ( x1 x2 -- x1 x1 x2 )
  \
  \ This word is defined in Z80. Its equivalent definition in
  \ Forth is the following:
  \
  \ ----
  \ : nup ( x1 x2 -- x1 x1 x2 ) over swap ;
  \ ----
  \
  \ See: `dup`, `tuck`, `drup`, `dip`.
  \
  \ }doc

unneeding drup ?( code drup ( x1 x2 -- x1 x1 )
  D1 c,  E1 c,  E5 c,  E5 c,  jpnext, end-code ?)
    \ pop de
    \ pop hl
    \ push hl
    \ push hl
    \ _jp_next

  \ doc{
  \
  \ drup ( x1 x2 -- x1 x1 )
  \
  \ This word is defined in Z80. Its equivalent definition in
  \ Forth is the following:
  \
  \ ----
  \ : drup ( x1 x2 -- x1 x1 ) drop dup ;
  \ ----
  \
  \ See: `dup`, `tuck`, `nup`, `dip`.
  \
  \ }doc

unneeding dip ?( code dip ( x1 x2 -- x2 x2 )
  E1 c, D1 c, E5 c, E5 c,  jpnext, end-code ?)
    \ pop hl
    \ pop de
    \ push hl
    \ push hl
    \ _jp_next

  \ doc{
  \
  \ dip ( x1 x2 -- x2 x2 )
  \
  \ This word is defined in Z80. Its equivalent definition in
  \ Forth is the following:
  \
  \ ----
  \ : dip ( x1 x2 -- x2 x2 ) nip dup ;
  \ ----
  \
  \ See: `nip`, `dup`, `tuck`, `drup`.
  \
  \ }doc

unneeding 0dup ?( code 0dup ( x -- x | 0 0 )
  E1 c,  78 04 + c,  B0 05 + c,
    \ pop hl
    \ ld a,h
    \ or l
  C2 c, pushhl , E5 c, E5 c, jpnext, end-code ?)
    \ jp z,push_hl
    \ push hl
    \ push hl
    \ _jp_next

  \ doc{
  \
  \ 0dup ( x -- x | 0 0 ) "zero-dup"
  \
  \ Duplicate _x_ if it's zero.
  \
  \ See: `dup`, `-dup`.
  \
  \ }doc

unneeding -dup ?( code -dup ( x -- x | x x )
  E1 c,  CB c, 7C c,  C2 c, pushhl , E5 c, E5 c, jpnext,
  end-code ?)
    \ pop hl
    \ bit 7,h ; negative?
    \ jp z,push_hl
    \ push hl
    \ push hl
    \ _jp_next

  \ doc{
  \
  \ -dup ( x -- x x | x ) "minus-dup"
  \
  \ Duplicate _x_ if it's negative.
  \
  \ See: `dup`, `0dup`.
  \
  \ }doc

( ndrop 2ndrop >true >false 2>true 2>false )

unneeding ndrop ?(

code ndrop ( x1...xn n -- )
  E1 c, 29 c, EB c, 21 c, 0000 , 39 c, 19 c, F9 c,
    \ pop hl
    \ add hl,hl
    \ ex de,hl ; DE = n cells
    \ ld hl,0
    \ add hl,sp ; HL = stack pointer
    \ add hl,de
    \ ld sp,hl ; update SP
  jpnext, end-code ?)

  \ doc{
  \
  \ ndrop ( x1...xn n -- ) "n-drop"
  \
  \ Drop _n_ cell items from the stack.
  \
  \ See: `2ndrop`, `drop`, `2drop`.
  \
  \ }doc

unneeding 2ndrop ?(

code 2ndrop ( dx1...dxn n -- )

  E1 c, 29 c, 29 c, EB c, 21 c, 0000 , 29 c, 19 c, F9 c,
    \ pop hl
    \ add hl,hl
    \ add hl,hl
    \ ex de,hl ; DE = n cells
    \ ld hl,0
    \ add hl,sp ; HL = stack pointer
    \ add hl,de
    \ ld sp,hl ; update SP
  jpnext, end-code ?)

  \ doc{
  \
  \ 2ndrop ( dx1...dxn n -- ) "two-n-drop"
  \
  \ Drop _n_ double cell items from the stack.
  \
  \ See: `ndrop`, `drop`, `2drop`.
  \
  \ }doc

unneeding >true unneeding 2>true and ?(

code 2>true ( x1 x2 -- true ) E1 c, end-code
  \ pop hl
  \ ; execution continues in `>true`

  \ doc{
  \
  \ 2>true ( x1 x2 -- true ) "two-to-true"
  \
  \ Replace _x1 x2_ with `true`.
  \
  \ See: `2>false`, `>true`.
  \
  \ }doc

code >true ( x -- true ) E1 c, ' true jp, end-code ?)
  \ pop hl
  \ jp true_

  \ doc{
  \
  \ >true ( x -- true ) "to-true"
  \
  \ Replace _x_ with `true`.
  \
  \ See: `>false`, `2>true`.
  \
  \ }doc

unneeding >false unneeding 2>false and ?(

code 2>false ( x1 x2 -- false ) E1 c, end-code
  \ pop hl
  \ ; execution continues in `>false`

  \ doc{
  \
  \ 2>false ( x1 x2 -- false ) "two-to-false"
  \
  \ Replace _x1 x2_ with `false`.
  \
  \ See: `2>true`, `>false`.
  \
  \ }doc

code >false ( x -- false ) E1 c, ' false jp, end-code ?)
  \ pop hl
  \ jp false_

  \ doc{
  \
  \ >false ( x -- false ) "to-false"
  \
  \ Replace _x_ with `false`.
  \
  \ See: `>true`, `2>false`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-06-22: Add `2nip`.
  \
  \ 2015-10-16: Move `roll` from the kernel.
  \
  \ 2015-11-09: Add `swapped`.
  \
  \ 2015-11-22: Add `3drop`, `4drop`.
  \
  \ 2015-12-16: Add `nup`, `drup`, `dip`.
  \
  \ 2015-12-22: Move `3dup` from the assembler and rewrite in
  \ Z80.
  \
  \ 2016-02-26: Add `ndrop`, `2ndrop`.
  \
  \ 2016-04-24: Move `pick` from the kernel.
  \
  \ 2016-05-02: Join several blocks to save space.
  \
  \ 2016-11-17: Use `?(` instead of `[if]`.
  \
  \ 2016-12-20: Rename `jppushhl` to `jppushhl,` and `jpnext`
  \ to `jpnext,`, after the change in the kernel.
  \
  \ 2017-01-01: Convert `roll` from `z80-asm` to `z80-asm,` and
  \ fix its needing.
  \
  \ 2017-01-02: Convert `ndrop` and 2ndrop` from `z80-asm` to
  \ Z80 opcodes.
  \
  \ 2017-01-05: Update `need z80-asm,` to `need assembler`.
  \
  \ 2017-01-07: Add `>true`, `2>true`, `>false`, `2>false`.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-31: Improve documentation of `swapped`.
  \
  \ 2017-02-16: Fix typo in documentation of `2ndrop`.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-02-21: Need `unresolved`, which now is optional, not
  \ part of the assembler.
  \
  \ 2017-02-22: Compact the code, saving one block. Fix needing
  \ of `ndrop`.
  \
  \ 2017-03-11: Need `>mark`, which now is optional, not
  \ included in the assembler by default.
  \
  \ 2017-03-21: Improve documentation.
  \
  \ 2017-05-09: Remove `jp pushhlde`. Remove `jppushhl,`.
  \
  \ 2017-07-27: Replace `jp next` with the actual macro
  \ `_jp_next` in Z80 comments.
  \
  \ 2017-12-11: Improve stack comments of `roll` and `pick`.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-06: Add `unpick`. Improve documentation. Add
  \ `4dup`.
  \
  \ 2018-03-07: Add words' pronunciaton.
  \
  \ 2018-04-15: Update notation ".." to "...".
  \
  \ 2020-06-08: Improve documentation: make _true_ and
  \ _false_ cross-references.

  \ vim: filetype=soloforth
