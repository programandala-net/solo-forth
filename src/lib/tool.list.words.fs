  \ tool.list.words.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202101072036.
  \ See change log at the end of the file.

  \ ===========================================================
  \ Description

  \ Words to list words.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018,
  \ 2020, 2021.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( .word .wordname more-words? words wordlist-words )

unneeding .word ?( need 'cr' need .name need cvariable

cvariable word-separator  'cr' word-separator c!

  \ doc{
  \
  \ word-separator  ( -- ca )
  \
  \ Return the address _ca_ containing the character printed by
  \ `(.word` after every word.
  \
  \ ``word-separator`` is a `cvariable`` whose default content
  \ is `'cr'`, i.e. a carriage return. Useful alternative
  \ values are `'tab'` (a tabulator) and `bl` (a space).
  \
  \ NOTE: ``word-separator`` is not locatable in the library.
  \ It's compiled together with `.word`.
  \
  \ See also: `.word`, `words`.
  \
  \ }doc

: (.word ( nt -- ) .name word-separator c@ emit ;

  \ doc{
  \
  \ (.word ( nt -- ) "paren-dot-word"
  \
  \ Default action of `.word`: display the name of the
  \ definition _nt_ and `emit` the content of `word-separator`.
  \
  \ NOTE: ``(.word`` is not locatable in the library.
  \ It's compiled together with `.word`.
  \
  \ }doc

defer .word ( nt -- ) ' (.word ' .word defer! ?)

  \ doc{
  \
  \ .word ( nt -- ) "dot-word"
  \
  \ A deferred word (see `defer`) whose default action is
  \ `(.word`.
  \
  \ ``.word`` is used by `words`, `words-like` and
  \ `wordlist-words`, therefore the user can modify the output
  \ of these three words by configuring ``.word`` with a
  \ different action.
  \
  \ See also: `word-separator`.
  \
  \ }doc

unneeding .wordname ?( need u.r need .name

: .wordname ( nt -- ) cr dup 5 u.r space .name ;
' .wordname ' .word defer! ?)

  \ doc{
  \
  \ .wordname ( nt -- ) "dot-wordname"
  \
  \ An alternative action for the deferred word `.word` (see
  \ `defer`), which is used by `words`, `words-like` and
  \ `wordlist-words`.  ``.wordname`` prints _nt_ and its
  \ correspondent name.
  \
  \ }doc

unneeding more-words? ?( need nuf?
: more-words? ( nt|0 -- nt|0 f ) dup 0<>  nuf? 0= and ; ?)

  \ doc{
  \
  \ more-words? ( nt|0 -- nt|0 f ) "more-words-question"
  \
  \ A common factor of `words` and `words-like`.
  \
  \ }doc

unneeding words ?( need trail need more-words?
                     need .word need name<name

: words ( -- ) trail begin  more-words?  while
                dup .word name<name  repeat  drop ; ?)

  \ Credit:
  \
  \ Code adapted from Spectrum Forth-83.

  \ doc{
  \
  \ words ( -- )
  \
  \ List the definition names in the first word list of
  \ the search order.
  \
  \ Origin: Forth-83 (Uncontrolled Reference Words), Forth-94
  \ (TOOLS), Forth-2012 (TOOLS).
  \
  \ See also: `wordlist-words`, `wordlists`.
  \
  \ }doc

unneeding wordlist-words  ?( need words

: wordlist-words ( wid -- ) >order words previous ; ?)

  \ doc{
  \
  \ wordlist-words ( wid -- )
  \
  \ List the definition names in word list _wid_.
  \
  \ See also: `words`, `wordlists`.
  \
  \ }doc

( words-like words# )

unneeding words-like ?( need trail need name<name
                         need lowers need more-words?
                         need contains need .word need tab

: words-like ( "name" -- )
  parse-name 2dup lowers trail ( ca len nt )
  begin  more-words?  while
    dup >r name>string 2over contains if  r@ .word  then
        r> name<name
  repeat drop 2drop ; ?)

  \ Credit:
  \
  \ Code of `words-like` adapted from pForth.

  \ doc{
  \
  \ words-like ( "name" -- )
  \
  \ List the definition names, from the first word list of
  \ the search order, that contain substring "name".
  \
  \ }doc

unneeding words# ?( need trail need name<name
: words# ( -- n ) 0 trail begin ( n nt ) dup 0<>  while
                             swap 1+ swap  name<name
                           repeat drop ; ?)

  \ doc{
  \
  \ words# ( -- n ) "words-number-sign"
  \
  \ Return number _n_ of words defined in the first word list
  \ of the search order.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-09-12: Fix `words-like`.
  \
  \ 2016-05-04: Compact the blocks. Add `words#`. Document.
  \
  \ 2016-11-17: Use `?(` instead of `[if]`.
  \
  \ 2016-11-24: Improve needing of individual words. Replace
  \ `.name` with `.word`, a deferred word with default
  \ behaviour `(.word`. Add its alternative behaviour
  \ `.wordname`.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-19: Remove remaining `exit` at the end of
  \ conditional interpretation.
  \
  \ 2017-02-01: Fix `words-like` with `lowers` (words are
  \ stored in lowercase).
  \
  \ 2017-02-17: Update notation "behaviour" to "action". Update
  \ cross references.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-05-07: Improve documentation.
  \
  \ 2017-12-15: Improve documentation.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-09: Add words' pronunciaton.
  \
  \ 2018-06-04: Fix documentation.
  \
  \ 2018-06-04: Update: remove trailing closing paren from word
  \ names.
  \
  \ 2020-02-27: Fix typo in documentation.
  \
  \ 2020-05-08: Update requirements: `.name` has been moved
  \ from the kernel to the library.
  \
  \ 2020-05-24: Replace "hash" notation with "number sign".
  \
  \ 2020-07-28: Improve documentation of deferred words.
  \
  \ 2021-01-07: Add `word-separator` to make `(.word`
  \ configurable; make a carriage return its default value
  \ instead; formerly a tabulator was emitted.

  \ vim: filetype=soloforth
