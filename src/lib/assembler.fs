  \ assembler.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202005181715
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
  \ 2015, 2016, 2017, 2018, 2018, 2020.

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
  \ ``need assembler`` will load the assembler from the
  \ library.
  \
  \ Origin: Forth-79 (Assembler Word Set), Forth-83 (Assembler
  \ Extension Word Set), Forth-94 (TOOLS EXT), Forth-2012
  \ (TOOLS EXT).
  \
  \ }doc

also assembler definitions base @ hex

need ?rel need inverse-cond

: ed, ( -- )  ED c, ;

  \ Registers

0 cconstant b   1 cconstant c   2 cconstant d   3 cconstant e
4 cconstant h   5 cconstant l   6 cconstant m   7 cconstant a

  \ doc{
  \
  \ b ( -- reg )
  \
  \ Return the identifier _reg_ of Z80 register "B", which is
  \ interpreted as register pair "BC" by assembler words that
  \ use register pairs (for example `ldp,`).
  \
  \ See: `<<src-lib-assembler-fs,a>>`,
  \ `<<src-lib-assembler-fs,c>>`, `<<src-lib-assembler-fs,d>>`,
  \ `<<src-lib-assembler-fs,e>>`, `<<src-lib-assembler-fs,h>>`,
  \ `<<src-lib-assembler-fs,l>>`, `<<src-lib-assembler-fs,m>>`,
  \ `ix`, `iy`, `sp`.
  \
  \ }doc

  \ doc{
  \
  \ c ( -- reg )
  \
  \ Return the identifier _reg_ of Z80 register "C".
  \
  \ See: `<<src-lib-assembler-fs,a>>`,
  \ `<<src-lib-assembler-fs,b>>`, `<<src-lib-assembler-fs,d>>`,
  \ `<<src-lib-assembler-fs,e>>`, `<<src-lib-assembler-fs,h>>`,
  \ `<<src-lib-assembler-fs,l>>`, `<<src-lib-assembler-fs,m>>`,
  \ `ix`, `iy`, `sp`.
  \
  \ }doc

  \ doc{
  \
  \ d ( -- reg )
  \
  \ Return the identifier _reg_ of Z80 register "D", which is
  \ interpreted as register pair "DE" by assembler words that
  \ use register pairs (for example `ldp,`).
  \
  \ See: `<<src-lib-assembler-fs,a>>`,
  \ `<<src-lib-assembler-fs,b>>`, `<<src-lib-assembler-fs,c>>`,
  \ `<<src-lib-assembler-fs,e>>`, `<<src-lib-assembler-fs,h>>`,
  \ `<<src-lib-assembler-fs,l>>`, `<<src-lib-assembler-fs,m>>`,
  \ `ix`, `iy`, `sp`.
  \
  \ }doc

  \ doc{
  \
  \ e ( -- reg )
  \
  \ Return the identifier _reg_ of Z80 register "E".
  \
  \ See: `<<src-lib-assembler-fs,a>>`,
  \ `<<src-lib-assembler-fs,b>>`, `<<src-lib-assembler-fs,c>>`,
  \ `<<src-lib-assembler-fs,d>>`, `<<src-lib-assembler-fs,h>>`,
  \ `<<src-lib-assembler-fs,l>>`, `<<src-lib-assembler-fs,m>>`,
  \ `ix`, `iy`, `sp`.
  \
  \ }doc

  \ doc{
  \
  \ h ( -- reg )
  \
  \ Return the identifier _reg_ of Z80 register "H", which is
  \ interpreted as register pair "HL" by assembler words that
  \ use register pairs (for example `ldp,`).
  \
  \ See: `<<src-lib-assembler-fs,a>>`,
  \ `<<src-lib-assembler-fs,b>>`, `<<src-lib-assembler-fs,c>>`,
  \ `<<src-lib-assembler-fs,d>>`, `<<src-lib-assembler-fs,e>>`,
  \ `<<src-lib-assembler-fs,l>>`, `<<src-lib-assembler-fs,m>>`,
  \ `ix`, `iy`, `sp`.
  \
  \ }doc

  \ doc{
  \
  \ l ( -- reg )
  \
  \ Return the identifier _reg_ of Z80 register "L".
  \
  \ See: `<<src-lib-assembler-fs,a>>`,
  \ `<<src-lib-assembler-fs,b>>`, `<<src-lib-assembler-fs,c>>`,
  \ `<<src-lib-assembler-fs,d>>`, `<<src-lib-assembler-fs,e>>`,
  \ `<<src-lib-assembler-fs,h>>`, `<<src-lib-assembler-fs,m>>`,
  \ `ix`, `iy`, `sp`.
  \
  \ }doc

  \ doc{
  \
  \ m ( -- reg )
  \
  \ Return the identifier _reg_ of Z80 pseudo-register "(HL)",
  \ i.e. the byte stored in the memory address pointed by
  \ register pair "HL".
  \
  \ See: `<<src-lib-assembler-fs,a>>`,
  \ `<<src-lib-assembler-fs,b>>`, `<<src-lib-assembler-fs,c>>`,
  \ `<<src-lib-assembler-fs,d>>`, `<<src-lib-assembler-fs,e>>`,
  \ `<<src-lib-assembler-fs,h>>`, `<<src-lib-assembler-fs,l>>`,
  \ `ix`, `iy`, `sp`.
  \
  \ }doc

  \ doc{
  \
  \ a ( -- reg )
  \
  \ Return the identifier _reg_ of Z80 register "A", which is
  \ interpreted as register pair "AF" by assembler words that
  \ use register pairs (for example `push,` and `pop,`).
  \
  \ See: `<<src-lib-assembler-fs,b>>`,
  \ `<<src-lib-assembler-fs,c>>`, `<<src-lib-assembler-fs,d>>`,
  \ `<<src-lib-assembler-fs,e>>`, `<<src-lib-assembler-fs,h>>`,
  \ `<<src-lib-assembler-fs,l>>`, `<<src-lib-assembler-fs,m>>`,
  \ `ix`, `iy`, `sp`.
  \
  \ }doc

6 cconstant sp

  \ doc{
  \
  \ sp ( -- regp ) "s-p"
  \
  \ Return the identifier _reg_ of Z80 register "sp".
  \
  \ See: `<<src-lib-assembler-fs,a>>`,
  \ `<<src-lib-assembler-fs,b>>`, `<<src-lib-assembler-fs,c>>`,
  \ `<<src-lib-assembler-fs,d>>`, `<<src-lib-assembler-fs,e>>`,
  \ `<<src-lib-assembler-fs,h>>`, `<<src-lib-assembler-fs,l>>`,
  \ `<<src-lib-assembler-fs,m>>`, `ix`, `iy`.
  \
  \ }doc

DD cconstant ix-op  FD cconstant iy-op

: ix ( -- regpi ) ix-op c, h ;

  \ doc{
  \
  \ ix ( -- regpi ) "i-x"
  \
  \ _regpi_ is the identifier of Z80 register "ix".
  \
  \ See: `<<src-lib-assembler-fs,a>>`,
  \ `<<src-lib-assembler-fs,b>>`, `<<src-lib-assembler-fs,c>>`,
  \ `<<src-lib-assembler-fs,d>>`, `<<src-lib-assembler-fs,e>>`,
  \ `<<src-lib-assembler-fs,h>>`, `<<src-lib-assembler-fs,l>>`,
  \ `<<src-lib-assembler-fs,m>>`, `iy`, `sp`.
  \
  \ }doc

: iy ( -- regpi ) iy-op c, h ;

  \ doc{
  \
  \ iy ( -- regpi ) "i-y"
  \
  \ _regpi_ is the identifier of Z80 register "iy".
  \
  \ See: `<<src-lib-assembler-fs,a>>`,
  \ `<<src-lib-assembler-fs,b>>`, `<<src-lib-assembler-fs,c>>`,
  \ `<<src-lib-assembler-fs,d>>`, `<<src-lib-assembler-fs,e>>`,
  \ `<<src-lib-assembler-fs,h>>`, `<<src-lib-assembler-fs,l>>`,
  \ `<<src-lib-assembler-fs,m>>`, `ix`, `sp`.
  \
  \ }doc

-->

( assembler )

  \ Defining words for z80 instructions

