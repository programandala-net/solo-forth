  \ memory.bank.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702220020

  \ -----------------------------------------------------------
  \ Description

  \ Words related to memory banks.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ History

  \ 2016-04-17: Move `!s` and `!cs` from the kernel.
  \
  \ 2016-04-24: Move `get-default-bank` and `set-default-bank`
  \ from the kernel.
  \
  \ 2016-10-24: Add `extra-memory` and related words to manage
  \ a virtual 64-KiB space using 4 memory banks.
  \
  \ 2016-10-25: Improve comment.
  \
  \ 2016-10-26: Remove `extra-memory` and `a>e`, which have
  \ been moved to the kernel.
  \
  \ 2016-11-13: Remove far-memory fetch and store words, which
  \ are already in the kernel.
  \
  \ 2017-01-05: Remove old system bank words `!s` and `c!s`.
  \
  \ 2017-02-21: Improve documentation.

( get-default-bank set-default-bank )


: get-default-bank ( -- +n ) default-bank# c@ ;

  \ doc{
  \
  \ get-default-bank ( -- +n )
  \
  \ Get the current default bank _+n_ by fetching the contents
  \ of `default-bank#`.
  \
  \ See also: `set-default-bank`, `default-bank`, `bank`,
  \ `banks`.
  \
  \ }doc

: set-default-bank ( +n -- ) default-bank# c! ;

  \ doc{
  \
  \ set-default-bank ( +n -- )
  \
  \ Set _+n_ as the default memory bank by modifyng the
  \ contents of `default-bank#`.
  \
  \ See also: `get-default-bank`, `default-bank`, `bank`,
  \ `banks`.
  \
  \ }doc

  \ vim: filetype=soloforth
