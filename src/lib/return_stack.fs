  \ return_stack.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702221550

  \ -----------------------------------------------------------
  \ Description

  \ Words that manipulate the return stack.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ History

  \ 2015-09-10: Add `n>r`, `nr>`.
  \
  \ 2015-10-27: Add `r'@`.
  \
  \ 2016-04-24: Move `2rdrop` from the kernel.
  \
  \ 2016-04-28: Add `dup>r`.
  \
  \ 2016-12-03: Use `cell` and `literal` in `rdepth`, for
  \ clarity. Use `jp,`. Compact the code, saving 2 blocks.
  \
  \ 2016-12-06: Remove mutual needing of `n>r` and `nr>`,
  \ because sometimes only one of them is required, and
  \ complete their documentation.
  \
  \ 2016-12-20: Rename `jppushhl` to `jppushhl,` and `jpnext`
  \ to `jpnext,`, after the change in the kernel.
  \
  \ 2017-01-02: Convert `n>r` and `nr>` from `z80-asm` to
  \ `z80-asm,`.
  \
  \ 2017-01-05: Update `need z80-asm,` to `need assembler`.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-19: Remove remaining `exit` at the end of
  \ conditional interpretation.
  \
  \ 2017-02-17: Update cross references.

( n>r )

need assembler

code n>r ( x1..xn n -- ) ( R: -- x1..xn n )

  exx,

  b pop, 0000 b stp, >amark
  rp fthl,
  rbegin  b tstp,  nz? rwhile
    d pop, h decp, d m ld, h decp, e m ld, b decp,
  rrepeat
  0000 d ldp#, >aresolve
  h decp, d m ld, h decp, e m ld,

  rp sthl, exx, jpnext,

  end-code

  \ doc{
  \
  \ n>r ( x#1..x#n n -- ) ( R: -- x#1..x#n n )
  \
  \ Remove _n+1_ items from the  data stack and store them  for
  \ later retrieval by `nr>`.  The return stack may  be used to
  \ store the data. Until this data has been retrieved by
  \ `nr>`:
  \
  \ - this data will not be overwritten by a subsequent
  \ invocation of `n>r` and
  \
  \ - a program may not access data placed on the return stack
  \ before the invocation of `n>r`.
  \
  \ Origin: Forth-2012 (TOOLS EXT).
  \
  \ }doc

( nr> )

need assembler

code nr> ( -- x1..xn n ) ( R: x1..xn n -- )

  exx,
  rp fthl,
  m c ld, h incp, m b ld, h incp,
  0000 b stp, >amark
  rbegin  b tstp, nz? rwhile
    m e ld, h incp, m d ld, h incp, d push, b decp,
  rrepeat
  rp sthl, exx,
  0000 h ldp#, >aresolve
  jppushhl,
  end-code

  \ doc{
  \
  \ nr> ( -- x#1..x#n n ) ( R: x#1..x#n n -- )
  \
  \ Retrieve the items previously stored by an invocation of
  \ `n>r`.  _n_ is the number of items placed on the  data
  \ stack.
  \
  \ Origin: Forth-2012 (TOOLS EXT).
  \
  \ }doc

( rdepth r'@ 2rdrop dup>r )

[unneeded] rdepth
?\ : rdepth ( -- n ) rp@ rp0 @ - [ cell negate ] literal / ;

  \ Credit:
  \
  \ `rdepth` from Afera.

  \ doc{
  \
  \ rdepth ( -- n )
  \
  \ }doc

  \ Credit:
  \
  \ `r'@` by Wil Baden.

[unneeded] r'@ ?(
: r'@ ( -- x1 ) ( R: x1 x2 -- x1 x2 )
  r> 2r@ drop swap >r ; ?)

  \ doc{
  \
  \ r'@ ( -- x1 ) ( R: x1 x2 -- x1 x2 )
  \
  \ }doc

[unneeded] 2rdrop ?(
code 2rdrop ( R: x1 x2 -- )
  2A c, rp , 11 c, 02 cells , 19 c, 22 c, rp ,
    \ ld hl,(return_stack_pointer)
    \ ld de,cell*2
    \ add hl,de
    \ ld (return_stack_pointer),hl
  jpnext, end-code ?)
    \ jp next


  \ doc{
  \
  \ 2rdrop ( R: x1 x2 -- )
  \
  \ }doc

[unneeded] dup>r ?(
code dup>r ( x -- x ) ( R: -- x )
  D1 c, D5 c, ' >r 1+ jp, end-code ?)
    \ pop de
    \ push de
    \ jp to_r_.de ; secondary entry of `>r`, in the kernel

  \ Credit:
  \
  \ Idea from IsForth.

  \ doc{
  \
  \ dup>r ( x -- x ) ( R: -- x )
  \
  \ A faster alternative to the idiom `dup >r`.
  \
  \ }doc

  \ vim: filetype=soloforth
