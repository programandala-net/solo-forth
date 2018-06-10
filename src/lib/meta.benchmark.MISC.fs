  \ meta.benchmark.MISC.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201806101707
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Misc benchmarks written during the development of Solo
  \ Forth in order to choose from different implementation
  \ options.
  \
  \ Unless otherwise stated, benchmark results were obtained on
  \ a ZX Spectrum 128 with G+DOS, emulated by Fuse.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( located-bench )

need ticks need timer

0 32 2constant missing$ ( -- ca len )
  \ A fake word that does not exist: the first 32 B of the
  \ ROM...

: run ( n -- )
  >r
  cr ." 1-line-(located    "
  ticks r@ 0 ?do missing$ 1-line-(located    drop loop timer
  cr ." multiline-(located "
  ticks r@ 0 ?do missing$ multiline-(located drop loop timer
  r> drop ;

  \                   Ticks
  \                   -------------------
  \ Date       Times  1-line    multiline Note
  \ ---------- -----  --------  --------- -------------------
  \ 2018-06-03     1      1716          ? Multiline freezes

( block-bench )

  \

  \ ////////////////////////////////////////////////////////////////

  \ // XXX OLD

  \ In order to compare the block access speed on every platform, the
  \ following code was executed with disk 1 (library) in the first drive
  \ and disk 2 (programs) in the second drive:

  \ ---
  \ 1 load need dtimer
  \ dticks need 2-block-drives need edit-sound dtimer
  \ ---

  \ The test was run several times on every platform, and the fastest
  \ result was noted.

  \ Final results as of 2018-04-09, from fastest to slowest:

  \ // XXX REMARK -- 2018-04-10: The results are wrong, because TR-DOS is
  \ // actually the slowest system. It seems the ticks clock is not
  \ // properly updated by TR-DOS. The test has to be repeated with a real
  \ // clock.

  \ |===
  \ | Computer        | Interface| DOS    | Ticks

  \ | Pentagon 128    |          | TR-DOS | 11056
  \ | Pentagon 1024   |          | TR-DOS | 11480
  \ | Pentagon 512    |          | TR-DOS | 11526
  \ | Scorpion ZS 256 |          | TR-DOS | 12395
  \ | ZX Spectrum 128 | Beta 128 | TR-DOS | 14754
  \ | ZX Spectrum +2  | Beta 128 | TR-DOS | 14774
  \ | ZX Spectrum +2  | Plus D   | G+DOS  | 19018
  \ | ZX Spectrum 128 | Plus D   | G+DOS  | 19778
  \ | ZX Spectrum +3e |          | +3DOS  | 24347
  \ | ZX Spectrum +3  |          | +3DOS  | 24505
  \ |===

  \ ////////////////////////////////////////////////////////////////

  \ The following table compares the block access speed on different
  \ platforms.

  \ The ticks clock can not be used for the tests, because it's not
  \ regulary updated by the DOS during disk operations. A chronometer
  \ watch was used instead.  The following line of code was executed with
  \ disk 1 (library) in the first drive and disk 2 (programs) in the
  \ second drive:

  \ ----
  \ 1 load 4 border need 2-block-drives need edit-sound 2 border
  \ ----

  \ The tests were done running Solo Forth on the
  \ http://fuse-emulator.sourceforge.net[Fuse emulator], on a Raspberry Pi
  \ 2 with Raspbian, at c. 700% the speed of the original machines.

  \ 2018-04-10:

  \ .Block access speed test in seconds
  \ |===
  \ | Computer        | Interface| DOS    | Seconds

  \ | ZX Spectrum +3  |          | +3DOS  | 100
  \ | ZX Spectrum +3e |          | +3DOS  | 101
  \ | ZX Spectrum +2  | Plus D   | G+DOS  | 119
  \ | ZX Spectrum 128 | Plus D   | G+DOS  | 121
  \ | Scorpion ZS 256 |          | TR-DOS | 506
  \ | ZX Spectrum 128 | Beta 128 | TR-DOS | 510
  \ | ZX Spectrum +2  | Beta 128 | TR-DOS | 515
  \ | Pentagon 128    |          | TR-DOS | 519
  \ | Pentagon 1024   |          | TR-DOS | 523
  \ | Pentagon 512    |          | TR-DOS | 528
  \ |===

( >in-bench )

need ticks need timer

: in>l1 >in @ dup c/l mod - ;
: in>l2 >in @ c/l / c/l * ;

: run ( n -- )
  >r
  cr ." in>l1 " ticks r@ 0 ?do in>l1 drop        loop timer
  cr ." in>l2 " ticks r@ 0 ?do in>l2 drop        loop timer
  cr ." >in @ " ticks r@ 0 ?do >in @ >in @ 2drop loop timer
  cr ." dup   " ticks r@ 0 ?do >in @ dup   2drop loop timer
  r> drop ;

  \ 2018-03-11:

  \        Ticks
  \        ---------------------------
  \ Times  in>l1  in>l2
  \ -----  -----  -----
  \   100      9     13
  \  1000     90    128
  \ 10000    897   1280

  \        Ticks
  \        ---------------------------
  \ Times  >in @  dup
  \ -----  -----  ---
  \   100      1    1
  \  1000     12    9
  \ 10000    113   90

( cells-+-bench )

need ticks need timer need array> need array<

: run ( n -- )
  >r
  cr ." cells +     "
  ticks r@ 0 ?do 0 1 cells +     drop loop timer
  cr ." swap array> "
  ticks r@ 0 ?do 0 1 swap array> drop loop timer
  cr ." swap array< "
  ticks r> 0 ?do 0 1      array< drop loop timer ;

  \ 2018-02-20:

  \        Ticks
  \        ---------------------------
  \ Times  cells +  swap array> array<
  \ -----  -------  ----------- ------
  \   100        1            1      0
  \  1000        9            9      7
  \ 10000       90           91     75
  \ 20000      180          182    150
  \ 65535      589          597    491

( 0>-bench )

need ticks need timer

: run ( n -- )
  >r
  cr ." 0< "
  ticks r@ 0 ?do 1 0< drop -1 0< drop loop timer
  cr ." 0> "
  ticks r> 0 ?do 1 0> drop -1 0> drop loop timer ;

  \ 2017-12-31:

  \        Ticks
  \        ----------------------
  \ Times       0<             0>
  \ -----  ------- --------------
  \   100        1       2
  \  1000       10      12
  \ 10000      103     120 (1.16)
  \ 20000      206     239 (1.16)
  \ 65535      677     784 (1.15)

( past?-bench )

need dnegate need ticks need dticks need d0< need du<
need timer

: dpast1? ( d -- f ) dnegate dticks d+ d0< 0= ;

: dpast2? ( d -- f ) dticks du< ;

: past1? ( n -- f ) ticks swap - 0< 0= ;

: past2? ( n -- f ) ticks u< ;

: run ( n -- )
  >r cr ." dpast1? " ticks r@ 0 ?do 0. dpast1? drop loop timer
     cr ." dpast2? " ticks r@ 0 ?do 0. dpast2? drop loop timer
     cr ." past1?  " ticks r@ 0 ?do 0  past1?  drop loop timer
     cr ." past2?  " ticks r> 0 ?do 0  past2?  drop loop timer
     ;

  \ 2017-12-12:

  \        Ticks
  \        -----------------------------
  \ Times  dpast1? dpast2? past1? past2?
  \ -----  ------- ------- ------ ------
  \   100        4       3      2      2
  \  1000       33      35     22     17
  \ 10000      332     350    218    172
  \ 20000      664     700    430    345
  \ 65535     2176    2292   1432   1131

( negate-+-bench )

  \ 2017-12-12

need ticks need timer

: run ( n -- )
  >r
  cr ." negate + "
  ticks r@ 0 ?do 0 negate 0 + drop loop timer
  cr ." swap - "
  ticks r> 0 ?do 0 0 swap -   drop loop timer ;

  \        Ticks
  \        ------------------
  \ Times  negate +  swap -
  \ -----  --------  --------
  \ 10000        93        91
  \ 20000       186       182
  \ 65535       609       597

( dnegate-d+-bench )

  \ 2017-12-12

need d- need dticks need dtimer

: run ( n -- )
  >r
  cr ." dnegate d+ "
  dticks r@ 0 ?do 0. dnegate 0. d+ 2drop loop dtimer
  cr ." 2swap d- "
  dticks r> 0 ?do 0. 0. 2swap d-   2drop loop dtimer ;

  \        Ticks
  \        ------------------
  \ Times  dnegate d+  2swap d-
  \ -----  ----------  --------
  \ 10000         136       881
  \ 65535         135       889

( s>d-bench )

need dticks need dtimer

: s>d-bench ( u -- )
  dticks rot 0 ?do   0 s>d            2drop loop dtimer ;

: 2literal-bench ( u -- )
  dticks rot 0 ?do [ 0 s>d ] 2literal 2drop loop dtimer ;

: run ( u -- )
  dup cr
      ." s>d              " s>d-bench      cr
      ." [ s>d ] 2literal " 2literal-bench cr ;

  \ Date        Times Ticks (20 ms)
  \ ----------  ----- ------------------
  \                         s>d 2literal
  \                   --------- --------
  \ 2017-12-04   1000         7        6
  \             10000        68       61
  \             65535       442      402

( times-bench )

need bench{ need }bench.  variable times-xt

: times-v1 ( u -- )
  rp@ @ dup cell+ rp@ ! @ times-xt !
  0 ?do times-xt perform loop ; compile-only ?)

: times-v2 ( u -- )
  rp@ @ dup cell+ rp@ ! @ swap
  0 ?do dup execute loop drop ; compile-only ?)
  \ NOTE: This is the fastest variant, but a copy of the _xt_
  \ is on the stack when the _xt_ is executed. This can be
  \ inconvenient in some cases.

: times-v3 ( u -- )
  rp@ @ dup cell+ rp@ ! @ swap
  0 ?do dup >r execute r> loop drop ; compile-only ?)

: run ( u -- )
  cr ." Results for " dup u. ." iterations"
  dup cr ." times noop \ v1 :" bench{ times-v1 noop }bench.
  dup cr ." times noop \ v2 :" bench{ times-v2 noop }bench.
      cr ." times noop \ v3 :" bench{ times-v3 noop }bench. ;

  \ 2017-08-13
  \
  \ Times Ticks (20 ms)
  \ ----- ----------------------------
  \         v1          v2          v3
  \       ---- ----------- -----------
  \  1000    5    5           9
  \ 10000   55   51 (0.92)   91 (1.65)
  \ 32000  176  164 (0.93)  290 (1.64)
  \ 65535  360  337 (0.93)  594 (1.65)

( ?throw-bench )

need bench{ need }bench.

: ?t1 ( f n -- ) swap if drop else drop then ;
  \ : ?throw ( f n -- ) swap if throw else drop then ;

: ?t2 ( f n -- ) swap 0<> and drop ;
  \ : ?throw ( f n -- ) swap 0<> and throw ;

: run ( u -- )
  cr ." Results for " dup u. ." iterations"
  dup cr ." false n ?throw \ v1 :"
  bench{ 0 ?do false i ?t1 loop }bench.
  dup cr ." false n ?throw \ v2 :"
  bench{ 0 ?do false i ?t2 loop }bench.
  dup cr ." true n ?throw \ v1 :"
  bench{ 0 ?do true i ?t1 loop }bench.
      cr ." true n ?throw \ v2 :"
  bench{ 0 ?do true i ?t2 loop }bench. cr ;

  \ 2017-08-13
  \
  \ Times Ticks (20 ms)
  \ ----- ---------------------------------
  \       false v1 false v2 true v1 true v2
  \       -------- -------- ------- -------
  \   100        2        2       1       2
  \  1000       15       16      16      17
  \ 10000      144      161     153     161
  \ 65535      944     1054    1000    1060

( create-bench )

need bench{ need }bench.

create try-create 256 c,

here 256 c, constant try-constant

: run ( u -- )
  cr ." Results for " dup u. ." iterations"
  dup cr ." create   :"
  bench{ 0 ?do try-create drop loop }bench.
      cr ." constant : "
  bench{ 0 ?do try-constant drop loop }bench. cr ;

  \ 2017-05-20:

  \ Times Ticks (20 ms)
  \ ----- -------------------------------
  \       create constant
  \       ------ --------
  \ 00100      0        1
  \ 01000      5        6
  \ 10000     48       56
  \ 65535    313      363

( type-udg-bench )

need bench{ need }bench. need type-udg

  \ XXX OLD -- This benchmark was written for Nuclear Waste
  \ Invaders.
  \
  \ `right-arrow$` was a `2constant` returning the address and
  \ length of a string containing the 2 UDG consecutive codes
  \ of the graphic.
  \
  \ `right-arrow` was a `cconstant` returning the first of the
  \ two consecutive UDG codes of the graphic.

: .2x1-udg-sprite ( c -- ) dup emit-udg 1+ emit-udg ;

: sprite-string-bench ( n -- )
  dup page ." type-udg :"
  bench{ 0 ?do 18 0 at-xy right-arrow$ type-udg
            loop }bench.
  0 1 at-xy ." .2x1-udg-sprite :"
  bench{ 0 ?do 18 1 at-xy right-arrow .2x1-udg-sprite
           loop }bench. ;

  \ 2017-07-27:
  \
  \ |===================================
  \ |       | Ticks (20 ms)
  \ |       | ==========================
  \ | Times | type-udg | .2x1-udg-sprite
  \
  \ |  1000 |      112 |              94
  \ | 10000 |     1122 |             937
  \ | 65535 |     7353 |            6142
  \ |===================================

( .2x1-udg-bench )

need .2x1-udg need .2x1-udg-fast need bench{ need }bench.

: .2x1-udg-old ( c -- ) dup emit-udg 1+ emit-udg ;

