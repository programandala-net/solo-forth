  \ random.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803091341
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Pseudo-random number generators.
  \
  \ See benchmark results in <meta.benchmark.rng.fsb>.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( rnd random fast-rnd fast-random )

unneeding rnd ?(

2variable rnd-seed  $0111 rnd-seed !

: rnd ( -- u )
  rnd-seed 2@ $62DC um* rot 0 d+ over rnd-seed 2! ; ?)

  \ doc{
  \
  \ rnd ( -- u ) "r-n-d"
  \
  \ Return a random number _u_.
  \
  \ See: `random`, `random-within`, `fast-rnd`.
  \
  \ }doc

unneeding random ?( need rnd

: random ( n1 -- n2 ) rnd um* nip ; ?)

  \ doc{
  \
  \ random ( n1 -- n2 )
  \
  \ Return a random number _n2_ from 0 to _n1_ minus 1.
  \
  \ See: `rnd`, `random-within`, `fast-random`.
  \
  \ }doc

  \ Credit:
  \
  \ Random Number Generator by C. G. Montgomery: `random` and
  \ `rnd`.
  \
  \ Found here (2015-12-13):
  \ http://web.archive.org/web/20060707001752/http://www.tinyboot.com/index.html
  \ http://web.archive.org/web/20060714230130/http://tinyboot.com:80/rng.txt

unneeding fast-rnd ?( need os-seed

code fast-rnd ( -- u )
  2A c, os-seed , 54 c, 5D c, 29 c, 19 c, 29 c, 19 c,
    \ ld hl,(seed)
    \ ld d,h
    \ ld e,l
    \ add hl,hl
    \ add hl,de
    \ add hl,hl
    \ add hl,de
  29 c, 19 c, 29 c, 29 c, 29 c, 29 c, 19 c,
    \ add hl,hl
    \ add hl,de
    \ add hl,hl
    \ add hl,hl
    \ add hl,hl
    \ add hl,hl
    \ add hl,de
  24 c, 23 c, 22 c, os-seed , E5 c, jpnext, end-code ?)
    \ inc h
    \ inc hl
    \ ld (seed),hl
    \ push hl
    \ _jp_next

  \ doc{
  \
  \ fast-rnd ( -- u ) "fast-r-n-d"
  \
  \ Return a random number _u_.
  \
  \ ``fast-rnd`` generates a sequence of pseudo-random values
  \ that has a cycle of 65536 (so it will hit every single
  \ number): ``f(n+1)=241f(n)+257``.
  \
  \ See: `fast-random`, `rnd`.
  \
  \ }doc

unneeding fast-random ?( need fast-rnd

: fast-random ( n1 -- n2 ) fast-rnd um* nip ; ?)

  \ doc{
  \
  \ fast-random ( n1 -- n2 )
  \
  \ Return a random number _n2_ from 0 to _n1_ minus 1.
  \
  \ See: `fast-rnd`, `random`.
  \
  \ }doc

  \ Credit:
  \
  \ Code adapted from:
  \ http://z80-heaven.wikidot.com/math#toc40

  \ Original code:

  \ ----
  \ PseudoRandWord:
  \
  \ ; this generates a sequence of pseudo-random values
  \ ; that has a cycle of 65536 (so it will hit every
  \ ; single number):
  \
  \ ;f(n+1)=241f(n)+257   ;65536
  \ ;181 cycles, add 17 if called
  \
  \ ;Outputs:
  \ ;     BC was the previous pseudorandom value
  \ ;     HL is the next pseudorandom value
  \ ;Notes:
  \ ;     You can also use B,C,H,L as pseudorandom 8-bit values
  \ ;     this will generate all 8-bit values
  \      .db 21h    ;start of ld hl,**
  \ randSeed:
  \      .dw 0
  \      ld c,l
  \      ld b,h
  \      add hl,hl
  \      add hl,bc
  \      add hl,hl
  \      add hl,bc
  \      add hl,hl
  \      add hl,bc
  \      add hl,hl
  \      add hl,hl
  \      add hl,hl
  \      add hl,hl
  \      add hl,bc
  \      inc h
  \      inc hl
  \      ld (randSeed),hl
  \      ret
  \ ----

( random-within random-between )

unneeding random-within ?( need random

: random-within ( n1 n2 -- n3 ) over - random + ; ?)

  \ doc{
  \
  \ random-within ( n1 n2 -- n3 )
  \
  \ Return a random number _n3_ from _n1_ (min) to _n2_ (max).
  \
  \ See: `random-between`, `random`, `within`.
  \
  \ }doc

unneeding random-between ?( need random-within

: random-between ( n1 n2 -- n3 ) 1+ random-within ; ?)

  \ doc{
  \
  \ random-between ( n1 n2 -- n3 )
  \
  \ Return a random number _n3_ from _n1_ (min) to _n2-1_
  \ (max).
  \
  \ See: `random-within`, `random`, `between`.
  \
  \ }doc

