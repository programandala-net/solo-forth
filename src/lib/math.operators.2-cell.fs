  \ math.operators.2-cell.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201705061656
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Double-cell operators.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( ud* d* )

  \ Credit:
  \
  \ Code of `ud*` from Z88 CamelForth.

[unneeded] ud*
?\ : ud* ( ud1 u2 -- ud3 ) dup >r um* drop  swap r> um* rot + ;

  \ doc{
  \
  \ ud* ( ud1 ud2 -- ud3 )
  \
  \ Multiply _ud1_ by _ud2_ giving the product _ud3_.
  \
  \ See also: `d*`, `um*`, `m*`, `*`.
  \
  \ }doc

[unneeded] d* ?(
: d* ( d|ud1 d|ud2 -- d|ud3 )
  >r swap >r 2dup um* rot r> * + rot r> * + ; ?)

  \ Credit:
  \
  \ Code of `d*` from DX-Forth 4.13.

  \ This implementation uses 31 bytes.
  \ Relative speed: 1.0000

  \ doc{
  \
  \ d* ( d|ud1 d|ud2 -- d|ud3 )
  \
  \ Multiply _d1|ud1_ by _d2|ud2_ giving the product _d3|ud3_.
  \
  \ See also: `ud*`, `um*`, `m*`, `*`.
  \
  \ }doc

  \ --------------------------------------------
  \ Alternative implementation.
  \
  \ Credit:
  \
  \ Adapted from code written by Robert L. Smith,
  \ published on Forth Dimensions (volume 4, number 1, page 3,
  \ 1982-05).
  \
  \ This implementation uses 36 bytes.
  \ Relative speed: 1.0582
  \
  \ : d* ( d1 d2 -- d3 )
  \  over 4 pick um*  5 roll 3 roll * +  2swap * + ;

  \ --------------------------------------------
  \ Alternative implementation.

  \ Credit:
  \
  \ Code by Wil Baden, published on Forth Dimensions (volume
  \ 19, number 6, page 33, 1998-04).

  \ This implementation uses 30 bytes.
  \ Relative speed: 1.0008

  \ : d* ( d1 d2 -- d3 )
  \   >r swap >r            ( d1lo d2lo ) ( R: d2hi d1hi )
  \   2dup um* 2swap        ( d1lo*d2lo d1lo d2lo )
  \   r> * swap r> * + + ; ( d1*d2 ) ( R: )

( du/mod )

  \ Credit:
  \
  \ Code by Wil Baden, published on Forth Dimensions (volume
  \ 19, number 6, page 34, 1998-04).

need tum* need t+ need t- need tum/ need d2* need lshift

