  \ tool.debug.fyi.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202007282031
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ `fyi` prints information about the currest status of the
  \ Forth system.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018,
  \ 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( fyi )

need u.r

: fyi. ( u -- ) cr 5 u.r space ;

: fyi ( -- )
  #words          fyi. ." #words"
  here            fyi. ." here"
  last-wordlist @ fyi. ." last-wordlist @"
  limit @         fyi. ." limit @"
  unused          fyi. ." unused"
  np@             fyi. ." np@"
  latest          fyi. ." latest"
  current-latest  fyi. ." current-latest"
  farlimit @      fyi. ." farlimit @"
  farunused       fyi. ." farunused" cr ;

  \ doc{
  \
  \ fyi ( -- ) "f-y-i"
  \
  \ Display information about the current status of the Forth
  \ system.
  \
  \ See also: `#words`, `here`, `last-wordlist`, `limit`,
  \ `unused`. `np@`, `latest`, `current-latest`, `farlimit`,
  \ `farunused`, `greeting`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-12-27: Start. Add `fyi`.
  \
  \ 2017-01-06: Update `voc-link` to `latest-wordlist`.
  \
  \ 2017-01-20: Fix. Add `#words` to `fyi`.
  \
  \ 2017-02-26: Update "hp" notation to "np", after the changes
  \ in the kernel.
  \
  \ 2017-05-07: Improve documentation.
  \
  \ 2018-03-09: Add words' pronunciaton. Improve documentation.
  \
  \ 2018-04-12: Fix links in documentation.
  \
  \ 2020-06-08: Update: rename `latest-wordlist` to
  \ `last-wordlist`.

  \ vim: filetype=soloforth

