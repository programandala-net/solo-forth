  \ display.gigatype.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202007282031
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Gigatype printing routine.

  \ ===========================================================
  \ Authors

  \ The original routine, called GigaText, was written by an
  \ anonymous author, and published in the Outlet magazine,
  \ issue 132 (1998-08).
  \
  \ Marcos Cruz (programandala.net) disassembled and adapted it
  \ to Solo Forth, 2017, 2018, 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( gigatype )

hex

here 01 c, 05 c, 03 c, 05 c, 08 c, 03 c, 0A c, 03 c, 0C c,
     03 c, 8A c, 03 c, 91 c, 01 c, 93 c, 01 c, 12 c, 01 c,
     FF c,
  \ Style 7 data

here 01 c, 05 c, 03 c, 05 c, 08 c, 03 c, 0A c, 03 c, 0C c,
     03 c, 8A c, 03 c, 91 c, 01 c, 93 c, 01 c, FF c,
  \ Style 6 data

here 24 c, 01 c, 9B c, 01 c, 01 c, 03 c, 08 c, 01 c, 0A c,
     01 c, 89 c, 01 c, FF c,
  \ Style 5 data

here 24 c, 01 c, 9B c, 01 c, 01 c, 03 c, 08 c, 01 c, 0A c,
     01 c, FF c,
  \ Style 4 data

here 09 c, 03 c, 10 c, 01 c, 12 c, 01 c, 80 c, 01 c, FF c,
  \ Style 3 data

here 09 c, 01 c, 80 c, 01 c, FF c,
  \ Style 2 data

here 01 c, 03 c, 08 c, 01 c, 0A c, 01 c, 89 c, 01 c, FF c,
  \ Style 1 data

here 01 c, 03 c, 08 c, 01 c, 0A c, 01 c, FF c,
  \ Style 0 data

decimal  create gigatype-styles , , , , , , , , -->

( gigatype )

need os-chars need os-attr-t need assembler
also assembler need l: #11 max-labels c! previous

