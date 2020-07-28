  \ multitask.gplusdos.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202007282031

  \ ===========================================================
  \ Description

  \ The Jiffy tool for multitasking on G+DOS.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( jiffy! jiffy@ -jiffy )

  \ Note: This code is specific for G+DOS.

  \ Credit:
  \
  \ Idea inspired by an article by Paul King, published on
  \ Format (volume 2, number 3, 1988-10).
  \
  \ XXX TODO link to the WoS archive ftp, when available

unneeding -jiffy ?( need !dosvar

: jiffy! ( a -- ) 16 !dosvar ; ?)

  \ doc{
  \
  \ jiffy! ( a -- ) "jiffy-store"
  \
  \ Set the address _a_ of the so called "jiffy call", a Z80
  \ routine to be called by G+DOS after the OS interrupts
  \ routine, every 50th of a second.
  \
  \ See also: `jiffy@`, `-jiffy`.
  \
  \ }doc

unneeding -jiffy ?( need @dosvar

: jiffy@ ( -- a ) 16 @dosvar ; ?)

  \ doc{
  \
  \ jiffy@ ( -- a ) "jiffy-fetch"
  \
  \ Get the address _a_ of the so called "jiffy call", a Z80
  \ routine that is called by G+DOS after the OS interrupts
  \ routine, every 50th of a second.
  \
  \ See also: `jiffy!`, `-jiffy`.
  \
  \ }doc

unneeding -jiffy ?( need jiffy!

: -jiffy ( -- ) 8335 jiffy! ; ?)

  \ doc{
  \
  \ -jiffy ( -- ) "minus-jiffy"
  \
  \ Deactivate the so called "jiffy call", the Z80 routine that
  \ is called by G+DOS after the OS interrupts routine (every
  \ 50th of a second), by setting its default value (a noop
  \ routine in the RAM of the Plus D interface).
  \
  \ See also: `jiffy!`, `jiffy@`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2017-05-07: Document the words.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-07: Add words' pronunciaton.

  \ vim: filetype=soloforth
