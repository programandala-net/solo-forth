  \ meta.benchmark.rng.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702220020

  \ -----------------------------------------------------------
  \ Description

  \ RNG benchmarks written during the development of Solo Forth
  \ in order to choose the best implementations.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ Latest changes

  \ 2016-04-24: Add `need 2rdrop`, because `2rdrop` has been
  \ moved from the kernel to the library.
  \
  \ 2016-11-23: Rename `c!set-bits` to `cset` after the changes
  \ in the system.
  \
  \ 2016-12-20: Rename `jppushhl` to `jppushhl,` after the
  \ change in the kernel.
  \
  \ 2017-01-03: Convert all assembly words from `z80-asm` to
  \ `z80-asm,`.
  \
  \ 2017-01-04: Fix: add another missing `need 2rdrop`, for
  \ `16-bit-random-pix-benchmarks`. Convert `zh-crnd` to
  \ `z80-asm,` and add its requirements. Shorten name parts:
  \ "random-pix" to "rng-pix"; "-benchmark" to "-bench". Try
  \ all of the benchs.
  \
  \ 2017-01-05: Update `need z80-asm,` to `need assembler`.
  \
  \ 2017-01-23: Fix name.
  \
  \ 2017-02-17: Update notation "behaviour" to "action".
  \
  \ 2017-02-19: Replace `do`, which has been moved to the
  \ library, with `?do`.

( rnd-bench )

  \ XXX UNDER DEVELOPMENT -- new benchmark
  \ 2016-04-07: Start

need bits need u% need cset

$FFFF constant #sampled
8 constant /byte  \ bits
#sampled /byte / constant /sample

create sample  /sample allot

: -sample ( -- ) sample /sample erase ;
  \ Erase the sample.

: sampled ( -- u ) sample /sample bits ;
  \ Count the bits of the sample.

: remember ( u -- ) /byte /mod sample + cset ;
  \ Remember random number _u_, by setting its associated bit
  \ in the sample.

: sampled%. ( u -- ) #sampled u% 0.r ." %" ;
  \ Print _u_ sampled numbers as a percentage.

: .sampled ( u -- ) dup u. ." (" sampled%. ." )" ;
  \ Print _u_ as the number of sampled numbers.

: report ( ca len -- ) type sampled .sampled cr ;

: rnd-bench ( ca len xt -- )
  -sample
  #sampled 0 ?do  dup execute remember  loop  drop
  report ;

( rng-pix-bench )

  \ Random pixels benchmark

