  \ math.floating_point.rom.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ XXX UNDER DEVELOPMENT

  \ Last modified: 201806041125
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ A floating point implementation that uses the ROM
  \ calculator.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ ===========================================================
  \ To-do

  \ XXX TODO -- Write `fliteral`, `ffield:`, `fvalue`,
  \ environmental queries.
  \
  \ XXX TODO -- Write safer alternatives for the ambiguous
  \ conditions listed in Forth-2012, or better yet, use the
  \ standard name for the safe version, and factor the faster
  \ unsafe code.
  \
  \ XXX TODO -- `init-fs`, call to ROM routine STK_STK ($16C5).
  \
  \ XXX TODO -- Test everything.
  \
  \ XXX TODO -- Document.

( float float+ float- floats )

5 cconstant float

  \ doc{
  \
  \ float ( -- n )
  \
  \ _n_ is the size in bytes of a floating-point number.
  \
  \ See: `floats`, `float+`, `float-`.
  \
  \ }doc

: float+ ( fa1 -- fa2 ) float + ;

  \ doc{
  \
  \ float+ ( fa1 -- fa2 ) "float-plus"
  \
  \ Add the size in bytes of a floating-point number to _fa1_,
  \ giving _fa2_.
  \
  \ See: `float-`, `float`, `floats`.
  \
  \ }doc

: float- ( fa1 -- fa2 ) float - ;

  \ doc{
  \
  \ float- ( fa1 -- fa2 ) "float-minus"
  \
  \ Subtract the size in bytes of a floating-point number from
  \ _fa1_, giving _fa2_.
  \
  \ See: `float+`, `float`, `floats`.
  \
  \ }doc

: floats ( n1 -- n2 ) float * ;

  \ doc{
  \
  \ floats ( n1 -- n2 )
  \
  \ _n2_ is the size in bytes of _n1_ floating-point
  \ numbers.
  \
  \ See: `float`, `float+`, `float-`.
  \
  \ }doc

