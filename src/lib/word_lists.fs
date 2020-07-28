  \ word_lists.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202007282031
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words related to word lists.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2016, 2017, 2018, 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( wordlist>link wordlist>name wordlist-name@ wordlist-name! )

unneeding wordlist>link

?\ need alias  ' cell+ alias wordlist>link ( wid -- a )

  \ doc{
  \
  \ wordlist>link ( wid -- a ) "wordlist-to-link"
  \
  \ Return the link field address _a_ of the `wordlist` identifier
  \ _wid_, which holds the word-list identifier of the previous
  \ word list defined in the system.
  \
  \ See also: `wordlist>name`, `wordlist>last`, `/wordlist`.
  \
  \ }doc

unneeding wordlist>name

?\ : wordlist>name ( wid -- a ) cell+ cell+ ;

  \ doc{
  \
  \ wordlist>name ( wid -- a ) "wordlist-to-name"
  \
  \ Return the address _a_ which holds the _nt_ of the
  \ `wordlist` identifier _wid_ (or zero if the word list has
  \ no associated name).
  \
  \ See also: `wordlist>link`, `wordlist>last`, `/wordlist`.
  \
  \ }doc

unneeding wordlist-name@ ?( need wordlist>name

: wordlist-name@ ( wid -- nt|0 ) wordlist>name @ ; ?)

  \ doc{
  \
  \ wordlist-name@ ( wid -- nt|0 ) "wordlist-name-fetch"
  \
  \ Fetch from the word-list identifier _wid_ its associated
  \ name _nt_, or zero if the word list has no associated name.
  \
  \ See also: `wordlist`, `wordlist-name!`, `wordlist>name`.
  \
  \ }doc

unneeding wordlist-name! ?( need wordlist>name

: wordlist-name! ( nt wid -- ) wordlist>name ! ; ?)

  \ doc{
  \
  \ wordlist-name! ( nt wid -- ) "wordlist-name-store"
  \
  \ Store _nt_ as the name associated to the word list
  \ identified by _wid_.  _nt_ is stored into the name field of
  \ the word-list metadata.
  \
  \ See also: `wordlist`, `wordlist-name@`, `wordlist>name`.
  \
  \ }doc

( +order -order /wordlist wordlist>last )

unneeding +order

?\ need -order  : +order ( wid -- ) dup -order >order ;

  \ Credit:
  \
  \ Original code by Julian Fondren:
  \
  \ http://forth.minimaltype.com/packages.html

  \ doc{
  \
  \ +order ( wid -- ) "plus-order"
  \
  \ Remove all instances of the word list identified by _wid_
  \ from the search order, then add it to the top.
  \
  \ See also: `-order`, `>order`, `set-order`, `order`.
  \
  \ }doc

unneeding -order ?( need n>r need under+

variable -order-wid
  \ XXX TMP -- used as a local
  \ XXX TODO -- use an actual local when available

: -order ( wid -- )
  -order-wid !  get-order n>r r> dup
  begin dup  while  1-
    r@ -order-wid @ = if  rdrop -1 under+  else  r> -rot  then
  repeat  drop set-order ; ?)

  \ Credit:
  \
  \ Original code for Gforth by Julian Fondren:
  \
  \ http://forth.minimaltype.com/packages.html

  \ doc{
  \
  \ -order ( wid -- ) "minus-order"
  \
  \ Remove all instances of word list identified by _wid_ from
  \ the search order.
  \
  \ See also: `+order`, `>order`, `set-order`, `order`.
  \
  \ }doc

unneeding /wordlist ?\ 3 cells cconstant /wordlist

  \ doc{
  \
  \ /wordlist ( -- n )
  \
  \ A `cconstant`. _n_ is the length in bytes of a `wordlist`
  \ data structure, created by `wordlist,`.
  \
  \ }doc

