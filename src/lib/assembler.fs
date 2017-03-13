  \ assembler.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201703132040
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ A Z80 assembler.

  \ ===========================================================
  \ Authors

  \ The original assembler, for the 8080, was written by John
  \ Cassady, in 1980-1981, and published on Forth Dimensions
  \ (volume 3, number 6, page 180, 1982-03).
  \
  \ Coos Haak wrote an improved version for Z80 for his own ZX
  \ Spectrum Forth, in the middle 1980's.
  \
  \ Lennart Benschop included Coos Haak's assembler in his
  \ Spectrum Forth-83 (1988).
  \
  \ Marcos Cruz (programandala.net) adapted, modified and
  \ improved the Spectrum Forth-83 version for Solo Forth,
  \ 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( assembler )

  \ XXX TODO - Finish the documentation.

get-order get-current only forth definitions

need ?pairs need 3dup need 8* need wordlist>vocabulary

assembler-wordlist wordlist>vocabulary assembler

  \ doc{
  \
  \ assembler ( -- )
  \
  \ Replace the first word list in the search order with
  \ `assembler-wordlist`.
  \
  \ Origin: Forth-79 (Assembler Word Set), Forth-83 (Assembler
  \ Extension Word Set), Forth-94 (TOOLS EXT), Forth-2012
  \ (TOOLS EXT).
  \
  \ }doc

also assembler definitions base @ hex

need ?rel need inverse-cond

  \ Registers

0 cconstant b   1 cconstant c   2 cconstant d   3 cconstant e
4 cconstant h   5 cconstant l   6 cconstant m   7 cconstant a

6 cconstant sp  6 cconstant af

DD cconstant ix-op  FD cconstant iy-op

: ix ( -- regpi ) ix-op c, h ;
: iy ( -- regpi ) iy-op c, h ;

-->

( assembler )

  \ Defining words for z80 instructions

