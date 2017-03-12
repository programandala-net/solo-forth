  \ graphics.circle.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201703041850
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Implementation of a configurable fast `circle`.

  \ ===========================================================
  \ Authors

  \ rtunes
  \ (https://worldofspectrum.org/forums/profile/259/rtunes),
  \ 2008.  Published in
  \ <http://worldofspectrum.org/forums/discussion/22058/bresenhams-circle-algorithm/>.
  \
  \ Marcos Cruz (programandala.net) adapted the code to Solo
  \ Forth, 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( uncolored-circle-pixel colored-circle-pixel )

[unneeded] uncolored-circle-pixel ?(

need assembler need (pixel-addr)

create uncolored-circle-pixel ( -- a ) asm
  h push, b push, d push, (pixel-addr) call,
  \ HL = screen address ; A = pixel position in hl (0..7)
  a b ld, b inc, 1 a ld#, rbegin  rrca,  rstep  m or, a m ld,
  d pop, b pop, h pop, ret, end-asm ?)

  \ doc{
  \
  \ uncolored-circle-pixel ( -- a )
  \
  \ _a_ is the address of a subroutine that `circle` can use to
  \ draw its pixels.  This routine sets a pixel without
  \ changing its color attributes on the screen (like
  \ `set-pixel`).  Therefore it's faster than its alternative
  \ `colored-circle-pixel` (0.6 its execution speed).
  \
  \ `set-circle-pixel` sets the routine used by `circle`. See
  \ the requirements of such routine in the documentation of
  \ `circle-pixel`.
  \
  \ }doc

[unneeded] colored-circle-pixel ?(

need assembler need (pixel-addr)

create colored-circle-pixel ( -- a ) asm
  h push, b push, d push, (pixel-addr) call,
  \ HL = screen address ; A = pixel position in hl (0..7)
  22EC call, d pop, b pop, h pop, ret, end-asm ?)
  \ Note: $22EC is an alternative entry to ROM PLOT_SUB: +7 bytes

  \ doc{
  \
  \ colored-circle-pixel ( -- a )
  \
  \ _a_ is the address of a subroutine that `circle` can use to
  \ draw its pixels.  This routine sets a pixel, changing its
  \ color attributes on the screen (like `plot`).  Therefore
  \ it's slower than its alternative `uncolored-circle-pixel`
  \ (1.64 its execution speed).
  \
  \ `set-circle-pixel` sets the routine used by `circle`.  See
  \ the requirements of such routine in the documentation of
  \ `circle-pixel`.
  \
  \ }doc

( circle-pixel set-circle-pixel )

[unneeded] circle-pixel

?\ create circle-pixel ( -- a ) asm noop_ jp, end-asm

  \ doc{
  \
  \ circle-pixel ( -- a )
  \
  \ _a_ is the address of a subroutine used by `circle` to set
  \ its pixels.  This routine does a jump to the actual
  \ routine, which by default does nothing. The desired routine
  \ must be set by `set-circle`.
  \
  \ Also any routine provided by the application can be used as
  \ the action of `circle-pixel`, provided the following
  \ requirements:

  \ - HL, DE and BC must be preserved.
  \ - Input parameters: B=gy and C=gx.

  \ }doc

[unneeded] set-circle-pixel ?( need circle-pixel

: set-circle-pixel ( a -- )
  [ circle-pixel 1+ ] literal ! ; ?)

  \ doc{
  \
  \ set-circle-pixel ( a -- )
  \
  \ Set the address _a_ of the routine `circle-pixel` will jump
  \ to. This word is used to make `circle-pixel` jump to
  \ `colored-circle-pixel`, `uncolored-circle-pixel`, or other
  \ routine provided by the application, therefore configuring
  \ `circle`.
  \
  \ }doc

( circle )

need assembler need circle-pixel need set-circle-pixel

code circle ( gx gy b -- )

  \ ;*************************************
  \ ;*******Setup of parameters***********
  \ ;*************************************

  h pop, l a ld, d pop, h pop, l d ld, b push, 0 h ld#, a l ld,
    \ pop hl
    \ ld a,l            ; A = radius
    \ pop de            ; E = gy
    \ pop hl
    \ ld d,l            ; D = gx
    \ push bc           ; Save Forth IP
    \ ld h,0            ; H = x -- init to 0
    \ ld l,a            ; L = y -- init to radius
  exx, cpl, a c ld, FF b ld#, b incp, 1 h ldp#, b addp, exde,
    \ exx
    \ cpl
    \ ld c,a
    \ ld b,$ff
    \ inc bc            ; BC' = -radius
    \ ld hl,1
    \ add hl,bc
    \ ex de,hl          ; DE' = f = 1-radius ; f error control
  c rl, b rl, 5 h ldp#, b addp, 3 b ldp#, exx,
    \ rl c
    \ rl b              ; -2*radius
    \ ld hl,5
    \ add hl,bc         ; HL' = ddfy = 5-2*r
    \ ld bc,3           ; BC' = ddfx = 3
    \ exx

  \ ;*************************************
  \ ;*******Main circle procedure*********
  \ ;*************************************

  \ begin:

  \ ;*******Set 8 pixels, one for each circle's octant*********

  rbegin  d a ld, h add, a c ld, e a ld, l add, a b ld,
  circle-pixel call,
    \ ld a,d            ; point #1
    \ add a,h
    \ ld c,a
    \ ld a,e
    \ add a,l
    \ ld b,a
    \ call circle_plot

  e a ld, l sub, a b ld, circle-pixel call,
    \ ld a,e            ; point #2
    \ sub l
    \ ld b,a
    \ call circle_plot

  d a ld, h sub, a c ld, circle-pixel call,
    \ ld a,d            ; point #4
    \ sub h
    \ ld c,a
    \ call circle_plot

  e a ld, l add, a b ld, circle-pixel call,
    \ ld a,e            ; point #3
    \ add a,l
    \ ld b,a
    \ call circle_plot

-->

( circle )

  d a ld, l add, a c ld, e a ld, h add, a b ld,
  circle-pixel call,
    \ ld a,d            ; point #5
    \ add a,l
    \ ld c,a
    \ ld a,e
    \ add a,h
    \ ld b,a
    \ call circle_plot

  e a ld, h sub, a b ld, circle-pixel call,
    \ ld a,e            ; point #6
    \ sub h
    \ ld b,a
    \ call circle_plot

  d a ld, l sub, a c ld, circle-pixel call,
    \ ld a,d            ; point #8
    \ sub l
    \ ld c,a
    \ call circle_plot

  e a ld, h add, a b ld, circle-pixel call,
    \ ld a,e            ; point #7
    \ add a,h
    \ ld b,a
    \ call circle_plot

  \ ;********main logic***********

  h a ld, l cp, c? rwhile  \ y x > while
  \ control:
    \ ld a,h            ; H = x
    \ cp l              ; L = y
    \ jr nc,end_circle  ; while (y>x)

  exx, d 7 bit, z? rif
    \ exx
    \ bit 7,d           ; if f>0
    \ jr nz,fneg

  exde, d addp, exde, h incp, h incp, exx, l dec,
    \ ex de,hl
    \ add hl,de
    \ ex de,hl
    \ inc hl            ; HL' = ddfy
    \ inc hl
    \ exx
    \ dec l
    \ jr fneg2

  relse  exde, b addp, exde, exx,  rthen
  \ fneg:
    \ ex de,hl
    \ add hl,bc
    \ ex de,hl
    \ exx

  exx, b incp, b incp, h incp, h incp, exx, h inc, rrepeat
  \ fneg2:
    \ exx
    \ inc bc            ; BC' = ddfx
    \ inc bc
    \ inc hl
    \ inc hl
    \ exx
    \ inc h             ; H = x
    \ jr begin          ; repeat

  \ end_circle:
  exx, b pop, next ix ldp#, jpnext, end-code

    \ exx
    \ pop bc            ; restore Forth IP
    \ ld ix,next        ; restore Forth IX
    \ _jp_next

  \ Credit:
  \
  \ <http://worldofspectrum.org/forums/discussion/22058/bresenhams-circle-algorithm/>.
  \ Code adapted from:
  \ <http://worldofspectrum.org/forums/discussion/22058/bresenhams-circle-algorithm/>,
  \ which was written and published by rtunes
  \ (https://worldofspectrum.org/forums/profile/259/rtunes),
  \ 2008.

  \ x^2+y^2=r^2 represents the real variable equation of a
  \ circle which is to be plotted using a grid of discrete
  \ pixels where each pixel has integer coordinates.

  \ Note: original ROM circles are slightly displaced to the
  \ right as noticed.

  \ Note: 125 B used.

  \ doc{
  \
  \ circle ( gx gy b -- )
  \
  \ Draw a circle at center coordinates _gx gy_ and with radius
  \ _b_.
  \
  \ This word does not use the ROM routine and it's much
  \ faster.
  \
  \ This word does no error checking: the whole circle must fit
  \ the screen. Otherwise, strange things will happen when
  \ other parts of the screen bitmap, the screen attributes or
  \ even the system variables will be altered.
  \
  \ Note: By default this word does nothing. Its factor routine
  \ `circle-pixel` must be configured first with
  \ `set-circle-pixel`, in order to choose the routine that
  \ creates the pixels of the circle: `uncolored-circle-pixel`,
  \ `colored-circle-pixel` or a routine provided by the
  \ application.
  \
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2017-01-02: Convert from `z80-asm` to `z80-asm,`.
  \
  \ 2017-01-05: Update `need z80-asm,` to `need assembler`.
  \
  \ 2017-01-28: Finish the conversion of the original code of
  \ `circle`.  Document the word.
  \
  \ 2017-01-29: Make the pixel routine configurable with a
  \ deferred word and provide code for color circles and faster
  \ colorless circles.
  \
  \ 2017-01-30: Fix and rewrite the configuration method, using
  \ an intermediate routine that does a jump.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-03-04: Update naming convention of Z80 routines, after
  \ the changes in the kernel.

  \ vim: filetype=soloforth

