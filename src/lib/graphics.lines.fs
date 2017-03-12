  \ graphics.lines.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702221550
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words to draw lines.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( rdraw176 x1 incx y1 incy y )

[unneeded] rdraw176 ?( need assembler

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
  \ rdraw176 ( gx gy -- )
  \
  \ Draw a line relative _gx gy_ to the current coordinates,
  \ using only the top 176 pixel rows of the screen (the lower
  \ 16 pixel rows are not used). _gx_ is 0..255; _gy_ is
  \ 0..175.
  \
  \ This word is equivalent to Sinclair BASIC's DRAW command.
  \
  \ See also: `adraw176`, `rdraw`.
  \
  \ }doc

[unneeded] x1 [unneeded] incx
[unneeded] y1 [unneeded] incy and and and

?\ 2variable x1  2variable incx  2variable y1  2variable incy

  \ doc{
  \
  \ x1 ( -- a )
  \
  \ A double-cell variable used by `adraw176` and `aline176`.
  \
  \ See also: `y1`, `incx`, `incy`.
  \
  \ }doc

  \ doc{
  \
  \ y1 ( -- a )
  \
  \ A double-cell variable used by `adraw176` and `aline176`.
  \
  \ See also: `x1`, `incx`, `incy`.
  \
  \ }doc

  \ doc{
  \
  \ incx ( -- a )
  \
  \ A double-cell variable used by `adraw176` and `aline176`.
  \
  \ See also: `incy`, `x1`, `y1`.
  \
  \ }doc

  \ doc{
  \
  \ incy ( -- a )
  \
  \ A double-cell variable used by `adraw176` and `aline176`.
  \
  \ See also: `incx`, `x1`, `y1`.
  \
  \ }doc

( rdraw )

  \ XXX UNDER DEVELOPMENT -- not usable yet

need assembler need l: need os-coords need (pixel-addr)

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

  c l ld, d push, a xor, a e ld, 00 l# jr,  rthen
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

  00 l: b h ld, b a ld, rra,
    \ dl_larger:
    \  ld      h,b
    \  ld      a,b
    \  rra

-->

( rdraw )

  rbegin  l add, 01 l# c? ?jr, h cp, 02 l# c? ?jr,
    \ d_l_loop:
    \  add     a,l
    \  jr      c,d_l_diag
    \  cp      h
    \  jr      c,d_l_hr_vt

  01 l: h sub, a c ld, exx, b pop, b push, 03 l# jr,
    \ d_l_diag:
    \  sub     h
    \  ld      c,a
    \  exx
    \  pop     bc
    \  push    bc
    \  jr      d_l_step

  02 l: a c ld, d push, exx, b pop,
    \ d_l_hr_vt:
    \  ld      c,a
    \  push    de
    \  exx
    \  pop     bc

  03 l:
  os-coords h ftp, b a ld, h add, a b ld, c a ld, a inc, l add,
  05 l# c? ?jr,
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
  04 l: a dec, a c ld,
    \  dec     a
    \  ld      c,a

  (pixel-addr) call, 22EC 07 + call, exx, c a ld, rstep
    \  call    pixel_addr ; alternative routine for 0..191 gy
    \  call    $22EC ; alternative entry to PLOT-SUB ROM routine
    \  exx
    \  ld      a,c
    \  djnz    d_l_loop

  d pop, ret,  05 l: 04 l# z? ?jr, b pop, jpnext, end-code
    \  pop     de
    \  ret
    \ d_l_range:
    \  jr      z,d_l_plot
    \  pop bc ; restore Forth IP
    \  jp next ; jp (ix)

  \ Credit:
  \
  \ This word is a modified version of the DRAW-LINE ROM
  \ routine.

  \ doc{
  \
  \ rdraw ( gx gy -- )
  \
  \ REMARK: This word is under development.
  \
  \ Draw a line relative _gx gy_ to the current coordinates.
  \ _gx_ is 0..255; _gy_ is 0..191.
  \
  \ See also: `rdraw176`, `adraw176`.
  \
  \ }doc

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
  \ adraw176 ( gx gy -- )
  \
  \ Draw a line from the current coordinates to the given
  \ absolute coordinates _gx gy_, using only the top 176 pixel
  \ rows of the screen (the lower 16 pixel rows are not used).
  \ _gx_ is 0..255; _gy_ is 0..175.
  \
  \ See also: `rdraw176`, `rline176`.
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
  \ aline176 ( gx gy -- )
  \
  \ Draw a line from the current coordinates to the given
  \ absolute coordinates _gx gy_, using only the top 176 pixel
  \ rows of the screen (the lower 16 pixel rows are not used)
  \ and preserving the current attributes of the screen.
  \ _gx_ is 0..255; _gy_ is 0..175. This word is faster than
  \ `adraw176`.
  \
  \ See also: `rdraw176`.
  \
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

  \ vim: filetype=soloforth
