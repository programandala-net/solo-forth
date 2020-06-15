  \ display.cursor.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202006152041
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words related to the cursor position.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018,
  \ 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( column last-column row last-row at-x at-y xy>r r>xy home? )

unneeding column ?\ : column ( -- col ) xy drop ;

  \ doc{
  \
  \ column ( -- col )
  \
  \ Current column (x coordinate).
  \
  \ See: `row`, `last-column`, `columns`.
  \
  \ }doc

unneeding last-column
?\ need columns : last-column ( -- col ) columns 1- ;

  \ doc{
  \
  \ last-column ( -- col )
  \
  \ Last column (x coordinate) in the current screen mode.
  \
  \ See: `last-row`, `columns`, `column`.
  \
  \ }doc

unneeding row ?\ : row ( -- row ) xy nip ;

  \ doc{
  \
  \ row ( -- row )
  \
  \ Current row (y coordinate).
  \
  \ See: `column`, `last-row`, `rows`.
  \
  \ }doc

unneeding last-row
?\ need rows  : last-row ( -- row  ) rows 1- ;

  \ doc{
  \
  \ last-row ( -- row )
  \
  \ Last row (y coordinate) in the current screen mode.
  \
  \ See: `last-column`, `row`, `rows`.
  \
  \ }doc

unneeding at-x ?\ need row  : at-x ( col -- ) row at-xy ;

  \ doc{
  \
  \ at-x ( col -- )
  \
  \ Set the cursor at the given column (x coordinate) _col_ and
  \ the current row (y coordinate).
  \
  \ See: `at-y`, `at-xy`, `row`, `column`.
  \
  \ }doc

unneeding at-y
?\ need column  : at-y ( row -- ) column swap at-xy ;

  \ doc{
  \
  \ at-y ( row -- )
  \
  \ Set the cursor at the current column (x coordinate) and the
  \ given row (y coordinate) _row_.
  \
  \ See: `at-x`, `at-xy`, `row`, `column`.
  \
  \ }doc


unneeding xy>r ?\ : xy>r ( R: -- col row ) r> xy 2>r >r ;

  \ doc{
  \
  \ xy>r ( -- ) ( R: -- col row ) "x-y-to-r"
  \
  \ Save the current cursor coordinates to the return stack.
  \
  \ See: `r>xy`, `save-mode`.
  \
  \ }doc

unneeding r>xy ?\ : r>xy ( R: col row -- ) r> 2r> at-xy >r ;

  \ doc{
  \
  \ r>xy ( -- ) ( R: col row -- ) "r-to-x-y"
  \
  \ Restore the current cursor coordinates from the return
  \ stack.
  \
  \ See: `xy>r`, `restore-mode`.
  \
  \ }doc

unneeding home? ?\ need xy : home? ( -- f ) xy + 0= ;

  \ doc{
  \
  \ home? ( -- f ) "home-question"
  \
  \ Is the cursor at home position (column 0, row 0)?
  \
  \ See: `xy`, `home`.
  \
  \ }doc

( xy>scra_ xy>scra )

unneeding xy>scra_ ?( need assembler

  \ XXX TODO -- Rewrite in Z80 opcodes.

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
  \ xy>scra_ ( -- a ) "x-y-to-s-c-r-a-underscore"
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
  \ See: `xy>scra`, `gxy>scra_`.
  \
  \ }doc

unneeding xy>scra ?( need assembler need xy>scra_

  \ XXX TODO -- Rewrite in Z80 opcodes.

code xy>scra ( col row -- a )

  h pop, l a ld, h pop, b push, a b ld, l c ld,
  xy>scra_ call, exde, b pop, h push, jpnext, end-code ?)

  \ pop hl
  \ ld a,l
  \ pop hl
  \ push bc          ; save Forth IP
  \ ld b,a           ; y coordinate
  \ ld c,l           ; x coordinate
  \ call cursor_addr
  \ ex de,hl         ; HL = screen address
  \ pop bc           ; restore Forth IP
  \ push hl
  \ _jp_next

  \ doc{
  \
  \ xy>scra ( col row -- a ) "x-y-to-s-c-r-a"
  \
  \ Convert cursor coordinates _col row_ to their correspondent
  \ screen address _a_.
  \
  \ See: `xy>scra_` , `gxy>scra`.
  \
  \ }doc

( xy>scra_ xy>scra )

