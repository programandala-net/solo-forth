  \ display.mode.32.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201704231343
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ The default 32 CPL display mode.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( mode-32 )

need columns need rows need set-font need set-mode-output

: mode-32 ( -- )
  [ latestxt ] literal current-mode !
  15360 set-font  2548 set-mode-output
  32 to columns  24 to rows
  ['] mode-32-emit  ['] emit  defer!
  ['] mode-32-xy    ['] xy    defer!
  ['] mode-32-at-xy ['] at-xy defer! ;

  \ doc{
  \
  \ mode-32 ( -- )
  \
  \ Set the default 32 CPL display mode. Usually this is not
  \ needed by the application, except when any other mode is
  \ used, e.g.  `mode-32iso`, `mode-42` or `mode-64`.
  \
  \ When any other mode is loaded, ``mode-32`` is automatically
  \ loaded and made the default display mode (therefore
  \ restored by `restore-mode`, which is called by `warm` and
  \ `cold`).
  \
  \ See also: `current-mode`, `set-font`, `set-mode-output`,
  \ `columns`, `rows`, `mode32-emit`, `mode-32-xy`,
  \ `mode-32-at-xy`.
  \
  \ }doc

' mode-32 ' default-mode defer!

  \ ===========================================================
  \ Change log

  \ 2016-04-26: Change `latest name>` to `latestxt`.
  \
  \ 2016-05-07: Improve documentation.
  \
  \ 2017-01-20: Fix typo.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-04-21: Rename module and words after the new
  \ convention for display modes.
  \
  \ 2017-04-23: Improve documentation.

  \ vim: filetype=soloforth
