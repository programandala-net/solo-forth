  \ display.mode.32.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modifed: 201806041324
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ The default 32-cpl display mode.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( mode-32 )

need columns need rows need set-font need set-mode-output
need >form need rom-font

variable mode-32-font  rom-font bl 8 * + mode-32-font !

  \ doc{
  \
  \ mode-32-font ( -- a )
  \
  \ A `variable`. _a_ is the address of a cell containing the
  \ address of the font used by `mode-32`. Note the address of
  \ the font must be the address of its character 32 (space).
  \
  \ The default value of ``mode-32-font`` is `rom-font` plus
  \ 256 (the address of the space character in the ROM font).
  \
  \ }doc

: mode-32 ( -- )
  [ latestxt ] literal current-mode !
  mode-32-font @ 256 - set-font
  $09F4 set-mode-output
  ['] mode-32-emit  ['] emit  defer!
  ['] mode-32-at-xy ['] at-xy defer! 32 24 >form
  ['] mode-32-xy    ['] xy    defer! ;

  \ doc{
  \
  \ mode-32 ( -- )
  \
  \ Set the default 32-cpl display mode. Usually this is not
  \ needed by the application, except when any other mode is
  \ used, e.g.  `mode-32iso`, `mode-42` or `mode-64`.
  \
  \ When any other mode is loaded, ``mode-32`` is automatically
  \ loaded and made the default display mode (therefore
  \ restored by `restore-mode`, which is called by `warm` and
  \ `cold`).
  \
  \ See: `current-mode`, `set-font`, `set-mode-output`,
  \ `columns`, `rows`, `mode32-emit`, `mode-32-xy`,
  \ `mode-32-at-xy`, `>form`.
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
  \
  \ 2017-05-15: Use `>form` for mode transition. Add
  \ `mode-32-font`.
  \
  \ 2018-06-04: Link `variable` in documentation.

  \ vim: filetype=soloforth
