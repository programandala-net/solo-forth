  \ data.MISC.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201705052335
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Misc words related to data structures.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( buffer: cvariable enum cenum enumcell link@ link, )

[unneeded] buffer:
?\ : buffer: ( u "name" -- ) create allot ;

  \ doc{
  \
  \ buffer: ( u "name" -- )
  \
  \ Define a named uninitialized buffer as follows: Reserve _u_
  \ address units of data space at an aligned address.  Create
  \ a definition for _name_ that will return the address of the
  \ space reserved by ``buffer:`` when it defined _name_.  The
  \ program is responsible for initializing the contents.
  \
  \ Origin: Forth-2012 (CORE EXT).
  \
  \ See: `create`, `allot`.
  \
  \ }doc

[unneeded] cvariable
?\ : cvariable ( "name"  -- ) create 1 allot ;

  \ doc{
  \
  \ cvariable ( "name" -- )
  \
  \ Create a character variable _name_ and reserve one
  \ character of data space. When _name_ is executed, it
  \ returns the address of the reserved space.
  \
  \ See: `c!`, `c@`, `variable`.
  \
  \ }doc

[unneeded] enum
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

[unneeded] cenum
?\ : cenum (  n "name" -- n+1 ) dup cconstant 1+ ;

  \ doc{
  \
  \ cenum ( n "name" -- n+1 )
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

[unneeded] enumcell
?\ : enumcell (  n "name" -- n+cell ) dup constant cell+ ;

  \ Credit:
  \
  \ Idea from SwiftForth's `enum4`.

  \ doc{
  \
  \ enumcell ( n "name" -- n+cell )
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

[unneeded] link@  [unneeded] link, and ?exit

  \ Credit:
  \
  \ Code of `link@` and `link,` written after the description
  \ by Rick VanNorman, published on Forth Dimensions (volume
  \ 20, number 3, pages 19-22, 1998-09).

need alias ' @ alias link@ ( node1 -- node2 )

  \ doc{
  \
  \ link@ ( node1 -- node2 )
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
  \ link, ( head -- )
  \
  \ Create a new node in data space for the linked list _head_:
  \
  \ Before:
  \
  \ - head --> old_node
  \
  \ After:
  \
  \ - head --> new_node
  \ - new_node --> old_node
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

  \ vim: filetype=soloforth
