  \ define.deferred.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201804111740
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words related to deferred words.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( deferred defers defer@ action-of )

unneeding deferred

?\ : deferred ( xt "name" -- ) defer latest name> defer! ;

  \ doc{
  \
  \ deferred ( xt "name" -- )
  \
  \ Create a deferred word _name_ that will execute _xt_.
  \ Therefore ``xt deferred name`` is equivalent to ``defer
  \ name  xt ' name defer!``.
  \
  \ See: `defer`, `defer!`.
  \
  \ }doc

unneeding defers ?( need defer@

: defers ' defer@ compile, ; immediate ?)
  \ Interpretation: ( "name" -- )
  \ Compilation:    ( "name" -- )
  \ Run-time:       ( -- )

  \ doc{
  \
  \ defers
  \   Interpretation: ( "name" -- )
  \   Compilation:    ( "name" -- )
  \   Run-time:       ( -- )
  \
  \ Compile the present contents of the deferred word "name"
  \ into the current definition. I.e. this produces static
  \ binding as if "name" was not deferred.
  \
  \ ``defers`` is an `immediate` word.
  \
  \ Origin: Gforth.
  \
  \ See: `defer`, `defer@`, `action-of`, `compile,`.
  \
  \ }doc

  \ Credit:
  \
  \ Code adapted from Afera.

unneeding defer@ ?\ : defer@ ( xt1 -- xt2 ) >action @ ;

  \ doc{
  \
  \ defer@ ( xt1 -- xt2 ) "defer-fetch"
  \
  \ Return the word _xt2_ currently associated to the deferred
  \ word _xt1_.
  \
  \ Origin: Forth-2012 (CORE EXT).
  \
  \ See: `defer!`, `defer`, `>action`.
  \
  \ }doc

unneeding deferred? ?\ : deferred? ( xt -- f ) c@ $C3 = ;

  \ doc{
  \
  \ deferred? ( xt -- f ) "deferred-question"
  \
  \ Is _xt_ a deferred word?
  \
  \ NOTE: The code of a deferred word starts with a Z80 jump
  \ ($C3) to the word it's associated to. This is what
  \ ``deferred?`` checks.
  \
  \ See: `defer`, `defer@`, `action-of`.
  \
  \ }doc

unneeding action-of ?( need defer@

: action-of
  \ Interpretation: ( "name" -- xt )
  \ Compilation:    ( "name" -- )
  \ Run-time:       ( -- xt )
  ' compiling? if   postpone literal postpone defer@
               else defer@ then ; immediate ?)
  \ doc{
  \
  \ action-of ( -- )
  \   Interpretation: ( "name" -- xt )
  \   Compilation:    ( "name" -- )
  \   Run-time:       ( -- xt )
  \

  \ .Interpretation
  \ Parse _name_, which is a word defined by
  \ `defer`. Return _xt_, which is the execution token that
  \ name is set to execute.
  \
  \ .Compilation
  \ Parse _name_, which is a word defined by
  \ `defer`. Append the runtime semantics given below to the
  \ current definition.
  \
  \ .Runtime
  \ Return _xt_, which is the execution token that
  \ name is set to execute.
  \
  \ ``action-of`` is an `immediate` word.
  \
  \ Origin: Forth-2012 (CORE EXT).
  \
  \ See: `defer@`, `defers`.
  \
  \ }doc

  \ Credit:
  \
  \ Code adapted from Afera.

( <is> [is] is  )

  \ Credit:
  \
  \ Code adapted from Afera.

unneeding <is> ?\ : <is> ( xt "name" -- ) ' defer! ;

  \ doc{
  \
  \ <is> ( xt "name" -- ) "less-is"

  \ Set _name_, which was defined by `defer`, to execute _xt_.
  \
  \ ``<is>`` is a factor of `is`.
  \
  \ Origin: Gforth.
  \
  \ See: `[is]`.
  \
  \ }doc


unneeding [is] ?(

: [is]
  \ Compilation: ( "name" -- )
  \ Run-time: ( xt -- )
  postpone ['] postpone defer! ; immediate compile-only ?)

  \ doc{
  \
  \ [is] "bracket-is"
  \   Compilation: ( xt "name" -- )
  \   Run-time:    ( xt -- )

  \ Compilation: ( "name" -- )
  \
  \ Append  the  run-time semantics given below to the current
  \ definition.
  \
  \ Run-time: ( xt -- )
  \
  \ Set _name_, which was defined by `defer`, to execute _xt_.
  \
  \ ``[is]`` is an `immediate` and `compile-only` factor of
  \ `is`.
  \
  \ Origin: Gforth.
  \
  \ See: `<is>`.
  \
  \ }doc

unneeding is ?( need [is] need <is>

: is
  \ Interpretation: ( xt "name" -- )
  \ Compilation: ( "name" -- )
  \ Run-time: ( xt -- )
  compiling? if postpone [is] else <is> then ; immediate ?)

  \ doc{
  \
  \ is
  \   Interpretation: ( xt "name" -- )
  \   Compilation:    ( "name" -- )
  \   Run-time:       ( xt -- )

  \ Interpretation: ( xt "name" -- )
  \
  \ Set _name_, which was defined by `defer`, to execute _xt_.
  \
  \ Compilation: ( "name" -- )
  \
  \ Append  the  run-time semantics given below to the current
  \ definition.
  \
  \ Run-time: ( xt -- )
  \
  \ Set _name_, which was defined by `defer`, to execute _xt_.
  \
  \ WARNING: ``is`` is a state-smart word.
  \
  \ Origin: Forth-2012 (CORE EXT).
  \
  \ See: `[is]`, `<is>`, `state`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-04-17: Added `deferred`, using the old definition of
  \ `alias`.
  \
  \ 2016-05-04: Move `defer@` from the kernel, document most
  \ words, compact the blocks.
  \
  \ 2016-08-05: Improve conditional compilation of `<is>`,
  \ `[is]` and `is`.
  \
  \ 2017-03-14: Improve documuntation.  Improve needing of
  \ `<is>`, `[is]` and `is`. Update name: `>defer` to
  \ `>action`.
  \
  \ 2017-05-07: Improve documentation.
  \
  \ 2017-12-20: Fix needing of `defer@`. Improve documentation.
  \
  \ 2018-01-11: Update layout. Improve documentation.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-08: Add words' pronunciaton.
  \
  \ 2018-04-11: Document `is`, `[is]`, and `<is>`.

  \ vim: filetype=soloforth
