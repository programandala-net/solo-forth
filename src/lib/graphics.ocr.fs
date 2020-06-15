  \ graphics.ocr.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202006152025
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words that recognize characters on the screen.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018,
  \ 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( ocr )

need assembler need unresolved need >amark

variable ocr-font  $3D00 ocr-font !

  \ doc{
  \
  \ ocr-font ( -- a ) "o-c-r-font"
  \
  \ A `variable`. _a_ is the address of a cell containing the
  \ address of the first printable character in the character
  \ set used by `ocr`.  By default it contains 0x3D00, the
  \ address of the space character in the `rom-font`.
  \
  \ The configuration of `ocr`, including this variable, can be
  \ changed by `ascii-ocr` and `udg-ocr`.
  \
  \ See: `ocr-chars`, `ocr-first`.
  \
  \ }doc

create ocr-first bl c,

  \ doc{
  \
  \ ocr-first ( -- ca ) "o-c-r-first"
  \
  \ A `cvariable`. _ca_ is the address of a byte
  \ containing the code of the first printable character in the
  \ character set used by `ocr`, pointed by `ocr-font`.  By
  \ default it contais `bl`, the code of the space character.
  \
  \ The configuration of `ocr`, including this variable, can be
  \ changed by `ascii-ocr` and `udg-ocr`.
  \
  \ See: `ocr-chars`, `ocr-font`.
  \
  \ }doc

create ocr-chars 127 bl - c,

  \ doc{
  \
  \ ocr-chars ( -- ca ) "o-c-r-chars"
  \
  \ A `cvariable`. _ca_ is the address of a byte
  \ containing the number of characters used by `ocr`, from the
  \ address pointed by `ocr-font`. By default it contais 95,
  \ the number of printable ASCII characters in the ROM
  \ character set.
  \
  \ The configuration of `ocr`, including this variable, can be
  \ changed by `ascii-ocr` and `udg-ocr`.
  \
  \ See: `ocr-first`, `ocr-font`.
  \
  \ }doc

code ocr ( col line -- n )

  d pop, h pop, b push,
    \ get row, get col, save the Forth IP
  l b ld, e c ld, ocr-font fthl,
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
  \ ocr ( col row -- c | 0 ) "o-c-r"
  \
  \ Try to recognize the character printed at the given cursor
  \ coordinates, using the character set whose first printable
  \ character is pointed by the variable `ocr-font`. The
  \ character variable `ocr-chars` contains the number of
  \ characters in the set, and its counterpart `ocr-first`
  \ contains the code of its first character.  If succesful,
  \ return the character number _c_ according to the said
  \ variables. Otherwise return 0.  Inverse characters are not
  \ recognized.
  \
  \ Note: The name `ocr` stands for "Optical Character
  \ Recognition".
  \
  \ See: `udg-ocr`, `ascii-ocr`.
  \
  \ }doc

( ascii-ocr udg-ocr )

unneeding ascii-ocr ?( need ocr need os-chars

: ascii-ocr ( -- )
  os-chars @ 256 + ocr-font !
  bl ocr-first c!  95 ocr-chars c! ; ?)

  \ doc{
  \
  \ ascii-ocr ( -- ) "ascii-o-c-r"
  \
  \ Set `ocr` to work with the current ASCII charset, pointed
  \ by `os-chars`.
  \
  \ See: `ocr-font`, `ocr-first`, `ocr-chars`,
  \ `udg-ocr`, `set-font`.
  \
  \ }doc

unneeding udg-ocr ?( need ocr need os-udg

: udg-ocr ( n -- )
  os-udg @ ocr-font !  0 ocr-first c!  ocr-chars c! ; ?)

  \ doc{
  \
  \ udg-ocr ( n -- ) "u-d-g-o-c-r"
  \
  \ Set `ocr` to work with the first _n_ chars of the current
  \ UDG set, pointed by `os-udg`.
  \
  \ See: `ocr-font`, `ocr-first`, `ocr-chars`,
  \ `ascii-ocr`, `set-udg`.
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
  \
  \ 2017-05-20: Improve documentation. Create `ocr-first` and
  \ `ocr-chars` with `create` instead of `here` and `constant`;
  \ `create` is a bit faster at run-time.
  \
  \ 2018-01-02: Rename `ocr-charset` `ocr-font`.
  \
  \ 2018-02-15: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-06-04: Link `variable` in documentation.
  \
  \ 2020-06-15: Improve documentation: Add cross-references to
  \ `cvariable`.

  \ vim: filetype=soloforth
