  \ graphics.display.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202007282031
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words that make display effects.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( fade-display )

need assembler

code fade-display ( -- )
  b push,
  8 b ld#, rbegin  5AFF h ldp#, halt, halt,
    rbegin
      m a ld, a d ld, %00000111 and#, nz? rif  a dec,   rthen
      a e ld, a d ld, %00111000 and#, nz? rif  8 sub#,  rthen
        e or,  d xor, %00111111 and#, d xor, a m ld,
     h decp, h a ld, 58 cp#, c? runtil
       \ repeat until HL<$5800 (first attribute address)
  rstep b pop, jpnext, end-code

  \ doc{
  \
  \ fade-display ( -- )
  \
  \ Do a screen fade to black, by decrementing the values of
  \ paper and ink in a loop.
  \
  \ See also: `blackout`, `attr-cls`.
  \
  \ }doc

  \ Credit:
  \
  \ Code adapted from a routine written by Pablo Ariza,
  \ published on Microhobby Especial, issue 7 (1987-12), page
  \ 46: <http://microhobby.org/mhes7.htm>.

( invert-display wave-display blackout )

unneeding invert-display ?( need assembler

code invert-display ( -- )

  4000 h ldp#, rbegin   m a ld, cpl, a m ld, h incp,
                        h a ld, 58 cp#,  z? runtil
  jpnext, end-code ?)

  \ Credit:
  \
  \ Code adapted from a routine written by Javier Granadino,
  \ published on Microhobby, issue 133 (1987-06), page 7:
  \ http://microhobby.org/numero133.htm
  \ http://microhobby.speccy.cz/mhf/133/MH133_07.jpg

  \ doc{
  \
  \ invert-display ( -- )
  \
  \ Invert the pixels of the whole screen.
  \
  \ See also: `wave-display`, `fade-display`.
  \
  \ }doc

unneeding wave-display ?( need assembler

code wave-display ( -- )

  b push, 20 b ld#,
  rbegin  57FF h ldp#,
          rbegin   m rrc, h decp, h 6 bit,  z? runtil
  rstep   b pop, jpnext, end-code ?)

  \ Credit:
  \
  \ Code adapted from a routine written by Juan José Rivas,
  \ published on Microhobby, issue 150 (1987-06), page 9:
  \ http://microhobby.org/numero150.htm
  \ http://microhobby.speccy.cz/mhf/150/MH133_09.jpg

  \ doc{
  \
  \ wave-display ( -- )
  \
  \ Modify the screen bitmap with a water effect. At the end
  \ the original image is  restored.
  \
  \ See also: `invert-display`, `fade-display`.
  \
  \ }doc

unneeding blackout ?(

code blackout ( -- )
  D9 c, 21 c, 4000 , 11 c, 4001 , 01 c, 1B00 , 75 c,
  \ exx         ; save Forth IP
  \ ld hl,$4000 ; start of screen
  \ ld de,$4001 ; destination
  \ ld bc,$1B00 ; count: bitmap and attributes
  \ ld (hl),l   ; zero
  ED c, B0 c, D9 c, jpnext, end-code ?)
  \ ldir        ; fill
  \ exx         ; restore Forth IP
  \ _jp_next

  \ doc{
  \
  \ blackout ( -- )
  \
  \ Erase the screen (bitmap and the attributes) with zeros.
  \
  \ See also: `fade-display`, `cls`, `attr-cls`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-12-20: Rename `jpnext` to `jpnext,` after the change
  \ in the kernel.
  \
  \ 2016-12-30: Compact the code, saving one block.
  \
  \ 2017-01-02: Convert `inverted` from `z80-asm` to `z80-asm,`
  \ and fix it.
  \
  \ 2017-01-05: Convert `water` from `z80-asm` to `z80-asm,`.
  \ Improve documentation.
  \
  \ 2017-01-05: Update `need z80-asm,` to `need assembler`.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-30: Add `blackout`.
  \
  \ 2017-02-03: Improve documentation.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-03-15: Rename the module file. Rename `inverted` to
  \ `invert-display`, `fade` to `fade-display` and `water` to
  \ `wave-display`. The previous names were too generic.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.

  \ vim: filetype=soloforth
