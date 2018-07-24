  \ strings.far.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201807250056
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words to manage far-memory strings.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( far," fars, farsconstant far>sconstant far>stringer )

unneeding far,"

?\ need fars,  : far," ( -- ) '"' parse fars, ;

  \ doc{
  \
  \ far," ( "ccc<quote>" -- ) "far-comma-quote"
  \
  \ Parse "ccc" delimited by a double-quote and compile the
  \ string in far memory.
  \
  \ }doc

unneeding fars, ?( need farplace need farallot

: fars, ( ca len -- ) tuck np@ farplace char+ farallot ; ?)

  \ doc{
  \
  \ fars, ( ca len -- ) "fars-comma"
  \
  \ Compile a string in far memory.
  \
  \
  \ }doc

unneeding farsconstant ?( need fars,

: farsconstant ( ca len "name" -- )
  np@ >r fars, r> farcount 2constant ; ?)

  \ doc{
  \
  \ farsconstant ( ca len "name" -- ) "far-s-constant"
  \
  \ Create a string constant _name_ in far memory with value
  \ _ca len_.
  \
  \ When _name_ is executed, it returns the string _ca len_ in
  \ far memory as _ca2 len_.
  \
  \ See: `far>sconstant`.
  \
  \ }doc

unneeding far>stringer ?( need cmove<far

: far>stringer ( ca1 len1 -- ca2 len1 )
  dup allocate-stringer swap 2dup 2>r cmove<far 2r> ; ?)

  \ doc{
  \
  \ far>stringer ( ca1 len1 -- ca2 len1 ) "far-to-stringer"
  \
  \ Save the string _ca1 len1_, which is in far memory, to the
  \ `stringer` and return it as _ca2 len1_.
  \
  \ See: `>stringer`.
  \
  \ }doc

unneeding far>sconstant ?(

need farsconstant need far>stringer

: far>sconstant ( ca len "name" -- )
  farsconstant does> 2@ far>stringer ; ?)

  \ doc{
  \
  \ far>sconstant ( ca len "name" -- ) "far-to-s-constant"
  \
  \ Create a string constant _name_ in far memory with value
  \ _ca len_.
  \
  \ When _name_ is executed, it returns the string _ca len_ in
  \ the `stringer` as _ca2 len_.
  \
  \ See: `farsconstant`.
  \
  \ }doc

( farsconstants, farsconstants> farsconstants far>sconstants )

unneeding farsconstants, ?( need far,

: farsconstants, ( 0 ca[n]..ca[1] "name" -- n )
  create np@ , 0 begin  swap ?dup while  far, 1+  repeat ; ?)

  \ doc{
  \
  \ farsconstants, ( 0 ca[n]..ca[1] "name" -- n ) "far-s-constants-comma"
  \
  \ Create a table of string constants _name_ in far memory,
  \ using counted strings _ca[n]..ca[1]_, being _0_ a mark for
  \ the last string on the stack, and return the number _n_ of
  \ compiled strings.
  \
  \ When _name_ is executed, it returns an address that holds
  \ the address of the table in far memory.
  \
  \ ``farconstants,`` is a common factor of `farsconstants` and
  \ `far>sconstants`.
  \
  \ }doc

unneeding farsconstants>
?\ : farsconstants> ( n a -- ca len ) @ array> far@ farcount ;

  \ doc{
  \
  \ farsconstants> ( n a -- ca len ) "far-s-constants-from"
  \
  \ Return the far-memory string _ca len_ whose address is
  \ stored at the _n_ cell of the table _a_ in data space.
  \
  \ ``farsconstants>`` is a factor of `farsconstants` and
  \ `far>sconstants`.
  \
  \ }doc

unneeding farsconstants ?(

need farsconstants, need array> need farsconstants>

: farsconstants ( 0 ca[n]..ca[1] "name" -- n )
  farsconstants,  does> ( n -- ca len )
  ( n dfa ) farsconstants> ; ?)

  \ doc{
  \
  \ farsconstants ( 0 ca[n]..ca[1] "name" -- ) "far-s-constants"
  \
  \ Create a table of string constants _name_ in far memory,
  \ using counted strings _ca[n]..ca[1]_, being _0_ a mark for
  \ the last string on the stack, and return the number _n_ of
  \ compiled strings.
  \
  \ When _name_ is executed, it converts the index on the stack
  \ (0.._n-1_) to the correspondent string _ca len_ in far
  \ memory.
  \
  \ Usage example:

  \ ----
  \
  \ 0                  \ end of strings
  \   np@ far," kvar"  \ string 4
  \   np@ far," tri"   \ string 3
  \   np@ far," du"    \ string 2
  \   np@ far," unu"   \ string 1
  \   np@ far," nul"   \ string 0
  \ farsconstants digitname  constant digitnames
  \
  \ cr .( There are ) digitnames . .( digit names:)
  \ 0 digitname cr fartype
  \ 1 digitname cr fartype
  \ 2 digitname cr fartype
  \ 3 digitname cr fartype cr
  \ ----

  \ See: `sconstants`, `far>sconstants`.
  \
  \ }doc

unneeding far>sconstants ?( need farsconstants,
need array> need farsconstants> need far>stringer

: far>sconstants ( 0 ca[n]..ca[1] "name" -- n )
  farsconstants,  does> ( n -- ca len )
  ( n dfa ) farsconstants> far>stringer ; ?)

  \ doc{
  \
  \ far>sconstants ( 0 ca[n]..ca[1] "name" -- n ) "far-to-s-constants"
  \
  \ Create a table of string constants _name_ in far memory,
  \ using counted strings _ca[n]..ca[1]_, being _0_ a mark for
  \ the last string on the stack, and return the number _n_ of
  \ compiled strings.
  \
  \ When _name_ is executed, it converts the index on the stack
  \ (0.._n-1_) to the correspondent string _ca len_ in far
  \ memory, and return a copy in the `stringer`.
  \
  \ Usage example:

  \ ----
  \
  \ 0                  \ end of strings
  \   np@ far," kvar"  \ string 4
  \   np@ far," tri"   \ string 3
  \   np@ far," du"    \ string 2
  \   np@ far," unu"   \ string 1
  \   np@ far," nul"   \ string 0
  \ far>sconstants digitname  constant digitnames
  \
  \ cr .( There are ) digitnames . .( digit names:)
  \ 0 digitname cr type
  \ 1 digitname cr type
  \ 2 digitname cr type
  \ 3 digitname cr type cr
  \ ----

  \ See: `sconstants`, `farsconstants`.
  \
  \ }doc

( faruppers )

unneeding faruppers ?(

need assembler
need far-hl_ need upper_ need ?next-bank_

code faruppers ( ca len -- )

  h pop, exsp, far-hl_ call, d pop,
  rbegin  d a ld, e or,  nz? rwhile
          m a ld, upper_ call, a m ld, h incp,
          d push, ?next-bank_ call, d pop, d decp,
  rrepeat ' default-bank jp, end-code ?)

  \   pop hl              ; string length
  \   ex (sp),hl          ; HL = string address
  \   call far_hl_routine ; HL = actual string address
  \                       ; the bank is paged in
  \   pop de              ; string length
  \ far_uppers.do:
  \   ld a,d
  \   or e
  \   jr z,faruppers.end
  \   ld a,(hl)
  \   call upper_routine
  \   ld (hl),a
  \   inc hl
  \   push de
  \   call question_next_bank_routine
  \   pop de
  \   dec de
  \   jp far_uppers.do
  \ far_uppers.end:
  \   jp default_bank_

  \ XXX TODO -- improve to save push/pop

  \ doc{
  \
  \ faruppers ( ca len -- ) "far-uppers"
  \
  \ Convert string _ca len_, which is stored in far memory, to
  \ uppercase.
  \
  \ See: `uppers`, `far-banks`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2017-01-10: Move all the far-memory string words from
  \ <strings.MISC.fsb>.  Simplify the string arrays: remove the
  \ variants that don't leave the count on the stack and remove
  \ the slash from the names of the others.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-19: Remove remaining `exit` at the end of
  \ conditional interpretation.
  \
  \ 2017-01-20: Fix documentation.
  \
  \ 2017-02-01: Move `faruppers` from the kernel.
  \
  \ 2017-02-02: Add `far>sconstant` and `farsconstants>`.
  \
  \ 2017-02-03: Fix needing of `far>sconstant`.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-02-25: Update the name of the circular string buffer,
  \ after the changes in the kernel.
  \
  \ 2017-02-26: Update "hp" notation to "np", after the changes
  \ in the kernel.
  \
  \ 2017-03-04: Update naming convention of Z80 routines, after
  \ the changes in the kernel.
  \
  \ 2017-03-12: Update the names of `stringer` words and
  \ mentions to it.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-09-09: Update notation "pfa" to the standard "dfa".
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-09: Add words' pronunciaton.
  \
  \ 2018-07-25: Use `char+` in `fars,`.

  \ vim: filetype=soloforth

