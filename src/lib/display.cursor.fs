  \ display.cursor.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201704191834
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words related to the cursor position.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( column last-column row last-row at-x at-y xy>r r>xy )

[unneeded] column ?\ : column ( -- col ) xy drop ;

  \ doc{
  \
  \ column ( -- col )
  \
  \ Current column (x coordinate).
  \
  \ See also: `row`, `last-column`, `columns`.
  \
  \ }doc

[unneeded] last-column
?\ need columns : last-column ( -- col ) columns 1- ;

  \ doc{
  \
  \ last-column ( -- col )
  \
  \ Last column (x coordinate) in the current screen mode.
  \
  \ See also: `last-row`, `columns`, `column`.
  \
  \ }doc

[unneeded] row ?\ : row ( -- row ) xy nip ;

  \ doc{
  \
  \ row ( -- row )
  \
  \ Current row (y coordinate).
  \
  \ See also: `column`, `last-row`, `rows`.
  \
  \ }doc

[unneeded] last-row
?\ need rows  : last-row ( -- row  ) rows 1- ;

  \ doc{
  \
  \ last-row ( -- row )
  \
  \ Last row (y coordinate) in the current screen mode.
  \
  \ See also: `last-column`, `row`, `rows`.
  \
  \ }doc

[unneeded] at-x ?\ need row  : at-x ( col -- ) row at-xy ;

  \ doc{
  \
  \ at-x ( col -- )
  \
  \ Set the cursor at the given column (x coordinate) _col_ and
  \ the current row (y coordinate).
  \
  \ See also: `at-y`, `at-xy`, `row`, `column`.
  \
  \ }doc

[unneeded] at-y
?\ need column  : at-y ( row -- ) column swap at-xy ;

  \ doc{
  \
  \ at-y ( row -- )
  \
  \ Set the cursor at the current column (x coordinate) and the
  \ given row (y coordinate) _row_.
  \
  \ See also: `at-x`, `at-xy`, `row`, `column`.
  \
  \ }doc


[unneeded] xy>r ?\ : xy>r ( R: -- col row ) r>    xy 2>r >r ;

  \ doc{
  \
  \ xy>r ( -- ) ( R: -- col row )
  \
  \ Save the current cursor coordinates to the return stack.
  \
  \ See also: `r>xy`, `save-mode`.
  \
  \ }doc

[unneeded] r>xy ?\ : r>xy ( R: col row -- ) r> 2r> at-xy >r ;

  \ doc{
  \
  \ r>xy ( -- ) ( R: col row -- )
  \
  \ Restore the current cursor coordinates from the return
  \ stack.
  \
  \ See also: `xy>r`, `restore-mode`.
  \
  \ }doc

( xy>scra_ xy>scra )

[unneeded] xy>scra_ ?( need assembler

create xy>scra_ ( -- a ) asm

  b a ld, %11000 and#, #64 add#, a d ld, b a ld, %111 and#,
  rrca, rrca, rrca, a e ld, c a ld, e add, a e ld,
  ret, end-asm ?)

  \ ld a,b     ; y coordinate
  \ and %11000 ; mask segment: 0..2
  \ add a,64   ; 64*256 = 16384, Spectrum's screen memory
  \ ld d,a     ; high byte of the screen address
  \ ld a,b     ; y coordinate
  \ and %111   ; mask row within segment: 0..7
  \ rrca       ; multiply row...
  \ rrca       ; ...
  \ rrca       ; ...by 32
  \ ld e,a     ; low byte of the screen address
  \ ld a,c     ; add on y coordinate
  \ add a,e    ; mix with low byte
  \ ld e,a     ; DE = address of screen position
  \ ret

  \ Credit:
  \
  \ How To Write ZX Spectrum Games – Chapter 9
  \ http://chuntey.arjunnair.in/?p=154

  \ doc{
  \
  \ xy>scra_ ( -- a )
  \
  \ Return address _a_ of a Z80 routine that calculates the
  \ screen address correspondent to given cursor coordinates.
  \
  \ Input registers:
  \
  \ - B = y coordinate (0..23)
  \ - C = x coordinate (0..31)
  \
  \ Output registers:
  \
  \ - DE = screen address
  \
  \ See also: `xy>scra`, `gxy>scra_`.
  \
  \ }doc

[unneeded] xy>scra ?( need assembler need xy>scra_

code xy>scra ( x y -- a )

  h pop, l a ld, h pop, b push, a b ld, l c ld,
  xy>scra_ call, exde, b pop, jppushhl, end-code ?)

  \ pop hl
  \ ld a,l
  \ pop hl
  \ push bc          ; save Forth IP
  \ ld b,a           ; y coordinate
  \ ld c,l           ; x coordinate
  \ call cursor_addr
  \ ex de,hl         ; HL = screen address
  \ pop bc           ; restore Forth IP
  \ jp push_hl

  \ doc{
  \
  \ xy>scra ( x y -- a )
  \
  \ Convert cursor coordinates _x y_ to their correspondent
  \ screen address _a_.
  \
  \ See also: `xy>scra_` , `gxy>scra`.
  \
  \ }doc

( xy>scra_ xy>scra )

