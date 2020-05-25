  \ prog.game.towers_of_hanoi.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ XXX UNDER DEVELOPMENT -- not ready yet

  \ Last modified: 202005252109
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Towers of Hanoi puzzle.

  \ ===========================================================
  \ Authors

  \ Raul Deluth Miller, original algorithm, published on
  \ comp.lang.forth, 1994.

  \ Marcel Hendrix and Brad Eckert, published on
  \ comp.lang.forth, 2002-05-30.

  \ K. Myneni, modified for kForth., 2002-05-30.

  \ Marcos Cruz (programandala.net), version for Solo Forth,
  \ 2015, 2016, 2017, 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( hanoi )

only forth definitions

need ms need mode-64ao need allotted need recurse

wordlist dup constant hanoi-wordlist dup >order set-current

variable slowness  1000 slowness !
  \ ms delay between screen updates

3 constant pegs

create PegSPS  pegs cells allot
  \ pointers for three disk stacks

: PegSP ( peg -- addr ) cells PegSPS + ;
: PUSH  ( c peg -- )    PegSP tuck @ c!  1 chars swap +! ;
: POP   ( peg -- c )    PegSP -1 chars over +!  @ c@ ;

create PegStacks  30 chars allot
  \ stack area for up to 10 disks

: PegStack ( peg -- addr ) 10 * PegStacks + ;

: clear-peg ( peg -- ) dup PegStack  swap PegSP ! ;
: clear-pegs ( -- ) pegs 0 ?do  i clear-peg  loop ;

  \ : PegDepth ( peg -- depth ) dup PegSP @  swap PegStack - ;
  \ XXX OLD not needed

-->

( hanoi )

: show-disk ( level diameter peg -- )
  22 * 10 + over -  rot 10 swap - at-xy
  2* '*' emits ;

: show-peg   ( peg -- )
  dup >r PegStack
  BEGIN   r@ PegSP @ over <>
  WHILE   dup r@ PegStack - over c@ ( addr level diameter )
          r@ show-disk  char+
  REPEAT  drop rdrop ;

-->

( hanoi )

: maketab ( n1..nn n -- XXX ) \ XXX TODO stack effect
  create
    dup allotted over 1- + swap 0
    2dup <> if    ?do  dup >r c! r> 1-  loop
            else  2drop
            then  drop
  does>  + c@ ;

#3 base !
00 02 01 12 00 10 21 20  #8 maketab TO!
00 21 12 20 00 02 10 01  #8 maketab FRO!
decimal

-->

( hanoi )

: finished ( -- ) key drop 0 11 at-xy ." Stopped" cr abort ;

: show-pegs ( -- )
  page  pegs 0 ?do  i show-peg  loop  slowness @ ms
  key? if  finished  then ;

: move-ring ( ring -- ring )
  dup to! 3 / pop  over fro! 3 mod push show-pegs ;

: tower ( depth direction -- depth direction )
  swap 1- swap over
  IF    to!  recurse  to! move-ring fro! recurse  fro!
  ELSE  move-ring
  THEN  swap 1+ swap ;

-->

( hanoi )

: run ( depth -- )

  clear-pegs

  dup BEGIN ?dup WHILE 1- dup 0 push REPEAT
    \ stack up some disks

  show-pegs 1 tower 2drop
    \ move them

  0 11 at-xy ;

mode-64ao page
  \  <-------------------------->
  .( Towers of Hanoi) cr
  .( Type 'n run' to play where) cr
  .( 'n' is the number of disks.) cr
  \  <-------------------------->

  \ ===========================================================
  \ Change log

  \ 2015..2016: Start.
  \
  \ 2016-04-17: Added the requisite of `recurse`, which is not
  \ in the kernel anymore. Updated the file headers.
  \
  \ 2016-04-24: Remove `[char]`, which has been moved to the
  \ library.
  \
  \ 2017-02-19: Replace `do`, which has been moved to the
  \ library, with `?do`.
  \
  \ 2017-04-26: Check and update.
  \
  \ 2020-05-04: Update: `mode-64` -> `mode-64ao`.
  \
  \ 2020-05-05: Fix requirement.
  \
  \ 2020-05-25: Replace `r> drop` with `rdrop`.

  \ vim: filetype=soloforth
