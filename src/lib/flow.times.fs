  \ flow.times.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201703132019
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ `times` and `dtimes`: control flow structures which execute
  \ _n_ or _d_ times the next word compiled.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ Credit

  \ `times` was inspired by cmForth's `repeats`.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( times dtimes )

[unneeded] times ?(

variable times-xt  \ the _xt_ executed by `times`

: times ( u -- )
  rp@ @  dup cell+ rp@ !  @ times-xt !
  0 ?do  times-xt perform  loop ; compile-only ?)

  \ doc{
  \
  \ times ( u -- )
  \
  \ Repeat the next compiled instruction _u_ times.  If _u_ is
  \ zero, continue executing the following instruction.
  \
  \ ``times`` is useful to implement complicated math
  \ operations, like shifts, multiply, divide and square root,
  \ from appropriate math step instructions.  It is also useful
  \ in repeating auto-indexing memory instructions.
  \
  \ This structure is not nestable.
  \
  \ Usage example:

  \ ----
  \ : blink ( -- ) 7 0 ?do  i border  loop  0 border ;
  \ : blinking ( -- ) 100 times blink  ." Done" cr ;
  \ ----

  \ Origin: cmForth's ``repeats``.
  \
  \ See also: `dtimes`, `executions`.
  \
  \ }doc

[unneeded] dtimes ?( need dfor need d-

variable dtimes-xt  \ the _xt_ executed by `dtimes`

: dtimes ( d -- )
  rp@ @  dup cell+ rp@ !  @ dtimes-xt !
  2dup or if    1. d- dfor  dtimes-xt perform  dstep  exit
          then  2drop
 ; compile-only ?)

  \ doc{
  \
  \ dtimes ( d -- )
  \
  \ Repeat the next compiled instruction _d_ times.  If _d_ is
  \ zero, continue executing the following instruction.
  \
  \ This structure is not nestable.
  \
  \ Usage example:

  \ ----
  \ : blink ( -- ) 7 0 ?do  i border  loop  0 border ;
  \ : blinking ( -- ) 100000. dtimes blink  ." Done" cr ;
  \ ----

  \ See also: `times`, `executions`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015..2016: Several drafts, with different behaviours
  \ during compilation.
  \
  \ 2016-04-16: Finished. Simplest version. Documented.
  \
  \ 2016-11-26: Move `dtimes` from its own module and finish it
  \ after `times` (do nothing if the parameter is zero). Finish
  \ documentation.
  \
  \ 2017-01-19: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-02-19: Replace `do`, which has been moved to the
  \ library, with `?do`.
  \
  \ 2017-03-13: Improve documentation.

  \ vim: filetype=soloforth