[unneeded] xy>scra_ ?( need assembler

create xy>scra_ ( -- a ) asm

  \ XXX TMP --
  \
  \ Alternative version that does not use the BC register and
  \ returns the address in HL.

  d a ld, %11000 and#, #64 add#, a h ld, d a ld, %111 and#,
  rrca, rrca, rrca, a l ld, e a ld, l add, a l ld,
  ret, end-asm ?)

  \ ld a,d     ; y coordinate
  \ and %11000 ; mask segment: 0..2
  \ add a,64   ; 64*256 = 16384, Spectrum's screen memory
  \ ld h,a     ; high byte of the screen address
  \ ld a,d     ; y coordinate
  \ and %111   ; mask row within segment: 0..7
  \ rrca       ; multiply row...
  \ rrca       ; ...
  \ rrca       ; ...by 32
  \ ld l,a     ; low byte of the screen address
  \ ld a,e     ; add on y coordinate
  \ add a,l    ; mix with low byte
  \ ld l,a     ; HL = address of screen position
  \ ret

  \ Credit:
  \
  \ How To Write ZX Spectrum Games – Chapter 9
  \ http://chuntey.arjunnair.in/?p=154

  \ xy>scra_ ( -- a )
  \
  \ Return address _a_ of a Z80 routine that calculates the
  \ screen address correspondent to given cursor coordinates.
  \
  \ Input registers:
  \
  \ - D = y coordinate (0..23)
  \ - E = x coordinate (0..31)
  \
  \ Output registers:
  \
  \ - HL = screen address
  \
  \ See also: `xy>scra`, `gxy>scra_`.

[unneeded] xy>scra ?( need assembler need xy>scra_

code xy>scra ( x y -- a )

  h pop, d pop, l d ld, xy>scra_ call, jppushhl, end-code ?)

  \ pop hl           ; L = y coordinate
  \ pop de           ; E = x coordinate
  \ ld d,l           ; D = y coordinate
  \ call cursor_addr
  \ jp push_hl

  \ xy>scra ( x y -- a )
  \
  \ Convert cursor coordinates _x y_ to their correspondent
  \ screen address _a_.
  \
  \ See also: `xy>scra_` , `gxy>scra`.

( xy>gxy xy>gxy176 )

[unneeded] xy>gxy ?(

code xy>gxy ( x y -- gx gy )
  D1 c, E1 c, 29 c, 29 c, 29 c, E5 c, EB c, 29 c, 29 c, 29 c,
  \ pop de
  \ pop hl
  \ add hl,hl
  \ add hl,hl
  \ add hl,hl
  \ push hl
  \ ex de,hl
  \ add hl,hl
  \ add hl,hl
  \ add hl,hl
  3E c, #191 c, 95 c, 6F c, jppushhl, end-code ?)
  \ ld a,191
  \ sub l
  \ ld l,a
  \ _jp_pushl

  \ doc{
  \
  \ xy>gxy ( x y -- gx gy )
  \
  \ Convert cursor coordinates _x y_ to graphic coordinates _gx
  \ gy_.  _x_ is 0..31, _y_ is 0..23, _gx_ is 0..255 and _gy_
  \ is 0..191.
  \
  \ See also: `xy>attra`, `xy>attr`, `xy>gxy176`, `plot`,
  \ `set-pixel`.
  \
  \ }doc

[unneeded] xy>gxy176 ?(

code xy>gxy176 ( x y -- gx gy )
  D1 c, E1 c, 29 c, 29 c, 29 c, E5 c, EB c, 29 c, 29 c, 29 c,
  \ pop de
  \ pop hl
  \ add hl,hl
  \ add hl,hl
  \ add hl,hl
  \ push hl
  \ ex de,hl
  \ add hl,hl
  \ add hl,hl
  \ add hl,hl
  3E c, #175 c, 95 c, 6F c, jppushhl, end-code ?)
  \ ld a,175
  \ sub l
  \ ld l,a
  \ _jp_pushl

  \ doc{
  \
  \ xy>gxy176 ( x y -- gx gy )
  \
  \ Convert cursor coordinates _x y_ to graphic coordinates _gx
  \ gy_ (as used by Sinclair BASIC, i.e. the lower 16 pixel
  \ rows are not used).  _x_ is 0..31, _y_ is 0..23, _gx_ is
  \ 0..255 and _gy_ is 0..175.
  \
  \ ``xy>gxy176`` is provided to make it easier to adapt
  \ Sinclair BASIC programs.
  \
  \ See also: `xy>gxy`, `plot176`, `set-pixel176`.
  \
  \ }doc

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
  \ See also: `xy>attra`, `xy>attr`, `xy>gxy`.
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
  \ See also: `xy>attra`, `xy>attra_`, `xy>gxy`.
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
  \ See also: `xy>attr`, `xy>attra_`, `xy>gxy`.
  \
  \ }doc

  \ ===========================================================
  \ Old change log of <graphics.attributes.fs>

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

  \ ===========================================================
  \ Change log

  \ 2015-10-14: Add `column`, `row`, `at-x`, `at-y`, adapted
  \ from Galope.
  \
  \ 2016-05-01: Add conditional compilation and documentation.
  \
  \ 2016-05-07: Fix typos and conditional compilation.
  \
  \ 2016-11-26: Improve needing and documentation.
  \
  \ 2017-01-18: Fix `last-column` and `last-row`. Improve
  \ documentation.  Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-03-15: Add `xy>r` and `r>xy`.
  \
  \ 2017-03-28: Move `xy>scra` and `xy>scra_` from the
  \ <printing.udg.fs> module.
  \
  \ 2017-03-29: Add `xy>gxy`, `xy>gxy176`.  Move `xy>attra_`,
  \ `xy>attr` and `xy>attra` from the <graphics.attributes.fs>
  \ module, which is deleted. Improve documentation.
  \
  \ 2017-04-19: Fix documentation.

  \ vim: filetype=soloforth
