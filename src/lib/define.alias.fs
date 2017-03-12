  \ define.alias.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702280015
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Implementation of `alias`.  Features of an alias defined with
  \ `alias`:
  \
  \ - It has the execution token of the original word.
  \ - It does not inherit the attributes of the original word,
  \   which can be set with `immediate` and `compile-only`.
  \ - It does not use data space memory.
  \ - It can be reconfigured with `realias` and `alias!`, but
  \   the compiled aliases don't change.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( alias! alias realias )

[unneeded] alias!
?\ need name>>  : alias! ( xt nt -- ) name>> far! ;

  \ doc{
  \
  \ alias! ( xt nt -- )
  \
  \ Set the alias _nt_ to execute _xt_.
  \
  \ See `alias`, `realias`.
  \
  \ }doc

[unneeded] alias dup
?\ need alias!
?\ : alias ( xt "name" -- ) header reveal latest alias! ;

  \ doc{
  \
  \ alias ( xt "name" -- )
  \
  \ Create an alias _name_ that will execute _xt_.
  \
  \ Aliases have the execution token _xt_ of the original word,
  \ but, contrary to synonyms created by `synonym`, don't
  \ inherit its attributes (`immediate` and `compile-only`).
  \
  \ See `realias`, `alias!`, `synonym`.
  \
  \ Origin: Gforth.
  \
  \ }doc

[unneeded] realias ?exit

need alias!

: realias ( xt "name" -- )
  defined dup 0= #-13 ?throw alias! ;

  \ doc{
  \
  \ realias ( xt "name" -- )
  \
  \ Set the alias _name_ to execute _xt_.
  \
  \ See `alias`, `alias!`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-10-25: First version of `alias`: it creates a deferred
  \ word and initializes it. Second version: it recognizes code
  \ words and patches their code field instead.
  \
  \ 2015-12-26: New alternative version, adapted to DTC: if
  \ _xt_ is a deferred word, the alias will point to the word
  \ it's associated to.
  \
  \ 2016-02-27: Fixed the DTC version: the alias of an
  \ unitialized deferred word executed the default error even
  \ after the initialization of the deferred word.
  \
  \ 2016-03-04: Removed the ITC version.
  \
  \ 2016-04-17: Improved `alias`: nowe the aliases have the xt
  \ of the original word.
  \
  \ 2016-04-18: Wrote `realias`.
  \
  \ 2016-04-29: Add `alias!`, a useful common factor of
  \ `alias` and `realias`.
  \
  \ 2016-05-05: Add conditional compilation. Improve
  \ documentation.
  \
  \ 2016-10-28: Adapt `alias!` to the extra-memory system.
  \
  \ 2016-11-13: Update the names of far-memory words.
  \
  \ 2016-11-18: Improve documentation of `alias`.
  \
  \ 2017-01-05: Remove old system bank support from `alias!`.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-02-27: Improve documentation.

  \ vim: filetype=soloforth