: normalize-divisor ( d1 -- d1' shift )
  0 >r begin  dup 0< while  d2*  r> 1+ >r  repeat  r> ;

  \ XXX TODO rename as `ud/mod`?
  \ XXX TODO stack comments

: du/mod ( ud1 ud2 -- ud3 ud4 )

  ?dup 0= if
    \ there is a leading zero "digit" in divisor
    >r  0 r@ um/mod  r> swap >r  um/mod  0 swap r>  exit
  then

  normalize-divisor dup >r rot rot 2>r
  1 swap lshift tum*
    \ normalize divisor and dividend

  dup  r@ = if   -1  else  2dup  r@ um/mod nip  then
    \ guess leading "digit" of quotient

  2r@  rot dup >r  tum*  t-
    \ multiply divisor by trial quot and substract from
    \ dividend

  dup 0< if  r> 1-  2r@  rot >r  0 t+
    \ if negative, decrement quot and add to dividend

    dup 0< if  r> 1-  2r@  rot >r  0 t+  then
    \ if still negative, do it one more time

  then

  r> 2r> 2drop  1 r>  rot >r  lshift tum/  r> 0 ;
    \ undo nurmalization of dividend to get remainder

  \ doc{
  \
  \ du/mod ( ud1 ud2 -- ud3 ud4 )
  \
  \ Divide _ud1_ by _ud2_, giving the remainder _ud3_ and
  \ the quotient _ud4_.
  \
  \ See also: `um/mod`, `/mod` ,`*/mod`.
  \
  \ }doc

( d0= d0< d< du< )

  \ Credit:
  \
  \ Code from DZX-Forth.

[unneeded] d0= ?(

code d0= ( d -- f )
  E1 c, D1 c, 19 c, 78 04 + c, B0 05 + c,
  \ pop hl
  \ pop de
  \ add hl,de
  \ ld a,h
  \ or l
  C2 c, ' false , 2B c, jppushhl, end-code
  \ jp nz,false_
  \ dec hl ; HL = true
  \ _jp_pushhl

  \ doc{
  \
  \ d0= ( d -- f )
  \
  \ _f_ is true if and only if _d_ is equal to zero.
  \
  \ ``d0=`` is written in Z80. Its equivalent definition in
  \ Forth is the following:

  \ ----
  \ : d0= ( d -- f ) + 0= ;
  \ ----

  \ See also: `0=`.
  \
  \ }doc

[unneeded] d0< ?\ : d0< ( d -- f ) nip 0< ;

  \ doc{
  \
  \ d0< ( d -- f )
  \
  \ _f_ is true if and only if _d_ is less than zero.
  \
  \ See also: `0<`.
  \
  \ }doc

[unneeded] d< ?(

need 2nip

: d< ( d1 d2 -- f )
  rot 2dup = if  2drop u< exit  then  2nip > ; ?)

  \ doc{
  \
  \ d< ( d1 d2 -- f )
  \
  \ _f_ is true only if and only if _d1_ is less than _d2_.
  \
  \ Origin: Forth-79 (Double Number Word Set), Forth-83 (Double
  \ Number Extension Word Set), Forth-94 (DOUBLE EXT),
  \ Forth-2012 (DOUBLE EXT).
  \
  \ See also: `du<`, `<`, `dmin`.
  \
  \ }doc

[unneeded] du< ?(

  \ XXX TODO rewrite in Z80

: du< ( ud1 ud2 -- f )
  rot swap 2dup
  u<  if  2drop 2drop true   exit  then
  -   if  2drop       false  exit  then  u< ; ?)

  \ doc{
  \
  \ du< ( ud1 ud2 -- f )
  \
  \ _f_ is true only if and only if _du1_ is less than _du2_.
  \
  \ Origin: Forth-79 (Double Number Word Set), Forth-83 (Double
  \ Number Extension Word Set), Forth-94 (DOUBLE EXT),
  \ Forth-2012 (DOUBLE EXT).
  \
  \ See also: `d<`, `<`, `dmin`.
  \
  \ }doc

( d= d<> dmin dmax )

[unneeded] d= ?\ : d= ( xd1 xd2 -- f ) d<> 0= ;
  \ XXX TODO -- rewrite in Z80

  \ doc{
  \
  \ d= ( xd1 xd2 -- f )
  \
  \ _f_ is true if and only if _xd1_ is equal to _xd2_.
  \
  \ See also: `=`.
  \
  \ }doc

[unneeded] d<>
?\ : d<> ( d1 d2 -- f ) rot <> if  2drop true exit  then  <> ;
  \ XXX TODO -- rewrite in Z80

  \ XXX OLD
  \ XXX TODO benchmark
  \ : d= ( xd1 xd2 -- f ) rot = >r = r> and ;
  \ : d<> ( xd1 xd2 -- f ) d= 0= ;

  \ doc{
  \
  \ d<> ( xd1 xd2 -- f )
  \
  \ _f_ is true if and only if _xd1_ is not bit-for-bit the
  \ same as _xd2_.
  \
  \ See also: `<>`.
  \
  \ }doc

[unneeded] dmin ?(
: dmin ( d1 d2 -- d3 )
  2over 2over d< 0= if  2swap  then  2drop ; ?)
  \ XXX TODO -- use `d>` when available

  \ Credit:
  \
  \ Code from DZX-Forth.

  \ doc{
  \
  \ dmin ( d1 d2 -- d3 )
  \
  \ _d3_ is the greater of _d1_ and _d2_.
  \
  \ Origin: Forth-79 (Double Number Word Set), Forth-83 (Double
  \ Number Extension Word Set), Forth-94 (DOUBLE), Forth-2012
  \ (DOUBLE).
  \
  \ See also: `dmax`, `min`, `umin`.
  \
  \ }doc

[unneeded] dmax ?(
: dmax ( d1 d2 -- d1 | d2 )
  2over 2over d< if  2swap  then  2drop ; ?)

  \ Credit:
  \
  \ Code from DZX-Forth.

  \ doc{
  \
  \ dmax ( d1 d2 -- d3 )
  \
  \ _d3_ is the lesser of _d1_ and _d2_.
  \
  \ Origin: Forth-79 (Double Number Word Set), Forth-83 (Double
  \ Number Extension Word Set), Forth-94 (DOUBLE), Forth-2012
  \ (DOUBLE).
  \
  \ See also: `dmin`, `max`, `umax`.
  \
  \ }doc

( d- d2* d2/ )

[unneeded] d- ?( code d- ( d1|ud1 d2|ud2 -- d3|ud3 )

  D1 c,  D9 c,  D1 c,  D9 c,  E1 c,  D9 c,  E1 c,
  \ de pop            \ DE=d2hi
  \ exx  de pop       \ DE'=d2lo
  \ exx  hl pop       \ HL=d1hi,DE=d2hi
  \ exx  hl pop       \ HL'=d1lo
  A0 07 + c,  ED c, 52 c,  E5 c,  D9 c,  ED c,  52 c,
  \ de subp  hl push  \ 2OS=d1lo-d2lo
  \ exx de  sbcp      \ HL=d1hi-d2hi-cy
  jppushhl, end-code ?)

  \ Credit:
  \
  \ Code adapted from Z88 CamelForth.

  \ doc{
  \
  \ d- ( d1|ud1 d2|ud2 -- d3|ud3 )
  \
  \ Subtract _d2|ud2_ from _d1|ud1_, giving the difference
  \ _d3|ud3_.
  \
  \ Origin: Forth-79 (Double Number Word Set), Forth-83 (Double
  \ Number Extension Word Set), Forth-94 (DOUBLE), Forth-2012
  \ (DOUBLE).
  \
  \ See also: `d+`, `-`, `dmin`.
  \
  \ }doc

[unneeded] d2* ?( code d2* ( xd1 -- xd2 )

  D1 c, E1 c,  29 c,  CB c, 13 c,  CB c, 12 c,  EB c,
    \ pop de
    \ pop hl
    \ add hl,hl
    \ rl e
    \ rl d
    \ ex de,hl
  pushhlde jp, end-code ?)
    \ jp pushhlde

  \ Credit:
  \
  \ Code converted to Z80 from the 8080 version of DZX-Forth.

  \ doc{
  \
  \ d2* ( xd1 -- xd2 )
  \
  \ _xd2_ is the result of shifting _xd1_ one bit toward the
  \ most-significant bit, filling the vacated bit with zero.
  \
  \ Origin: Forth-94 (DOUBLE), Forth-2012 (DOUBLE).
  \
  \ See also: `d2/`, `2*`, `lshift`.
  \
  \ }doc

[unneeded] d2/ ?( code d2/ ( xd1 -- xd2 )

  E1 c, D1 c,  CB c, 2C c,  CB c, 1C c,  CB c, 1D c,
    \ pop hl
    \ pop de
    \ sra h
    \ rr h
    \ rr l
  CB c, 1A c,  CB c, 1B c,  EB c,  pushhlde jp, end-code ?)
    \ rr d
    \ rr e
    \ ex de,hl
    \ jp pushhlde

  \ Credit:
  \
  \ Code converted to Z80 from the 8080 version of DZX-Forth.

  \ doc{
  \
  \ d2/ ( xd1 -- xd2 )
  \
  \ _xd2_ is the result of shifting _xd1_ one bit toward the
  \ least-significant bit, leaving the most-significant bit
  \ unchanged.
  \
  \ Origin: Forth-94 (DOUBLE), Forth-2012 (DOUBLE).
  \
  \ See also: `d2*`, `2/`, `rshift`.
  \
  \ }doc

( dxor dor dand d10* )

  \ Credit:
  \
  \ Code of `dxor`, `dor` and `dand` written by Everett F.
  \ Carter, published on Forth Dimensions (volume 16, number 2,
  \ page 17, 1994-08).

[unneeded] dxor
?\ : dxor ( xd1 xd2 -- xd3 ) rot xor -rot xor swap ;

  \ doc{
  \
  \ dxor ( xd1 xd2 -- xd3 )
  \
  \ _xd3_ is the bit-by-bit exclusive-or of _xd1_ and _xd2_.
  \
  \ See also: `xor`, `dor`, `dand`.
  \
  \ }doc

[unneeded] dor
?\ : dor ( xd1 xd2 -- xd3 ) rot or -rot or swap ;

  \ doc{
  \
  \ dor ( xd1 xd2 -- xd3 )
  \
  \ _xd3_ is the bit-by-bit inclusive-or of _xd1_ and _xd2_.
  \
  \ See also: `or`, `dxor`, `dand`.
  \
  \ }doc

[unneeded] dand
?\ : dand ( xd1 xd2 -- xd3 ) rot and -rot and swap ;

  \ doc{
  \
  \ dand ( xd1 xd2 -- xd3 )
  \
  \ _xd3_ is the bit-by-bit logical "and" of _xd1_ and _xd2_.
  \
  \ See also: `and`, `dor`, `dxor`.
  \
  \ }doc

[unneeded] d10*
?\ : d10* ( ud1 -- ud2 ) d2* 2dup d2* d2* d+ ;

  \ Credit:
  \
  \ Code of `d10*` from Pygmy Forth.

  \ doc{
  \
  \ d10* ( ud1 -- ud2 )
  \
  \ Multiply _ud1_ per 10, resulting _ud2_.
  \
  \ See also: `d2*`, `d*`, `2*`, `8*`.
  \
  \ }doc

( m+ )

  \ Credit:
  \
  \ Code adapted from Z88 CamelForth.

need assembler

code m+ ( d1|ud1 n -- d2|ud2 )
  exx,    \ save the Forth IP
  b pop,  \ n
  d pop,  \ d1 hi cell
  h pop,  \ d1 lo cell
  b addp, h push,
  c? rif  d inc, rthen  d push,
  exx,    \ restore the Forth IP
  jpnext, end-code

  \ doc{
  \
  \ m+ ( d1|ud1 n -- d2|ud2 )
  \
  \ Add _n_ to _d1|ud1_, giving the sum _d2|ud2_.
  \
  \ ``m+`` is written in Z80. An equivalent definition in Forth
  \ (1.48 slower, but 4 bytes smaller) is the following:

  \ ----
  \ : m+ ( d1|ud1 n -- d2|ud2 ) s>d d+ ;
  \ ----

  \ Origin: Forth-94 (DOUBLE) Forth-2012 (DOUBLE).
  \
  \ See also: `+`, `d+`.
  \
  \ }doc

( m*/ )


: m*/ ( d1 n1 +n2 -- d2 )
  >r s>d >r abs -rot s>d r> xor r> swap >r >r dabs
  rot tuck um* 2swap um* swap
  >r 0 d+ r> -rot i um/mod -rot r> um/mod -rot r>
  if    if  1 0 d+  then  dnegate
  else  drop  then ;

  \ Credit:
  \
  \ Code of `m*/` from Gforth 0.7.3.

  \ doc{
  \
  \ m*/ ( d1 n1 +n2 -- d2 )
  \
  \ Multiply _d1_ by _n1_ producing the triple-cell
  \ intermediate result _t_.  Divide _t_ by _+n2_ giving the
  \ double-cell quotient _d2_.
  \
  \ Origin: Forth-94 (DOUBLE), Forth-2012 (DOUBLE).
  \
  \ See also: `*/`, `m*`.
  \
  \ }doc

( dsqrt )

  \ Credit:
  \
  \ Original code by Wil Baden, published on Forth Dimensions
  \ 18/5 p. 29 (1997-01).

need q2* need d2* need d< need m+ need d- need 2rot
need 2nip need cell-bits

  \ XXX FIXME wrong results
  \
  \ It worked fine, but something got wrong
  \ Perhaps because of some wrong dependency?
  \ maybe `d<`?
  \
  \ It works in Gforth

: (dsqrt) ( d1 -- d2 d3 )
  0. 0.  ( radicand . remainder . root . )
  [ cell-bits ] cliteral 0 do
    \ cr .s  key drop  \ XXX INFORMER
    2>r q2* q2* 2r>  d2*
    2over 2over d2* 2swap
      \ cr .s ." d< ?"  \ XXX INFORMER
      d< if
      \ cr .s ." d<"  \ XXX INFORMER
      2dup 2>r d2* d- -1 m+ 2r>  1 m+
    then
  loop  2rot 2drop ;

: dsqrt ( d1 -- d2 ) (dsqrt) 2nip ;

  \ dsqrt ( d1 -- d2 )
  \
  \ Calculate integer square root _d2_ of radicand _d1_.
  \
  \ See also: `sqrt`.

  \ ===========================================================
  \ Change log

  \ 2015-11-13: Add `dsqrt`.
  \
  \ 2016-11-17: Use `?(` instead of `[if]`. Compact the module,
  \ saving 3 blocks.
  \
  \ 2016-11-26: Rewrite `d-` with Z80 opcodes, without
  \ `z80-asm`. Compact `d2*` and `d2/`, saving two blocks.
  \
  \ 2016-12-20: Rename `jppushhl` to `jppushhl,` and `jpnext`
  \ to `jpnext,`, after the change in the kernel.
  \
  \ 2016-12-30: Compact the code, saving two blocks. Don't
  \ compile flags as literals in `du<`, because `true` and
  \ `false` are code words.
  \
  \ 2017-01-02: Convert `m+` from `z80-asm` to `z80-asm,`.
  \
  \ 2017-01-05: Update `need z80-asm,` to `need assembler`.
  \
  \ 2017-01-13: Remove alternative implementation of `m*`, by
  \ by Robert L. Smith, published on Forth Dimensions (volume
  \ 4, number 1, page 3, 1982-05). It's much slower than the
  \ current version included in the kernel.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-03-29: Update needing of `cell-bits`.
  \
  \ 2017-04-04: Improve documentation.
  \
  \ 2017-04-20: Fix index line.
  \
  \ 2017-05-05: Improve documentation.
  \
  \ 2017-05-06: Rewrite `d0=` in Z80. Improve documentation.

  \ vim: filetype=soloforth
