  \ data.begin-structure.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201709091154
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Forth-2012 structures.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( +field field: 2field: cfield: begin-structure end-structure )

[unneeded] +field ?\ defer +field ( n1 n2 "name" -- n3 ) exit

  \ doc{
  \
  \ +field ( n1 n2 "name -- n3 )
  \
  \ Create a definition for _name_ with the execution semantics
  \ defined below. Return _n3_ = _n1_ + _n2_ where _n1_ is the
  \ offset in the data  structure before ``+field`` executes,
  \ and _n2_ is the size of the data to be added to the data
  \ structure. _n1_ and _n2_ are in address units.
  \
  \ _name_ execution: ``( a1 -- a2 )``
  \
  \ Add _n1_ to _a1_ giving _a2_.
  \
  \ ``+field`` is not  required to  align items.  This is
  \ deliberate and allows  the construction  of unaligned  data
  \ structures for communication with external elements such as
  \ a hardware register map or protocol packet.  Field
  \ alignment has been left to the appropriate field
  \ definition, e.g. `field:`, `2field:`, `cfield:`.
  \
  \ In Solo Forth, ``+field`` is an unitialized deferred word,
  \ for which three implementations are provided:
  \ `+field-unopt`, `+field-opt-0` and `+field-opt-0124`.
  \
  \ Origin: Forth-2012 (FACILITY EXT).
  \
  \ See: `begin-structure`.
  \
  \ }doc

[unneeded] field: ?( need +field
: field:   ( n1 "name" -- n2 ) cell +field ; ?)

  \ doc{
  \
  \ field: ( n1 "name" -- n2 )
  \
  \ Parse _name_.  _offset_  is the first cell aligned
  \ value greater than or equal to _n1_. _n2_ = _offset_ + 1
  \ cell.
  \
  \ Create a definition for _name_ with the execution semantics
  \ defined below.
  \
  \ _name_ execution: ``( a1 -- a2 )``
  \
  \ Add the _offset_ calculated during the compile-time action
  \ to _a1_ giving the address _a2_.
  \
  \ Origin: Forth-2012 (FACILITY EXT).
  \
  \ See: `begin-structure`, `+field`.
  \
  \ }doc

[unneeded] 2field: ?( need +field
: 2field: ( n1 "name" -- n2 ) [ 2 cells ] cliteral +field ; ?)

  \ doc{
  \
  \ 2field: ( n1 "name" -- n2 )
  \
  \ Parse _name_.  _offset_  is the first double-cell aligned
  \ value greater than or equal to _n1_. _n2_ = _offset_ + 2
  \ cells.
  \
  \ Create a definition for _name_ with the execution semantics
  \ defined below.
  \
  \ _name_ execution: ``( a1 -- a2 )``
  \
  \ Add the _offset_ calculated during the compile-time action
  \ to _a1_ giving the address _a2_.
  \
  \ See: `begin-structure`, `+field`.
  \
  \ }doc

[unneeded] cfield: ?( need +field
: cfield: ( n1 "name" -- n2 ) [ 1 chars ] cliteral +field ; ?)

  \ doc{
  \
  \ cfield: ( n1 "name" -- n2 )
  \
  \ Parse _name_.  _offset_  is the first character aligned
  \ value greater than or equal to _n1_. _n2_ = _offset_ + 1
  \ character.
  \
  \ Create a definition for _name_ with the execution semantics
  \ defined below.
  \
  \ _name_ execution: ``( a1 -- a2 )``
  \
  \ Add the _offset_ calculated during the compile-time action
  \ to _a1_ giving the address _a2_.
  \
  \ Origin: Forth-2012 (FACILITY EXT).
  \
  \ See: `begin-structure`, `+field`.
  \
  \ }doc

[unneeded] begin-structure [unneeded] end-structure and ?(

: begin-structure ( "name" -- struct-sys 0 )
  create  >mark 0 does> ( -- n ) ( dfa ) @ ;

  \ doc{
  \
  \ begin-structure ( "name" -- struct-sys 0 )
  \
  \ Parse _name_.  Create  a definition  for _name_ with  the
  \ execution semantics defined below. Return a _struct-sys_
  \ that will be used by `end-structure` and an initial offset
  \ of 0.
  \
  \ _name_ execution: ``( -- +n )``
  \
  \ _+n_ is the size in memory expressed in address units of
  \ the data structure.
  \
  \ Example usage:

  \ ----
  \ begin-structure /record
  \   field:  ~year
  \   cfield: ~month
  \   cfield: ~day
  \ end-structure
  \
  \ 10 #records
  \ create records #records /record * allot
  \
  \ : record> ( n -- a ) /record * records + ;
  \   \ Address _a_ of record _n_.
  \
  \ 1887  0 record> ~year !    \ store a year into record 0
  \       9 record> ~month c@  \ fetch the month from record 9
  \ ----

  \ Note: ``begin-structure`` and `end-structure` are not
  \ necessary to create a structure. Only the initial offset 0
  \ is needed at the start, and saving the structure size at
  \ the end, e.g. using a `constant` or a `value`:

  \ ----
  \ 0
  \   field:  ~the-cell
  \   cfield: ~the-char
  \ constant /record
  \ ----

  \
  \ Origin: Forth-2012 (FACILITY EXT).
  \
  \ See: `end-structure`, `field:`, `cfield:`, `2field:`,
  \ `+field`.
  \
  \ }doc

: end-structure ( struct-sys +n -- ) swap ! ; ?)

  \ doc{
  \
  \ end-structure ( struct-sys +n -- )
  \
  \ Terminate definition of a structure started by
  \ `begin-structure`.
  \
  \ Origin: Forth-2012 (FACILITY EXT).
  \
  \ }doc

( +field-unopt +field-opt-0 )

[unneeded] +field-unopt ?( need +field

: +field-unopt ( n1 n2 "name" -- n3 )
  create over , + does> ( a -- a' ) ( a dfa ) @ + ;

' +field-unopt ' +field defer! ?)

  \ Credit:
  \
  \ Code copied from the Forth-2012 documentation.

  \ doc{
  \
  \ +field-unopt ( n1 n2 "name" -- n3 )
  \
  \ Unoptimized implementation of `+field`.  This
  \ implementation is less efficient than `+field-opt-0` and
  \ `+field-opt-0124` because the field offset is calculated
  \ also when it is 0.
  \
  \ The advantage of this implementation is it uses only 22
  \ bytes of data space, so it could be useful in some cases.
  \
  \ }doc

[unneeded] +field-opt-0 ?( need +field

  \ This implementation optimizes field 0 of the structure.  It
  \ uses 31 bytes of data space.

: +field-opt-0 ( n1 n2 "name" -- n3 )
  : over ?dup if    postpone literal postpone +
              else  immediate
              then  postpone ; + ;

' +field-opt-0 ' +field defer! ?)

  \ Credit:
  \
  \ Code adapted (local variables removed) from:
  \
  \ Newsgroups: comp.lang.forth
  \ From: anton AT mips DOT complang DOT tuwien DOT ac DOT at (Anton Ertl)
  \ Subject: Re: ColorForth - another dead end?
  \ Date: Mon, 01 Jun 2015 13:21:34 GMT
  \ Message-ID: <2015Jun1.152134@mips.complang.tuwien.ac.at>
  \
  \ : +FIELD {: n1 n2 -- n3 :}
  \   : n1 if
  \     n1 postpone literal postpone +
  \   else
  \      immediate
  \   then
  \   postpone ; n1 n2 + ;

  \ doc{
  \
  \ +field-opt-0 ( n1 n2 "name" -- n3 )
  \
  \ Optimized implementation of `+field`.  This implementation
  \ is more efficient than `+field-unopt` (but less than
  \ `+field-opt-0124`) because the field 0 does not calculate
  \ the field offset.
  \
  \ ``+field-opt-0`` uses 31 bytes of data space.
  \
  \ }doc

( +field-opt-0124 )

[unneeded] +field-opt-0124 ?( need case need +field

: +field-opt-0124 ( n1 n2 "name" -- n3 )
  :
  over case
  0                    of immediate                     endof
  1                    of postpone 1+                   endof
  cell                 of postpone cell+                endof
  [ 2 cells ] cliteral of postpone cell+ postpone cell+ endof
  dup  postpone literal postpone +  \ default
  endcase postpone ; + ;

' +field-opt-0124 ' +field defer! ?)

  \ doc{
  \
  \ +field-opt-0124 ( n1 n2 "name" -- n3 )
  \
  \ Optimized implementation of `+field` that optimizes the
  \ calculation of field offsets 0, 1, 2 and 4. Therefore it is
  \ more efficient than `+field-unopt` and `+field-opt-0`, but
  \ it uses 106 bytes of data space and needs `case`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-11-28: Make `+field` deferred and provide three
  \ optional implementations. Make all words accessible to
  \ `need`.
  \
  \ 2016-12-24: Fix typo in documentation. Fix needing of
  \ `+field-opt-0` and `+field-opt-0124`.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-19: Remove remaining `exit` at the end of
  \ conditional interpretation.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-02-22: Improve documentation.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-03-16: Complete and improve documentation.  Improve
  \ `+field-opt-0124` (one byte less).
  \
  \ 2017-09-09: Update notation "pfa" to the standard "dfa".

  \ vim: filetype=soloforth