: (c ( b "name" -- ) create c, ;

: m1 ( 8b "name" -- ) (c does> ( -- ) ( pfa ) c@ c, ;
  \ 1-byte opcode without parameters.

: m2 ( 8b "name" -- ) (c does> ( reg -- ) ( reg pfa ) c@ + c, ;
  \ 1-byte opcode with register encoded in bits 0-3.

: m3 ( 8b "name" -- )
  (c does> ( reg -- ) ( reg pfa ) c@ swap 8* + c, ;
  \ 1-byte opcode with register encoded in bits 3-5.

: m4 ( 8b "name" -- ) (c does> ( 8b -- ) ( 8b pfa ) c@ c, c, ;
  \ 1-byte opcode with 1-byte parameter.

: m5 ( 8b "name" -- ) (c does> ( 16b -- ) ( 16b pfa ) c@ c, , ;
  \ 1-byte opcode with 2-byte parameter.

: m6 ( 8b "name" -- )
  (c does> ( reg -- ) ( reg pfa ) CB c, c@ + c, ;
  \ Rotation of registers.

: m7 ( 8b "name" -- )
  (c does> ( reg bit -- )
    ( reg bit pfa ) CB c, c@ swap 8* + + c, ;  -->
  \ Bit manipulation of registers.

( assembler )

  \ Defining words for z80 instructions

: m8 ( 16b "name" -- ) create , does> ( -- ) ( pfa ) @ , ;
  \ 2-byte opcodes.

: (jr,) ( a op -- ) c, here 1+ - dup ?rel c, ;
  \ Compile a relative jump _op_ to absolute address _a_.
  \ XXX TODO -- use `<rresolve`

: m9 ( 8b "name" -- ) (c does> ( a -- ) ( a pfa ) c@ (jr,) ;
  \ Relative jumps.

: ma ( 8b "name" -- )
  (c does> ( disp regph -- ) ( disp regph pfa ) c@ c, drop c, ;
  \ Index registers with register.

: mb ( 8b "name" -- )
  (c does> ( disp regph -- ) ( disp regph pfa )
  CB c, c@ c, drop c, ;
  \ Rotation with index registers.

: mc ( 8b "name" -- )
  (c does> ( disp regph bit -- ) ( disp regph bit pfa )
  CB c, c@ rot drop rot c, swap 8* + c, ;  -->
  \ Bit manipulation with index registers.

( assembler )

  \ Opcodes

00 m1 nop, 02 m3 stap, 03 m3 incp, 04 m3 inc, 05 m3 dec, 07 m1
rlca, 08 m1 exaf, 09 m3 addp, 0A m3 ftap, 0B m3 decp, 0F m1
rrca, 10 m9 djnz, 17 m1 rla, 18 m9 jr,  1F m1 rra, 22 m5 sthl,
27 m1 daa, 2A m5 fthl, 2F m1 cpl, 32 m5 sta, 37 m1 scf, 3A m5
fta, 3F m1 ccf, 76 m1 halt, 80 m2 add, 88 m2 adc, 90 m2 sub, 98
m2 sbc, B8 m2 cp, C1 m3 pop, C5 m3 push, C6 m4 add#, C7 m2 rst,
C9 m1 ret, CE m4 adc#, D3 m4 out, 41 m3 outbc, D6 m4 sub#, D9
m1 exx, DB m4 in, 40 m3 inbc, 0DE m4 sbc#, E3 m1 exsp, E6 m4
and#, E9 m1 jphl, EB m1 exde, EE m4 xor#, F3 m1 di,  F6 m4 or#,
F9 m1 ldsp, FB m1 ei, FE m4 cp#, 00 m6 rlc, 08 m6 rrc, 10 m6
rl, 18 m6 rr, 20 m6 sla, 28 m6 sra, 38 m6 srl,  40 m7 bit, 80
m7 res, C0 m7 set, A0ED m8 ldi, B0ED m8 ldir, A8ED m8 ldd, B8ED
m8 lddr, 44ED m8 neg, 57ED m8 ldai, 47ED m8 ldia, 56ED m8 im1,
5EED m8 im2, B1ED m8 cpir, 6FED m8 rld, A0 m2 and, B0 m2 or,
A8 m2 xor, -->

( assembler )

  \ Opcodes

: jpix, ( -- ) ix-op c, jphl, ;
  \ XXX TODO -- study changes needed to use `ix jpp` or similar
  \ instead.

: ldp#, ( 16b regp -- ) 8* 1+ c, , ;
: ld#, ( 8b reg -- ) 8* 06 + c, c, ;
: ld, ( reg1 reg2 -- ) 8* 40 + + c, ;
: sbcp, ( regp -- ) ED c, 8* 42 + c, ;
: adcp, ( regp1 regp2 -- ) ED c, 8* 4A + c, ;
: stp, ( a regp -- ) ED c, 8* 43 + c, , ;
: ftp, ( a regp -- ) ED c, 8* 4B + c, , ;

: addix, ( regp -- ) ix-op c, addp, ;
: addiy, ( regp -- ) iy-op c, addp, ;

  \ Macros

: clr, ( regp -- ) 0 swap ld#, ;
  \ Macro to clear an 8-bit register with zero.
: clrp, ( regp -- ) 0 swap ldp#, ;
  \ Macro to clear a 16-bit register with zero.
: ldp, ( regp1 regp2 -- ) 2dup ld, 1+ swap 1+ swap ld, ;
  \ Macro, 16-bit register load.
: subp, ( regp -- ) a and, sbcp, ;
  \ Macro, 16-bit subtract.
: tstp, ( regp -- ) dup a ld, 1+ or, ;  -->
  \ Macro to test 16-bit register for zero.

( assembler )

  \ ZX Spectrum specific

CF m4 hook,  \ rst $08
D7 m1 prt,   \ rst $16

  \ Index register opcodes

86 ma addx, 8E ma adcx, 96 ma subx, 9E ma sbcx, A6 ma andx,
AE ma xorx, B6 ma orx,  BE ma cpx,  34 ma incx, 35 ma decx,
06 mb rlcx, 0E mb rrcx, 16 mb rlx,  1E mb rrx,  26 mb slax,
2E mb srax, 3E mb srlx, 46 mc bitx, 86 mc resx, C6 mc setx,

: ftx, ( disp regpi reg -- ) nip 8* 46 + c, c, ;
: stx, ( reg disp regpi -- ) drop swap 70 + c, c, ;
: st#x, ( 8b disp regpi -- ) drop 36 c, swap c, c, ;
: ftpx, ( disp regpi regp -- ) 3dup 1+ ftx, rot 1+ -rot ftx, ;
: stpx, ( disp regpi regp -- ) 3dup 1+ stx, rot 1+ -rot stx, ;

-->

( assembler )

  \ Conditions (Z80 opcodes for the required absolute jump
  \ instruction)

C2 cconstant nz?  CA cconstant z?
D2 cconstant nc?  DA cconstant c?
E2 cconstant po?  EA cconstant pe?
F2 cconstant p?   FA cconstant m?

: jp>jr ( op1 -- op2 )
  dup C3 = if drop 18 exit then dup c? > #-273 ?throw A2 - ;
  \ Convert an absolute-jump opcode to its relative-jump
  \ equivalent.  Throw error #-273 if the jump condition is
  \ invalid.
  \
  \ Note: Opcodes: $C3 is `jp`; $18 is `jr`.

: ?ret, ( op -- ) 2- c, ;
  \ Conditional return.

: ?jp, ( a op -- ) c, , ;
  \ Conditional absolute jump.

: ?call, ( a op -- ) 2+ ?jp, ;
  \ Conditional call.

: ?jr, ( a op -- ) jp>jr (jr,) ;
  \ Conditional relative jump.

  \ Control structures with relative jumps

: >rmark ( -- orig ) here 1- ;
  \ Leave the origin address of a forward relative branch
  \ just compiled, to be resolved by `>rresolve`.

: rresolve ( orig dest -- ) 1- over - dup ?rel swap c! ;
  \ Resolve a relative branch.

: >rresolve ( orig -- ) here rresolve ;
  \ Resolve a forward relative branch.

: <rresolve ( dest -- ) here 1- swap rresolve ; -->
  \ Resolve a backward relative branch.

( assembler )

  \ Control structures with relative jumps

: rahead ( -- orig ) 18 , >rmark ;
  \ Create a relative branch forward.
  \ Leave the origin address of a forward relative branch
  \ just compiled, to be resolved by `>rresolve`.
  \
  \ Note: $18 is the Z80 opcode for `jr`.

: (rif) ( op -- orig cs-id ) , >rmark 0A ;

: rif ( op -- orig cs-id ) jp>jr inverse-cond (rif) ;

: rthen ( orig cs-id -- ) 0A ?pairs >rresolve ;

: relse ( orig cs-id -- orig cs-id )
  0A ?pairs 18 (rif) rot swap rthen 0A ;
  \ Note: $18 is the opcode of `jr`.

: rbegin ( -- dest cs-id ) <mark 0B ;

: rwhile ( op -- orig cs-id ) rif 2+ ;

: (runtil) ( dest cs-id op -- ) , 0B ?pairs <rresolve ;
  \ Compile a relative conditional jump.
  \ ``(runtil)`` is common factor of `runtil` and `rstep`.

: runtil ( dest cs-id op -- ) jp>jr inverse-cond (runtil) ;
  \ End a `rbegin runtil` loop.

: ragain ( dest cs-id -- ) 18 (runtil) ;
  \ End a `rbegin ragin` loop by compiling `jr`.
  \
  \ Note: $18 is the opcode of `jr`.

: rrepeat ( dest cs-id1 orig cs-id2 --) 2swap ragain 2- rthen ;
  \ End a `rbegin rrepeat` loop.

: rstep ( dest cs-id -- ) 10 (runtil) ;
  \ End a `rbegin rstep` loop by compiling `djnz`.
  \
  \ Note: $10 is the Z80 opcode for `djnz`.

base ! set-current set-order

( aif athen aelse abegin awhile auntil aagain arepeat )

  \ Control structures with absolute jumps

get-order get-current
only forth-wordlist set-current         need ?pairs
assembler-wordlist >order set-current   need inverse-cond

: (aif) ( op -- orig cs-id ) c, >mark $08 ;

: aif ( op -- orig cs-id ) inverse-cond (aif) ;

: athen ( orig cs-id -- ) $08 ?pairs >resolve ;

: aelse ( orig cs-id -- orig cs-id )
  $08 ?pairs $C3 (aif) rot swap athen $08 ;
  \ Note: $C3 is the opcode of `jp`.

: abegin ( -- dest cs-id ) <mark $09 ;

: awhile ( op -- orig cs-id ) aif 2+ ;

: auntil  inverse-cond c, $09 ?pairs <resolve ;
  \ ( dest cs-id op -- )
  \ Compile an absolute conditional jump.

: aagain ( cs-id -- ) $C3 auntil ;
  \ Compile an absolute jump.
  \
  \ Note: $C3 is the opcode of `jp`

: arepeat ( dest cs-id1 orig cs-id2 ) 2swap aagain 2- athen ;

set-current set-order

( inverse-cond >amark >aresolve ?rel unresolved )

[unneeded] inverse-cond ?\ : inverse-cond ( op1 -- op2) 8 xor ;

  \ doc{
  \
  \ inverse-cond ( op1 -- op2)
  \
  \ Convert an assembler condition flag (actually, an absolute
  \ jump opcode) to its opposite.
  \
  \ Examples: `c?` to `nc?`; `nz?` to `z?`, etc.
  \
  \ }doc

[unneeded] >amark ?\ : >amark ( -- a ) here 2- ;

  \ doc{
  \
  \ >amark ( -- a )
  \
  \ Leave the address of an assembler absolute forward
  \ reference.
  \
  \ }doc

[unneeded] >aresolve ?( need >amark

: >aresolve ( a -- ) >amark swap ! ; ?)

  \ doc{
  \
  \ >aresolve ( a -- )
  \
  \ Resolve an assembler absolute forward reference.
  \
  \ See also: `>amark`.
  \
  \ }doc

[unneeded] ?rel

?\ : ?rel ( n -- ) 80 + FF swap u< #-269 ?throw ;

  \ doc{
  \
  \ ?rel ( n -- )
  \
  \ If assembler relative branch _n_ is too long, throw
  \ exception #-269 (relative jump too long).
  \
  \ }doc

[unneeded] unresolved ?( need array>

create unresolved0> ( -- a ) 8 cells allot

  \ doc{
  \
  \ unresolved0> ( -- a )
  \
  \ Address _a_ is the default value of `unresolved>`: an
  \ 8-cell array.
  \
  \ }doc

variable unresolved> ( -- a ) unresolved0> unresolved> !

  \ doc{
  \
  \ unresolved> ( -- a )
  \
  \ A variable. Address _a_ contains the address of a cell
  \ array accessed by `unresolved`. Its default value is
  \ `unresolved0>`, which is an 8-cell array.
  \
  \ The cell array pointed by ``unresolved>`` is used to store
  \ unresolved addresses during the compilation of code words.
  \ This method is a simpler alternative to labels created by
  \ `l:`.
  \
  \ }doc

: unresolved ( n -- a ) unresolved> @ array> ; ?)

  \ doc{
  \
  \ unresolved ( n -- a )
  \
  \ _a_ is the address of element _n_ of the cell array pointed
  \ by `unresolved>`.
  \
  \ }doc

( execute-hl, call-xt, )

  \ Assembler macros to call any Forth word from code words.

  \ Credit:
  \
  \ Code inspired by Spectrum Forth-83, where similar code is
  \ embedded in `KEY` and `PAUSE` to call an xt hold in a
  \ variable.  The code was factored to two assembler macros in
  \ order to make it reusable.

need assembler need macro need >amark need >aresolve

macro execute-hl, ( -- )
  0000 b stp,  >amark      \ save the Forth IP
  0000 b ldp#, >amark      \ point IP to phony_compiled_word
  jphl,                    \ execute the xt in HL
  >resolve                 \ phony_compiled_word
  here cell+ ,             \ point to the phony xt following
  0000 b ldp#  >aresolve   \ restore the Forth IP
  endm

  \ doc{
  \
  \ execute-hl, ( -- )
  \
  \ Compile an `execute` with the _xt_ hold in the HL register.
  \ ``execute-hl,`` is used to call Forth words from Z80.
  \
  \ See also: `call-xt,`.
  \
  \ }doc

macro call-xt, ( xt -- )
  h ldp#,  execute-hl,
  endm

  \ doc{
  \
  \ call-xt, ( xt -- )
  \
  \ Compile a call to _xt_.
  \ ``call-xt,`` is the low-level equivalent of `execute`.
  \
  \ See also: `execute-hl,`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-12-25: First changes to the previous version, which
  \ is called `z80-asm`:
  \
  \   1. "," suffixes in Z80 instructions;
  \   2. one single set of conditions;
  \   3. "a" and "r" prefixes in control structures;
  \   4. condition "m" is renamed to "ne".

  \ 2016-04-11: Moved `macro` to its own module.
  \
  \ 2016-04-13: Made `calc` independent from the assembler and
  \ moved it to the floating point module.  Fixed `execute-hl`,
  \ then renamed it and `call-xt` with a trailing comma, to
  \ avoid loading them instead of the versions written for the
  \ first assembler.

  \ 2016-05-08:
  \
  \ - Rename conditions to the original names plus "?".
  \ - Rename `|mark` to `>amark`.
  \ - Rename `|resolve` to `>aresolve`.
  \ - Rename "resmark"-like words to "rmark"-like.
  \ - Rename "resresolve"-like words to "rresolve"-like.
  \ - Remove "retCOND"-like and "callCOND"-like macros.
  \ - Compact the blocks.
  \ - Add `?jp` and `?jr` for conditional jumps.
  \ - Remove "jpCOND"-like and "jrCOND"-like opcodes.
  \ - Change the opcode values of the conditions.
  \ - Rename `?page` to `?jr-range`.
  \ - Rename `clr,` to `clrp,`; add new `clr,`.

  \ 2016-05-09: Save and restore the compile word list, the
  \ current radix and the search order.
  \
  \ 2016-11-14: Now `call,` is defined in the kernel, where it
  \ existed with the old name `code-field,`. Compact the code,
  \ save one block.  Move `8*` to the 1-cell operators module.
  \
  \ 2016-11-19: Now `jp,` is defined in the kernel, factored
  \ from `defer`.
  \
  \ 2016-12-06: Rename `?jr-range` to `?rel` and make it
  \ consume its argument, in order to reuse it in the future
  \ implementation of local labels.
  \
  \ 2016-12-20: Fix stack comments of `rrepeat` and `auntil`.
  \ Fix `jp>jr` to manage also unconditional jumps. Factor
  \ `?call` with `?jp`.  Fix `relse`, `rwhile` and `runtil`.
  \
  \ 2016-12-25: Fix `jp>jr`. Rename `im1`, `im2` to `im1,`
  \ `im2,`. Make `inverse-cond` and `jp>jr` compatible with
  \ `z80-asm`, in case `z80-asm` was loaded first.
  \
  \ 2016-12-26: Factor `runtil`, fix `rstep`.
  \
  \ 2016-12-31: Fix: Move the location of unresolved references
  \ from the string stack to data space. Using the string stack
  \ is not safe for this.
  \
  \ 2017-01-02: Fix `runtil`, `auntil` and `ragain`.
  \
  \ 2017-01-05: The previous version of this assembler has been
  \ deleted. Rename this module from
  \ <assembler.z80-asm-comma.fsb> to <assembler.fsb>. Remove
  \ `bc`, `de` and `hl`. Move `assembler` from the kernel.
  \ Remove `z80-asm,`.
  \
  \ 2017-01-07: Improve the Z80 registers stack notation.
  \ Update `wid>vocabulary` to `wordlist>vocabulary`.
  \
  \ 2017-02-12: Fix `relse` and `aelse`.
  \
  \ 2017-02-13: Replace `constant` with `cconstant`.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-02-21: Make `unresolved` optional. Make `?rel`
  \ independent from the assembler, to be reused by `l:`.
  \
  \ 2017-02-27: Add `ldi,` and `ldd,`.
  \
  \ 2017-03-11: Make absolute-jump control structures optional.
  \ Improve documentation.
  \
  \ 2017-03-13: Factor `create c,` to `(c`. This saves 13
  \ bytes. Improve documentation.

  \ vim: filetype=soloforth
