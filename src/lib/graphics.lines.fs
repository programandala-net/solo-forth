  \ graphics.lines.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202005241405
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words to draw lines.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018,
  \ 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( rdraw176 x1 incx y1 incy y )

unneeding rdraw176 ?( need assembler

code rdraw176 ( gx gy -- )

  h pop, d pop, b push, d b ldp,
  \ HL = gy
  \ BC = gx
  1 e ld#,
  b 7 bit,  \ negative gx?
  nz? rif  c a ld, neg, -1 e ld#, a c ld,  rthen  \ negative gx

  l b ld,   \ B = gy
  1 d ld#,
  h 7 bit,  \ negative gy?
  nz? rif  b a ld, neg, -1 d ld#, a b ld,  rthen  \ negative gy

  \ XXX TODO -- factor the above part, it's common to `rdraw`

  \ B = gy
  \ D = sign of gy (1/-1)
  \ C = gx
  \ E = sign of gx (1/-1)

  24BA call, \ alternative entry to the DRAW-LINE ROM routine

  b pop, jpnext, end-code ?)

  \ Credit:
  \
  \ Code adapted from Spectrum Forth-83's `DRAW`.

  \ doc{
  \
  \ rdraw176 ( gx gy -- ) "r-draw-176"
  \
  \ Draw a line relative _gx gy_ to the current coordinates,
  \ using only the top 176 pixel rows of the screen (the lower
  \ 16 pixel rows are not used). _gx_ is 0..255; _gy_ is
  \ 0..175.
  \
  \ ``rdraw176`` is equivalent to Sinclair BASIC's ``DRAW``
  \ command.
  \
  \ See: `adraw176`.
  \
  \ }doc

unneeding x1 unneeding incx
unneeding y1 unneeding incy and and and ?( need 2variable

2variable x1  2variable incx  2variable y1  2variable incy ?)

  \ doc{
  \
  \ x1 ( -- a ) "x-one"
  \
  \ A `2variable` used by `adraw176` and `aline176`.
  \
  \ See: `y1`, `incx`, `incy`.
  \
  \ }doc

  \ doc{
  \
  \ y1 ( -- a ) "y-one"
  \
  \ A `2variable` used by `adraw176` and `aline176`.
  \
  \ See: `x1`, `incx`, `incy`.
  \
  \ }doc

  \ doc{
  \
  \ incx ( -- a ) "inc-x"
  \
  \ A `2variable` used by `adraw176` and `aline176`.
  \
  \ See: `incy`, `x1`, `y1`.
  \
  \ }doc

  \ doc{
  \
  \ incy ( -- a ) "ink-y"
  \
  \ A `2variable` used by `adraw176` and `aline176`.
  \
  \ See: `incx`, `x1`, `y1`.
  \
  \ }doc

( rdraw )

  \ XXX UNDER DEVELOPMENT -- not usable yet

need os-coords need gxy>scra_
need assembler also assembler need l: previous

code rdraw ( gx gy -- )

  h pop, d pop, b push, d b ldp,
  \ HL = gy
  \ BC = gx
  1 e ld#,  \ default positive sign of gx
  b 7 bit,  \ negative gx?
  nz? rif  c a ld, neg, -1 e ld#, a c ld,  rthen  \ negative gx

  l b ld,   \ B = gy
  1 d ld#,  \ default positive sign of gy
  h 7 bit,  \ negative gy?
  nz? rif  b a ld, neg, -1 d ld#, a b ld,  rthen  \ negative gy

  \ XXX TODO -- factor the above part, it's common to
  \ `rdraw176`

  \ B = gy
  \ D = sign of gy (1/-1)
  \ C = gx
  \ E = sign of gx (1/-1)

  \ Modified copy of DRAW-LINE ROM routine from address $24BA:

  c a ld, b cp, c? rif
    \  ld      a,c
    \  cp      b ; is gx
    \  jr      nc,dl_x_ge_y
    \ ; gy is greater than gx

  c l ld, d push, a xor, a e ld, #0 rl# jr,  rthen
    \  ld      l,c
    \  push    de
    \  xor     a
    \  ld      e,a
    \  jr      dl_larger

    \ dl_x_ge_y:
    \ ; gy is not greater than gx
  c or, z? ?ret, b l ld, c b ld, d push, 00 d ld#,
    \  or      c
    \  ret     z
    \  ld      l,b
    \  ld      b,c
    \  push    de
    \  ld      d,$00

  #0 l: b h ld, b a ld, rra, -->
    \ dl_larger:
    \  ld      h,b
    \  ld      a,b
    \  rra

