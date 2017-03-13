  \ blocks.indexer.thru.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201703132009
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Blocks Thru Indexer
  \
  \ A blocks indexer that changes the default action of
  \ `need` and related words: The whole disk is indexed first,
  \ and then `need and family uses the index instead of
  \ searching the blocks.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( make-thru-index use-thru-index )

only forth definitions

need common-indexer need get-order need set-order
need evaluate need catch need use-default-located

: thru-index-reneeded ( ca len -- )
  indexed-name? 0= #-277 ?throw load ;

  \ doc{
  \
  \ thru-index-reneeded ( ca len-- )
  \
  \ Search the index word list for word _ca len_. If found,
  \ load the block it's associated to.  If not found, throw an
  \ exception -277 ("needed, but not indexed").
  \
  \ This is an alternative action of the deferred word
  \ `reneeded`.
  \
  \ }doc

: thru-index-reneed ( "name" -- )
  parse-name thru-index-reneeded ;

  \ doc{
  \
  \ thru-index-reneed ( "name" -- )
  \
  \ Search the index word list for word "name". If found,
  \ execute it, causing its associated block be loaded.  If not
  \ found, throw an exception -277 ("needed, but not
  \ indexed").
  \
  \ This is an alternative action of the deferred word
  \ `reneed`.
  \
  \ }doc

: thru-index-needed ( ca len -- )
  needed-word 2@ 2>r  new-needed-word  2dup undefined?
  if    thru-index-reneeded
  else  2drop  then  2r> needed-word 2! ;

  \ doc{
  \
  \ thru-index-needed ( ca len -- )
  \
  \ If word _ca len_ is found in the current search order, do
  \ nothing. Otherwise search the index word list for it. If
  \ found, execute it, causing its associated block be loaded.
  \ If not found, throw an exception -277 ("needed, but not
  \ indexed").
  \
  \ This is an alternative action of the deferred word
  \ `needed`.
  \
  \ }doc

: thru-index-need ( "name" -- )
  parse-name thru-index-needed ; -->

  \ doc{
  \
  \ thru-index-need ( "name" -- )
  \
  \ If word "name" is found in the current search order, do
  \ nothing. Otherwise search the index word list for it. If
  \ found, execute it, causing its associated block be loaded.
  \ If not found, throw an exception -277 ("needed, but not
  \ indexed").
  \
  \ This is an alternative action of the deferred word
  \ `need`.
  \
  \ }doc

( make-thru-index use-thru-index )

: use-thru-index ( -- )
  ['] thru-index-reneeded ['] reneeded  defer!
  ['] thru-index-reneed   ['] reneed    defer!
  ['] thru-index-need     ['] need      defer!
  ['] thru-index-needed   ['] needed    defer!
  use-default-located ;

  \ doc{
  \
  \ use-thru-index ( -- )
  \
  \ Change the action of `need`, `needed`, `reneed`,
  \ `reneeded`, `located` and `unlocated` in order to use the
  \ blocks index created by `make-thru-index`.
  \
  \ The default action of all said words can be restored by
  \ `use-no-index`.
  \
  \ See also: `use-fly-index`.
  \
  \ }doc

: (make-thru-index) ( -- )
  last-locatable @ 1+ first-locatable @
  ?do i (index-block) loop ;

  \ XXX TODO -- factor `last-locatable @ 1+ first-locatable @`,
  \ to `need-bounds`; see `(located)`.

  \ doc{
  \
  \ (make-thru-index) ( -- )
  \
  \ Create the blocks index, from `first-locatable` to
  \ `last-locatable`.
  \
  \ ``(make-thru-index)`` is a factor of `make-thru-index`.
  \
  \ See also: `use-thru-index`.
  \
  \ }doc

: make-thru-index ( -- )
  get-current get-order set-index-order
  ['] (make-thru-index) catch  dup #-278 <> swap ?throw
  set-order set-current use-thru-index ;

  \ XXX TODO -- #-278 \ empty block found: quit indexing

  \ doc{
  \
  \ make-thru-index ( -- )
  \
  \ Create the blocks index and activate it. The current word
  \ list and the current search order are preserved.
  \
  \ ``make-thru-index`` first creates a blocks index, i.e. a
  \ word list from the names that are on the index (header)
  \ line of every searchable block, ignoring duplicates;
  \ second, it executes `use-thru-index` to activate the blocks
  \ index, changing the default behaivour of `need` and related
  \ words.
  \
  \ The words in the index have a fake execution token, which
  \ is the block they belong to.  This way, after indexing all
  \ the disk blocks only once, `need` will search the word list
  \ and load the block of the word found. On the contrary, the
  \ default action of `need` is to search all the blocks every
  \ time.
  \
  \ The default action of `need` and related words can be
  \ restored with `use-no-index`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-04-02: Start.
  \
  \ 2016-04-03: First working version.
  \
  \ 2016-04-24: Add `need nextname`, because `nextname` has
  \ been moved from the kernel to the library.
  \
  \ 2016-05-05: Update `s=` to `str=`. Improve documentation.
  \
  \ 2016-05-07: New method: the indexed word is an alias, which
  \ doesn't use data space, and its execution token is the
  \ block it's associated to. This way, no data space is used
  \ by the index.
  \
  \ 2016-05-14: Update: `evaluate` has been moved to the
  \ library.
  \
  \ 2016-08-05: Compact the code to save one block.
  \
  \ 2016-11-13: Check the code with the far-memory system
  \ recently implemented in the kernel. Now the library disk
  \ can be indexed. Improve the documentation. Remove old
  \ unused code.
  \
  \ 2016-11-19: Move to <blocks.index-wordlist.fsb> the code
  \ shared with the new module <blocks.fly-indexer.fsb>.
  \
  \ 2016-11-24: Rename the module and some words, to be
  \ consistent with the alternative version Fly Indexer.
  \
  \ 2016-11-25: Factor `use-default-located`. Improve
  \ documentation.
  \
  \ 2016-11-26: Need `catch`, which has been moved to the
  \ library.
  \
  \ 2016-12-30: Replace `search-index`, which was renamed on
  \ 2016-11-25, with `indexed-name?`.
  \
  \ 2017-01-17: Fix text of exception #-277 in documentation.
  \
  \ 2017-02-17: Update notation "behaviour" to "action".
  \ Update cross references.
  \
  \ 2017-02-21: Need `use-default-located`, which now is optional.
  \
  \ 2017-03-13: Improve documentation.

  \ vim: filetype=soloforth