need set-pixel need bench{ need pixels need u% need 3dup
need 2rdrop

256 192 * constant #pixels
  \ number of pixels of the screen

defer rng ( n -- 0..n-1 )

: pixels%. ( u -- ) #pixels u% 0.r ." %" ;
  \ Print _u_ pixels as a percentage of the maximum number of
  \ pixels.

: .pixels ( u -- ) dup u. ." pixels (" pixels%. ." )" ;
  \ Print _u_ as the number of pixels.

: .title ( ca len -- ) ." Code: " type ;

variable cycles

defer .cycles ( -- )

: (.cycles) ( -- )
  cycles ?  s" cycles" cycles @ 1 = + type ;
  \ Print the number of cycles.

: .time ( d -- ) bench. ." per cycle" cr ;

: .result ( ca len d -- )
  2>r pixels >r  .title cr  r> .pixels cr
  2r> .time .cycles ;  -->
  \ Calculate and print the result of the benchmark.
  \ _d_ is the time in frames; _ca len_ is the title.

( rng-pix-bench )

defer random-coords ( -- gx gy )
  \ Random graphic coordinates. Configurable depending on the
  \ type of `random` to benchmark.

: (random-coords) ( -- gx gy ) 256 rng 192 rng ;
  \ Default action of `random-coords`.

: fill-screen ( -- )
  #pixels 0 ?do  random-coords set-pixel  loop ;
  \ Fill the screen with random pixels.

: signal ( -- ) cycles @ %111 and border ;
  \ Change the border color according to the current count
  \ of cycles, just to show that the benchmark is running.

: (rnd-pix-bench) ( -- d )
  1 cycles +!  signal  bench{ fill-screen }bench ;
  \ Do one cycle of the benchmark and return its result.

: wait ( -- ) key drop ;

: finish ( ca len d -- ) 0 border  .result  wait ;
  \ Finish the benchmark.
  \ _d_ is the time in frames; _ca len_ is the title.

: init ( xt1 xt2 xt3 -- )
  ['] random-coords defer!  ['] .cycles defer!  ['] rng defer!
  page  -1 cycles ! ;

defer finish? ( i*x -- j*x f )
  \ Finish the benchmark?

: new-pixels? ( n1 -- n2 f ) pixels tuck = ;
  \ Are there new pixels on the screen, comparing the previous
  \ count _n1_ with the new count _n2_?

' new-pixels? ' finish? defer!  -->

( rng-pix-bench )

defer rng-pix-bench ( ca len xt -- )
  \ Do a RNG benchmark for the `random` word _xt_ with title
  \ _ca len_.

: (rnd-pix-bench2) ( ca len -- )
  0 begin   (rnd-pix-bench) 2>r
            finish? dup 0= if  2rdrop  then
  until     drop 2r> finish ;
  \ Do a double RNG benchmark with title _ca len_: The time
  \ required to complete one cycle (49152 random pixels), plus
  \ the number of cycles required until the number of pixels
  \ doesn't change.

: rng-pix-bench2 ( ca len xt -- )
  ['] (.cycles) ['] (random-coords) init
  (rnd-pix-bench2) ;
  \ Do a double RNG benchmark for the `random` word _xt_ with
  \ title _ca len_: The time required to complete one cycle
  \ (49152 random pixels), plus the number of cycles required
  \ until the number of pixels doesn't change.

  \ The best `random` words need several cycles. In such cases
  \ it's useful a simpler test to show only the pixels set at
  \ the end of the first cycle:

' rng-pix-bench2 ' rng-pix-bench defer!
  \ default benchmark

: (.cycle) ( -- ) ." First cycle only" ;

: (rnd-pix-bench1) ( ca len -- )
  (rnd-pix-bench) .result wait ;
  \ Do a one-cycle RNG benchmark with title _ca len_: Only the
  \ time required to complete one cycle (49152 random pixels).

: rng-pix-bench1 ( ca len xt -- )
  ['] (.cycle) ['] (random-coords) init
  (rnd-pix-bench1) ;  -->
  \ Do a one-cycle RNG benchmark for `random` word _xt_ with
  \ title _ca len_: Only the time required to complete one
  \ cycle (49152 random pixels).

( rng-pix-bench )

  \ Versions for 8-bit `rnd`.

: crnd-coords ( -- gx gy ) rng rng 192 min ;
  \ Random graphic coordinates for 8-bit `rnd`.

: crnd-pix-bench2 ( ca len xt -- )
  ['] (.cycles) ['] crnd-coords init
  (rnd-pix-bench2) ;
  \ Do a double RNG benchmark for 8-bit `rnd` word _xt_ with
  \ title _ca len_: The time required to complete one cycle
  \ (49152 random pixels), plus the number of cycles required
  \ until the number of pixels doesn't change.

: crnd-pix-bench1 ( ca len xt -- )
  ['] (.cycle) ['] crnd-coords init
  (rnd-pix-bench1) ;
  \ Do a one-cycle RNG benchmark for 8-bit `rnd` word _xt_ with
  \ title _ca len_: Only the time required to complete one
  \ cycle (49152 random pixels).

( 16-bit-rng-pix-benchs )

  \ Execute all of the 16-bit random pixels benchmarks

need rng-pix-bench need +thru  2 21 +thru

ace-rng-pix-bench

cgm-5E9B-rng-pix-bench cgm-61BF-rng-pix-bench
cgm-62DC-rng-pix-bench cgm-6363-rng-pix-bench
cgm-6594-rng-pix-bench cgm-65E8-rng-pix-bench

dx-rng-pix-bench gf-rng-pix-bench
jer-rng-pix-bench jml-rng-pix-bench
lb-rng-pix-bench lina-rng-pix-bench
mb-rng-pix-bench

  \ mm-rng-pix-bench  \ XXX TMP --

sf83-rng-pix-bench tt-rng-pix-bench
vf-rng-pix-bench z88-rng-pix-bench
zh-rng-pix-bench

-->

( 16-bit-rng-pix-benchs )

  \ Execute single-cycle benchmarks of RNG that need more than
  \ one cycle to finish:

' rng-pix-bench1 ' rng-pix-bench defer!

cgm-5E9B-rng-pix-bench cgm-61BF-rng-pix-bench
cgm-62DC-rng-pix-bench cgm-6363-rng-pix-bench
cgm-6594-rng-pix-bench cgm-65E8-rng-pix-bench

dx-rng-pix-bench  vf-rng-pix-bench

( ace-random )

  \ Credit:
  \
  \ Adapted from ACE Forth, after the Jupiter ACE manual.
  \ Also used by Abersoft Forth in its bundled game
  \ "Bertie".

need os-seed

: ace-rnd ( -- u )
  os-seed @ 75 um* 75. d+ 2dup u< - - 1- dup os-seed ! ;

: ace-random ( n -- 0..n-1 ) ace-rnd um* nip ;

need rng-pix-bench

: ace-rng-pix-bench ( -- )
  os-seed off  s" Jupiter ACE manual"
  ['] ace-random rng-pix-bench ;

( cgm-5E9B-random )

  \ Random Number Generator by C. G. Montgomery

  \ 2015-12-13: found here:
  \ http://web.archive.org/web/20060707001752/http://www.tinyboot.com/index.html
  \
  \ 2016-03-31: adapted to Solo Forth.

2variable rloc  $111 rloc !  \ seed with nonzero

: cgm-5E9B-rnd ( -- u )
  rloc 2@ $5E9B um* rot 0 d+ over rloc 2! ;
  \ good values for 16-bit systems: 61BF 62DC 6594 6363 5E9B 65E8

: cgm-5E9B-random ( n -- 0..n-1 ) cgm-5E9B-rnd um* nip ;

need rng-pix-bench

: cgm-5E9B-rng-pix-bench ( -- )
  s" C. G. Montgomery $5E9B"
  ['] cgm-5E9B-random rng-pix-bench ;

( cgm-61BF-random )

  \ Random Number Generator by C. G. Montgomery

  \ 2015-12-13: found here:
  \ http://web.archive.org/web/20060707001752/http://www.tinyboot.com/index.html
  \
  \ 2016-03-31: adapted to Solo Forth.

2variable rloc  $111 rloc !  \ seed with nonzero

: cgm-61BF-rnd ( -- u )
  rloc 2@ $61BF um* rot 0 d+ over rloc 2! ;
  \ good values for 16-bit systems: 61BF 62DC 6594 6363 5E9B 65E8

: cgm-61BF-random ( n -- 0..n-1 ) cgm-61BF-rnd um* nip ;

need rng-pix-bench

: cgm-61BF-rng-pix-bench ( -- )
  s" C. G. Montgomery $61BF"
  ['] cgm-61BF-random rng-pix-bench ;

( cgm-62DC-random )

  \ Random Number Generator by C. G. Montgomery

  \ 2015-12-13: found here:
  \ http://web.archive.org/web/20060707001752/http://www.tinyboot.com/index.html
  \
  \ 2016-03-31: adapted to Solo Forth.

2variable rloc  $111 rloc !  \ seed with nonzero

: cgm-62DC-rnd ( -- u )
  rloc 2@ $62DC um* rot 0 d+ over rloc 2! ;
  \ good values for 16-bit systems: 61BF 62DC 6594 6363 5E9B 65E8

: cgm-62DC-random ( n -- 0..n-1 ) cgm-62DC-rnd um* nip ;

need rng-pix-bench

: cgm-62DC-rng-pix-bench ( -- )
  s" C. G. Montgomery $62DC"
  ['] cgm-62DC-random rng-pix-bench ;

( cgm-6363-random )

  \ Random Number Generator by C. G. Montgomery

  \ 2015-12-13: found here:
  \ http://web.archive.org/web/20060707001752/http://www.tinyboot.com/index.html
  \
  \ 2016-03-31: adapted to Solo Forth.

2variable rloc  $111 rloc !  \ seed with nonzero

: cgm-6363-rnd ( -- u )
  rloc 2@ $6363 um* rot 0 d+ over rloc 2! ;
  \ good values for 16-bit systems: 61BF 62DC 6594 6363 5E9B 65E8

: cgm-6363-random ( n -- 0..n-1 ) cgm-6363-rnd um* nip ;

need rng-pix-bench

: cgm-6363-rng-pix-bench ( -- )
  s" C. G. Montgomery $6363"
  ['] cgm-6363-random rng-pix-bench ;

( cgm-6594-random )

  \ Random Number Generator by C. G. Montgomery

  \ 2015-12-13: found here:
  \ http://web.archive.org/web/20060707001752/http://www.tinyboot.com/index.html
  \
  \ 2016-03-31: adapted to Solo Forth.

2variable rloc  $111 rloc !  \ seed with nonzero

: cgm-6594-rnd ( -- u )
  rloc 2@ $6594 um* rot 0 d+ over rloc 2! ;
  \ good values for 16-bit systems: 61BF 62DC 6594 6363 5E9B 65E8

: cgm-6594-random ( n -- 0..n-1 ) cgm-6594-rnd um* nip ;

need rng-pix-bench

: cgm-6594-rng-pix-bench ( -- )
  s" C. G. Montgomery $6594"
  ['] cgm-6594-random rng-pix-bench ;

( cgm-65E8-random )

  \ Random Number Generator by C. G. Montgomery

  \ 2015-12-13: found here:
  \ http://web.archive.org/web/20060707001752/http://www.tinyboot.com/index.html
  \
  \ 2016-03-31: adapted to Solo Forth.

2variable rloc  $111 rloc !  \ seed with nonzero

: cgm-65E8-rnd ( -- u )
  rloc 2@ $65E8 um* rot 0 d+ over rloc 2! ;
  \ good values for 16-bit systems: 61BF 62DC 6594 6363 5E9B 65E8

: cgm-65E8-random ( n -- 0..n-1 ) cgm-65E8-rnd um* nip ;

need rng-pix-bench

: cgm-65E8-rng-pix-bench ( -- )
  s" C. G. Montgomery $65E8"
  ['] cgm-65E8-random rng-pix-bench ;

( dx-random )

  \ Credit:
  \
  \ Code from DX-Forth 4.13.

2variable dx-seed  1. dx-seed 2!

need d*

: dx-rnd ( -- u )
  dx-seed 2@ $15A4E35. d* 1. d+ tuck dx-seed 2! ;
  \ Get random number

: dx-random ( u -- 0..u-1 ) dx-rnd um* nip ;
  \ Get random number between 0 and u-1

need rng-pix-bench

: dx-rng-pix-bench ( -- )
  s" DX-Forth" ['] dx-random rng-pix-bench ;

( gf-random )

  \ Credit:
  \
  \ Adapted from Gforth.

need os-seed need ud*

: gf-rnd ( -- n )
  272958469. os-seed @ ud* d>s 1+ dup os-seed ! ;

: gf-random ( n -- 0..n-1 ) gf-rnd um* nip ;

need rng-pix-bench

: gf-rng-pix-bench ( -- )
  os-seed off  s" Gforth"
  ['] gf-random rng-pix-bench ;

( jer-random )

  \ Credit:
  \
  \ Random number generator by J. E. Rickenbacker, published on
  \ Forth Dimensions (volume 2, number 2, page 34, 1980-07).

need os-seed

: jer-rnd ( -- n )
  os-seed @ 259 * 3 + 32767 and dup os-seed ! ;

: jer-random ( n1 -- n2 )
  jer-rnd 32767 */ ;
  \ Return a random number _n2_ (0 <= n2 < n1).

  \ XXX Note: patterns

need rng-pix-bench

: jer-rng-pix-bench ( -- )
  os-seed off  s" J. E. Rickenbacker"
  ['] jer-random rng-pix-bench ;

( jml-random )

  \ Credit:
  \
  \ Adapted from code written by Jos� Manuel Lazo,
  \ published on Microhobby, issue 85 (1986-07), page 24:
  \ http://microhobby.org/numero085.htm
  \ http://microhobby.speccy.cz/mhf/085/MH085_24.jpg

need assembler need os-seed

code jml-rnd ( -- u )

  os-seed fthl, h push,
  h addp, h addp, h addp, h addp, h addp, h addp,
  d pop, d addp, 0029 d ldp#, d addp,
  os-seed sthl,
  jppushhl,
  end-code

: jml-random ( n -- 0..n-1 ) jml-rnd um* nip ;

need rng-pix-bench

: jml-rng-pix-bench ( -- )
  os-seed off  s" J. M. Lazo"
  ['] jml-random rng-pix-bench ;

( lb-random )

  \ Credit:
  \
  \ Code adapted from Leo Brodie's _Starting Forth_.

need os-seed

: lb-rnd ( -- u ) os-seed @ 31421 * 6927 + dup os-seed ! ;

: lb-random ( n -- 0..n-1 ) lb-rnd um* nip ;

need rng-pix-bench

: lb-rng-pix-bench ( -- )
  os-seed off  s" Leo Brodie"
  ['] lb-random rng-pix-bench ;

( lina-random )

need os-seed

: lina-rnd ( -- n )
  os-seed @ 107465 * 234567 + dup os-seed ! ;

: lina-random ( n -- 0..n-1 ) lina-rnd um* nip ;

need rng-pix-bench

: lina-rng-pix-bench ( -- )
  os-seed off  s" lina"
  ['] lina-random rng-pix-bench ;

( mb-random )

  \ Credit:
  \
  \ Adapted from code published by Milos Bazelides:
  \ http://web.archive.org/web/20150225121110/http://baze.au.com/misc/z80bits.html#4

need assembler need os-seed

code mb-rnd ( -- u )
  os-seed d ftp,
  d a ld, e h ld, #253 l ld#,
  a or, d sbcp,
  0 sbc#, d sbcp,
  0 d ld#, d sbc, a e ld, d sbcp,
  c? rif  h incp,  rthen
  os-seed sthl, jppushhl, end-code

: mb-random ( n -- 0..n-1 ) mb-rnd um* nip ;

  \ Original code:

  \ ----
  \ ; Input: none
  \ ; Output: HL = pseudo-random number, period 65536

  \ Rand16:
  \  ld  de,Seed ; Seed is usually 0
  \  ld  a,d
  \  ld  h,e
  \  ld  l,253
  \  or  a
  \  sbc  hl,de
  \  sbc  a,0
  \  sbc  hl,de
  \  ld  d,0
  \  sbc  a,d
  \  ld  e,a
  \  sbc  hl,de
  \  jr  nc,Rand
  \  inc  hl
  \ Rand:
  \  ld  (Rand16+1),hl
  \  ret
  \ ----

need rng-pix-bench

: mb-rng-pix-bench ( -- )
  os-seed off  s" Milos Bazelides"
  ['] mb-random rng-pix-bench ;

( mm-random )

  \ Credit:
  \ IsForth Random Number Generator, by Mark I Manning IV.

  \ 2016-04-04: Adapted to Solo Forth. Strange negative
  \ results.
  \ XXX TODO -- check the original

need cell/ need frames@

variable seed1  variable seed2

: randomize ( -- ) frames@ seed1 ! seed2 ! ;

: 0seed ( -- ) seed1 off seed2 off ;  0seed
  \ Reset random number generator seed to zero.

: mm-random ( n1 --- n2 )
  seed1 @ 123 * 234 + seed2 @ 234 * 123 +
  2dup + seed2 ! 2dup xor seed1 !
  + swap cells mod cell/ ;

need rng-pix-bench

: mm-rng-pix-bench ( -- )
  s" IsForth" ['] mm-random rng-pix-bench ;

  \ ' rng-pix-bench2 ' rng-pix-bench defer!

  \ mm-rng-pix-bench

  \ ' rng-pix-bench1 ' rng-pix-bench defer!

  \ mm-rng-pix-bench

( sf83-random )

  \ Credit:
  \
  \ Code from Spectrum Forth-83.

need os-seed  3 os-seed !

: sf83-random ( n -- 0..n-1 )
  os-seed @ 743 * 43 + dup os-seed ! um* swap drop ;

need rng-pix-bench

: sf83-rng-pix-bench ( -- )
  s" Spectrum Forth-83"
  ['] sf83-random rng-pix-bench ;

( tt-random )

  \ Credit:
  \
  \ Code from tt.pfe, Tetris for terminals, redone in
  \ ANSI-Forth.  Written 1994-04-05 by Dirk Uwe Zoller.
  \
  \ Note: the seed can not be zero.

need os-seed

: tt-random ( n -- 0..n-1 )
    os-seed @ 13 * $7FFF and
    dup os-seed !  swap mod ;

need rng-pix-bench

: tt-rng-pix-bench ( -- )
  os-seed on  s" Tetris for terminals"
  ['] tt-random rng-pix-bench ;

( vf-random )

  \ Credit:
  \
  \ Code from vForth.

need os-frames

: vf-random ( n -- 0..n-1 )
  1+ 8195 os-frames @ um* 1. d+
  16383 um/mod drop
  \ dup os-seed !
  swap mod ;

need rng-pix-bench

: vf-rng-pix-bench ( -- )
  s" vForth" ['] vf-random rng-pix-bench ;

( z88-random )

  \ Credit:
  \
  \ Code adapted from Z88 CamelForth.

need ud* need os-seed need 2rdrop

: z88-random ( n -- 0..n-1 )
  1103515245. \ 20077 16838
  os-seed @ ud* 12345. d+ over os-seed !
  rot ud/mod 2drop ;

need rng-pix-bench

: z88-rng-pix-bench ( -- )
  os-seed off  s" Z88 CamelForth"
  ['] z88-random rng-pix-bench ;

( zh-random )

  \ Credit:
  \
  \ Code adapted from:
  \ http://z80-heaven.wikidot.com/math#toc40

need assembler need os-seed

code zh-rnd ( -- u )

  os-seed fthl, h d ldp,
    \ ld hl,(seed)
    \ ld c,l
    \ ld b,h
  h addp, d addp, h addp, d addp, h addp,
  d addp, h addp, h addp, h addp, h addp, d addp,
    \ add hl,hl
    \ add hl,de
    \ add hl,hl
    \ add hl,de
    \ add hl,hl
    \ add hl,de
    \ add hl,hl
    \ add hl,hl
    \ add hl,hl
    \ add hl,hl
    \ add hl,de
  h inc, h incp, os-seed sthl, jppushhl, end-code
    \ inc h
    \ inc hl
    \ ld (seed),hl
    \ jp push_hl

: zh-random ( n -- 0..n-1 ) zh-rnd um* nip ;

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

need rng-pix-bench

: zh-rng-pix-bench ( -- )
  os-seed off  s" Z80 Heaven"
  ['] zh-random rng-pix-bench ;

( random-byte )

code random-byte ( -- b )
  ED c, 5F c,     \ ld a,r
  C3 c, pusha ,   \ jp pusha
  end-code

need bench{

: random-byte-bench ( -- )
  ['] random-byte ['] rng defer!  cls  bench{ pixels
  ?do  rng rng 192 min set-pixel  loop  }bench.
  ." Z80 R register" cr key drop ;

( lcm-random )

  \ XXX UNDER DEVELOPMENT

  \ Credit:
  \
  \ Adapted from code written by Everett F. Carter, published
  \ on Forth Dimensions (volume 16, number 2, page 17,
  \ 1994-08).
  \
  \ Linear Congruential Method, the "minimal standard
  \ generator", Park & Miller, 1988, Comm of the ACM, 31(10),
  \ pp. 1192-1201

need d* need du/mod need 2nip

2variable 2seed

2147483647. 2constant max32

: lcm-rnd ( -- d )
  2seed 2@ 16807. d*
  max32 du/mod  2nip
  2dup 2seed 2! ;
  \ XXX FIXME -- it always returns 0

  \ \ Original code:
  \ : lcm-rnd ( -- d )
  \   2seed 2@ 16807. umd*
  \   max32 umd/mod
  \   2drop 2seed 2! ;

: lcm-random ( n -- 0..n-1 ) lcm-rnd d>s um* nip ;

need rng-pix-bench

: lcm-rng-pix-bench ( -- )
  s" LCM" ['] lcm-random rng-pix-bench ;

( 8-bit-rng-pix-benchs )

  \ Execute all of the 8-bit random pixels benchmarks

need rng-pix-bench need +thru  1 8 +thru

' crnd-pix-bench2 ' rng-pix-bench defer!

  \ XXX TODO -- check the libzx rng benchs

  jw-rng-pix-bench
  \ mb1-rng-pix-bench  mb2-rng-pix-bench
  zh-rng-pix-bench
  libzx-rng-pix-bench
  libzx-rng-pix-bench-opt2
  libzx-rng-pix-bench-opt1
  libzx-rng-pix-bench-opt3  \ XXX FIXME -- crash after!

  \ Execute single-cycle benchmarks of RNG that need more than
  \ one cycle to finish:

' crnd-pix-bench1 ' rng-pix-bench defer!

  jw-rng-pix-bench
  libzx-rng-pix-bench  \ XXX FIXME -- crash after!
  libzx-rng-pix-bench-opt1
  libzx-rng-pix-bench-opt2
  libzx-rng-pix-bench-opt3

( jw-crnd )

  \ 2015-12-25

  \ Credit:
  \
  \ http://wikiti.brandonw.net/index.php?title=Z80_Routines:Math:Random
  \ Joe Wingbermuehle

need assembler need os-seed

code jw-crnd ( -- b )

  os-seed fthl, ED c, 5F c, a d ld, m e ld,
    \ ld      hl,(randData)
    \ ld      a,r
    \ ld      d,a
    \ ld      e,(hl)
  d addp, l add, h xor, os-seed sthl, pusha jp, end-code
    \ add     hl,de
    \ add     a,l
    \ xor     h
    \ ld      (randData),hl
    \ jp push_a

need rng-pix-bench

: jw-rng-pix-bench ( -- )
  os-seed off  s" Joe Wingbermuehle"
  ['] jw-crnd rng-pix-bench ;

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

( mb1-crnd )

  \ XXX UNDER DEVELOPMENT

  \ 2015-12-25

  \ Credit:
  \ http://web.archive.org/web/20150225121110/http://baze.au.com/misc/z80bits.html#4

  \ This is a very simple linear congruential generator. The
  \ formula is x[i + 1] = (5 * x[i] + 1) mod 256. Its only
  \ advantage is small size and simplicity. Due to nature of
  \ such generators only a couple of higher bits should be
  \ considered random.

  \ Input: none
  \ Output: A = pseudo-random number, period 256

need assembler need os-seed

code mb1-crnd ( -- b )

  os-seed fta, a d ld, a add, a add, d add,
    \ ld  a,(seed) ; Seed is usually 0
    \ ld  d,a
    \ add  a,a
    \ add  a,a
    \ add  a,d
  a inc, os-seed sta, pusha jp, end-code
    \ inc  a ; another possibility is ADD A,7
    \ ld  (seed),a
    \ jp push_a

: mb1-crandom ( n1 -- n2 ) mb1-crnd um* nip ;
  \ XXX FIXME -- it always return zero

need rng-pix-bench

  \ : mb1-rng-pix-bench ( -- )
  \   s" Milos Bazelides 1" ['] mb1-crandom rng-pix-bench ;

: mb1-rng-pix-bench ( -- )
  os-seed off  s" Milos Bazelides 1 (8 bit)"
  ['] mb1-crnd rng-pix-bench ;

( mb2-crnd )

  \ XXX UNDER DEVELOPMENT

  \ 2015-12-25

  \ Credit:
  \ http://web.archive.org/web/20150225121110/http://baze.au.com/misc/z80bits.html#4

   \ This is a very simple linear congruential generator. The
   \ formula is x[i + 1] = (5 * x[i] + 1) mod 256. Its only
   \ advantage is small size and simplicity. Due to nature of
   \ such generators only a couple of higher bits should be
   \ considered random.

  \ Input: none
  \ Output: A = pseudo-random number, period 256

need assembler need os-seed

code mb2-crnd ( -- b )

  os-seed fta, a d ld, a add, a add, d add,
    \ ld  a,(seed) ; Seed is usually 0
    \ ld  d,a
    \ add  a,a
    \ add  a,a
    \ add  a,d
  07 add#, os-seed sta, pusha jp, end-code
    \ add a,7
    \ ld  (seed),a
    \ jp push_a

: mb2-crandom ( n1 -- n2 ) mb2-crnd um* nip ;
  \ XXX FIXME -- it always return zero

need rng-pix-bench

  \ : mb2-rng-pix-bench ( -- )
  \   s" Milos Bazelides 2" ['] mb2-crandom rng-pix-bench ;

: mb2-rng-pix-bench ( -- )
  os-seed off  s" Milos Bazelides 2 (8 bit)"
  ['] mb2-crnd rng-pix-bench ;

( zh-crnd )

  \ 2015-12-25

  \ Credit:
  \
  \ Code adapted from:
  \ http://z80-heaven.wikidot.com/math#toc40

need assembler need os-seed

code zh-crnd ( -- b )

  os-seed fta, a e ld,
  a add, e add, a add, a add, e add, #83 add#,
  os-seed sta,
  pusha jp,

  end-code

need rng-pix-bench

: zh-rng-pix-bench ( -- )
  s" Z80 Heaven (8 bit)" ['] zh-crnd rng-pix-bench ;

  \ This is one of many variations of PRNGs. This routine is
  \ not particularly useful for many games, but is fairly
  \ useful for shuffling a deck of cards. It uses SMC, but that
  \ can be fixed by defining randSeed elsewhere and using ld
  \ a,(randSeed) at the beginning.

  \ PseudoRandByte:
  \ ;f(n+1)=13f(n)+83
  \ ;97 cycles
  \      .db 3Eh     ;start of ld a,*
  \ randSeed:
  \      .db 0
  \      ld c,a
  \      add a,a
  \      add a,c
  \      add a,a
  \      add a,a
  \      add a,c
  \      add a,83
  \      ld (randSeed),a
  \      ret

( libzx-crnd-opt3 )

  \ Credit:
  \ Original code from the ZX Spectrum libzx library,
  \ written by Sebastian Mihai, 2016

  \ 2016-04-09: Adapted to Solo Forth. Optimized and modified
  \ the original code.

need assembler need os-seed need rng-pix-bench

variable rom-pointer  rom-pointer off  os-seed off

code libzx-crnd-opt3 ( -- b )

  \ Gets an 8-bit random number.
  \ It is computed using a combination of:
  \     - the last returned random number
  \     - a byte from ROM, in increasing order
  \     - current values of various registers
  \     - a flat incremented value

  b push, af push,
    \ save Forth IP and the AF register

  \ 1) advance ROM pointer

  rom-pointer h ftp, h incp,
  h a ld, %00111111 and, a h ld, h rom-pointer stp,

    \ ld hl,(romPointer)
    \ inc hl
    \ ld a, h
    \ and %00111111
    \ ld h, a ; H := H mod %00111111
    \ ; essentially, HL := HL mod 16384, to make sure
    \ ; HL points at a ROM location
    \ ld (romPointer), hl ; save new location

  \ 2) compute the random number

  b pop, c rlc, b rlc, os-seed fta,
    \ pop bc ; BC := AF
    \ rlc c
    \ rlc b
    \ ld a, (lastRandomNumber)
  47 add#, b add, c add, d add, e add, h add, l add,
    \ ; current register values are "pretty random"
    \ ; so add them in the mix:
    \ add a, $47
    \ add a, b
    \ add a, c
    \ add a, d
    \ add a, e
    \ add a, h
    \ add a, l

  rom-pointer h ldp#, m add,
    \ ld hl, romPointer
    \ add a, (hl) ; the contents of the ROM are "pretty random"
    \ ; so add it in the mix

  os-seed sta, b pop, pusha jp, end-code

: libzx-rng-pix-bench-opt3 ( -- )
  rom-pointer off  os-seed off  s" libzx opt3 (8 bit)"
  ['] libzx-crnd-opt3 rng-pix-bench ;

( libzx-crnd-opt2 )

  \ Credit:
  \ Original code from the ZX Spectrum libzx library,
  \ written by Sebastian Mihai, 2016

  \ 2016-04-09: Adapted to Solo Forth. Optimized and modified
  \ the original code.

need assembler need os-seed need rng-pix-bench

variable rom-pointer  rom-pointer off  os-seed off

code libzx-crnd-opt2 ( -- b )

  \ Gets an 8-bit random number.
  \ It is computed using a combination of:
  \     - the last returned random number
  \     - a byte from ROM, in increasing order
  \     - current values of various registers
  \     - a flat incremented value

  b push, af push,
    \ save Forth IP and the AF register

  \ 1) advance ROM pointer

  rom-pointer h ftp, h incp,
  h a ld, %00111111 and, a h ld, h rom-pointer stp,

    \ ld hl,(romPointer)
    \ inc hl
    \ ld a, h
    \ and %00111111
    \ ld h, a ; H := H mod %00111111
    \ ; essentially, HL := HL mod 16384, to make sure
    \ ; HL points at a ROM location
    \ ld (romPointer), hl ; save new location

  \ 2) compute the random number

  b pop, c rlc, b rlc, os-seed fta,
    \ pop bc ; BC := AF
    \ rlc c
    \ rlc b
    \ ld a, (lastRandomNumber)
  47 add#, b add, c add, d add, e add, h add, l add,
    \ add a, 47
    \ add a, b ; current register values are "pretty random"
    \ add a, c ; so add them in the mix
    \ add a, d
    \ add a, e
    \ add a, h
    \ add a, l

  rom-pointer h ldp#, m add,
    \ ld hl, romPointer
    \ add a, (hl) ; the contents of the ROM are "pretty random"
    \ ; so add it in the mix

  os-seed sta,
    \ ld (lastRandomNumber), a ; save this number

  b pop, pusha jp, end-code

: libzx-rng-pix-bench-opt2 ( -- )
  rom-pointer off  os-seed off  s" libzx opt2 (8 bit)"
  ['] libzx-crnd-opt2 rng-pix-bench ;

( libzx-crnd-opt1 )

  \ Credit:
  \ Original code from the ZX Spectrum libzx library,
  \ written by Sebastian Mihai, 2016

  \ 2016-04-09: Adapted to Solo Forth. Optimized the original
  \ code.

need assembler need os-seed need rng-pix-bench

variable rom-pointer  3 rom-pointer !  33 os-seed c!

code libzx-crnd-opt1 ( -- b )

  \ Gets an 8-bit random number.
  \ It is computed using a combination of:
  \     - the last returned random number
  \     - a byte from ROM, in increasing order
  \     - current values of various registers
  \     - a flat incremented value

  b push, af push,
    \ save Forth IP and the AF register

  \ 1) advance ROM pointer

  rom-pointer b ftp, 3 h ldp#, b addp,

    \ ld bc,(romPointer)
    \ ld hl,3
    \ add hl,bc ; HL := ROM pointer advanced by 3

  h a ld, %00111111 and, a h ld, h rom-pointer stp,

    \ ld a, h
    \ and %00111111
    \ ld h, a ; H := H mod %00111111
    \ ; essentially, HL := HL mod 16384, to make sure
    \ ; HL points at a ROM location
    \ ld (romPointer), hl ; save new location

  \ 2) compute the random number

  b pop, c rlc, b rlc, os-seed fta,
    \ pop bc ; BC := AF
    \ rlc c
    \ rlc b
    \ ld a, (lastRandomNumber)
  47 add#, b add, c add, d add, e add, h add, l add,
    \ add a, 47
    \ add a, b ; current register values are "pretty random"
    \ add a, c ; so add them in the mix
    \ add a, d
    \ add a, e
    \ add a, h
    \ add a, l

  rom-pointer h ldp#, m add,
    \ ld hl, romPointer
    \ add a, (hl) ; the contents of the ROM are "pretty random"
    \ ; so add it in the mix

  os-seed sta,
    \ ld (lastRandomNumber), a ; save this number

  b pop, pusha jp, end-code

: libzx-rng-pix-bench-opt1 ( -- )
  3 rom-pointer !  33 os-seed c!  s" libzx opt1 (8 bit)"
  ['] libzx-crnd-opt1 rng-pix-bench ;

( libzx-crnd )

  \ Credit:
  \ Original code from the ZX Spectrum libzx library,
  \ written by Sebastian Mihai, 2016

  \ 2016-04-09: Adapted to Solo Forth.

need assembler need os-seed need rng-pix-bench

variable rom-pointer  3 rom-pointer !  33 os-seed c!

code libzx-crnd ( -- b )

  \ Gets an 8-bit random number.
  \ It is computed using a combination of:
  \     - the last returned random number
  \     - a byte from ROM, in increasing order
  \     - current values of various registers
  \     - a flat incremented value

  b push, af push,
    \ save Forth IP and the AF register

  \ 1) advance ROM pointer

  rom-pointer h ldp#,
  m c ld, h incp, m b ld, 3 h ldp#, b addp,
    \ XXX TODO -- simpler
    \ XXX REMARK -- original code is not optimized

    \ ld hl, romPointer
    \ ld c, (hl)
    \ inc hl
    \ ld b, (hl) ; BC := word (romPointer)
    \ ld hl, 3
    \ add hl, bc ; HL := ROM pointer advanced by 3

  h a ld, %00111111 and, a h ld, h rom-pointer stp,

    \ ld a, h
    \ and %00111111
    \ ld h, a ; H := H mod %00111111
    \ ; essentially, HL := HL mod 16384, to make sure
    \ ; HL points at a ROM location
    \ ld (romPointer), hl ; save new location

  \ 2) compute the random number

  b pop, c rlc, b rlc, os-seed fta,
    \ pop bc ; BC := AF
    \ rlc c
    \ rlc b
    \ ld a, (lastRandomNumber)
  47 add#, b add, c add, d add, e add, h add, l add,
    \ add a, 47
    \ add a, b ; current register values are "pretty random"
    \ add a, c ; so add them in the mix
    \ add a, d
    \ add a, e
    \ add a, h
    \ add a, l

  rom-pointer h ldp#, m add,
    \ ld hl, romPointer
    \ add a, (hl) ; the contents of the ROM are "pretty random"
    \ ; so add it in the mix

  os-seed h ldp#, m a ld,
    \ ld hl, lastRandomNumber
    \ ld (hl), a ; save this number
    \ XXX REMARK -- original code is not optimized

  b pop, 0 h ld#, a l ld, jppushhl, end-code

: libzx-rng-pix-bench ( -- )
  3 rom-pointer !  33 os-seed c!
  s" libzx (8 bit)" ['] libzx-crnd rng-pix-bench ;


  \ ===========================================================

  \ 2016-03-31
  \ Results of the 16-bit version of `rnd-pix-bench`:

  \ | Code | Pixels | Time per cycle in frames (and seconds) | Cycles

  \ | Jupiter ACE manual     | 05937 (012%) | 07652 (153 s) | 1
  \ | C. G. Montgomery $5E9B | 49151 (099%) | 06917 (138 s) | 11
  \ | C. G. Montgomery $5E9B | 30985 (063%) | 06917 (138 s) | first only
  \ | C. G. Montgomery $61BF | 49152 (100%) | 06916 (138 s) | 11
  \ | C. G. Montgomery $61BF | 31024 (063%) | 06916 (138 s) | first only
  \ | C. G. Montgomery $62DC | 49152 (100%) | 06917 (138 s) | 12
  \ | C. G. Montgomery $62DC | 30964 (063%) | 06916 (138 s) | first only
  \ | C. G. Montgomery $6363 | 49152 (100%) | 06917 (138 s) | 11
  \ | C. G. Montgomery $6363 | 30917 (062%) | 06917 (138 s) | first only
  \ | C. G. Montgomery $6594 | 49151 (099%) | 06917 (138 s) | 10
  \ | C. G. Montgomery $6594 | 31009 (063%) | 06916 (138 s) | first only
  \ | C. G. Montgomery $65E8 | 49152 (100%) | 06917 (138 s) | 12
  \ | C. G. Montgomery $65E8 | 31006 (063%) | 06917 (138 s) | first only
  \ | DX-Forth               | 49152 (100%) | 17733 (354 s) | 12
  \ | DX-Forth               | 31076 (063%) | 17734 (354 s) | first only
  \ | Gforth                 | 31189 (063%) | 09746 (194 s) | 1
  \ | J. E. Rickenbacker     | 08149 (016%) | 18458 (369 s) | 1
  \ | J. M. Lazo             | 12637 (025%) | 03349 (066 s) | 1
  \ | Leo Brodie             | 20818 (042%) | 09150 (183 s) | 1
  \ | lina                   | 23945 (048%) | 09179 (183 s) | 1
  \ | Milos Bazelides        | 28465 (057%) | 03316 (066 s) | 1
  \ | Spectrum Forth-83      | 05194 (010%) | 08741 (174 s) | 1
  \ | Tetris for terminals   | 02038 (004%) | 14200 (284 s) | 1
  \ | vForth                 | 27448 (055%) | 14806 (296 s) | 10
  \ | vForth                 | 20804 (042%) | 14806 (296 s) | first only
  \ | Z88 CamelForth         | 05496 (011%) | 15683 (313 s) | 1
  \ | Z80 Heaven             | 32599 (066%) | 03371 (067 s) | 1

  \ 2016-03-31
  \ Results of the 8-bit version of `rnd-pix-bench`:

  \ | Code | Pixels | Time per cycle in frames (and seconds) | Cycles

  \ | Joe Wingbermuehle      | 49145 (099%) | 01076 (021 s) | 29
  \ | Joe Wingbermuehle      | 25234 (051%) | 01075 (021 s) | first only
  \ | Milos Bazelides 1      | 00096 (000%) | 01047 (020 s) | 1
  \ | Milos Bazelides 2      | 00096 (000%) | 01048 (021 s) | 1
  \ | Z80 Heaven             | 00096 (000%) | 01055 (021 s) | 1

  \ 2016-04-07
  \ Results of the 16-bit version of `rnd-pix-bench`,
  \ with `os-seed` initialized to zero every time, except when
  \ the implementation needs a non-zero value:

  \ | Code | Pixels | Time per cycle in frames (and seconds) | Cycles

  \ | Jupiter ACE manual     | 02272 (004%) | 07635 (152 s) | 1
  \ | C. G. Montgomery $5E9B | 49151 (099%) | 06899 (137 s) | 11
  \ | C. G. Montgomery $5E9B | 31064 (063%) | 06900 (138 s) | first only
  \ | C. G. Montgomery $61BF | 49152 (100%) | 06899 (137 s) | 12
  \ | C. G. Montgomery $61BF | 30983 (063%) | 06899 (137 s) | first only
  \ | C. G. Montgomery $62DC | 49152 (100%) | 06900 (138 s) | 10
  \ | C. G. Montgomery $62DC | 31054 (063%) | 06899 (137 s) | first only
  \ | C. G. Montgomery $6363 | 49152 (100%) | 06899 (137 s) | 10
  \ | C. G. Montgomery $6363 | 31063 (063%) | 06899 (137 s) | first only
  \ | C. G. Montgomery $6594 | 49152 (100%) | 06900 (138 s) | 11
  \ | C. G. Montgomery $6594 | 31084 (063%) | 06899 (137 s) | first only
  \ | C. G. Montgomery $65E8 | 49151 (099%) | 06900 (138 s) | 11
  \ | C. G. Montgomery $65E8 | 31123 (063%) | 06899 (137 s) | first only
  \ | DX-Forth               | 49152 (100%) | 17717 (354 s) | 11
  \ | DX-Forth               | 31031 (063%) | 17718 (354 s) | first only
  \ | Gforth                 | 31104 (063%) | 09725 (194 s) | 1
  \ | J. E. Rickenbacker     | 08192 (016%) | 18460 (369 s) | 1
  \ | J. M. Lazo             | 12608 (025%) | 03325 (066 s) | 1
  \ | Leo Brodie             | 20928 (042%) | 09131 (182 s) | 1
  \ | lina                   | 24064 (048%) | 09161 (183 s) | 1
  \ | Milos Bazelides        | 28715 (058%) | 03297 (065 s) | 1
  \ | Spectrum Forth-83      | 05120 (010%) | 08723 (174 s) | 1
  \ | Tetris for terminals   | 00096 (000%) | 14210 (284 s) | 1
  \ | vForth                 | 29248 (059%) | 14807 (296 s) | 9
  \ | vForth                 | 21694 (044%) | 14806 (296 s) | first only
  \ | Z88 CamelForth         | 00384 (000%) | 15683 (313 s) | 1
  \ | Z80 Heaven             | 32768 (066%) | 03352 (067 s) | 1

  \ 2016-04-08
  \ Results of the 8-bit version of `rnd-pix-bench`:

  \ | Code | Pixels | Time per cycle in frames (and seconds) | Cycles

  \ | Joe Wingbermuehle      | 49140 (099%) | 01075 (021 s) | 27
  \ | Joe Wingbermuehle      | 25308 (051%) | 01075 (021 s) | first only
  \ | libzx                  | 25650 (052%) | 01389 (027 s) | 2
  \ | libzx                  | 25380 (051%) | 01390 (027 s) | first only
  \ | libzx (opt1)           | 25650 (052%) | 01389 (027 s) | 2
  \ | libzx (opt2)           | 25784 (052%) | 01382 (027 s) | 2
  \ | libzx (opt2)           | 25650 (052%) | 01382 (027 s) | first only
  \ | Milos Bazelides 1      | 00096 (000%) | 01052 (021 s) | 1
  \ | Milos Bazelides 2      | 00096 (000%) | 01055 (021 s) | 1
  \ | Z80 Heaven             | 00096 (000%) | 01061 (021 s) | 1


  \ vim: filetype=soloforth