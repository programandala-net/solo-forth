  \ blocks.indexer.COMMON.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702241212

  \ -----------------------------------------------------------
  \ Description

  \ Code common to the Thru Indexer and the Fly Indexer.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2016, 2017.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ History

  \ 2016-11-19: Extracted from <blocks.indexer.fsb> in order to
  \ share the code with <blocks.fly-indexer.fsb>.
  \
  \ 2016-11-21: Add missing `need`.
  \
  \ 2016-11-24: Rename the module.
  \
  \ 2016-11-25: Combine `search-index` and `name-indexed?` into
  \ `indexed-name?`.  Add `set-index-order`. Improve
  \ documentation.
  \
  \ 2016-11-26: Need `search-wordlist`, which has been moved to
  \ the library.
  \
  \ 2016-12-08: Rename the module filename with uppercase
  \ "COMMON", after the new convention.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-02-24: Improve documentation markup.

( common-indexer )

get-current forth-wordlist dup >order set-current

need alias need nextname need evaluate need search-wordlist

wordlist constant index-wordlist

  \ doc{
  \
  \ index-wordlist ( -- wid )
  \
  \ Word list for the indexed words.
  \
  \ }doc

: indexed-name? ( ca len -- false | block true )
  index-wordlist search-wordlist 0<> ;

  \ doc{
  \
  \ name-indexed? ( ca len -- false | block true )
  \
  \ Search the index for word _ca len_. If found, return
  \ its _block_ and _true_, else return _false_.
  \
  \ }doc

variable indexed-block

: index-name ( ca len -- )
  2dup indexed-name? if  drop 2drop exit  then
  nextname indexed-block @ alias ;

  \ doc{
  \
  \ index-name ( ca len -- )
  \
  \ Add word _ca len_ to the blocks index, if not done before.
  \
  \ The current word list must be `index-wordlist`.
  \
  \ WARNING: The block where _ca len_ was found is stored as
  \ the execution token of its definition in the index. This
  \ way the index uses no data space. Don't put
  \ `index-wordlist` in the search order unless you know what
  \ you're doing.
  \
  \ }doc

: (index-block) ( block -- )
  dup indexed-block ! 0 swap line>string evaluate ; -->

  \ doc{
  \
  \ (index-block) ( u -- )
  \
  \ Index block _u_, evaluating its header line.  The only
  \ word list in the search order must be `index-wordlist`.
  \
  \ This is a common factor of `(indexer)` (from the `indexer`
  \ tool) and `(index-block)` (from the `fly-indexer` tool).
  \
  \ }doc

( common-indexer )

wordlist constant indexer-wordlist
  \ Word list for the words that parse the block index lines.

: set-index-order ( -- )
  index-wordlist set-current  indexer-wordlist 1 set-order ;

indexer-wordlist set-current

: ( ( "ccc<space><paren><space|eof>" -- )
  begin  parse-name 2dup s" )" str= 0=
  while  index-name  repeat  2drop ;
  \ Parse and index the names until the next right paren name.

indexer-wordlist >order  ' ( alias .(  previous

: \ ( "ccc<space><backslash><space|eof>" -- )
  begin  parse-name 2dup s" \" str= 0=
  while  index-name  repeat  2drop ;
  \ Parse and index the names until the next backslash name.

previous set-current
  \ Restore the original search order.

: common-indexer ( -- ) ;

( index-words indexer-words )

  \ XXX TMP -- for debugging

need common-indexer

[unneeded] index-words ?( need wordlist-words need .wordname

: index-words ( -- ) index-wordlist wordlist-words ; ?)

[unneeded] indexer-words ?( need wordlist-words need .wordname

: indexer-words ( -- ) indexer-wordlist wordlist-words ; ?)

  \ vim: filetype=soloforth