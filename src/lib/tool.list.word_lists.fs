  \ tool.list.word_lists.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202006082154
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Tool words to list word lists.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018,
  \ 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( .current .context .wordlist dump-wordlist order )

unneeding .current
?\ : .current ( -- ) get-current .wordlist ;

  \ doc{
  \
  \ .current ( -- ) "dot-current"
  \
  \ Display the compilation word list.
  \
  \ See: `get-current`, `.wordlist`, `order`.
  \
  \ }doc

unneeding .context ?( need .wordlist

: .context ( -- )
  get-order begin ?dup while swap .wordlist 1- repeat ; ?)

  \ doc{
  \
  \ .context ( -- ) "dot-context"
  \
  \ Display the word lists in the search order in their search
  \ order sequence, from first searched to last searched.
  \
  \ See: `get-order`, `.wordlist`, `order`.
  \
  \ }doc

unneeding .wordlist ?( need wordlist>name need .name

: .wordlist ( wid -- )
  dup wordlist>name @ ?dup if .name drop exit then u. ; ?)

  \ doc{
  \
  \ .wordlist ( wid -- ) "dot-wordlist"
  \
  \ If the `wordlist` identified by _wid_ has an associated
  \ name, display it; else display _wid_.
  \
  \ See: `wordlists`, `dump-wordlist`, `wordlist>name`.
  \
  \ }doc

unneeding dump-wordlist ?( need .wordlist need wordlist>last
                           need .name

: dump-wordlist ( wid -- ) dup cr ." Word list: " .wordlist
  cr ." Latest definition: " wordlist>last @ ?dup 0exit
  .name ; ?)

  \ doc{
  \
  \ dump-wordlist ( wid -- )
  \
  \ Dump the data of the `wordlist` identified by _wid_, with
  \ labels: its associated name (or, if none, just the _wid_)
  \ and the name of the latest definition created in the word
  \ list.
  \
  \ See: `.wordlist`, `dump-wordlists`, `wordlist>last`,
  \ `.name`.
  \
  \ }doc

unneeding order ?( need .context need .current  : order ( -- )
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

( wordlists dump-wordlists> dump-wordlists )

unneeding wordlists ?( need .wordlist need wordlist>link

: wordlists ( -- )
  last-wordlist
  begin @ ( wid|0) ?dup while dup .wordlist wordlist>link
  repeat ; ?)

  \ doc{
  \
  \ wordlists ( -- )
  \
  \ List all wordlists defined in the system, either by name
  \ (if they have an associated name) or by number (its word
  \ list identifier, if they have no associated name). The word
  \ lists are listed in reverse chronological order: The first
  \ word list listed is the most recently defined.
  \
  \ See: `.wordlist`, `words`, `wordlist-words`, `wordlist`,
  \ `last-wordlist`.
  \
  \ }doc

unneeding dump-wordlists> ?( need dump-wordlist
                             need wordlist>link

: dump-wordlists> ( wid -- )
  begin ( wid|0) ?dup
  while dup dump-wordlist cr wordlist>link @ repeat ; ?)

  \ doc{
  \
  \ dump-wordlists> ( wid -- ) "dump-wordlists-from"
  \
  \ Dump the data of all the word lists defined in the system,
  \ starting from the `wordlist` identified by _wid_.
  \
  \ ``dump-wordlists>`` is a useful factor of `dump-wordlists`.
  \
  \ See: `dump-wordlist`, `wordlists`, `wordlist>link`.
  \
  \ }doc

unneeding dump-wordlists ?( need dump-wordlists>

: dump-wordlists ( -- )
  last-wordlist @ dump-wordlists> ; ?)

  \ doc{
  \
  \ dump-wordlists ( -- )
  \
  \ Dump the data of all the word lists defined in the system,
  \ starting from the `wordlist` pointed by `last-wordlist`.
  \
  \ See: `dump-wordlist`, `dump-wordlists>`, `wordlists`.
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
  \ `wid>link` to `wordlist>link`, and `wid>vocabulary` to
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
  \
  \ 2017-12-15: Improve documentation.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-09: Add words' pronunciaton.
  \
  \ 2020-05-08: Update requirements: `.name` has been moved
  \ from the kernel to the library.
  \
  \ 2020-06-06: Add `dump-wordlists`, `dump-wordlists>` and
  \ `dump-wordlist`. Update source style.
  \
  \ 2020-06-07: Improve `dump-wordlist`: ignore empty latest
  \ definition.
  \
  \ 2020-06-08: Update: rename `latest-wordlist` to
  \ `last-wordlist`. Update: now `0exit` is in the kernel.

  \ vim: filetype=soloforth
