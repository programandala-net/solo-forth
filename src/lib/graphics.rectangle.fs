  \ graphics.rectangle.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201801030000
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words that draw rectangles.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( wipe-rectangle )

need assembler

code wipe-rectangle ( column row width height -- )

  exx, 0000 ix ldp#, sp addix,
    \ exx ; save the Forth IP
    \ ld ix,0
    \ add ix,sp ; ix = address of TOS
    \
    \ ; ix+6 = column
    \ ; ix+4 = row
    \ ; ix+2 = width
    \ ; ix+0 = height

  04 ix a ftx, a d ld, rrca, rrca, rrca, #224 and#, 06 ix orx,
    \ ld a,(ix+4) ; row
    \ ld d,a
    \ rrca
    \ rrca
    \ rrca
    \ and 224
    \ or (ix+6) ; column
  a e ld, d a ld, #24 and#, #64 or#, a d ld,
    \ ld e,a
    \ ld a,d ; column
    \ and 24
    \ or 64
    \ ld d,a
  02 ix c ftx, 00 ix a ftx, a add, a add, a add, a b ld,
    \ ld c,(ix+2) ; width
    \ ld a,(ix+0) ; height
    \ add a,a
    \ add a,a
    \ add a,a
    \ ld b,a ; width*8

  rbegin
    \ delete_bitmap:
    d push, d h ldp, d incp, 00 m ld#, b push, c dec,
      \ push de     ; save the address of the rectangle scan
      \ ld l,e
      \ ld h,d      ; HL = origin, start of the scan
      \ inc de      ; DE = destination
      \ ld (hl),0   ; delete the first byte
      \ push bc     ; save the counts
      \ dec c       ; is width greater than 1?
    nz? rif  00 b ld#, ldir,  rthen
      \ jr z,label1
      \ ld b,0      ; BC = width
      \ ldir        ; erase the rest of the scan
      \ label1:
    b pop, d pop, d inc, d a ld, 07 and#,
      \ pop bc      ; restore counts
      \ pop de      ; restore address of scan
      \ inc d
      \ ld a,d
      \ and 7
    z? rif  #32 a ld#, e add, a e ld,
      nc? rif  d a ld, 08 sub#, a d ld,  rthen
    rthen
      \ jr nz,inc_char
      \ ld a,32
      \ add a,e
      \ ld e,a
      \ jr c,inc_char
      \ ld a,d
      \ sub 8
      \ ld d,a
      \ inc_char:
  rstep  0000 h ldp#, sp addp, 04 cells d ldp#, d addp, ldsp,
    \ djnz delete_bitmap
    \ ; Drop the parameters:
    \ ld hl,0
    \ add hl,sp
    \ ld de,4*cells
    \ add hl,de
    \ ld sp,hl

  exx, next ix ldp#, jpnext, end-code
    \ exx           ; restore the Forth IP
    \ ld ix,next    ; restore IX
    \ _jp_next

  \ Credit:
  \
  \ Code extracted and adapted from a routine written by Pablo
  \ Ariza, published on Microhobby Especial, issue 7 (1987-12),
  \ page 50: <http://microhobby.org/mhes7.htm>.

  \ doc{
  \
  \ wipe-rectangle ( column row width height -- )
  \
  \ Clear a screen rectangle at the given character coordinates
  \ and of the given size in characters.  Only the bitmap is
  \ cleared. The color attributes remain unchanged.
  \
  \ See: `clear-rectangle`, `color-rectangle`, `wcls`.
  \
  \ }doc

( color-rectangle )

need assembler

