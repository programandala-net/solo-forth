  \ math.operators.1-cell.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702271829
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Single-cell operators.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( under+ +under )

[unneeded] under+ ?(

code under+ ( n1|u1 x n2|u2 -- n3|u3 x )
  D9 c, D1 c, C1 c, E1 c, 19 c, E5 c, C5 c, D9 c,
  \ exx
  \ pop de
  \ pop bc
  \ pop hl
  \ add hl,de
  \ push hl
  \ push bc
  \ exx
  jpnext, end-code ?)

  \ doc{
  \
  \ under+ ( n1|u1 x n2|u2 -- n3|u3 x )
  \
  \ Add _n2|u2_ to _n1|u2_, giving the sum _n3|u3_.
  \
  \ The word is written in Z80. This is an example
  \ implementation in Forth:
  \
  \ ----
  \ : under+ ( n1|u1 x n2|u2 -- n3|u3 x )
  \   rot + swap ;
  \ ----
  \
  \ Origin: Comus.
  \
  \ See also: `+under`.
  \
  \ }doc

[unneeded] +under ?(

code +under ( n1|u1 n2|u2 x -- n3|u3 x )
  D9 c, C1 c, D1 c, E1 c, 19 c, E5 c, C5 c, D9 c,
  \ exx
  \ pop bc
  \ pop de
  \ pop hl
  \ add hl,de
  \ push hl
  \ push bc
  \ exx
  jpnext, end-code ?)

  \ doc{
  \
  \ +under ( n1|u1 n2|u2 x -- n3|u3 x )
  \
  \ Add _n2|u2_ to _n1|u2_, giving the sum _n3|u3_.
  \
  \ The word is written in Z80. This is an example
  \ implementation in Forth:
  \
  \ ----
  \ : +under ( n1|u1 n2|u2 x -- n3|u3 x )
  \   >r + r> ;
  \ ----
  \
  \ Origin: Comus.
  \
  \ See also: `under+`
  \
  \ }doc

  \ XXX TODO -- variant after PFE's `(under+)`:
  \ : +under ( n1 n2 -- n1+n2 n2 ) tuck + swap ;

( % u% u>ud within between gcd )

[unneeded] % ?\ : % ( n1 n2 -- n3 ) 100 swap */ ;

  \ doc{
  \
  \ % ( n1 n2 -- n3 )
  \
  \ _n1_ is percentage _n3_ of _n2_.
  \
  \ See also: `u%`.
  \
  \ }doc

[unneeded] u%
?\ : u% ( u1 u2 -- u3 ) >r 100 um* r> um/mod nip ;

  \ doc{
  \
  \ u% ( u1 u2 -- u3 )
  \
  \ _u1_ is percentage _u3_ of _u2_.
  \
  \ See also: `%`.
  \
  \ }doc

[unneeded] u>ud ?\ need alias  ' 0 alias u>ud ( u -- ud )

  \ doc{
  \
  \ u>ud ( u -- ud )
  \
  \ Extend a single unsigned number _u_ to form a double
  \ unsigned number _ud_. This word is just an alias of `0`.
  \
  \ See also: `s>d`.
  \
  \ }doc

[unneeded] within

?\ : within ( n1|u1 n2|u2 n3|u3 -- f ) over - >r - r> u< ;

  \ Credit:
  \
  \ Code from DZX-Forth.

  \ doc{
  \
  \ within ( n1|u1 n2|u2 n3|u3 -- f )
  \
  \ Perform a comparison of a test value n1|u1 with a lower
  \ limit _n2|u2_ and an upper limit _n3|u3_, returning _true_
  \ if either (_n2|u2_ < _n3|u3_ and (_n2|u2_ <= _n1|u1_ and
  \ _n1|u1_ < _n3|u3_)) or (_n2|u2_ > _n3|u3_ and (_n2|u2_ <=
  \ _n1|u1_ or _n1|u1_ < _n3|u3_)) is true, returning _false_
  \ otherwise.
  \
  \ Origin: Forth-94 (CORE EXT), Forth-2012 (CORE EXT).
  \
  \ See also: `between`.
  \
  \ }doc

[unneeded] between

