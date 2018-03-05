  \ flow.switch-colon.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803052149
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ An extendable case selector implemented with word lists.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( switch: switch :clause )

  \ XXX TODO -- add default xt

unneeding switch:
?\ : switch: ( "name" -- ) wordlist constant ;

  \ doc{
  \
  \ switch: ( "name" -- ) "switch-colon"
  \
  \ Create a new switch control structure _name_, which is a
  \ word list the clauses of the structure will be added to.
  \
  \ The keys can be 1-byte, 1-cell or 2-cell numbers, but the
  \ correspondent words must be used to create the clauses and
  \ execute them later:
  \
  \ Usage example:

  \ ----
  \ switch: mynumber
  \
  \ \ Define clauses:
  \
  \ 0       mynumber :clause  ( -- ) cr ." zero" ;
  \ 1       mynumber :cclause ( -- ) cr ." one" ;
  \ 2048    mynumber :clause  ( -- ) cr ." 2 KiB" ;
  \ 100000. mynumber :2clause ( -- ) cr ." big" ;
  \
  \ \ Execute the clauses:
  \
  \ 0       mynumber switch
  \ 1       mynumber cswitch
  \ 2048    mynumber switch
  \ 100000. mynumber 2switch
  \ ----

  \ New clauses can be added any time, in any order, with any
  \ key.
  \
  \ Clauses created with `:clause` (for 1-cell keys),
  \ `:cclause` (for character keys) and `:2clause` (for 2-cell
  \ keys) must be executed with `switch`, `cswitch` and
  \ `2switch` respectively. The smaller the key type, the less
  \ memory used by clauses in headers space (every clause is a
  \ definition whose name is the binary string of its key) and
  \ the less execution time, though the difference will be
  \ unimportant in most cases.
  \
  \ If a new clause is added with a previously used key, the
  \ new clause will replace the old one.
  \
  \ There's no default clause: if the a given key is not found,
  \ no code is executed and no exception is thrown.
  \
  \ }doc

unneeding switch unneeding :clause and ?(

need search-wordlist need nextname need >bstring

: switch ( x switch -- )
  swap >bstring rot search-wordlist if  execute  then ;

  \ doc{
  \
  \ switch ( x switch -- )
  \
  \ Execute the switch _switch_ for the key _x_.
  \
  \ See: `switch:`.
  \
  \ }doc

: :clause ( x switch -- )
  get-current >r set-current  >bstring nextname :
              r> set-current ; ?)

  \ doc{
  \
  \ :clause ( x switch -- ) "colon-clause"
  \
  \ Start the definition of a switch clause _x_ for switch
  \ _switch_.
  \
  \ See: `switch:`, `switch`.
  \
  \ }doc

( cswitch :cclause 2switch :2clause )

unneeding cswitch unneeding :cclause and ?(

need search-wordlist need nextname need >bstring

: cswitch ( c switch -- )
  swap c>bstring rot search-wordlist if  execute  then ;

  \ doc{
  \
  \ cswitch ( c switch -- ) "c-switch"
  \
  \ Execute the switch _switch_ for the key _c_.
  \
  \ See: `switch:`, `:cclause`.
  \
  \ }doc

: :cclause ( c switch -- )
  get-current >r set-current  c>bstring nextname :
              r> set-current ; ?)

  \ doc{
  \
  \ :cclause (  switch -- ) "colon-c-clause"
  \
  \ Start the definition of a switch clause _c_ for switch
  \ _switch_.
  \
  \ See: `switch:`, `cswitch`.
  \
  \ }doc

unneeding 2switch unneeding :2clause and ?(

need search-wordlist need nextname need 2>bstring

: 2switch ( xd switch -- )
  swap 2>bstring rot search-wordlist if  execute  then ;

  \ doc{
  \
  \ 2switch ( xd switch -- ) "two-switch"
  \
  \ Execute the switch _switch_ for the key _xd_.
  \
  \ See: `switch:`, `:2clause`.
  \
  \ }doc

: :2clause ( xd switch -- )
  get-current >r set-current  2>bstring nextname :
              r> set-current ; ?)

  \ doc{
  \
  \ :2clause ( xd switch -- ) "colon-two-clause"
  \
  \ Start the definition of a switch clause _xd_ for switch
  \ _switch_.
  \
  \ See: `switch:`, `2switch`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-11-16: Start.
  \
  \ 2016-11-26: Need `search-wordlist`, which has been moved to
  \ the library.
  \
  \ 2016-12-06: Update: Replace `next-name` with `nextname`.
  \ Fix the key to string converter, rename it and move it to
  \ <strings.misc.fsb>. Document and test the code.
  \
  \ 2016-12-07: Rename module to <flow.switch-colon.fsb>.
  \ Rename `>cell-string` to `>bstring`.
  \
  \ 2016-12-27: Improve documentation.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2018-02-15: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.

  \ vim: filetype=soloforth
