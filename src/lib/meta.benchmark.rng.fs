  \ meta.benchmark.rng.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201707282344
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ RNG benchmarks written during the development of Solo Forth
  \ in order to choose the best implementations.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

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
  \ Display _u_ sampled numbers as a percentage.

: .sampled ( u -- ) dup u. ." (" sampled%. ." )" ;
  \ Display _u_ as the number of sampled numbers.

: report ( ca len -- ) type sampled .sampled cr ;

: rnd-bench ( ca len xt -- )
  -sample
  #sampled 0 ?do  dup execute remember  loop  drop
  report ;

( rng-px-bench )

  \ RNG pixel bench basic tools, common to 16-bit and 8-bit.

need set-pixel need bench{ need fast-pixels need u% need 3dup
need 2rdrop need display>tape-file

256 192 * constant #pixels
  \ Number of pixels of the screen.

defer rng ( n -- 0..n-1 )

: pixels%. ( u -- ) #pixels u% 0.r ." %" ;
  \ Display _u_ pixels as a percentage of the maximum number of
  \ pixels.

: .pixels ( u -- ) dup u. ." pixels (" pixels%. ." )" ;
  \ Display _u_ as the number of pixels.

: .title ( ca len -- ) ." Code: " type ;

variable cycles

: one-cycle? ( -- f ) cycles @ 1 = ;

defer .cycles ( -- )

: ?cycles ( len -- len | len-1 ) cycles @ 1 = + ;
  \ If the contents of `cycles` is not zero, decrement _len_.

: (.cycles) ( -- ) cycles ?  s" cycles" ?cycles type ;
  \ Display the number of cycles.

: .time ( d -- ) bench. ." per cycle" cr ;

: .result ( ca len d -- ) 2>r pixels >r  .title cr  r> .pixels
                          cr 2r> .time .cycles ; -->
  \ Calculate and display the result of the benchmark.
  \ _d_ is the time in frames; _ca len_ is the title.

( rng-px-bench )

defer random-coords ( -- gx gy )
  \ Random graphic coordinates. Configurable depending on the
  \ type of `random` to benchmark.

: fill-screen ( -- )
  #pixels 0 ?do random-coords set-pixel loop ;
  \ Fill the screen with random pixels.

: signal ( -- ) cycles @ %111 and border ;
  \ Change the border color according to the current count
  \ of cycles, just to show that the benchmark is running.

: (rnd-px-bench) ( -- d )
  1 cycles +!  signal  bench{ fill-screen }bench ;
  \ Do one cycle of the benchmark and return its result.

: save-result ( -- ) s" rng-px-bench" display>tape-file ;

: finish ( ca len d -- ) 0 border .result save-result ;
  \ Finish the benchmark.
  \ _d_ is the time in frames; _ca len_ is the title.

: init ( xt1 xt2 -- )
  ['] .cycles defer! ['] rng defer! page -1 cycles ! ;

defer finish? ( i*x -- j*x f )
  \ Finish the benchmark?

: new-pixels? ( n1 -- n2 f ) pixels tuck = ;
  \ Are there new pixels on the screen, comparing the previous
  \ count _n1_ with the new count _n2_?

' new-pixels? ' finish? defer!  -->

( rng-px-bench )

defer rng-px-bench ( ca len xt -- )
  \ Do a RNG benchmark for the `random` word _xt_ with title
  \ _ca len_.

: (multi-cycle-rnd-px-bench) ( ca len -- )
  0 begin   (rnd-px-bench) 2>r
            finish? dup 0= if  2rdrop  then
  until     drop 2r> finish ;
  \ Do a multi-cycle RNG benchmark with title _ca len_:
  \ Do as many cycles (49152 random pixels) as needed until the
  \ number of pixels doesn't change.

defer multi-cycle ( -- )
  \ Set `rng-px-bench` to multi-cycle mode, either 16-bit or
  \ 8-bit.

: (.cycle) ( -- ) ." First cycle only" ;

defer single-cycle ( -- )
  \ Set `rng-px-bench` to single-cycle mode, either 16-bit or
  \ 8-bit.

-->

( rng-px-bench )

: do-rng-px-bench ( a -- )
  dup perform rot
  cell+ dup @ swap
  cell+ perform rng-px-bench ;
  \ Execute the RNG pixel benchmark whose description is stored
  \ at _a_, as described in `create-rng-px-bench`.

: main-rng-px-bench ( a -- ) multi-cycle do-rng-px-bench ;
  \ Execute the main phase of an RNG pixel benchmark, whose
  \ description is stored at _a_, as described in
  \ `create-rng-px-bench`.

: secondary-rng-px-bench ( a -- )
  one-cycle? if   drop
             else single-cycle do-rng-px-bench then ;
  \ If needed, execute the secondary phase of an RNG pixel
  \ benchmark, whose description is stored at _a_, as described
  \ in `create-rng-px-bench`.

: create-rng-px-bench ( xt1 xt2 xt3 "name" -- )
  create , , , ;
  \ Create an RNG pixel benchmark _name_ for the `random` word
  \ _xt2_, with initialization _xt1_ and title _ca len_.
  \ Cell offset    Description
  \ +0             xt3, which returns the title string
  \ +1             xt2, `random`
  \ +2             xt1, `random` init

: (does-rng-px-bench) ( a -- )
  dup main-rng-px-bench secondary-rng-px-bench ;
  \ Run-time action of a RNG pixel benchmark, whose
  \ data is stored at _a_.

  \ XXX TODO -- Improve: Do not run the benchmark twice when it
  \ needs more than one cycle to complete, in order to preserve
  \ the result of its first cycle. Instead, always save the
  \ result of the first cycle into a memory buffer, and use it
  \ at the end of the benchmark if needed.

( 16b-rng-px-bench )

  \ 16-bit RNG pixel bench tools.

need rng-px-bench

: 16b-random-coords ( -- gx gy ) 256 rng 192 rng ;
  \ Random graphic coordinates for 16-bit `rnd`.

: 16b-single-cycle-rng-px-bench ( ca len xt -- )
  ['] (.cycle) init (rnd-px-bench) finish ;
  \ Do a one-cycle 16-bit RNG benchmark for `random` word _xt_
  \ with title _ca len_: 49152 random pixels.

: 16b-multi-cycle-rng-px-bench ( ca len xt -- )
  ['] (.cycles) init (multi-cycle-rnd-px-bench) ;
  \ Do a 16-bit multi-cycle RNG benchmark for the `random` word
  \ _xt_ with title _ca len_: complete as many cycles (49152
  \ random pixels) as required until the number of pixels
  \ doesn't change.

-->

( 16b-rng-px-bench )

: 16b-single-cycle ( -- )
  ['] 16b-single-cycle-rng-px-bench ['] rng-px-bench defer! ;
  \ Set `rng-px-bench` to 16-bit single-cycle mode.

: 16b-multi-cycle ( -- )
  ['] 16b-multi-cycle-rng-px-bench ['] rng-px-bench defer! ;
  \ Set `rng-px-bench` to 16-bit multi-cycle mode.

: set-16b-rng-px-bench ( -- )
  ['] 16b-random-coords ['] random-coords defer!
  ['] 16b-single-cycle  ['] single-cycle  defer!
  ['] 16b-multi-cycle   ['] multi-cycle   defer! ;

: 16b-rng-px-bench ( xt1 xt2 xt3 "name" -- )
  create-rng-px-bench
  does> ( -- ) ( pfa )
        set-16b-rng-px-bench (does-rng-px-bench) ;
  \ Create a 16-bit RNG pixel benchmark _name_ for the `random`
  \ word _xt2_, with initialization _xt1_ and title string
  \ returned by _x3_.

( 8b-rng-px-bench )

  \ 8-bit RNG pixel bench tools.

need rng-px-bench

: 8b-random-coords ( -- gx gy ) rng rng 191 min ;
  \ Random graphic coordinates for 8-bit `rnd`.

: 8b-single-cycle-rng-px-bench ( ca len xt -- )
  ['] (.cycle) init (rnd-px-bench) finish ;
  \ Do a one-cycle 8-bit RNG benchmark for `random` word _xt_
  \ with title _ca len_: 49152 random pixels.

: 8b-multi-cycle-rng-px-bench ( ca len xt -- )
  ['] (.cycles) init (multi-cycle-rnd-px-bench) ;
  \ Do an 8-bit multi-cycle RNG benchmark for the `random` word
  \ _xt_ with title _ca len_: complete as many cycles (49152
  \ random pixels) as required until the number of pixels
  \ doesn't change.

-->

( 8b-rng-px-bench )


: 8b-single-cycle ( -- )
  ['] 8b-single-cycle-rng-px-bench ['] rng-px-bench defer! ;
  \ Set `rng-px-bench` to 8-bit single-cycle mode.

: 8b-multi-cycle ( -- )
  ['] 8b-multi-cycle-rng-px-bench ['] rng-px-bench defer! ;
  \ Set `rng-px-bench` to 8-bit multi-cycle mode.

: set-8b-rng-px-bench ( -- )
  ['] 8b-random-coords ['] random-coords defer!
  ['] 8b-single-cycle  ['] single-cycle  defer!
  ['] 8b-multi-cycle   ['] multi-cycle   defer! ;

: 8b-rng-px-bench ( xt1 xt2 xt3 "name" -- )
  create-rng-px-bench
  does> ( -- ) ( pfa )
        set-8b-rng-px-bench (does-rng-px-bench) ;
  \ Create an 8-bit RNG pixel benchmark _name_ for the `random`
  \ word _xt2_, with initialization _xt1_ and title string
  \ returned by _x3_.

( show-rng )

need tape-file>display

: show-rng ( -- )
  page ." Rewind tape; press 'q' to quit" key 'q' = ?exit
  begin s" " tape-file>display key 'q' = until ;
  \ Show the benchmark results, loading their screens from tape.

( rng-px-benchs-intro )

: rng-px-benchs-intro ( ca len -- )
  page type ." random pixels benchmarks" cr
  ." --------------------------------" cr cr

  \  <------------------------------>
  ." The benchmarks that need more" cr
  ." than one cycle to complete," cr
  ." will be executed a second time" cr
  ." in one-cycle mode." cr cr

  \  <------------------------------>
  ." All the result displays will be" cr
  ." saved to tape. Use 'show-rng'" cr
  ." to display them later." cr cr

  \  <------------------------------>
  ." The process can not be stopped." cr cr

  \  <------------------------------>
  ." Press 'q' to quit or any other" cr
  ." key to start." key page 'q' = if quit then ;
  \ Display the intro of the batch RNG pixel benchmarks.  _ca
  \ len_ is the start of the title: "8-bit" or "16-bit".

( 16b-rng-px-benchs )

  \ Execute all the 16-bit random pixels benchmarks

need rng-px-benchs-intro  s" 16-bit" rng-px-benchs-intro
need show-rng

need ace-rng-px-bench        ace-rng-px-bench
need cgm-5E9B-rng-px-bench   cgm-5E9B-rng-px-bench
need cgm-61BF-rng-px-bench   cgm-61BF-rng-px-bench
need cgm-62DC-rng-px-bench   cgm-62DC-rng-px-bench
need cgm-6363-rng-px-bench   cgm-6363-rng-px-bench
need cgm-6594-rng-px-bench   cgm-6594-rng-px-bench
need cgm-65E8-rng-px-bench   cgm-65E8-rng-px-bench
need dx-rng-px-bench         dx-rng-px-bench
need gf-rng-px-bench         gf-rng-px-bench
need jer-rng-px-bench        jer-rng-px-bench
need jml-rng-px-bench        jml-rng-px-bench
need lb-rng-px-bench         lb-rng-px-bench
need lina-rng-px-bench       lina-rng-px-bench -->

( 16b-rng-px-benchs )

need mb-rng-px-bench         mb-rng-px-bench
need sf83-rng-px-bench       sf83-rng-px-bench
need tt-rng-px-bench         tt-rng-px-bench
need vf-rng-px-bench         vf-rng-px-bench
need z88-rng-px-bench        z88-rng-px-bench
need zh-rng-px-bench         zh-rng-px-bench
need xorshift-rng-px-bench   xorshift-rng-px-bench

( ace-random ace-rng-px-bench )

  \ Credit:
  \
  \ Adapted from ACE Forth, after the Jupiter ACE manual.
  \ Also used by Abersoft Forth in its bundled game
  \ "Bertie".

need os-seed

: ace-rnd ( -- u )
  os-seed @ 75 um* 75. d+ 2dup u< - - 1- dup os-seed ! ;

: ace-random ( n -- 0..n-1 ) ace-rnd um* nip ;

need 16b-rng-px-bench need :noname

:noname ( -- ) os-seed off ;
' ace-random
:noname ( -- ca len ) s" Jupiter ACE manual" ;
16b-rng-px-bench ace-rng-px-bench ( -- )

( cgm-5E9B-random cgm-5E9B-rng-px-bench )

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

need 16b-rng-px-bench need :noname

' noop
' cgm-5E9B-random
:noname ( -- ca len ) s" C. G. Montgomery $5E9B" ;
16b-rng-px-bench cgm-5E9B-rng-px-bench ( -- )

( cgm-61BF-random cgm-61BF-rng-px-bench )

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

need 16b-rng-px-bench need :noname

' noop
' cgm-61BF-random
:noname ( -- ca len ) s" C. G. Montgomery $61BF" ;
16b-rng-px-bench cgm-61BF-rng-px-bench ( -- )

( cgm-62DC-random cgm-62DC-rng-px-bench )

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

need 16b-rng-px-bench need :noname

' noop
' cgm-62DC-random
:noname ( -- ca len ) s" C. G. Montgomery $62DC" ;
16b-rng-px-bench cgm-62DC-rng-px-bench ( -- )

( cgm-6363-random cgm-6363-rng-px-bench )

  \ Random Number Generator by C. G. Montgomery

  \ 2015-12-13: found here:
  \ http://web.archive.org/web/20060707001752/http://www.tinyboot.com/index.html
  \
  \ 2016-03-31: adapted to Solo Forth.

2variable rloc  $111 rloc !  \ seed with nonzero

: cgm-6363-rnd ( -- u )
  rloc 2@ $6363 um* rot 0 d+ over rloc 2! ;
  \ good values for 16-bit systems: 61BF 62DC 6594 6363 5E9B 65E8

: cgm-6363-random ( n -- 0..n-1 " cgm-6363-rnd um* nip ;

need 16b-rng-px-bench need :noname

' noop
' cgm-6363-random
:noname ( -- ca len ) s" C. G. Montgomery $6363" ;
16b-rng-px-bench cgm-6363-rng-px-bench ( -- )

( cgm-6594-random cgm-6594-rng-px-bench )

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

need 16b-rng-px-bench need :noname

' noop
' cgm-6594-random
:noname ( -- ca len ) s" C. G. Montgomery $6594" ;
16b-rng-px-bench cgm-6594-rng-px-bench ( -- )

( cgm-65E8-random cgm-65E8-rng-px-bench )

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

need 16b-rng-px-bench need :noname

' noop
' cgm-65E8-random
:noname ( -- ca len ) s" C. G. Montgomery $65E8" ;
16b-rng-px-bench cgm-65E8-rng-px-bench ( -- )

( dx-random dx-rng-px-bench )

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

need 16b-rng-px-bench need :noname

' noop
' dx-random
:noname ( -- ca len ) s" DX-Forth" ;
16b-rng-px-bench dx-rng-px-bench ( -- )

( gf-random gf-rng-px-bench )

  \ Credit:
  \
  \ Adapted from Gforth.

need os-seed need ud*

: gf-rnd ( -- n )
  272958469. os-seed @ ud* d>s 1+ dup os-seed ! ;

: gf-random ( n -- 0..n-1 ) gf-rnd um* nip ;

need 16b-rng-px-bench need :noname

:noname ( -- ) os-seed off ;
' gf-random
:noname ( -- ca len ) s" Gforth" ;
16b-rng-px-bench gf-rng-px-bench ( -- )

( jer-random jer-rng-px-bench )

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

need 16b-rng-px-bench need :noname

:noname ( -- ) os-seed off ;
' jer-random
:noname ( -- ca len ) s" J. E. Rickenbacker" ;
16b-rng-px-bench jer-rng-px-bench ( -- )

( jml-random jml-rng-px-bench )

  \ Credit:
  \
  \ Adapted from code written by José Manuel Lazo,
  \ published on Microhobby, issue 85 (1986-07), page 24:
  \ http://microhobby.org/numero085.htm
  \ http://microhobby.speccy.cz/mhf/085/MH085_24.jpg

need assembler need os-seed

code jml-rnd ( -- u )

  os-seed fthl, h push,
  h addp, h addp, h addp, h addp, h addp, h addp,
  d pop, d addp, 0029 d ldp#, d addp,
  os-seed sthl, h push,
  jpnext,
  end-code

: jml-random ( n -- 0..n-1 ) jml-rnd um* nip ;

need 16b-rng-px-bench need :noname

:noname ( -- ) os-seed off ;
' jml-random
:noname ( -- ca len ) s" J. M. Lazo" ;
16b-rng-px-bench jml-rng-px-bench ( -- )

( lb-random lb-rng-px-bench )

  \ Credit:
  \
  \ Code adapted from Leo Brodie's _Starting Forth_.

need os-seed

: lb-rnd ( -- u ) os-seed @ 31421 * 6927 + dup os-seed ! ;

: lb-random ( n -- 0..n-1 ) lb-rnd um* nip ;

need 16b-rng-px-bench need :noname

:noname ( -- ) os-seed off ;
' lb-random
:noname ( -- ca len ) s" Leo Brodie" ;
16b-rng-px-bench lb-rng-px-bench ( -- )

( lina-random lina-rng-px-bench )

need os-seed

: lina-rnd ( -- n )
  os-seed @ 107465 * 234567 + dup os-seed ! ;

: lina-random ( n -- 0..n-1 ) lina-rnd um* nip ;

need 16b-rng-px-bench need :noname

:noname ( -- ) os-seed off ;
' lina-random
:noname ( -- ca len ) s" lina" ;
16b-rng-px-bench lina-rng-px-bench ( -- )

( mb-random mb-rng-px-bench )

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
  os-seed sthl, h push, jpnext, end-code

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

need 16b-rng-px-bench need :noname

:noname ( -- ) os-seed off ;
' mb-random
:noname ( -- ca len ) s" Milos Bazelides" ;
16b-rng-px-bench mb-rng-px-bench ( -- )

( mm-random mm-rng-px-bench )

  \ Credit:
  \ IsForth Random Number Generator, by Mark I Manning IV.

  \ 2016-04-04: Adapted to Solo Forth. Strange negative
  \ results.
  \ XXX FIXME --
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

need 16b-rng-px-bench need :noname

' noop
' mm-random
:noname ( -- ca len ) s" IsForth" ;
16b-rng-px-bench mm-rng-px-bench ( -- )

( sf83-random sf83-rng-px-bench )

  \ Credit:
  \
  \ Code from Spectrum Forth-83.

need os-seed  3 os-seed !

: sf83-random ( n -- 0..n-1 )
  os-seed @ 743 * 43 + dup os-seed ! um* swap drop ;

need 16b-rng-px-bench need :noname

' noop
' sf83-random
:noname ( -- ca len ) s" Spectrum Forth-83" ;
16b-rng-px-bench sf83-rng-px-bench ( -- )

( tt-random tt-rng-px-bench )

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

need 16b-rng-px-bench need :noname

:noname ( -- ) os-seed on ;
' tt-random
:noname ( -- ca len ) s" Tetris for terminals" ;
16b-rng-px-bench tt-rng-px-bench

( vf-random vf-rng-px-bench )

  \ Credit:
  \
  \ Code from vForth.

need os-frames

: vf-random ( n -- 0..n-1 )
  1+ 8195 os-frames @ um* 1. d+
  16383 um/mod drop
  \ dup os-seed !
  swap mod ;

need 16b-rng-px-bench need :noname

' noop
' vf-random
:noname ( -- ca len ) s" vForth" ;
16b-rng-px-bench vf-rng-px-bench ( -- )

( z88-random z88-rng-px-bench )

  \ Credit:
  \
  \ Code adapted from Z88 CamelForth.

need ud* need os-seed need 2rdrop

: z88-random ( n -- 0..n-1 )
  1103515245. \ 20077 16838
  os-seed @ ud* 12345. d+ over os-seed !
  rot ud/mod 2drop ;

need 16b-rng-px-bench need :noname

:noname ( -- ) os-seed off ;
' z88-random
:noname ( -- ca len ) s" Z88 CamelForth" ;
16b-rng-px-bench z88-rng-px-bench ( -- )

( zh-random zh-rng-px-bench )

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
  h inc, h incp, os-seed sthl, h push, jpnext, end-code
    \ inc h
    \ inc hl
    \ ld (seed),hl
    \ push hl
    \ _jp_next

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

need 16b-rng-px-bench need :noname

:noname ( -- ) os-seed off ;
' zh-random
:noname ( -- ca len ) s" Z80 Heaven" ;
16b-rng-px-bench zh-rng-px-bench ( -- )

( lcm-random lcm-rng-px-bench )

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

need 16b-rng-px-bench need :noname

' noop
' lcm-random
:noname ( -- ca len ) s" LCM" ;
16b-rng-px-bench lcm-rng-px-bench ( -- )

( xorshift-random xorshift-rng-px-bench )

  \ Xorshift is a simple, fast pseudorandom number generator
  \ developed by George Marsaglia. The generator combines
  \ three xorshift operations where a number is exclusive-ored
  \ with a shifted copy of itself.

  \ Credit:
  \
  \ Original Z80 code by John Metcalf
  \ http://www.retroprogramming.com/2017/07/xorshift-pseudorandom-numbers-in-z80.html

need os-seed need assembler

1 os-seed ! \ seed must not be 0

code xorshift-rnd ( -- x )
  os-seed h ftp,
    \ ld hl,(seed)
  h a ld, rra, l a ld, rra, h xor, a h ld, l a ld, rra, h a ld,
    \ ld a,h
    \ rra
    \ ld a,l
    \ rra
    \ xor h
    \ ld h,a
    \ ld a,l
    \ rra
    \ ld a,h
  rra, l xor, a l ld, h xor, a h ld, os-seed h stp, h push,
    \ rra
    \ xor l
    \ ld l,a
    \ xor h
    \ ld h,a
    \ ld (xrnd+1),hl
    \ push hl
  jpnext, end-code

: xorshift-random ( n -- 0..n-1 ) xorshift-rnd um* nip ;

need 16b-rng-px-bench need :noname

' noop
' xorshift-random
:noname ( -- ca len ) s" xorshift" ;
16b-rng-px-bench xorshift-rng-px-bench ( -- )

( 8b-rng-px-benchs )

  \ Execute all the 8-bit random pixels benchmarks

  \ XXX TODO -- check the libzx rng benchs

need rng-px-benchs-intro  s" 8-bit" rng-px-benchs-intro
need show-rng

need jw-rng-px-bench       jw-rng-px-bench
need mb1-rng-px-bench      mb1-rng-px-bench
need mb2-rng-px-bench      mb2-rng-px-bench
need zh-rng-px-bench       zh-rng-px-bench
need libzx-rng-px-bench    libzx-rng-px-bench
need libzx-1-rng-px-bench  libzx-1-rng-px-bench
need libzx-2-rng-px-bench  libzx-2-rng-px-bench
need libzx-3-rng-px-bench  libzx-3-rng-px-bench
need r-rng-px-bench        r-rng-px-bench

( r-crnd r-rng-px-bench )

code r-crnd ( -- b )
  ED c, 5F c,     \ ld a,r
  C3 c, pusha ,   \ jp pusha
  end-code

need 8b-rng-px-bench need :noname

:noname ( -- ) os-seed off ;
' r-crnd
:noname ( -- ca len ) s" R register (8 bit)" ;
8b-rng-px-bench r-rng-px-bench ( -- )

( jw-crnd jw-rng-px-bench )

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

need 8b-rng-px-bench need :noname

:noname ( -- ) os-seed off ;
' jw-crnd
:noname ( -- ca len ) s" Joe Wingbermuehle" ;
8b-rng-px-bench jw-rng-px-bench ( -- )

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

( mb1-crnd mb1-rng-px-bench )

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

need 8b-rng-px-bench need :noname

:noname ( -- ) os-seed off ;
' mb1-crnd
:noname ( -- ca len ) s" Milos Bazelides 1 (8 bit)" ;
8b-rng-px-bench mb1-rng-px-bench ( -- )

  \ : mb1-rng-px-bench ( -- )
  \   s" Milos Bazelides 1" ['] mb1-crandom rng-px-bench ;

( mb2-crnd mb2-rng-px-bench )

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

need 8b-rng-px-bench need :noname

:noname ( -- ) os-seed off ;
' mb2-crnd
:noname ( -- ca len ) s" Milos Bazelides 2 (8 bit)" ;
8b-rng-px-bench mb2-rng-px-bench ( -- )

  \ : mb2-rng-px-bench ( -- )
  \   s" Milos Bazelides 2" ['] mb2-crandom rng-px-bench ;
  \   XXX OLD

( zh-crnd zx-rgn-px-bench )

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

need 8b-rng-px-bench need :noname

' noop
' zh-crnd
:noname ( -- ca len ) s" Z80 Heaven (8 bit)" ;
8b-rng-px-bench zh-rng-px-bench ( -- )

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

( libzx-3-crnd libzx-3-rng-px-bench )

  \ Credit:
  \ Original code from the ZX Spectrum libzx library,
  \ written by Sebastian Mihai, 2016
  \ http://sebastianmihai.com/main.php?t=131

  \ 2016-04-09: Adapted to Solo Forth. Optimized and modified
  \ the original code.

need assembler need os-seed

variable rom-pointer  rom-pointer off  os-seed off

code libzx-3-crnd ( -- b )

  \ Get an 8-bit random number.
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

need 8b-rng-px-bench need :noname

:noname ( -- ) rom-pointer off  os-seed off ;
' libzx-3-crnd
:noname ( -- ca len ) s" libzx opt3 (8 bit)" ;
8b-rng-px-bench libzx-3-rng-px-bench ( -- )

( libzx-2-crnd libzx-2-rng-px-bench )

  \ Credit:
  \ Original code from the ZX Spectrum libzx library,
  \ written by Sebastian Mihai, 2016
  \ http://sebastianmihai.com/main.php?t=131

  \ 2016-04-09: Adapted to Solo Forth. Optimized and modified
  \ the original code.

need assembler need os-seed

variable rom-pointer  rom-pointer off  os-seed off

code libzx-2-crnd ( -- b )

  \ Get an 8-bit random number.
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

  rom-pointer h ldp#, m add, os-seed sta,
    \ ld hl, romPointer
    \ add a, (hl) ; the contents of the ROM are "pretty random"
    \ ; so add it in the mix
    \ ld (lastRandomNumber), a ; save this number

  b pop, pusha jp, end-code

need 8b-rng-px-bench need :noname

:noname ( -- ) rom-pointer off  os-seed off ;
' libzx-2-crnd
:noname ( -- ca len ) s" libzx opt2 (8 bit)" ;
8b-rng-px-bench libzx-2-rng-px-bench ( -- )

( libzx-1-crnd libzx-1-rng-px-bench )

  \ Credit:
  \ Original code from the ZX Spectrum libzx library,
  \ written by Sebastian Mihai, 2016
  \ http://sebastianmihai.com/main.php?t=131

  \ 2016-04-09: Adapted to Solo Forth. Optimized the original
  \ code.

need assembler need os-seed

variable rom-pointer  3 rom-pointer !  33 os-seed c!

code libzx-1-crnd ( -- b )

  \ Get an 8-bit random number.
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

  rom-pointer h ldp#, m add, os-seed sta,
    \ ld hl, romPointer
    \ add a, (hl) ; the contents of the ROM are "pretty random"
    \ ; so add it in the mix
    \ ld (lastRandomNumber), a ; save this number

  b pop, pusha jp, end-code

need 8b-rng-px-bench need :noname

:noname ( -- ) 3 rom-pointer ! 33 os-seed c! ;
' libzx-1-crnd
:noname ( -- ca len ) s" libzx opt1 (8 bit)" ;
8b-rng-px-bench libzx-1-rng-px-bench ( -- )

( libzx-crnd libzx-rng-px-bench )

  \ Credit:
  \ Original code from the ZX Spectrum libzx library,
  \ written by Sebastian Mihai, 2016
  \ http://sebastianmihai.com/main.php?t=131

  \ 2016-04-09: Adapted to Solo Forth.

need assembler need os-seed

variable rom-pointer  3 rom-pointer !  33 os-seed c!

code libzx-crnd ( -- b )

  \ Get an 8-bit random number.
  \ It is computed using a combination of:
  \     - the last returned random number
  \     - a byte from ROM, in increasing order
  \     - current values of various registers
  \     - a flat incremented value

  b push, af push,
    \ save Forth IP and the AF register

  \ 1) advance ROM pointer

  rom-pointer h ldp#, m c ld, h incp, m b ld, 3 h ldp#, b addp,
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

  rom-pointer h ldp#, m add, os-seed h ldp#, m a ld,
    \ ld hl, romPointer
    \ add a, (hl) ; the contents of the ROM are "pretty random"
    \ ; so add it in the mix
    \ ld hl, lastRandomNumber
    \ ld (hl), a ; save this number
    \ XXX REMARK -- original code is not optimized

  b pop, 0 h ld#, a l ld, h push, jpnext, end-code

need 8b-rng-px-bench need :noname

:noname ( -- ) 3 rom-pointer ! 33 os-seed c! ;
' libzx-crnd
:noname ( -- ca len ) s" libzx (8 bit)" ;
8b-rng-px-bench libzx-rng-px-bench ( -- )

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


  \ ===========================================================
  \ Change log

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
  \
  \ 2017-05-07: Improve documentation.
  \
  \ 2017-05-09: Remove `jppushhl,`.
  \
  \ 2017-07-26: Add `xorshift-random`. Improve the way 16-bit
  \ benchs are loaded. Improve documentation. Save the result
  \ screens to tape.
  \
  \ 2017-07-27: Improve loading of 16-bit benchs. Add URL to
  \ the credits of the libzx library.
  \
  \ 2017-07-28: Write a definer of 16-bit benchmarks, in order
  \ to make their secondary phase automatic. Shorten "-pix-" to
  \ "-px-", "16-bit-" to "16-bit-" and "8-bit-" to "8b-" in
  \ word names, except old comments. Write a definer of 8-bit
  \ benchmarks. Rename and rewrite the R-register benchmark.

  \ vim: filetype=soloforth
