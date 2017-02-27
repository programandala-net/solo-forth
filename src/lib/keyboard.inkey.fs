  \ keyboard.inkey.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702220020

  \ -----------------------------------------------------------
  \ Description

  \ `inkey`.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ Latest changes

  \ 2016-12-25: Improve documentation. Convert from `z80-asm`
  \ to `z80-asm,` assembler.
  \
  \ 2017-01-05: Update `need z80-asm,` to `need assembler`.
  \
  \ 2017-02-17: Update cross references.

( inkey )

need assembler

code inkey ( -- c | 0 )

  a xor,
  01 iy 5 bitx,  \ a new key pressed?
  nz? rif
    5C08 h ldp#,  \ LAST-K system variable
    m a ld,
    \ 0 m ld#, \ XXX OLD
    01 iy 5 resx,
  rthen
  pusha jp, end-code

  \ doc{
  \
  \ inkey ( -- 0 | c )
  \
  \ Leave the value of the key being pressed. If no key being
  \ pressed, leave 0.
  \
  \ This word works only when an interrupts routine reads the
  \ keyboard and updates the related system variables.
  \
  \ See also: `get-inkey`, `key`.
  \
  \ }doc

  \ XXX FIXME -- Some times this word returns zero when the key
  \ is pressed; `get-inkey` works fine.  Anyway this version is
  \ smaller and can be useful.

  \ vim: filetype=soloforth