?\ : between ( n1|u1 n2|u2 n3|u3 -- f ) over - -rot - u< 0= ;

  \ Credit:
  \
  \ http://dxforth.netbay.com.au/between.html

  \ doc{
  \
  \ between ( n1|u1 n2|u2 n3|u3 -- f )
  \
  \ Perform a comparison of a test value _n1|u1_ with a lower
  \ limit _n2|u2_ and an upper limit _n3|u3_, returning _true_
  \ if either (_n2|u2_ <= _n3|u3_ and (_n2|u2_ <= _n1|u1_ and
  \ _n1|u1_ <= _n3|u3_)) or (_n2|u2_ > _n3|u3_ and (_n2|u2_ <
  \ _n1|u1_ or _n1|u1_ < _n3|u3_)) is true, returning _false_
  \ otherwise.
  \
  \ See also: `within`.
  \
  \ }doc

[unneeded] gcd

?\ : gcd ( n1 n2 -- n3 ) begin ?dup while tuck mod repeat ;

  \ Credit:
  \
  \ Code by Will Baden (1986-04-10), from:
  \ <http://atariwiki.strotmann.de/wiki/Wiki.jsp?page=Local%20Variables>

  \ doc{
  \
  \ gcd ( n1 n2 -- n3 )
  \
  \ _n3_ is the greatest common divisor of _n1_ and _n2_.
  \
  \ }doc

( odd? even? )

[unneeded] odd? ?(

code odd? ( n -- f )
  E1 c, CB c, 40 05 + c, CA c, ' false , C3 c, ' true ,
  \ pop hl
  \ bit 0,l
  \ jp z,false_
  \ jp true_
  end-code ?)

  \ doc{
  \
  \ odd? ( n -- f )
  \
  \ Is _n_ an odd number?
  \
  \ This word is written in Z80. The equivalent code in Forth
  \ is the following:
  \
  \ ----
  \ : odd? ( n -- f ) 1 and 0<> ;
  \ ----
  \
  \ See also: `odd?`.
  \
  \ }doc

[unneeded] even? ?(

code even? ( n -- f )
  E1 c, CB c, 40 05 + c, CA c, ' true , C3 c, ' false ,
  \ pop hl
  \ bit 0,l
  \ jp z,true_
  \ jp false_
  end-code ?)

  \ doc{
  \
  \ even? ( n -- f )
  \
  \ Is _n_ an even number?
  \
  \ This word is written in Z80. The equivalent code in Forth
  \ is the following:
  \
  \ ----
  \ : even? ( n -- f ) 1 and 0= ;
  \ ----
  \
  \ See also: `odd?`.
  \
  \ }doc

( 8* polarity <=> )

[unneeded] 8* ?(

code 8* ( n1 -- n2 )
  e1 c, 29 c, 29 c, 29 c, jppushhl, end-code ?)
  \ pop hl
  \ add hl,hl
  \ add hl,hl
  \ add hl,hl
  \ _jp_pushhl

  \ doc{
  \
  \ 8* ( x1 -- x2 )
  \
  \ _x2_ is the result of shifting _x1_ three bits toward the
  \ most-significant bit, filling the vacated least-significant
  \ bit with zero.
  \
  \ This is the same as ``3 lshift`` or ``2* 2* 2*``, but
  \ faster.
  \
  \ See also: `2*`, `lshift`.
  \
  \ }doc

[unneeded] polarity ?(

code polarity ( n -- -1 | 0 | 1 )
  D1 c, 78 02 + c,  B0 03 + c,  CA c, ' false ,
    \ pop de
    \ ld a,d
    \ or e
    \ jp z,false_code
  CB c, 10 03 + c,  ED c, 62 c,
    \ rl d ; set carry if DE -ve
    \ sbc hl,hl ; HL=0 if DE +ve, or -1 if DE -ve
  78 05 + c,  F6 c, 01 c,  68 07 + c,  jppushhl, end-code ?)
    \ ld a,l
    \ or 1
    \ ld l,a ; HL=1 or -1
    \ jp push_hl

  \ doc{
  \
  \ polarity ( n -- -1|0|1 )
  \
  \ If _n_ is zero, return zero.
  \ If _n_ is negative, return negative one.
  \ If _n_ is positive, return positive one.
  \
  \ ``polarity`` is written in Z80. This is an example
  \ implementation in Forth:
  \
  \ ----
  \ : polarity ( n -- -1|0|1 )
  \   dup 0= ?exit  0< ?dup ?exit  1 ;
  \ ----
  \
  \ See also: `<=>`, `negate`.
  \
  \ }doc

  \ Credit:
  \
  \ Assembler version of `polarity` adapted from Z88
  \ CamelForth.

