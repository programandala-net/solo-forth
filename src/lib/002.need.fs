  \ 002.need.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202005241405
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ The `need` utility which manages the code dependencies.
  \ For convenience it is in block 2.
  \
  \ The utility consists of words `need`, `needed`, `reneed`
  \ and `reneeded`. All of them are deferred words. Their
  \ default action is set by `use-no-index`: locate the
  \ required word searching a configurable range of blocks.
  \
  \ Alternative actions are provided by the optional tools
  \ Thru Indexer and Fly Indexer.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018,
  \ 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( delimited located )

: ((in-block-header? ( ca len -- f )
  \ 0 3 at-xy 64 spaces 0 3 at-xy \ XXX INFORMER
  begin 2dup parse-name
        \ 2dup type \ XXX INFORMER
        2dup s" )" str= if 2drop 2drop 2drop false exit then
                   str= if 2drop             true  exit then
  again ;

: (in-block-header? ( ca len u -- f )
  block>source parse-name s" (" str= if   ((in-block-header?
                                     else 2drop false then ;

  \ (in-block-header? ( ca len u -- f )
  \
  \ Is name _ca len_ in the header of block _u_? The current
  \ source is not preserved.
  \
  \ ``(in-block-header?`` is a factor of `in-block-header?`.

: in-block-header? ( ca len u -- f )
  nest-source (in-block-header? unnest-source ; -->

  \ in-block-header? ( ca len u -- f )
  \
  \ Is name _ca len_ in the header of block _u_? The current
  \ source is preserved with `nest-source` and `unnest-source`.
  \
  \ See: `(in-block-header?`.

( ... )

: contains ( ca1 len1 ca2 len2 -- f ) search nip nip ;
  \ Does string _ca1 len1_ contain string _ca2 len2_?
  \
  \ ``contains`` is defined also in <strings.misc.fsb>, because
  \ it cannot be loaded by the applications from this block
  \ (because `unneeding` is not defined at this point).
  \ That's why `contains` is not included in the block header.
  \
  \ XXX TODO -- Not needed with multiline block headers.

variable default-first-locatable  variable first-locatable
variable last-locatable  blocks/disk 1- last-locatable !
  \ Variables that define the range of blocks to be searched
  \ by `located` and its descendants.

  \ doc{
  \
  \ first-locatable ( -- a )
  \
  \ A `variable`. _a_ is the address of a cell containing  the
  \ number of the first block to be searched by `located` and
  \ its descendants.
  \
  \ See: `last-locatable`, `need-from`.
  \
  \ }doc

  \ doc{
  \
  \ default-first-locatable ( -- a )
  \
  \ A `variable`. _a_ is the address of a cell containing the
  \ default number of the first block to be searched by
  \ `located` and its descendants.
  \
  \ See: `first-locatable`.
  \
  \ }doc

  \ doc{
  \
  \ last-locatable ( -- a )
  \
  \ A `variable`. _a_ is the address of a cell containing the
  \ number of the last block to be searched by `located` and
  \ its descendants. Its default value is the last block of the
  \ disk.
  \
  \ See: `first-locatable`.
  \
  \ }doc

: delimited ( ca1 len1 -- ca2 len2 )
  dup 2+ dup allocate-stringer swap ( ca1 len1 ca2 len2 )
  2dup blank  2dup 2>r drop char+ smove 2r> ;
  \ XXX TODO -- Not needed with multiline block headers.

  \ doc{
  \
  \ delimited ( ca1 len1 -- ca2 len2 )
  \
  \ Add one leading space and one trailing space to string _ca1
  \ len1_, returning the result _ca2 len2_ in the `stringer`.
  \
  \ }doc

defer unlocated ( block -- )

  \ doc{
  \
  \ unlocated ( block -- )
  \
  \ Deferred word called in the loop of `located`, when the
  \ word searched for is not located in _block_.  Its default
  \ action is `drop`, which is changed by `use-fly-index`
  \ in order to index the blocks on the fly.
  \
  \ }doc

  \ defer .info ( -- ) ' noop ' .info defer!

-->

( ... )

: 1-line-(located ( ca len -- block | false )

  \ ----------------------------
  \ 2dup type space
  \ cr 2dup type space .s
  \ cr 2dup type get-current dup u. 0= if quit then
  \ cr 2dup type space .s depth 2 > if key drop then
  \ cr 2dup type space rp@ rp0 @ - -2 / . np@ u. cr .s
  \ cr 2dup type space rp@ rp0 @ - -2 / . .s
  \ cr 2dup type space rp@ rp0 @ - -2 / . .s .info
  \ cr ." (located<" 2dup type ." >"
  \ cr ."  base: #" base @ dup decimal . base !
  \ cr ."  rdepth: " rp@ rp0 @ - -2 / .
  \ cr ."  unused-stringer: " unused-stringer .
  \ ----------------------------
    \ XXX INFORMER -- options for debugging

  ?dup 0= #-32 ?throw
  delimited
  last-locatable @ 1+  first-locatable @
  default-first-locatable @  first-locatable !

  ?do 0 i line>string 2over contains
      if 2drop i unloop exit then break-key? #-28 ?throw
      i unlocated loop 2drop 0 ;

  \ Note:
  \ Error #-32 is "invalid name argument".
  \ Error #-28 is "user interrupt".

  \ doc{
  \
  \ 1-line-(located ( ca len -- block | 0 ) "one-line-paren-located"
  \
  \ Locate the first block whose single-line header contains
  \ the string _ca len_ (surrounded by spaces), and return its
  \ number. If not found, return zero.  The search is
  \ case-sensitive.
  \
  \ Only the blocks delimited by `first-locatable` and
  \ `last-locatable` are searched.
  \
  \ `1-line-(located` is an alternative, deprecated action of
  \ `(located`.
  \
  \ }doc

-->

( ... )

: multiline-(located ( ca len -- block | false )

  \ ----------------------------
  \ 2dup type space
  \ cr 2dup type space .s
  \ cr 2dup type get-current dup u. 0= if quit then
  \ cr 2dup type space .s depth 2 > if key drop then
  \ cr 2dup type space rp@ rp0 @ - -2 / . np@ u. cr .s
  \ cr 2dup type space rp@ rp0 @ - -2 / . .s
  \ cr 2dup type space rp@ rp0 @ - -2 / . .s .info
  \ cr ." (located<" 2dup type ." >"
  \ cr ."  base: #" base @ dup decimal . base !
  \ cr ."  rdepth: " rp@ rp0 @ - -2 / .
  \ cr ."  unused-stringer: " unused-stringer .
  \ ----------------------------
    \ XXX INFORMER -- options for debugging

  \ home 2dup ." needed: " type space
  \ cr ." latest: " latest .name space
    \ XXX INFORMER

  ?dup 0= #-32 ?throw
  nest-source last-locatable @ 1+  first-locatable @
              default-first-locatable @  first-locatable !
  ?do
      \ 0 2 at-xy i . \ XXX INFORMER
      \ depth .  \ XXX INFORMER
      \ rp@ rp0 @ - [ cell negate ] literal / .  \ XXX INFORMER
      2dup i (in-block-header?
      if   2drop i unloop unnest-source exit
      then break-key? #-28 ?throw
      i unlocated loop 2drop 0 unnest-source ;
  \ Note:
  \ Error #-32 is "invalid name argument".
  \ Error #-28 is "user interrupt".

  \ doc{
  \
  \ multiline-(located ( ca len -- block | 0 ) "multiline-paren-located"
  \
  \ Locate the first block whose multiline header
  \ contains the string _ca len_ (surrounded by spaces), and
  \ return its number. If not found, return zero.  The search
  \ is case-sensitive.
  \
  \ Only the blocks delimited by `first-locatable` and
  \ `last-locatable` are searched.
  \
  \ `multiline-(located` is the default action of `(located`.
  \
  \ }doc

defer (located ' 1-line-(located ' (located defer!  -->

  \ doc{
  \
  \ (located ( ca len -- block | 0 ) "paren-located"
  \
  \ Locate the first block whose header contains the string _ca
  \ len_ (surrounded by spaces), and return its number. If not
  \ found, return zero.  The search is case-sensitive.
  \
  \ Only the blocks delimited by `first-locatable` and
  \ `last-locatable` are searched.
  \
  \ ``(located`` is a deferred word. Its default value is
  \ `multiline-(located`, which is under development; its
  \ alternative old value is `1-line-(located`.
  \
  \ ``(located`` is the default action of `located`, which is
  \ changed by `use-fly-index`.
  \
  \ See: `default-first-locatable`.
  \
  \ }doc

( located ?located reneeded reneed needed-word unneeding )

defer located ( ca len -- block | 0 )

  \ doc{
  \
  \ located ( ca len -- block | 0 )
  \
  \ Locate the first block whose header contains the string _ca
  \ len_ (surrounded by spaces), and return its number. If not
  \ found, return zero.  The search is case-sensitive.
  \
  \ Only the blocks delimited by `first-locatable` and
  \ `last-locatable` are searched`.
  \
  \ This is a deferred word whose default action is
  \ `(located`.
  \
  \ See: `need-from`.
  \
  \ }doc

create needed-word 2 cells allot  0. needed-word 2!

  \ doc{
  \
  \ needed-word ( -- a )
  \
  \ A `2variable`. _a_ is the address of a double-cell
  \ containing the address and length of the string containing
  \ the word currently needed by `need` and friends.
  \
  \ }doc

: ?located ( n -- ) \ cr ." ?located " dup .
                      \ XXX INFORMER
  ?exit needed-word 2@ parsed-name 2! #-268 throw ;

  \ XXX FIXME -- When multiline block headers are active,
  \ command `need zxzx` (i.e. any unexistent word) makes the
  \ system freeze after this `throw`, while `need zxzx ` (note
  \ the trailing space) does not. This problem has to do with
  \ un/nesting the sources.

  \ doc{
  \
  \ ?located ( n -- ) "question-located"
  \
  \ If _n_ is zero, store `needed-word` into `parsed-name` (in
  \ order to make `needed-word` displayed) and `throw` an
  \ exception #-268 ("needed, but not located").  Otherwise do
  \ nothing.
  \
  \ }doc

defer reneeded ( ca len -- )

  \ doc{
  \
  \ reneeded ( ca len -- )
  \
  \ Load the first block whose header contains the string _ca
  \ len_ (surrounded by spaces).  If not found, `throw` an
  \ exception #-268 ("needed, but not located").
  \
  \ This is a deferred word whose default action is
  \ `locate-reneeded`.
  \
  \ See: `make-thru-index`.
  \
  \ }doc

: locate-reneeded ( ca len -- ) located dup ?located load ;

  \ doc{
  \
  \ locate-reneeded ( ca len -- )
  \
  \ Locate the first block whose header contains the string _ca
  \ len_ (surrounded by spaces), and `load` it. If not found,
  \ `throw` an exception #-268 ("needed, but not located").
  \
  \ This is the default action of the deferred word
  \ `reneeded`.
  \
  \ See: `make-thru-index`.
  \
  \ }doc

defer reneed ( "name" -- )  defer needed ( ca len -- )

  \ doc{
  \
  \ reneed ( "name" -- )
  \
  \ Load the first block whose header contains _name_
  \ (surrounded by spaces).
  \
  \ This is a deferred word whose default action is
  \ `locate-reneed`.
  \
  \ See: `make-thru-index`.
  \
  \ }doc

  \ doc{
  \
  \ needed ( ca len -- )
  \
  \ If the string _ca len_ is not the name of a word found in
  \ the current search order, load the first block where _ca
  \ len_ is included in the block header (surrounded by
  \ spaces).  If not found, `throw` an exception #-268 ("needed,
  \ but not located").
  \
  \ This is a deferred word whose default action is
  \ `locate-needed`.
  \
  \ See: `make-thru-index`.
  \
  \ }doc

: locate-reneed ( "name" -- )
  parse-name >stringer locate-reneeded ;

  \ doc{
  \
  \ locate-reneed ( "name" -- )
  \
  \ Locate the first block whose header contains _name_
  \ (surrounded by spaces), and load it.  If not found, `throw`
  \ an exception #-268 ("needed, but not located").
  \
  \ This is the default action of the deferred word `reneed`.
  \
  \ See: `make-thru-index`.
  \
  \ }doc

: unneeding ( "name" -- f )
  parse-name needed-word 2@ 2dup or
  if compare 0<> exit then 2drop 2drop false ;

  \ doc{
  \
  \ unneeding ( "name" -- f )
  \
  \ Parse _name_.  If there's no unresolved `need`, `needed`,
  \ `reneed` or `reneeded`, return _false_.  Otherwise, if _name_
  \ is the needed word specified by the last execution of
  \ `need` or `needed`, return _false_, else return _true_.
  \
  \ See: `needing`.
  \
  \ }doc

: new-needed-word ( ca len -- ca' len' )
  -trailing -leading >stringer 2dup needed-word 2! ;  -->

  \ doc{
  \
  \ new-needed-word ( ca1 len -- ca2 len' )
  \
  \ Remove trailing and leading spaces from the word _ca1 len_,
  \ which is the parameter of the latest `need` `needed`,
  \ `reneed` or `reneeded`, store it in the `stringer`
  \ and return it as _ca2 len'_ for further processing.
  \
  \ }doc

( locate-needed need )

: locate-needed ( ca len -- )
  \ cr ." LN<" 2dup type ." >"  \ XXX INFORMER
  \ rp@ rp0 @ - -2 / ." rdepth=" .  \ XXX INFORMER
  needed-word 2@ 2>r new-needed-word  2dup undefined?
  if locate-reneeded else 2drop then 2r> needed-word 2! ;

  \ doc{
  \
  \ locate-needed ( ca len -- )
  \
  \ If the string _ca len_ is not the name of a word found in
  \ the current search order, locate the first block where _ca
  \ len_ is included in the block header (surrounded by
  \ spaces), and load it.  If not found, `throw` an exception
  \ #-268 ("needed, but not located").
  \
  \ This is the default action of the deferred word `needed`.
  \
  \ See: `make-thru-index`.
  \
  \ }doc

defer need ( "name" -- )

  \ doc{
  \
  \ need ( "name" -- )
  \
  \ If _name_ is not found in the current search order, locate
  \ the first block where _name_ is included is the block
  \ header (surrounded by spaces), and load it.  If not found,
  \ `throw` an exception #-268 ("needed, but not located").
  \
  \ This is a deferred word whose default action is
  \ `locate-need`.
  \
  \ See: `make-thru-index`.
  \
  \ }doc

: locate-need ( "name" -- ) parse-name locate-needed ;

  \ doc{
  \
  \ locate-need ( "name" -- )
  \
  \ If _name_ is not found in the current search order, locate
  \ the first block where _name_ is included is the block
  \ header (surrounded by spaces), and load it.  If not found,
  \ `throw` an exception #-268 ("needed, but not located").
  \
  \ This is the default action of the deferred word `need`.
  \
  \ See: `make-thru-index`.
  \
  \ }doc

' locate-reneeded ' reneeded    defer!
' locate-reneed   ' reneed      defer!
' locate-need     ' need        defer!
' locate-needed   ' needed      defer!
  \ Set the default action of `need`, `needed`, `reneed`,
  \ and `reneeded`: Use `locate` to search the blocks.
  \
  \ Note: This initialization is done also by
  \ `use-default-need`, which is an optional word.

' (located        ' located    defer!
' drop             ' unlocated  defer!
  \ Set the default action of `located` and `unlocated`:
  \ Search the blocks.
  \
  \ Note: This initialization is done also by
  \ `use-default-located`, which is an optional word.

blk @ 1+ dup default-first-locatable !  first-locatable !
  \ Set the default first block searched by `(locate`.

( use-default-need use-default-located use-no-index )

unneeding use-default-need ?(

: use-default-need ( -- )
  ['] locate-reneeded ['] reneeded  defer!
  ['] locate-reneed   ['] reneed    defer!
  ['] locate-need     ['] need      defer!
  ['] locate-needed   ['] needed    defer! ; ?)

  \ doc{
  \
  \ use-default-need ( -- )
  \
  \ Set the default actions of `need`, `needed`, `reneed`, and
  \ `reneeded`: Use `locate` to search the blocks.
  \
  \ ``use-default-need`` is a common factor of `use-no-index`
  \ and `use-fly-index`.
  \
  \ }doc

unneeding use-default-located ?(

: use-default-located ( -- ) ['] (located ['] located defer!
                             ['] drop ['] unlocated defer! ; ?)

  \ doc{
  \
  \ use-default-located ( -- )
  \
  \ Set the default actions of `located` and `unlocated`:
  \ Search the blocks.
  \
  \ ``use-default-located`` is a common factor of
  \ `use-no-index` and `use-thru-index`.
  \
  \ }doc

unneeding use-no-index ?(

need use-default-need need  use-default-located

: use-no-index ( -- )
  use-default-need use-default-located ; ?)

  \ doc{
  \
  \ use-no-index ( -- )
  \
  \ Set the default action of `need`, `needed`, `reneed`,
  \ `reneeded` and `unlocated`: Use `locate` to search the
  \ blocks.
  \
  \ The alternative actions are set by `use-thru-index` and
  \ `use-fly-index`.
  \
  \ See: `use-default-need`, `use-default-located`.
  \
  \ }doc

( needing locate need-from need-here )

unneeding needing ?\ : needing ( "name" -- f ) unneeding 0= ;

  \ doc{
  \
  \ needing ( "name" -- f )
  \
  \ Parse _name_.  If there's no unresolved `need`, `needed`,
  \ `reneed` or `reneeded`, return _true_.  Otherwise, if _name_
  \ is the needed word specified by the last execution of
  \ `need` or `needed`, return _true_, else return _false_.
  \
  \ See: `unneeding`.
  \
  \ }doc

unneeding locate ?(

: locate ( "name" -- block | false )
  parse-name >stringer located ; ?)

  \ doc{
  \
  \ locate ( "name" -- block | false )
  \
  \ Locate the first block whose header contains _name_
  \ (surrounded by spaces), and return its number _block_. If
  \ not found, return _false_.  The search is case-sensitive.
  \
  \ Only the blocks delimited by `first-locatable` and
  \ `last-locatable` are searched.
  \
  \ See: `located`.
  \
  \ }doc

unneeding need-from ?( need locate

: need-from ( "name" -- )
  locate dup ?located first-locatable ! ; ?)

  \ doc{
  \
  \ need-from ( "name" -- )
  \
  \ Locate the first block whose header contains _name_
  \ (surrounded by spaces), and set it the first one `located`
  \ will search from. If not found, `throw` an exception #-268
  \ ("needed, but not located").
  \
  \ ``need-from`` is intended to prevent undesired name clashes
  \ during the execution of `need` and related words. _name_ is
  \ supposed to be a conventional marker.
  \
  \ Usage example:

  \ ----
  \ ( x )
  \
  \ : x ( -- ) ." Wrong x!" ;
  \
  \ ( use-x )
  \
  \ need-from ==data-structures== need x
  \
  \ x
  \
  \ ( y ==data-structures== )
  \
  \ : y ." Y data structure; ;
  \
  \ ( x )
  \
  \ : x ." X data structure; ;
  \
  \ ----

  \ }doc

unneeding need-here ?(

: need-here ( "name" -- )
  parse-name needed-word 2@ 2>r
  new-needed-word  2dup needed-word 2! undefined?
  if blk @ load else 2drop then 2r> needed-word 2! ; ?)

  \ doc{
  \
  \ need-here ( "name" -- )
  \
  \ If _name_ is not a word found in the current search order,
  \ load the current block.
  \
  \ This is a faster alternative to `need`, when the needed
  \ word is in the same block, and conditional compilation is
  \ used with `?\`, `?(` or `[if]`.
  \
  \ }doc

  \ ( (.info checkpoint )

  \   \ XXX TMP -- 2017-02-12, for debugging

  \ need get-drive

  \ : (.info ( -- ) get-drive dup ." Drive " .
  \                 1 = if ." CHANGED!" quit then ;

  \ ' (.info ' .info defer!

  \ : checkpoint ( n -- )
  \   2 border cr ." Check point " . (.info key drop
  \   0 border ;

  \ ===========================================================
  \ Change log

  \ 2015-06: First version, partly based on code from Afera.
  \
  \ 2015-06-25: Fix: `require` and `locate` needed to save the
  \ parsed words to the circular string buffer.
  \
  \ 2015-09-13: Rename `require` to `need`, and all related
  \ words accordingly.  The reason is `require` and `required`
  \ are standard words (in Forth-94 and Forth-2012), and should
  \ not be used for different purposes.
  \
  \ 2015-10-05: Fix `needed`. The trailing and leading spaces
  \ of the string, sometimes used to prevent name clashes, had
  \ to be removed before `undefined?`. `-leading` has to be
  \ moved to the kernel.
  \
  \ 2015-10-16: Add `[needed]`.  It allows selective
  \ compilation depending on the word specified by `need` or
  \ `needed`.  Improve `located`: now the string searched for
  \ is delimited with spaces. This prevents name clashes and
  \ makes it unnecessary to add the spaces explicitly in risky
  \ cases.
  \
  \ 2015-10-25: Improve `need` and `needed` a bit.
  \
  \ 2016-04-02: Factor `new-needed-word` from `needed`.  This
  \ change was needed for `indexer`.
  \
  \ 2016-04-03: Make `need` and related words deferred. Factor
  \ `new-needed-word` from `needed`. These changes were needed
  \ for `indexer`.
  \
  \ 2016-04-26: Improve `located`: when the user press the
  \ break key, throw exception #-28 ("user interrupt");
  \ formerly the ordinary #-268 ("required, but not located")
  \ was thrown by the calling word.
  \
  \ 2016-05-06: Make `from` and `locate` optional. Compact the
  \ blocks.
  \
  \ 2016-05-07: Improve documentation. Fix the word shown when
  \ `?locate` throws an error.
  \
  \ 2016-05-10: Fix a harmless bug: `locate-reneed` called the
  \ deferred `reneeded` instead of `locate-reneeded`.
  \
  \ 2016-05-31: Fix block header.
  \
  \ 2016-06-01: Fix `need-here`, which left the string on the
  \ stack.
  \
  \ 2016-10-28: Add a temporary debugging informer to
  \ `located`.
  \
  \ 2016-11-19: Add `unlocated` in order to implement
  \ `fly-indexer`. Improve the documentation.
  \
  \ 2016-11-21: Document why `contains` is not accessible here,
  \ and therefore it's defined also in the strings module.
  \
  \ 2016-11-24: Rename some words to be consistent with the
  \ changes in the Thru Index and the Fly Index.  Factor
  \ `use-no-index`. Improve documentation.
  \
  \ 2016-11-25: Factor `use-no-index`, `use-default-located`.
  \ Fix and improve documentation and names.
  \
  \ 2016-11-26: Improve `(located)` to detect empty strings.
  \
  \ 2016-12-03: Add debugging code.
  \
  \ 2016-12-25: Rename `from` to `need-from` and improve its
  \ documentation.
  \
  \ 2016-12-30: Compact the code, saving one block.
  \
  \ 2017-01-05: Update `indexer` to `make-thru-index` in
  \ documentation.
  \
  \ 2017-01-17: Improve documentation.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-19: Improve optional debugging code in `(located)`.
  \
  \ 2017-01-23: Increase and tidy optional debugging code in
  \ `(located)`.
  \
  \ 2017-02-09: Fix documentation.
  \
  \ 2017-02-12: Add more optional debugging code for
  \ `(located)`.
  \
  \ 2017-02-17: Update notation "behaviour" to "action". Update
  \ cross references.
  \
  \ 2017-02-19: Replace `do`, which has been moved to the
  \ library, with `?do`. Fix markup typo in documentation.a
  \
  \ 2017-02-21: Make `use-default-need`, `use-default-located`
  \ and `use-no-index` optional. They are needed only when a
  \ blocks indexer is used.  This change saves 102 bytes from
  \ the `need` tool. Now a copy of the code of
  \ `use-default-need` and `use-default-located` is executed
  \ during the loading of `need`.
  \
  \ 2017-02-25: Update the name of the circular string buffer,
  \ after the changes in the kernel.
  \
  \ 2017-02-26: Update "hp" notation to "np", after the changes
  \ in the kernel.
  \
  \ 2017-02-27: Improve documentation.
  \
  \ 2017-02-28: Fix typos in documentation.
  \
  \ 2017-03-12: Update the names of `stringer` words, and
  \ mentions to it.
  \
  \ 2017-03-13: Update names including "rec" to "sector(s)";
  \ update names including "blk" to "block(s)".  Improve
  \ documentation.
  \
  \ 2017-04-16: Improve documentation.
  \
  \ 2017-12-09: Improve documentation of variables.
  \
  \ 2018-01-21: Fix layout of one-liner.
  \
  \ 2018-01-24: Improve error checking in `(locate)`.
  \
  \ 2018-02-14: Comment out `(.info` and `checkpoint`, saving
  \ one block.
  \
  \ 2018-03-01: Make `[needed]` optional. Improve
  \ documentation.
  \
  \ 2018-03-05: Rename `[unneeded]` `unneeding`; rename
  \ `[needed]` `needing`; make both words non-immediate.
  \
  \ 2018-03-07: Add words' pronunciaton.
  \
  \ 2018-04-09: Update source style (remove double spaces).
  \
  \ 2018-04-26: Make `?located` consume its argument (its stack
  \ comment was wrong, anyway). Improve documentation.
  \
  \ 2018-06-02: Draft an alternative `(located)` in order to
  \ support multiline block headers. Fix needing of
  \ `use-no-index`.  Document `needed-word`.
  \
  \ 2018-06-03: Make `(located)` deferred, for testing. Fix,
  \ rewrite and factor `in-block-header?`.
  \
  \ 2018-06-04: Update: remove trailing closing paren from word
  \ names. Fix typo in documentation. Link `variable` and
  \ `2variable` in documentation.
  \
  \ 2018-06-16: Fix typos in documentation and comments.
  \
  \ 2018-07-21: Improve documentation, linking `throw`.
  \
  \ 2018-09-24: Fix needing of `needing`.
  \
  \ 2020-05-19: Update: `2variable` has been moved to the
  \ library; `create` is used instead.
  \
  \ 2020-05-24: Fix typo.

  \ vim: filetype=soloforth
