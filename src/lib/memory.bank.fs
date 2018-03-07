  \ memory.bank.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803072228
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words related to memory banks.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( /bank bank-start )

unneeding /bank ?\ $4000 constant /bank

  \ doc{
  \
  \ /bank ( -- n ) "slash-bank"
  \
  \ _n_ is the size in bytes of a memory bank: $4000.
  \
  \ See: `bank-start`.
  \
  \ }doc

unneeding bank-start ?\ $C000 constant bank-start

  \ doc{
  \
  \ bank-start ( -- a )
  \
  \ _a_ is the memory address where banks are paged in: $C000.
  \
  \ See: `/bank`, `bank`, `banks`, `far-banks`,
  \ `default-bank`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

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
  \
  \ 2017-05-11: Move `/bank` and `bank-start` from the kernel.
  \
  \ 2018-01-23: Remove `get-default-bank` and
  \ `set-default-bank`.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-07: Add words' pronunciaton.

  \ vim: filetype=soloforth
