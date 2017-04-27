  \ meta.benchmark.all.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201704271853
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ A wrapper to execute all main benchmarks.

( all-benchmarks )

  \ Credit:
  \
  \ Code adapted from: Forth Dimensions (volume 17, number 4,
  \ page 11, 1995-11).

need byte-magazine-benchmark
need interface-age-benchmark
need vector-loop-benchmark

: all-benchmarks ( n -- )
  dup byte-magazine-benchmark
      interface-age-benchmark
      vector-loop-benchmark ;

  \ doc{
  \
  \ all-benchmarks ( n -- )
  \
  \ Run all three main benchmarks: `byte-magazine-benchmark`,
  \ `interface-age-benchmark` and `vector-loop-benchmark`.
  \
  \ _n_ is the iterations done by `byte-magazine-benchmark` and
  \ `interface-age-benchmark`.
  \
  \ }doc

  \  <------------------------------>
  cr
  .( To run all main benchmarks) cr
  .( type:) cr
  .(   n all-benchmarks) cr
  .( where _n_ is the number of) cr
  .( iterations done by) cr
  .( 'byte-magazine-benchmark' and) cr
  .( and 'interface-age-benchmark'.) cr

  \ ===========================================================
  \ Change log

  \ 2017-04-27: Move `all-benchmarks` here from <benchmark.fs>, which
  \ is deleted. Add instructions. Fix code typo. Improve
  \ documentation.

  \ vim: filetype=soloforth
