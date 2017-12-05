  \ tool.list.word_lists.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702221550
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Tool words to list word lists.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( .current .context .wordlist )

[unneeded] .current
?\ : .current ( -- ) get-current .wordlist ;

  \ doc{
  \
  \ .current ( -- )
  \
  \ Display the compilation word list.
  \
  \ See: `get-current`, `.wordlist`, `order`.
  \
  \ }doc

[unneeded] .context ?( need .wordlist

: .context ( -- )
  get-order begin  ?dup  while  swap .wordlist 1-  repeat ; ?)

  \ doc{
  \
  \ .context ( -- )
  \
  \ Display the word lists in the search order in their search
  \ order sequence, from first searched to last searched.
  \
  \ See: `get-order`, `.wordlist`, `order`.
  \
  \ }doc

[unneeded] .wordlist ?( need wordlist>name

: .wordlist ( wid -- )
  dup wordlist>name @ ?dup if  .name drop exit  then  u. ; ?)

  \ doc{
  \
  \ .wordlist ( wid -- )
  \
  \ If the word list identified by _wid_ has an associated
  \ name, display it; else display _wid_.
  \
  \ See: `wordlists`.
  \
  \ }doc

( wordlists order )

[unneeded] wordlists ?( need .wordlist need wordlist>link

: wordlists ( -- )
  latest-wordlist
  begin  @ ( wid|0) ?dup  while  dup .wordlist wordlist>link
  repeat ; ?)

  \ doc{
  \
  \ wordlists ( -- )
  \
  \ List all wordlists defined in the system, either by name
  \ (if they have an associated name) or by number (its word
  \ list identifier, if they have no associated name). The
  \ word lists are listed in reverse chronological order: The
  \ first word list listed is the most recently defined.
  \
  \ See: `.wordlist`, `wordlist`, `latest-wordlist`.
  \
  \ }doc

[unneeded] order ?( need .context need .current

: order ( -- )
  cr ." Search: " .context cr ." Compile: " .current ; ?)

  \ Display the search order currently in effect and the name
  \ of the `current` vocabulary.

  \ doc{
  \
  \ order ( -- )
  \
  \ Display the word lists in the search order in their search
  \ order sequence, from first searched to last searched. Also
  \ display the word list into which new definitions will be
  \ placed.
  \
  \ Origin: Forth-2012 (SEARCH EXT).
  \
  \ See: `.context`, `.current`, `.wordlist`, `set-order`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015..2016: Main development.
  \
  \ 2016-04-11: Documented.
  \
  \ 2016-05-01: Update.
  \
  \ 2016-05-02: Join two blocks to save space.
  \
  \ 2016-05-05: Remove unnecessary `space` from `.wid`.
  \
  \ 2016-05-06: Improve printing of nameless word lists.
  \
  \ 2016-11-17: Use `?(` instead of `[if]`.
  \
  \ 2017-01-06: Make all words independent to `need`. Fix and
  \ finish `wordlists`.
  \
  \ 2017-01-07: Update `wid>name` to `wordlist>name @`,
  \ `wid>link` to `wordlist>link`,  and `wid>vocabulary` to
  \ `wordlist>vocabulary`, after the changes in the word-list
  \ interface words. Rename `.wid` to `.wordlist`.  Rename
  \ `named-wid` to `latest>wordlist` in documentation.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-19: Remove remaining `exit` at the end of
  \ conditional interpretation.
  \
  \ 2017-02-17: Update cross references.

  \ vim: filetype=soloforth
