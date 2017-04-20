  \ display.fonts.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201703132047
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words to manipulate or build fonts.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( get-font rom-font )

[unneeded] get-font ?( need os-chars

code get-font ( -- a ) 2A c, os-chars , jppushhl, end-code ?)
  \ ld hl, (sys_chars)
  \ jp pushhl

  \ doc{
  \
  \ get-font ( -- a )
  \
  \ Get address _a_ of the current font (characters 32..127),
  \ by fetching the system variable `os-chars`.  _a_ is the
  \ bitmap address of character 0.
  \
  \ See also: `set-font`, `default-font`.
  \
  \ }doc

[unneeded] rom-font ?\ 15360 constant rom-font

  \ doc{
  \
  \ rom-font ( -- a )
  \
  \ A constant that holds the address _a_ of the ROM font,
  \ which is 15360 ($3C00), the bitmap address of character 0,
  \ 256 bytes below the bitmap of the space (character 32),
  \ which is the first printable character. This is the default
  \ hold in `os-chars`.
  \
  \ See also: `default-font`, `set-font`, `get-font`,
  \ `outlet-autochars`.
  \
  \ }doc

( outlet-autochars )

need assembler  need os-chars

code outlet-autochars ( a -- )
  h pop, b push, h push,
  \ pop hl          ; HL = address of the new font
  \ push bc         ; save Forth IP
  \ push hl         ; save the new font address for later
  #767 d ldp#, d addp, exde, #16383 h ldp#, #768 b ldp#,
  \ ld de,767       ; font size - 1
  \ add hl,de       ; HL = last address of the new font
  \ ex de,hl        ; DE = last address of the new font
  \ ld hl,16383     ; HL = last address of the ROM font
  \ ld bc,768       ; BC = count, font size
  b push, lddr,
  \ push bc         ; save count for later
  \ lddr            ; copy the ROM font to the new one

  b pop, d incp, d h ldp,
  \ pop bc          ; BC = count, font size
  \ inc de          ; DE = first address of the new font
  \ ld h,d
  \ ld l,e          ; HL = first address of the new font

  \ Modify the font:

  rbegin m a ld, a sra, m or, a m ld, ldi, c a ld, b or,
         z? runtil
  \ next_scan:
  \   ld a,(hl)
  \   sra a
  \   or (hl)
  \   ld (hl),a
  \   ldi
  \   ld a,c
  \   or b            ; finished?
  \   jr nz,next_scan ; if not finished, continue

  h pop, h dec, os-chars h stp,
  \ pop hl,           ; HL = new font address (character 32)
  \ dec h             ; HL = new font address (character 0)
  \ ld (OS_CHARS),hl  ; activate the new font

  \ Fix character 'm':

  #877 d ldp#, d addp, %1101010 a ld#, a m ld, h incp, a m ld,
  \ ld de,877
  \ add hl,de       ; HL = address of scan 5 of character 'm'
  \ ld a,%01101010
  \ ld (hl),a
  \ inc hl          ; HL = address of scan 6 of character 'm'
  \ ld (hl),a

  \ Fix character 'w':

  #76 d ldp#, d addp, %1100011 a ld#, a m ld,
  \ ld de,76
  \ add hl,de       ; HL = address of scan 2 of character 'w'
  \ ld a,%01100011
  \ ld (hl),a
  h incp, a m ld, h incp, %1101011 a ld#, a m ld,
  \ inc hl          ; HL = address of scan 3 of character 'w'
  \ ld (hl),a
  \ inc hl          ; HL = address of scan 4 of character 'w'
  \ ld a,%01101011
  \ ld (hl),a
  h incp, %111110 a ld#, a m ld, h incp, %110110 a ld#, a m ld,
  \ inc hl          ; HL = address of scan 5 of character 'w'
  \ ld a,%00111110
  \ ld (hl),a
  \ inc hl          ; HL = address of scan 6 of character 'w'
  \ ld a,%00110110
  \ ld (hl),a
  b pop, jpnext, end-code
  \ pop bc          ; restore the Forth IP
  \ _jp_next

  \ doc{
  \
  \ outlet-autochars ( a -- )
  \
  \ Create a modified, bolder copy of the ZX Spectrum ROM font
  \ and store it at _a_. 768 bytes will be used from _a_. Then
  \ activate the new font by modifing the contents of
  \ `os-chars`.
  \
  \ The code word of `outlet-autochars`` has been adapted from
  \ the Autochars routine used by the Outlet magazine,
  \ published in its issue #1 (1987-09).
  \
  \ Usage example:

  \ ----
  \ create outlet-font  768 allot
  \ need outlet-autochars
  \ outlet-font outlet-autochars
  \ ----

  \ See also: `set-font`, `rom-font`.
  \
  \ }doc

  \ Credit:
  \
  \ Original routine, by Chezron Software from Outlet #1
  \ (1987-09).
  \
  \ 2017-02-27: Disassembled and commented by Marcos Cruz
  \ (programandala.net).

  \ ld hl,(23670)     ; HL = address of the new font
  \ ld de,767         ; font size - 1
  \ add hl,de         ; HL = last address of the new font
  \ ex de,hl          ; DE = last address of the new font
  \ ld hl,16383       ; HL = last address of the ROM font
  \ ld bc,768         ; BC = count, font size
  \ push bc           ; save count for later
  \ lddr              ; copy the ROM font to the new one
  \
  \ pop bc            ; BC = count, font size
  \ inc de            ; DE = first address of the new font
  \ push de
  \ pop hl            ; HL = first address of the new font
  \
  \ ; Modify the font:
  \
  \ next_scan:
  \   ld a,(hl)
  \   sra a
  \   or (hl)
  \   ld (hl),a
  \   ldi
  \   ld a,c
  \   or b            ; finished?
  \   jr nz,next_scan ; if not finished, continue

  \ ld hl,(23670)     ; HL = data font address (from character 32)
  \ dec h             ; HL = address of character 0
  \ ld (23606),hl     ; activate the new font
  \
  \ ; Fix character 'm':
  \
  \ ld de,877
  \ add hl,de         ; HL = address of scan 5 of character 'm'
  \ ld a,%01101010
  \ ld (hl),a
  \ inc hl            ; HL = address of scan 6 of character 'm'
  \ ld (hl),a
  \
  \ ; Fix character 'w':
  \
  \ ld de,76
  \ add hl,de         ; HL = address of scan 2 of character 'w'
  \ ld a,%01100011
  \ ld (hl),a
  \ inc hl            ; HL = address of scan 3 of character 'w'
  \ ld (hl),a
  \ inc hl            ; HL = address of scan 4 of character 'w'
  \ ld a,%01101011
  \ ld (hl),a
  \ inc hl            ; HL = address of scan 5 of character 'w'
  \ ld a,%00111110
  \ ld (hl),a
  \ inc hl            ; HL = address of scan 6 of character 'w'
  \ ld a,%00110110
  \ ld (hl),a
  \
  \ ret

  \ ===========================================================
  \ Change log

  \ 2017-02-27: Add `outlet-autochars`. Move `get-font` and
  \ `rom-font` from the UDG module.
  \
  \ 2017-03-13: Improve documentation.

  \ vim: filetype=soloforth