( crnd -1|1 -1..1 randomize randomize0 )

unneeding crnd ?( need os-seed

code crnd ( -- b )
  2A c, os-seed , ED c, 5F c, 57 c, 5E c, 19 c,
    \ ld      hl,(randData)
    \ ld      a,r
    \ ld      d,a
    \ ld      e,(hl)
    \ add     hl,de
  85 c, AC c, 22 c, os-seed , pusha jp, end-code ?)
    \ add     a,l
    \ xor     h
    \ ld      (randData),hl
    \ jp push_a

  \ Credit:
  \
  \ http://wikiti.brandonw.net/index.php?title=Z80_Routines:Math:Random
  \ Joe Wingbermuehle

  \ Original code:

  \ ----
  \ ; ouput a=answer 0<=a<=255
  \ ; all registers are preserved except: af
  \ random:
  \         push    hl
  \         push    de
  \         ld      hl,(randData)
  \         ld      a,r
  \         ld      d,a
  \         ld      e,(hl)
  \         add     hl,de
  \         add     a,l
  \         xor     h
  \         ld      (randData),hl
  \         pop     de
  \         pop     hl
  \         ret
  \ ----

  \ doc{
  \
  \ crnd ( -- b ) "c-r-n-d"
  \
  \ Return a random 8-bit number _b_ (0..255).
  \
  \ See: `rnd`.
  \
  \ }doc

unneeding -1|1
?\ need random : -1|1 ( -- -1|1 ) 2 random 2* 1- ;

  \ doc{
  \
  \ -1|1 ( -- -1|1 ) "minus-one-bar-one"
  \
  \ Return a random number: -1 or 1.
  \
  \ See: `-1..1`, `rnd`, `fast-random`.
  \
  \ }doc

unneeding -1..1
?\ need random : -1..1 ( -- -1|0|1 ) 3 random 1- ;

  \ doc{
  \
  \ -1..1 ( -- -1|0|1 ) "minus-one-dot-dot-one"
  \
  \ Return a random number: -1, 0 or 1.
  \
  \ See: `-1|1`, `rnd`, `fast-random`.
  \
  \ }doc

unneeding randomize
?\ need os-seed : randomize ( n -- ) os-seed ! ;

  \ doc{
  \
  \ randomize ( n -- )
  \
  \ Set the seed used by `fast-rnd` and `fast-random` to _n_.
  \
  \ See: `randomize0`.
  \
  \ }doc

unneeding randomize0 ?( need os-frames need randomize

: randomize0 ( n -- )
  ?dup 0= if os-frames @ then randomize ; ?)

  \ doc{
  \
  \ randomize0 ( -- ) "randomize-zero"
  \
  \ Set the seed used by `fast-rnd` and `fast-random` to _n_;
  \ if _n_ is zero use the system frames counter instead.
  \
  \ See: `randomize`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-12-25: Add `crnd`.
  \
  \ 2016-03-31: Adapted C. G. Montgomery's `rnd`.
  \
  \ 2016-04-08: Updated the literal in C. G. Montgomery's `rnd`
  \ after the latest benchmarks.
  \
  \ 2016-10-18: Update the name of the benchmarks library
  \ module.
  \
  \ 2016-12-06: Add `-1|1`. Improve documentation and needing
  \ of `randomize` and `randomize0`.
  \
  \ 2016-12-12: Fix needing of `-1|1` and `randomize`.
  \
  \ 2016-12-20: Rename `jppushhl` to `jppushhl,` after the
  \ change in the kernel.
  \
  \ 2016-12-30: Compact the code, saving one block.
  \
  \ 2017-01-02: Convert `fast-rnd` from `z80-asm` to
  \ `z80-asm,`.
  \
  \ 2017-01-04: Convert `crnd` from `z80-asm` to `z80-asm,`;
  \ add its missing requirement. Make `crnd` and `crandom`
  \ accessible to `need`; improve their documentation.
  \
  \ 2017-01-05: Update `need z80-asm,` to `need assembler`.
  \
  \ 2017-01-12: Add `-1..1`.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-03-02: Fix `crnd` (a bug introduced when the word was
  \ converted to the new assembler).
  \
  \ 2017-03-16: Compact the code, saving two blocks.  Complete
  \ and improve documentation. Rewrite `fast-rnd` and `crnd`
  \ with Z80 opcodes.
  \
  \ 2017-05-09: Remove `jppushhl,`. Improve documentation.
  \
  \ 2017-12-11: Add `random-within`. Rewrite `random-range` as
  \ `random-between`. Improve needing of `rnd`.
  \
  \ 2018-01-23: Fix, benchmark and remove `crandom`, which is
  \ slower than `random`.  Fix cross reference.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-09: Add words' pronunciaton.

  \ vim: filetype=soloforth
