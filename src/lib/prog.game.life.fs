  \ prog.game.life.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ XXX UNDER DEVELOPMENT -- not ready yet

  \ Last modified: 201709091154
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Conway's Game of Life, or Occam's Razor Dulled

  \ ===========================================================
  \ Authors

  \ Original ANS Forth version: Copyright (C) 1995 Leo Wong.
  \
  \ Version for kForth: K. Myneny, 2001-12-26.
  \
  \ Version for Solo Forth: Marcos Cruz (programandala.net),
  \ 2015, 2016, 2017.

  \ ===========================================================
  \ Credit

  \ Code adapted from kForth. Original Credit:

  \ The original ANS Forth version by Leo Wong (see bottom) has
  \ been modified slightly to allow it to run under kForth.
  \ Also, delays have been changed from 1000 ms to 100 ms for
  \ faster update --- K. Myneni, 12-26-2001
  \
  \ 950724 + 970703 +
  \ Copyright 1995 Leo Wong
  \ hello at albany dot net
  \ http://www.albany.net/~hello/

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( life )

  \ XXX FIXME -- works in Gforth, but freezes here.

need ms need c+! need 2/

1 CHARS CCONSTANT /Char

  \ the universal pattern
32 CCONSTANT How-Deep  24 CCONSTANT How-Wide

How-Wide How-Deep * CONSTANT Homes

  \ world wrap
: World ( "name" -- )
  CREATE  Homes CHARS ALLOT
  DOES> ( u -- c-addr )
    ( u dfa ) SWAP Homes +  Homes MOD  CHARS + ;

World old  World new

  \ biostatistics

  \ begin hexadecimal numbering
  \ hex xy : x holds life , y holds neighbors count

$10 CCONSTANT Alive  \ 0y = not alive

  \ Conway's rules:
  \ a life depends on the number of its next-door neighbors

  \ it dies if it has fewer than 2 neighbors
: Lonely ( c -- f ) $12 < ;

  \ it dies if it has more than 3 neighbors
: Crowded ( c -- f ) $13 > ;

: -Sustaining ( c -- f ) DUP Lonely  SWAP Crowded  OR ;

  \ it is born if it has exactly 3 neighbors
: Quickening ( c -- f ) $03 = ;

-->

( life )

  \ compass points
: N ( i -- j ) How-Wide - ;
: S ( i -- j ) How-Wide + ;
: E ( i -- j ) 1+ ;
: W ( i -- j ) 1- ;

  \ census
: Home+! ( -1|1 i -- ) >R  Alive *  R> new C+! ;

: Neighbors+! ( -1|0|1 i -- )
  2DUP N W new C+!  2DUP N new C+!  2DUP N E new C+!
  2DUP   W new C+! (     i      )   2DUP   E new C+!
  2DUP S W new C+!  2DUP S new C+!       S E new C+! ;

: Bureau-of-Vital-Statistics ( -1|1 i -- )
  2DUP Home+!  Neighbors+! ;

  \ mortal coils
'?' CCONSTANT Soul  BL CCONSTANT Body

-->

( life )

: Home ( c i -- ) How-Wide /MOD AT-XY  EMIT ;

: Is-Born ( i -- )
  Soul OVER Home  1 SWAP Bureau-of-Vital-Statistics ;

: Dies ( i -- )
  Body OVER Home  -1 SWAP Bureau-of-Vital-Statistics ;

: One ( c-addr -- i ) 0 old -  /Char / ;

: there ( -- ) How-Wide 1- 0 AT-XY ;

: Everything ( -- )
  0 old  Homes
  BEGIN  DUP
  WHILE  OVER C@  DUP Alive AND
     IF   -Sustaining IF  OVER One Dies     THEN
     ELSE  Quickening IF  OVER One Is-Born  THEN THEN
     1 /STRING
  REPEAT  2DROP  there ;  -->

( life )

  \ in the beginning
: Void ( -- ) 0 old  Homes BLANK ;

  \ spirit
: Voice ( -- c-addr u )
  PAGE ." Say: "  0 new  DUP Homes ACCEPT ;

  \ subtlety
: Serpent ( -- )
  0 2 AT-XY  ." Press a key for knowledge."  KEY DROP
  0 2 AT-XY  ." Press space to re-start, Esc to escape life." ;

  \ the primal state
: Innocence ( -- )
  Homes 0 ?DO  I new C@  Alive /  I Neighbors+!  LOOP ;

  \ children become parents
: Passes ( -- ) 0 new  0 old  Homes  CMOVE ;

-->

( life )

  \ a garden
: Paradise ( c-addr u -- )
  >R  How-Deep How-Wide *  How-Deep 2 MOD 0=  How-Wide AND -
  R@  -  2/  old
  R>  CMOVE
  0 old  Homes 0
  ?DO  COUNT BL <>
      DUP IF  Soul I Home  THEN  Alive AND  I new C!
  LOOP  DROP  Serpent Innocence Passes ;

-->

( life )

: Creation ( -- ) Void Voice Paradise ;

  \ the human element

100 CCONSTANT Ideas
: Dreams ( -- ) Ideas MS ;

100 CCONSTANT Images
: Meditation ( -- ) Images MS ;

  \ free will
: Action ( -- c )
  KEY? DUP
  IF  DROP KEY  DUP BL = IF  Creation  THEN  THEN ;

  \ environmental dependence
7 CCONSTANT Escape

  \ history
: Goes-On ( -- )
  BEGIN  Everything Passes  Dreams Action Meditation
         Escape = UNTIL ;

  \ a vision
: Life ( -- ) Creation Goes-On ;

  \ Life

: run-message ( -- ) cr ." Type LIFE to run" cr ;

run-message

  \ ===========================================================
  \ Change log

  \ 2015: Start.
  \
  \ 2016-04-03: Header reorganized after the original credit.
  \
  \ 2016-04-24: Remove `char`, which has been moved to the
  \ library. Change the stack notation.
  \
  \ 2016-05-02: Compact two blocks to save space in the
  \ library.
  \
  \ 2017-04-26: Add `run-message`. Use `cconstant`. Replace
  \ `do` with `?do`.
  \
  \ 2017-09-09: Update notation "pfa" to the standard "dfa".

  \ vim: filetype=soloforth
