  \ keyboard.get-key-question.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702220020
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ `get-key?` and `fast-get-key?`.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( get-key? )


need assembler need unresolved

code get-key? ( -- f )

  b push,
  028E call,  \ ROM KEY_SCAN
  here nz? ?jr,  >rmark 0 unresolved ! \ to return_false
  031E call,  \ ROM KEY_TEST
  here nc? ?jr,  >rmark 1 unresolved ! \ to return_false

  \ return_true:
  b pop, ' true jp,

  \ return_false:
  0 unresolved @ >rresolve
  1 unresolved @ >rresolve
  b pop, ' false jp,

  end-code

  \ doc{
  \
  \ get-key? ( -- f )
  \
  \ An alternative to `key?`. It works also when the system
  \ interrupts are off. Variant with relative jumps.
  \
  \ See: `key?`, `fast-get-key?`.
  \
  \ }doc

( fast-get-key? )

need assembler need unresolved

code fast-get-key? ( -- f )

  b push,
  028E call,  \ ROM KEY_SCAN
  0000 nz? ?jp,  |mark 0 unresolved ! \ to return_false
  031E call,  \ ROM KEY_TEST
  0000 nc? ?jp,  |mark 1 unresolved ! \ to return_false

  \ return_true:
  b pop, ' true jp,

  \ return_false:
  0 unresolved @ >resolve
  1 unresolved @ >resolve
  b pop, ' false jp,

  \ doc{
  \
  \ fast-get-key? ( -- f )
  \
  \ An alternative to `key?`. It works also when the system
  \ interrupts are off. Faster variant with absolute jumps.
  \
  \ See: `get-key?`.
  \
  \ }doc

  end-code

  \ ===========================================================
  \ Change log

  \ 2017-01-02: Convert `get-key?` and `fast-get-key?` from
  \ `z80-asm` to `z80-asm,`. Improve documentation.
  \
  \ 2017-01-05: Update `need z80-asm,` to `need assembler`.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-02-21: Need `unresolved`, which now is optional, not
  \ part of the assembler.

  \ vim: filetype=soloforth
