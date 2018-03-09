  \ data.data.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803091537
  \ See change log at the end of the file

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2016, 2017, 2018.

  \ ===========================================================
  \ Credit

  \ Words inspired by TurboForth's `data`, which nevertheless
  \ works different: it's a parsing word that parses and
  \ compiles a given number of literals, including the count.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( data end-data )

: data ( n "name" --  n orig )
  create >mark
  does> ( -- a len ) ( dfa ) dup cell+ swap @ ;

  \ doc{
  \
  \ data ( n "name" -- n orig )
  \
  \ Create a definition for _name_, in order to compile data
  \ items of _n_ bytes each, finished by `end-data`.  Leave _n_
  \ and _orig_ to be consumeb by `end-cdata`.  When _name_ is
  \ executed, it will leave the start address of the data and
  \ the number of items, which depends on _n_.
  \
  \ Usage example:

  \ ----
  \ cell data my-cells ( -- a u )
  \   1 c 2 c 3 c 4 c 5 c  end-data
  \
  \ 2 cells data my-double-cells ( -- a u )
  \   0. 2, 1. 2, 2. 2,  end-data
  \
  \ 1 chars data my-characters ( -- a u )
  \   'a' c, 'b' c, 'c' c,  end-data
  \ ----
  \
  \ }doc

: end-data ( n orig -- ) here over cell+ - rot / swap ! ;

  \ doc{
  \
  \ end-data ( n orig -- )
  \
  \ Finish the definition started by `data`, calculating the
  \ number of data items of _n_ bytes that were compiled and
  \ store it at _orig_.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-11-25: Start.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-19: Remove remaining `exit` at the end of
  \ conditional interpretation.
  \
  \ 2017-02-16: Replace all words (`data:`, `;data`, `cdata:`,
  \ `;cdata`, `2data:` and `;2data`) with the single pair
  \ `data`, `end-data`, which is more versatile: it can do the
  \ same as all the old words and more.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-09-09: Update notation "pfa" to the standard "dfa".
  \
  \ 2018-03-09: Update notation "address units" to "bytes".

  \ vim: filetype=soloforth
