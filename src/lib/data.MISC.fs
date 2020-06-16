  \ data.MISC.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202006161654
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Misc words related to data structures.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018,
  \ 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( buffer: 2variable cvariable enum cenum enumcell link@ link, )

unneeding buffer:
?\ : buffer: ( u "name" -- ) create allot ;

  \ doc{
  \
  \ buffer: ( u "name" -- ) "buffer-colon"
  \
  \ Define a named uninitialized buffer as follows: Reserve _u_
  \ bytes of data space.  Create a definition for _name_ that
  \ will return the address of the space reserved by
  \ ``buffer:`` when it defined _name_.  The program is
  \ responsible for initializing the contents.
  \
  \ Origin: Forth-2012 (CORE EXT).
  \
  \ See: `reserve`, `allotted`, `create`, `allot`.
  \
  \ }doc

unneeding 2variable ?( : 2variable ( "name"  -- ) create
  [ 2 cells ] cliteral allot ; ?)

  \ doc{
  \
  \ 2variable ( "name" -- ) "two-variable"
  \
  \ Parse _name_.  `create` a definition for _name_, which is
  \ referred to as a "two-variable".  `allot` two cells of data
  \ space, the data field of _name_, to hold the contents of
  \ the two-variable. When _name_ is later executed, the
  \ address of its data field is placed on the stack.
  \
  \ The program is responsible for initializing the contents of
  \ the two-variable.
  \
  \ Origin: Forth-79 (Double Number Word Set), Forth-83 (Double
  \ Number Extension Word Set), Forth-94 (DOUBLE), Forth-2012
  \ (DOUBLE).
  \
  \ See: `cells`, `literal`, `variable`, `2variable`,
  \ `2constant`.
  \
  \ }doc

unneeding cvariable
?\ : cvariable ( "name"  -- ) create 1 allot ;

  \ doc{
  \
  \ cvariable ( "name" -- ) "c-variable"
  \
  \ Create a character variable _name_ and reserve one
  \ character of data space. When _name_ is executed, it
  \ returns the address of the reserved space.
  \
  \ See: `c!`, `c@`, `variable`.
  \
  \ }doc

unneeding enum
?\ : enum (  n "name" -- n+1 ) dup constant 1+ ;

  \ doc{
  \
  \ enum ( n "name" -- n+1 )
  \
  \ Create a constant _name_ with value _n_ and return _n+1_.
  \
  \ Usage example:
  \
  \ ----
  \ 0 enum first
  \   enum second
  \   enum third
  \   enum fourth
  \ drop
  \ ----
  \
  \ See: `cenum`, `enumcell`.
  \
  \ }doc

unneeding cenum
?\ : cenum (  n "name" -- n+1 ) dup cconstant 1+ ;

  \ doc{
  \
  \ cenum ( n "name" -- n+1 ) "c-enum"
  \
  \ Create a cconstant _name_ with value _n_ and return _n+1_.
  \
  \ Usage example:
  \
  \ ----
  \ 0 cenum first
  \   cenum second
  \   cenum third
  \   cenum fourth
  \ drop
  \ ----
  \
  \ See: `enum`, `enumcell`.
  \
  \ }doc

unneeding enumcell
?\ : enumcell (  n "name" -- n+cell ) dup constant cell+ ;

  \ Credit:
  \
  \ Idea from SwiftForth's `enum4`.

  \ doc{
  \
  \ enumcell ( n "name" -- n+cell ) "enum-cell"
  \
  \ Create a constant _name_ with value _n_ and return
  \ _n+cell_.
  \
  \ Usage example:
  \
  \ ----
  \ 0 enumcell first
  \   enumcell second
  \   enumcell third
  \   enumcell fourth
  \ drop
  \ ----
  \
  \ See: `enum`.
  \
  \ }doc

unneeding link@  unneeding link, and ?exit

  \ Credit:
  \
  \ Code of `link@` and `link,` written after the description
  \ by Rick VanNorman, published on Forth Dimensions (volume
  \ 20, number 3, pages 19-22, 1998-09).

need alias ' @ alias link@ ( node1 -- node2 )

  \ doc{
  \
  \ link@ ( node1 -- node2 ) "link-fetch"
  \
  \ Fetch the node _node2_ from the linked list node _node1_.
  \ ``link@`` is an alias of `@`.
  \
  \ See: `link,`.
  \
  \ }doc

: link, ( node -- ) here over @ , swap ! ;

  \ doc{
  \
  \ link, ( head -- ) "link-comma"
  \
  \ Create a new node in data space for the linked list _head_:
  \
  \ Before:
  \
  \ - head -> old_node
  \
  \ After:
  \
  \ - head -> new_node
  \ - new_node -> old_node
  \
  \ See: `link@`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-11-15: Add `link@` and `link,`.
  \
  \ 2016-04-28: Rename `set` to `storer` and improve it. Add
  \ `cstorer`.
  \
  \ 2016-05-02: Join two blocks to save space.
  \
  \ 2016-05-10: Add `2storer`.
  \
  \ 2016-11-17: Add `const`, `cconst`, `2const`.
  \
  \ 2016-11-25: Move `storer`, `cstorer` and `2storer` to their
  \ own module <data.storer.fsb>.  Move `const`, `cconst` and
  \ `2const` to their own module <data.const.fsb>.  Document
  \ all remaining words.  Convert `link@` from deferred to
  \ alias. Add `enumcell`.
  \
  \ 2016-12-04: Add `cenum`.
  \
  \ 2016-12-08: Rename the module filename with uppercase
  \ "MISC" after the new convention.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-02-17: Fix markup in documentation.  Update cross
  \ references.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-05-05: Improve documentation.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-07: Add words' pronunciaton.
  \
  \ 2018-03-09: Update notation "address units" to "bytes".
  \
  \ 2018-06-04: Update documentation: remove mentions of
  \ aligned addresses.
  \
  \ 2020-05-02: Improve documentation.
  \
  \ 2020-05-04: Fix cross reference.
  \
  \ 2020-05-19: Move `2variable` from the kernel.
  \
  \ 2020-06-16: Fix markup of arrows.

  \ vim: filetype=soloforth