( rdraw )

  rbegin  l add, #1 rl# c? ?jr, h cp, #2 rl# c? ?jr,
    \ d_l_loop:
    \  add     a,l
    \  jr      c,d_l_diag
    \  cp      h
    \  jr      c,d_l_hr_vt

  #1 l: h sub, a c ld, exx, b pop, b push, #3 rl# jr,
    \ d_l_diag:
    \  sub     h
    \  ld      c,a
    \  exx
    \  pop     bc
    \  push    bc
    \  jr      d_l_step

  #2 l: a c ld, d push, exx, b pop,
    \ d_l_hr_vt:
    \  ld      c,a
    \  push    de
    \  exx
    \  pop     bc

  #3 l:
  os-coords h ftp, b a ld, h add, a b ld, c a ld, a inc, l add,
  #5 rl# c? ?jr,
  \ XXX z? ?jr, ; XXX TODO -- adapt, integer out of range
    \ d_l_step:
    \  ld      hl,($5c7d) ; coords
    \  ld      a,b
    \  add     a,h
    \  ld      b,a
    \  ld      a,c
    \  inc     a
    \  add     a,l
    \  jr      c,d_l_range
    \  jr      z,report_bc ; XXX TODO -- adapt, integer out of range

    \ d_l_plot:
  #4 l: a dec, a c ld,
    \  dec     a
    \  ld      c,a

  gxy>scra_ call, 22EC 07 + call, exx, c a ld, rstep
    \  call    pixel_addr ; alternative routine for 0..191 gy
    \  call    $22EC ; alternative entry to PLOT-SUB ROM routine
    \  exx
    \  ld      a,c
    \  djnz    d_l_loop

  d pop, ret,  #5 l: #4 rl# z? ?jr, b pop, jpnext, end-code
    \  pop     de
    \  ret
    \ d_l_range:
    \  jr      z,d_l_plot
    \  pop bc ; restore Forth IP
    \  _jp_next

  \ Credit:
  \
  \ `rdraw` is a modified version of the DRAW-LINE ROM
  \ routine.

  \
  \ rdraw ( gx gy -- )
  \
  \ WARNING: ``rdraw`` is under development, cannot be used
  \ yet. See the source code for details.
  \
  \ Draw a line relative _gx gy_ to the current coordinates.
  \ _gx_ is 0..255; _gy_ is 0..191.
  \
  \ NOTE: ``rdraw`` is a modified version of the DRAW-LINE ROM
  \ routine.
  \
  \ See: `rdraw176`, `adraw176`.
  \

( adraw176 )

need plot176 need os-coordx need os-coordy

need x1 need incx need y1 need incy

: adraw176 ( gx gy -- )

  [ os-coordy ] literal c@ dup 0 swap y1 2! - dup abs rot
  \ ( +-ydiff ydiff x )

  [ os-coordx ] literal c@ dup 0 swap x1 2! - dup abs rot
  \ ( +-ydiff +-xdiff xdiff ydiff )

  max >r dup 0<  \ negative xdiff?
  if    abs 0 swap r@ ud/mod dnegate
  else  0 swap r@ ud/mod  then

  incx 2! drop dup 0<  \ negative ydiff?
  if    abs 0 swap r@ ud/mod dnegate
  else  0 swap r@ ud/mod  then

  incy 2! drop r> 1+ 0
  do  x1 @ y1 @ plot176
      x1 2@ incx 2@ d+ x1 2!
      y1 2@ incy 2@ d+ y1 2!  loop ;

  \ Credit:
  \
  \ Code adapted from Abersoft Forth's `DRAW`.

  \ doc{
  \
  \ adraw176 ( gx gy -- ) "a-draw-176"
  \
  \ Draw a line from the current coordinates to the given
  \ absolute coordinates _gx gy_, using only the top 176 pixel
  \ rows of the screen (the lower 16 pixel rows are not used).
  \ _gx_ is 0..255; _gy_ is 0..175.
  \
  \ See: `rdraw176`.
  \
  \ }doc

( aline176 )

need set-save-pixel176 need os-coordx need os-coordy

need x1 need incx need y1 need incy