code color-rectangle ( column row width height color -- )

  exx, 0 ix ldp#, sp addix,
    \ exx           ; save the Forth IP
    \ ld ix,0
    \ add ix,sp     ; ix = address of TOS
    \
    \ ; ix+8 = column
    \ ; ix+6 = row
    \ ; ix+4 = width
    \ ; ix+2 = height
    \ ; ix+0 = color

  #6 ix a ftx, #22 d ld#, a add, a add, a add, a add,
    \ ld a,(ix+6) ; row
    \ ld d,22
    \ add a
    \ add a
    \ add a
    \ add a         ; row*8
  d rl, a add, d rl, #8 ix orx, a e ld,
    \ rl d
    \ add a
    \ rl d
    \ or (ix+8)     ; column
    \ ld e,a
  #2 ix b ftx, #4 ix c ftx,
    \ ld b,(ix+2)   ; height
    \ ld c,(ix+4)   ; width
  rbegin
    \ delete_attributes:
    d push, d h ldp, d incp, b push, #0 b ld#,
      \ push de
      \ ld h,d
      \ ld l,e
      \ inc de
      \ push bc
      \ ld b,0
    00 ix a ftx, a m ld, c dec,
      \ ld a,(ix+0) ; color
      \ ld (hl),a
      \ dec c
    nz? rif  ldir,  rthen
      \ jr z,no_more_attributes
      \ ldir
      \ no_more_attributes:
    b pop, h pop, #32 d ldp#, d addp, exde,
      \ pop bc
      \ pop hl
      \ ld de,32
      \ add hl,de
      \ ex de,hl
  rstep
    \ djnz delete_attributes

  \ Drop the parameters:
  0000 h ldp#, sp addp, 05 cells d ldp#, d addp, ldsp,
    \ ld hl,0
    \ add hl,sp
    \ ld de,5*cells
    \ add hl,de
    \ ld sp,hl

  exx, next ix ldp#, jpnext, end-code
    \ exx           ; restore the Forth IP
    \ ld ix,next    ; restore IX
    \ _jp_next

  \ Credit:
  \
  \ Code extracted and adapted from a routine written by Pablo
  \ Ariza, published on Microhobby Especial, issue 7 (1987-12),
  \ page 50: <http://microhobby.org/mhes7.htm>.

  \ doc{
  \
  \ color-rectangle ( column row width height color -- )
  \
  \ Color a screen rectangle at the given character coordinates
  \ and of the given size in characters with the given color
  \ attribute.  Only the color attributes are changed; the
  \ bitmap remains unchanged.
  \
  \ See: `wcolor`, `wipe-rectangle`, `clear-rectangle`.
  \
  \ }doc

( clear-rectangle )

need assembler

code clear-rectangle ( column row width height color -- )

  exx,  0 ix ldp#, sp addix,
    \ exx ; save the Forth IP
    \ ld ix,0
    \ add ix,sp ; ix = address of TOS
    \
    \ ; ix+8 = column
    \ ; ix+6 = row
    \ ; ix+4 = width
    \ ; ix+2 = height
    \ ; ix+0 = color

  #6 ix a ftx, a d ld, rrca, rrca, rrca, #224 and#, #8 ix orx,
    \ ld a,(ix+6) ; row
    \ ld d,a
    \ rrca
    \ rrca
    \ rrca
    \ and 224
    \ or (ix+8)     ; column
  a e ld, d a ld, #24 and#, #64 or#, a d ld,
    \ ld e,a
    \ ld a,d        ; column
    \ and 24
    \ or 64
    \ ld d,a        ; DE = top left address of the rectangle
  #4 ix c ftx, #2 ix a ftx, a add, a add, a add, a b ld,
    \ ld c,(ix+4)   ; width
    \ ld a,(ix+2)   ; height
    \ add a,a
    \ add a,a
    \ add a,a
    \ ld b,a        ; width*8

  rbegin
    \ delete_bitmap:
    d push, d h ldp, d incp, 0 m ld#, b push, c dec,
      \ push de     ; save the address of the rectangle scan
      \ ld l,e
      \ ld h,d      ; HL = origin, start of the scan
      \ inc de      ; DE = destination
      \ ld (hl),0   ; delete the first byte
      \ push bc     ; save the counts
      \ dec c       ; is width greater than 1?
    nz? rif  0 b ld#, ldir,  rthen
      \ jr z,label1
      \ ld b,0      ; BC = width
      \ ldir        ; erase the rest of the scan
      \ label1:
    b pop, d pop, d inc, d a ld, 7 and#,
      \ pop bc      ; restore counts
      \ pop de      ; restore address of scan
      \ inc d
      \ ld a,d
      \ and 7
    z? rif  #32 a ld#, e add, a e ld,
      nc? rif  d a ld, 8 sub#, a d ld,  rthen
    rthen
      \ jr nz,inc_char
      \ ld a,32
      \ add a,e
      \ ld e,a
      \ jr c,inc_char
      \ ld a,d
      \ sub 8
      \ ld d,a
      \ inc_char:
  rstep  -->
    \ djnz delete_bitmap