unneeding xy>scra_ ?( need assembler

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
  \ See: `xy>scra`, `gxy>scra_`.

unneeding xy>scra ?( need assembler need xy>scra_

code xy>scra ( col row -- a )

  h pop, d pop, l d ld, xy>scra_ call, h push,

  \ pop hl           ; L = y coordinate
  \ pop de           ; E = x coordinate
  \ ld d,l           ; D = y coordinate
  \ call cursor_addr
  \ push hl
  jpnext, end-code ?)
  \ _jp_next

  \ xy>scra ( col row -- a )
  \
  \ Convert cursor coordinates _col row_ to their correspondent
  \ screen address _a_.
  \
  \ See: `xy>scra_` , `gxy>scra`.

( xy>gxy x>gx y>gy xy>gxy176 )

unneeding xy>gxy ?(

code xy>gxy ( col row -- gx gy )
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
  3E c, #191 c, 95 c, 6F c, E5 c, jpnext, end-code ?)
  \ ld a,191
  \ sub l
  \ ld l,a
  \ push hl
  \ _jp_next

  \ doc{
  \
  \ xy>gxy ( col row -- gx gy ) "x-y-to-g-x-y"
  \
  \ Convert cursor coordinates _col row_ to graphic coordinates
  \ _gx gy_.  _col_ is 0..31, _row_ is 0..23, _gx_ is 0..255
  \ and _gy_ is 0..191.
  \
  \ See: `xy>attra`, `xy>attr`, `xy>gxy176`, `plot`,
  \ `set-pixel`.
  \
  \ }doc
  \
  \ XXX TODO --  Adapt to 64-CPL and 42-CPL modes. Or rename
  \ with prefix "mode-32-".

unneeding x>gx ?( need alias need 8*

' 8* alias x>gx ( col -- gx ) ?)

  \ doc{
  \
  \ x>gx ( col -- gx ) "x-to-g-x"
  \
  \ Convert cursor coordinate _col_ (0..31) to graphic
  \ coordinate _gx_ (0..255).
  \
  \ ``x>gx`` is an `alias` of `8*`.
  \
  \ See: `xy>gxy`, `xy>gxy176`.
  \
  \ }doc
  \
  \ XXX TODO --  Adapt to 64-CPL and 42-CPL modes.

unneeding y>gy ?(

code y>gy ( row -- gx )
  D1 c, E1 c, 29 c, 29 c, 29 c, E5 c, EB c, 29 c, 29 c, 29 c,
  \ pop hl
  \ add hl,hl
  \ add hl,hl
  \ add hl,hl
  3E c, #191 c, 95 c, 6F c, E5 c, jpnext, end-code ?)
  \ ld a,191
  \ sub l
  \ ld l,a
  \ push hl
  \ _jp_next

  \ doc{
  \
  \ y>gy ( row -- gy ) "y-to-g-y"
  \
  \ Convert cursor coordinate _row_ (0..23) to graphic
  \ coordinate _gy_ (0..191).
  \
  \ See: `xy>gxy`, `x>gx`.
  \
  \ }doc
  \
  \ XXX TODO --  Adapt to 64-CPL and 42-CPL modes. Or rename
  \ with prefix "mode-32-".

unneeding xy>gxy176 ?(

code xy>gxy176 ( col row -- gx gy )
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
  3E c, #175 c, 95 c, 6F c, E5 c, jpnext, end-code ?)
  \ ld a,175
  \ sub l
  \ ld l,a
  \ push hl
  \ _jp_next

  \ doc{
  \
  \ xy>gxy176 ( col row -- gx gy ) "x-y-to-g-x-y-176"
  \
  \ Convert cursor coordinates _col row_ to graphic coordinates
  \ _gx gy_ (as used by Sinclair BASIC, i.e. the lower 16 pixel
  \ rows are not used).  _col_ is 0..31, _row_ is 0..23, _gx_
  \ is 0..255 and _gy_ is 0..175.
  \
  \ ``xy>gxy176`` is provided to make it easier to adapt
  \ Sinclair BASIC programs.
  \
  \ See: `xy>gxy`, `plot176`, `set-pixel176`.
  \
  \ }doc

( xy>attra_ xy>attr xy>attra )

unneeding xy>attra_ ?(

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
  \ xy>attra_ ( -- a ) "x-y-to-attribute-a-underscore"
  \
  \ Return the address _a_ of a Z80 routine that calculates the
  \ attribute address of a cursor position.  This routine is a
  \ modified version of the ROM routine at 0x2583.
  \
  \ Input:
  \
  \ - D = column (0..31) - E = row (0..23)
  \
  \ Output:
  \
  \ - HL = address of the attribute in the screen
  \
  \ See: `xy>attra`, `xy>attr`, `xy>gxy`.
  \
  \ }doc

unneeding xy>attr ?( need xy>attra_

code xy>attr ( col row -- b )
  D1 c, E1 c, 55 c, xy>attra_ call, 6E c, 26 c, 00 c, E5 c,
    \ pop de ; E = row
    \ pop hl
    \ ld d,l ; D = col
    \ call attr_addr
    \ ld l,(hl)
    \ ld h,0 ; HL = attribute
    \ push hl
  jpnext, end-code ?)
    \ _jp_next

  \ doc{
  \
  \ xy>attr ( col row -- b ) "x-y-to-attribute-a"
  \
  \ Return the color attribute _b_ of the given cursor
  \ coordinates _col row_.
  \
  \ See: `xy>attra`, `xy>attra_`, `xy>gxy`.
  \
  \ }doc

unneeding xy>attra ?( need xy>attra_

code xy>attra ( col row -- a )
  D1 c, E1 c, 55 c, xy>attra_ call, E5 c, jpnext, end-code ?)
    \ pop de ; E = row
    \ pop hl
    \ ld d,l ; D = col
    \ call attr_addr
    \ push hl
    \ _jp_next

  \ doc{
  \
  \ xy>attra ( col row -- a ) "x-y-to-attribute-a"
  \
  \ Return the color attribute address _a_ of the given cursor
  \ coordinates _col row_.
  \
  \ See: `xy>attr`, `xy>attra_`, `xy>gxy`.
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
  \
  \ 2017-05-09: Remove `jppushhl,`.
  \
  \ 2017-09-08: Add `home?`.
  \
  \ 2018-01-03: Add `x>gx`, `y>gy`.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-08: Add words' pronunciaton. Update stack notation
  \ for cursor coordinates.
  \
  \ 2020-05-05: Fix typos.
  \
  \ 2020-06-15: Improve documentation.

  \ vim: filetype=soloforth