: run ( u -- )
  cls 0 1 at-xy ." Results for " dup u. ." iterations"
  dup 0 2 at-xy ." .2x1-udg-old :" cr space
  bench{ 0 ?do 128 .2x1-udg-old home loop }bench
  0 3 at-xy bench.
  dup 0 4 at-xy cr ." .2x1-udg :" cr space
  bench{ 0 ?do 128 .2x1-udg home loop }bench
  0 5 at-xy bench.
      0 6 at-xy cr ." .2x1-udg-fast :" cr space
  bench{ 0 ?do 128 .2x1-udg-fast home loop }bench
  0 7 at-xy bench. ;

  \ 2017-05-20:

  \ Times Ticks (20 ms)
  \ ----- ------------------------------------
  \       .2x1-udg-old .2x1-udg  .2x1-udg-fast
  \       ------------ --------  -------------
  \ 00100           10       10             10
  \ 01000           99       97             97
  \ 10000          989      968            965
  \ 65535         6485     6345           6323

  \ Notes:
  \
  \ `.2x1-udg` = Wrapper word. The low-level factor is a code
  \ word that uses a loop to display the two columns of the UDG
  \ block.
  \
  \ `.2x1-udg-fast` = Wrapper word. The low-level factor is a
  \ code word that uses duplicated code instead of a loop, to
  \ display the two columns of the UDG block.

( sqrt-bench )

need baden-sqrt need newton-sqrt need bench{ need }bench.

: run ( u -- )
  cls ." Results for " dup u. ." iterations"
  dup cr ." baden-sqrt :" cr space
  bench{ 0 ?do i abs baden-sqrt  drop loop }bench.
      cr ." newton-sqrt   :" cr space
  bench{ 0 ?do i abs newton-sqrt drop loop }bench. ;

  \ 2017-03-29:

  \ Times Ticks (20 ms)
  \ ----- --------------------------------
  \       baden[1]  baden[2]        newton
  \       --------  -------- -------------
  \ 00100       20        20    170 (8.50)
  \ 01000      220       219   1717 (7.80)
  \ 10000     2260      2258  17170 (7.59)
  \ 65535    15118     15105 112515 (7.44)
  \
  \ Notes:
  \ [1] Loop parameter compiled with `literal`.
  \ [2] Loop parameter compiled with `cliteral`.

( orthodraw-bench ortholine-bench )

need orthodraw need ortholine need bench{ need }bench.

: run ( u -- )
  cls ." Results for " dup u. ." iterations"
  dup cr ." orthodraw :" cr space
  bench{ 0 ?do  0 0 1 1 100 orthodraw loop }bench.
      cr ." ortholine :" cr space
  bench{ 0 ?do  0 0 1 1 100 ortholine  loop }bench. ;

  \ 2017-03-29:

  \ Times Ticks (20 ms)
  \ ----- -------------------------------
  \       orthodraw          ortholine
  \       ---------  -----------------
  \ 00100        97         56
  \ 01000       975        562
  \ 10000      9752       5621  (0.57)
  \ 65535     63911      36833  (0.57)

( ink-bench )

need bench{ need }bench.
need ink. need set-ink need attr@ need attr!

: old-set-ink ( b -- ) attr@ %11111000 and or attr! ;

: run ( u -- )
  cr ." Bench of ink "
  cr ." Results for " dup u. ." iterations"
  dup cr ." ink. :" cr space
  bench{ 0 ?do  i ink.         loop default-colors }bench.
  dup cr ." set-ink in Forth :" cr space
  bench{ 0 ?do  i old-set-ink  loop default-colors }bench.
      cr ." set-ink in Z80 :" cr space
  bench{ 0 ?do  i set-ink      loop default-colors }bench. ;

  \ 2017-01-31:

  \ Times Ticks (20 ms)
  \ ----- ------------------------------------------
  \       ink.   set-ink in Forth   set-ink in Z80
  \       ------ ------------------ ----------------
  \ 00100      2                  2                1
  \ 01000     25                 17                7
  \ 10000    248                173               70
  \ 65535   1630               1136              460

  \ Legend:
  \
  \ `ink.`: Written in Z80: It prints the ink control
  \ character followed by the ink number (so the temporary
  \ attribute is updated).
  \
  \ `set-ink` in Forth: Old version, written in Forth: It
  \ modifies only the needed bits of the temporary attribute:

  \ ----
  \ : set-ink ( b -- )
  \   attr@ %11111000 and or attr! ;
  \ ----

  \ `set-ink`: New version, written in Z80 in the library.

( paper-bench )

need bench{ need }bench.
need paper. need set-paper need papery
need attr@ need attr!

: old-set-paper ( b -- )
  papery attr@ %11000111 and or attr! ;

: run ( u -- )
  cr ." Bench of paper "
  cr ." Results for " dup u. ." iterations"
  dup cr ." paper. :" cr space
  bench{ 0 ?do  i paper.         loop default-colors }bench.
  dup cr ." set-paper in Forth :" cr space
  bench{ 0 ?do  i old-set-paper  loop default-colors }bench.
      cr ." set-paper in Z80 :" cr space
  bench{ 0 ?do  i set-paper      loop default-colors }bench. ;

  \ 2017-01-31:

  \ Times Ticks (20 ms)
  \ ----- ------------------------------------------
  \       paper. set-paper in Forth set-paper in Z80
  \       ------ ------------------ ----------------
  \ 00100      3                  2                1
  \ 01000     25                 19                8
  \ 10000    250                189               76
  \ 65535   1638               1234              498

  \ Legend:
  \
  \ `paper.`: Written in Z80: It prints the paper control
  \ character followed by the paper number (so the temporary
  \ attribute is updated).
  \
  \ `set-paper` in Forth: Old version, written in Forth: It
  \ modifies only the needed bits of the temporary attribute:

  \ ----
  \ : set-paper ( b -- )
  \   paper>attr attr@ %11000111 and or attr! ;
  \ ----

  \ `set-paper`: New version, written in Z80 in the library.

( circle-bench )

need bench{ need }bench.

need circle
need colored-circle-pixel need uncolored-circle-pixel

