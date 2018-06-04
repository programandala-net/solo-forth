  \ flow.doer.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modifed: 201806041324
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Leo Brodie's `doer make` construct.

  \ ===========================================================
  \ Authors

  \ Leo Brodie, published on _Thinking Forth_, Appendix B,
  \ 1984. Public domain.

  \ Marcos Cruz (programandala.net) adapted this version from
  \ PFE, 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( doer )

need >body

: doer-noop ( -- ) ;

  \ doc{
  \
  \ doer-noop ( -- )
  \
  \ Do nothing. ``does-noop`` is an empty colon definition
  \ which is the default action of words created by `doer`.
  \
  \ }doc

unneeding doer ?(

: doer ( "name" -- ) create [ ' doer-noop >body ] literal ,
                     does>  ( -- ) ( dfa ) @ >r ;

  \ doc{
  \
  \ doer ( "name" -- )
  \
  \ Define a word _name_ whose action is configurable.  By
  \ default _name_ executes `doer-noop`, which does nothing.
  \
  \ The action of _name_ can be changed by `make`.
  \
  \ NOTE: ``doer`` is superseded by the standard word `defer`.
  \
  \ }doc

: (make ( -- ) ( R: a1 -- | a4 )
  r> dup cell+ dup cell+ ( a1 a2 a3 )
  swap @ >body !      \ store _a3_ into dfa of _a2_
  @ ?dup if >r then ; \ manage _a1_, continuation after `;and`
  \ Stuff the address of further code into the data field
  \ of a `doer` word. Compiled by `make`.
  \ a1 = address containing the address of an optional
  \      continuation after `;and`, or zero
  \ a2 = address of the `doer` word
  \ a3 = address of the code to associate the `doer` word with
  \ a4 = address of the optional continuation after `;and`

variable >;and ( -- a )
  \ A `variable`. _a_ is the address of a cell containing the
  \ address of the optional continuation of a definition where
  \ `make` is used.  Used by `make` and `;and`.

: make
  \ Interpretation: ( "name" -- )
  \ Compilation:    ( -- )
  compiling? if   postpone (make here >;and ! 0 ,
             else here ' >body ! ] then ; immediate ?)

  \ doc{
  \
  \ make
  \   Interpretation: ( "name" -- )
  \   Compilation:    ( -- )
  \
  \ Interpretation: Parse _name_, which is the name of a word
  \ created by `doer`, and make it execute the colon definition
  \ that follows.
  \
  \ Usage example:

  \ ----
  \ doer flashes
  \ flashes \ does nothing
  \ make flashes 8 0 ?do i border loop ;
  \ flashes \ works
  \ ----

  \ Compilation: Modify the next inline compiled word of the
  \ current definition, which was created by `doer`, and make
  \ it execute the rest of the definition after it.
  \
  \ Usage example:

  \ ----
  \ doer flashes
  \ flashes \ does nothing
  \ : activate ( -- ) make flashes 8 0 ?do i border loop ;
  \ activate
  \ flashes \ works
  \ ----

  \ ``make`` is an `immediate` word.
  \
  \ See: `;and`, `undo`.
  \
  \ }doc

( ;and undo )

unneeding ;and ?( need doer

: ;and ( -- ) postpone exit here >;and @ ! ; immediate ?)

  \ doc{
  \
  \ ;and ( -- ) "colon-and"
  \
  \ Allow continuation of a definition where `make` is used.

  \ ----
  \ doer flashes
  \ cls \ does nothing
  \ : activate ( -- ) make cls page ;and ." cls is ready" ;
  \ activate \ reconfigure ``cls`` and display "cls is ready"
  \ cls \ do ``page``
  \ ----

  \ ``;and`` is an `immediate` word.
  \
  \ See: `undo`.
  \
  \ }doc

unneeding undo ?( need doer

: undo ( "name" -- )
  [ ' doer-noop >body ] literal ' >body ! ; ?)

  \ doc{
  \
  \ undo ( `name`-- )
  \
  \ Parse _name_, which is the name of a word created by
  \ `doer`, and make it do nothing.
  \
  \ See: `make`, `;and`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-05-17: Need `>body`, which has been moved to the
  \ library.
  \
  \ 2016-11-27: Move `doer-test` to the tests module.
  \
  \ 2017-02-17: Update notation "behaviour" to "action".
  \
  \ 2017-09-09: Update notation "pfa" to the standard "dfa".
  \
  \ 2017-12-08: Update code style.  Improve the calculation of
  \ `doer-noop`'s body.  Improve documentation. Make `;and` and
  \ `undo` optional.
  \
  \ 2018-02-05: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-04-15: Fix markup in documentation.
  \
  \ 2018-06-04: Update: remove trailing closing paren from word
  \ names.  Link `variable` in documentation.

  \ vim: filetype=soloforth