( clear-rectangle )

  #6 ix a ftx, #22 d ld#, a add, a add, a add, a add,
    \ ld a,(ix+6)   ; row
    \ ld d,22
    \ add a
    \ add a
    \ add a
    \ add a         ; row*8
  d rl, a add, d rl, #8 ix orx, a e ld, #2 ix b ftx,
    \ rl d
    \ add a
    \ rl d
    \ or (ix+8)     ; column
    \ ld e,a
    \ ld b,(ix+2)   ; height
  rbegin
    \ delete_attributes:
    d push, d h ldp, d incp, b push, #0 b ld#,
      \ push de
      \ ld h,d
      \ ld l,e
      \ inc de
      \ push bc
      \ ld b,0
    0 ix a ftx, a m ld, c dec,
      \ ld a,(ix+0) ; color
      \ ld (hl),a
      \ dec c
    nz? rif  ldir,  rthen
      \ jr z,no_more_attributes
      \ ldir
      \ no_more_attributes:
    b pop, h pop, #32 d ldp#, d addp, exde,
      \ pop bc
      \ pop hl
      \ ld de,32
      \ add hl,de
      \ ex de,hl
  rstep
    \ djnz delete_attributes

  \ Drop the parameters:
  0 h ldp#, sp addp, #5 cells d ldp#, d addp, ldsp,
    \ ld hl,0
    \ add hl,sp
    \ ld de,5*cells
    \ add hl,de
    \ ld sp,hl

  exx, next ix ldp#, jpnext, end-code
    \ exx           ; restore the Forth IP
    \ ld ix,next    ; restore IX
    \ _jp_next

  \ Credit:
  \
  \ Code adapted from a routine written by Pablo Ariza,
  \ published on Microhobby Especial, issue 7 (1987-12), page
  \ 50: <http://microhobby.org/mhes7.htm>.

  \ doc{
  \
  \ clear-rectangle ( column row width height color -- )
  \
  \ Clear a screen rectangle at the given character coordinates
  \ and of the given size in characters.  The bitmap is erased
  \ and the color attributes are changed with the given color
  \ attribute.
  \
  \ ``clear-rectangle`` is written in Z80 and it combines the
  \ functions of `wipe-rectangle` and `color-rectangle`. It may
  \ be defined also this way (with slower but much smaller
  \ code):

  \ ----
  \ : clear-rectangle ( column row width height color -- )
  \   >r 2over 2over wipe-rectangle r> color-rectangle ;
  \ ----

  \ See: `attr-wcls`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-12-20: Rename `jpnext` to `jpnext,` after the change
  \ in the kernel.
  \
  \ 2017-01-05: Convert `color-block`, `clear-block` and
  \ `wipe-block` from `z80-asm` to `z80-asm,`.
  \
  \ 2017-01-05: Update `need z80-asm,` to `need assembler`.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-05-12: Rename the module and the words: Change "block"
  \ to "rectangle". Exchange the functions of `clear-rectangle`
  \ and `wipe-rectangle`, to make `clear-rectangle` consistent
  \ with `cls` ("clear screen"), because `clear-rectangle` is
  \ going to be used as a factor of `wcls`.
  \
  \ 2017-07-27: Replace `jp next` with the actual macro
  \ `_jp_next` in Z80 comments.
  \
  \ 2018-01-02: Improve documentation.

  \ vim: filetype=soloforth
