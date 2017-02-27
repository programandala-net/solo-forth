  \ game.toe.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702220020

  \ -----------------------------------------------------------
  \ Description

  \ The sample game Toe.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ Latest changes

  \ 2017-02-19: Replace `do`, which has been moved to the
  \ library, with `?do`.

( toe )

  \ XXX UNDER DEVELOPMENT

  \ 2015-11-24: Changes.

  \ Credit:
  \
  \ Based on code from Leo Brodie's _Starting Forth_, ANSized
  \ by Benjamin Hoyt in 1997.

9 constant squares

1 constant player-x  2 constant player-o

create board  squares allot

: clear ( -- ) board squares erase ;  clear

: >square ( square -- ca ) board + ;
: square@ ( square -- c ) >square c@ ;
: square! ( c square -- ) >square c! ;

: bar ( -- ) ." | " ;
: dashes ( -- ) cr  9 0 ?do  '-' emit  loop cr ;

: .player-mark ( player -- )
  player-x = if ." x " else ." o " then ;

: .box-contents ( n -- )
  ?dup if  .player-mark  else  2 spaces  then ;

: .box ( square -- ) square@  .box-contents ;

-->

( toe )

: display ( -- )
  home
  squares 0 ?do
    i if    i 3 mod  0= if  dashes  else  bar  then
      then  i .box
  loop  cr ;

: limited ( square -- square' ) 0 max squares min ;

: play ( square player -- ) swap 1- limited square! ;

: x ( square -- ) player-x play  display ;
: o ( square -- ) player-o play  display ;

  \ vim: filetype=soloforth
