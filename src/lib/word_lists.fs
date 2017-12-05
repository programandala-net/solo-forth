  \ word_lists.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702221550
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words related to word lists.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( wordlist>link wordlist>name wordlist-name@ wordlist-name! )

[unneeded] wordlist>link
?\ need alias  ' cell+ alias wordlist>link ( wid -- a ) exit

  \ doc{
  \
  \ wordlist>link ( wid -- a )
  \
  \ Return the link field address _a_ of word-list identifier
  \ _wid_, which holds the word-list identifier of the previous
  \ word list defined in the system.
  \
  \ See: `wordlist`, `wid>name`.
  \
  \ }doc

[unneeded] wordlist>name
?\ : wordlist>name ( wid -- a ) cell+ cell+ ;

  \ doc{
  \
  \ wordlist>name ( wid -- a )
  \
  \ Return the address _a_ which holds the _nt_ of word-list
  \ identifier _wid_ (or zero if the word list has no
  \ associated name).
  \
  \ See: `wordlist`, `wordlist>link`.
  \
  \ }doc

[unneeded] wordlist-name@ dup
?\ need wordlist>name
?\ : wordlist-name@ ( wid -- nt|0 ) wordlist>name @ ;

  \ doc{
  \
  \ wordlist-name@ ( wid -- nt|0 )
  \
  \ Fetch from the word-list identifier _wid_ its associated
  \ name _nt_, or zero if the word list has no associated name.
  \
  \ See: `wordlist`, `wordlist-name!`, `wordlist>name`.
  \
  \ }doc

[unneeded] wordlist-name! dup
?\ need wordlist>name
?\ : wordlist-name! ( nt wid -- ) wordlist>name ! ;

  \ doc{
  \
  \ wordlist-name! ( nt wid -- )
  \
  \ Store _nt_ as the name associated to the word list
  \ identified by _wid_.  _nt_ is stored into the name field of
  \ the word-list metadata.
  \
  \ See: `wordlist`, `wordlist-name@`, `wordlist>name`.
  \
  \ }doc

( +order -order )

[unneeded] +order
?\ need -order  : +order ( wid -- ) dup -order >order ;

  \ Credit:
  \
  \ Original code by Julian Fondren:
  \
  \ http://forth.minimaltype.com/packages.html

  \ doc{
  \
  \ +order ( wid -- )
  \
  \ Remove all instances of the word list identified by _wid_
  \ from the search order, then add it to the top.
  \
  \ See: `-order`, `>order`, `set-order`, `order`.
  \
  \ }doc

[unneeded] -order ?exit

need n>r need under+

variable -order-wid
  \ XXX TMP -- used as a local
  \ XXX TODO -- use an actual local when available

: -order ( wid -- )
  -order-wid !  get-order n>r r> dup
  begin dup  while  1-
    r@ -order-wid @ = if  rdrop -1 under+  else  r> -rot  then
  repeat  drop set-order ;

  \ Credit:
  \
  \ Original code for Gforth by Julian Fondren:
  \
  \ http://forth.minimaltype.com/packages.html

  \ doc{
  \
  \ -order ( wid -- )
  \
  \ Remove all instances of word list identified by _wid_ from
  \ the search order.
  \
  \ See: `+order`, `>order`, `set-order`, `order`.
  \
  \ }doc

( wordlist-of latest>wordlist wordlist>vocabulary vocabulary )

[unneeded] wordlist-of
?\ need >body  : wordlist-of ( "name" -- wid ) ' >body @ ;

  \ doc{
  \
  \ wordlist-of ( "name" -- wid )
  \
  \ Return the word-list identifier _wid_ associated to
  \ vocabulary _name_.
  \
  \ Origin: eForth's `widof`.
  \
  \ See: `wordlist`, `vocabulary`.
  \
  \ }doc

[unneeded] latest>wordlist  ?( need wordlist-name!

: latest>wordlist ( wid -- ) latest swap wordlist-name! ; ?)

  \ doc{
  \
  \ latest>wordlist ( wid -- )
  \
  \ Associate the latest name to the word list identified by
  \ _wid_.
  \
  \ See: `wordlist`, `wordlist-name!`,
  \ `wordlist>vocabulary`, `wordlists`, `latest`.
  \
  \ }doc

[unneeded] wordlist>vocabulary ?( need latest>wordlist
: wordlist>vocabulary ( wid "name" -- )
  create dup , latest>wordlist dovocabulary ; ?)

  \ doc{
  \
  \ wordlist>vocabulary ( wid "name" -- )
  \
  \ Create a vocabulary _name_ for the word list identified by
  \ _wid_.
  \
  \ See: `wordlist`, `vocabulary`, `latest>wordlist`,
  \ `wordlists`.
  \
  \ }doc

[unneeded] vocabulary ?( need wordlist>vocabulary
: vocabulary ( "name" -- )
  wordlist wordlist>vocabulary ; ?)

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
  \ See: `wordlist`, `definitions`, `wordlist-of`.
  \
  \ }doc

( seal trail find-name-in find swap-current search-wordlist )

[unneeded] seal
?\ : seal ( -- ) 1 #order ! ;

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
  \ See: `only`, `set-order`, `#order`.
  \
  \ }doc

[unneeded] trail ?\ : trail ( -- nt ) context @ @ ;

  \ doc{
  \
  \ trail ( -- nt )
  \
  \ Leave the _nt_ of the topmost word in the first word list
  \ of the search order.
  \
  \ See: `set-order`, `context`.
  \
  \ }doc

[unneeded] find-name-in
?\ : find-name-in ( ca len wid -- nt | 0 ) @ find-name-from ;

  \ doc{
  \
  \ find-name-in ( ca len wid -- nt | 0 )
  \
  \ Find the definition named in the string at _ca len_, in the
  \ word list identified by _wid_. If the definition is found,
  \ return its _nt_, else return zero.
  \
  \ See: `search-wordlist`, `find-name-from`, `find-name`,
  \ `find`.
  \
  \ }doc

[unneeded] find ?(

: find ( ca -- ca 0 | xt 1 | xt -1 )
  dup count find-name dup
  if  nip name>immediate? 1 or negate  then ; ?)

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
  \ See: `find-name`, `find-name-from`, `find-name-in`.
  \
  \ }doc

[unneeded] swap-current ?(
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
  \ which is idenfified by _wid2_, with the word list
  \ identified by `wid1`.
  \
  \ Origin: lpForth.
  \
  \ }doc

[unneeded] search-wordlist ?(
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
  \ See: `find-name`, `find-name-from`, `find-name-in`,
  \ `find`.
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

  \ vim: filetype=soloforth
