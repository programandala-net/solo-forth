  \ graphics.pixels.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201707271623
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words that manipulate pixels.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ ===========================================================
  \ To-do

  \ XXX TODO -- Reorganize relation between
  \ `slow-gxy>scra_`, `gxy>scra_` and
  \ `fast-gxy>scra_`. Remove `fast-gxy>scra_` and the
  \ deferred `gxy>scra_`, then rename `slow-gxy>scra_` to
  \ `gxy>scra_`.

( gxy>scra_ slow-gxy>scra_ fast-gxy>scra_ )

[unneeded] gxy>scra_ [unneeded] slow-gxy>scra_ and ?(

defer gxy>scra_ ( -- a )

  \ doc{
  \
  \ gxy>scra_ ( -- a )
  \
  \ A deferred word that executes `fast-gxy>scra_` or, by
  \ default, `slow-gxy>scra_`:  Return address _a_ of an
  \ alternative to the PIXEL-ADD ROM routine ($22AA), to let
  \ the range of the y coordinate be 0..191 instead of 0..175.
  \
  \ See: `gxy176>scra_`, `xy>scra_`.
  \
  \ }doc

create slow-gxy>scra_ ( -- a ) asm
  3E c, BF c, 90 00 + c, 22B0 jp, end-asm
  \ ld a,191 ; max Y coordinate
  \ sub b
  \ jp $22B0 ; and return

' slow-gxy>scra_ ' gxy>scra_ defer! ?)

  \ doc{
  \
  \ slow-gxy>scra_ ( -- a )
  \
  \ Return address _a_ of an alternative entry point to the
  \ PIXEL-ADD ROM routine ($22AA), to let the range of the y
  \ coordinate be 0..191 instead of 0..175.
  \
  \ This is the default action of `gxy>scra_`.
  \
  \ When `fast-gxy>scra_` (which is faster but bigger, and
  \ requires the assembler) is needed, the application must use
  \ ``need fast-gxy>scra_`` before ``need set-pixel`` or any
  \ other word that needs `gxy>scra_`.
  \
  \ Input registers:

  \ - C = x cordinate (0..255)
  \ - B = y coordinate (0..191)

  \ Output registers:

  \ - HL = address of the pixel byte in the screen bitmap
  \ - A = position of the pixel in the byte address (0..7),
  \       note: position 0=bit 7, position 7=bit 0.

  \ See: `gxy176>scra_`.
  \
  \ }doc

[unneeded] fast-gxy>scra_ ?(

need gxy>scra_ need assembler

create fast-gxy>scra_ ( -- a ) asm

  #191 a ld#, b sub,  a b ld, rra, scf, rra, a and, rra,
    \ B = adjusted Y coordinate (0..191)

    \ B = the line number from top of screen
    \ A = 0xxxxxxx
    \ set carry flag
    \ A = 10xxxxxx
    \ clear carry flag
    \ A = 010xxxxx

  b xor, F8 and#, b xor, a h ld, c a ld,
    \ keep the top 5 bits 11111000
    \                     010xxbbb
    \ H = high byte
    \ A = the x value 0..255

  rlca, rlca, rlca, b xor, C7 and#,  b xor, rlca, rlca,
    \ the y value
    \ apply mask             11000111

    \ restore unmasked bits  xxyyyxxx
    \ rotate to              xyyyxxxx
    \ required position      yyyxxxxx

  a l ld, c a ld, 07 and#, ret, end-asm
    \ L = low byte
    \ A = pixel position

' fast-gxy>scra_ ' gxy>scra_ defer! ?)

  \ doc{
  \
  \ fast-gxy>scra_ ( -- a )
  \
  \ Return address _a_ of a a modified copy of the PIXEL-ADD
  \ ROM routine ($22AA), to let the range of the y coordinate
  \ be 0..191 instead of 0..175.
  \
  \ This code is a bit faster than `slow-gxy>scra_` because
  \ the necessary jump to the ROM is saved and a useless `and
  \ a` has been removed. But in most cases the speed gain is so
  \ small (only 0.01: see `set-pixel-bench`) that it's not
  \ worth the extra space, including the assembler.
  \
  \ When ``fast-gxy>scra_`` is loaded, it is set as the
  \ current action of `gxy>scra_`.
  \
  \ Input registers:

  \ - C = x cordinate (0..255)
  \ - B = y coordinate (0..191)

  \ Output registers:

  \ - HL = address of the pixel byte in the screen bitmap
  \ - A = position of the pixel in the byte address (0..7),
  \       note: position 0=bit 7, position 7=bit 0.

  \ See: `gxy176>scra_`.
  \
  \ }doc

