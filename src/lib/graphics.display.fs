  \ graphics.display.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201703132006
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words to save and restore the display status, in order to
  \ call ROM routines that prints to the screen.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( nonfull-display full-display save-display restore-display )

: nonfull-display ( -- ) 2 23659 c! ;

  \ XXX TODO -- Rewrite in Z80.

  \ doc{
  \
  \ nonfull-display ( -- )
  \
  \ Set the nonfull screen mode: 2 lines in the lower screen
  \ and 22 lines in the upper main screen, which is the default
  \ configuration in BASIC.
  \
  \ See also: `full-display`, `save-display`.
  \
  \ }doc

  \ Note: 23659 is the system variable DF_SZ (lines in the
  \ lower screen).

: full-display ( -- ) 0 23659 c! ;

  \ XXX TODO -- Rewrite in Z80.

  \ doc{
  \
  \ full-display ( -- )
  \
  \ Set the full screen mode: no lines in the lower screen,
  \ thus 24 lines in the upper main screen, which is the
  \ default configuration in Solo Forth.
  \
  \ See also: `nonfull-display`, `save-display`.
  \
  \ }doc

  \ Note: 23659 is the system variable DF_SZ (lines in the
  \ lower screen).

: save-display ( -- ) ( R: -- col row )
  r> xy 2>r >r save-mode nonfull-display ;

  \ doc{
  \
  \ save-display ( -- ) ( R: -- col row )
  \
  \ Save the status of the display.  ``save-display`` is
  \ intended to be used before calling a ROM routine that uses
  \ the display.  The display can be restored to its previous
  \ status with `restore-display`.
  \
  \ See also: `save-mode`, `nonfull-display`.
  \
  \ }doc

: restore-display ( -- ) ( R: col row -- )
  display full-display restore-mode  r> 2r> at-xy >r ;

  \ doc{
  \
  \ restore-display ( -- ) ( R: col row -- )
  \
  \ Restore the status of the display, saved by `save-display`.
  \ ``restore-display`` is intended to be used after calling a
  \ ROM routine that uses the display.
  \
  \ See also: `display`, `full-display`, `restore-mode`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2017-03-13: Improve documentation.

  \ vim: filetype=soloforth