: aline176 ( gx gy -- )

  [ os-coordy ] literal c@ dup 0 swap y1 2! - dup abs rot
  \ ( +-ydiff ydiff x )

  [ os-coordx ] literal c@ dup 0 swap x1 2! - dup abs rot
  \ ( +-ydiff +-xdiff xdiff ydiff )

  max >r dup 0<  \ negative xdiff?
  if    abs 0 swap r@ ud/mod dnegate
  else  0 swap r@ ud/mod  then

  incx 2! drop dup 0<  \ negative ydiff?
  if    abs 0 swap r@ ud/mod dnegate
  else  0 swap r@ ud/mod  then

  incy 2! drop r> 1+ 0
  do  x1 @ y1 @ set-save-pixel176
      x1 2@ incx 2@ d+ x1 2!
      y1 2@ incy 2@ d+ y1 2!  loop ;

  \ Credit:
  \
  \ Code adapted from Abersoft Forth's `DRAW`.

  \ doc{
  \
  \ aline176 ( gx gy -- ) "a-line-176"
  \
  \ Draw a line from the current coordinates to the given
  \ absolute coordinates _gx gy_, using only the top 176 pixel
  \ rows of the screen (the lower 16 pixel rows are not used)
  \ and preserving the current attributes of the screen.  _gx_
  \ is 0..255; _gy_ is 0..175.
  \
  \ ``aline176`` is faster than `adraw176`.
  \
  \ See: `rdraw176`.
  \
  \ }doc

( orthodraw )

need assembler need gxy>scra_

code orthodraw ( gx gy gxinc gyinc len -- )

  exx, d pop, e a ld,
       d pop, e b ld, d pop, e c ld,
       d pop, e h ld, d pop, e l ld,
    \   exx               ; save Forth IP
    \   pop de
    \   ld a,e            ; A = len
    \   pop de
    \   ld b,e            ; B = gyinc
    \   pop de
    \   ld c,e            ; C = gxinc
    \   pop de
    \   ld h,e            ; H = gy
    \   pop de
    \   ld l,e            ; L = gx

  rbegin a push, h push, b push, h b ldp,
    \ begin:
    \   push af           ; save registers
    \   push hl           ;
    \   push bc           ;
    \   ld   b,h          ; B = gy
    \   ld   c,l          ; C = gx
  5C7D b stp, gxy>scra_ call, 22EC call,
    \ ld ($5C7D),bc     ; update COORDS
    \ call pixel_addr   ; HL = screen address
    \                   ; A = pixel position in HL (0..7)
    \ call $22EC        ; ROM PLOT-SUB + 7

  b pop, h pop, h a ld, b add, a h ld, l a ld, c add, a l ld,
    \   pop  bc           ; increments
    \   pop  hl           ; coordinates
    \   ld a,h
    \   add a,b
    \   ld h,a            ; update coordinate gx
    \   ld a,l
    \   add a,c
    \   ld l,a            ; update coordinate gy
  a pop, a dec, z? runtil
    \   pop  af           ;
    \   dec  a            ;
    \   jr   nz,begin     ; repeat for all pixels
  exx, next ix ldp#, jpnext, end-code
    \ exx               ; restore Forth IP
    \ ld ix,next        ; restore ix
    \ _jp_next

  \ doc{
  \
  \ orthodraw ( gx gy gxinc gyinc len -- )
  \
  \ Draw a line formed by _len_ pixels, starting from _gx gy_
  \ and using _gxinc gyinc_ as increments to calculate the
  \ coordinates of every next pixel.
  \
  \ The status of `inverse` and `overprint` modes are obeyed;
  \ the screen attributes and the system graphic coordinates
  \ are updated.  That's what makes ``orthodraw`` much slower
  \ than `ortholine`.
  \
  \ See: `adraw176`, `rdraw176`.
  \
  \ }doc

  \ Credit:
  \
  \ Based on the following code, from the ZX Spectrum 128 ROM 0
  \ (disassembled by Matthew Wilson, Andrew Owen, Geoff
  \ Wearmouth, Rui Tunes and Paul Farrow):

  \ ; -----------
  \ ; Plot a Line
  \ ; -----------
  \ ; Entry: H=Line pixel coordinate.
  \ ;        L=Column pixel coordinate.
  \ ;        B=Offset to line pixel coordinate ($FF, $00 or $01).
  \ ;        C=Offset to column pixel coordinate ($FF, $00 or $01).
  \ ;        A=number of pixels to plot.

  \ L3719:  PUSH AF           ; Save registers.
  \         PUSH HL           ;
  \         PUSH DE           ;
  \         PUSH BC           ;

  \         LD   B,H          ; Coordinates to BC.
  \         LD   C,L          ;
  \         RST  28H          ;
  \         DEFW PLOT_SUB+4   ; $22E9. Plot pixel

  \         POP  BC           ; Restore registers.
  \         POP  DE           ;
  \         POP  HL           ;
  \         POP  AF           ;

  \         ADD  HL,BC        ; Determine coordinates of next pixel.
  \         DEC  A            ;
  \         JR   NZ,L3719     ; Repeat for all pixels.

  \         RET               ;

