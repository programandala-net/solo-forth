  \ define.deferred.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702220020

  \ -----------------------------------------------------------
  \ Description

  \ Words related to deferred words.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ Latest changes

  \ 2016-04-17: Added `deferred`, using the old definition of
  \ `alias`.
  \
  \ 2016-05-04: Move `defer@` from the kernel, document most
  \ words, compact the blocks.
  \
  \ 2016-08-05: Improve conditional compilation of `<is>`,
  \ `[is]` and `is`.

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
  \ }doc

[unneeded] defers
?\ : defers ( "name" -- ) ' defer@ compile, ; immediate

  \ doc{
  \
  \ defers ( Compilation: "name" -- )
  \
  \ Compile the present contents of the deferred word "name"
  \ into the current definition. I.e. this produces static
  \ binding as if "name" was not deferred.
  \
  \ Origin: Gforth.
  \
  \ }doc

  \ Credit:
  \
  \ Code adapted from Afera.

[unneeded] defer@ ?\ : defer@ ( xt1 -- xt2 ) >defer @ ;

  \ doc{
  \
  \ defer@ ( xt1 -- xt2 )
  \
  \ Return the word _xt2_ currently associated to the deferred
  \ word _xt1_.
  \
  \ Origin: Forth-2012 (CORE EXT).
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

: action-of ( Interpretation: "name" -- xt )
             ( Compilation:    "name" -- )
             ( Runtime:        -- xt )
  ' compiling? if    postpone literal postpone defer@
               else  defer@  then ; immediate
  \ doc{
  \
  \ action-of ( -- )
  \   ( Interpretation: "name" -- xt )
  \   ( Compilation:    "name" -- )
  \   ( Runtime:        -- xt )
  \
  \ Return the code field address of a deferred word.
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

[needed] [is]
?\ : <is> ( xt "name" -- ) ' defer! ;  [needed] <is> ?exit

: [is] ( xt "name" -- )
  postpone ['] postpone defer! ; immediate compile-only

[needed] [is] ?exit

: is ( xt "name" -- )
  compiling? if  postpone [is]  else  <is>  then ; immediate

  \ vim: filetype=soloforth