( gxy176>scra_ gxy176>scra gxy>scra )

[unneeded] gxy176>scra_ ?(

  \ XXX UNDER DEVELOPMENT -- 2016-12-26

create gxy176>scra_ ( -- a ) asm
  3E c, #175 c, 90 00 + c, 22B0 jp, end-asm ?)
  \ ld a,175 ; max Y coordinate in BASIC
  \ sub b
  \ jp $22B0 ; call ROM routine and return

  \ doc{
  \
  \ gxy176>scra_ ( -- a )
  \
  \ Return address _a_ of a routine that uses an alternative
  \ entry point to the PIXEL-ADD ROM routine ($22AA), to bypass
  \ the error check.
  \
  \ Input registers:

  \ - C = x cordinate (0..255)
  \ - B = y coordinate (0..176)

  \ Output registers:

  \ - HL = address of the pixel byte in the screen bitmap
  \ - A = position of the pixel in the byte address (0..7),
  \       note: position 0=bit 7, position 7=bit 0.

  \ See: `gxy176>scra`, `gxy>scra_`.
  \
  \ }doc

[unneeded] gxy176>scra ?( need gxy176>scra_

code gxy176>scra ( gx gy -- n a )
  E1 c,  D1 c, C5 c, 40 05 + c, 48 03 + c,
  \ pop hl
  \ pop de
  \ push bc
  \ ld b,l ; b=gy
  \ ld c,e ; c=gx
  gxy176>scra_ call, C1 c, 16 c, 0 c,  58 07 + c,
  \ call pixel_addr176
  \ pop bc
  \ ld d,0
  \ ld e,a
  D5 c, E5 c, jpnext, end-code ?)
  \ push de
  \ push hl
  \ _jp_next

  \ doc{
  \
  \ gxy176>scra ( gx gy -- n a )
  \
  \ Return screen address _a_ and pixel position _n_ (0..7) of
  \ pixel coordinates _gx_ (0..255) and _gy_ (0..175).
  \
  \ See: `gxy176>scra_`, `gxy>scra`, `xy>scra`.
  \
  \ }doc

[unneeded] gxy>scra ?( need gxy>scra_

code gxy>scra ( gx gy -- n a )
  E1 c,  D1 c, C5 c, 40 05 + c, 48 03 + c, gxy>scra_ call,
  \ pop hl
  \ pop de
  \ push bc
  \ ld b,l ; b=gy
  \ ld c,e ; c=gx
  \ call pixel_addr
  C1 c, 16 c, 0 c,  58 07 + c, D5 c, E5 c, jpnext, end-code ?)
  \ pop bc
  \ ld d,0
  \ ld e,a
  \ push de
  \ push hl
  \ _jp_next

  \ doc{
  \
  \ gxy>scra ( gx gy -- n a )
  \
  \ Return screen address _a_ and pixel position _n_ (0..7) of
  \ pixel coordinates _gx_ (0..255) and _gy_ (0..191).
  \
  \ See: `gxy>scra_`, `gxy176>scra`, `xy>scra`.
  \
  \ }doc

( plot plot176 )

[unneeded] plot ?( need gxy>scra_

code plot ( gx gy -- )

  D9 c, E1 c, C1 c, 40 05 + c,
    \ exx               ; save Forth IP
    \ pop hl
    \ pop bc            ; C = x coordinate
    \ ld b,l            ; B = y coordinate (0..191)
  ED c, 43 c, 5C7D , gxy>scra_ call,
    \ ld ($5C7D),bc     ; update COORDS
    \ call pixel_addr   ; hl = screen address
    \                   ; A = pixel position in hl (0..7)
  22EC call, D9 c, DD c, 21 c, next , jpnext, end-code ?)
    \ call $22EC        ; ROM PLOT-SUB + 7
    \ exx               ; restore Forth IP
    \ ld ix,next        ; restore ix
    \ _jp_next

  \ doc{
  \
  \ plot ( gx gy -- )
  \
  \ Set a pixel, changing its attribute on the screen and the
  \ current graphic coordinates.  _gx_ is 0..255; _gy_ is
  \ 0..191.
  \
  \ See: `set-pixel`, `plot176`, `xy>gxy`.
  \
  \ }doc

[unneeded] plot176 ?(

code plot176 ( gx gy -- )

  D9 c, E1 c, C1 c, 40 05 + c,
    \ exx ; save Forth IP
    \ pop hl
    \ pop bc            ; C = x coordinate
    \ ld b,l            ; B = y coordinate (0..175)
  22E5 call, D9 c, DD c, 21 c, next , jpnext, end-code ?)
    \ call $22E5        ; ROM PLOT-SUB
    \ exx               ; restore Forth IP
    \ ld ix,next        ; restore Forth IX
    \ _jp_next

  \ doc{
  \
  \ plot176 ( gx gy -- )
  \
  \ Set a pixel, changing its attribute on the screen and the
  \ current graphic coordinates, using only the top 176 pixel
  \ rows of the screen (the lower 16 pixel rows are not used).
  \ _gx_ is 0..255; _gy_ is 0..175.
  \
  \ ``plot176`` is equivalent to Sinclair BASIC's ``PLOT``
  \ command.
  \
  \ WARNING: If parameters are out of range, the ROM will throw
  \ a BASIC error, and the system will crash.
  \
  \ See: `set-pixel176`, `plot`, `xy>gxy176`.
  \
  \ }doc

( set-pixel set-pixel176 )

[unneeded] set-pixel ?( need gxy>scra_ need assembler

code set-pixel ( gx gy -- )

  h pop, d pop, b push, l b ld, e c ld, gxy>scra_ call,
  a b ld, b inc, 1 a ld#,
  rbegin  rrca,  rstep
  m or, a m ld,  \ combine pixel with byte in the screen
  b pop, jpnext, end-code ?)

  \ Credit:
  \
  \ Author of the original code: José Manuel Lazo.
  \ Published on Microhobby, issue 85 (1986-07), page 24:
  \ http://microhobby.org/numero085.htm
  \ http://microhobby.speccy.cz/mhf/085/MH085_24.jpg

  \ doc{
  \
  \ set-pixel ( gx gy -- )
  \
  \ Set a pixel without changing its attribute on the screen or
  \ the current graphic coordinates.  _gx_ is 0..255; _gy_ is
  \ 0..191.
  \
  \ See:  `plot`, `plot176`, `reset-pixel`,
  \ `toggle-pixel`, `xy>gxy`.
  \
  \ }doc

[unneeded] set-pixel176 ?( need assembler need gxy176>scra_

code set-pixel176 ( gx gy -- )

  h pop, d pop, b push, l b ld, e c ld, gxy176>scra_ call,
  a b ld, b inc, 1 a ld#,
  rbegin  rrca,  rstep
  m or, a m ld,  \ combine pixel with byte in the screen
  b pop, jpnext, end-code ?)

  \ Credit:
  \
  \ Author of the original code: José Manuel Lazo.
  \ Published on Microhobby, issue 85 (1986-07), page 24:
  \ http://microhobby.org/numero085.htm
  \ http://microhobby.speccy.cz/mhf/085/MH085_24.jpg

  \ doc{
  \
  \ set-pixel176 ( gx gy -- )
  \
  \ Set a pixel without changing its attribute on the screen or
  \ the current graphic coordinates, and using only the top 176
  \ pixel rows of the screen (the lower 16 pixel rows are not
  \ used).  _gx_ is 0..255; _gy_ is 0..175.
  \
  \ See:  `set-save-pixel176`, `set-pixel`, `plot`,
  \ `plot176`, `reset-pixel`, `toggle-pixel`, `reset-pixel176`,
  \ `toggle-pixel176`, `xy>gxy176`.
  \
  \ }doc

( set-save-pixel176 )

need assembler need gxy176>scra_ need os-coords

code set-save-pixel176 ( gx gy -- )

  h pop, d pop, b push,
  l b ld, e c ld, os-coords bc stp, gxy176>scra_ call,
  a b ld, b inc, 1 a ld#,
  rbegin  rrca,  rstep
  m or, a m ld,  \ combine pixel with byte in the screen
  b pop, jpnext, end-code

  \ Credit:
  \
  \ Author of the original code: José Manuel Lazo.
  \ Published on Microhobby, issue 85 (1986-07), page 24:
  \ http://microhobby.org/numero085.htm
  \ http://microhobby.speccy.cz/mhf/085/MH085_24.jpg

  \ doc{
  \
  \ set-save-pixel176 ( gx gy -- )
  \
  \ Set a pixel without changing its attribute on the screen,
  \ and using only the top 176 pixel rows of the screen (the
  \ lower 16 pixel rows are not used).  _gx_ is 0..255; _gy_ is
  \ 0..175.  ``set-save-pixel176`` updates the graphic
  \ coordinates (contrary to `set-pixel176`).
  \
  \ See:  `set-pixel`, `plot`, `plot176`, `reset-pixel`,
  \ `toggle-pixel`, `reset-pixel176`, `toggle-pixel176`.
  \
  \ }doc

( reset-pixel reset-pixel176 )

[unneeded] reset-pixel ?( need gxy>scra_ need assembler

code reset-pixel ( gx gy -- )

  h pop, d pop, b push, l b ld, e c ld, gxy>scra_ call,
  a b ld, b inc, 1 a ld#, rbegin  rrca,  rstep
  cpl, m and, a m ld,  \ combine pixel with byte in the screen
  b pop, jpnext, end-code ?)

  \ Credit:
  \
  \ Based on code written by José Manuel Lazo,
  \ published on Microhobby, issue 85 (1986-07), page 24:
  \ http://microhobby.org/numero085.htm
  \ http://microhobby.speccy.cz/mhf/085/MH085_24.jpg

  \ doc{
  \
  \ reset-pixel ( gx gy -- )
  \
  \ Reset a pixel without changing its attribute on the screen
  \ or the current graphic coordinates.  _gx_ is 0..255; _gy_
  \ is 0..191.
  \
  \ See: `set-pixel`, `toggle-pixel`, `reset-pixel176`.
  \
  \ }doc

[unneeded] reset-pixel176 ?( need gxy176>scra_ need assembler

code reset-pixel176 ( gx gy -- )

  h pop, d pop, b push, l b ld, e c ld, gxy176>scra_ call,
  a b ld, b inc, 1 a ld#, rbegin  rrca,  rstep
  cpl, m and, a m ld,  \ combine pixel with byte in the screen
  b pop, jpnext, end-code

  \ Credit:
  \
  \ Based on code written by José Manuel Lazo,
  \ published on Microhobby, issue 85 (1986-07), page 24:
  \ http://microhobby.org/numero085.htm
  \ http://microhobby.speccy.cz/mhf/085/MH085_24.jpg

  \ doc{
  \
  \ reset-pixel176 ( gx gy -- )
  \
  \ Reset a pixel without its attribute on the screen or the
  \ current graphic coordinates, and using only the top 176
  \ pixel rows of the screen (the lower 16 pixel rows are not
  \ used).  _gx_ is 0..255; _gy_ is 0..175.
  \
  \ See: `set-pixel176`, `toggle-pixel176`, `reset-pixel`,
  \ `set-pixel`, `toggle-pixel`, `plot`, `plot176`.
  \
  \ }doc

( toggle-pixel toggle-pixel176 )

[unneeded] toggle-pixel ?( need gxy>scra_ need assembler

code toggle-pixel ( gx gy -- )

  h pop, d pop, b push, l b ld, e c ld, gxy>scra_ call,
  a b ld, b inc, 1 a ld#, rbegin  rrca,  rstep
  m xor, a m ld,  \ combine pixel with byte in the screen
  b pop, jpnext, end-code ?)

  \ Credit:
  \
  \ Based on code written by José Manuel Lazo,
  \ published on Microhobby, issue 85 (1986-07), page 24:
  \ http://microhobby.org/numero085.htm
  \ http://microhobby.speccy.cz/mhf/085/MH085_24.jpg

  \ doc{
  \
  \ toggle-pixel ( gx gy -- )
  \
  \ Toggle a pixel without changing its attribute on the screen
  \ or the current graphic coordinates.  _gx_ is 0..255; _gy_
  \ is 0..191.
  \
  \ See: `set-pixel`, `reset-pixel`, `toggle-pixel176`,
  \ `set-pixel176`, `reset-pixel176`, `plot`, `plot176`.
  \
  \ }doc

[unneeded] toggle-pixel176 ?( need gxy176>scra_ need assembler

code toggle-pixel176 ( gx gy -- )

  h pop, d pop, b push, l b ld, e c ld, gxy176>scra_ call,
  a b ld, b inc, 1 a ld#, rbegin  rrca,  rstep
  m xor, a m ld,  \ combine pixel with byte in the screen
  b pop, jpnext, end-code ?)

  \ Credit:
  \
  \ Based on code written by José Manuel Lazo,
  \ published on Microhobby, issue 85 (1986-07), page 24:
  \ http://microhobby.org/numero085.htm
  \ http://microhobby.speccy.cz/mhf/085/MH085_24.jpg

  \ doc{
  \
  \ toggle-pixel176 ( gx gy -- )
  \
  \ Toggle a pixel without changing its attribute on the screen
  \ or the current graphic coordinates, and using only the top
  \ 176 pixel rows of the screen (the lower 16 pixel rows are
  \ not used).  _gx_ is 0..255; _gy_ is 0..175.
  \
  \ See: `toggle-pixel`, `set-pixel`, `reset-pixel`,
  \ `set-pixel176`, `reset-pixel176`, `plot`, `plot176`.
  \
  \ }doc

( get-pixel get-pixel176 )

[unneeded] get-pixel ?( need gxy>scra_ need assembler

code get-pixel ( gx gy -- f )
  h pop, d pop, b push, l b ld, e c ld, gxy>scra_ call,
  \ HL = screen address
  \ A = pixel position in HL
  a b ld, b inc, m a ld,
  rbegin  rlca,  rstep \ rotate to bit 0
  b pop,   \ restore the Forth IP
  1 and#, ' true nz? ?jp, ' false jp, end-code ?)

[unneeded] get-pixel176 ?( need gxy176>scra_ need assembler

code get-pixel176 ( gx gy -- f )
  h pop, d pop, b push, l b ld, e c ld, gxy176>scra_ call,
  \ HL = screen address
  \ A = pixel position in HL
  a b ld, b inc, m a ld,
  rbegin  rlca,  rstep \ rotate to bit 0
  b pop,   \ restore the Forth IP
  1 and#, ' true nz? ?jp, ' false jp, end-code ?)

( pixels fast-pixels slow-pixels )

[unneeded] pixels ?\ defer pixels ( -- n )

  \ doc{
  \
  \ pixels ( -- u )
  \
  \ Return the number _u_ of pixels that are set on the screen.
  \ This is a deferred word set by `fast-pixels` or
  \ `slow-pixels`.
  \
  \ See: `bits`.
  \
  \ }doc

[unneeded] fast-pixels ?( need assembler need pixels

code fast-pixels ( -- n )

  exx, 4000 h ldp#, l b ld, l c ld,
  rbegin  \ byte
    08 d ld#, rbegin  \ bit
      m rrc, c? rif  b incp,  rthen  d dec,
    z? runtil  h incp, h a ld, 58 cp#,
  z? runtil  b push, exx, jpnext, end-code

' fast-pixels ' pixels defer! ?)

  \ 26 bytes used.

  \ Credit:
  \
  \ Original code written by Juan Antonio Paz,
  \ published on Microhobby, issue 170 (1988-05), page 21:
  \ http://microhobby.org/numero170.htm
  \ http://microhobby.speccy.cz/mhf/170/MH170_21.jpg

  \ Original code:
  \
  \ ld hl,16384
  \ ld b,l
  \ ld c,l
  \   byte:
  \ ld d,8
  \   bit:
  \ rrc (hl)
  \ jr nc,next_bit
  \ inc bc
  \   next_bit:
  \ dec d
  \ jr nz,bit
  \ inc hl
  \ ld a,h
  \ cp 88
  \ jr nz,byte
  \ ret

  \ doc{
  \
  \ fast-pixels ( -- n )
  \
  \ Return the number _n_ of pixels set on the screen.
  \ This is the default action of `pixels`.
  \
  \ See: `slow-pixels`, `bits`.
  \
  \ }doc

  \ Slower version of `pixels`.

[unneeded] slow-pixels ?( need bits need pixels

: slow-pixels ( -- n ) 16384 6144 bits ;

' slow-pixels ' pixels defer! ?)

  \ doc{
  \
  \ slow-pixels ( -- n )
  \
  \ Return the number _u_ of pixels that are set on the screen.
  \ This is the alternative action of the deferred word
  \ `pixels`. ``slow-pixels`` simply executes `bits` with the
  \ screen address and length on the stack.
  \
  \ See: `fast-pixels`.
  \
  \ }doc

( scra>attra gxy>attra x>gx y>gy gx>x gy>y )

[unneeded] scra>attra ?(

code scra>attra ( a1 -- a2 )
  E1 c, 7C c, 0F c, 0F c, 0F c, E6 c, 03 c, F6 c, 58 c, 67 c,
    \ pop hl
    \ ld a,h ; fetch high byte $40..$57
    \ rrca
    \ rrca
    \ rrca ; shift bits 3 and 4 to right
    \ and $03 ; range is now 0..2
    \ or $58 ; form correct high byte for third of screen
    \ ld h,a
  E5 c, jpnext, end-code ?)
    \ push hl
    \ _jp_next

  \ Credit:
  \
  \ The code is extracted from the PO-ATTR ROM routine
  \ (at $0BDB).

  \ doc{
  \
  \ scra>attra ( a1 -- a2 )
  \
  \ Convert screen bitmap address _a1_ to its correspondent
  \ attribute address _a2_.
  \
  \ }doc

[unneeded] gxy>attra ?( need gxy>scra need scra>attra

: gxy>attra ( gx gy -- a ) gxy>scra nip scra>attra ; ?)

  \ XXX TODO -- Rewrite in Z80.

  \ doc{
  \
  \ gxy>attra ( gx gy -- a )
  \
  \ Convert pixel coordinates _gx gy_ to their correspondent
  \ attribute address _a_.
  \
  \ }doc

[unneeded] x>gx

?\ need alias need 8* ' 8* alias x>gx ( x -- gx )

  \ doc{
  \
  \ x>gx ( x -- gx )
  \
  \ Convert column _x_ to graphic x coordinate _gx_.
  \
  \ See: `y>gy`, `gx>x`.
  \
  \ }doc

[unneeded] y>gy

?\ need rows need 8* : y>gy ( y -- gy ) rows swap - 8* 1- ;

  \ doc{
  \
  \ y>gy ( y -- gy )
  \
  \ Convert row _y_ to graphic y coordinate _gy_.
  \
  \ See: `x>gx`, `gy>y`.
  \
  \ }doc

[unneeded] gx>x ?\ : gx>x ( gx -- x ) 8 / ;

  \ doc{
  \
  \ gx>x ( gx -- x )
  \
  \ Convert graphic x coordinate _gx_ to column _x_.
  \
  \ See: `gy>y`, `x>gx`.
  \
  \ }doc

[unneeded] gy>y ?\ : gy>y ( gy -- y ) #191 swap - 8 / ;

  \ doc{
  \
  \ gy>y ( gy -- y )
  \
  \ Convert graphic y coordinate _gy_ to row _y_.
  \
  \ See: `gx>x`, `y>gy`.
  \
  \ }doc

( gxy>attra2 )

  \ XXX UNDER DEVELOPMENT 2017-03-02
  \
  \ XXX TODO --  Adapt to 0..191.

need assembler

code gxy>attra2 ( gx gy -- a )

  exx, b pop, h pop, l b ld,
  \ exx                 ; save Forth IP
  \ pop bc              ; C = gy
  \ pop hl              ; L = gx
  \ ld b,l              ; B = gx

  \ Calculate address of attribute for pixel at B (gx), C (gy):

  c a ld, rlca, rlca, a l ld, 03 and, 58 add#, a h ld, l a ld,
  \ ld a,c              ; look at the vertical first
  \ rlca                ; divide by 64
  \ rlca                ; quicker than 6 rrca operations
  \ ld l,a              ; store in l register for now
  \ and 3               ; mask to find segment
  \ add a,88            ; attributes start at 88*256=22528
  \ ld h,a              ; that's our high byte sorted
  \ ld a,l              ; vertical/64 - same as vertical*4
  E0 and#, a l ld, b a ld, rra, rra, rra, 1F and#, l add,
  \ and 224             ; want a multiple of 32
  \ ld l,a              ; vertical element calculated
  \ ld a,b              ; get horizontal position
  \ rra                 ; divide by 8
  \ rra
  \ rra
  \ and 31              ; want result in range 0..31
  \ add a,l             ; add to existing low byte
  a l ld, h push, exx, jpnext, end-code
  \ ld l,a              ; that's the low byte done
  \ push hl             ; result
  \ exx                 ; restore Forth IP
  \ _jp_next

  \ Credit:
  \
  \ Title: How To Write ZX Spectrum Games – Chapter 11
  \ Date: Wed, 02 Oct 2013 13:45:37 +0200
  \ Link: http://chuntey.arjunnair.in/?p=158

  \ XXX TMP -- Test tool:

need gxy>attra
: p1 ( x y -- ) gxy>attra u. ;
: p2 ( x y -- ) gxy>attra1 u. ;
: p ( x y -- ) 2dup p1 p2 ;

  \ ===========================================================
  \ Change log

  \ 2016-10-15: Make `(pixel-addr)` deferred. Rename previous
  \ versions to `slow-(pixel-addr)` and `fast-(pixel-addr)`.
  \ This way the application can choose the version associated
  \ to `(pixel-addr)`, which will be used by other words.
  \
  \ 2016-10-15: Make `pixels` deferred. Rename previous
  \ versions to `slow-pixels` and `fast-pixels`.  This way the
  \ application can choose the version associated to `pixels`.
  \
  \ 2016-10-15: Add `bitmap>attr-addr`, `pixel-attr-addr`.
  \
  \ 2016-12-20: Rename `jppushhl` to `jppushhl,` and `jpnext`
  \ to `jpnext,`, after the change in the kernel.
  \
  \ 2016-12-25: Improve documentation. Write `plot176`.
  \
  \ 2016-12-26: Convert all code words (`fast-(pixel-addr)`,
  \ `set-pixel`, `reset-pixel`, `toggle-pixel` `test-pixel`,
  \ and `pixels`) from the `z80-asm` assembler to the
  \ `z80-asm,` assembler. Add `(pixel-addr176)`,
  \ `pixel-addr176`, `set-pixel176`, `reset-pixel176`,
  \ `toggle-pixel176`, `test-pixel176`, `set-save-pixel176`.
  \
  \ 2017-01-04: Rename `test-pixel` to `get-pixel` and
  \ `test-pixel176` to `get-pixel176`.
  \
  \ 2017-01-05: Update `need z80-asm,` to `need assembler`.
  \
  \ 2017-01-09: Improve documentation with references to
  \ `cursor-addr` and `(cursor-addr)`.
  \
  \ 2017-01-19: Remove remaining `exit` at the end of
  \ conditional interpretation.
  \
  \ 2017-01-28: Compact the code, saving 8 blocks. Improve
  \ documentation.
  \
  \ 2017-01-29: Improve documentation.
  \
  \ 2017-02-17: Update notation "behaviour" to "action".
  \ Update cross references.  Change markup of inline code that
  \ is not a cross reference.
  \
  \ 2017-02-20: Improve documentation.
  \
  \ 2017-02-28: Improve documentation.
  \
  \ 2017-03-13: Add `x>gx`, `y>gy`, `gx>x`, `gy>y`.
  \
  \ 2017-03-13: Improve documentation.  Rename:
  \ `bitmap>attr-addr` to `scra>attra`, `pixel-attr-addr` to
  \ `gxy>attra`, `(pixel-addr176)` to  `gxy176>scra_`
  \ `pixel-addr176` to  `gxy176>scra`, `(pixel-addr)` to
  \ `gxy>scra_`, `fast-(pixel-addr)` to  `fast-gxy>scra_`,
  \ `slow-(pixel-addr)` to  `slow-gxy>scra_`, `pixel-addr` to
  \ `gxy>scra`.  Update references: `(cursor-addr)` to
  \ `xy>scra_`, `cursor-addr` to  `xy>scra`.
  \
  \ 2017-03-27: Update index lines.
  \
  \ 2017-03-29: Use `call,` and `jp,`, which are in the kernel,
  \ instead of opcodes. Improve documentation.
  \
  \ 2017-05-09: Remove `jp pushhlde`. Remove `jppushhl,`.
  \
  \ 2017-05-13: Fix needing of `set-pixel` (code typo).
  \
  \ 2017-07-27: Replace `jp next` with the actual macro
  \ `_jp_next` in Z80 comments.

  \ vim: filetype=soloforth