( ortholine )

need assembler need gxy>scra_

code ortholine ( gx gy gxinc gyinc len -- )

  exx, d pop, e a ld,
       d pop, e b ld, d pop, e c ld,
       d pop, e h ld, d pop, e l ld,
    \   exx               ; save Forth IP
    \   pop de
    \   ld a,e            ; A = len
    \   pop de
    \   ld b,e            ; B = gyinc
    \   pop de
    \   ld c,e            ; C = gxinc
    \   pop de
    \   ld h,e            ; H = gy
    \   pop de
    \   ld l,e            ; L = gx

  rbegin a push, h push, b push, h b ldp,
    \ begin:
    \   push af           ; save registers
    \   push hl           ;
    \   push bc           ;
    \   ld   b,h          ; B = gy
    \   ld   c,l          ; C = gx

  gxy>scra_ call, a b ld, b inc, 1 a ld#, rbegin rrca, rstep
    \   call pixel_addr   ; HL = screen address
    \                     ; A = pixel position in HL (0..7)
    \   ld b,a
    \   inc b
    \   ld a,1
    \ rotate:
    \   rrca
    \   djnz rotate
  m or, a m ld,
    \   or (hl)           ; combine with byte in the screen
    \   ld (hl),a         ; update screen

  b pop, h pop, h a ld, b add, a h ld, l a ld, c add, a l ld,
    \   pop  bc           ; restore registers
    \   pop  hl
    \   ld a,h
    \   add a,b
    \   ld h,a            ; update coordinate gx
    \   ld a,l
    \   add a,c
    \   ld l,a            ; update coordinate gy
  a pop, a dec, z? runtil
    \   pop  af           ;
    \   dec  a            ;
    \   jr   nz,begin     ; repeat for all pixels
  exx, next ix ldp#, jpnext, end-code
    \ exx               ; restore Forth IP
    \ ld ix,next        ; restore ix
    \ _jp_next

  \ doc{
  \
  \ ortholine ( gx gy gxinc gyinc len -- )
  \
  \ Draw a line formed by _len_ pixels, starting from _gx gy_
  \ and using _gxinc gyinc_ as increments to calculate the
  \ coordinates of every next pixel.
  \
  \ The status of `inverse` and `overprint` modes is ignored;
  \ the attributes of the screen are not modified; and the
  \ system graphic coordinates are not updated. That's what
  \ makes ``ortholine`` almost twice faster than `orthodraw`.
  \
  \ }doc

  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-05-07: Make block titles compatible with `indexer`.
  \
  \ 2016-12-20: Rename `jpnext` to `jpnext,` after the change
  \ in the kernel.
  \
  \ 2016-12-25: Rename `rdraw` to `rdraw176`, and `adraw` to
  \ `adraw176`. Improve documentation. Convert `rdraw176` to
  \ the `z80-asm,` assembler. First steps to write `rdraw` with
  \ a modified version of the ROM routine.
  \
  \ 2016-12-26: Add `aline176`.
  \
  \ 2017-01-05: Update `need z80-asm,` to `need assembler`.
  \
  \ 2017-01-12: Fix and improve documentation.
  \
  \ 2017-01-28: Improve documentation.
  \
  \ 2017-02-03: Compact the code, saving one block.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-03-13: Improve documentation.  Update name:
  \ `(pixel-addr)` to `gxy>scra_`.
  \
  \ 2017-03-21: Adapt to the new implementation of assembler
  \ labels.
  \
  \ 2017-03-25: Change the notation of assembler label numbers.
  \
  \ 2017-03-29: Add `orthodraw` and `ortholine`.
  \
  \ 2017-07-27: Replace `jp next` with the actual macro
  \ `_jp_next` in Z80 comments.
  \
  \ 2017-12-10: Update to `a push,` and `a pop,`, after the
  \ change in the assembler.
  \
  \ 2018-02-15: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-06-04: Link `2variable` in documentation.
  \
  \ 2020-05-04: Remove cross references to `rline176`, which is
  \ not implemented yet.
  \
  \ 2020-05-05: Improve the hidden documentation of `rdraw`,
  \ which is an unfinished word, and remove the cross
  \ references to it.
  \
  \ 2020-05-19: Update: `2variable` has been moved to the
  \ library.
  \
  \ 2020-05-24: Fix typo.

  \ vim: filetype=soloforth
