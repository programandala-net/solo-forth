  \ benchmark.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702221224
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Generic benchmarking tools.
  \
  \ Specific benchmarks written during the development of Solo
  \ Forth, in order to choose between different implementation
  \ options, are in the file <development_benchmarks.fsb>.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( bench{ }bench }bench. bench. benched )

  \ Credit:
  \
  \ Code adapted from Forth Dimensions (volume 17, number 4
  \ page 11, 1995-11).

  \ System-dependent timing routines.

need reset-frames need frames@

: bench{ ( -- ) reset-frames ;

  \ doc{
  \
  \ bench{ ( -- )
  \
  \ Start timing, setting the system frames counter to zero.
  \
  \ See also: `}bench`, `reset-frames`.
  \
  \ }doc

: }bench ( -- d ) frames@ ;

  \ doc{
  \
  \ }bench ( -- d ) frames@ ;
  \
  \ Return the current value of the system frames counter.
  \
  \ See also: `bench{`, `frames@`, `bench.`, `}bench.`.
  \
  \ }doc

: bench. ( d -- ) 2dup d. ." frames (" 50 m/ nip . ." s) " ;

  \ doc{
  \
  \ bench. ( d -- )
  \
  \ Print the timing result _d_.
  \
  \ See also: `bench{`, `}bench`, `}bench.`.
  \
  \ }doc

: }bench. ( -- ) frames@ bench. ;

  \ doc{
  \
  \ }bench. ( -- )
  \
  \ Stop timing and print the result.
  \
  \ See also: `bench{`, `}bench`, `bench.`.
  \
  \ }doc

: benched ( xt n -- d )
  bench{ 0 ?do  dup execute  loop  }bench rot drop ;

  \ doc{
  \
  \ benched ( xt n -- d )
  \
  \ Execute _n_ times the benchmark _xt_ and return the timer
  \ result _d_.
  \
  \ See also: `bench{`, `}bench`, `benched.`.
  \
  \ }doc

: benched. ( xt n -- )
  bench{ 0 ?do  dup execute  loop  }bench. drop ;

  \ doc{
  \
  \ benched. ( xt n -- d )
  \
  \ Execute _n_ times the benchmark _xt_ and print the
  \ result.
  \
  \ See also: `bench{`, `}bench.`, `benched`.
  \
  \ }doc

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
      vector-loop-benchmarks ;

  \ doc{
  \
  \ all-benchmarks ( n -- )
  \
  \ Run all three main benchmarks (`byte-magazine-benchmark`,
  \ `interface-age-benchmark` and `vector-loop-benchmark`) _n_
  \ times each (if applicable).
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2017-02-19: Replace `do`, which has been moved to the
  \ library, with `?do`.
  \
  \ 2017-02-22: Improve documentation.

  \ vim: filetype=soloforth