: (c ( b "name" -- ) create c, ;

: m1 ( 8b "name" -- ) (c does> ( -- ) ( dfa ) c@ c, ;
  \ 1-byte opcode without parameters.

: m2 ( 8b "name" -- ) (c does> ( reg -- ) ( reg dfa ) c@ + c, ;
  \ 1-byte opcode with register encoded in bits 0-3.

: m3 ( 8b "name" -- )
  (c does> ( reg -- ) ( reg dfa ) c@ swap 8* + c, ;
  \ 1-byte opcode with register encoded in bits 3-5.

: m3p ( 8b "name" -- )
  (c does> ( reg -- )
  ( reg dfa ) c@ swap %11111110 and 8* + c, ;
  \ 1-byte opcode with register encoded in bits 3-5, accepting
  \ any register in range A..L. `m3p` is a variant of `m3`
  \ which is used to define `push,` and `pop,`. This way
  \ those instructions accept register A instead of double
  \ register AF, making the syntax regular.

: m4 ( 8b "name" -- ) (c does> ( 8b -- ) ( 8b dfa ) c@ c, c, ;
  \ 1-byte opcode with 1-byte parameter.

: m5 ( 8b "name" -- ) (c does> ( 16b -- ) ( 16b dfa ) c@ c, , ;
  \ 1-byte opcode with 2-byte parameter.

: m6 ( 8b "name" -- )
  (c does> ( reg -- ) ( reg dfa ) CB c, c@ + c, ;
  \ Rotation of registers.

: m7 ( 8b "name" -- )
  (c does> ( reg bit -- )
  ( reg bit dfa ) CB c, c@ swap 8* + + c, ;  -->
  \ Bit manipulation of registers.

( assembler )

  \ Defining words for z80 instructions

: m8 ( 16b "name" -- ) create , does> ( -- ) ( dfa ) @ , ;
  \ 2-byte opcodes.

: (jr, ( a op -- ) c, here 1+ - dup ?rel c, ;

  \ doc{
  \
  \ (jr, ( a op -- ) "paren-j-r-comma"
  \
  \ Compile a relative jump _op_ to absolute address _a_.
  \
  \ ``(jr,`` is a factor of `jr,`.
  \
  \ }doc

: m9 ( 8b "name" -- ) (c does> ( a -- ) ( a dfa ) c@ (jr, ;
  \ Relative jumps.

: ma ( 8b "name" -- )
  (c does> ( disp regph -- ) ( disp regph dfa ) c@ c, drop c, ;
  \ Index registers with register.

: mb ( 8b "name" -- )
  (c does> ( disp regph -- ) ( disp regph dfa )
  CB c, c@ c, drop c, ;
  \ Rotation with index registers.

: mc ( 8b "name" -- )
  (c does> ( disp regph bit -- ) ( disp regph bit dfa )
  CB c, c@ rot drop rot c, swap 8* + c, ;  -->
  \ Bit manipulation with index registers.

( assembler )

  \ Opcodes

00 m1 nop, 02 m3 stap, 03 m3 incp, 04 m3 inc, 05 m3 dec, 07 m1
rlca, 08 m1 exaf, 09 m3 addp, 0A m3 ftap, 0B m3 decp, 0F m1
rrca, 10 m9 djnz, 17 m1 rla, 18 m9 jr,  1F m1 rra, 22 m5 sthl,
27 m1 daa, 2A m5 fthl, 2F m1 cpl, 32 m5 sta, 37 m1 scf, 3A m5
fta, 3F m1 ccf, 76 m1 halt, 80 m2 add, 88 m2 adc, 90 m2 sub, 98
m2 sbc, B8 m2 cp, C1 m3p pop, C5 m3p push, C6 m4 add#, C7 m2
rst, C9 m1 ret, CE m4 adc#, D3 m4 out, 41 m3 outbc, D6 m4 sub#,
D9 m1 exx, DB m4 in, 40 m3 inbc, 0DE m4 sbc#, E3 m1 exsp, E6 m4
and#, E9 m1 jphl, EB m1 exde, EE m4 xor#, F3 m1 di,  F6 m4 or#,
F9 m1 ldsp, FB m1 ei, FE m4 cp#, 00 m6 rlc, 08 m6 rrc, 10 m6
rl, 18 m6 rr, 20 m6 sla, 28 m6 sra, 30 m6 sll, 38 m6 srl,  40
m7 bit, 80 m7 res, C0 m7 set, A0ED m8 ldi, B0ED m8 ldir, A8ED
m8 ldd, B8ED m8 lddr, 44ED m8 neg, 57ED m8 ldai, 47ED m8 ldia,
56ED m8 im1, 5EED m8 im2, B1ED m8 cpir, 6FED m8 rld, A0 m2 and,
B0 m2 or, A8 m2 xor, 5FED m8 ldar, 4FED m8 ldra, -->

  \ doc{
  \
  \ nop, ( -- ) "nop-comma"
  \
  \ Compile the Z80 instruction ``NOP``.
  \
  \ }doc

  \ doc{
  \
  \ stap, ( regp -- ) "s-t-a-p-comma"
  \
  \ Compile the Z80 instruction ``LD (_regp_),A``.
  \
  \ See: `ftap,`.
  \
  \ }doc

  \ doc{
  \
  \ incp, ( regp -- ) "inc-p-comma"
  \
  \ Compile the Z80 instruction ``INC _regp_``.
  \
  \ See: `decp,`, `inc,`.
  \
  \ }doc

  \ doc{
  \
  \ inc, ( reg -- ) "inc-comma"
  \
  \ Compile the Z80 instruction ``INC _reg_``.
  \
  \ See: `dec,`, `incp,`.
  \
  \ }doc

  \ doc{
  \
  \ dec, ( reg -- ) "dec-comma"
  \
  \ Compile the Z80 instruction ``DEC _reg_``.
  \
  \ See: `decp,`, `inc,`.
  \
  \ }doc

  \ doc{
  \
  \ rlca, ( -- ) "r-l-c-a-comma"
  \
  \ Compile the Z80 instruction ``RLCA``.
  \
  \ See: `rrca,`, `rlc,`, `rl,`, `rla,`, `rld,`.
  \
  \ }doc

  \ doc{
  \
  \ exaf, ( -- ) "ex-a-f-comma"
  \
  \ Compile the Z80 instruction ``EX AF, AF'``.
  \
  \ See: `exx,`, `exde,`.
  \
  \ }doc

  \ doc{
  \
  \ addp, ( regp -- ) "add-p-comma"
  \
  \ Compile the Z80 instruction ``ADD HL,_regp_``.
  \
  \ See: `add,`.
  \
  \ }doc

  \ doc{
  \
  \ ftap, ( repg -- ) "f-t-a-p-comma"
  \
  \ Compile the Z80 instruction ``LD A,(_regp_)``.
  \
  \ See: `stap,`.
  \
  \ }doc

  \ doc{
  \
  \ decp, ( regp -- ) "dec-p-comma"
  \
  \ Compile Z80 instruction ``DEC _regp_``.
  \
  \ See: `incp,`, `dec,`.
  \
  \ }doc

  \ doc{
  \
  \ rrca, ( -- ) "r-r-c-a-comma"
  \
  \ Compile the Z80 instruction ``RRCA``.
  \
  \ See: `rlca,`, `rrc,`, `rr,`, `rra,`.
  \
  \ }doc

  \ doc{
  \
  \ djnz, ( a -- ) "d-j-n-z-comma"
  \
  \ Compile the Z80 instruction ``DJNZ n``, being _n_ an offset
  \ from the current address to address _a_.
  \
  \ See: `?jr,`, `dec,`.
  \
  \ }doc

  \ doc{
  \
  \ rla, ( -- ) "r-l-a-comma"
  \
  \ Compile the Z80 instruction ``RLA``.
  \
  \ See: `rra,`, `rl,`, `rlc,`, `rlca,`, `rld,`.
  \
  \ }doc

  \ doc{
  \
  \ jr, ( a -- ) "j-r-comma"
  \
  \ Compile the Z80 instruction ``JR n``, being _n_ an offset
  \ from the current address to address _a_.
  \
  \ See: `?jr,`, `djnz,`, `jp,`.
  \
  \ }doc

  \ doc{
  \
  \ rra, ( -- ) "r-r-a-comma"
  \
  \ Compile the Z80 instruction ``RRA``.
  \
  \ See: `rla,`, `rr,` `rrc,`, `rrca,`.
  \
  \ }doc

  \ doc{
  \
  \ sthl, ( a -- ) "s-t-h-l-comma"
  \
  \ Compile the Z80 instruction ``LD (a),HL``, i.e. store the
  \ contents of register pair "HL" into memory address _a_.
  \
  \ See: `fthl,`, `stp,`.
  \
  \ }doc

  \ doc{
  \
  \ daa, ( -- ) "d-a-a-comma"
  \
  \ Compile Z80 instruction ``DAA``.
  \
  \ }doc

  \ doc{
  \
  \ fthl, ( a -- ) "f-t-h-l-comma"
  \
  \ Compile the Z80 instruction ``LD HL,(a)``, i.e. fetch the
  \ contents of memory address _a_ into register pair "HL".
  \
  \ See: `sthl,`, `ftp,`.
  \
  \ }doc

  \ doc{
  \
  \ cpl, ( -- ) "c-p-l-comma"
  \
  \ Compile Z80 instruction ``CPL``.
  \
  \ See: `scf,`, `ccf,`, `neg,`, `and,`, `cp,`.
  \
  \ }doc

  \ doc{
  \
  \ sta, ( a -- ) "s-t-a-comma"
  \
  \ Compile the Z80 instruction ``LD (a),A``,
  \ i.e. store the contents of register "A" into memory address
  \ _a_.
  \
  \ See: `fta,`, `ld,`, `ld#,`.
  \
  \ }doc

  \ doc{
  \
  \ scf, ( -- ) "s-c-f-comma"
  \
  \ Compile Z80 instruction ``SCF``.
  \
  \ See: `cpl,`, `ccf,`, `neg,`, `set,`, `and,`.
  \
  \ }doc

  \ doc{
  \
  \ fta, ( a -- ) "f-t-a-comma"
  \
  \ Compile the Z80 instruction ``LD A,(a)``, i.e. fetch the
  \ contents of memory address _a_ into register "A".
  \
  \ See: `sta,`, `ld,`, `ld#,`.
  \
  \ }doc

  \ doc{
  \
  \ ccf, ( -- ) "c-c-f-comma"
  \
  \ Compile the Z80 instruction ``CCF``.
  \
  \ See: `cpl,`, `scf,`, `neg,`, `bit,`, `set,`, `cp,`.
  \
  \ }doc

  \ doc{
  \
  \ halt, ( -- ) "halt-comma"
  \
  \ Compile the Z80 instruction ``HALT``.
  \
  \ See: `im1,`, `im2,`, `di,`, `ei,`.
  \
  \ }doc

  \ doc{
  \
  \ add, ( reg -- ) "add-comma"
  \
  \ Compile the Z80 instruction ``ADD _reg_``.
  \
  \ See: `sub,`, `sbc,`, `addp,`.
  \
  \ }doc

  \ doc{
  \
  \ adc, ( reg -- ) "a-d-c-comma"
  \
  \ Compile the Z80 instruction ``ADC _reg_``.
  \
  \ See: `add,`, `sub,`, `sbc,`, `addp,`.
  \
  \ }doc

  \ doc{
  \
  \ sub, ( reg -- ) "sub-comma"
  \
  \ Compile the Z80 instruction ``SUB _reg_``.
  \
  \ See: `sbc,`, `add,`, `adc,`, `subp,`.
  \
  \ }doc

  \ doc{
  \
  \ sbc, ( reg -- ) "s-b-c-comma"
  \
  \ Compile the Z80 instruction ``SBC _reg_``.
  \
  \ See: `sub,`, `adc,`, `add,`, `subp,`.
  \
  \ }doc

  \ doc{
  \
  \ cp, ( reg -- ) "c-p-comma"
  \
  \ Compile the Z80 instruction ``CP _reg_``.
  \
  \ See: `tstp,`, `cpl,`.
  \
  \ }doc

  \ doc{
  \
  \ pop, ( regp -- ) "pop-comma"
  \
  \ Compile the Z80 instruction ``PUSH _regp_``.
  \
  \ See: `pop,`, `ret,`, `sp`.
  \
  \ }doc

  \ doc{
  \
  \ push, ( regp -- ) "push-comma"
  \
  \ Compile the Z80 instruction ``PUSH _regp_``.
  \
  \ See: `push,`, `ret,`, `sp`.
  \
  \ }doc

  \ doc{
  \
  \ add#, ( b -- ) "add-number-sign-comma,"
  \
  \ Compile the Z80 instruction ``ADD A,_b_``.
  \
  \ }doc

  \ doc{
  \
  \ rst, ( b -- ) "r-s-t-comma"
  \
  \ Compile the Z80 instruction ``RST _b_``.
  \
  \ }doc

  \ doc{
  \
  \ ret, ( -- ) "ret-comma"
  \
  \ Compile the Z80 instruction ``RET``.
  \
  \ See: `?ret,`, `call,`, `pop,`.
  \
  \ }doc

  \ doc{
  \
  \ adc#, ( b -- ) "a-d-c-number-sign-comma"
  \
  \ Compile the Z80 instruction ``ADC A,_b_``.
  \
  \ }doc

  \ doc{
  \
  \ out, ( b -- ) "out-comma"
  \
  \ Compile Z80 instruction ``OUT (b),A``.
  \
  \ See: `in,`, `outbc,`.
  \
  \ }doc

  \ doc{
  \
  \ outbc, ( reg -- ) "out-b-c-comma"
  \
  \ Compile Z80 instruction ``OUT (C),_reg_``.
  \
  \ See: `inbc,`, `out,`.
  \
  \ }doc

  \ doc{
  \
  \ sub#, ( b -- ) "sub-number-sign-comma"
  \
  \ Compile the Z80 instruction ``SUB _b_``.
  \
  \ }doc

  \ doc{
  \
  \ exx, ( -- ) "ex-x-comma"
  \
  \ Compile the Z80 instruction ``EXX``.
  \
  \ See: `exde,`, `exaf,`.
  \
  \ }doc

  \ doc{
  \
  \ in, ( b -- ) "in-comma"
  \
  \ Compile the Z80 instruction ``IN A,(b)``.
  \
  \ See: `out,`, `inbc,`.
  \
  \ }doc

  \ doc{
  \
  \ inbc, ( reg -- ) "in-b-c-comma"
  \
  \ Compile the Z80 instruction ``IN _reg_,(C)``.
  \
  \ See: `outbc,` `in,`.
  \
  \ }doc

  \ doc{
  \
  \ sbc#, ( b -- ) "s-b-c-number-sign-comma"
  \
  \ Compile the Z80 instruction ``SBC A,_b_``.
  \
  \ }doc

  \ doc{
  \
  \ exsp, ( -- ) "ex-s-p-comma"
  \
  \ Compile the Z80 instruction ``EX (SP),HL``.
  \
  \ }doc

  \ doc{
  \
  \ and#, ( b -- ) "and-number-sign-comma"
  \
  \ Compile the Z80 instruction ``AND _b_``.
  \
  \ See: `or#,`, `xor#,`, `sub#,`.
  \
  \ }doc

  \ doc{
  \
  \ jphl, ( -- ) "j-p-h-l-comma"
  \
  \ Compile the Z80 instruction ``JP (HL)``.
  \
  \ See: `jpix,`.
  \
  \ }doc

  \ doc{
  \
  \ exde, ( -- ) "ex-de-comma"
  \
  \ Compile the Z80 instruction ``EX DE,HL``.
  \
  \ See: `exaf,`, `exx,`.
  \
  \ }doc

  \ doc{
  \
  \ xor#, ( b -- ) "x-or-number-sign-comma"
  \
  \ Compile the Z80 instruction ``XOR _b_``.
  \
  \ See: `or#,`, `and#,`, `add#,`, `sub#,`.
  \
  \ }doc

  \ doc{
  \
  \ di, ( -- ) "d-i-comma"
  \
  \ Compile the Z80 instruction ``DI``.
  \
  \ See: `ei,`, `im1,`, `im2,`, `halt,`.
  \
  \ }doc

  \ doc{
  \
  \ or#, ( b -- ) "or-number-sign-comma"
  \
  \ Compile the Z80 instruction ``OR _b_``.
  \
  \ See: `xor#,`, `and#,`, `add#,`.
  \
  \ }doc

  \ XXX TODO -- Document the others variants of ``LD SP,X``:

  \ doc{
  \
  \ ldsp, ( -- ) "l-d-s-p-comma"
  \
  \ Compile the Z80 instruction ``LD SP,HL``.
  \
  \ }doc

  \ doc{
  \
  \ ei, ( -- ) "e-i-comma"
  \
  \ Compile the Z80 instruction ``EI``.
  \
  \ See: `di,`, `im1,`, `im2,`, `halt,`.
  \
  \ }doc

  \ doc{
  \
  \ cp#, ( b -- ) "c-p-number-sign-comma"
  \
  \ Compile the Z80 instruction ``CP _b_``.
  \
  \ }doc

  \ doc{
  \
  \ rlc, ( reg -- ) "r-l-c-comma"
  \
  \ Compile the Z80 instruction ``RLC _reg_``.
  \
  \ See: `rrc,`, `rlca,`, `rl,`, `rla,`.
  \
  \ }doc

  \ doc{
  \
  \ rrc, ( reg -- ) "r-r-c-comma"
  \
  \ Compile the Z80 instruction ``RRC _reg_``.
  \
  \ See: `rlc,`, `rr,`, `rra,`, `rrca,`.
  \
  \ }doc

  \ doc{
  \
  \ rl, ( reg -- ) "r-l-comma"
  \
  \ Compile the Z80 instruction ``RL _reg_``.
  \
  \ See: `rr,`, `rla,`, `rlc,`, `rlca,`.
  \
  \ }doc

  \ doc{
  \
  \ rr, ( reg -- ) "r-r-comma"
  \
  \ Compile the Z80 instruction ``RR _reg_``.
  \
  \ See: `rl,`, `rra,`, `rrc,`, `rrca,`.
  \
  \ }doc

  \ doc{
  \
  \ sla, ( reg -- ) "s-l-a-comma"
  \
  \ Compile the Z80 instruction ``SLA _reg_``.
  \
  \ }doc

  \ doc{
  \
  \ sra, ( reg -- ) "s-r-a-comma"
  \
  \ Compile the Z80 instruction ``SRA _reg_``.
  \
  \ }doc

  \ doc{
  \
  \ sll, ( reg -- ) "s-l-l-comma"
  \
  \ Compile the Z80 instruction ``SLL _reg_``.
  \
  \ }doc

  \ doc{
  \
  \ srl, ( reg -- ) "s-r-l-comma"
  \
  \ Compile the Z80 instruction ``SRL _reg_``.
  \
  \ }doc

  \ doc{
  \
  \ bit, ( reg b -- ) "bit-comma"
  \
  \ Compile the Z80 instruction ``BIT _b_,_reg_``.
  \
  \ See: `res,`, `set,`, `cp#,`.
  \
  \ }doc

  \ doc{
  \
  \ res, ( reg b -- ) "res-comma"
  \
  \ Compile the Z80 instruction ``RES _b_,_reg_``.
  \
  \ See: `bit,`, `set,`, `sub#,`.
  \
  \ }doc

  \ doc{
  \
  \ set, ( reg b -- ) "set-comma"
  \
  \ Compile the Z80 instruction ``SET _b_,_reg_``.
  \
  \ See: `bit,`, `res,`, `add#,`.
  \
  \ }doc

  \ doc{
  \
  \ ldi, ( -- ) "l-d-i-comma"
  \
  \ Compile the Z80 instruction ``LDI``.
  \
  \ See: `ldd,`, `ldir,`.
  \
  \ }doc

  \ doc{
  \
  \ ldir, ( -- ) "l-d-i-r-comma"
  \
  \ Compile the Z80 instruction ``LDIR``.
  \
  \ See: `lddr,`, `ldi,`.
  \
  \ }doc

  \ doc{
  \
  \ ldd, ( -- ) "l-d-d-comma"
  \
  \ Compile the Z80 instruction ``LDD``.
  \
  \ See: `ldi,`, `lddr,`.
  \
  \ }doc

  \ doc{
  \
  \ lddr, ( -- ) "l-d-d-r-comma"
  \
  \ Compile the Z80 instruction ``LDDR``.
  \
  \ See: `ldir,`, `ldd,`.
  \
  \ }doc

  \ doc{
  \
  \ neg, ( -- ) "neg-comma"
  \
  \ Compile the Z80 instruction ``NEG``.
  \
  \ See: `cpl,`, `scf,`, `ccf,`.
  \
  \ }doc

  \ doc{
  \
  \ ldai, ( -- ) "l-d-a-i-comma"
  \
  \ Compile the Z80 instruction ``LD A,I``.
  \
  \ See: `ldia,`, `ldar,`, `ld,`.
  \
  \ }doc

  \ doc{
  \
  \ ldia, ( -- ) "l-d-i-a-comma"
  \
  \ Compile the Z80 instruction ``LD I,A``.
  \
  \ See: `ldai,`, `ldra,`, `ld,`.
  \
  \ }doc

  \ doc{
  \
  \ ldar, ( -- ) "l-d-a-r-comma"
  \
  \ Compile the Z80 instruction ``LD A,R``.
  \
  \ See: `ldra,`, `ldai,`, `ld,`.
  \
  \ }doc

  \ doc{
  \
  \ ldra, ( -- ) "l-d-r-a-comma"
  \
  \ Compile the Z80 instruction ``LD R,A``.
  \
  \ See: `ldar,`, `ldir,`, `ld,`.
  \
  \ }doc

  \ doc{
  \
  \ im1, ( -- ) "i-m-one-comma"
  \
  \ Compile the Z80 instruction ``IM 1``.
  \
  \ See: `im2,`, `di,`, `ei,`, `halt,`.
  \
  \ }doc

  \ doc{
  \
  \ im2, ( -- ) "i-m-two-comma"
  \
  \ Compile the Z80 instruction ``IM 2``.
  \
  \ See: `im1,`, `di,`, `ei,`, `halt,`.
  \
  \ }doc

  \ doc{
  \
  \ cpir, ( -- ) "c-p-i-r-comma"
  \
  \ Compile the Z80 instruction ``CPIR``.
  \
  \ See: `cp,`, `ldir,`, `djnz,`.
  \
  \ }doc

  \ doc{
  \
  \ rld, ( -- ) "r-l-d-comma"
  \
  \ Compile the Z80 instruction ``RLD``.
  \
  \ See: `rla,`, `rlca,`, `rra,`.
  \
  \ }doc

  \ doc{
  \
  \ and, ( reg -- ) "and-comma"
  \
  \ Compile the Z80 instruction ``AND _reg_``.
  \
  \ See: `xor,`, `or,`.
  \
  \ }doc

  \ doc{
  \
  \ or, ( reg -- ) "or-comma"
  \
  \ Compile the Z80 instruction ``OR _reg_``.
  \
  \ See: `and,`, `xor,`.
  \
  \ }doc

  \ doc{
  \
  \ xor, ( reg -- ) "x-or-comma"
  \
  \ Compile the Z80 instruction ``XOR _reg_``.
  \
  \ See: `and,`, `or,`.
  \
  \ }doc

( assembler )

  \ Opcodes

: jpix, ( -- ) ix-op c, jphl, ;

  \ XXX TODO -- Study changes needed to use a common syntax,
  \ for example: `hl jpreg,`, `ix jpreg,`.

  \ doc{
  \
  \ jpix, ( -- ) "j-p-i-x-comma"
  \
  \ Compile the Z80 instruction ``JP (IX)``.
  \
  \ See: `jphl,`.
  \
  \ }doc

: ldp#, ( 16b regp -- ) 8* 1+ c, , ;

  \ doc{
  \
  \ ldp#, ( 16b regp -- ) "l-d-p-number-sign-comma"
  \
  \ Compile the Z80 instruction ``LD _regp_,_16b_``.
  \
  \ See: `ldp,`, `ld#,`.
  \
  \ }doc

: ld#, ( 8b reg -- ) 8* 06 + c, c, ;

  \ doc{
  \
  \ ld#, ( 8b reg -- ) "l-d-number-sign-comma"
  \
  \ Compile the Z80 instruction ``LD _reg_,_8b_``.
  \
  \ See: `ld,`, `ldp#,`.
  \
  \ }doc

: ld, ( reg1 reg2 -- ) 8* 40 + + c, ;

  \ doc{
  \
  \ ld, ( reg1 reg2 -- ) "l-d-comma"
  \
  \ Compile the Z80 instruction ``LD _reg2_,_reg1_``.
  \
  \ See: `ld#,`, `ldp,`.
  \
  \ }doc

: sbcp, ( regp -- ) ed, 8* 42 + c, ;

  \ doc{
  \
  \ sbcp, ( regp -- ) "s-b-c-p-comma"
  \
  \ Compile the Z80 instruction ``SBC HL,_regp_``.
  \
  \ See: `subp,`, `sbc,`.
  \
  \ }doc

: adcp, ( regp1 regp2 -- ) ed, 8* 4A + c, ;

  \ doc{
  \
  \ adcp, ( regp1 regp2 -- ) "a-d-c-p-comma"
  \
  \ Compile the Z80 instruction ``ADC _regp2_,_regp1_``.
  \
  \ See: `adcp,`.
  \
  \ }doc

: stp, ( a regp -- ) ed, 8* 43 + c, , ;

  \ doc{
  \
  \ stp, ( a regp -- ) "s-t-p-comma"
  \
  \ Compile the Z80 instruction ``LD (_a_),_regp_``, i.e.
  \ store the contents of pair register _regp_ into memory
  \ address _a_.
  \
  \ NOTE: For the "HL" register there is a specific word:
  \ `fthl,`, which compiles shorten and faster code.
  \
  \ See: `ftp,`.
  \
  \ }doc

: ftp, ( a regp -- ) ed, 8* 4B + c, , ;

  \ doc{
  \
  \ ftp, ( a regp -- ) "f-t-p-comma"
  \
  \ Compile the Z80 instruction ``LD _regp_,(a)``, i.e.  fetch
  \ the contents of pair register _regp_ from memory address
  \ _a_.
  \
  \ NOTE: For the "HL" register has a specific word: `fthl,`,
  \ which compiles shorten and faster code.
  \
  \ See: `stp,`.
  \
  \ }doc

: addix, ( regp -- ) ix-op c, addp, ;

  \ doc{
  \
  \ addix, ( regp -- ) "add-i-x-comma"
  \
  \ Compile the Z80 instruction ``ADD IX,_regp_``.
  \
  \ See: `addiy,`, `addp,`.
  \
  \ }doc

: addiy, ( regp -- ) iy-op c, addp, ;

  \ doc{
  \
  \ addiy, ( regp -- ) "add-i-y-comma"
  \
  \ Compile the Z80 instruction ``ADD IY,_regp_``.
  \
  \ See: `addiy,`, `addp,`.
  \
  \ }doc

  \ Macros

: clr, ( reg -- ) 0 swap ld#, ;

  \ doc{
  \
  \ clr, ( reg -- ) "c-l-r-comma"
  \
  \ Compile the Z80 instruction ``LD _reg_,0``.
  \
  \ See: `clrp,`, `ld#,`.
  \
  \ }doc

: clrp, ( regp -- ) 0 swap ldp#, ;

  \ doc{
  \
  \ clrp, ( regp -- ) "c-l-r-p-comma"
  \
  \ Compile the Z80 instruction ``LD _regp_,0``.
  \
  \ See: `clr,`, `ldp#,`.
  \
  \ }doc

: ldp, ( regp1 regp2 -- ) 2dup ld, 1+ swap 1+ swap ld, ;

  \ doc{
  \
  \ ldp, ( regp1 regp2 -- ) "l-d-p-comma"
  \
  \ Compile the Z80 instructions required to load register pair
  \ _regp2_ with register pair _regp1_.
  \
  \ Example: ``b d ldp,`` compiles the Z80 instructions ``LD
  \ D,B`` and ``LD E,C``.
  \
  \ See: `ld,`, `subp,`, `tstp,`, `clrp,`.
  \
  \ }doc

: subp, ( regp -- ) a and, sbcp, ;

  \ doc{
  \
  \ subp, ( regp -- ) "sub-p-comma"
  \
  \ Compile the Z80 instructions required to subtract register
  \ pair _regp_ from register pair "HL".
  \
  \ Example: ``d subp,`` compiles the Z80 instructions ``AND
  \ A`` (to reset the carry flag) and ``SBC DE``.
  \
  \ See: `sbcp,`, `sub,`, `ldp,`, `tstp,`.
  \
  \ }doc

: tstp, ( regp -- ) dup a ld, 1+ or, ;  -->

  \ doc{
  \
  \ tstp, ( regp -- ) "t-s-t-p-comma"
  \
  \ Compile the Z80 instructions required to test the register
  \ pair _regp_ for zero.  Register "A" is modified.
  \
  \ Example: ``b tstp,`` compiles the Z80 instructions ``LD
  \ A,B`` and ``OR C``.
  \
  \ See: `ldp,`, `subp,`, `cp#,`, `cp,`, `or,`, `ld,`.
  \
  \ }doc

( assembler )

  \ Index register opcodes

86 ma addx, 8E ma adcx, 96 ma subx, 9E ma sbcx, A6 ma andx,
AE ma xorx, B6 ma orx,  BE ma cpx,  34 ma incx, 35 ma decx,
06 mb rlcx, 0E mb rrcx, 16 mb rlx,  1E mb rrx,
26 mb slax, 2E mb srax, 36 mb sllx, 3E mb srlx,
46 mc bitx, 86 mc resx, C6 mc setx,

  \ doc{
  \
  \ addx, ( disp regpi -- ) "add-x-comma"
  \
  \ Compile the Z80 instruction ``ADD A,(_regpi_+_disp_)``.
  \
  \ See: `adcx,`, `subx,`.
  \
  \ }doc

  \ doc{
  \
  \ adcx, ( disp regpi --  ) "a-d-c-x-comma"
  \
  \ Compile the Z80 instruction ``ADC A,(_regpi_+_disp_)``.
  \
  \ See: `addx,`, `sbcx,`.
  \
  \ }doc

  \ doc{
  \
  \ subx, ( disp regpi --  ) "sub-x-comma"
  \
  \ Compile the Z80 instruction ``SUB (_regpi_+_disp_)``.
  \
  \ See: `sbcx,`, `addx,`.
  \
  \ }doc

  \ doc{
  \
  \ sbcx, ( disp regpi --  ) "s-b-c-x-comma"
  \
  \ Compile the Z80 instruction ``SBC (_regpi_+_disp_)``.
  \
  \ See: `subx,`, `adcx,`.
  \
  \ }doc

  \ doc{
  \
  \ andx, ( disp regpi --  ) "and-x-comma"
  \
  \ Compile the Z80 instruction ``AND (_regpi_+_disp_)``.
  \
  \ See: `xorx,`, `orx,`, `cpx,`.
  \
  \ }doc

  \ doc{
  \
  \ xorx, ( disp regpi --  ) "x-or-x-comma"
  \
  \ Compile the Z80 instruction ``XOR (_regpi_+_disp_)``.
  \
  \ See: `xorx,`, `orx,`, `cpx,`.
  \
  \ }doc

  \ doc{
  \
  \ orx, ( disp regpi --  ) "or-x-comma"
  \
  \ Compile the Z80 instruction ``OR (_regpi_+_disp_)``.
  \
  \ See: `andx,`, `xorx,`, `cpx,`.
  \
  \ }doc

  \ doc{
  \
  \ cpx, ( disp regpi --  ) "c-p-x-comma"
  \
  \ Compile the Z80 instruction ``CP (_regpi_+_disp_)``.
  \
  \ See: `addx,`, `adcx,`, `subx,`, `sbcx,`, `andx,`, `xorx,`,
  \ `orx,`, `incx,`, `decx,`.
  \
  \ }doc

  \ doc{
  \
  \ incx, ( disp regpi --  ) "inc-x-comma"
  \
  \ Compile the Z80 instruction ``INC (_regp_+_disp_)``.
  \
  \ See: `decx,`, `addx,`, `adcx,`.
  \
  \ }doc

  \ doc{
  \
  \ decx, ( disp regpi --  ) "dec-x-comma"
  \
  \ Compile the Z80 instruction ``DEC (_regp_+_disp_)``.
  \
  \ See: `addx,`, `subx,`, `sbcx,`.
  \
  \ }doc

  \ doc{
  \
  \ rlcx, ( disp regpi --  ) "r-l-c-x-comma"
  \
  \ Compile the Z80 instruction ``RLC (_regpi_+_disp_)``.
  \
  \ See: `rrcx,`, `rlx,`, `rrx,`, `slax,`, `srax,`, `sllx,`,
  \ `srlx,`, `bitx,`, `resx,`, `setx,`.
  \
  \ }doc

  \ doc{
  \
  \ rrcx, ( disp regpi --  ) "r-r-c-x-comma"
  \
  \ Compile the Z80 instruction ``RRC (_regpi_+_disp_)``.
  \
  \ See: `rlcx,`, `rlx,`, `rrx,`, `slax,`, `srax,`, `sllx,`,
  \ `srlx,`, `bitx,`, `resx,`, `setx,`.
  \
  \ }doc

  \ doc{
  \
  \ rlx, ( disp regpi --  ) "r-l-x-comma"
  \
  \ Compile the Z80 instruction ``RL (_regpi_+_disp_)``.
  \
  \ See: `rlcx,`, `rrcx,`, `rrx,`, `slax,`, `srax,`, `sllx,`,
  \ `srlx,`, `bitx,`, `resx,`, `setx,`.
  \
  \ }doc

  \ doc{
  \
  \ rrx, ( disp regpi --  ) "r-r-x-comma"
  \
  \ Compile the Z80 instruction ``RR (_regpi_+_disp_)``.
  \
  \ See: `rlcx,`, `rrcx,`, `rlx,`, `slax,`, `srax,`, `sllx,`,
  \ `srlx,`, `bitx,`, `resx,`, `setx,`.
  \
  \ }doc

  \ doc{
  \
  \ slax, ( disp regpi --  ) "s-l-a-x-comma"
  \
  \ Compile the Z80 instruction ``SLA (_regpi_+_disp_)``.
  \
  \ See: `rlcx,`, `rrcx,`, `rlx,`, `rrx,`, `srax,`, `sllx,`,
  \ `srlx,`, `bitx,`, `resx,`, `setx,`.
  \
  \ }doc

  \ doc{
  \
  \ srax, ( disp regpi --  ) "s-r-a-x-comma"
  \
  \ Compile the Z80 instruction ``SRA (_regpi_+_disp_)``.
  \
  \ See: `rlcx,`, `rrcx,`, `rlx,`, `rrx,`, `slax,`, `sllx,`,
  \ `srlx,`, `bitx,`, `resx,`, `setx,`.
  \
  \ }doc

  \ doc{
  \
  \ sllx, ( disp regpi --  ) "s-l-l-x-comma"
  \
  \ Compile the Z80 instruction ``SLL (_regpi_+_disp_)``.
  \
  \ See: `rlcx,`, `rrcx,`, `rlx,`, `rrx,`, `slax,`, `srax,`,
  \ `srlx,`, `bitx,`, `resx,`, `setx,`.
  \
  \ }doc

  \ doc{
  \
  \ srlx, ( disp regpi --  ) "s-r-l-x-comma"
  \
  \ Compile the Z80 instruction ``SRL (_regpi_+_disp_)``.
  \
  \ See: `rlcx,`, `rrcx,`, `rlx,`, `rrx,`, `slax,`, `srax,`,
  \ `sllx,`, `bitx,`, `resx,`, `setx,`.
  \
  \ }doc

  \ doc{
  \
  \ bitx, ( disp regpi b --  ) "bit-x-comma"
  \
  \ Compile the Z80 instruction ``BIT _b_,(_regpi_+_disp_)``.
  \
  \ See: `resx,`, `setx,`, `cpx,`.
  \
  \ }doc

  \ doc{
  \
  \ resx, ( disp regpi b --  ) "res-x-comma"
  \
  \ Compile the Z80 instruction ``RES _b_,(_regpi_+_disp_)``.
  \
  \ See: `bitx,`, `setx,`, `subx,`, `sbcx,`, `andx,`, `xorx,`,
  \ `orx,`, `decx,`.
  \
  \ }doc

  \ doc{
  \
  \ setx, ( disp regpi b --  ) "set-x-comma"
  \
  \ Compile the Z80 instruction ``SET _b_,(_regpi_+_disp_)``.
  \
  \ See: `bitx,`, `resx,`, `addx,`, `adcx,`, `andx,`, `xorx,`,
  \ `orx,`, `incx,`.
  \
  \ }doc

: ftx, ( disp regpi reg -- ) nip 8* 46 + c, c, ;

  \ doc{
  \
  \ ftx, ( disp regpi reg -- ) "f-t-x-comma"
  \
  \ Compile the Z80 instruction ``LD
  \ _reg_,(_regpi_+_disp_)``.
  \
  \ See: `stx,`.
  \
  \ }doc

: stx, ( reg disp regpi -- ) drop swap 70 + c, c, ;

  \ doc{
  \
  \ stx, ( reg disp regpi -- ) "s-t-x-comma"
  \
  \ Compile the Z80 instruction ``LD (_regpi_+_disp_),_reg_``.
  \
  \ See: `st#x,`, `ftx,`.
  \
  \ }doc

: st#x, ( 8b disp regpi -- ) drop 36 c, swap c, c, ;

  \ doc{
  \
  \ st#x, ( 8b disp regpi -- ) "s-t-number-sign-x-comma"
  \
  \ Compile the Z80 instruction ``LD (_regpi_+_disp_),_8b_``.
  \
  \ See: `stx,`.
  \
  \ }doc

: ftpx, ( disp regpi regp -- ) 3dup 1+ ftx, rot 1+ -rot ftx, ;

  \ doc{
  \
  \ ftpx, ( disp regpi regp -- ) "f-t-p-x-comma"
  \
  \ Compile the Z80 instructions required to fetch register
  \ pair _regp_ from the address pointed by _regpi_ plus
  \ _disp_.
  \
  \ Example: ``16 ix h ftpx,`` will compile the Z80
  \ instructions ``LD L,(IX+16)`` and ``LD H,(IX+17)``.
  \
  \ See: `stpx,`, `ftx,`.
  \
  \ }doc

: stpx, ( disp regpi regp -- ) 3dup 1+ stx, rot 1+ -rot stx, ;

  \ doc{
  \
  \ stpx, ( disp regpi regp -- ) "s-t-p-x-comma"
  \
  \ Compile the Z80 instructions required to store register
  \ pair _regp_ into the address pointed by _regpi_ plus
  \ _disp_.
  \
  \ Example: ``16 ix h stpx,`` will compile the Z80
  \ instructions ``LD (IX+16),L`` and ``LD (IX+17),H``.
  \
  \ See: `ftpx,`, `stx,`.
  \
  \ }doc

-->

( assembler )

  \ Conditions (Z80 opcodes for the required absolute jump
  \ instruction)

C2 cconstant nz?  CA cconstant z?
D2 cconstant nc?  DA cconstant c?
E2 cconstant po?  EA cconstant pe?
F2 cconstant p?   FA cconstant m?

  \ doc{
  \
  \ z? ( -- op ) "z-question"
  \
  \ Return the opcode _op_ of the Z80 instruction ``jp z``, to
  \ be used as condition and consumed by `?ret,`, `?jp,`,
  \ `?call,`, `?jr,`, `aif`, `rif`, `awhile`, `rwhile`, `auntil`
  \ or `runtil`.
  \
  \ See: `nz?`, `c?`, `nc?`, `po?`, `pe?`, `p?`, `m?`.
  \
  \ }doc

  \ doc{
  \
  \ nz? ( -- op ) "n-z-question"
  \
  \ Return the opcode _op_ of the Z80 instruction ``jp nz``, to
  \ be used as condition and consumed by `?ret,`, `?jp,`,
  \ `?call,`, `?jr,`, `aif`, `rif`, `awhile`, `rwhile`,
  \ `auntil` or `runtil`.
  \
  \ See: `z?`, `c?`, `nc?`, `po?`, `pe?`, `p?`, `m?`.
  \
  \ }doc

  \ doc{
  \
  \ c? ( -- op ) "c-question"
  \
  \ Return the opcode _op_ of the Z80 instruction ``jp c``, to
  \ be used as condition and consumed by `?ret,`, `?jp,`,
  \ `?call,`, `?jr,`, `aif`, `rif`, `awhile`, `rwhile`,
  \ `auntil` or `runtil`.
  \
  \ See: `z?`, `nz?`, `nc?`, `po?`, `pe?`, `p?`, `m?`.
  \
  \ }doc

  \ doc{
  \
  \ nc? ( -- op ) "n-c-question"
  \
  \ Return the opcode _op_ of the Z80 instruction ``jp nc``,
  \ to be used as condition and consumed by
  \ `?ret,`, `?jp,`, `?call,`, `?jr,`, `aif`, `rif`, `awhile`,
  \ `rwhile`, `auntil` or `runtil`.
  \
  \ See: `z?`, `nz?`, `c?`, `po?`, `pe?`, `p?`, `m?`.
  \
  \ }doc

  \ doc{
  \
  \ po? ( -- op ) "p-o-question"
  \
  \ Return the opcode _op_ of the Z80 instruction ``jp op``,
  \ to be used as condition and consumed by
  \ `?ret,`, `?jp,`, `?call,`, `aif`, `awhile` or `auntil`.
  \
  \ See: `z?`, `nz?`, `c?`, `nc?`, `pe?`, `p?`, `m?`.
  \
  \ }doc

  \ doc{
  \
  \ pe? ( -- op ) "p-e-question"
  \
  \ Return the opcode _op_ of the Z80 instruction ``jp pe``,
  \ to be used as condition and consumed by
  \ `?ret,`, `?jp,`, `?call,`, `aif`, `awhile` or `auntil`.
  \
  \ See: `z?`, `nz?`, `c?`, `nc?`, `po?`, `p?`, `m?`.
  \
  \ }doc

  \ doc{
  \
  \ p? ( -- op ) "p-question"
  \
  \ Return the opcode _op_ of the Z80 instruction ``jp p``,
  \ to be used as condition and consumed by
  \ `?ret,`, `?jp,`, `?call,`, `aif`, `awhile` or `auntil`.
  \
  \ See: `z?`, `nz?`, `c?`, `nc?`, `po?`, `pe?`, `m?`.
  \
  \ }doc

  \ doc{
  \
  \ m? ( -- op ) "m-question"
  \
  \ Return the opcode _op_ of the Z80 instruction ``jp m``,
  \ to be used as condition and consumed by
  \ `?ret,`, `?jp,`, `?call,`, `aif`, `awhile` or `auntil`.
  \
  \ See: `z?`, `nz?`, `c?`, `nc?`, `po?`, `pe?`, `p?`.
  \
  \ }doc

: jp>jr ( op1 -- op2 )
  dup C3 = if drop 18 exit then dup c? > #-273 ?throw A2 - ;
  \ Note: Opcodes: $C3 is `jp`; $18 is `jr`.

  \ doc{
  \
  \ jp>jr ( op1 -- op2 ) "j-p-greater-than-j-r"
  \
  \ Convert an absolute-jump opcode to its relative-jump
  \ equivalent.  Throw error #-273 if the jump condition is
  \ invalid.
  \
  \ ``jp>jr`` is a common factor of `?jr,`, `rif` and `runtil`.
  \
  \ }doc

: ?ret, ( op -- ) 2- c, ;

  \ doc{
  \
  \ ?ret, ( op -- ) "question-ret-comma"
  \
  \ Compile a Z80 conditional return instruction, being _op_
  \ the identifier of the condition, which has been put on the
  \ stack by `nz?`, `c?`, `nc?`, `po?`, `pe?`, `p?`, or `m?`.
  \
  \ See: `ret,`, `?jp,`, `?call,`.
  \
  \ }doc

: ?jp, ( a op -- ) c, , ;

  \ doc{
  \
  \ ?jp, ( a op -- ) "question-j-p-comma"
  \
  \ Compile a Z80 conditional absolute jump instruction to
  \ address _a_, being _op_ the identifier of the condition,
  \ which has been put on the stack by `nz?`, `c?`, `nc?`,
  \ `po?`, `pe?`, `p?`, or `m?`.
  \
  \ See: `jp,`, `?jr,`, `?ret,`, `?call,`.
  \
  \ }doc

: ?call, ( a op -- ) 2+ ?jp, ;

  \ doc{
  \
  \ ?call, ( a op -- ) "question-call-comma"
  \
  \ Compile a Z80 conditional absolute call instruction to
  \ address _a_, being _op_ the identifier of the condition,
  \ which has been put on the stack by `nz?`, `c?`, `nc?`,
  \ `po?`, `pe?`, `p?`, or `m?`.
  \
  \ See: `call,`, `?ret,`, `?jp,`.
  \
  \ }doc

: ?jr, ( a op -- ) jp>jr (jr, ;

  \ doc{
  \
  \ ?jr, ( a op -- ) "question-j-r-comma"
  \
  \ Compile a Z80 conditional relative jump instruction to
  \ address _a_, being _op_ the identifier of the condition,
  \ which has been put on the stack by `nz?`, `c?`, or `nc?`.
  \
  \ See: `jr,`, `?jp,`, `djnz,`, `jp>jr`, `(jr,`.
  \
  \ }doc

  \ Control-flow structures with relative jumps

: >rmark ( -- orig ) here 1- ;

  \ doc{
  \
  \ >rmark ( -- orig ) "greater-than-r-mark"
  \
  \ Leave the origin address of a forward relative branch
  \ just compiled, to be resolved by `>rresolve`.
  \
  \ }doc

: rresolve ( orig dest -- ) 1- over - dup ?rel swap c! ;

  \ doc{
  \
  \ rresolve ( orig dest -- ) "r-resolve"
  \
  \ Resolve a relative branch.
  \
  \ See: `<rresolve`, `>rresolve`, `?rel`.
  \
  \ }doc

: >rresolve ( orig -- ) here rresolve ;

  \ doc{
  \
  \ >rresolve ( orig -- ) "greater-than-r-resolve"
  \
  \ Resolve a forward relative branch.
  \
  \ See: `<rresolve`, `rresolve`.
  \
  \ }doc

: <rresolve ( dest -- ) here 1- swap rresolve ; -->

  \ doc{
  \
  \ <rresolve ( dest -- ) "less-than-r-resolve"
  \
  \ Resolve a backward relative branch.
  \
  \ See: `>rresolve`, `rresolve`.
  \
  \ }doc

( assembler )

  \ Control-flow structures with relative jumps

: rahead ( -- orig ) 18 , >rmark ;
  \ Note: $18 is the Z80 opcode for `jr`.

  \ doc{
  \
  \ rahead ( -- orig ) "r-ahead"
  \
  \ Create a relative branch forward.
  \ Leave the origin address of a forward relative branch
  \ just compiled, to be resolved by `>rresolve`.
  \
  \ }doc

: (rif ( op -- orig cs-id ) , >rmark 0A ;

  \ doc{
  \
  \ (rif ( op -- orig cs-id ) "paren-r-if"
  \
  \ Common factor of `rif` and `relse`.
  \
  \ }doc

: rif ( op -- orig cs-id ) jp>jr inverse-cond (rif ;

  \ doc{
  \
  \ rif ( op -- orig cs-id ) "r-if"
  \
  \ Part of the relative-address control-flow structure ``rif``
  \ .. `relse` .. `rthen`.
  \
  \ See: `aif`, `rbegin`, `jp>jr`.
  \
  \ }doc

: rthen ( orig cs-id -- ) 0A ?pairs >rresolve ;

  \ doc{
  \
  \ rthen ( orig cs-id -- ) "r-then"
  \
  \ Part of the relative-address control-flow structure `rif`
  \ .. `relse` .. ``rthen``.
  \
  \ See: `athen`, `>rresolve`.
  \
  \ }doc

: relse ( orig cs-id -- orig cs-id )
  0A ?pairs 18 (rif rot swap rthen 0A ;
  \ Note: $18 is the opcode of `jr`.

  \ doc{
  \
  \ relse ( orig cs-id -- orig cs-id ) "r-else"
  \
  \ Part of the relative-address control-flow structure `rif`
  \ .. ``relse`` .. `rthen`.
  \
  \ See: `aelse`, `(rif`.
  \
  \ }doc

: rbegin ( -- dest cs-id ) <mark 0B ;

  \ doc{
  \
  \ rbegin ( -- dest cs-id ) "r-begin"
  \
  \ Part of the relative-address control-flow structures
  \ ``rbegin`` .. `ragain`, ``rbegin`` .. `runtil` and
  \ ``rbegin`` .. `rwhile` ..  `rrepeat`.
  \
  \ See: `abegin`.
  \
  \ }doc

: rwhile ( op -- orig cs-id ) rif 2+ ;

  \ doc{
  \
  \ rwhile ( op -- orig cs-id ) "r-while"
  \
  \ Part of the relative-address control-flow structures
  \ ``rbegin`` .. `rwhile` ..  `rrepeat`.
  \
  \ See: `awhile`.
  \
  \ }doc

: (runtil ( dest cs-id op -- ) , 0B ?pairs <rresolve ;

  \ doc{
  \
  \ (runtil ( dest cs-id op -- ) "paren-r-until"
  \
  \ Compile a relative conditional jump Z80 instruction _op_.
  \ ``(runtil`` is common factor of `runtil`, `ragain` and
  \ `rstep`.
  \
  \ }doc

: runtil ( dest cs-id op -- ) jp>jr inverse-cond (runtil ;

  \ doc{
  \
  \ runtil ( dest cs-id op -- ) "r-until"
  \
  \ Part of the relative-address control-flow structure
  \ `rbegin` .. ``runtil``.
  \
  \ See: `auntil`, `(runtil`, `jp>jr`.
  \
  \ }doc

: ragain ( dest cs-id -- ) 18 (runtil ;
  \ Note: $18 is the opcode of `jr`.

  \ doc{
  \
  \ ragain ( dest cs-id -- ) "r-again"
  \
  \ Part of the relative-address control-flow structure
  \ `rbegin` .. `ragain`.
  \
  \ See: `aagain`, `(runtil`.
  \
  \ }doc

: rrepeat ( dest cs-id1 orig cs-id2 --) 2swap ragain 2- rthen ;

  \ doc{
  \
  \ rrepeat ( dest cs-id1 orig cs-id2 --) "r-repeat"
  \
  \ Part of the relative-address control-flow structure
  \ `rbegin` .. `rwhile` ..  `rrepeat`.
  \
  \ See: `arepeat`.
  \
  \ }doc

: rstep ( dest cs-id -- ) 10 (runtil ;
  \ Note: $10 is the Z80 opcode for `djnz`.

  \ doc{
  \
  \ rstep ( dest cs-id -- ) "r-step"
  \
  \ Part of the relative-address control-flow structure
  \ `rbegin` .. ``rstep``.
  \
  \ See: `(runtil`.
  \
  \ }doc

base ! set-current set-order

( aif athen aelse abegin awhile auntil aagain arepeat )

  \ Control-flow structures with absolute jumps

get-order get-current
only forth-wordlist set-current         need ?pairs
assembler-wordlist >order set-current   need inverse-cond

: (aif ( op -- orig cs-id ) c, >mark $08 ;

  \ doc{
  \
  \ (aif ( op -- orig cs-id ) "paren-a-if"
  \
  \ Common factor of `aif` and `aelse`.
  \
  \ }doc

: aif ( op -- orig cs-id ) inverse-cond (aif ;

  \ doc{
  \
  \ aif ( op -- orig cs-id ) "a-if"
  \
  \ Part of the absolute-address control-flow structure ``aif``
  \ .. `aelse` .. `athen`.
  \
  \ See: `rif`.
  \
  \ }doc

: athen ( orig cs-id -- ) $08 ?pairs >resolve ;

  \ doc{
  \
  \ athen ( orig cs-id -- ) "a-then"
  \
  \ Part of the absolute-address control-flow structure `aif`
  \ .. `aelse` .. ``athen``.
  \
  \ See: `rthen`.
  \
  \ }doc

: aelse ( orig cs-id -- orig cs-id )
  $08 ?pairs $C3 (aif rot swap athen $08 ;
  \ Note: $C3 is the opcode of `jp`.

  \ doc{
  \
  \ aelse ( orig cs-id -- orig cs-id ) "a-else"
  \
  \ Part of the absolute-address control-flow structure `aif`
  \ .. ``aelse`` .. `athen`.
  \
  \ See: `relse`.
  \
  \ }doc

: abegin ( -- dest cs-id ) <mark $09 ;

  \ doc{
  \
  \ abegin ( -- dest cs-id ) "a-begin"
  \
  \ Part of the absolute-address control-flow structure
  \ ``abegin`` .. `awhile` ..  `arepeat`.
  \
  \ See: `rbegin`.
  \
  \ }doc

: awhile ( op -- orig cs-id ) aif 2+ ;

  \ doc{
  \
  \ awhile ( op -- orig cs-id ) "a-while"
  \
  \ Part of the absolute-address control-flow structure
  \ `abegin` .. ``awhile`` ..  `arepeat`.
  \
  \ See: `rwhile`.
  \
  \ }doc

: (auntil ( dest cs-id op ) c, $09 ?pairs <resolve ;

  \ doc{
  \
  \ (auntil ( dest cs-id op ) "paren-a-until"
  \
  \ Compile an absolute conditional jump.  ``(auntil`` is
  \ common factor of `auntil` and `aagain`.
  \
  \ }doc

: auntil ( dest cs-id op -- ) inverse-cond (auntil ;

  \ doc{
  \
  \ auntil ( dest cs-id op -- ) "a-until"
  \
  \ Part of the absolute-address control-flow structure
  \ `abegin` .. ``auntil``.
  \
  \ See: `runtil`.
  \
  \ }doc

: aagain ( dest cs-id -- ) $C3 (auntil ;
  \ Note: $C3 is the opcode of `jp`

  \ doc{
  \
  \ aagain ( dest cs-id -- ) "a-again"
  \
  \ Part of the absolute-address control-flow structure
  \ `abegin` .. `aagain`.
  \
  \ See: `ragain`.
  \
  \ }doc

: arepeat ( dest cs-id1 orig cs-id2 ) 2swap aagain 2- athen ;

  \ doc{
  \
  \ arepeat ( dest cs-id1 orig cs-id2 ) "a-repeat"
  \
  \ Part of the absolute-address control-flow structure
  \ `abegin` .. `awhile` ..  ``arepeat``.
  \
  \ See: `rrepeat`.
  \
  \ }doc

set-current set-order

( inverse-cond >amark >aresolve ?rel unresolved )

unneeding inverse-cond ?\ : inverse-cond ( op1 -- op2) 8 xor ;

  \ doc{
  \
  \ inverse-cond ( op1 -- op2 )
  \
  \ Convert an assembler condition flag (actually, an absolute
  \ jump opcode) to its opposite.
  \
  \ Examples: The opcode returned by `c?` is converted to the
  \ opcode returned by `nc?`; `nz?` to `z?`, etc.
  \
  \ }doc

unneeding >amark ?\ : >amark ( -- a ) here 2- ;

  \ doc{
  \
  \ >amark ( -- a ) "greater-than-a-mark"
  \
  \ Leave the address of an assembler absolute forward
  \ reference.
  \
  \ }doc

unneeding >aresolve ?( need >amark

: >aresolve ( a -- ) >amark swap ! ; ?)

  \ doc{
  \
  \ >aresolve ( a -- ) "greater-than-a-resolve"
  \
  \ Resolve an assembler absolute forward reference.
  \
  \ See: `>amark`.
  \
  \ }doc

unneeding ?rel

?\ : ?rel ( n -- ) $80 + $FF swap u< #-269 ?throw ;

  \ doc{
  \
  \ ?rel ( n -- ) "question-rel"
  \
  \ If assembler relative branch _n_ is too long, `throw`
  \ exception #-269 (relative jump too long).
  \
  \ }doc

unneeding unresolved ?( need array>

create unresolved0> ( -- a ) 8 cells allot

  \ doc{
  \
  \ unresolved0> ( -- a ) "unresolved-zero-greater-than"
  \
  \ Address _a_ is the default value of `unresolved>`: an
  \ 8-cell array.
  \
  \ }doc

variable unresolved> ( -- a ) unresolved0> unresolved> !

  \ doc{
  \
  \ unresolved> ( -- a ) "unresolved-greater-than"
  \
  \ A `variable`. Address _a_ contains the address of a cell
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
  0000 b ldp#, >aresolve   \ restore the Forth IP
  endm

  \ doc{
  \
  \ execute-hl, ( -- ) "execute-h-l-comma"
  \
  \ Compile an `execute` with the _xt_ hold in the HL register.
  \ ``execute-hl,`` is used to call Forth words from Z80.
  \
  \ See: `call-xt,`, `call`.
  \
  \ }doc

macro call-xt, ( xt -- ) 21 c, , execute-hl, endm

  \ doc{
  \
  \ call-xt, ( xt -- ) "call-x-t-comma"
  \
  \ Compile a Z80 call to _xt_, by compiling the Z80
  \ instruction that loads the HL register with _xt_, and then
  \ executing `execute-hl,` to compile the rest of the
  \ necessary code.
  \
  \ ``call-xt,`` is the low-level equivalent of `execute`: it's
  \ used to call a colon word from a code word.
  \
  \ See: `call`.
  \
  \ }doc

( hook, prt, )

  \ ZX Spectrum specific macros.

need assembler

get-current assembler-wordlist dup >order set-current

unneeding hook, ?\ $CF m4 hook,
  \ Equivalent to ``$08 rst,`` (``rst $08``).

  \ doc{
  \
  \ hook, ( -- ) "hook-comma"
  \
  \ Compile the Z80 instruction ``rst $08``. Therefore
  \ ``hook,`` is equivalent to ``$08 rst,``.
  \
  \ See: `rst,`, `prt,`.
  \
  \ }doc

unneeding prt, ?\ $D7 m1 prt,
  \ Equivalent to ``$16 rst,`` (``rst $16``).

  \ doc{
  \
  \ prt, ( -- ) "p-r-t-comma"
  \
  \ Compile the Z80 instruction ``rst $16``. Therefore
  \ ``prt,`` is equivalent to ``$16 rst,``.
  \
  \ See: `rst,`, `hook,`.
  \
  \ }doc

set-current

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
  \
  \ 2017-03-21: Fix number notation in `?rel`.
  \
  \ 2017-03-22: Add undocumented instructions `sll,` and
  \ `sllx,`.
  \
  \ 2017-03-26: Fix `aagain`. Improve documentation.
  \
  \ 2017-03-28: Fix code typo in `execute-hl`. Rewrite
  \ `call-xt,` with Z80 opcodes. Improve documentation.
  \
  \ 2017-09-09: Update notation "pfa" to the standard "dfa".
  \ Finish documentation of conditions (`z?`, `nz?`...).
  \ Document `?ret,`, `?call,`, `?jp,`, `?jr,`.
  \
  \ 2017-12-10: Add `m3p` to define `push,` and `pop,` with.
  \ This makes register A usable with those instructions
  \ instead of register AF.  Remove constant `af.`
  \
  \ 2017-12-11: Improve documentation.
  \
  \ 2018-02-01: Make `hook,` and `prt,` optional.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-07: Add words' pronunciaton.
  \
  \ 2018-04-14: Fix markup in documentation.
  \
  \ 2018-06-04: Update: remove trailing closing paren from word
  \ names.
  \
  \ 2018-06-04: Link `variable` in documentation.
  \
  \ 2018-07-21: Improve documentation, linking `throw`.
  \
  \ 2020-02-27: Improve documentation.
  \
  \ 2020-02-28: Complete the documentation of all instructions.
  \
  \ 2020-02-29: Improve documentation. Add `ldar,` and `ldra,`.
  \ Factor `ed,` from `sbcp,`, `adcp,`, `stp,` and `ftp,`.
  \
  \ 2020-03-30: Complete the words' pronunciations.
  \
  \ 2020-05-02: Fix cross references.
  \
  \ 2020-05-04: Fix cross reference. Remove cross references to
  \ inexistent word `ft#x,`,
  \
  \ 2020-05-05: Fix cross references.
  \
  \ 2020-05-18: Add explicit cross references.

  \ vim: filetype=soloforth