unneeding wordlist>last ?( need alias

' noop alias wordlist>last ( wid -- a ) immediate ?)

  \ doc{
  \
  \ wordlist>last ( wid -- a ) "wordlist-to-last"
  \
  \ Return the field address _a_ of `wordlist` identifier
  \ _wid_, which holds the name token of the latest word
  \ defined in _wid_.
  \
  \ As _a_ is the first field of a word-list structure,
  \ ``wordlist>last`` is provided only for legibility. It is
  \ an `immediate` `alias` of `noop`.
  \
  \ See also: `wordlist>name`, `wordlist>link`, `/wordlist`, `last`,
  \ `latest`.
  \
  \ }doc

( wordlist-of latest>wordlist wordlist>vocabulary vocabulary )

unneeding wordlist-of

?\ need >body  : wordlist-of ( "name" -- wid ) ' >body @ ;

  \ doc{
  \
  \ wordlist-of ( "name" -- wid )
  \
  \ Return the word-list identifier _wid_ associated to
  \ vocabulary _name_.
  \
  \ Origin: eForth's ``widof``.
  \
  \ See also: `wordlist`, `vocabulary`.
  \
  \ }doc

unneeding latest>wordlist  ?( need wordlist-name!

: latest>wordlist ( wid -- ) latest swap wordlist-name! ; ?)

  \ doc{
  \
  \ latest>wordlist ( wid -- ) "latest-to-wordlist"
  \
  \ Associate the latest name to the word list identified by
  \ _wid_.
  \
  \ See also: `wordlist`, `wordlist-name!`,
  \ `wordlist>vocabulary`, `wordlists`, `latest`.
  \
  \ }doc

unneeding wordlist>vocabulary ?( need latest>wordlist

: wordlist>vocabulary ( wid "name" -- )
  create dup , latest>wordlist dovocabulary ; ?)

  \ doc{
  \
  \ wordlist>vocabulary ( wid "name" -- ) "wordlist-to-vocabulary"
  \
  \ Create a vocabulary _name_ for the word list identified by
  \ _wid_.
  \
  \ See also: `wordlist`, `vocabulary`, `latest>wordlist`,
  \ `wordlists`.
  \
  \ }doc

unneeding vocabulary ?( need wordlist>vocabulary

: vocabulary ( "name" -- ) wordlist wordlist>vocabulary ; ?)

  \ doc{
  \
  \ vocabulary ( "name" -- )
  \
  \ Create a vocabulary _name_. A vocabulary is a named word
  \ list. Subsequent execution of _name_ replaces the first
  \ entry in the search order with the word list associated to
  \ _name_. When _name_ becomes the compilation word list new
  \ definitions will be appended to _name_'s word list.
  \
  \ Origin: Forth-83 (Required Word Set).
  \
  \ See also: `wordlist`, `definitions`, `wordlist-of`,
  \ `set-current`.
  \
  \ }doc

( seal trail find-name-in find swap-current search-wordlist )

unneeding seal ?\ : seal ( -- ) 1 #order ! ;

  \ doc{
  \
  \ seal ( -- )
  \
  \ Remove all word lists from the search order other than the
  \ word list that is currently on top of the search order.
  \ I.e., change the search order such that only the word list
  \ at the top of the search order will be searched.
  \
  \ Origin: Gforth.
  \
  \ See also: `only`, `set-order`, `#order`.
  \
  \ }doc

unneeding trail ?\ : trail ( -- nt ) context @ @ ;

  \ doc{
  \
  \ trail ( -- nt )
  \
  \ Leave the _nt_ of the topmost word in the first word list
  \ of the search order.
  \
  \ See also: `set-order`, `context`.
  \
  \ }doc

unneeding find-name-in

?\ : find-name-in ( ca len wid -- nt | 0 ) @ find-name-from ;

  \ doc{
  \
  \ find-name-in ( ca len wid -- nt | 0 )
  \
  \ Find the definition named in the string at _ca len_, in the
  \ word list identified by _wid_. If the definition is found,
  \ return its _nt_, else return zero.
  \
  \ See also: `search-wordlist`, `find-name-from`, `find-name`,
  \ `<<src-lib-word_lists-fs, find>>`.
  \
  \ }doc

unneeding find ?(

: find ( ca -- ca 0 | xt 1 | xt -1 )
  dup count find-name dup
  if nip name>immediate? 1 or negate then ; ?)

  \ doc{
  \
  \ find ( ca -- ca 0 | xt 1 | xt -1 )
  \
  \ Find the definition named in the counted  string at _ca_.
  \ If the definition is  not found, return _ca_ and zero. If
  \ the definition is found, return its execution token _xt_.
  \ If the definition  is immediate,  also  return one  (1),
  \ otherwise  also  return minus-one (-1).
  \
  \ Origin: Forth-83 (Required Word Set), Forth-94 (CORE,
  \ SEARCH), Forth-2012 (CORE, SEARCH).
  \
  \ See also: `find-name`, `find-name-from`, `find-name-in`.
  \
  \ }doc

unneeding swap-current ?(

: swap-current ( wid1 -- wid2 )
  get-current swap set-current ; ?)

  \ Credit:
  \
  \ Idea for `swap-current` from lpForth.

  \ doc{
  \
  \ swap-current ( wid1 -- wid2 )
  \
  \ Exchange the contents of the current compilation word list,
  \ which is identified by _wid2_, with the word list
  \ identified by _wid1_.
  \
  \ Origin: lpForth.
  \
  \ }doc

unneeding search-wordlist ?(

: search-wordlist ( ca len wid -- 0 | xt 1 | xt -1 )
  @ find-name-from dup 0= ?exit  name>immediate? 0= 1 or ; ?)

  \ doc{
  \
  \ search-wordlist ( ca len wid -- 0 | xt 1 | xt -1 )
  \
  \ Find the definition identified by the string _ca len_ in
  \ the word list identified by _wid_. If the definition is not
  \ found, return zero. If the definition is found, return its
  \ _xt_ and one (1) if the definition is immediate, minus-one
  \ (-1) otherwise.
  \
  \ Origin: Forth-94 (SEARCH), Forth-2012 (SEARCH).
  \
  \ See also: `find-name`, `find-name-from`, `find-name-in`,
  \ `<<src-lib-word_lists-fs, find>>`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-04-17: Added the requisite of `recurse`, which is not
  \ in the kernel anymore.
  \
  \ 2016-05-02: Join several blocks, to save space.
  \
  \ 2016-05-04: Improve the documentation of `trail`.
  \
  \ 2016-05-17: Need `>body`, which has been moved to the
  \ library. Improve conditional compilation.
  \
  \ 2016-05-18: Simplify conditional compilation. Move
  \ `vocabulary` from the kernel and rewrite it after
  \ `wid>vocabulary` and the new word `dovocabulary`.
  \
  \ 2016-11-26: Improve `(wid>name`. Remove old definitions
  \ `get-order` and `order@`. Move `seal` and `search-wordlist`
  \ from the kernel.
  \
  \ 2016-12-06: Improve documentation of `wid>link`, `wid>name`
  \ and `(wid>name`.  Add `-order` and `+order`.
  \
  \ 2017-01-06: Add `link>wid`. Improve documentation.
  \
  \ 2017-01-07: Remove `link>wid`, not needed anymore, because
  \ the word-list fields are accessed from its identifier, and
  \ the linked list of word lists uses the word-list
  \ identifiers.  Rename `wid>link` to `wordlist>link`.  Rename
  \ `(wid>name` to `wordlist>name`.  Remove `wid>name`.  Rename
  \ `wid>vocabulary` to `wordlist>vocabulary`.  Add
  \ `wordlist-name@` and `wordlist-name!`.  Rename `wid-of` to
  \ `wordlist-of`.  Rename `named-wid` to `latest>wordlist`.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-18: Remove remaining `exit` at the end of
  \ conditional interpretation.
  \
  \ 2017-02-17: Fix markup in documentation.  Update cross
  \ references.
  \
  \ 2017-02-20: Update notation of word sets.
  \
  \ 2017-12-15: Remove remaining `exit` at the end of
  \ conditional interpretation.  Improve documentation,
  \ needings and layout.
  \
  \ 2018-02-05: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-04-14: Fix typo.
  \
  \ 2020-05-02: Fix false link in documentation.
  \
  \ 2020-05-05: Fix markup in documentation. Update cross
  \ reference.
  \
  \ 2020-05-26: Make links to `find` explicit (there's another
  \ `find` in the Specforth editor).
  \
  \ 2020-06-04: Fix cross references.
  \
  \ 2020-06-06: Add `/wordlist`, `wordlist>last`.

  \ vim: filetype=soloforth
