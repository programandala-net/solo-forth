  \ graphics.attributes.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201703132351
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words related to screen attributes.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( xy>attra_ xy>attr xy>attra )

[unneeded] xy>attra_ ?(

create xy>attra_ ( -- a ) asm

  7B c, 0F c, 0F c, 0F c, 5F c, E6 c, E0 c, AA c, 6F c, 7B c,
    \ ld a,e    ; line to a $00..$17 (max %00010111)
    \ rrca
    \ rrca
    \ rrca      ; rotate bits left
    \ ld e,a    ; store in E as an intermediate value
    \ and $E0   ; pick up bits %11100000 (was %00011100)
    \ xor d     ; combine with column $00..$1F
    \ ld l,a    ; low byte now correct
    \ ld a,e    ; bring back intermediate result from E
  E6 c, 03 c, EE c, 58 c, 67 c, C9 c, end-asm ?)
    \ and 003h  ; mask to give correct third of screen
    \ xor 058h  ; combine with base address
    \ ld h,a    ; high byte correct
    \ ret

  \ doc{
  \
  \ xy>attra_ ( -- a )
  \
  \ Return the address _a_ of a Z80 routine that calculates the
  \ attribute address of a cursor position.  This is a modified
  \ version of the ROM routine at 0x2583.
  \
  \ Input:
  \
  \ - D = column (0..31) - E = row (0..23)
  \
  \ Output:
  \
  \ - HL = address of the attribute in the screen
  \
  \ }doc

[unneeded] xy>attr ?( need xy>attra_

code xy>attr ( col row -- b )
  D1 c, E1 c, 55 c, xy>attra_ call, 6E c, 26 c, 00 c,
    \ pop de ; E = row
    \ pop hl
    \ ld d,l ; D = col
    \ call attr_addr
    \ ld l,(hl)
    \ ld h,0 ; HL = attribute
  jppushhl, end-code ?)
    \ jp pushhl

  \ doc{
  \
  \ xy>attr ( col row -- b )
  \
  \ Return the color attribute _b_ of the given cursor
  \ coordinates _col row_.
  \
  \ }doc

[unneeded] xy>attra ?( need xy>attra_

code xy>attra ( col row -- a )
  D1 c, E1 c, 55 c, xy>attra_ call, jppushhl, end-code ?)
    \ pop de ; E = row
    \ pop hl
    \ ld d,l ; D = col
    \ call attr_addr
    \ jp pushhl

  \ doc{
  \
  \ xy>attra ( col row -- a )
  \
  \ Return the color attribute address _a_ of the given cursor
  \ coordinates _col row_.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-10-15: Improve the format of the documentation.
  \
  \ 2016-12-20: Rename `jppushhl` to `jppushhl,` after the
  \ change in the kernel.
  \
  \ 2016-12-30: Compact the code, saving one block.
  \
  \ 2016-12-31: Rewrite `attr`, `attr-addr` and `(attr-addr)`
  \ with Z80 opcodes, without assembler. Compact the code,
  \ saving one block.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-19: Remove remaining `exit` at the end of
  \ conditional interpretation, after `end-asm`.
  \
  \ 2017-03-13: Rename: `(attr-addr)` to `xy>attra_`,
  \ `attr-addr` to `xy>attra`.  `attr` to `xy>attr`.

  \ vim: filetype=soloforth