( fp0 fp (fp@ fp@ empty-fs fdepth )

need float need float-

23651 constant fp0 \ STKBOT system variable

  \ doc{
  \
  \ fp0  ( -- a ) "f-p-zero"
  \
  \ _a_ is the address of a cell containing the bottom address
  \ of the floating-point stack. _a_ is the STKBOT variable of
  \ the OS.
  \
  \ NOTE: The floating-point stack (which is the OS calculator
  \ stack) grows towards higher memory.
  \
  \ See: `fp`.
  \
  \ }doc

23653 constant fp \ STKEND system variable

  \ doc{
  \
  \ fp  ( -- a ) "f-p"
  \
  \ _a_ is the address of a cell containing the floating-point
  \ stack pointer. _a_ is the STKEND variable of the OS.
  \
  \ NOTE: The floating-point stack (which is the OS calculator
  \ stack) grows towards higher memory, and ``fp`` points to
  \ the first free position, therefore above top of stack.
  \
  \ See: `fp@`, `fp0`.
  \
  \ }doc

: (fp@ ( -- fa ) fp @ ;

  \ doc{
  \
  \ (fp@ ( -- fa ) "paren-f-p-fetch"
  \
  \ _fa_ is the address above the top of the floating-point
  \ stack. ``(fp@``  is a factor of `fp@`.
  \
  \ See: `fp`.
  \
  \ }doc

: fp@ ( -- fa ) (fp@ float- ;

  \ doc{
  \
  \ fp@ ( -- fa ) "f-p-fetch"
  \
  \ _fa_ is the address of the top of the floating-point stack.
  \
  \ See: `fp`.
  \
  \ }doc

: empty-fs ( -- ) fp0 @ fp ! ;

  \ doc{
  \
  \ empty-fs ( -- ) "empty-f-s"
  \
  \ Empty the floating-point stack, by storing the content of
  \ `fp0` into `fp`.
  \
  \ }doc

: fdepth ( -- +n ) (fp@ fp0 @ - float / ;

  \ doc{
  \
  \ fdepth ( -- +n ) "f-depth"
  \
  \ _+n_ is the number of values contained on the
  \ floating-point stack.
  \
  \ Origin: Forth-94 (FLOATING), Forth-2012 (FLOATING).
  \
  \ See: `fp0`, `(fp@` ,`float`, `depth`, `rdepth`.
  \
  \ }doc

( f>flag )

need (f>s

: f>flag ( -- f ) ( F: rf -- ) (f>s negate ;

  \ doc{
  \
  \ f>flag ( -- f ) ( F: rf -- ) "f-to-flag"
  \
  \ Convert a floating-poing flag _rf_ (1|0) to an actual flag
  \ _f_ in the data stack.
  \
  \ }doc

( end-calculator-flag )

need macro need f>flag need call-xt

macro end-calculator-flag ( -- f ) ( F: 1|0 -- )
  [ calculator-wordlist >order ] end-calculator [ previous ]
  ['] f>flag call-xt  jpnext, endm

  \ doc{
  \
  \ end-calculator-flag ( -- f ) ( F: 1|0 -- )
  \
  \ A Z80 `macro` that compiles code to exit the ROM calculator
  \ and convert a flag calculated by it (_1|0_) to a
  \ well-formed flag on the data stack.
  \
  \ ``end-calculator-flag`` is a common factor of all
  \ floating-point logical operators.
  \
  \ See: `calculator-command`.
  \
  \ }doc

( calculator-command )

need calculator

: calculator-command ( b -- )
  $C5 c,  $06 c, c,
    \ push bc ; save the Forth IP
    \ ld b,command
  calculator  $3B c,
    \ `fp-calc-2` calculator command, which executes the
    \ calculator command stored in the b register.
  [ calculator-wordlist >order ] end-calculator [ previous ]
  $C1 c, ;
    \ pop bc ; restore the Forth IP

  \ doc{
  \
  \ calculator-command ( b -- )
  \
  \ Compile the assembly instructions needed to execute the
  \ _b_ command of the ROM calculator.
  \
  \ See: `end-calculator-flag`.
  \
  \ }doc

( calculator-command>flag )

need calculator-command need f>flag need call-xt

: calculator-command>flag ( b -- )
  calculator-command ['] f>flag call-xt
  [ assembler-wordlist >order ] jpnext, [ previous ] ;

  \ doc{
  \
  \ calculator-command>flag ( b -- ) "calculator-command-to-flag"
  \
  \ Compile the assembly instructions needed to execute the
  \ _b_ command of the ROM calculator and to return the
  \ floating-point result as a flag on the data stack.
  \
  \ }doc

( f= f<> )

need calculator-command>flag

code f= ( -- f ) ( F: r1 r2 -- )
  0E calculator-command>flag end-code
  \ `nos-eql` calculator command

code f<> ( -- f ) ( F: r1 r2 -- )
  0B calculator-command>flag end-code
  \ `nos-neql` calculator command

( f~abs f~rel f~relabs f== )

  \ Credit:
  \
  \ Most of this code is based on the words `f~`, `f~abs` and
  \ `f~rel` implemented in Gforth 0.7.3. Parts have been
  \ factored and adapted.

need frot need f- need fabs need fswap need f< need fover
need f+ need f* need fp@ need float- need float need fdrop
need fsgn

: f~abs ( -- f ) ( F: r1 r2 r3 -- )
  frot frot f- fabs fswap f< ;

  \ doc{
  \
  \ f~abs ( -- f ) ( F: r1 r2 r3 -- ) "f-tilde-abs"
  \
  \ Approximate equality with absolute error: `|r1-r2|<r3`.
  \
  \ Flag _f_ is true if the absolute value of _r1-r2_ is less
  \ than _r3_.
  \
  \ Origin: Gforth.
  \
  \ See: `f~rel`, `f~relabs`.
  \
  \ }doc

: f~rel ( -- f ) ( F: r1 r2 r3 -- )
  frot frot fover fabs fover fabs f+
  frot frot f- fabs frot frot f* f< ;

  \ doc{
  \
  \ f~rel ( -- f ) ( F: r1 r2 r3 -- ) "f-tilde-rel"
  \
  \ Approximate equality with relative error:
  \ ``|r1-r2|<r3*|r1+r2|``.
  \
  \ Flag _f_ is true if the absolute value of _r1-r2_ is less
  \ than the value of _r3_ times the sum of the absolute values
  \ of _r1_ and _r2_.
  \
  \ See: `f~abs`, `f~relabs`.
  \
  \ }doc

: f~relabs ( -- f ) ( F: r1 r2 r3 -- ) fabs f~rel ;

  \ XXX TODO -- better name

  \ doc{
  \
  \ f~relabs ( -- f ) ( F: r1 r2 r3 -- ) "f-tilde-rel-abs"
  \
  \ Approximate equality with relative error:
  \ ``|r1-r2|<|r3|*|r1+r2|``.
  \
  \ Flag _f_ is true if the absolute value of _r1-r2_ is less
  \ than the absolute value of _r3_ times the sum of the
  \ absolute values of _r1_ and _r2_.
  \
  \ See: `f~rel`, `f~abs`.
  \
  \ }doc

: f== ( -- f ) ( F: r1 r2 -- )
  fp@ dup float- float tuck str= fdrop fdrop ;

  \ doc{
  \
  \ f== ( -- f ) ( F: r1 r2 -- ) "f-equals-equals"
  \
  \ Exact bitwise equality.
  \
  \ Are _r1_ and _r2_ exactly identical? Flag _f_ is true if
  \ the bitwise comparison of _r1_ and _r2_ is succesful.
  \
  \ See: `f~`.
  \
  \ }doc

( f~ )

need f~abs need f== need f~relabs

     ' f~abs ,
here ' f== ,
     ' f~relabs ,

      constant (f~ \ execution table of `f~`

: f~ ( -- f ) ( F: r1 r2 r3 -- )
  fdup fsgn f>s cells (f~ + perform ;

  \ doc{
  \
  \ f~ ( -- f ) ( F: r1 r2 r3 -- ) "f-tilde"
  \
  \ Medley for comparing _r1_ and _r2_ for equality:
  \
  \ - _r3_>0: `f~abs`;
  \ - _r3_=0: `f==`;
  \ - _r3_<0: `f~relabs`.
  \
  \ Origin: Forth-94 (FLOATING EXT), Forth-2012 (FLOATING EXT).
  \
  \ See: `f~rel`.
  \
  \ }doc

( f< f<= f> f>= )

need calculator-command>flag

code f< ( -- f ) ( F: r1 r2 -- )
  0D calculator-command>flag end-code
  \ `no-less` calculator command

code f<= ( -- f ) ( F: r1 r2 -- )
  09 calculator-command>flag end-code
  \ `no-l-eql` calculator command

code f> ( -- f ) ( F: r1 r2 -- )
  0C calculator-command>flag end-code
  \ `no-grtr` calculator command

code f>= ( -- f ) ( F: r1 r2 -- )
  0A calculator-command>flag end-code
  \ `no-gr-eql` calculator command

( f0< f0= f0<> f0> )

need calculator need end-calculator-flag

code f0< ( -- f ) ( F: r -- )
  calculator |0< end-calculator-flag end-code

code f0= ( -- f ) ( F: r -- )
  calculator |0= end-calculator-flag end-code

code f0<> ( -- f ) ( F: r -- )
  calculator |0= |0= end-calculator-flag end-code

code f0> ( -- f ) ( F: r -- )
  calculator |0> end-calculator-flag end-code

( fdrop fdup fswap fover )

need calculator

code fdrop ( F: r -- )
  calculator |drop end-calculator  jpnext, end-code

code fdup ( F: r -- r r )
  calculator |dup end-calculator  jpnext, end-code

code fswap ( F: r1 r2 -- r2 r1 )
  calculator |swap end-calculator  jpnext, end-code

code fover ( F: r1 r2 -- r1 r2 r1 )
  calculator |over end-calculator  jpnext, end-code

( f2dup f2drop )

code f2dup ( F: r -- r r )
  calculator |2dup end-calculator  jpnext, end-code

code f2drop ( F: r -- )
  calculator |drop |drop end-calculator  jpnext, end-code

( fnip ftuck )

need calculator

code fnip ( F: r1 r2 -- r2 )
  calculator |swap |drop end-calculator  jpnext,
  end-code

code ftuck ( F: r1 r2 -- r2 r1 r2 )
  calculator 2 |>mem |swap 2 |mem> end-calculator  jpnext,
  end-code

( frot -frot )

need calculator

code frot ( F: r1 r2 r3 -- r2 r3 r1 )
  calculator
    1 |>mem |drop |swap 1 |mem> |swap
  end-calculator  jpnext, end-code

code -frot ( F: r1 r2 r3 -- r3 r1 r2 )
  calculator
    |swap 1 |>mem |drop |swap 1 |mem>
  end-calculator  jpnext, end-code

( f+ f- f* f/ ?f/ fmod )

need calculator need fdup need f0=

code f+ ( F: r1 r2 -- r3 )
  calculator |+ end-calculator  jpnext, end-code

code f- ( F: r1 r2 -- r3 )
  calculator |- end-calculator  jpnext, end-code

code f* ( F: r1 r2 -- r3 )
  calculator |* end-calculator  jpnext, end-code

code f/ ( F: r1 r2 -- r3 )
  calculator |/ end-calculator  jpnext, end-code
  \ XXX FIXME -- when _r2_ is zero, the calculator issues
  \ "number too big" BASIC error, what crashes the system.  A
  \ safe alternative `?f/` is provided.

: ?f/ ( F: r1 r2 -- r3 ) fdup f0= #-42 ?throw f/ ;
  \ Safe version of `f/`. If _r2_ is zero, an exception
  \ is thrown.

code fmod ( F: r1 -- r2 )
  calculator |mod end-calculator  jpnext, end-code

( fmax )

need calculator need calculator-command

code fmax ( F: r1 r2 -- r1|r2 )
  calculator |2dup end-calculator
  0C calculator-command ( F: r1 r2 rf -- )
    \ `no-grtr` ROM calculator command
  calculator
    |if   |drop ( F: r1 )
    |else |swap |drop ( F: r2 )
    |then
  end-calculator  jpnext, end-code

  \ XXX OLD -- Original, simpler version. The problem is the
  \ calculator's `|>`. See the calculator module for details of
  \ the problem.

  \ code fmax ( F: r1 r2 -- r1|r2 )
  \   calculator
  \     |2dup |> ( F: r1 r2 rf -- )
  \     |if   |drop ( F: r1 )
  \     |else |swap |drop ( F: r2 )
  \     |then
  \   end-calculator  jpnext, end-code

( fmin )

need calculator need calculator-command

code fmin ( F: r1 r2 -- r1|r2 )
  calculator |2dup end-calculator
  0D calculator-command ( F: r1 r2 rf -- )
    \ `no-less` ROM calculator command
  calculator
    |if   |drop ( F: r1 )
    |else |swap |drop ( F: r2 )
    |then
  end-calculator  jpnext, end-code

  \ XXX OLD -- Original, simpler version. The problem is the
  \ calculator's `|<`. See the calculator module for details of
  \ the problem.

  \ code fmin ( F: r1 r2 -- r1|r2 )
  \   calculator
  \     |2dup |< ( F: r1 r2 rf -- )
  \     |if   |drop ( F: r1 )
  \     |else |swap |drop ( F: r2 )
  \     |then
  \   end-calculator  jpnext, end-code

( fsgn fabs fnegate )

need calculator

code fsgn ( F: r1 -- -1|0|1 )
  calculator |sgn end-calculator  jpnext, end-code

code fabs ( F: r1 -- r2 )
  calculator |abs end-calculator  jpnext, end-code

code fnegate ( F: r1 -- r2 )
  calculator |negate end-calculator  jpnext, end-code

( fln ?fln flnp1 ?flnp1 fexp f** fsqrt ?fsqrt )

need calculator need fdup need f0< need f<=

code fln ( F: r1 -- r2 )
  calculator |ln end-calculator  jpnext, end-code
  \ XXX FIXME -- The ROM calculator checks that the argument is
  \ a positive non-zero number (address $3713). If not, it
  \ throws a BASIC error "invalid argument", what crashes the
  \ system.  A safe alternative `?fln` is provided.

: ?fln ( F: r1 -- r2 ) fdup f0 f<= #-46 ?throw fln ;
  \ Safe version of `fln`. If _r1_ is less than or equal to
  \ zero, an exception is thrown.

code flnp1 ( F: r1 -- r2 )
  calculator |1 |+ |fln end-calculator  jpnext, end-code

: ?flnp1 ( F: r1 -- r2 ) fdup f1 fnegate f<= #-46 ?throw fln ;
  \ Safe version of `flnp1`. If _r1_ is less than or equal to
  \ negative one, an exception is thrown.

code fexp ( F: r1 -- r2 )
  calculator |exp end-calculator  jpnext, end-code

code f** ( F: r1 -- r2 )
  calculator |** end-calculator  jpnext, end-code

code fsqrt ( F: r1 -- r2 )
  calculator |sqrt end-calculator  jpnext, end-code
  \ XXX FIXME -- when _r1_ is negative, the calculator issues
  \ "invalid argument" BASIC error, what crashes the system.  A
  \ safe alternative `?fsqrt` is provided.

: ?fsqrt ( F: r1 -- r2 ) fdup f0< #-46 ?throw fsqrt ;
  \ Safe version of `fsqrt`. If _r1_ is negative, an exception
  \ is thrown.

( f0 f1 fhalf fpi2/ f10 )

need calculator

code f0 ( F: -- r )
  calculator |0 end-calculator  jpnext, end-code

code f1 ( F: -- r )
  calculator |1 end-calculator  jpnext, end-code

code fhalf ( F: -- r )
  calculator |half end-calculator  jpnext, end-code

code fpi2/ ( F: -- r )
  calculator |pi2/ end-calculator  jpnext, end-code

code f10 ( F: -- r )
  calculator |10 end-calculator  jpnext, end-code

( (f>s )

  \ XXX REMARK -- `(f>s` must be in other block than `f>s`, to
  \ avoid a circular `need`.

code (f>s ( -- n ) ( F: r -- )
  C5 c, CD c, 2DA2 ,
    \ push bc
    \ call $2DA2 ; FP_TO_BC ROM routine
  60 00 + c, 68 01 + c,  C1 c, E5 c, jpnext,
    \ ld h,b
    \ ld l,c
    \ pop bc
    \ push hl
    \ _jp_next
  end-code

( frestack b>f u>f s>f f>s )

need calculator need fnegate need fdup need (f>s need f0<

code frestack ( F: r -- r' )
  calculator |re-stack end-calculator  jpnext, end-code
  \ Restack an integer in full floating-point form.

code b>f ( b -- ) ( F: -- r )
  D9 c, E1 c, 78 05 + c, CD c, 2D28 , D9 c,  jpnext, end-code
    \ exx
    \ pop hl
    \ ld a,l
    \ call $2D28 ; STACK_A ROM routine
    \ exx
  \ XXX TODO -- test

code u>f ( u -- ) ( F: -- r )
  D9 c, C1 c, CD c, 2D2B , D9 c,  jpnext, end-code
    \ exx
    \ pop bc
    \ call $2D2B ; STACK_BC ROM routine
    \ exx

: s>f ( n -- ) ( F: -- r )
 dup 0< if abs u>f fnegate else u>f then ;
  \ XXX TODO -- test

: f>s ( -- n ) ( F: r -- ) fdup (f>s f0< ?negate ;
  \ XXX TODO -- test

  \ code f>d ( -- d ) ( F: r -- )
  \ end-code
  \ XXX TODO

  \ code d>f ( d -- ) ( F: -- r )
  \ end-code
  \ XXX TODO

  \ code f>string ( -- ca len ) ( F: r -- )
  \ end-code
  \ XXX TODO -- ROM calculator command $2E

  \ : >float ;
  \ XXX TODO -- ROM calculator command `val`

( f! f@ )

need assembler

code f! ( fa -- ) ( F: r -- )
  exx, 2BF1 call, \ STK_FETCH ROM routine
       h pop, a m ld, h incp,
              e m ld, h incp, d m ld, h incp,
              c m ld, h incp, b m ld,
  exx, jpnext, end-code

  \ doc{
  \
  \ f! ( fa -- ) ( F: r -- ) "f-store"
  \
  \ Store _r_ at _fa_.
  \
  \ Origin: Forth-94 (FLOATING), Forth-2012 (FLOATING).
  \
  \ }doc

code f@ ( fa -- ) ( F: -- r )
  exx, h pop, m a ld, h incp,
              m e ld, h incp, m d ld, h incp,
              m c ld, h incp, m b ld
              2AB6 call, \ STK_STORE ROM routine
  exx, jpnext, end-code

  \ doc
  \
  \ f@ ( fa -- ) ( F: -- r )
  \
  \ _r_ is the value stored at _fa_.
  \
  \ Origin: Forth-94 (FLOATING), Forth-2012 (FLOATING).
  \
  \ }doc

( f, fconstant fvariable )

need float need f! need f@

: f, ( -- ) ( F: r -- ) here float allot f! ;

  \ doc{
  \
  \ f, ( -- ) ( F: r -- ) "f-comma"
  \
  \ Reserve data space for one floating-point number and store
  \ _r_ in that space.
  \
  \ Origin: Gforth.
  \
  \ }doc

: fconstant ( "name" -- ) ( F: r -- ) create  f,  does>  f@ ;

  \ doc{
  \
  \ fconstant ( "name" -- ) ( F: r -- ) "f-constant"
  \
  \ Create a floating-point constant called "name" with value
  \ _r_.
  \
  \ Origin: Forth-94 (FLOATING), Forth-2012 (FLOATING).
  \
  \ }doc

: fvariable ( "name" -- ) create  float allot ;

( facos fasin fatan fcos fsin ftan )

need calculator

code facos ( F: r1 -- r2 )
  calculator |acos end-calculator  jpnext, end-code

  \ doc{
  \
  \ facos ( F: r1 -- r2 )
  \
  \ }doc
  \
  \ XXX TODO -- Document.

code fasin ( F: r1 -- r2 )
  calculator |asin end-calculator  jpnext, end-code

  \ doc{
  \
  \ fasin ( F: r1 -- r2 )
  \
  \ }doc
  \
  \ XXX TODO -- Document.

code fatan ( F: r1 -- r2 )
  calculator |atan end-calculator  jpnext, end-code

  \ doc{
  \
  \ fatan ( F: r1 -- r2 )
  \
  \ }doc
  \
  \ XXX TODO -- Document.

code fcos ( F: r1 -- r2 )
  calculator |cos end-calculator  jpnext, end-code

  \ doc{
  \
  \ fcos ( F: r1 -- r2 )
  \
  \ }doc
  \
  \ XXX TODO -- Document.

code fsin ( F: r1 -- r2 )
  calculator |sin end-calculator  jpnext, end-code

  \ doc{
  \
  \ fsin ( F: r1 -- r2 )
  \
  \ }doc
  \
  \ XXX TODO -- Document.

code ftan ( F: r1 -- r2 )
  calculator |tan end-calculator  jpnext, end-code

  \ doc{
  \
  \ ftan ( F: r1 -- r2 )
  \
  \ }doc
  \
  \ XXX TODO -- Document.

( (f. f. )

need fdepth need fdrop

code (f. ( F: r -- ) C5 c, CD c, 2DE3 , C1 c, jpnext, end-code
    \ push bc
    \ call $2DE3 ; PRINT_FP ROM routine
    \ pop bc
    \ _jp_next
  \ Note: `exx` can no be used to preserve `bc`, the Forth IP,
  \ because the routine uses the alternative registers.  `bc`
  \ is saved on the stack instead.

: f. ( F: r -- )
  fdepth >r (f. space fdepth r> = if fdrop then ;

  \ XXX TODO -- Document.

  \ Note: the depth of the stack must be checked because
  \ there's a bug in the PRINT-FP ROM routine called "unbalaced
  \ stack error". When the number is a non-integer less than 1,
  \ a zero is left on the stack.  This bug is documented in the
  \ ZX Spectrum ROM disassembly. Credit: Tony Stratton, 1982.

  \ XXX FIXME -- The Forth-2012 standard reads `f.` must use
  \ fixed-point notation, but in this implementation the
  \ decimal point is not shown at the end when the number is
  \ integer.

( .fs dump-fs )

need (fp@ need fp0 need f@ need f.
need fdepth need float need float+ need .depth

: (.fs ( -- ) (fp@ fp0 @ ?do i f@ f. float +loop ;

: .fs ( -- ) fdepth dup .depth 0> if (.fs then ;

: (dump-fs ( -- )
  cr ." Bottom"
  (fp@ fp0 @ ?do
    i dup cr u. float bounds ?do i c@ 4 .r loop
  float +loop  cr ." Top" cr ;
  \ XXX TODO -- improve: display the top at the top

: dump-fs ( -- ) fdepth dup .depth 0> if (dump-fs then ;

( floor ftrunc fround )

need calculator need fdup need fsgn need f* need f+

code floor ( F: r1 -- r2 )
  calculator |int end-calculator jpnext, end-code

  \ doc{
  \
  \ floor ( F: r1 -- r2 )
  \
  \ Round _r1_ to an integral value using the "round toward
  \ negative infinity" rule, giving _r2_.
  \
  \ Origin: Forth-94 (FLOATING), Forth-2012 (FLOATING).
  \
  \ See: `ftrunc`, `fround`.
  \
  \ }doc


code ftrunc ( F: r1 -- r2 )
  calculator |truncate end-calculator jpnext, end-code

  \ doc{
  \
  \ ftrunc ( F: r1 -- r2 ) "f-trunc"
  \
  \ Round _r1_ to an integral value using the "round toward
  \ zero" rule, giving _r2_.
  \
  \ Origin: Forth-94 (FLOATING), Forth-2012 (FLOATING).
  \
  \ See: `fround` ,`floor`.
  \
  \ }doc

  \ Example from the documentation of Forth-2012:

  \ : ftrunc ( F: r1 -- r2 )
  \   fdup f0= 0= if
  \     fdup f0< if fnegate floor fnegate else floor then
  \   then ;

  \ From Gforth:

  \ : ftrunc ( F: r1 -- r2 ) f>d d>f ;

: fround ( F: r1 -- r2 ) fdup fsgn fhalf f* f+ ftrunc ;

  \ doc{
  \
  \ fround ( r1 -- r2 ) "f-round"
  \
  \ Round _r1_ to an integral value using the "round to
  \ nearest" rule, giving _r2_.
  \
  \ Origin: Forth-94 (FLOATING), Forth-2012 (FLOATING).
  \
  \ See: `ftrunc`, `floor`.
  \
  \ }doc

( falign faligned sfalign sfaligned dfalign dfaligned )

unneeding falign
?\ need alias ' noop alias falign ( -- ) immediate

  \ doc{
  \
  \ falign ( -- ) "f-align"
  \
  \ If the data space is not float aligned, reserve enough
  \ space to make it so.
  \
  \ In Solo Forth, ``falign`` does nothing: it's an `immediate`
  \ `alias` of `noop`.
  \
  \ Origin: Forth-94 (FLOATING), Forth-2012 (FLOATING).
  \
  \ See: `faligned`, `sfalign`, `dfalign`, `float`.
  \
  \ }doc

unneeding faligned
?\ need alias ' noop alias faligned ( a -- fa ) immediate

  \ doc{
  \
  \ faligned ( a -- fa ) "f-aligned"
  \
  \ _fa_ is the first float-aligned address greater than or
  \ equal to _a_
  \
  \ In Solo Forth, ``faligned`` does nothing: it's an
  \ `immediate` `alias` of `noop`.
  \
  \ Origin: Forth-94 (FLOATING), Forth-2012 (FLOATING).
  \
  \ See: `falign`, `sfaligned`, `dfaligned`, `float`.
  \
  \ }doc

unneeding sfalign
?\ need alias ' noop alias sfalign ( -- ) immediate

  \ doc{
  \
  \ sfalign ( -- ) "s-f-align"
  \
  \ If the data space is not single-float aligned, reserve
  \ enough space to make it so.
  \
  \ In Solo Forth, ``sfalign`` does nothing: it's an `immediate`
  \ `alias` of `noop`.
  \
  \ Origin: Forth-94 (FLOATING EXT), Forth-2012 (FLOATING EXT).
  \
  \ See: `sfaligned`, `falign`, `dfalign`, `float`.
  \
  \ }doc

unneeding sfaligned
?\ need alias ' noop alias sfaligned ( a -- dfa ) immediate

  \ doc{
  \
  \ sfaligned ( a -- fa ) "s-f-aligned"
  \
  \ _fa_ is the first single-float-aligned address greater than
  \ or equal to _a_
  \
  \ In Solo Forth, ``sfaligned`` does nothing: it's an
  \ `immediate` `alias` of `noop`.
  \
  \ Origin: Forth-94 (FLOATING EXT), Forth-2012 (FLOATING EXT).
  \
  \ See: `sfalign`, `faligned`, `dfaligned`, `float`.
  \
  \ }doc

unneeding dfalign
?\ need alias ' noop alias dfalign ( -- ) immediate

  \ doc{
  \
  \ dfalign ( -- ) "d-f-align"
  \
  \ If the data space is not double-float aligned, reserve
  \ enough space to make it so.
  \
  \ In Solo Forth, ``dfalign`` does nothing: it's an `immediate`
  \ `alias` of `noop`.
  \
  \ Origin: Forth-94 (FLOATING EXT), Forth-2012 (FLOATING EXT).
  \
  \ See: `dfaligned`, `falign`, `sfalign`, `float`.
  \
  \ }doc

unneeding dfaligned
?\ need alias ' noop alias dfaligned ( a -- dfa ) immediate

  \ doc{
  \
  \ dfaligned ( a -- fa ) "d-f-aligned"
  \
  \ _fa_ is the first double-float-aligned address greater than
  \ or equal to _a_
  \
  \ In Solo Forth, ``dfaligned`` does nothing: it's an
  \ `immediate` `alias` of `noop`.
  \
  \ Origin: Forth-94 (FLOATING EXT), Forth-2012 (FLOATING EXT).
  \
  \ See: `dfalign`, `faligned`, `sfaligned`, `float`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-09-23: Start. Main development.
  \
  \ 2016-04-11: Revision. Code reorganized. First improvements.
  \
  \ 2016-04-12: Started `f.` and `f,`.
  \
  \ 2016-04-13: Fixes and improvements. First usable version.
  \
  \ 2016-04-18: Made `f.` immune to the ROM bug. Fixed
  \ `ftrunc`. Moved the ROM calculator to its own file.
  \ Improved. Added `floor`.
  \
  \ 2016-04-20: Added `fnip`, `ftuck`, `f2dup`, `f2drop`.
  \ Wrote `calculator-command>flag` and rewrote `f=`, `f<>`,
  \ `f<`, `f<=`, `f>` and `f>=` after it, because calling the
  \ equivalents command of the ROM calculator directly always
  \ returned a true flag; the details of the debugging are
  \ noted in the ROM calculator module. Fixed `fmax` and
  \ `fmin`.
  \
  \ 2016-04-21: Added `fround`, `f~`, `f~abs`, `f~rel`,
  \ `f~relabs`, `f==`, `flnp1`, `?fln`, `'?flnp1`.
  \
  \ 2016-05-05: Update `s=` to `str=`.
  \
  \ 2016-12-20: Rename `jppushhl` to `jppushhl,` and `jpnext`
  \ to `jpnext,`, after the change in the kernel.
  \
  \ 2017-01-02: Convert `f!` and `f@` from `z80-asm` to
  \ `z80-asm,`.
  \
  \ 2017-01-05: Update `need z80-asm,` to `need assembler`.
  \ Update `also assembler` to `assembler-wordlist >order`.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-05-06: Update the ROM calculator commands, which have
  \ been renamed. Improve documentation.
  \
  \ 2017-05-07: Improve documentation.
  \
  \ 2017-05-09: Remove `jppushhl,`. Improve documentation.
  \
  \ 2018-02-17: Improve documentation: add pronunciation to
  \ words that need it. Update source layout (remove double
  \ spaces).
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-09: Update notation "address units" to "bytes".
  \
  \ 2018-04-12: Fix and improve documentation.
  \
  \ 2018-06-04: Update: remove trailing closing paren from
  \ word names.

  \ vim: filetype=soloforth