code (gigatype ( ca len a1 a2 -- )

  \ ca len = string
  \ a1 = screen address
  \ a2 = style data table

d pop, h pop, #1 h stp, al# h pop, l a ld, #3 sta, al#
h pop, #2 h stp, al# exde, a and, next z? ?jp, b push,
  \   pop de                        ; style data table
  \   pop hl                        ; screen address
  \   ld (set_screen_address+1),hl
  \   pop hl
  \   ld a,l                        ; string length
  \   ld (set_string_length+1),a
  \   pop hl                        ; string address
  \   ld (set_string_address+1),hl
  \   ex de,hl                      ; HL = style data table
  \   and a                         ; empty string?
  \   jp z,next                     ; if so, exit
  \   push bc                       ; save the Forth IP

#0 l: m a ld, FF cp#, z? rif b pop, jpnext, rthen
  \ begin:
  \                        ; HL = address of the style data
  \   ld a,(hl)
  \   cp $FF               ; end of data?
  \   jr nz,begin.continue
  \   pop bc               ; restore the Forth IP
  \   _jp_next             ; exit
  \ begin.continue:

exaf, h incp, m a ld, #5 sta, al#
  \   ex af,af'
  \   inc hl
  \   ld a,(hl)
  \   ld (sub_fe07h+1),a

exaf, h push, exx, a c ld, exx,
  \   ex af,af'
  \   push hl
  \   exx
  \   ld c,a
  \   exx

here 1+ #1 l! 0 h ldp#,
  \ set_screen_address:
  \   ld hl,$0000

here 1+ #2 l! 0 d ldp#, here 1+ #3 l! 0 b ld#,
  \ set_string_address:
  \   ld de,$0000
  \ set_string_length:
  \   ld b,$00

#6 l: d ftap, b push, d push, h push, h push, a l ld,
  \ print_b_chars_from_de:
  \   ld a,(de)
  \   push bc
  \   push de
  \   push hl
  \   push hl
  \   ld l,a
00 h ld#, h addp, h addp, h addp, os-chars d ftp, d addp, exde,
  \   ld h,$00
  \   add hl,hl
  \   add hl,hl
  \   add hl,hl
  \   ld de,(sys_chars)
  \   add hl,de
  \   ex de,hl
h pop, exx, c a ld, exx, a b ld, %111 and#, a c ld, b a ld,
  \   pop hl
  \   exx
  \   ld a,c
  \   exx
  \   ld b,a
  \   and $07
  \   ld c,a
  \   ld a,b
38 and#, rrca, rrca, rrca, #7 call, al# b a ld, 80 and#,
  \   and $38
  \   rrca
  \   rrca
  \   rrca
  \   call sub_fdebh
  \   ld a,b
  \   and $80

#8 sta, al# 08 b ld#, rbegin #9 call, al# d incp, rstep -->
  \   ld (flag),a
  \   ld b,$08
  \ lfedah:
  \   call sub_fe3ah
  \   inc de
  \   djnz lfedah

( gigatype )

h pop, d pop, b pop, h incp, h incp, d incp, b dec,
  \   pop hl
  \   pop de
  \   pop bc
  \   inc hl
  \   inc hl
  \   inc de
  \   dec b
#6 nz? ?jp, al# h pop, h incp, #0 jp, al#
  \   jp nz,print_b_chars_from_de
  \   pop hl
  \   inc hl
  \   jp begin

#7 l: a and, z? ?ret, b push, a b ld,
  \ sub_fdebh:
  \   and a
  \   ret z
  \   push bc
  \   ld b,a
rbegin h inc, h a ld, %111 and#,
  \ lfdefh:
  \   inc h
  \   ld a,h
  \   and $07
z? rif l a ld, 20 add#, a l ld, E0 and#,
  \   jr nz,lfe01h
  \   ld a,l
  \   add a,$20
  \   ld l,a
  \   and $E0
nz? rif h a ld, 08 sub#, a h ld, rthen rthen
  \   jr z,lfe01h
  \   ld a,h
  \   sub $08
  \   ld h,a
rstep b pop, ret,
  \ lfe01h:
  \   djnz lfdefh
  \   pop bc
  \   ret

#4 l: here 1+ #5 l! 01 b ld#, a and, z? ?ret,
  \ sub_fe07h:
  \   ld b,$01
  \   and a
  \   ret z
  \
rbegin a push, #10 call, al# 01 a ld#, #7 call, al# a pop,
  \ lfe0bh:
  \   push af
  \   call sub_fe17h
  \   ld a,$01
  \   call sub_fdebh
  \   pop af
rstep
  \   djnz lfe0bh
  \
#10 l: exaf, h a ld, 58 cp#, nc? ?ret, h push, h a ld, 18 and#,
  \ sub_fe17h:
  \   ex af,af'
  \   ld a,h
  \   cp $58
  \   ret nc
  \   push hl
  \   ld a,h
  \   and $18
rrca, rrca, rrca, 58 add#, a h ld, os-attr-t fta, a m ld,
  \   rrca
  \   rrca
  \   rrca
  \   add a,$58
  \   ld h,a
  \   ld a,(sys_attr_t)
  \   ld (hl),a
h pop, here 1+ #8 l! 80 a ld#, a and,
  \   pop hl
  \   ld a,$80
  \   and a
z? rif  exaf, m or, a m ld, ret, rthen
  \   jr nz,lfe35h
  \   ex af,af'
  \   or (hl)
  \   ld (hl),a
  \   ret
  \ lfe35h:
exaf, cpl, m and, a m ld, ret, -->
  \   ex af,af'
  \   cpl
  \   and (hl)
  \   ld (hl),a
  \   ret

( gigatype )

#9 l: b push, d push, h push, exde, m a ld, 0 h ldp#, 4 b ld#,
  \ sub_fe3ah:
  \   push bc
  \   push de
  \   push hl
  \   ex de,hl
  \   ld a,(hl)
  \   ld hl,$0000
  \   ld b,$04
rrca, h rr, h sra, rrca, h rr, h sra, rrca, h rr, h sra, rrca,
  \   rrca
  \   rr h
  \   sra h
  \   rrca
  \   rr h
  \   sra h
  \   rrca
  \   rr h
  \   sra h
  \   rrca
h rr, h sra, rrca, l rr, l sra, rrca, l rr, l sra, rrca,
  \   rr h
  \   sra h
  \   rrca
  \   rr l
  \   sra l
  \   rrca
  \   rr l
  \   sra l
  \   rrca

l rr, l sra, rrca, l rr, l sra, c a ld, 00 c ld#, a and,
  \   rr l
  \   sra l
  \   rrca
  \   rr l
  \   sra l
  \   ld a,c
  \   ld c,$00
  \   and a
nz? rif a b ld, rbegin l rr, h rr, c rr, rstep rthen
  \   jr z,lfe7bh
  \   ld b,a
  \ lfe73h:
  \   rr l
  \   rr h
  \   rr c
  \   djnz lfe73h
  \ lfe7bh:
exde, e a ld, h push, #4 call, al# h pop, h incp, d a ld,
  \   ex de,hl
  \   ld a,e
  \   push hl
  \   call sub_fe07h
  \   pop hl
  \   inc hl
  \   ld a,d
h push, #4 call, al# h pop, h incp, c a ld,
  \   push hl
  \   call sub_fe07h
  \   pop hl
  \   inc hl
  \   ld a,c
h push, #4 call, al# h pop, h pop, d pop, b pop, 02 a ld#,
  \   push hl
  \   call sub_fe07h
  \   pop hl
  \   pop hl
  \   pop de
  \   pop bc
  \   ld a,$02
#7 call, al# ret, end-code
  \   call sub_fdebh
  \   ret

  \ doc{
  \
  \ (gigatype ( ca len a1 a2 -- ) "paren-gigatype"
  \
  \ If _len_ is greater than zero, display text string _ca len_
  \ at screen address _a1_ using the current fonts, doubled
  \ pixels (16x16 pixels per character) and modifying the
  \ characters on the fly after style data table _a2_.
  \
  \ ``(gigatype`` is written in Z80 and it's the low-level
  \ procedure of `gigatype`.
  \
  \ }doc

create gigatype-style 0 c,

  \ doc{
  \
  \ gigatype-style ( -- ca )
  \
  \ _ca_ is the address of a byte containing the font style
  \ used by `gigatype` (0..7).
  \
  \ }doc

need xy>scra need array>

: gigatype ( ca len -- )
  xy xy>scra
  gigatype-style c@ gigatype-styles array> @ (gigatype ;

  \ doc{
  \
  \ gigatype ( ca len -- )
  \
  \ If _len_ is greater than zero, display text string _ca len_
  \ using the current font, with doubled pixels (16x16 pixels
  \ per character) and modifying the characters on the fly
  \ after the style stored in `gigatype-style`.  The text is
  \ combined with the current content of the screen, as if
  \ `overprint` were active. The current attribute, set by
  \ `attr!` and other words, is used to color the text.
  \
  \ Usage example:

  \ ----
  \ : demo ( -- )
  \   cls
  \   8 0 ?do
  \     i gigatype-style c!
  \     17 0 i 3 * tuck at-xy s" GIGATYPE" gigatype
  \                     at-xy ." style "   i .
  \   loop
  \   key drop home ;
  \ ----

  \ See also: `gigatype-title`, `set-font`, `(gigatype`,
  \ `type`.
  \
  \ }doc

  \ ===========================================================
  \ Disassembly of the original code

  \ ; z80dasm 1.1.3

  \ ; command line: z80dasm --origin=0xFDE8 --address --labels
  \ ; --source --block-def=gigatext_blocks.txt
  \ ; --sym-input=gigatext_input_symbols.z80s
  \ ; --sym-output=gigatext_output_symbols.z80s --output=gigatext.z80s
  \ ; gigatext.bin

  \   org 0fde8h
  \ rom_look_vars:  equ 0x28b2
  \ sys_chars:  equ 0x5c36
  \ sys_ch_add: equ 0x5c5d
  \ sys_attr_p: equ 0x5c8d
  \ screen_address: equ 0xfea9
  \ start_of_string:  equ 0xfeac
  \ length_of_string: equ 0xfeaf

  \   jp get_parameters   ;fde8 c3 f2 fe  . . .
  \
  \ sub_fdebh:
  \   and a     ;fdeb a7  .
  \   ret z     ;fdec c8  .
  \   push bc     ;fded c5  .
  \   ld b,a      ;fdee 47  G
  \
  \ lfdefh:
  \   inc h     ;fdef 24  $
  \   ld a,h      ;fdf0 7c  |
  \   and 007h    ;fdf1 e6 07   . .
  \   jr nz,lfe01h    ;fdf3 20 0c     .
  \   ld a,l      ;fdf5 7d  }
  \   add a,020h    ;fdf6 c6 20   .
  \   ld l,a      ;fdf8 6f  o
  \   and 0e0h    ;fdf9 e6 e0   . .
  \   jr z,lfe01h   ;fdfb 28 04   ( .
  \   ld a,h      ;fdfd 7c  |
  \   sub 008h    ;fdfe d6 08   . .
  \   ld h,a      ;fe00 67  g
  \ lfe01h:
  \   djnz lfdefh   ;fe01 10 ec   . .
  \
  \   pop bc      ;fe03 c1  .
  \   ret     ;fe04 c9  .

  \   defb 047h      ;fe05 47  G
  \ lfe06h:
  \   defb 080h      ;fe06 80  .
  \
  \ sub_fe07h:
  \   ld b,001h   ;fe07 06 01   . .
  \   and a     ;fe09 a7  .
  \   ret z     ;fe0a c8  .
  \
  \ lfe0bh:
  \   push af     ;fe0b f5  .
  \   call sub_fe17h    ;fe0c cd 17 fe  . . .
  \   ld a,001h   ;fe0f 3e 01   > .
  \   call sub_fdebh    ;fe11 cd eb fd  . . .
  \   pop af      ;fe14 f1  .
  \   djnz lfe0bh   ;fe15 10 f4   . .
  \
  \ sub_fe17h:
  \   ex af,af'     ;fe17 08  .
  \   ld a,h      ;fe18 7c  |
  \   cp 058h   ;fe19 fe 58   . X
  \   ret nc      ;fe1b d0  .
  \   push hl     ;fe1c e5  .
  \   ld a,h      ;fe1d 7c  |
  \   and 018h    ;fe1e e6 18   . .
  \   rrca      ;fe20 0f  .
  \   rrca      ;fe21 0f  .
  \   rrca      ;fe22 0f  .
  \   add a,058h    ;fe23 c6 58   . X
  \   ld h,a      ;fe25 67  g
  \   ld a,(sys_attr_p)   ;fe26 3a 8d 5c  : . \
  \   ld (hl),a     ;fe29 77  w
  \   pop hl      ;fe2a e1  .
  \   ld a,(lfe06h)   ;fe2b 3a 06 fe  : . .
  \   and a     ;fe2e a7  .
  \   jr nz,lfe35h    ;fe2f 20 04     .
  \   ex af,af'     ;fe31 08  .
  \   or (hl)     ;fe32 b6  .
  \   ld (hl),a     ;fe33 77  w
  \   ret     ;fe34 c9  .
  \ lfe35h:
  \   ex af,af'     ;fe35 08  .
  \   cpl     ;fe36 2f  /
  \   and (hl)      ;fe37 a6  .
  \   ld (hl),a     ;fe38 77  w
  \   ret     ;fe39 c9  .
  \
  \ sub_fe3ah:
  \   push bc     ;fe3a c5  .
  \   push de     ;fe3b d5  .
  \   push hl     ;fe3c e5  .
  \   ex de,hl      ;fe3d eb  .
  \   ld a,(hl)     ;fe3e 7e  ~
  \   ld hl,00000h    ;fe3f 21 00 00  ! . .
  \   ld b,004h   ;fe42 06 04   . .
  \   rrca      ;fe44 0f  .
  \   rr h    ;fe45 cb 1c   . .
  \   sra h   ;fe47 cb 2c   . ,
  \   rrca      ;fe49 0f  .
  \   rr h    ;fe4a cb 1c   . .
  \   sra h   ;fe4c cb 2c   . ,
  \   rrca      ;fe4e 0f  .
  \   rr h    ;fe4f cb 1c   . .
  \   sra h   ;fe51 cb 2c   . ,
  \   rrca      ;fe53 0f  .
  \   rr h    ;fe54 cb 1c   . .
  \   sra h   ;fe56 cb 2c   . ,
  \   rrca      ;fe58 0f  .
  \   rr l    ;fe59 cb 1d   . .
  \   sra l   ;fe5b cb 2d   . -
  \   rrca      ;fe5d 0f  .
  \   rr l    ;fe5e cb 1d   . .
  \   sra l   ;fe60 cb 2d   . -
  \   rrca      ;fe62 0f  .
  \   rr l    ;fe63 cb 1d   . .
  \   sra l   ;fe65 cb 2d   . -
  \   rrca      ;fe67 0f  .
  \   rr l    ;fe68 cb 1d   . .
  \   sra l   ;fe6a cb 2d   . -
  \   ld a,c      ;fe6c 79  y
  \   ld c,000h   ;fe6d 0e 00   . .
  \   and a     ;fe6f a7  .
  \   jr z,lfe7bh   ;fe70 28 09   ( .
  \   ld b,a      ;fe72 47  G
  \ lfe73h:
  \   rr l    ;fe73 cb 1d   . .
  \   rr h    ;fe75 cb 1c   . .
  \   rr c    ;fe77 cb 19   . .
  \   djnz lfe73h   ;fe79 10 f8   . .
  \
  \ lfe7bh:
  \   ex de,hl      ;fe7b eb  .
  \   ld a,e      ;fe7c 7b  {
  \   push hl     ;fe7d e5  .
  \   call sub_fe07h    ;fe7e cd 07 fe  . . .
  \   pop hl      ;fe81 e1  .
  \   inc hl      ;fe82 23  #
  \   ld a,d      ;fe83 7a  z
  \   push hl     ;fe84 e5  .
  \   call sub_fe07h    ;fe85 cd 07 fe  . . .
  \   pop hl      ;fe88 e1  .
  \   inc hl      ;fe89 23  #
  \   ld a,c      ;fe8a 79  y
  \   push hl     ;fe8b e5  .
  \   call sub_fe07h    ;fe8c cd 07 fe  . . .
  \   pop hl      ;fe8f e1  .
  \   pop hl      ;fe90 e1  .
  \   pop de      ;fe91 d1  .
  \   pop bc      ;fe92 c1  .
  \   ld a,002h   ;fe93 3e 02   > .
  \   call sub_fdebh    ;fe95 cd eb fd  . . .
  \   ret     ;fe98 c9  .
  \
  \ begin:
  \   ld a,(hl)     ;fe99 7e  ~
  \   cp 0ffh   ;fe9a fe ff   . .
  \   ret z     ;fe9c c8  .
  \   ex af,af'     ;fe9d 08  .
  \   inc hl      ;fe9e 23  #
  \   ld a,(hl)     ;fe9f 7e  ~
  \   ld (sub_fe07h+1),a    ;fea0 32 08 fe  2 . .
  \   ex af,af'     ;fea3 08  .
  \   push hl     ;fea4 e5  .
  \   exx     ;fea5 d9  .
  \   ld c,a      ;fea6 4f  O
  \   exx     ;fea7 d9  .
  \ set_screen_address:
  \   ld hl,04207h    ;fea8 21 07 42  ! . B
  \ set_start_of_string:
  \   ld de,06e7eh    ;feab 11 7e 6e  . ~ n
  \ set_length_of_string:
  \   ld b,009h   ;feae 06 09   . .
  \ print_b_chars_from_de:
  \   ld a,(de)     ;feb0 1a  .
  \   push bc     ;feb1 c5  .
  \   push de     ;feb2 d5  .
  \   push hl     ;feb3 e5  .
  \   push hl     ;feb4 e5  .
  \   ld l,a      ;feb5 6f  o
  \   ld h,000h   ;feb6 26 00   & .
  \   add hl,hl     ;feb8 29  )
  \   add hl,hl     ;feb9 29  )
  \   add hl,hl     ;feba 29  )
  \   ld de,(sys_chars)   ;febb ed 5b 36 5c   . [ 6 \
  \   add hl,de     ;febf 19  .
  \   ex de,hl      ;fec0 eb  .
  \   pop hl      ;fec1 e1  .
  \   exx     ;fec2 d9  .
  \   ld a,c      ;fec3 79  y
  \   exx     ;fec4 d9  .
  \   ld b,a      ;fec5 47  G
  \   and 007h    ;fec6 e6 07   . .
  \   ld c,a      ;fec8 4f  O
  \   ld a,b      ;fec9 78  x
  \   and 038h    ;feca e6 38   . 8
  \   rrca      ;fecc 0f  .
  \   rrca      ;fecd 0f  .
  \   rrca      ;fece 0f  .
  \   call sub_fdebh    ;fecf cd eb fd  . . .
  \   ld a,b      ;fed2 78  x
  \   and 080h    ;fed3 e6 80   . .
  \   ld (lfe06h),a   ;fed5 32 06 fe  2 . .
  \   ld b,008h   ;fed8 06 08   . .
  \ lfedah:
  \   call sub_fe3ah    ;feda cd 3a fe  . : .
  \   inc de      ;fedd 13  .
  \   djnz lfedah   ;fede 10 fa   . .
  \   pop hl      ;fee0 e1  .
  \   pop de      ;fee1 d1  .
  \   pop bc      ;fee2 c1  .
  \   inc hl      ;fee3 23  #
  \   inc hl      ;fee4 23  #
  \   inc de      ;fee5 13  .
  \   dec b     ;fee6 05  .
  \   jp nz,print_b_chars_from_de   ;fee7 c2 b0 fe  . . .
  \   pop hl      ;feea e1  .
  \   inc hl      ;feeb 23  #
  \   jp begin    ;feec c3 99 fe  . . .
  \
  \ variable:
  \   defb 04dh   ;feef 4d  M
  \   defb 024h   ;fef0 24  $
  \   defb 00dh   ;fef1 0d  .
  \ get_parameters:
  \   ld hl,(sys_ch_add)    ;fef2 2a 5d 5c  * ] \
  \   push hl     ;fef5 e5  .
  \   ld hl,variable    ;fef6 21 ef fe  ! . .
  \   ld (sys_ch_add),hl    ;fef9 22 5d 5c  " ] \
  \   ld c,04dh   ;fefc 0e 4d   . M
  \   call rom_look_vars    ;fefe cd b2 28  . . (
  \   pop de      ;ff01 d1  .
  \   ld (sys_ch_add),de    ;ff02 ed 53 5d 5c   . S ] \
  \   ret c     ;ff06 d8  .
  \   inc hl      ;ff07 23  #
  \ get_length_of_string:
  \   ld a,(hl)     ;ff08 7e  ~
  \   dec a     ;ff09 3d  =
  \   dec a     ;ff0a 3d  =
  \   dec a     ;ff0b 3d  =
  \   ld b,a      ;ff0c 47  G
  \   and a     ;ff0d a7  .
  \   ret z     ;ff0e c8  .
  \   ld (length_of_string),a   ;ff0f 32 af fe  2 . .
  \   inc hl      ;ff12 23  #
  \   inc hl      ;ff13 23  #
  \ get_x_coordinate:
  \   ld a,(hl)     ;ff14 7e  ~
  \   cp 01fh   ;ff15 fe 1f   . .
  \   jr c,lff1ch   ;ff17 38 03   8 .
  \   ld a,010h   ;ff19 3e 10   > .
  \   sub b     ;ff1b 90  .
  \ lff1ch:
  \   ex af,af'     ;ff1c 08  .
  \   inc hl      ;ff1d 23  #
  \ get_y_coordinate:
  \   ld a,(hl)     ;ff1e 7e  ~
  \   ld c,a      ;ff1f 4f  O
  \   ex de,hl      ;ff20 eb  .
  \   ld a,c      ;ff21 79  y
  \   and 0c0h    ;ff22 e6 c0   . .
  \   rrca      ;ff24 0f  .
  \   rrca      ;ff25 0f  .
  \   rrca      ;ff26 0f  .
  \   add a,040h    ;ff27 c6 40   . @
  \   ld h,a      ;ff29 67  g
  \   ld a,c      ;ff2a 79  y
  \   and 007h    ;ff2b e6 07   . .
  \   or h      ;ff2d b4  .
  \   ld h,a      ;ff2e 67  g
  \   ld a,c      ;ff2f 79  y
  \   and 038h    ;ff30 e6 38   . 8
  \   add a,a     ;ff32 87  .
  \   add a,a     ;ff33 87  .
  \   ld l,a      ;ff34 6f  o
  \   ex af,af'     ;ff35 08  .
  \   or l      ;ff36 b5  .
  \   ld l,a      ;ff37 6f  o
  \ save_screen_address:
  \   ld (screen_address),hl    ;ff38 22 a9 fe  " . .
  \   ex de,hl      ;ff3b eb  .
  \   inc hl      ;ff3c 23  #
  \ get_style:
  \   ld a,(hl)     ;ff3d 7e  ~
  \   inc hl      ;ff3e 23  #
  \   ld (start_of_string),hl   ;ff3f 22 ac fe  " . .
  \   ld hl,style_1_data    ;ff42 21 5c ff  ! \ .
  \   dec a     ;ff45 3d  =
  \   and 007h    ;ff46 e6 07   . .
  \   and a     ;ff48 a7  .
  \   jr z,repeat   ;ff49 28 0e   ( .
  \   ld c,a      ;ff4b 4f  O
  \   ld b,000h   ;ff4c 06 00   . .
  \ get_style_data_byte:
  \   ld a,(hl)     ;ff4e 7e  ~
  \   inc hl      ;ff4f 23  #
  \   cp 0ffh   ;ff50 fe ff   . .
  \   jr nz,get_style_data_byte   ;ff52 20 fa     .
  \   inc b     ;ff54 04  .
  \   ld a,b      ;ff55 78  x
  \   cp c      ;ff56 b9  .
  \   jr nz,get_style_data_byte   ;ff57 20 f5     .
  \ repeat:
  \   jp begin    ;ff59 c3 99 fe  . . .
  \
  \ style_1_data:
  \   defb 001h   ;ff5c 01  .
  \   defb 003h   ;ff5d 03  .
  \   defb 008h   ;ff5e 08  .
  \   defb 001h   ;ff5f 01  .
  \   defb 00ah   ;ff60 0a  .
  \   defb 001h   ;ff61 01  .
  \   defb 0ffh   ;ff62 ff  .
  \ style_2_data:
  \   defb 001h   ;ff63 01  .
  \   defb 003h   ;ff64 03  .
  \   defb 008h   ;ff65 08  .
  \   defb 001h   ;ff66 01  .
  \   defb 00ah   ;ff67 0a  .
  \   defb 001h   ;ff68 01  .
  \   defb 089h   ;ff69 89  .
  \   defb 001h   ;ff6a 01  .
  \   defb 0ffh   ;ff6b ff  .
  \ style_3_data:
  \   defb 009h   ;ff6c 09  .
  \   defb 001h   ;ff6d 01  .
  \   defb 080h   ;ff6e 80  .
  \   defb 001h   ;ff6f 01  .
  \   defb 0ffh   ;ff70 ff  .
  \ style_4_data:
  \   defb 009h   ;ff71 09  .
  \   defb 003h   ;ff72 03  .
  \   defb 010h   ;ff73 10  .
  \   defb 001h   ;ff74 01  .
  \   defb 012h   ;ff75 12  .
  \   defb 001h   ;ff76 01  .
  \   defb 080h   ;ff77 80  .
  \   defb 001h   ;ff78 01  .
  \   defb 0ffh   ;ff79 ff  .
  \ style_5_data:
  \   defb 024h   ;ff7a 24  $
  \   defb 001h   ;ff7b 01  .
  \   defb 09bh   ;ff7c 9b  .
  \   defb 001h   ;ff7d 01  .
  \   defb 001h   ;ff7e 01  .
  \   defb 003h   ;ff7f 03  .
  \   defb 008h   ;ff80 08  .
  \   defb 001h   ;ff81 01  .
  \   defb 00ah   ;ff82 0a  .
  \   defb 001h   ;ff83 01  .
  \   defb 0ffh   ;ff84 ff  .
  \ style_6_data:
  \   defb 024h   ;ff85 24  $
  \   defb 001h   ;ff86 01  .
  \   defb 09bh   ;ff87 9b  .
  \   defb 001h   ;ff88 01  .
  \   defb 001h   ;ff89 01  .
  \   defb 003h   ;ff8a 03  .
  \   defb 008h   ;ff8b 08  .
  \   defb 001h   ;ff8c 01  .
  \   defb 00ah   ;ff8d 0a  .
  \   defb 001h   ;ff8e 01  .
  \   defb 089h   ;ff8f 89  .
  \   defb 001h   ;ff90 01  .
  \   defb 0ffh   ;ff91 ff  .
  \ style_7_data:
  \   defb 001h   ;ff92 01  .
  \   defb 005h   ;ff93 05  .
  \   defb 003h   ;ff94 03  .
  \   defb 005h   ;ff95 05  .
  \   defb 008h   ;ff96 08  .
  \   defb 003h   ;ff97 03  .
  \   defb 00ah   ;ff98 0a  .
  \   defb 003h   ;ff99 03  .
  \   defb 00ch   ;ff9a 0c  .
  \   defb 003h   ;ff9b 03  .
  \   defb 08ah   ;ff9c 8a  .
  \   defb 003h   ;ff9d 03  .
  \   defb 091h   ;ff9e 91  .
  \   defb 001h   ;ff9f 01  .
  \   defb 093h   ;ffa0 93  .
  \   defb 001h   ;ffa1 01  .
  \   defb 0ffh   ;ffa2 ff  .
  \ style_8_data:
  \   defb 001h   ;ffa3 01  .
  \   defb 005h   ;ffa4 05  .
  \   defb 003h   ;ffa5 03  .
  \   defb 005h   ;ffa6 05  .
  \   defb 008h   ;ffa7 08  .
  \   defb 003h   ;ffa8 03  .
  \   defb 00ah   ;ffa9 0a  .
  \   defb 003h   ;ffaa 03  .
  \   defb 00ch   ;ffab 0c  .
  \   defb 003h   ;ffac 03  .
  \   defb 08ah   ;ffad 8a  .
  \   defb 003h   ;ffae 03  .
  \   defb 091h   ;ffaf 91  .
  \   defb 001h   ;ffb0 01  .
  \   defb 093h   ;ffb1 93  .
  \   defb 001h   ;ffb2 01  .
  \   defb 012h   ;ffb3 12  .
  \   defb 001h   ;ffb4 01  .
  \   defb 0ffh   ;ffb5 ff  .

( gigatype-title )

need gigatype need 2/

: gigatype-title ( ca len -- )
  32 over 2* - 2/ xy nip at-xy gigatype ;

  \ doc{
  \
  \ gigatype-title ( ca len -- )
  \
  \ If _len1_ is greater than zero, display the character
  \ string _ca len_ at the center of the current row (the
  \ current column is not used), using `gigatype`.
  \
  \ WARNING: `gigatype` prints double-size (16x16 pixels)
  \ characters.  Therefore, the maximum value of _len1_ is 16
  \ characters, but ``gigatype-title`` does no check. Beside,
  \ it calculates the column of the title assuming the current
  \ mode is `mode-32` (32 characters per line), which is the
  \ default one.
  \
  \ See also: `gigatype-style`, `type-center-field`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2017-03-27: Start adapting the disassembly of the original
  \ routine.
  \
  \ 2017-04-16: Start draft of `gigatype-center`, based on
  \ `type-center-field`. Fix `(gigatype`: do nothing when the
  \ length of the string is zero.  Improve documentation.
  \
  \ 2017-04-17: Add `gigatype-title` (a simpler but enough
  \ implementation of `gigatype-center`, which has been
  \ discarded). Improve documentation.
  \
  \ 2017-05-06: Move the `gigatype` style from the stack to
  \ `gigatype-style`. This makes the usage simpler and
  \ compatible with the ordinary `type`. Update and improve
  \ documentation.
  \
  \ 2017-12-10: Update to `a push,` and `a pop,`, after the
  \ change in the assembler.
  \
  \ 2018-03-08: Add words' pronunciaton.
  \
  \ 2020-05-05: Fix cross references.

  \ vim: filetype=soloforth

