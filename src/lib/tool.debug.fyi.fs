  \ tool.debug.fyi.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201705071838
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ `fyi` prints information about the currest status of the
  \ Forth system.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( fyi )

need u.r

: fyi. ( u -- ) cr 5 u.r space ;

: fyi ( -- )
  #words            fyi. ." #words"
  here              fyi. ." here"
  latest-wordlist @ fyi. ." latest-wordlist @"
  limit @           fyi. ." limit @"
  unused            fyi. ." unused"
  np@               fyi. ." np@"
  latest            fyi. ." latest"
  current-latest    fyi. ." current-latest"
  farlimit @        fyi. ." farlimit @"
  farunused         fyi. ." farunused" cr ;

  \ doc{
  \
  \ fyi ( -- )
  \
  \ Display information about the current status of the Forth
  \ system.
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

  \ vim: filetype=soloforth

