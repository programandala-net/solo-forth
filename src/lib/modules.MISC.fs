  \ modules.MISC.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803072250
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Implementation of simple unnamed modules.
  \
  \ Modules hide the internal implementation and leave visible
  \ the words of the outer interface.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( seclusion isolate )

unneeding seclusion ?(

: seclusion ( -- wid1 wid2 )
  get-current wordlist dup >order dup set-current ;

  \ doc{
  \
  \ seclusion ( -- wid1 wid2 )
  \
  \ Start a seclusion module.  Private definitions follow.
  \
  \ Modules hide the internal implementation and leave visible
  \ the words of the outer interface.
  \
  \ _wid1_ is the identifier of the compilation word list
  \ before ``seclusion`` was executed. _wid2_ is the identifier
  \ of the word list where private definitions of the seclusion
  \ module will be created.  They are used by `-seclusion`,
  \ which marks the start of public definitions, `+seclusion`,
  \ which optionally marks the start of new private
  \ definitions, and `end-seclusion`, which ends the module.
  \
  \ Usage example:

  \ ----
  \ seclusion
  \   \ Inner/private words.
  \ -seclusion
  \   \ Interface/public words.
  \ +seclusion
  \   \ More inner/private words.
  \ -seclusion
  \   \ More interface/public words.
  \   \ Etc.
  \ end-seclusion
  \ ----

  \ A copy of _wid2_ may be kept by the application in order to
  \ access private words later, e.g.  ``seclusion dup constant
  \ my-module`.
  \
  \ See: `internal`, `isolate`, `module`, `package`,
  \ `privatize`.
  \
  \ }doc

: -seclusion ( wid1 wid2 -- wid1 wid2 ) over set-current ;

  \ doc{
  \
  \ -seclusion ( wid1 wid2 -- wid1 wid2 ) "minus-seclusion"
  \
  \ Start the public definitions of a `seclusion` module.
  \
  \ See: `+seclusion`, `end-seclusion`.
  \
  \ }doc

: +seclusion ( wid1 wid2 -- wid1 wid2 ) dup set-current ;

  \ doc{
  \
  \ +seclusion ( wid1 wid2 -- wid1 wid2 ) "plus-seclusion"
  \
  \ Start more private definitions of a `seclusion` module.
  \
  \ See: `-seclusion`, `end-seclusion`.
  \
  \ }doc

: end-seclusion ( wid1 wid2 -- ) -seclusion 2drop previous ; ?)

  \ doc{
  \
  \ end-seclusion ( wid1 wid2 -- )
  \
  \ End a `seclusion` module.
  \
  \ See: `-seclusion`, `+seclusion`.
  \
  \ }doc

unneeding isolate

?\ : isolate ( -- ) wordlist >order definitions ;

  \ doc{
  \
  \ isolate ( -- )
  \
  \ Create a word list, push it on the search order and set it
  \ as the compilation word list.
  \
  \ ``isolate`` is the simplest way to create a module.  Usage
  \ example:

  \ ----
  \ get-current isolate
  \   \ Inner words.
  \ set-current
  \   \ Interface words.
  \ previous
  \ ----

  \ See: `internal`, `module`, `package`, `privatize`,
  \ `seclusion`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2017-03-14: Start. Write `seclusion` and `isolate`.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-07: Add words' pronunciaton.

  \ vim: filetype=soloforth

