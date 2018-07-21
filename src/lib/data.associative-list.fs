  \ data.associative-list.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201807212110
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ An associative list implemented with standard word lists.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ Credit

  \ Based on code written by Wil Baden, published in Forth
  \ Dimensions (volume 17, number 4, page 36, 1995-11).

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( associative-list item? item create-entry )

need search-wordlist

: associative-list ( "name" -- ) wordlist constant ;

  \ doc{
  \
  \ associative-list ( "name" -- )
  \
  \ Create a new associative list "name".
  \
  \ See: `entry:`, `centry:`, `2entry:`, `sentry:`, `item`,
  \ `item?`, `items`, `associative:`.
  \
  \ }doc

: item? ( ca len wid -- false | xt true ) search-wordlist 0<> ;

  \ doc{
  \
  \ item? ( ca len wid -- false | xt true ) "item-question"
  \
  \ Is _ca len_ an item of the `associative-list` _wid_?  If so
  \ return its _xt_ and _true_, else return _false_.
  \
  \ See: `item`.  `entry:`, `centry:`, `2entry:`, `sentry:`,
  \ `items`.
  \
  \ }doc

: item ( ca len wid -- i*x ) item? 0= #-13 ?throw execute ;

  \ doc{
  \
  \ item ( ca len wid -- i*x )
  \
  \ If _ca len_ is an item of the `associative-list` _wid_,
  \ return its value _i*x_; else `throw` an exception #-13
  \ ("undefined word").
  \
  \ See: `item?`.  `entry:`, `centry:`, `2entry:`, `sentry:`,
  \ `items`.
  \
  \ }doc

: create-entry ( i*x wid xt "name" -- )
  get-current >r swap set-current  create execute
  r> set-current ;

  \ doc{
  \
  \ create-entry ( i*x wid xt "name" -- )
  \
  \ Create an entry "name" in the `associative-list` _wid_,
  \ using _xt_ to store its value _i*x_.
  \
  \ ``create-entry`` is a factor of `entry:`, `centry:`,
  \ `2entry:` and `sentry:`.
  \
  \ }doc

-->

( entry: centry: 2entry: sentry: items )

need create-entry  unneeding entry: ?(
: entry: ( x wid "name" -- )
  ['] , create-entry does> ( -- x ) ( dfa ) @ ; ?)

  \ doc{
  \
  \ entry: ( x wid "name" -- ) "entry-colon"
  \
  \ Create a cell entry "name" in the `associative-list`
  \ _wid_, with value _x_.
  \
  \ See: `centry:`, `2entry`, `sentry:`, `create-entry`.
  \
  \ }doc

unneeding centry: ?(
: centry: ( c wid "name" -- )
  ['] c, create-entry does> ( -- c ) ( dfa ) c@ ; ?)

  \ doc{
  \
  \ centry: ( c wid "name" -- ) "c-entry-colon"
  \
  \ Create a character entry "name" in the `associative-list`
  \ _wid_, with value _c_.
  \
  \ See: `entry:`, `2entry`, `sentry:`, `create-entry`.
  \
  \ }doc

unneeding 2entry: ?(
: 2entry: ( dx wid "name" -- )
  ['] 2, create-entry does> ( -- dx ) ( dfa ) 2@ ; ?)

  \ doc{
  \
  \ 2entry: ( dx wid "name" -- ) "two-entry-colon"
  \
  \ Create a double-cell entry "name" in the `associative-list`
  \ _wid_, with value _dx_.
  \
  \ See: `entry:`, `centry:`, `sentry:`, `create-entry`.
  \
  \ }doc

unneeding sentry: ?(
: sentry: ( ca len wid "name" -- )
  ['] s, create-entry does> ( -- ca len ) ( dfa ) count ; ?)

  \ doc{
  \
  \ sentry: ( ca len wid "name" -- ) "s-entry-colon"
  \
  \ Create a string entry "name" in the `associative-list`
  \ _wid_, with value _ca len_.
  \
  \ See: `entry:`, `centry:`, `2entry:`, `create-entry`.
  \
  \ }doc

unneeding items ?exit need alias need wordlist-words

' wordlist-words alias items ( wid -- )

  \ doc{
  \
  \ items ( wid -- )
  \
  \ List items of the `associative-list` _wid_.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-11-06: Start, adapted from Wil Baden's code.
  \
  \ 2016-03-24: Comments.
  \
  \ 2016-04-15: Improved with different types of items.
  \ Factored. An obscure bug was discovered during the changes.
  \ Finally its origin was found in `(;code)`, in the kernel,
  \ and fixed.
  \
  \ 2016-11-26: Need `search-wordlist`, which has been moved to
  \ the library.
  \
  \ 2016-12-15: Improve documentation. Rename `entry` and
  \ friends after `field:` and friends, and make them
  \ individually accessible by `need`. Compact the code to save
  \ one block. Move the test to the tests module.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-19: Remove remaining `exit` at the end of
  \ conditional interpretation.
  \
  \ 2017-09-09: Update notation "pfa" to the standard "dfa".
  \
  \ 2017-11-06: Improve documentation with cross-references.
  \
  \ 2018-02-06: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-07-21: Improve documentation, linking `throw`.

  \ vim: filetype=soloforth
