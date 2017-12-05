  \ define.deferred.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201705071751
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words related to deferred words.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( deferred defers defer@ action-of )

[unneeded] deferred
?\ : deferred ( xt "name" -- ) defer latest name> defer! ;

  \ doc{
  \
  \ deferred ( xt "name" -- )
  \
  \ Create a deferred word _name_ that will execute _xt_.  The
  \ effect is the same than `defer name  xt ' name defer!`.
  \
  \ See: `defer`, `defer!`.
  \
  \ }doc

[unneeded] defers
?\ : defers ( "name" -- ) ' defer@ compile, ; immediate

  \ doc{
  \
  \ defers ( "name" -- )
  \
  \ Compile the present contents of the deferred word "name"
  \ into the current definition. I.e. this produces static
  \ binding as if "name" was not deferred.
  \
  \ ``defers`` is an `immediate` word.
  \
  \ Origin: Gforth.
  \
  \ }doc

  \ Credit:
  \
  \ Code adapted from Afera.

[unneeded] defer@ ?\ : defer@ ( xt1 -- xt2 ) >action @ ;

  \ doc{
  \
  \ defer@ ( xt1 -- xt2 )
  \
  \ Return the word _xt2_ currently associated to the deferred
  \ word _xt1_.
  \
  \ Origin: Forth-2012 (CORE EXT).
  \
  \ See: `defer!`, `defer`, `>action`.
  \
  \ }doc

[unneeded] deferred? ?\ : deferred? ( xt -- f ) c@ $C3 = ;

  \ doc{
  \
  \ deferred? ( xt -- f )
  \
  \ Is _xt_ a deferred word?
  \
  \ The code of a deferred word starts with a Z80 jump ($C3) to
  \ the word it's associated to.
  \
  \ }doc

[unneeded] action-of ?exit

: action-of
  \ Interpretation: ( "name" -- xt )
  \ Compilation:    ( "name" -- )
  \ Run-time:       ( -- xt )
  ' compiling? if    postpone literal postpone defer@
               else  defer@  then ; immediate
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
  \ }doc

  \ Credit:
  \
  \ Code adapted from Afera.

( <is> [is] is  )

  \ Credit:
  \
  \ Code adapted from Afera.

[unneeded] <is> ?\ : <is> ( xt "name" -- ) ' defer! ;

  \ XXX TODO -- Documentation.

[unneeded] [is] ?(
: [is] ( xt "name" -- )
  postpone ['] postpone defer! ; immediate compile-only ?)

  \ XXX TODO -- Documentation.

[unneeded] is ?( need [is] need <is>

: is ( xt "name" -- )
  compiling? if postpone [is] else <is> then ; immediate ?)

  \ XXX TODO -- Documentation.

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

  \ vim: filetype=soloforth