[unneeded] <=>

?\ need polarity  : <=> ( n1 n2 -- -1|0|1 ) - polarity ;

  \ doc{
  \
  \ <=> ( n1 n2 -- -1|0|1 )
  \
  \ If _n1_ equals _n2_, return zero.
  \ If _n1_ is less than _n2_, return negative one.
  \ If _n1_ is greater than _n2_, return positive one.
  \
  \ See also: `polatiry`, `<`, `=`, `>`.
  \
  \ }doc

( u<= u>= <= >= 0>= 0<= 0max )

[unneeded] u<= ?\ : u<= ( u1 u2 -- f ) u> 0= ;

  \ doc{
  \
  \ u<= ( u1 u2 -- f )
  \
  \ _f_ is _true_ if and only if _u1_ is less than or equal
  \ to _u2_.
  \
  \ See also: `u>=`, `<=`, `0<=`.
  \
  \ }doc

[unneeded] u>= ?\ : u>= ( u1 u2 -- f ) u< 0= ;

  \ doc{
  \
  \ u>= ( u1 u2 -- f )
  \
  \ _f_ is _true_ if and only if _u1_ is greater than or
  \ equal to _u2_.
  \
  \ See also: `u<=`, `>=`, `0>=`.
  \
  \ }doc

[unneeded] <= ?\ : <= ( n1 n2 -- f ) > 0= ;

  \ doc{
  \
  \ <= ( n1 n2 -- f )
  \
  \ _f_ is _true_ if and only if _n1_ is less than or
  \ equal to _n2_.
  \
  \ See also: `>=`, `u<=`, `0<=`.
  \
  \ }doc

[unneeded] >= ?\ : >= ( n1 n2 -- f ) < 0= ;

  \ doc{
  \
  \ >= ( n1 n2 -- f )
  \
  \ _f_ is _true_ if and only if _n1_ is greater than or
  \ equal to _n2_.
  \
  \ See also: `<=`, `u>=`, `0>=`.
  \
  \ }doc

[unneeded] 0>= ?\ : 0>= ( n0 -- f ) 0< 0= ;

  \ doc{
  \
  \ 0>= ( n -- f )
  \
  \ _f_ is _true_ if and only if _n_ is greater than or equal
  \ to zero.
  \
  \ See also: `0<=`, `>=`, `u>=`.
  \
  \ }doc

[unneeded] 0<= ?\ : 0<= ( n -- f ) 0> 0= ;

  \ doc{
  \
  \ 0<= ( n -- f )
  \
  \ _f_ is _true_ if and only if _n_ is less than or equal to
  \ zero.
  \
  \ See also: `0>=`, `<=`, `u<=`.
  \
  \ }doc

[unneeded] 0max ?(

code 0max ( n -- n | 0 )
  E1 c,  CB c, 10 05 + c,  DA c, ' false ,  CB c, 18 05 + c,
    \ pop hl
    \ rl h ; negative?
    \ jp c,false_
    \ rr h
  jppushhl, end-code ?)
    \ jp push_hl

  \ Credit:
  \
  \ Idea from IsForth.

  \ doc{
  \
  \ 0max ( n -- n | 0 )
  \
  \ If _n_ is negative, return 0; else return _n_.  This is a
  \ faster alternative to the idiom ``0 max``.
  \
  \ See also: `max`, `min`.
  \
  \ }doc

( lshift rshift )

[unneeded] lshift ?( need assembler need unresolved

code lshift ( x1 u -- x2 )

  exx, b pop, c b ld, h pop,
  \ B = loop counter  (high 8 bits of _u_ are ignored)
  \ HL = _x1_
  b inc, rahead 0 unresolved !
  rbegin  h addp, 0 unresolved @ >rresolve  rstep
  h push, exx, jpnext, end-code ?)

  \ Credit:
  \
  \ Code adapted from Z88 CamelForth.

  \ Data space used: 16 bytes.

  \ doc{
  \
  \ lshift ( x1 u -- x2 )
  \
  \ Perform a logical left shift of _u_ bit-places on _x_,
  \ giving _x2_. Put zeroes into the least significant  bits
  \ vacated by the shift.
  \
  \ Origin: Forth-94 (CORE), Forth-2012 (CORE).
  \
  \ See also: `rshift`, `?shift`.
  \
  \ }doc

