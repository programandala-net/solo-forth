  \ graphics.ocr.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201705091223
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words that recognize characters on the screen.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( ocr )

need assembler need unresolved need >amark

variable ocr-charset  $3D00 ocr-charset !

  \ doc{
  \
  \ ocr-charset ( -- a )
  \
  \ Variable that holds the address of the first printable char
  \ in the charset used by `ocr`. By default it contains
  \ 0x3D00, the address of the space char in the ROM charset.
  \
  \ The configuration of `ocr`, including this variable, can be
  \ changed by `ascii-ocr` and `udg-ocr`.
  \
  \ See also: `ocr-chars`, `ocr-first`.
  \
  \ }doc

here bl c, constant ocr-first

  \ doc{
  \
  \ ocr-first ( -- ca )
  \
  \ A character variable that holds the code of the first
  \ printable char in the charset used by `ocr`, pointed by
  \ `ocr-charset`.  By default it contais `bl`, the code of the
  \ space character.
  \
  \ The configuration of `ocr`, including this variable, can be
  \ changed by `ascii-ocr` and `udg-ocr`.
  \
  \ See also: `ocr-chars`, `ocr-charset`.
  \
  \ }doc

here 127 bl - c, constant ocr-chars

  \ doc{
  \
  \ ocr-chars ( -- ca )
  \
  \ A character variable that holds the number of chars used by
  \ `ocr`, from the address pointed by `ocr-charset`. By
  \ default it contais 95, the number of printable ASCII chars
  \ in the ROM charset.
  \
  \ The configuration of `ocr`, including this variable, can be
  \ changed by `ascii-ocr` and `udg-ocr`.
  \
  \ See also: `ocr-first`, `ocr-charset`.
  \
  \ }doc

code ocr ( col line -- n )

  d pop, h pop, b push,
    \ get row, get col, save the Forth IP
  l b ld, e c ld, ocr-charset fthl,
    \ B=colum, C=row, HL=udg

  c a ld, rrca, rrca, rrca, E0 and#, b xor, a e ld,
  c a ld, 18 and#, 40 xor#, a d ld,
    \ DE = screen address
  0 d stp, >amark 0 unresolved !
    \ modify the code to get the screen address later

  ocr-chars fta, a b ld,
    \ number of chars in the charset
  rbegin
    \ B=remaining chars
    \ HL = address of scan 0 of the current char
    b push, h push,
    0 d ldp#,  \ restore the screen address
    >amark 0 unresolved @ !  -->
      \ compilation: resolve the address of the screen address
    \ DE = screen address

( ocr )

    08 b ld#,  \ scans
    rbegin
      d ftap, m xor,  \ scan match?
      here nz? ?jr, >rmark 1 unresolved !
        \ if not, goto next_char
      d inc, h incp,  \ update the pointers
    rstep  \ next scan

    \ all eight scans match: udg found

    b pop, b pop,
      \ discard the saved pointer
      \ B = chars left
    ocr-chars fta, b sub, a b ld,
    ocr-first fta, b add, a b ld,
      \ B = char number
    here jr, >rmark 2 unresolved !
      \ go to end

    \ next_char:
    1 unresolved @ >rresolve
    h pop, 0008 d ldp#, d addp, b pop,
  rstep
  \ B = 0 (no char matches)

  \ end:
  2 unresolved @ >rresolve  0 h ld#, b l ld,
  b pop, h push, jpnext, end-code

  \ Credit:
  \
  \ Adapted from anonymous code published on Todospectrum,
  \ issue 19 (1986-03), page 65.
  \ http://microhobby.speccy.cz/zxsf/revistas-ts.htm

  \ XXX FIXME -- return a flag apart from the code, in order to
  \ make it possible to recognize character zero:
  \
  \ ocr ( col row -- c true | false )
  \
  \ Or a variant:
  \
  \ ocr? ( col row -- c true | false )

  \ doc{
  \
  \ ocr ( col row -- c | 0 )
  \
  \ Try to recognize the character printed at the given cursor
  \ coordinates, using the font whose first printable character
  \ is pointed by the variable `ocr-charset`. The variable
  \ `ocr-chars` holds the number of characters in the set, and
  \ `ocr-first` holds the code of its first character.  If
  \ succesful, return the character number _c_ according to the
  \ said variables. Otherwise return 0. Inverse characters are
  \ not recognized.
  \
  \ Note: The name `ocr` stands for "Optical Character
  \ Recognition".
  \
  \ See also: `udg-ocr`, `ascii-ocr`.
  \
  \ }doc

( ascii-ocr udg-ocr )

[unneeded] ascii-ocr ?( need ocr need os-chars

: ascii-ocr ( -- )
  os-chars @ 256 + ocr-charset !
  bl ocr-first c!  95 ocr-chars c! ; ?)

  \ doc{
  \
  \ ascii-ocr ( -- )
  \
  \ Set `ocr` to work with the current ASCII charset, pointed
  \ by `os-chars`.
  \
  \ See also: `udg-ocr`, `set-font`.
  \
  \ }doc

[unneeded] udg-ocr ?( need ocr need os-udg

: udg-ocr ( n -- )
  os-udg @ ocr-charset !  0 ocr-first c!  ocr-chars c! ; ?)

  \ doc{
  \
  \ udg-ocr ( n -- )
  \
  \ Set `ocr` to work with the first _n_ chars of the current
  \ UDG set.
  \
  \ See also: `ascii-ocr`, `set-udg`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-12-20: Rename `jppushhl` to `jppushhl,` after the
  \ change in the kernel.
  \
  \ 2017-01-02: Convert `ocr` to `z80-asm,`. Improve
  \ documentation.
  \
  \ 2017-01-05: Update `need z80-asm,` to `need assembler`.
  \
  \ 2017-01-13: Fix `ocr`: `de` was still used as register, but
  \ it doesn't exist anymore. Reorganize the source. Make
  \ `ascii-ocr` and `udg-ocr` optional. Add `0udg-ocr`. Improve
  \ documentation.
  \
  \ 2017-02-04: Adapt to 0-index-only UDG, after the changes in
  \ the kernel and the library: Remove `udg-ocr` and rename
  \ `0udg-ocr` to `udg-ocr`. Convert `ocr-chars` and
  \ `ocr-first` to character variables.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-02-21: Need `unresolved`, which now is optional, not
  \ part of the assembler.
  \
  \ 2017-02-25: Fix typo.
  \
  \ 2017-03-11: Need `>amark`, which now is optional, not
  \ included in the assembler by default.
  \
  \ 2017-05-09: Remove `jppushhl,`.

  \ vim: filetype=soloforth
