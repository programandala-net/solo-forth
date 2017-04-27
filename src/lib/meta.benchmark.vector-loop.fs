  \ meta.benchmark.vector-loop.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201704271853
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ The Vector-Loop benchmark.

  \ ===========================================================
  \ Authors

  \ M. Edward Borasky, 1995-07-30; code published on Forth
  \ Dimensions (volume 17, number 4, page 11, 1995-11).

  \ Adapted by Marcos Cruz (programandala.net), 2015, 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( vector-loop-benchmark )

  \ Credit:
  \
  \ Code adapted from: Forth Dimensions (volume 17, number 4,
  \ page 11, 1995-11).

  \ M. Edward Borasky, 1995-07-30

  \ Uses BEGIN ... UNTIL loops; all tested Forth have them
  \ Some small Forth are missing DO ... LOOP or FOR ... NEXT

need bench{

1000 constant vsize  \ vector size

: vector ( n -- )
  \ make an array
  \ compiling, reserve memory
  create  cells allot
  \ executing, compute address
  does> ( index -- address ) ( index pfa ) swap cells + ;

vsize vector vec1  vsize vector vec2  vsize vector vec3

: vecload ( -- ) \ put some stuff into the vectors
  0 begin
     dup vec1 dup !               \ vec1 gets its own address
     dup vec2 dup negate swap !   \ vec2 gets negated address
     1+ dup vsize =
  until  drop ;

: loop0 ( -- ) \ null loop
  0 begin  1+ dup vsize =  until  drop ;  -->

( vector-loop-benchmark )

: loop1 ( -- ) \ vector add
  0 begin
     dup vec1 @ over vec2 @ + over vec3 !
     1+ dup vsize =
  until  drop ;

: loop2 ( -- ) \ vector multiply
  0 begin
     dup vec1 @ over vec2 @ * over vec3 !
     1+ dup vsize =
  until  drop ;

: loop3 ( -- ) \ vector divide
  0 begin
    dup vec1 @ over vec2 @ / over vec3 !
    1+ dup vsize =
  until  drop ;  -->

( vector-loop-benchmark )

: loop4 ( -- ) \ vector scale
  0 begin
     dup vec1 @ 10000 10000 */ over vec2 !  1+ dup vsize =
  until  drop ;

1000 constant reps  \ repetitions

: bench0 ( -- ) \ benchmark loop0
  bench{ 0 begin  loop0 1+ dup reps =  until  drop }bench.
  ." Vector No-Op" cr ;

: bench1 ( -- ) \ benchmark loop1
  bench{ 0 begin  loop1 1+ dup reps =  until  drop }bench.
  ." Vector +    " cr ;

: bench2 ( -- ) \ benchmark loop2
  bench{ 0 begin  loop2 1+ dup reps =  until  drop }bench.
  ." Vector *    " cr ;

-->

( vector-loop-benchmark )

: bench3 ( -- ) \ benchmark loop3
  bench{ 0 begin  loop3 1+ dup reps =  until  drop }bench.
  ." Vector /    " cr ;

: bench4 ( -- ) \ benchmark loop4
  bench{ 0 begin  loop4 1+ dup reps =  until  drop }bench.
  ." Vector */   " cr ;

: vector-loop-benchmark ( -- )
  cr ." Vector Loop Benchmark:" cr
  vecload  cr bench0 bench1 bench2 bench3 bench4 ;

  cr
  \  <------------------------------>
  .( To run the vector loop) cr
  .( benchmarks type:) cr
  .(   vector-loop-benchmarks ) cr

  \ 2015-12-24:
  \
  \ Benchmark     Frames (1 frame = 50th of second)
  \ ---------     -----------------------------------
  \               ITC           DTC
  \               ------        -------------
  \ Vector noop    10919 (1.0)    9033 (0.82)
  \ Vector +       58650 (1.0)   47462 (0.80)
  \ Vector *      107770 (1.0)   91611 (0.85)
  \ Vector /      149002 (1.0)  127495 (0.85)
  \ Vector */     178854 (1.0)  154480 (0.86)

  \ 2016-03-16:
  \
  \ Benchmark     Frames (1 frame = 50th of second)
  \ ---------     -----------------------------------
  \               jp pushhl        push hl + jp (ix) [1]
  \               ------------     ---------------------
  \ Vector noop     9033 (1.0)       8943 (0.99)
  \ Vector +       47461 (1.0)      47177 (0.99)
  \ Vector *       91920 (1.0)      91153 (0.99)
  \ Vector /      127496 (1.0)     126783 (0.99)
  \ Vector */     155192 (1.0)     154364 (0.99)

  \ [1] Changed only in the kernel.

  \                    Frames (1 frame = 50th of second)
  \                    ----------------------------------
  \ Date       Vector:   noop      +      *      /     */
  \ ----------         ------ ------ ------ ------ ------
  \ 2017-04-27           8214  45270  89108 125102 152674

  \ ===========================================================
  \ Change log

  \ 2016-12-30: Compact the code, saving one block.
  \
  \ 2017-04-27: Rename the file in order to move the code to
  \ the "workbench" disk image. Display title.

  \ vim: filetype=soloforth