[unneeded] rshift ?( need assembler need unresolved

code rshift ( x1 u -- x2 )

  exx, b pop, c b ld, h pop,
  \ B = loop counter (high 8 bits of _u_ are ignored)
  \ HL = _x1_
  b inc, rahead 0 unresolved !
  rbegin  h srl, l rr, 0 unresolved @ >rresolve  rstep
  h push, exx, jpnext, end-code ?)

  \ Credit:
  \
  \ Code adapted from Z88 CamelForth.

  \ Data space used: 19 bytes.

  \ doc{
  \
  \ rshift ( x1 u -- x2 )
  \
  \ Perform a logical right shift of _u_ bit-places on _x_,
  \ giving _x2_. Put zeroes into the most significant  bits
  \ vacated by the shift.
  \
  \ Origin: Forth-94 (CORE), Forth-2012 (CORE).
  \
  \ See also: `lshift`, `?shift`.
  \
  \ }doc

( ?shift )

need 0exit need rshift need lshift

: ?shift ( x n -- x | x' )
  ?dup 0exit
  dup 0< if  abs rshift exit  then
  lshift ;

  \ XXX TODO -- Rewrite in Z80.

  \ doc{
  \
  \ ?shift ( x1 n -- x1 | x2 )
  \
  \ If _n_ is zero, drop it return _x1_.  If _n_ is negative,
  \ convert it to its absolute value and execute `rshift`,
  \ returning _x2_.  If _n_ is positive execute `lshift`,
  \ returning _x2_.
  \
  \ }doc

( clshift crshift )

[unneeded] clshift ?(

code clshift ( b1 u -- b2 )

  D1 c,  E1 c,  78 05 + c,  1C c,
    \ pop de ; E = u (8 high bits are ignored)
    \ pop hl ; L = b1
    \ ld a,l ; A = b1
    \ inc e
  here
    \ begin:
  1D c,  CA c, pusha ,  80 07 + c,  C3 c, , end-code ?)
    \ dec e
    \ jp z,push_a
    \ add a,a
    \ jp begin

[unneeded] crshift ?(

  \ XXX UNDER DEVELOPMENT -- 2016-05-01

code crshift ( b1 u -- b2 )

  D1 c,  E1 c,  78 05 + c,  1C c,
    \ pop de
    \ pop hl
    \ ld a,l
    \ inc e
  here
    \ begin:
  1D c,  CA c, pusha ,
    \ dec e
    \ jp z,push_a
  \ 80 07 + c,
    \ rra
  C3 c, , end-code ?)
    \ jp begin

( bits )

need assembler

code bits ( ca len -- u )

  0 h ldp#,  \ init bit count
  exx, \ save IP and count
  d pop, h pop,  \ memory zone
  rbegin
    d a ld, e or, nz? rif
      08 b ld#,  \ bits per byte
      rbegin  m rrc, c? rif  exx, h incp, exx,  rthen  rstep
      h incp, d decp,  \ next byte
  2swap ragain rthen
    \ Note: `2swap` is needed because `rbegin ragain` and `rif
    \ rthen` are not nested.

  exx, jppushhl, end-code

  \ Credit:
  \
  \ Based on a pixels counter written by Juan Antonio Paz,
  \ published on Microhobby, issue 170 (1988-05), page 21:
  \ http://microhobby.org/numero170.htm
  \ http://microhobby.speccy.cz/mhf/170/MH170_21.jpg

  \ Data space used: 29 bytes.

  \ doc{
  \
  \ bits ( ca len -- u )
  \
  \ Count the number _u_ of bits that are set in memory zone
  \ _ca len_.
  \
  \ See also: `pixels`.
  \
  \ }doc

( 2/ cell/ )

[unneeded] 2/ ?(

code 2/ ( x1 -- x2 )
  E1 c, CB c, 2C c, CB c, 1D c, jppushhl, end-code ?)
  \ pop hl
  \ sra h
  \ rr l
  \ jp push_hl

  \ Credit:
  \
  \ Code from Spectrum Forth-83.
  \ Documentation partly based on lina.

  \ doc{
  \
  \ 2/ ( x1 -- x2 )
  \
  \ _x2_ is the result of shifting _x1_ one bit toward the
  \ least-significant bit, leaving the most-significant bit
  \ unchanged.
  \
  \ This is the same as ``s>d 2 fm/mod swap drop``. It is not
  \ the same as ``2 /``, nor is it the same as ``1 rshift``.
  \
  \ Origin: Forth-83 (Required Word Set), Forth-94 (CORE),
  \ Forth-2012 (CORE).
  \
  \ }doc

[unneeded] cell/ ?\ need alias need 2/  ' 2/ alias cell/

  \ Credit:
  \
  \ Idea from IsForth.

  \ doc{
  \
  \ cell/ ( n1 -- n2 )
  \
  \ Divide _n1_ by the size of a cell, returning the result
  \ _n2_.
  \
  \ See also: `cell`, `cells`, `cell+`, `cell-`.
  \
  \ }doc

( sqrt )

  \ Credit:
  \
  \ Original code by Wil Baden, published on Forth Dimensions
  \ (volume 18, number 5, page 27, 1997-01).

  \ XXX TODO -- benchmark

need d2* need 2/

[defined] cell-bits ?\ 16 constant cell-bits

: (sqrt) ( radicand -- remainder root )
  0 0                           ( radicand remainder root )
  [ cell-bits 2/ ] literal 0 ?do
    >r d2* d2* r>               \ shift remainder left 2 bits
    2*                          \ shift root left 1 bit
    2dup 2* u> if               \ check for next bit of root
      >r r@ 2* - 1- r>          \ reduce remainder
      1+                        \ add a bit to root
    then
  loop  cr .s rot drop ;

: sqrt ( radicand -- root ) (sqrt) nip ;

( sqrt )

  \ Integer square root by Newton's method

  \ Credit:
  \
  \ Adapted from Sinclair QL's Computer One Forth.

  \ XXX TODO -- benchmark

need 2/

: sqrt ( n1 -- n2 )
  dup 0< -24 ?throw  \ invalid numeric argument
  dup
  if  dup 2/  20 0
      ?do     2dup / + 2/
      loop    swap drop
  then ;

( /-rem /- -rem */-rem */- )

  \ Symmetric-division operators

  \ Credit:
  \
  \ Forth-94 documentation.

[unneeded] /-rem ?( need sm/rem

: /-rem ( n1 n2 -- n3 n4 ) >r  s>d  r> sm/rem ; ?)

[unneeded] /- ?( need /-rem

: /- (  n1 n2 -- n3 ) /-rem nip ; ?)

[unneeded] -rem ?( need /-rem

: -rem ( n1 n2 -- n3 ) /-rem drop ; ?)

[unneeded] */-rem ?( need sm/rem

: */-rem (  n1 n2 n3 -- n4 n5 ) >r  m*  r> sm/rem ; ?)

[unneeded] ?( need */-rem

: */- ( n1 n2 n3 -- n4 ) */-rem nip ; ?)

( fm/mod )

  \ Credit:
  \
  \ Code from Z88 CamelForth.

: fm/mod ( d1 n1 -- n2 n3 )
  dup >r                \ save divisor
  sm/rem
  over 0<> over 0< and  \ quotient<0 and remainder<>0?
  if
    swap r> +           \ add divisor to remainder
    swap 1-             \ decrement quotient
  else r> drop then ;

  \ doc{
  \
  \ fm/mod ( d1 n1 -- n2 n3 )
  \
  \ Floored division:
  \
  \ ----
  \   d1 = n3*n1+n2
  \   n1>n2>=0 or 0>=n2>n1
  \ ----
  \
  \ Divide _d1_ by _n1_, giving the floored quotient _n3_ and
  \ the remainder _n2_. Input and output stack arguments are
  \ signed.
  \
  \ Origin: Forth-94 (CORE), Forth-2012 (CORE).

  \ [caption="Floored Division Example"]
  \ |===
  \ | Dividend   | Divisor  | Remainder  | Quotient

  \ >|       10  >|      7  >|        3  >|        1
  \ >|      -10  >|      7  >|        4  >|       -2
  \ >|       10  >|     -7  >|       -4  >|       -2
  \ >|      -10  >|     -7  >|       -3  >|        1
  \ |===

  \ See also: `sm/rem`, `m/`.
  \
  \ }doc

( /_mod /_ _mod */_mod */_ )

  \ Floored-division operators

  \ Credit:
  \
  \ Forth-94 documentation.

[unneeded] /_mod ?( need fm/mod
: /_mod ( n1 n2 -- n3 n4 ) >r s>d r> fm/mod ; ?)

[unneeded] /_ ?( need /_mod
: /_ ( n1 n2 -- n3 ) /_mod nip ; ')

[unneeded] _mod ?( need /_mod
: _mod ( n1 n2 -- n3 ) /_mod drop ; ?)

[unneeded] */_mod ?( need fm/mod
: */_mod ( n1 n2 n3 -- n4 n5 ) >r m* r> fm/mod ; ?)

[unneeded] */_ ?( need */_mod
: */_ ( n1 n2 n3 -- n4 )  */_mod nip ; ?)

( any? either neither ifelse )

[unneeded] any? ?( need roll  variable (any?)

: any? ( x0 x1..xn n -- f )
  dup 1+ roll (any?) !
  0 swap 0 ?do  swap (any?) @ = or  loop ; ?)

  \ Credit:
  \
  \ Originally written by John A. Peters in 1984 as part of a
  \ tool set for the CP/M implementation of Laxen&Perry's F83
  \ 2.1.1.

  \ doc{
  \
  \ any? ( x0 x1..xn n -- f )
  \
  \ Is any _x1..xn_ equal to _x0_?
  \
  \ Origin: John A. Peters' tools for CP/M F83 2.1.1, 1984.
  \
  \ See also: `either`, `neither`, `ifelse`.
  \
  \ }doc

[unneeded] either

?\ : either ( n1|u1 n2|u2 n3|u3 -- f ) -rot over = -rot = or ;

  \ Credit:
  \
  \ Code from IsForth (version 1.23b).

  \ doc{
  \
  \ either ( n1|u1 n2|u2 n3|u3 -- f )
  \
  \ Return _true_ if _n1|u1_ equals either _n2|u2_ or _n3|u3_;
  \ else return _false_.
  \
  \ Origin: IsForth.
  \
  \ See also: `neither`, `ifelse`, `any?`.
  \
  \ }doc

[unneeded] neither ?(

: neither ( n1|u1 n2|u2 n3|u3 -- f )
  -rot over <> -rot <> and ; ?)

  \ Credit:
  \
  \ Code from IsForth (version 1.23b).

  \ doc{
  \
  \ neither ( n1|u1 n2|u2 n3|u3 -- f )
  \
  \ Return _true_ if _n1|u1_ is not equal to either _n2|u2_ or
  \ _n3|u3_; else return _false_.
  \
  \ Origin: IsForth.
  \
  \ See also: `either`, `ifelse`, `any?`.
  \
  \ }doc

[unneeded] ifelse

?\ : ifelse ( x1 x2 f -- x1 | x2 ) if  drop  else  nip  then ;

  \ Credit:
  \
  \ Idea from: http://hyperpolyglot.org/stack#ifelse

  \ doc{
  \
  \ ifelse ( x1 x2 f -- x1 | x2 )
  \
  \ If _f_ is true return _x1_, otherwise return _x2_.
  \
  \ }doc

( split join )

  \ XXX OLD -- 2016-10-27: Moved to the kernel, needed by the
  \ extra memory system.

[unneeded] split ?(

code split ( x -- b1 b2 )
  E1 c,
    \ pop hl
  16 c, 00 c,  58 05 + c,  68 04 + c,  26 c, 00 c,
    \ ld d,0
    \ ld e,l
    \ ld l,h
    \ ld h,0
  C3 c, pushhlde , end-code ?)
    \ jp push_hlde

  \ Credit:
  \
  \ Idea from IsForth.

  \ doc{
  \
  \ split ( x -- b1 b2 )
  \
  \ Get _b1_ and _b2_ from the 2 bytes which compose _x_: _b1_
  \ is the low-order byte and _b2_ is the high-order byte.
  \
  \ Origin: IsForth.
  \
  \ See also: `join`.
  \
  \ }doc

[unneeded] join ?(

code join ( b1 b2 -- x )
  D1 c,  60 03 + c,  D1 c,  68 03 + c, jppushhl, end-code ?)
    \ pop de
    \ ld h,e
    \ pop de
    \ ld l,e
    \ jp push_hl

  \ doc{
  \
  \ join ( b1 b2 -- x )
  \
  \ _b1_ is the low-order byte of _x_, and _b2_ is the
  \ high-order byte of _x_.
  \
  \ Origin: IsForth.
  \
  \ See also: `split`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015: Add `within`, `between`, and common operators.
  \
  \ 2015-08-12: Add `lshift`. Improve `2/`.
  \
  \ 2015-08-14: Add `under+`, in Forth.
  \
  \ 2015-11-01: Add `rshift` and faster `lshift`.
  \
  \ 2015-11-13: Add `sqrt` (version by Wil Baden).
  \
  \ 2015-12-15: Rewrote `under+` in Z80.
  \
  \ 2015-12-21: Add `polarity`.
  \
  \ 2015-12-22: Add `%` and `u%`.
  \
  \ 2015-12-29: Add second version of `sqrt` (from Computer One
  \ Forth).
  \
  \ 2016-03-20: Add `+under`, a variant of `under+`.
  \
  \ 2016-04-05: Add `cell/`.
  \
  \ 2016-04-07: Add `bits`, generic version of `pixels`.
  \
  \ 2016-04-27: Add `sgn`, `<=>`, `either`, `neither`.
  \
  \ 2016-04-28: Fix `<=>`. Add `0max`.
  \
  \ 2016-05-01: Add `clshift`.
  \
  \ 2016-05-02: Compact the blocks to save space. Remove `sgn`
  \ because `polarity` does the same already.
  \
  \ 2016-07-14: Fix and complete credits of `any?`.
  \
  \ 2016-07-29: Add `gcd`.
  \
  \ 2016-10-16: Document `%`, `u%` and `gcd`.
  \
  \ 2016-10-24: Fix description of `split`.
  \
  \ 2016-10-24: Fix requiring `gcd`, `%` and `u%`.
  \
  \ 2016-10-24: Add `u>ud`.
  \
  \ 2016-11-14: Fix requiring `<=>`. Remove `sm/rem`, which is
  \ in the kernel. Move `8*` here from the assemblers.
  \
  \ 2016-11-17: Use `?(` instead of `[if]`. Compact the module,
  \ saving one block.
  \
  \ 2016-12-06: Simplify documentation of `between` and
  \ `within`.
  \
  \ 2016-12-20: Rename `jppushhl` to `jppushhl,` and `jpnext`
  \ to `jpnext,`, after the change in the kernel.
  \
  \ 2016-12-21: Add `odd?`. Rewrite `8*` in Z80.
  \
  \ 2016-12-22: Add `even?`.
  \
  \ 2016-12-28: Add `ifelse`. Fix Typo. Improve documentation
  \ of `either`, `neither` and `any?`.
  \
  \ 2016-12-30: Compact the code, saving two blocks.
  \
  \ 2017-01-02: Convert `lshift`, `rshift` and `bits` from
  \ `z80-asm` to `z80-asm,`.
  \
  \ 2017-01-04: Fix: replace `>relresolve` with `>rresolve` in
  \ `lshift` and `rshift`.
  \
  \ 2017-01-05: Update `need z80-asm,` to `need assembler`.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-21: Remove the slower (and only 3 bytes smaller)
  \ alternative implementations of `rshift` and `lshift`.
  \ Combine `rshift` and `lshift` in one block.
  \
  \ 2017-01-23: Add `?shift`.
  \
  \ 2017-01-24: Improve documentation of `fm/mod`. Improve
  \ needing of symmetric-division and floored-division
  \ operators.
  \
  \ 2017-01-26: Rewrite `odd?` and `even?` in Z80 and document
  \ them.
  \
  \ 2017-02-17: Fix Asciidoctor table in documentation.  Update
  \ cross references.  Change markup of inline code that is not
  \ a cross reference.
  \
  \ 2017-02-19: Replace `do`, which has been moved to the
  \ library, with `?do`.
  \
  \ 2017-02-20: Improve documentation.
  \
  \ 2017-02-21: Need `unresolved`, which now is optional, not
  \ part of the assembler.
  \
  \ 2017-02-27: Improve documentation.

  \ vim: filetype=soloforth
