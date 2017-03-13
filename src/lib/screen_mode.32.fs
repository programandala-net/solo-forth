  \ screen_mode.32.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201703132017
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ The screen mode 32.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( mode32 )

need columns need rows need set-font need set-mode-output

: mode32 ( -- )
  [ latestxt ] literal current-mode !
  15360 set-font  2548 set-mode-output
  32 to columns  24 to rows
  ['] mode32-xy ['] xy defer!
  ['] mode32-at-xy ['] at-xy defer! ;

  \ doc{
  \
  \ mode32 ( -- )
  \
  \ Set the default printing mode: the 32 cpl ROM routine, the
  \ ROM font, and the special code for `at-xy` (required to use
  \ the whole screen).
  \
  \ ``mode32`` is loaded when `mode42` or `mode64` are loaded,
  \ in order to make it the default mode.
  \
  \ }doc

' mode32 ' default-mode defer!

  \ ===========================================================
  \ Change log

  \ 2016-04-26: Change `latest name>` to `latestxt`.
  \
  \ 2016-05-07: Improve documentation.
  \
  \ 2017-01-20: Fix typo.
  \
  \ 2017-03-13: Improve documentation.

  \ vim: filetype=soloforth