defer (number-base

: (run ( u -- )
  bench{ 0 ?do  127 95 95 circle  loop }bench. ;

: run ( u -- )
  cr ." Bench of circle "
  cr ." Results for " dup u. ." iterations"
  dup cr ." With colored-circle-pixel :" cr space
      colored-circle-pixel set-circle-pixel (run
      cr ." With uncolored-circle-pixel :" cr space
      uncolored-circle-pixel set-circle-pixel (run ;

  \ 2017-01-30:

  \ Times Ticks (20 ms)
  \ ----- ---------------------------------------
  \       colored circle  uncolored circle
  \       --------------- -----------------------
  \ 00010              53                32
  \ 00100             525               319
  \ 01000            5250              3191 (0.6)
  \ 10000           52501             31907 (0.6)

( mask+attr!-bench )

need bench{ need }bench.
need attr! need attr-mask! need os-attr-t need mask+attr!

4 cconstant green

: v1 ( x1 x2 -- ) attr! attr-mask! ;
: v2 ( x1 x2 -- ) os-attr-t 2! ;
: v3 ( x1 x2 -- ) mask+attr! ;

: run ( u -)
  cr ." Bench of mask+attr! "
  cr ." Results for " dup u. ." iterations"
  dup cr ." Variant 1: "
  bench{ 0 ?do  0 green v1  loop }bench.
  dup cr ." Variant 2: "
  bench{ 0 ?do  0 green v2  loop }bench.
      cr ." Variant 3: "
  bench{ 0 ?do  0 green v3  loop }bench. ;

  \ 2017-01-27:

  \ Times Ticks (20 ms)
  \ ----- -----------------------------
  \       mask+attr!
  \       -----------------------------
  \       Variant 1 Variant 2 Variant 3
  \       --------- --------- ---------
  \ 00010         0         0         0
  \ 00100         1         2         1
  \ 01000        12        13        11
  \ 10000       121       136       111

( -beep>note-bench )

  \ `-beep>note` is part of `beep>note`

need bench{ need }bench. need under+

12 cconstant /octave
  \ need cconst  12 cconst /octave

: -beep>note1 ( n1 -- -n2 +n3 )
  dup 1+ /octave / 1- tuck ( n2 n1 n2 )
  1+ /octave * - /octave + ;

: -beep>note2 ( n1 -- -n2 +n3 )
  dup 1+ /octave / tuck ( n2' n1 n2' )
  /octave * - /octave +  swap 1- swap ;

: -beep>note3 ( n1 -- -n2 +n3 )
  dup 1+ /octave / tuck ( n2' n1 n2' )
  /octave * - /octave +  -1 under+ ;

: -beep>note4 ( n1 -- -n2 +n3 )
  abs [ /octave 1- ] literal +
  /octave /mod negate [ /octave 1- ] literal rot - ;  -->

( -beep>note-bench )

: run ( u -)
  cr ." Bench of -beep>note"
  cr ." Results for " dup u. ." iterations"
  dup cr ." Variant 1: "
  bench{ 0 ?do  i -beep>note1 2drop  loop }bench.
  dup cr ." Variant 2: "
  bench{ 0 ?do  i -beep>note2 2drop  loop }bench.
  dup cr ." Variant 3: "
  bench{ 0 ?do  i -beep>note3 2drop  loop }bench.
      cr ." Variant 4: "
  bench{ 0 ?do  i -beep>note4 2drop  loop }bench. ;

  \ 2017-01-24:

  \ Times Ticks (20 ms)
  \ ----- ---------------------------------------
  \       -beep>note with `12 cconstant /octave`
  \       ---------------------------------------
  \       Variant 1 Variant 2 Variant 3 Variant 4
  \       --------- --------- --------- ---------
  \ 00010         2         2         1         1
  \ 00100        15        15        15        10
  \ 01000       143       145       144        95
  \ 10000      1430      1438      1436       951
  \ 65535      9618      9722      9660      6267

  \ Times Ticks (20 ms)
  \ ----- ---------------------------------------
  \       -beep>note with `12 cconst /octave`
  \       ---------------------------------------
  \       Variant 1 Variant 2 Variant 3 Variant 4
  \       --------- --------- --------- ---------
  \ 00010         2         1         1         1
  \ 00100        15        15        14         9
  \ 01000       143       144       144        96
  \ 10000      1421      1437      1428       948
  \ 65535      9548      9671      9597      6247

( hz>bleep-bench )

need bench{ need }bench. need */

: hz>bleep1 ( frequency duration1 -- duration2 pitch )
  over 1000 */ swap  4375 100 rot */ 30 - ;

: hz>bleep2 ( frequency duration1 -- duration2 pitch )
  over m* 1000 m/ nip swap  437500. rot m/ nip 30 - ;

: run ( u -)
  cr ." Bench of hz>bleep"
  cr ." Results for " dup u. ." iterations" dup
  cr ." with */        : "
  bench{ 0 ?do  i dup hz>bleep1 2drop  loop }bench.
  cr ." with m* and m/ : "
  bench{ 0 ?do  i dup hz>bleep2 2drop  loop }bench. ;

  \ 2017-01-23:

  \ Times Ticks (20 ms)
  \ ----- -------------------------------
  \       hz>bleep
  \       -------------------------------
  \       variant 1 variant 2
  \       --------- ---------
  \ 00010         3        2
  \ 00100        25       19
  \ 01000       247      185
  \ 10000      2463     1848
  \ 65535     16290    12263 (0.75)

( 1/string-bench )

need bench{ need }bench.

: run ( u -)
  cr ." Results for " dup u. ." iterations" dup
  cr ." 1 /string : "
  bench{ 0 ?do  0 0 1 /string 2drop  loop }bench.
  cr ." 1/string  : "
  bench{ 0 ?do  0 0 1/string  2drop  loop }bench. ;

  \ 2017-01-23:

  \ Times Ticks (20 ms)
  \ ----- --------------------------
  \                   `1/string`
  \                   --------------
  \       `1 /string` push jr   jp
  \       ----------- ---- ---- ----
  \ 00100           1    1    1    1
  \ 01000          11    9    9    9
  \ 10000         100   91   90   90
  \ 65535         661  594  590  588
  \       ----------- ---- ---- ----

  \ Data-space:          4    5    6  (bytes)

  \ Legend:
  \
  \ push: `1/string` pushes 1 on the stack and runs into
  \ `/string`. This was the chosen option.
  \
  \ jr: `1/string` loads DE with 1 and does a relative jump
  \ into `/string`.
  \
  \ jp: `1/string` loads DE with 1 and does an absolute jump
  \ into `/string`.

( substitute-bench )

need bench{ need }bench.
need substitute need substitute2 need replaces

create result 255 chars allot

s" saluton"  s" hello"    replaces
s" bonvenon" s" welcome"  replaces

: old ( -- ca len )
  s" Say '%hello%' and '%welcome%' then percentage sign %%." ;

result 255 2constant new

: run ( u -)
  cr ." Results for " dup u. ." iterations" dup
  cr ." substitute w/rot :      "
  bench{ 0 ?do  old new substitute  drop 2drop  loop }bench.
  cr ." substitute w/variable : "
  bench{ 0 ?do  old new substitute2 drop 2drop  loop }bench. ;

  \ 2017-01-22

  \ Times Ticks (20 ms)
  \ ----- --------------------------------------
  \       substitute w/rot substitute w/variable
  \       ---------------- ---------------------
  \ 00010               25                    26
  \ 00100              256                   256
  \ 01000             2558                  2556
  \ 10000            25574                 25571

  \ No difference!

  \ Version with `rot`:

  \ : substitute ( ca1 len1 ca2 len2 -- ca2 len3 n )
  \    /substitute-result ! 0 substitute-result 2! 0 -rot
  \    \ ( -- 0 ca1 len1 )
  \    substitute-error off
  \    begin  dup 0>  while ( -- n ca1 len1 )
  \      over substitution-delimiter? if
  \        over char+ substitution-delimiter?
  \        if    substitution-delimiter c>substitute-result
  \              2 /string
  \        else  get-substitution
  \              substituted? if  rot 1+ -rot  then
  \        then
  \      else  over c@ c>substitute-result 1 /string  then
  \    repeat  2drop substitute-result 2@ rot
  \            substitute-error @ ?dup if  nip  then ;

  \ Version with `variable`:

  \ variable substitutions
  \ : substitute2 ( ca1 len1 ca2 len2 -- ca2 len3 n )
  \    /substitute-result ! 0 substitute-result 2!
  \    \ ( -- ca1 len1 )
  \    substitute-error off  substitutions off
  \    begin  dup 0>  while ( -- n ca1 len1 )
  \      over substitution-delimiter? if
  \        over char+ substitution-delimiter?
  \        if    substitution-delimiter c>substitute-result
  \              2 /string
  \        else  get-substitution substituted? abs substitutions +!
  \        then
  \      else  over c@ c>substitute-result 1 /string  then
  \    repeat  2drop substitute-result 2@
  \            substitute-error @ ?dup 0=
  \            if  substitutions @  then ;

( >name-bench2 )

  \ 2017-12-12
  \ Compare two versions of `>name`.

need bench{ need }bench.  need >>name need name>>
need name>name need array> need name<name

: >name-forward ( xt -- nt | 0 )
  0 begin ( xt xtp )
    dup >>name >r  far@ over = if  drop r> exit  then
    r> name>name name>>
  np@ over u< until  2drop false ;
  \ Search all words from oldest to newest.
  \ This is a copy of `>name2` from `>name-bench1`.
  \
  \ WARNING: This implementation of `name>` is not absolutely
  \ reliable, because the dictionary is searched from oldest to
  \ newest definitions: The address of the next name field
  \ address is calculated from the name length of the previous
  \ one.  If something was compiled in name space between both
  \ definition headers, the result will be wrong, or `>name`
  \ may never return.

: >name-backward ( xt -- nt | 0 )
  #order @ 0 ?do
    i context array> @ @ ( xt nt0 )
    begin  dup
    while  2dup name>> far@ = if nip unloop exit then name<name
    repeat drop
  loop drop false ; -->
  \ Search all word lists, from newest to oldest,
  \ for _xt_, searching each word list from newest to oldest
  \ word.

( >name-bench2 )

: run ( u -)

  cr ." Results for " dup u. ." iterations"

  dup cr ." forward >name  : "
      bench{ 0 ?do
        [ latestxt ] literal
        >name-forward drop  loop }bench.
          \ Search forward for the latest header.

      cr ." backward >name : "
      bench{ 0 ?do
        [ root-wordlist >order ' forth previous ] literal
        >name-backward drop  loop }bench. ;
          \ Search backward for the oldest header.

  \ 2017-12-12
  \
  \ Times Ticks (20 ms)
  \ ----- -----------------------------
  \       forward >name  backward >name
  \       -------------  --------------
  \ 00010           379             183
  \ 00100          3788            1803
  \ 01000         37902           17996

( >name-bench1 )

  \ 2017-01-20

  \ Compare the current version of `>name`, which is written in
  \ Z80 in the kernel [and removed on 2017-12-12 from version
  \ 0.14.0-pre.369], with an equivalent new version written in
  \ Forth.
  \
  \ WARNING: These implementations of `name>` are not
  \ absolutely reliable, because the dictionary is searched
  \ from oldest to newest definitions: The address of the next
  \ name field address is calculated from the name length of
  \ the previous one.  If something was compiled in name space
  \ between both definition headers, the result will be wrong.

need bench{ need }bench.
need >>name need name>> need name>name

: >name2 ( xt -- nt | 0 )
  0 begin ( xt xtp )
    dup >>name >r  far@ over = if  drop r> exit  then
    r> name>name name>>
  np@ over u< until  2drop false ;

: run ( u -)
  cr ." Results for " dup u. ." iterations"
  dup cr ." >name in Z80:   "
      bench{ 0 ?do  latestxt >name  drop  loop }bench.
      cr ." >name in Forth: "
      bench{ 0 ?do  latestxt >name2 drop  loop }bench. ;

  \ 2017-01-20
  \
  \ Times Ticks (20 ms)
  \ ----- -----------------------------------
  \       >name in Z80  >name in Forth
  \       ------------  ---------------------
  \ 00010           62             380 (6.12)
  \ 00100          656            3793 (5.78)
  \ 01000         6532           37924 (5.80)

( m*-bench )

need bench{ need }bench. need d*

: km* ( n1 n2 - d) 2dup xor >r abs swap abs um* r> ?dnegate ;
  \ `m*` currently in the kernel:
  \ Code adapted from Abersoft Forth; used also by Z88
  \ CamelForth and Z80 eForth.

: lm* ( n1 n2 - d) >r s>d r> s>d d* ;
  \ `m*` in the library:
  \ Code by Robert L. Smith, published on Forth Dimensions
  \ (volume 4, number 1, page 3, 1982-05).

: run ( u -)
  cr ." Results for " dup u. ." iterations"
  dup cr ." Kernel m*: "
      bench{ 0 ?do  i i km* 2drop  loop }bench.
      cr ." Library m*: "
      bench{ 0 ?do  i i lm* 2drop  loop }bench. ;

  \ 2017-01-13:

  \ Times Ticks (20 ms)
  \ ----- ---------------------
  \       Kernel m*  Library m*
  \       ---------  ----------
  \ 00010         0           1
  \ 00100         5          14
  \ 01000        46         143
  \ 10000       462        1427

( dot-quote-bench )

need bench{ need }bench need bench.
need column need row need .\"

: run ( u -- )
  cr ." Results for " dup u. ." iterations"
  cr ." 'X' emit : "    column row 2>r
  dup bench{ 0 ?do  home 'X' emit       loop }bench
  2r> at-xy bench.
  cr .\" .\" X\"    : " column row 2>r
      bench{ 0 ?do  home ." X"          loop }bench
  2r> at-xy bench. ;

  \ 2017-01-11:

  \             Ticks (20 ms)
  \             ------------------------------
  \ Iterations  'X' emit    ." X"
  \ ----------  ----------- ------------------
  \         10            1           1
  \        100            8          12
  \       1000           79         120
  \      10000          795        1201
  \      65535         5211        7872 (1.51)

( at-xy-display-0udg-bench )

need bench{ need }bench.
need at-xy-display-0udg ( c col row -- )
need 0udg-at-xy-display ( col row c -- )

: run ( u -- )
  cr ." Results for " dup u. ." iterations"
  dup cr ." at-xy-display-0udg: "
  dup bench{ 0 ?do  0 0 0 at-xy-display-0udg  loop }bench.
      cr ." 0udg-at-xy-display: "
      bench{ 0 ?do  0 0 0 0udg-at-xy-display  loop }bench. ;

  \ 2017-01-09:

  \ Times Ticks (20 ms)
  \ ----- -------------------------------------
  \       at-xy-display-0udg 0udg-at-xy-display
  \       ------------------ ------------------
  \ 00010                  1                  1
  \ 00100                  1                  2
  \ 01000                 15                 16
  \ 01000                 16                 15
  \ 10000                156                156
  \ 32768                511                510
  \ 32768                511                510
  \ 65535               1022               1020

( aline176-bench )

need bench{ need }bench. need adraw176 need aline176

: adraw ( -- )   0 175 adraw176 255 175 adraw176
                 255   0 adraw176   0   0 adraw176 ;

: aline ( -- )   0 175 aline176 255 175 aline176
                 255   0 aline176   0   0 aline176 ;

: run ( u -- )
  cr ." Results for " dup u. ." iterations"
  dup cr ." adraw176: "
      0 swap bench{ 0 ?do  adraw  loop }bench. drop
      cr ." aline176: "
      0 swap bench{ 0 ?do  aline  loop }bench. drop ;

  \ 2016-12-27:

  \ Times Ticks (20 ms)
  \ ----- ------------------------------------
  \       adraw176  aline176 (1)  aline176 (2)
  \       --------  ------------  ------------
  \ 00010      369           360           336
  \ 00100     3691          3600          3359
  \ 01000    36905         36000         33587

  \ (1) First version, which saves the coordinates on the stack
  \ during the loop:

  \ incy 2! drop r> 1+ 0 2dup
  \ ?do  2drop x1 @ y1 @ 2dup set-pixel176
  \     x1 2@ incx 2@ d+ x1 2!
  \     y1 2@ incy 2@ d+ y1 2!  loop ( gx gy )
  \ [ os-coordy ] literal c! [ os-coordx ] literal c! ;

  \ (2) Second version, which uses `set-save-pixel176`, a
  \ variant of `set-pixel176` that saves the new graphic
  \ coordinates:

  \ incy 2! drop r> 1+ 0
  \ ?do  x1 @ y1 @ set-save-pixel176
  \     x1 2@ incx 2@ d+ x1 2!
  \     y1 2@ incy 2@ d+ y1 2!  loop ;

( +field-bench )

  \ 2016-11-28

need bench{ need }bench.

  \ Implementation of `+field` from the Forth-2012
  \ documentation:

: +field-cr ( n1 n2 "name" -- n3 )
  create  over , +  does> @ + ;

  \ The zero-field improved implementation,
  \ is a version without local variables
  \ of the following code:
  \
  \ Newsgroups: comp.lang.forth
  \ From: anton AT mips DOT complang DOT tuwien DOT ac DOT at (Anton Ertl)
  \ Subject: Re: ColorForth - another dead end?
  \ Date: Mon, 01 Jun 2015 13:21:34 GMT
  \ Message-ID: <2015Jun1.152134@mips.complang.tuwien.ac.at>
  \
  \ : +FIELD {: n1 n2 -- n3 :}
  \   : n1 if
  \     n1 postpone literal postpone +
  \   else
  \      immediate
  \   then
  \   postpone ; n1 n2 + ;

: +field-co ( n1 n2 "name" -- n3 )
  :
  over ?dup if    postpone literal postpone +
            else  immediate  then  postpone ;  + ;

-->

( +field-bench )

need case

: +field-ca ( n1 n2 "name" -- n3 )
  :
  over case
  0                   of  immediate                      endof
  1                   of  postpone 1+                    endof
  cell                of  postpone cell+                 endof
  [ 2 cells ] literal of  postpone cell+ postpone cell+  endof
  dup  postpone literal postpone +  \ default
  endcase  postpone ;  + ;

  \ Fields from offset 0:

$0000 1 +field-cr 0f-cr drop
$0000 1 +field-co 0f-co drop
$0000 1 +field-ca 0f-ca drop  -->

( +field-bench )

  \ Fields from offset greater than 0,
  \ with standard field size:

$0001 1 +field-cr 1f-cr drop
$0001 1 +field-co 1f-co drop
$0001 1 +field-ca 1f-ca drop
$0002 1 +field-cr 2f-cr drop
$0002 1 +field-co 2f-co drop
$0002 1 +field-ca 2f-ca drop
$0004 1 +field-cr 4f-cr drop
$0004 1 +field-co 4f-co drop
$0004 1 +field-ca 4f-ca drop

  \ Fields from offset greater than 0,
  \ with non-standard field size:

$0005 1 +field-cr 5f-cr drop
$0005 1 +field-co 5f-co drop
$0005 1 +field-ca 5f-ca drop  -->

( +field-bench )

: run ( u -- )
  cls ." +field benchmark"
  ." Results for " dup u. ." iterations"
  cr ." Field 0:"     cr ." - 'create'    "
  dup bench{ 0 ?do  0 0f-cr drop  loop }bench.
                      cr ." - '+'         "
  dup bench{ 0 ?do  0 0f-co drop  loop }bench.
                      cr ." - 'case'      "
  dup bench{ 0 ?do  0 0f-ca drop  loop }bench.
  cr ." Field non-0:" cr ." - +1 'create' "
  dup bench{ 0 ?do  0 1f-cr drop  loop }bench.
                      cr ." - +1 '+'      "
  dup bench{ 0 ?do  0 1f-co drop  loop }bench.
                      cr ." - +1 'case'   "
  dup bench{ 0 ?do  0 1f-ca drop  loop }bench.  -->

( +field-bench )

                      cr ." - +2 'create' "
  dup bench{ 0 ?do  0 2f-cr drop  loop }bench.
                      cr ." - +2 '+'      "
  dup bench{ 0 ?do  0 2f-co drop  loop }bench.
                      cr ." - +2 'case'   "
  dup bench{ 0 ?do  0 2f-ca drop  loop }bench.
                      cr ." - +4 'create' "
  dup bench{ 0 ?do  0 4f-cr drop  loop }bench.
                      cr ." - +4 '+'      "
  dup bench{ 0 ?do  0 4f-co drop  loop }bench.
                      cr ." - +4 'case'   "
  dup bench{ 0 ?do  0 4f-ca drop  loop }bench.  -->

( +field-bench )

                      cr ." - +5 'create' "
  dup bench{ 0 ?do  0 5f-cr drop  loop }bench.
                      cr ." - +5 '+'      "
  dup bench{ 0 ?do  0 5f-co drop  loop }bench.
                      cr ." - +5 'case'   "
      bench{ 0 ?do  0 5f-ca drop  loop }bench. ;

  \ 2016-11-28:

  \                     Ticks (20 ms)
  \ Field  +field       ---------------------
  \ offset implement.   Times             Id
  \ ------ ------------ ----------------- ---
  \                     01000 10000 65535
  \                     ----- ----- -----
  \ 0      create does>    12   122   796 #00
  \ 0      colon plus       5    51   330 #01
  \ 0      colon case       5    51   330 #02
  \ 1      create does>    13   121   796 #03
  \ 1      colon plus      12   120   789 #04
  \ 1      colon case      11   103   675 #05
  \ 2      create does>    12   121   796 #06
  \ 2      colon plus      12   121   789 #07
  \ 2      colon case      11   105   684 #08
  \ 4      create does>    12   121   796 #09
  \ 4      colon plus      12   121   788 #10
  \ 4      colon case      12   119   777 #11
  \ 5      create does>    12   122   796 #12
  \ 5      colon plus      12   121   788 #13
  \ 5      colon case      12   120   788 #14

  \ Notes:
  \
  \ Why #11 is not slower than #14?
  \ Why #14 is not faster than #11?
  \ Why #14 is not slower than #04, #07 and #10?

( transfer-bench  )

  \ 2016-11-26: Confirm this benchmark for the parameters used
  \ by `transfer-sector` in `transfer-block`.

need bench{ need }bench.

: constsum ( -- n ) buffer-data b/sector + ;
: litsum ( -- n ) [ buffer-data b/sector + ] literal ;

: run ( u -- )
  cr ." Results for " dup u. ." iterations"
  dup cr ." constants sum "
      0 swap bench{ 0 ?do  constsum drop  loop }bench. drop
      cr ." literal       "
      0 swap bench{ 0 ?do  litsum drop    loop }bench. drop ;

  \ Times Ticks (20 ms)
  \ ----- ---------------------------------
  \       constants sum             literal
  \       -------------  ------------------
  \ 00100             2                   1
  \ 01000            14                  10
  \ 10000           131                  94
  \ 65535           858                 613

( 2cells+-bench )

  \ 2016-11-26

need bench{ need }bench.

: cell+cell+ ( n -- ) cell+ cell+ ;
: 2cellslit+ ( n -- ) [ 2 cells ] literal + ;

: run ( u -- )
  cr ." Results for " dup u. ." iterations"
  dup cr ." cell+ cell+ "
      0 swap bench{ 0 ?do  cell+cell+  loop }bench. drop
      cr ." 4 +         "
      0 swap bench{ 0 ?do  2cellslit+  loop }bench. drop ;

  \ Times Ticks (20 ms)
  \ ----- ----------------------------------
  \       cell+ cell+  [ 2 cells ] literal +
  \       -----------  ---------------------
  \ 00100           1                      2
  \ 01000          10                      9
  \ 10000          95                     99
  \ 65535         625                    649

( number-base-bench )

  \ 2015-10-09

: number-base-1 ( ca len -- ca' len' n )
  \ This is the current version defined in the kernel.
  over c@ '$' = if  1 /string 16  exit  then
  over c@ '%' = if  1 /string  2  exit  then
  over c@ '#' = if  1 /string 10  exit  then  base @ ;

: number-base-2 ( ca len -- ca' len' n )
  over c@ >r
  r@ '$' = if  1 /string 16  rdrop exit  then
  r@ '%' = if  1 /string  2  rdrop exit  then
  r> '#' = if  1 /string 10  exit  then  base @ ;

: number-base-3 ( ca len -- ca' len' n )
  over c@
  dup >r '$' = if  1 /string 16  rdrop exit  then
      r@ '%' = if  1 /string  2  rdrop exit  then
      r> '#' = if  1 /string 10  exit  then  base @ ;

-->

( number-base-bench )

: number-base-4 ( ca len -- ca' len' n )
  over c@
  dup '$' = if  drop 1 /string 16  exit  then
  dup '%' = if  drop 1 /string  2  exit  then
      '#' = if  1 /string 10  exit  then  base @ ;

need ticks need reset-ticks  defer (number-base

: (number-base-bench ( n xt -- )
  ['] (number-base defer!
  reset-ticks  0 ?do  s" 000" (number-base drop 2drop  loop
  ticks d. cr ;

: number-base-bench ( u -- )
  dup ['] number-base-1 (number-base-bench
  dup ['] number-base-2 (number-base-bench
  dup ['] number-base-3 (number-base-bench
      ['] number-base-4 (number-base-bench ;

  \ 2015-10-09
  \
  \ Times Ticks (20 ms)
  \ ----- -------------------
  \          1    2    3    4
  \       ---- ---- ---- ----
  \ 01000   73   75   74   69
  \ 10000  732  744  736  686
  \ 32000 2343 2382 2367 2194

( fill-bench )

  \ 2015-09-25: Benchmark three implementations of `fill`:
  \
  \ `fill` is the original implementation from Abersoft Forth
  \ `fill2` is a modified version
  \ `fill88` is the code adapted from Z88 CamelForth

need ticks need reset-ticks need rnd

defer (fill

: (fill-bench ( n xt -- )
  ['] (fill defer!
  reset-ticks  0
  ?do  16384 6144 rnd (fill loop
  \ ?do  16384 1 rnd (fill loop
  \ ?do  16384 0 rnd (fill loop
  \ ?do  16384 2048 rnd (fill loop
  ticks cr d.
  key drop ;

: fill-bench ( u -- )
  dup ['] fill (fill-bench
  dup ['] fill2 (fill-bench
      ['] fill88 (fill-bench ;

  \ Kernel code: `16384 6144 rnd (fill`
  \
  \ Times Ticks (20 ms)
  \ ----- -----------------
  \       fill fill2 fill88
  \       ---- ----- ------
  \ 00010   10    10      5
  \ 00100  491   522    252
  \ 01000 4909  5218   2524

  \ Kernel code: `16384 1 rnd (fill`
  \
  \ Times Ticks (20 ms)
  \ ----- -----------------
  \       fill fill2 fill88
  \       ---- ----- ------
  \ 00010    1     0      0
  \ 00100    9     8      8
  \ 01000   85    84     84
  \ 05000  425   423    422
  \ 10000  850   846    845

  \ Kernel code: `16384 0 rnd (fill`
  \
  \ Times Ticks (20 ms)
  \ ----- -----------------
  \       fill fill2 fill88
  \       ---- ----- ------
  \ 00010    1     0      0
  \ 00100    8     8      8
  \ 01000   84    83     84
  \ 05000  421   418    421
  \ 10000  842   837    842

  \ Kernel code: `16384 2048 rnd (fill`
  \
  \ Times Ticks (20 ms)
  \ ----- ------------------
  \       fill  fill2 fill88
  \       ----- ----- ------
  \ 00010    17    18      9
  \ 00100   169   180     89
  \ 01000  1693  1795    898
  \ 30000 50770 53863  26933

( 2fetch-bench )

  \ This benchmark compares fetching of 2-cell variables,
  \ values, constants and literals.

  \ Change log:
  \
  \ 2016-05-31: `2constant-bench`, `2literal-bench`.
  \
  \ 2016-10-14: Update. Add `variable-bench` and
  \ `2variable-bench`.
  \
  \ 2018-01-24: Remove 1-cell benchmarks. Move old results to
  \ `fetch-bencthmark`. Combine with `2constant-bench` and
  \ `2literal-bench`.

need 2value need bench{ need }bench.

0. 2value val2

: 2val-bench ( u -- ) bench{ 0 ?do val2 2drop loop }bench. ;

2variable var2

: 2var-bench ( u -- ) bench{ 0 ?do var2 2@ 2drop loop }bench. ;

0. 2constant 2zero

: 2con-bench ( u -- ) bench{ 0 ?do 2zero 2drop loop }bench. ;

: 2lit-bench ( u -- )
  bench{ 0 ?do [ 0. ] 2literal 2drop loop }bench. ;

: run ( u -- )
  cr dup ." 2value    " 2val-bench cr
     dup ." 2variable " 2var-bench cr
     dup ." 2constant " 2con-bench cr
         ." 2literal  " 2lit-bench cr ;

  \ Date            Times Ticks
  \ --------------  ----- --------------------
  \                        2val 2var 2con 2lit
  \                        ---- ---- ---- ----
  \ 2016-05-31       1000               7    7
  \                 10000              68   64
  \                 65535             448  417
  \
  \ 2016-05-31 (0)  65535             435  417
  \
  \ 2016-10-14 (1)   1000      6   7
  \                 10000     64  73
  \                 65535    423 476
  \
  \ 2018-01-24 (2)   1000     11   7
  \                 10000    112  70
  \                 65535    731 458
  \
  \ 2018-01-24 (3)   1000      7   7    6    6
  \                 10000     63  70   63   61
  \                 65535    414 458  407  402


  \ Notes:
  \
  \ 0) After improving `2constant` (version
  \ 0.10.0-pre.13+20160531).
  \
  \ 1) Version 0.10.0-pre.79+20161014. The default `value` and
  \ `2value` are aliases of `constant` and `2constant`.
  \
  \ 2) Version 0.14.0-pre.422+20180124. `value` and `2value`
  \ are standard. In order to share `to`, they store a type
  \ identifier in their headers, which must be skiped in order
  \ to fetch the actual value. That's why they are much slower
  \ now.
  \
  \ 3) Version 0.14.0-pre.423+20180124. The run-time part of
  \ `value` and `2value` has been rewriten in Z80.

( fetch-bench )

  \ This benchmark compares the time needed to get the contents
  \ of values and variables.

  \ 2016-10-15: Start.

need value need cvalue need cvariable
need bench{ need }bench.

0 value val
: val-bench ( u -- ) bench{ 0 ?do val drop loop }bench. ;

0 cvalue cval
: cval-bench ( u -- ) bench{ 0 ?do cval drop loop }bench. ;

variable var
: var-bench ( u -- ) bench{ 0 ?do var @ drop loop }bench. ;

cvariable cvar
: cvar-bench ( u -- ) bench{ 0 ?do cvar c@ drop loop }bench. ;

: run ( u -- )
  cr dup ." value     " val-bench cr
     dup ." cvalue    " cval-bench cr
     dup ." variable  " var-bench cr
         ." cvariable " cvar-bench cr ;

  \ Date            Times Ticks (20 ms)
  \ --------------  ----- -------------------------------------
  \                           value   cvalue variable cvariable
  \                       --------- -------- -------- ---------
  \ 2016-10-15 (1)   1000         6        5        6         6
  \                 10000        57       56       64        63
  \                 65535       371      366      418       413
  \
  \ 2018-01-24 (2)   1000        10                 6
  \                 10000       103                61
  \                 65535       677               403
  \
  \ 2018-01-24 (3)   1000         5        6        7         6
  \                 10000        58       57       61        61
  \                 65535       378      375      404       401

  \ Notes:
  \
  \ 1) Version 0.10.0-pre.79+20161014. The default `value` and
  \ `2value` are aliases of `constant` and `2constant`.
  \
  \ 2) Version 0.14.0-pre.422+20180124. `value` and `cvalue`
  \ are standard. In order to share `to`, they store a type
  \ identifier in their headers, which must be skiped in order
  \ to fetch the actual value. That's why they are much slower
  \ now.
  \
  \ 3) Version 0.14.0-pre.423+20180124. The run-time part of
  \ `value` and `cvalue` has been rewriten in Z80.

( store-bench )

  \ Compare storing into 1-cell and character values and
  \ variables.

  \ Change log:
  \
  \ 2016-10-16: Start.
  \
  \ 2018-01-24: Update: replace `cto` with `to`.

need value need cvalue need cvariable
need bench{ need }bench.

0 value val  0 cvalue cval  variable var  cvariable cvar

: val-bench ( u -- ) bench{ 0 ?do i to val loop }bench. ;

: cval-bench ( u -- ) bench{ 0 ?do i to cval loop }bench. ;

: var-bench ( u -- ) bench{ 0 ?do i var ! loop }bench. ;

: cvar-bench ( u -- ) bench{ 0 ?do i cvar c! loop }bench. ;

: run ( u -- ) cr dup ." value     " val-bench cr
                  dup ." cvalue    " cval-bench cr
                  dup ." variable  " var-bench cr
                      ." cvariable " cvar-bench cr ;

  \ Date            Times Ticks
  \ --------------  ----- -------------------------------------
  \                           value   cvalue variable cvariable
  \                       --------- -------- -------- ---------
  \
  \ 2016-10-16 (1)   1000         9        8        8         8
  \                 10000        87       84       81        79
  \                 65535       568      552      532       519
  \
  \ 2018-01-24 (2)   1000         8        8        8         8
  \                 10000        85       83       79        76
  \                 65535       554      542      521       500

  \ Notes:
  \
  \ 1) Version 0.10.0-pre.84+20161015. With non-standard `cto`
  \ for "cvalues".
  \
  \ 2) Version 0.14.0-pre.423+20180124. With standard `to` for
  \ "cvalues", and improved `value` and `cvalue` with run-time
  \ code rewritten in Z80.

( to-value-bench to-2value-bench 2to-2value-bench )

need value need 2value need bench{ need }bench.

0 value v1

: to-value-bench ( u -- )
  bench{  0 ?do  0 to v1  loop  }bench. ;

0. 2value v2

: to-2value-bench ( u -- )
  bench{  0 ?do  0. to v2  loop  }bench. ;

0. 2value v2

: 2to-2value-bench ( u -- )
  bench{  0 ?do  0. 2to v2   loop  }bench. ;

( rshift-bench lshift-bench )

  \ 2015-11-01: Compare two Z80 implementation of `rshift` and
  \ `lshift`.
  \
  \ 2017-01-21: Remove the slower implementations (and 3 bytes
  \ smaller) from the library.

need ticks need reset-ticks

: rshift-bench ( u -- )
  reset-ticks  0
  ?do  128 255 rshift drop   loop
  ticks cr d. ;

: lshift-bench ( u -- )
  reset-ticks  0
  ?do  128 255 lshift drop   loop
  ticks cr d. ;

  \ 2015-11-01

  \ Times Ticks (20 ms)
  \ ----- -----------------------------
  \       rshift         lshift
  \       -------------- --------------
  \        Z88  DZX    %  Z88  DZX    %
  \       ---- ---- ---- ---- ---- ----
  \ 10000 1203 1609 133% 1016 1723 169%
  \ 30000 3607 4826 133% 3048 5170 169%

  \ 2017-01-21

  \ Data space used by the code
  \ ---------------------------
  \ rshift  lshift
  \ ------- -------
  \ Z88 DZX Z88 DZX
  \ --- --- --- ---
  \ 16  13  19  16

  \ Legend:
  \ Z88 = code adapted from Z88 CamelForth
  \ DZX = code adapted from DZX-Forth

( /-bench )

  \ 2015-09-22: This bench compares the execution speed of
  \ Abersoft Forth's `m/` and Z88 CamelForth's `sm/rem`. Both
  \ words are equivalent.  Abersoft Forth's `m/` is much
  \ faster.

need ticks need reset-ticks need rnd

: drnd ( -- d ) rnd rnd ;

defer (/ ( d n1 -- n2 n3 )

: (/-bench ( n -- )
  reset-ticks
  1+ 1 ?do  drnd i (/ 2drop  loop  ticks cr d. ;

: /-bench ( u -- )
  dup ['] m/ ['] (/ defer! (/-bench
      ['] sm/rem ['] (/ defer! (/-bench ;

  \ Times Ticks (20 ms)
  \ ----- -------------
  \       m/    sm/rem
  \       ----- ------
  \ 00010     3      4
  \ 00100    33     44
  \ 01000   326    442

  \ m/     = word from Abersoft Forth
  \ sm/rem = word from Z88 Camel Forth

( um*-bench um/mod-bench )

need bench{ need }bench.

  \ --------------------------------------------

unneeding um*-bench ?(

  \ 2017-02-01: Update.

: run ( u -- )
  bench{  0 ?do  i i um* 2drop  loop  }bench. ;

  \ Times Ticks (20 ms)
  \ ----- -------------------------------------------
  \       In 2015                             In 2017
  \       ----------------------------------- -------
  \       DZX   hForth R hForth A Z88 R Z88 A DZX2
  \       ----- -------- -------- ----- ----- ----

  \ 01000    29       32       31    32    31   28
  \ 10000   297      328      319   323   316  282
  \ 20000   598      659      643   647   633  566
  \ 32000   961     1060     1037  1037  1016  911

  \            Bytes free Code from
  \            ---------- ---------
  \ DZX      = 33783      DZX-Forth
  \ hForth R = 33787      hForth, with relative jumps
  \ hForth A = 33784      hForth, with absolute jumps
  \ Z88 R    = 33786      Z88 CamelForth, with relative jumps
  \ Z88 A    = 33784      Z88 CamelForth, with absolute jumps
  \ DZX2     = N/A        DZX-Forth (running on Solo Forth
  \                       v0.13.0-pre.204+20170201)

  \ Note: The difference from DZX to DZX2 is caused by the
  \ Forth-83 `do` in the benchmark loop. The benchmarks in 2015
  \ still used the fig-Forth `do`.`

  \ --------------------------------------------

unneeding um/mod-bench ?(

  \ 2015-11-24.
  \ 2017-02-01: Update.

: um/mod-bench ( u -- )
  bench{  0 ?do  i s>d i um/mod 2drop  loop  }bench. ;

: um/mod-bench88 ( n -- )
  bench{  0 ?do  i s>d i um/mod88 2drop  loop  }bench. ;

: run ( u -- )
  dup cr ." Abersoft Forth  U/MOD ..." um/mod-bench
      cr ." Z88 CamelForth UM/MOD ..." um/mod-bench88 ; ?)

  \ Times Ticks (20 ms)
  \ ----- --------------------
  \          AF      Z88
  \       ----- --------------
  \ 00100     6        5
  \ 01000    59       42 (71%)
  \ 10000   587      428 (72%)
  \ 20000  1157      875 (75%)
  \ 32000  1881     1372 (72%)

  \            Bytes free Code from
  \            ---------- --------------
  \ AF         32689      Abersoft Forth
  \ Z88        32707      Z88 CamelForth

( ud/mod-bench )

  \ 2015-12-21

need bench{

: a-m/mod ( n -- )
  bench{  0 ?do  i s>d i ud/mod drop 2drop  loop  }bench. ;

: z1-ud/mod ( n -- )
  bench{  0 ?do  i s>d i ud/mod881 drop 2drop  loop  }bench. ;

: z2-ud/mod ( n -- )
  bench{  0 ?do  i s>d i ud/mod882 drop 2drop  loop  }bench. ;

: run ( u -- )
  dup cr ." Abersoft Forth  M/MOD ..." a-m/mod
  dup cr ." Z88 CamelForth UD/MOD 1..." z1-ud/mod
      cr ." Z88 CamelForth UD/MOD 2..." z2-ud/mod ;

  \ 10000 run  20000  run 65535 run

  \ Times   Ticks (20 ms)
  \ -----   ------------------------
  \            AF    Z88 (1) Z88 (2)
  \         ----- ---------- -------
  \ 10000     964    967      944
  \ 20000    1928   1934     1888
  \ 65535    6300   6316     6161

  \            AF    Z88 (1) Z88 (2)
  \         ----- ---------- -------
  \ B used:    22     22      20 (3)

  \ (1) Z88 CamelForth code
  \ (2) Z88 CamelForth code, with `-rot` instead of `rot rot`
  \ (3) Not including the size of `-rot`

( ud/mod-bench )

  \ 2016-03-15: Second benchmark. Faster results because of
  \ DTC.

need bench{

: a-m/mod ( n -- )
  bench{  0 ?do  i s>d i ud/mod drop 2drop  loop  }bench. ;

: z-ud/mod ( n -- )
  bench{  0 ?do  i s>d i ud/mod88 drop 2drop  loop  }bench. ;


: run ( u -- )
      cr ." UD/MOD from:"
  dup cr ." Abersoft Forth and Gforth ..." a-m/mod
  dup cr ." Z88 CamelForth .............." z-ud/mod ;

  \ 10000 run  20000  run 65535 run

  \ Times   Ticks (20 ms)
  \ -----   -------------
  \            AF    Z88
  \         ----- ------
  \ 10000     900    882
  \ 20000    1799   1765
  \ 65535    5870   5759

( number?-bench )

  \ 2015-10-14

need ticks need reset-ticks

: empty-stack ( -- ) sp0 @ sp! ;

defer num?

: number?-bench ( u -- )
  reset-ticks  0 ?do
    s" " num?  s" 12345" num?   s" 12345." num?
    s" -12345" num?  s" -12345." num?  empty-stack
  loop  ticks cr d. ;

: benchs ( -- )
  100 number?-bench 1000 number?-bench 10000 number?-bench ;

                                    \ Version of `number?`
  \    ' number? ' num? defer! benchs  \ pForth
  \  ' c.number? ' num? defer! benchs  \ CamelForth
  \ ' dzx-number? ' num? defer! benchs  \ DZX-Forth
   ' solo-number? ' num? defer! benchs  \ Solo Forth

  \ Note: The CamelForth code is for single numbers only.
  \       The DZX-Forth code is a bit obfuscated.

  \ Times Ticks (20 ms)
  \ ----- --------------------------------------
  \       pForth CamelForth DZX-Forth Solo Forth
  \       ------ ---------- --------- ----------
  \ 00100    256        257       259        266
  \ 01000   2559       2565      2594       2658
  \ 10000  25591      25652     25933      26581

( number?-bench )

  \ 2015-10-14

need ticks need reset-ticks

: empty-stack ( -- ) sp0 @ sp! ;

defer num?

: number?-bench ( u -- )
  reset-ticks  0 ?do
    s" " num?  s" 123x45." num?   s" 12345.999x" num?
    s" -12345.x" num?  s" -12345.999x" num?
    s" -12345.000.000" num?
    empty-stack
  loop  ticks cr d. ;

: benchs ( -- )
  100 number?-bench 1000 number?-bench 10000 number?-bench ;

' solo-number? ' num? defer! benchs

  \ Times Ticks (20 ms)
  \ ----- -------------
  \ 00100   416
  \ 01000  4165
  \ 10000 41649

( dummy-needed )

( buffer-bench1 )

  2 load need ticks need reset-ticks
  reset-ticks

  need dummy-needed need dummy-needed need dummy-needed
  need dummy-needed need dummy-needed need dummy-needed
  need dummy-needed need dummy-needed need dummy-needed
  need dummy-needed need dummy-needed need dummy-needed
  need dummy-needed need dummy-needed need dummy-needed
  need dummy-needed

  ticks cr .( Ticks ) d. cr

  \ 2015-11-04:

  \ Benchmark: Locate and load 16 times empty block #457.

  \ Times Ticks (20 ms)
  \ ----- ----------------------------------
  \        512-byte buffer  1024-byte buffer
  \       ---------------- -----------------
  \    16             6323       8621 (136%)

( buffer-bench2 )

  2 load need ticks need reset-ticks  reset-ticks

  need list need dump need wdump need decode
  need life need hanoi need tt need siderator need pong
  need doer need a! need defer need value need editor
  need case need times need dtimes need for

  ticks cr .( Ticks ) d. cr

  \ Benchmark: interpretation of many source blocks from disk.

  \ ===========================================================
  \ Date       Condition             Bytes free   Ticks (=20ms)
  \ ---------- --------------------- ------------ -------------
  \ 2015-11-04 512-byte buffer       33742        20960 (1.00)
  \            1024-byte buffer      33277 (-465) 24310 (1.15)
  \                                               24042 (1.14)

  \ This is not good for benchmarking the headers, because most
  \ of the time is wasted locating the blocks. That's why
  \ both methods are equally fast:

  \ ===========================================================
  \ Date       Condition             Bytes free   Ticks (=20ms)
  \ ---------- --------------------- ------------ -------------
  \ 2015-11-17 `next-name` (1)       32807        40530
  \                                               40485
  \                                               40526
  \            `nextname` (2)        32781        40555
  \                                               40510
  \                                               40554

  \ (1) First method: `next-name` is a double-cell variable
  \ that may hold a string to be used as name by the next
  \ defining word.  `header` always checks this string and, if
  \ it's not empty, uses it instead of parsing and then empties
  \ it.
  \
  \ (2) Second method (written after Gforth): `nextname` stores
  \ a string into the double-cell variable `nextname-string`,
  \ and sets the deferred word `header` to `nextname-header`,
  \ which creates the header with the string name and restores
  \ the default action of `header`: `input-stream-header`. This
  \ method is more versatile and, beside, words with emtpy
  \ names can be created.

( 143-header-bench )

need bench{ need }bench. bench{ : w ; : w ; : w ; : w ;
: w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ;
: w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ;
: w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ;
: w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ;
: w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ;
: w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ;
: w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ;
: w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ;
: w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ;
: w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ;
: w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ;
: w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ;
: w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ;
: w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ; : w ; }bench.

  \ Benchmark: Create 143 empty colon words.

  \ ===========================================================
  \ Date       Condition             Bytes free   Ticks (=20ms)
  \ ---------- --------------------- ------------ -------------
  \ 2015-11-17 `next-name` (1)       32807        490 (9 s)
  \            `next-name` (4)       32791        494 (9 s)
  \            `nextname` (2)        32781        491 (9 s)
  \            `nextname` (3)        32765        494 (9 s)

  \ (1) First method: `next-name` is a double-cell variable
  \ that may hold a string to be used as name by the next
  \ defining word.  `header` always checks this string and, if
  \ it's not empty, uses it instead of parsing and then emptis
  \ it.
  \
  \ (2) Second method (written after Gforth): `nextname` stores
  \ a string into the double-cell variable `nextname-string`,
  \ and sets the deferred word `header` to `nextname-header`,
  \ which creates the header with the string name and restores
  \ the default action of `header`: `input-stream-header`. This
  \ method is more versatile and, beside, words with emtpy
  \ names can be created.
  \
  \ (3) Same as (2) but with zero-length name check.
  \
  \ (4) Same as (1) but with zero-length name check.

( 15120-header-bench )

need bench{ blk @ 1+ constant b bench{ b load
b load b load b load b load b load b load b load b load b load
b load b load b load b load b load b load b load b load b load
b load b load b load b load b load b load b load b load b load
b load b load b load b load b load b load b load b load b load
b load b load b load b load b load b load b load b load b load
b load b load b load b load b load b load b load b load b load
b load b load b load b load b load b load b load b load b load
b load b load b load b load b load b load b load b load b load
b load b load b load b load b load b load b load b load b load
b load b load b load b load b load b load b load b load b load
b load b load b load b load b load b load b load b load b load
b load b load b load b load b load b load b load b load b load
b load b load b load b load b load b load b load b load b load
b load b load b load b load b load b load b load b load }bench.

  \ Benchmark: Create 15120 empty colon words, loading 170 a
  \ block that contains 120 definitions.

  \ ===========================================================
  \ Date       Condition             Bytes free   Ticks (=20ms)
  \ ---------- --------------------- ------------ -------------
  \ 2015-11-17 `next-name` (1)       32807        40530
  \                                               40485
  \                                               40526
  \            `nextname` (2)        32781        40555
  \                                               40510
  \                                               40554

  \ (1) First method: `next-name` is a double-cell variable
  \ that may hold a string to be used as name by the next
  \ defining word.  `header` always checks this string and, if
  \ it's not empty, uses it instead of parsing and then emptis
  \ it.
  \
  \ (2) Second method (written after Gforth): `nextname` stores
  \ a string into the double-cell variable `nextname-string`,
  \ and sets the deferred word `header` to `nextname-header`,
  \ which creates the header with the string name and restores
  \ the default action of `header`: `input-stream-header`. This
  \ method is more versatile and, beside, words with emtpy
  \ names can be created.

( 15120-header-bench )

: foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ;
: foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ;
: foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ;
: foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ;
: foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ;
: foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ;
: foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ;
: foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ;
: foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ;
: foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ;
: foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ;
: foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ;
: foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ;
: foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ;
: foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ; : foo ;

( interpret-[#-bench )

  \ Benchmark `interpret` interpreting numbers

  need bench{  bench{

1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20
1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20
1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 sp0 @ sp!
1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20
1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20
1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 sp0 @ sp!
1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20
1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20
1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 sp0 @ sp!
1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20
1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20
1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 sp0 @ sp!
1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20

  }bench. sp0 @ sp!

  \ How `interpret`
  \ interprets numbers   Bytes free  Ticks          Date
  \ ------------------   ----------  -------------  ----------
  \ branches (1)              32766     500 (1.00)  2015-11-12
  \ execution table (1)       32770     497 (0.99)  2015-11-12
  \ execution table (2)       32761     498 (0.99)  2015-11-12
  \ execution table (3)                 476 (0.95)  2016-03-19

  \ (1): before implementing an execution table for words
  \ (2): shared with the words, integrating the common factor
  \ (3): same as (2), but when the Forth system is DTC

  \ Note: 1 tick = 20 ms.

( interpret-]#-bench )

  \ Benchmark `interpret` compiling numbers

  need bench{  bench{

: foo  1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20
1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20
1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20
1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20
1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20
1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20
1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20
1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20
1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20
1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20
1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20
1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20
1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 ;

  }bench.

  \ How `interpret`
  \ compiles numbers    Bytes free  Ticks          Date
  \ ----------------    ----------  -------------  ----------
  \ branches (1)             32766     510 (1.00)  2015-11-12
  \ execution table (1)      32770     507 (0.99)  2015-11-12
  \ execution table (2)      32761     508 (0.99)  2015-11-12
  \ execution table (3)                483 (0.94)  2016-03-19

  \ (1): before implementing an execution table for words
  \ (2): shared with the words, integrating the common factor
  \ (3): same as (2), but when the Forth system is DTC

  \ Note: 1 tick = 20 ms.

( interpret-[name-bench )

  \ Benchmark `interpret` interpreting words

  need bench{  bench{

noop noop noop noop noop noop noop noop noop noop noop noop
noop noop noop noop noop noop noop noop noop noop noop noop
noop noop noop noop noop noop noop noop noop noop noop noop
noop noop noop noop noop noop noop noop noop noop noop noop
noop noop noop noop noop noop noop noop noop noop noop noop
noop noop noop noop noop noop noop noop noop noop noop noop
noop noop noop noop noop noop noop noop noop noop noop noop
noop noop noop noop noop noop noop noop noop noop noop noop
noop noop noop noop noop noop noop noop noop noop noop noop
noop noop noop noop noop noop noop noop noop noop noop noop
noop noop noop noop noop noop noop noop noop noop noop noop
noop noop noop noop noop noop noop noop noop noop noop noop
noop noop noop noop noop noop noop noop noop noop noop noop

  }bench.

  \ How `interpret`
  \ interprets words       Bytes free  Ticks       Date
  \ ---------------------  ----------  ----------- ----------
  \ branches (0)           32770       192 (1.00)  2015-11-12
  \ independent table (1)  32746       190 (0.98)  2015-11-12
  \ combined table (2)     32747       190 (0.98)  2015-11-12
  \ combined table (3)     32753       192 (1.00)  2015-11-12
  \ combined table (4)     32761       190 (0.98)  2015-11-12
  \ combined table (5)                 191 (0.99)  2016-03-19

  \ (0): after implementing an execution table for numbers
  \ (1): separate from the numbers table
  \ (2): shared with the numbers
  \ (3): shared with the numbers, using a common factor
  \ (4): shared with the numbers, integrating the common factor
  \ (5): same as (4), but when the Forth system is DTC

  \ Note: 1 tick = 20 ms.

( interpret-]name-bench )

  \ Benchmark `interpret` compiling words

  need bench{  bench{

: foo noop noop noop noop noop noop noop noop noop noop noop
noop noop noop noop noop noop noop noop noop noop noop noop
noop noop noop noop noop noop noop noop noop noop noop noop
noop noop noop noop noop noop noop noop noop noop noop noop
noop noop noop noop noop noop noop noop noop noop noop noop
noop noop noop noop noop noop noop noop noop noop noop noop
noop noop noop noop noop noop noop noop noop noop noop noop
noop noop noop noop noop noop noop noop noop noop noop noop
noop noop noop noop noop noop noop noop noop noop noop noop
noop noop noop noop noop noop noop noop noop noop noop noop
noop noop noop noop noop noop noop noop noop noop noop noop
noop noop noop noop noop noop noop noop noop noop noop noop
noop noop noop noop noop noop noop noop noop noop noop noop ;

  }bench.

  \ How `interpret`
  \ compiles words        Bytes free  Ticks       Date
  \ ------------------    ----------  ----------  ----------
  \ branches (0)          32770       199 (1.00)  2015-11-12
  \ independent table (1) 32746       198 (0.98)  2015-11-12
  \ combined table (2)    32747       198 (0.99)  2015-11-12
  \ combined table (3)    32753       200 (1.00)  2015-11-12
  \ combined table (4)    32761       198 (0.98)  2015-11-12
  \ combined table (5)                191 (0.95)  2016-03-19

  \ (0): after implementing an execution table for numbers
  \ (1): separate from the numbers table
  \ (2): shared with the numbers
  \ (3): shared with the numbers, using a common factor
  \ (4): shared with the numbers, integrating the common factor
  \ (5): same as (4), but when the Forth system is DTC

  \ Note: 1 tick = 20 ms.

( constants-bench )

  \ Compare all 1-cell and character constants: actual
  \ constants, literals, code constants and values.

  \ Change log:
  \
  \ 2015: `constant` vs `cconstant`.
  \ 2016-02-16: `constant` vs `literal`.
  \ 2016-05-31: Combined, improved and completed with
  \ `cliteral` and code constants.
  \ 2018-01-24: Add `value` and `cvalue`.

need bench{ need }bench. need value need cvalue

1000 constant thousand 100 cconstant hundred

: constant-bench ( u -- )
  bench{ 0 ?do thousand drop loop }bench. ;

: cconstant-bench ( u -- )
  bench{ 0 ?do hundred drop loop }bench. ;

: code-constant-bench ( u -- )
  bench{ 0 ?do true drop loop }bench. ;

: literal-bench ( u -- )
  bench{ 0 ?do [ 1000 ] literal drop loop }bench. ;

: cliteral-bench ( u -- )
  bench{ 0 ?do [ 100 ] cliteral drop loop }bench. ; -->

( constants-bench )

1000 cvalue v1000 100 cvalue v100

: value-bench ( u -- ) bench{ 0 ?do v1000 drop loop }bench. ;

: cvalue-bench ( u -- ) bench{ 0 ?do v100 drop loop }bench. ;

: run ( u -- )
  cr
  dup ." constant      " constant-bench cr
  dup ." cconstant     " cconstant-bench cr
  dup ." code constant " code-constant-bench cr
  dup ." literal       " literal-bench cr
  dup ." cliteral      " cliteral-bench cr
  dup ." value         " value-bench cr
      ." cvalue        " cvalue-bench cr ;

  \ Date           Times Ticks
  \ -------------- ----- ---------------------------
  \                      con cco cod lit cli val cva
  \                      --- --- --- --- --- --- ---
  \
  \ 2015 (1)       32000     251 236
  \
  \ 2015-02-16      1000   6         5
  \                52000 307         284
  \                65535 386         358
  \
  \ 2016-05-31     32000 189 187 161 173 169
  \                65535 387 383 330 355 346
  \
  \ 2016-05-31 (2) 65535 371 366 330 355 346
  \
  \ 2016-10-14 (3)  1000   6
  \                10000  57
  \                52000 294
  \                65535 371 366 330 355 346
  \
  \ 2018-01-24 (4)  1000   6   6   5   5   5   6   6
  \                10000  55  55  50  52  51  58  58
  \                32000 177 176 160 168 162 183 183
  \                52000 288 285 259 272 264 298 298
  \                65535 363 360 326 343 333 376 375

  \ Notes:
  \
  \ 1) With the old fig-Forth `do loop` structure.
  \
  \ 2) After improving `constant` and `cconstant` (version
  \ 0.10.0-pre.12+20160531).
  \
  \ 3) Version 0.10.0-pre.79+20161014.
  \
  \ 4) Version 0.14.0-pre.423+20180124.

( d*-bench )

  \ `d*` by Wil Baden, published on Forth Dimensions
  \ (volume 19, number 6, page 33, 1998-04).

: baden-d* ( d1 d2 -- d3 )
  >r swap >r            ( d1lo d2lo ) ( R: d2hi d1hi )
  2dup um* 2swap        ( d1lo*d2lo d1lo d2lo )
  r> * swap r> * + + ; ( d1*d2 ) ( R: )

  \ `d*` by Robert L. Smith, published on Forth Dimensions
  \ (volume 4, number 1, page 3, 1982-05).

need pick need roll

: smith-d* ( d1 d2 -- d3 )
  over 4 pick um*  5 roll 3 roll * +  2swap * + ;

  \ `d*` from DX-Forth 4.13

unused
: dx-d* ( d|ud1 d|ud2 -- d|ud3 )
  >r swap >r 2dup um* rot r> * + rot r> * + ;
unused - . cr key drop

-->

( d*-bench )

need bench{

: baden-d*-bench ( u -- ) 0 ?do  1. 2. baden-d* 2drop  loop ;
: smith-d*-bench ( u -- ) 0 ?do  1. 2. smith-d* 2drop  loop ;
: dx-d*-bench ( u -- )    0 ?do  1. 2.    dx-d* 2drop  loop ;

: d*-benchs ( -- )
  page
  32767 dup bench{ baden-d*-bench }bench.
        dup bench{ smith-d*-bench }bench.
            bench{ dx-d*-bench }bench. ;

  \ 2015-11-09: baden-d*, smith-d*

  \ Bench     Ticks for 32767 iterations
  \ -----     --------------------------
  \ baden-d*  4920 (98 seconds)  1.00
  \ smith-d*  5189 (103 seconds) 1.05

  \ 2015-12-22:
  \
  \ Bench     Ticks for 32767 iterations
  \ -----     --------------------------
  \ baden-d*  4860 (97 seconds)   1.0008
  \ smith-d*  5139 (102 seconds)  1.0582
  \ dx-d*     4856 (97 seconds)   1.0000

( misc-benchs )

  \ Some misc speed benchs.

need bench{ need 0if

: bench1 ( n -- )
  begin  ?dup if  1 - else  exit  then  again ;

: bench1a ( n -- )
  begin  ?dup if  1- else  exit  then  again ;

: bench2 ( n -- )
  begin  dup 0= if  drop  exit  then  1-  again ;

: bench2a ( n -- )
  begin  dup 0if  drop exit  then  1-  again ;

: bench3 ( n -- )
   begin  ?dup 0if  exit  then  1-  again ;


: misc-benchs ( -- )
  32767 dup bench{ bench1 }bench.
        dup bench{ bench2 }bench.
            bench{ bench3 }bench. ;

  \ Bench    Ticks for 32767 iterations
  \ -----   ---------------------------
  \ bench1   655 (`1 -`)
  \ bench1a  576 (`1-`)
  \ bench2   320 (`0= if`: 100%)
  \ bench2a  245 (`0if`:    76%)
  \ bench3   528

( 2swap-bench )

  \ 2015-11-24

need bench{

: 2swap-bench ( -- )
  32767 0 bench{ 2dup ?do  2swap  loop  }bench. 2drop ;

  \ Code                          Ticks for 32767 iterations
  \ -----                         --------------------------
  \ From DZX-Forth                271 (5 s) (1.00)
  \ Adapted from Z88 CamelForth   243 (4 s) (0.89)

( dnegate-bench )

  \ 2015-11-24

need bench{

: dnegate-bench ( -- )
  32767 0 bench{ 2dup ?do  dnegate  loop  }bench. 2drop ;

: dnegate-bench2 ( -- )
  32767 0 bench{ 2dup ?do  dnegate2  loop  }bench. 2drop ;

  \ Code                          Ticks for 32767 iterations
  \ -----                         --------------------------
  \ From Abersoft Forth           243 (4 s) (1.00)
  \ From Spectrum Forth-83        253 (5 s) (1.04)

( ?dup-bench )

  \ 2016-01-01

need bench{  variable times  40000 times !

: iterations ( -- n1 n2 ) times @ 0 ;

: forth-0-?dup-bench ( -- )
  bench{ iterations ?do  0 ?dup drop  loop  }bench. ;

: z80-0-?dup-bench ( -- )
  bench{ iterations ?do  0 ?dup80 drop  loop  }bench. ;

: forth-1-?dup-bench ( -- )
  bench{ iterations ?do  1 ?dup 2drop  loop  }bench. ;

: z80-1-?dup-bench ( -- )
  bench{ iterations ?do  1 ?dup80 2drop  loop  }bench. ;

: ?dup-bench ( -- )
  cr ." Forth version:" cr ." 0 ?dup :" forth-0-?dup-bench cr
                           ." 1 ?dup :" forth-1-?dup-bench cr
     ." Z80 version:"   cr ." 0 ?dup :" z80-0-?dup-bench cr
                           ." 1 ?dup :" z80-1-?dup-bench cr ;

  \ Code        Ticks for 40000 iterations
  \ -----       --------------------------
  \             Forth  Z80
  \             -----  ----
  \ `0 ?dup`    532    288
  \ `1 ?dup`    585    312

( #spaces-bench )

need under+

: #spaces1 ( ca len -- +n )
  0 rot rot  0 ?do  count bl = 1 and under+  loop  drop ;
  \ From:
  \ http://forth.sourceforge.net/mirror/comus/index.html

: #spaces2 ( ca len -- +n )
  0 rot rot  bounds ?do  i c@ bl = +  loop  abs ;
  \ First variant.

: #spaces3 ( ca len -- +n )
  0 rot rot  0 ?do  count bl = under+  loop  drop abs ;
  \ Second variant, the fastest one.

need bench{

defer #spaces

: #spaces-bench ( u -- )
  cr bench{  0 ?do  0 32767 #spaces drop  loop  }bench. ;

: run ( u -- )
  dup ['] #spaces1 ['] #spaces defer! ." 1..." #spaces-bench
  dup ['] #spaces2 ['] #spaces defer! ." 2..." #spaces-bench
      ['] #spaces3 ['] #spaces defer! ." 3..." #spaces-bench ;

  \         Ticks (20 ms)
  \         ----------------------------
  \ Version 10 iterations 100 iterations
  \ ------- ------------- --------------
  \ 1                 123           1231
  \ 2                 103           1036
  \ 3                  95            951

( emit-udg-bench )

need bench{ need }bench.

: emit-udg-bench ( n -- )
  dup bench{ 0 ?do  home   128 emit      loop  }bench
      0 1 at-xy ." emit      " bench.
  dup bench{ 0 ?do  home   128 emit-udg  loop  }bench
      0 2 at-xy ." emit-udg  " bench.
      bench{ 0 ?do  home 65368 emit-udga loop  }bench
      0 3 at-xy ." emit-udga " bench. ;

  \                        Ticks (20 ms)
  \                        ---------------------------------
  \ Date       Iterations  emit        emit-udg    emit-udga
  \ ---------- ----------  ----------- ----------- ---------
  \ 2015-12-18 32000       1904 (1.00) 1856 (0.97)
  \ 2017-07-27 32000       2382        2318        2307

( m+-bench )

  \ 2016-04-15

need bench{ need m+

: code-m+-bench ( u -- )
  bench{  0 ?do  i s>d i m+ 2drop  loop  }bench. ;

unused
: high-m+ ( n -- ) s>d d+ ;
unused - cr .( bytes of high M+ ) .

: high-m+-bench ( d1|ud1 n -- d2|ud2 )
  bench{  0 ?do  i s>d i high-m+ 2drop  loop  }bench. ;


: run ( u -- )
  cr dup cr ." Code M+ ..." code-m+-bench
         cr ." High M+ ..." high-m+-bench ;

  \ 10000 run  65535 run

  \ Times   Ticks (20 ms)
  \ -----   -------------------------------------
  \         code M+ (13 bytes)  high M+ (9 bytes)
  \         ------------------  -----------------
  \ 10000                  134         196 (1.44)
  \ 65535                  883        1308 (1.48)

( du<-bench )

  \ 2016-04-15

need bench{ need j

unused
  \ 2016-04-15: Current version in the library
: dzx-forth-du< ( ud1 ud2 -- f )
  rot swap 2dup
  u<  if  2drop 2drop -1 exit  then
  -   if  2drop 0 exit  then  u< ;
unused - cr .( DZX-Forth ) . .( bytes)  \ 41

unused
: m3forth-du< ( ud1 ud2 -- f )
  rot 2dup = if 2drop u< else u> nip nip then ;
unused - cr .( m3forth ) . .( bytes)  \ 29
  \ https://github.com/oco2000/m3forth/blob/master/lib/include/double.f

-->

( du<-bench )

defer (u<

: du<-bench ( u xt -- )
  ['] (u< defer!
  bench{
    dup 0 ?do  0 ?do  i s>d j s>d (u< drop  loop  loop
  }bench. ;

: run ( u -- )
      cr ." DU< from:"
  dup cr ." DZX-Forth ..." ['] dzx-forth-du< du<-bench
      cr ." 3mforth ....." ['] m3forth-du< du<-bench ;

  \ 10 run

  \ Times   Ticks (20 ms)
  \ -----   -------------------------
  \         DZX-Forth DU< m3forth DU<
  \         ------------- -----------
  \ 10               3623      crash!

( m*/-bench )

  \ XXX UNDER DEVELOPMENT

  \ 2016-04-15: Start.

unused

  \ 2016-04-15: This is the current implementation.
  \ Credit: from Gforth 0.7.3.

: gforth-m*/ ( d1 n1 +n2 -- d2 )
  >r s>d >r abs -rot s>d r> xor r> swap >r >r dabs
  rot tuck um* 2swap um* swap
  >r 0 d+ r> -rot i um/mod -rot r> um/mod -rot r>
  if     if     1 0 d+
         then
         dnegate
  else   drop
  then ;
unused - cr .( Gforth ) . .( bytes)  \ 89 bytes

-->

( m*/-bench )

  \ Alternative implementation of `m*/`
  \
  \ Credit:
  \
  \ Robert Smith (from COLDFORTH Version 0.8, GPL)
  \ https://github.com/oco2000/m3forth/blob/master/lib/include/double.f

unused
need mt* need tnegate need ut/
unused
: coldforth-m*/ ( d1 n1 +n2 -- d2 )
    >r mt* dup 0< if    tnegate r> ut/ dnegate
                  else  r> ut/  then ;
         cr .( Coldforth:)
unused - cr .( m*/ only ) . .( bytes)  \ 33 bytes
unused - cr .( with requirements) . .( bytes)  \ 185 bytes

-->

( m*/-bench )

need bench{

defer (m*/

: m*/-bench ( u xt -- )
  ['] (m*/ defer!
  bench{
    1+ 1 ?do  i s>d i i (m*/ 2drop  loop
    \ XXX FIXME -- use better range of numbers
  }bench. ;

: run ( u -- )
      cr ." M*/ from:"
  dup cr ." Gforth ......" ['] gforth-m*/ m*/-bench
      cr ." ColdForth ..." ['] coldforth-m*/ m*/-bench ;

  \ Times   Ticks (20 ms)
  \ -----   --------------------------------
  \         Gforth M*/  ColdForth M*/
  \         ----------  --------------------
  \ 10000         1690           1719 (1.01)
  \ 20000         3381           3442 (1.01)
  \ 65535        11621              0 \ XXX FIXME --
  \

( u<-bench )

  \ 2016-04-16: Start.
  \ 2016-04-29: Fix the loop. First working version.

need bench{ need j

defer (u<

variable times

: u<-bench ( u xt -- )
  ['] (u< defer!  times !
  bench{
    times @ 0 ?do  times @ 0 ?do  i j
    \ 2dup . . key drop  \ XXX INFORMER
    (u< drop  loop  loop
  }bench. ;

: run ( u -- )
      cr ." Implementation of U<"
  dup cr ."   DZX-Forth .........." ['] u< u<-bench
      cr ."   Z88 CamelForth ....." ['] z88u< u<-bench ;

  \ 2016-04-29

  \ Times   Ticks (20 ms)
  \ -----   --------------------------------------
  \         DZX-Forth u<  Z88 CamelForth u<
  \         ------------- ------------------------
  \   50               31                30 (0.96)
  \  100              125               120 (0.96)
  \  250              773               744 (0.96)
  \ 1000            12148             11869 (0.97)

( u<=-bench u>=-bench )

  \ 2016-04-29

need alias need alias! need j need bench{ need }bench.

: 0(u<= ( u1 u2 -- f ) u> 0= ;
: 1(u<= ( u1 u2 -- f ) 1+ u< ;

: 0(u>= ( u1 u2 -- f ) u< 0= ;
: 1(u>= ( u1 u2 -- f ) 1- u> ;

' drop alias operator
latest constant operator-nt

variable times

: operator-bench ( u xt -- )
  operator-nt alias!  times !
  bench{
    times @ 0 ?do  times @ 0 ?do  i j operator drop  loop  loop
  }bench. ;

-->

( u<=-bench u>=-bench )

: run ( u -- )
      cr ." Implementation of U<="
  dup cr ."   u> 0= ..." ['] 0(u<= operator-bench
  dup cr ."   1+ u< ..." ['] 1(u<= operator-bench
      cr ." Implementation of U>="
  dup cr ."   u< 0= ..." ['] 0(u>= operator-bench
      cr ."   1- u< ..." ['] 1(u>= operator-bench ;

  \ Times   Ticks (20 ms)
  \ -----   ----------------------------
  \         u<=             u>=
  \         ------------    ------------
  \         u> 0=  1+ u<    u< 0=  1- u<
  \         -----  -----    -----  -----
  \  100      106    107      107    107
  \ 1000    10560  10560    10560  10560

( 3clshift-bench 8*-bench )

  \ 2016-05-01

need bench{ need }bench. need clshift

: 3clshift-bench ( u -- )
  bench{  0 ?do  1 3 clshift drop   loop  }bench. ;

: 8*-bench ( u -- )
  bench{  0 ?do  1 8 * drop   loop  }bench. ;

: run ( u -- )
  dup cr ." 3 clshift ..." 3clshift-bench
      cr ." 8 * ........." 8*-bench ;

  \ Times  Ticks (20 ms)
  \ -----  ------------------
  \        3 clshift  8 *
  \        ---------  -------
  \ 10000        106      480

( search-bench )

  \ 2016-05-05

need bench{ need }bench. need alias need alias!

0 $FFFF 2constant haystack
  \ String to search: the whole memory

s" Need" $FF00 1- place 'l' $FF04 c! 'e' $FF05 c!
  \ Store "Needle" at $FF00, but hide it in this block

$FF00 6 2constant needle

defer do-search

: search-bench ( u xt -- )
  ['] do-search defer!
  bench{  0 ?do  haystack needle do-search drop 2drop   loop
  }bench. ;

: run ( u -- )
  dup cr ." DZX-Forth .........." ['] search search-bench
  dup cr ." Z88 CamelForth 1 ..." ['] search881 search-bench
      cr ." Z88 CamelForth 2 ..." ['] search88 search-bench ;

  \ Times  Ticks (20 ms)
  \ -----  ---------------------------------------------
  \        DZX-Forth  Z88 CamelForth 1  Z88 CamelForth 2
  \        ---------  ----------------  ----------------
  \     1        207                24                24
  \   100      20704              2396              2395
  \   200                         4790              4791

  \                   Bytes free
  \                   ----------
  \ DZX-Forth              32797
  \ Z88 CamelForth 1       32776
  \ Z88 CamelForth 2       32786

( <-bench )

  \ 2016-05-06

need bench{ need j

defer (<

variable times

: <-bench ( u xt -- )
  ['] (< defer!  times !
  bench{
    times @ 0 ?do  times @ 0 ?do  i j
    (< drop  loop  loop
  }bench. ;

: run ( u -- )
      cr ." Implementation of <"
  dup cr ."   DZX-Forth .........." ['] < <-bench
      cr ."   Z88 CamelForth ....." ['] <88 <-bench ;

  \ 2016-05-06

  \ Times   Ticks (20 ms)
  \ -----   --------------------------------------
  \         DZX-Forth `<`  Z88 CamelForth `<`
  \         ------------- ------------------------
  \   50               33                31 (0.93)
  \  100              129               122 (0.94)
  \  250              803               755 (0.94)
  \ 1000            12534             12049 (0.96)

( =-bench )

  \ 2016-05-06

need bench{

defer (=

variable times

: =-bench ( u xt -- )
  ['] (= defer!  times !
  bench{
    times @ 0 ?do  2 2 (= 1 0 (= 2drop  loop
  }bench. ;

: run ( u -- )
      cr ." Implementation of ="
  dup cr ."   DZX-Forth .........." ['] = =-bench
  dup cr ."   Z88 CamelForth 1...." ['] =881 =-bench
      cr ."   Z88 CamelForth 2...." ['] =882 =-bench ;

  \ 2016-05-06

  \ ; DXZ-Forth
  \ _code_header equals_,'='
  \ pop de
  \ pop hl
  \ call compare_de_hl_unsigned
  \ jp z,true_
  \ jp false_

  \ ; Z88 CamelForth 1
  \ _code_header equals881_,'=881'
  \ pop de
  \ pop hl
  \ or a
  \ sbc hl,de
  \ jp z,true_
  \ jp false_

  \ ; Z88 CamelForth 2
  \ _code_header equals882_,'=882'
  \ pop de
  \ pop hl
  \ or a
  \ sbc hl,de
  \ jp nz,false_
  \ ; execution continues into `true`

  \ Times   Ticks (20 ms)
  \ -----   -------------------------------------------
  \         DZX-Forth Z88 CamelForth 1 Z88 CamelForth 2
  \         --------- ---------------- ----------------
  \ 10000         175       164 (0.93)       162 (0.92)
  \ 65535        1148      1073 (0.93)      1060 (0.92)

( min-bench )

  \ 2016-05-06

need bench{ need j

defer (<

variable times

: <-bench ( u xt -- )
  ['] (< defer!  times !
  bench{
    times @ 0 ?do  times @ 0 ?do  i j
    (< drop  loop  loop
  }bench. ;

: run ( u -- )
      cr ." Implementation of <"
  dup cr ."   DZX-Forth .........." ['] < <-bench
      cr ."   Z88 CamelForth ....." ['] <88 <-bench ;

  \ 2016-05-06

  \ Times   Ticks (20 ms)
  \ -----   --------------------------------------
  \         DZX-Forth `<`  Z88 CamelForth `<`
  \         ------------- ------------------------
  \   50               33                31 (0.93)
  \  100              129               122 (0.94)
  \  250              803               755 (0.94)
  \ 1000            12534             12049 (0.96)

( ?throw-bench )

need >body need bench{ need }bench.

: ?throw0 ( f n -- ) swap if  throw  else  drop  then ;
: ?throw1 ( f n -- )
  swap ?branch [ ' throw >body , ] drop ;
: ?throw2 ( f n -- ) swap 0<> and throw ;
: ?throw3 ( f n -- ) and throw ;
: ?throw4 ( f n -- ) swap if  throw  then  drop ;

: run ( u -- )
  cr ." Versions"
  dup cr ." 0: " bench{ 0 ?do  0 0 ?throw0  loop }bench.
  dup cr ." 1: " bench{ 0 ?do  0 0 ?throw1  loop }bench.
  dup cr ." 2: " bench{ 0 ?do  0 0 ?throw2  loop }bench.
  dup cr ." 3: " bench{ 0 ?do  0 0 ?throw3  loop }bench.
      cr ." 4: " bench{ 0 ?do  0 0 ?throw4  loop }bench. ;

  \ 2016-05-13

  \ Times   Ticks (20 ms)
  \ -----   --------------------------------------------
  \          ?throw0  ?throw1  ?throw2  ?throw3  ?throw4
  \          -------  -------  -------  -------  -------
  \  1000         15       15       23       20       15
  \ 10000        149      148      230      198      150
  \ 65535        979      969     1507     1294      978

( set-pixel-bench )

need bench{ need }bench need bench.
need slow-gxy>scra_ need fast-gxy>scra_ need set-pixel
need j

: set-pixel-bench ( -- d )
  bench{
    192 0 ?do  256 0 ?do  i j set-pixel  loop  loop
  }bench ;

: mode ( xt -- ) ['] gxy>scra_ defer! ;
: slow ( -- ) ['] slow-gxy>scra_ mode ;
: fast ( -- ) ['] fast-gxy>scra_ mode ;

: run ( -- )
  cls fast set-pixel-bench cls slow set-pixel-bench cls
  ." Results of set-pixel in ticks:" cr
  ." slow-gxy>scra_:" bench. cr
  ." fast-gxy>scra_:" bench. cr ;

  \ Date            Ticks (20 ms)
  \ --------------  -----------------------------
  \                 slow set-pixel fast set-pixel
  \                 -------------- --------------
  \ 2016-10-15 (1)      683 (1.00)     682 (0.99)

  \ Notes:
  \ (1) Version 0.10.0-pre.80+20161015.

( pixels-bench )

need bench{ need }bench need bench.
need slow-pixels need fast-pixels

: garbage ( -- ) 0 16384 6144 move ;
  \ Fill the screen bitmap with the start of the ROM.

: pixels-bench ( -- d )
  cls garbage bench{ pixels }bench rot drop ;

: mode ( xt -- ) ['] pixels defer! ;
: slow ( -- ) ['] slow-pixels mode ;
: fast ( -- ) ['] fast-pixels mode ;

: run ( -- )
  fast pixels-bench slow pixels-bench cls
  ." Results of pixels in ticks:" cr
  ." slow-pixels:" bench. cr
  ." fast-pixels:" bench. cr ;

  \ Date            Ticks (20 ms)
  \ --------------  -----------------------
  \                 slow-pixels fast pixels
  \                 ----------- -----------
  \ 2016-10-15 (1)    37 (1.00)   36 (0.97)

  \ Notes:
  \ (1) Version 0.10.0-pre.81+20161015.

( d0=-bench )

need bench{ need }bench.

: o-d0= ( d -- f ) or 0= ;  : +-d0= ( d -- f ) + 0= ;

code c1-d0= ( d -- f )
  E1 c, D1 c, 19 c, 78 04 + c, B0 05 + c,
  \ pop hl
  \ pop de
  \ add hl,de
  \ ld a,h
  \ or l
  CA c, ' true , ' false jp, end-code
  \ jp z,true_
  \ jp false_

code c2-d0= ( d -- f )
  E1 c, D1 c, 19 c, 78 04 + c, B0 05 + c,
  \ pop hl
  \ pop de
  \ add hl,de
  \ ld a,h
  \ or l
  C2 c, ' false , 2B c, E5 c, jpnext, end-code
  \ jp nz,false_
  \ dec hl ; HL = true
  \ push hl
  \ _jp_next

-->

( d0=-bench )

: run-true ( -- ) cr ." d0= benchmark with true results" cr
  65535 dup cr ." or:"
            bench{ 0 ?do 0 0 o-d0= drop loop }bench.
        dup cr ."  +:"
            bench{ 0 ?do 0 0 +-d0= drop loop }bench.
        dup cr ." c1:"
            bench{ 0 ?do 0 0 c1-d0= drop loop }bench.
            cr ." c2:"
            bench{ 0 ?do 0 0 c2-d0= drop loop }bench. ; -->

( d0=-bench )

: run-false ( -- ) cr ." d0= benchmark with false results" cr
  65535 dup cr ." or:"
            bench{ 0 ?do 0 1 o-d0= drop loop }bench.
        dup cr ."  +:"
            bench{ 0 ?do 0 1 +-d0= drop loop }bench.
        dup cr ." c1:"
            bench{ 0 ?do 0 1 c1-d0= drop loop }bench.
            cr ." c2:"
            bench{ 0 ?do 0 1 c2-d0= drop loop }bench. ; -->

( d0=-bench )

: run-alt ( -- ) cr ." d0= benchmark with alternate results" cr
  65535 dup cr ." or:"
            bench{ 0 ?do 0 i 1 and o-d0= drop loop }bench.
        dup cr ."  +:"
            bench{ 0 ?do 0 i 1 and +-d0= drop loop }bench.
        dup cr ." c1:"
            bench{ 0 ?do 0 i 1 and c1-d0= drop loop }bench.
            cr ." c2:"
            bench{ 0 ?do 0 i 1 and c2-d0= drop loop }bench. ;

  \ 2017-05-06:
  \
  \ Benchmark results in ticks (20 ms).

  \
  \ `or 0=` `+ 0=`      c1 (*)      c2 (*) Note
  \ ------- ------ ----------- ----------- ----------
  \     896    879  532 (0.60)  525 (0.59) run-true
  \     897    879  546 (0.62)  533 (0.60) run-false
  \    1178   1170  831 (0.71)  827 (0.70) run-alt

  \ * The figure in parens is the percentage of `+ 0=`.

( d=-bench )

need bench{ need }bench.

: d<>(2 ( xd1 xd2 -- f ) rot <> if 2drop true exit then <> ;
: d=(2n ( xd1 xd2 -- f ) d<>(2 0= ;
: d=(2 ( xd1 xd2 -- f ) rot <> if 2drop false exit then = ;

: d=(1   ( xd1 xd2 -- f ) rot = >r = r> and ;
: d<>(1n ( xd1 xd2 -- f ) d=(1 0= ;
: d<>(1 ( xd1 xd2 -- f ) rot <> >r <> r> or ;

-->

( d=-bench )

: run ( -- )
  cr ." d= benchmark" cr  65535
  dup cr ." d=(1  :"
  bench{ 0 ?do 0. 0. d=(1  0. 1. d=(1  2drop loop }bench.
  dup cr ." d=(2n :"
  bench{ 0 ?do 0. 0. d=(2n  0. 1. d=(2n  2drop loop }bench.
  dup cr ." d=(2  :"
  bench{ 0 ?do 0. 0. d=(2 0. 1. d=(2 2drop loop }bench.
  dup cr ." d<>(1n:"
  bench{ 0 ?do 0. 0. d<>(1n 0. 1. d<>(1n 2drop loop }bench.
  dup cr ." d<>(2 :"
  bench{ 0 ?do 0. 0. d<>(2 0. 1. d<>(2 2drop loop }bench.
      cr ." d<>(1 :"
  bench{ 0 ?do 0. 0. d<>(1 0. 1. d<>(1 2drop loop }bench. ;

  \ 2017-05-08:
  \
  \ Benchmark results in ticks (20 ms).

  \ d=(1 d=(2n d=(2 d<>(1n d<>(2 d<>(1
  \ ----- ------ ----- ------- ------ ------
  \  2967   3159  2426    3703   2442   2980
  \
  \ Combinations:
  \ d=(1 + d<>(1n = 2967 + 3703 = 6670
  \ d=(2n + d<>(2 = 3159 + 2442 = 5601

  \ ===========================================================
  \ Change log

  \ 2016-04-24: Remove `[char]`, which has been moved to the
  \ library.
  \
  \ 2016-04-29: Add benchmark of two versions of `u<=` and
  \ `u>=`.  Fix benchmark of `u<`.
  \
  \ 2016-05-01: Add benchmark for comparing `3 clshift` and `8
  \ *`.
  \
  \ 2016-05-05: Add benchmark for `search`.
  \
  \ 2016-05-06: Add benchmarks for `<` and `=`.
  \
  \ 2016-05-13: Add benchmark for `?throw`.
  \
  \ 2016-05-17: Need `>body`, which has been moved to the
  \ library.
  \
  \ 2016-05-31: Add new benchmarks for `cliteral`, `literal`,
  \ `2literal`, `2constant`, `constant`, `cconstant` and code
  \ constants (like `true` and the new versions of `0`, `1` and
  \ `2`). Combine previous code.
  \
  \ 2016-08-05: Combine some blocks in order to save space.
  \
  \ 2016-10-14: Add new benchmarks for fetching variables and
  \ double variables.
  \
  \ 2016-10-15: Add `fetch-bench`, `set-pixel-bench`,
  \ `pixels-bench`.
  \
  \ 2016-10-16: Add `store-bench`.
  \
  \ 2016-11-26: Add `2cells+-bench`.  Remove `warnings off`,
  \ because now warnings are deactivated by default.
  \
  \ 2016-11-28: Add `+field-bench`.
  \
  \ 2016-12-08: Rename the module filename with uppercase
  \ "MISC" after the new convention.
  \
  \ 2016-12-26: Add `aline176-bench`.
  \
  \ 2017-01-09: Add `at-xy-display-0udg-bench`. Fix stack
  \ notation of all benchmarks.
  \
  \ 2017-01-11: Add `dot-quote-bench`.
  \
  \ 2017-01-13: Add `m*-bench`.
  \
  \ 2017-01-20: Add `>name-bench`.
  \
  \ 2017-01-21: Update the documentation of `rshift-bench` and
  \ `lshift-bench`.
  \
  \ 2017-01-22: Add `substitute-bench`.
  \
  \ 2017-01-23: Add `1/string-bench`, `hz>bleep-bench`.
  \
  \ 2017-01-24: Fix `hz>bleep2` in `hz>bleep-bench` (the result
  \ of the benchmark is not affected). Add `-beep>note-bench`.
  \
  \ 2017-01-27: Add `mask+attr!-bench`.
  \
  \ 2017-01-30: Add `circle-bench`.
  \
  \ 2017-01-31: Add `paper-bench`, `ink-bench`.
  \
  \ 2017-02-01: Revise `um*-bench` and `um/mod-bench`.
  \
  \ 2017-02-19: Replace `do`, which has been moved to the
  \ library, with `?do`.
  \
  \ 2017-02-26: Update "hp" notation to "np", after the changes
  \ in the kernel.
  \
  \ 2017-03-11: Fix typo.
  \
  \ 2017-03-13: Update names including "rec" to "sector(s)";
  \ update names including "blk" to "block(s)".  Rename:
  \ `(pixel-addr)` to `gxy>scra_`, `fast-(pixel-addr)` to
  \ `fast-gxy>scra_`, `slow-(pixel-addr)` to `slow-gxy>scra_`.
  \
  \ 2017-03-29: Add `orthodraw-bench`, `ortholine-bench`
  \ `sqrt-bench`.
  \
  \ 2017-04-04: Improve comments.
  \
  \ 2017-05-06: Add `d0=-bench`.
  \
  \ 2017-05-08: Add `d=-bengh`.
  \
  \ 2017-05-08: Improve module description.
  \
  \ 2017-05-09: Remove `jppushhl,`.
  \
  \ 2017-05-10: Improve some benchmark names.
  \
  \ 2017-05-11: Improve some benchmark names.
  \
  \ 2017-05-20: Add `.2x1-udg-bench` and `create-bench`.
  \
  \ 2017-07-27: Add `udg-type-bench`, from Nuclear Waste
  \ Invaders.  Improve `emit-udg-bench` and add `emit-udga` to
  \ it.
  \
  \ 2017-08-13: Add `?throw-bench`.
  \
  \ 2017-11-27: Add `times-bench`.
  \
  \ 2017-11-28: Update: replace "frames" words with "ticks"
  \ words; update headings of tables accordingly.
  \
  \ 2017-12-04: Add `s>d-bench`.
  \
  \ 2017-12-09: Update with `need */`, since `*/` was moved to
  \ the library.
  \
  \ 2017-12-09: Remove useless `[defined] (/) ?\`.
  \
  \ 2017-12-12: Rename `>name-bench` `>name-bench1`.  Add
  \ `>name-bench2`, `dnegate-d+-bench`, `negate-+-bench`,
  \ `past?-bench`.
  \
  \ 2017-12-31: Add `0>-bench`.
  \
  \ 2018-01-24. Combine and divide some benchmarks to make the
  \ results more coherent and useful.  Update the results of
  \ `fetch-bench`, `2fetch-bench`, `constants-bench`, and
  \ `store-bench`.
  \
  \ 2018-02-20: Add `cells-+-bench`.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-09: Update stack notation "x y" to "col row".
  \
  \ 2018-03-11: Add `>in-bench`.
  \
  \ 2018-04-11: Update notation "double variable" to
  \ "double-cell variable".
  \
  \ 2018-06-02: Add `located-bench`.
  \
  \ 2018-06-03: Simplify `located-bench`: use the code already
  \ defined in the `need` tool.
  \
  \ 2018-06-04: Update: remove trailing closing paren from
  \ word names.
  \
  \ 2018-06-10: Move the block access speed results from the
  \ manual.

  \ vim: filetype=soloforth
