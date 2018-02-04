  \ display.mode.64ao.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201802041810
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ A 64-cpl display mode.

  \ ===========================================================
  \ Authors

  \ Author of the original code: Andrew Owen, 2007.
  \ Published on the World of Spectrum forum:
  \ http://www.worldofspectrum.org/forums/discussion/14526/redirect/p1

  \ Marcos Cruz (programandala.net) adapted it to Solo Forth,
  \ 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( (mode-64ao-output_ )

  \ Credit:
  \
  \ Code adapted from the 4x8 Font Driver written by Andrew
  \ Owen.
  \
  \ http://www.worldofspectrum.org/forums/discussion/14526/redirect/p1

need os-chars need assembler need l:

create mode-64ao-at-flag 0 c, \ XXX TODO -- move to the code
create mode-64ao-column 0 c,  create mode-64ao-row 0 c,

also assembler max-labels c@ 9 max-labels c! previous

create (mode-64ao-output_ ( -- a ) asm

  \ Input:
  \   A = character code

  \ ; 4x8 FONT DRIVER
  \ ; (c) 2007, 2011 Andrew Owen
  \ ; optimized by Crisis (to 602 bytes)
  \ ; http://www.worldofspectrum.org/forums/discussion/14526/redirect/p1
  \
  \ ; -----------------------------
  \ ; Modified by Marcos Cruz in order to integrate the code into
  \ ; Solo Forth
  \ ; (http://programandala.net/en.program.solo_forth.html):
  \
  \ ; 2015-09-08: Removed the channels stuff. That will be done in
  \ ; Forth. First changes to add the left control char.
  \ ;
  \ ; 2015-09-10: Added left control char. Exchanged column and row
  \ ; to suit the Forth convention. Added scroll. Separated the
  \ ; font from the code; now the system variable CHARS is used to
  \ ; point to the font.
  \ ;
  \ ; 2015-09-11: Modified some labels and comments.
  \
  \ 2017-05-14: Convert to Forth assembler.
  \
  \ ; -----------------------------
  \
  \ rom_cl_all:  equ 0x0dfe
  \ sys_chars:   equ 23606 ; (2) 256 less than address of character set
  \
  \ original_font: equ 0 ; flag
  \
  \         org     60000
  \
  \ print_character:
  \
  \         ; Based on code by Tony Samuels from Your Spectrum issue 13, April 85.
  \
  \         ; A = character
  \

  a b ld, mode-64ao-at-flag fta, a and, z? rif

  \         ld      b, a            ; save character
  \         ld      a, (at_flag)    ; value of AT flag
  \         and     a               ; test against zero
  \         jr      nz, getcol      ; jump if not


    b a ld, 16 cp#, #1 rl# nz? ?jr, FF a ld#,
  \         ld      a, b            ; restore character
  \
  \ check_at:
  \         cp      0x16            ; test for AT
  \         jr      nz, check_cr    ; if not test for CR
  \         ld      a,0xFF          ; next character will be column

    #0 l: mode-64ao-at-flag sta, ret,

  \ save_at_flag_and_exit:
  \         ld      (at_flag), a
  \         ret

  rthen

  \ getcol:

  FE cp#, nz? rif b a ld, #64 cp#, nc? rif #63 a ld#, rthen

  \         cp      0xfe            ; test AT flag
  \         jr      z, getrow       ; jump if setting row
  \         ld      a, b            ; restore character
  \         cp      64              ; greater than 63?
  \         jr      c, valid_col
  \         ld      a,63            ; maximum
  \ valid_col:

    mode-64ao-column sta, mode-64ao-at-flag h ldp#, m dec, ret,

  \         ld      (col), a        ; store it in col
  \         ld      hl, at_flag     ; AT flag
  \         dec     (hl)            ; next character will be row
  \         ret
  \

  rthen

  \ getrow:

  b a ld, #24 cp#, nc? rif #23 a ld#, rthen -->
  \         ld      a, b            ; restore character
  \         cp      24              ; greater than 23?
  \         jr      c,valid_row
  \         ld      a, 23           ; maximum
  \ valid_row:

( (mode-64ao-output_ )

  mode-64ao-row sta, a xor, #0 rl# jr,
  \         ld      (row), a        ; store it in row
  \         xor     a               ; set a to zero
  \         jr      save_at_flag_and_exit
  \

  #1 l: 0D cp#, #2 rl# z? ?jr,
  \ check_cr:
  \         cp      0x0D            ; carriage return?
  \         jr      z, do_cr        ; if so, jump
  08 cp#, #3 rl# z? ?jr,
  \         cp      0x08            ; cursor left?
  \         jr      z, do_left      ; if so, jump
  #4 call, al#
  mode-64ao-column h ldp#, m inc, m a ld, #64 cp#, nz? ?ret,
  \         call    print_printable_character
  \         ld      hl, col         ; increment
  \         inc     (hl)            ; the column
  \         ld      a, (hl)         ; get it
  \         cp      64              ; column 64?
  \         ret     nz              ; return if not

  #2 l: a xor, mode-64ao-column sta,
  \ do_cr:
  \         xor     a               ; set A to zero
  \         ld      (col), a        ; reset column
  mode-64ao-row fta, #23 cp#, 0DFE z? ?jp,
  \         ld      a, (row)        ; get the row
  \         cp      23              ; row 23?
  \         jp      z,rom_cl_all    ; if so, scroll
  a inc, mode-64ao-row sta, ret,
  \         inc     a               ; increment the row
  \         ld      (row), a        ; write it back
  \         ret

  #3 l: mode-64ao-column fta, a and, z? rif
    mode-64ao-row fta, a and, z? ?ret,
  \ do_left:
  \         ld      a, (col)        ; get the column
  \         and     a               ; is it zero?
  \         jr      nz,do_left.same_line
  \         ; the column is zero
  \         ld      a, (row)        ; get the row
  \         and     a               ; is it zero?
  \         ret z                   ; if so, return

    a dec, mode-64ao-row sta, #64 a ld#,
  \         ; the column is zero
  \         ; the row is not zero
  \         dec     a
  \         ld      (row),a
  \         ld      a,64            ; last column

  rthen a dec, mode-64ao-column sta, ret,
  \ do_left.same_line:
  \         dec     a
  \         ld      (col),a
  \         ret

  #4 l: rra, a l ld, 0 a ld#, a h ld, exaf, h d ld, l e ld,
  \ print_printable_character:
  \
  \         ; A = printable char
  \
  \         rra                     ; divide by two with remainder in carry flag
  \         ld      l, a            ; char to low byte of HL
  \         ld      a, 0            ; don't touch carry flag
  \         ld      h, a            ; clear H
  \         ex      af, af'         ; save the carry flag
  \         ld      d, h            ; store HL in DE
  \         ld      e, l            ; without using the stack

  h addp, h addp, h addp, d sbcp, -->
  \         add     hl, hl          ; multiply
  \         add     hl, hl          ; by
  \         add     hl, hl          ; eight
  \         sbc     hl, de          ; subtract DE to get original x7
  \

( (mode-64ao-output_ )

  \ ; calculate address of first byte of character map in font
  \
  \ if original_font
  \
  \         ld      de, font - 0x71 ; offset to FONT
  \         add     hl, de
  \
  \ else

  os-chars d ftp, d addp, h push,

  \         ld      de,(sys_chars)
  \         add     hl,de
  \         ; ld      de,0x0071
  \         ; and     a               ; reset carry flag
  \         ; sbc     hl,de
  \
  \ endif
  \
  \         ; HL holds address of first byte of character map in font
  \         push    hl              ; save font address

  mode-64ao-row fta, a b ld, %00011000 and#, a d ld, b a ld,
  \ ; convert the row to the base screen address
  \
  \         ld      a, (row)        ; get the row
  \         ld      b, a            ; save it
  \         and     %00011000       ; mask off bit 3-4
  \         ld      d, a            ; store high byte of offset in D
  \         ld      a, b            ; retrieve it

  %00000111 and#, rlca, rlca, rlca, rlca, rlca, a e ld,
  \         and     %00000111       ; mask off bit 0-2
  \         rlca                    ; shift
  \         rlca                    ; five
  \         rlca                    ; bits
  \         rlca                    ; to the
  \         rlca                    ; left
  \         ld      e, a            ; store low byte of offset in E
  \

  mode-64ao-column fta, rra, a push, 40 h ld#, a l ld, d addp,
  exde, a pop, h pop, 8 b ld#, c? rif
  \ ; add the column
  \
  \         ld      a, (col)        ; get the column
  \         rra                     ; divide by two with remainder in carry
  \                                 ; flag
  \         push    af              ; store the carry flag
  \         ld      h, 0x40         ; base location
  \         ld      l, a            ; plus column offset
  \         add     hl, de          ; add the offset
  \         ex      de, hl          ; put the result back in DE
  \
  \ ; HL now points to the location of the first byte of char data in FONT
  \ ; DE points to the first byte of the screen address
  \ ; C holds the offset to the routine
  \
  \         pop     af              ; restore column carry flag
  \         pop     hl              ; restore the font address
  \         ld      b, 8            ; 8 bytes to write
  \         jr      nc, odd_col     ; jump if odd column
  \

  exaf, #5 rl# c? ?jr, #8 rl# jr,
  \ even_col:
  \         ex      af, af'         ; restore char position carry flag
  \         jr      c, l_on_l       ; left char on left col
  \         jr      r_on_l          ; right char on left col

  rthen exaf, #6 rl# nc? ?jr, #7 rl# jr,
  \ odd_col:
  \         ex      af, af'         ; restore char position carry flag
  \         jr      nc, r_on_r      ; right char on right col
  \         jr      l_on_r          ; left char on right col

  rbegin m a ld,
         #5 l: %00001111 and#, a c ld, d ftap,
               %11110000 and#, c or, d stap, d inc, h incp,
  rstep ret, -->

  \ ; WRITE A CHARACTER TO THE SCREEN
  \ ; There are four separate routines
  \ ; HL points to the first byte of a character in the font
  \ ; DE points to the first byte of the screen address
  \
  \ ; left nibble on left hand side
  \
  \ ll_lp:
  \         ld      a, (hl)         ; get byte of font
  \
  \ l_on_l:
  \         and     %00001111       ; mask area used by new character
  \         ld      c, a            ; store in c
  \         ld      a, (de)         ; read byte at destination
  \         and     %11110000       ; mask off unused half
  \         or      c               ; combine with background
  \         ld      (de), a         ; write it back
  \         inc     d               ; point to next screen location
  \         inc     hl              ; point to next font data
  \         djnz    ll_lp           ; loop 8 times
  \         ret

( (mode-64ao-output_ )

  rbegin m a ld,
  \ ; right nibble on right hand side
  \
  \ rr_lp:
  \         ld      a, (hl)         ; read byte at destination
  \

  #6 l: %11110000 and#, a c ld, d ftap, %00001111 and#,
  \ r_on_r:
  \         and     %11110000       ; mask area used by new character
  \         ld      c, a            ; store in c
  \         ld      a, (de)         ; get byte of font
  \         and     %00001111       ; mask off unused half

  c or, d stap, d inc, h incp, rstep ret,
  \         or      c               ; combine with background
  \         ld      (de), a         ; write it back
  \         inc     d               ; point to next screen location
  \         inc     hl              ; point to next font data
  \         djnz    rr_lp           ; loop 8 times
  \         ret

  rbegin m a ld, rrca, rrca, rrca, rrca,
  \ ; left nibble on right hand side
  \
  \ lr_lp:
  \         ld      a, (hl)         ; read byte at destination
  \         rrca                    ; shift right
  \         rrca                    ; four bits
  \         rrca                    ; leaving 7-4
  \         rrca                    ; empty

  #7 l: %11110000 and#, a c ld, d ftap, %00001111 and#,
  \ l_on_r:
  \         and     %11110000       ; mask area used by new character
  \         ld      c, a            ; store in c
  \         ld      a, (de)         ; get byte of font
  \         and     %00001111       ; mask off unused half

  c or, d stap, d inc, h incp, rstep ret,
  \         or      c               ; combine with background
  \         ld      (de), a         ; write it back
  \         inc     d               ; point to next screen location
  \         inc     hl              ; point to next font data
  \         djnz    lr_lp           ; loop 8 times
  \         ret

  rbegin m a ld, rlca, rlca, rlca, rlca,

  \ ; right nibble on left hand side
  \
  \ rl_lp:
  \         ld      a, (hl)         ; read byte at destination
  \         rlca                    ; shift left
  \         rlca                    ; four bits
  \         rlca                    ; leaving 3-0
  \         rlca                    ; empty
  \

  #8 l: %00001111 and#, a c ld, d ftap, %11110000 and#,

  \ r_on_l:
  \         and     %00001111       ; mask area used by new character
  \         ld      c, a            ; store in c
  \         ld      a, (de)         ; get byte of font
  \         and     %11110000       ; mask off unused half

  c or, d stap, d inc, h incp, rstep ret, end-asm
  \         or      c               ; combine with background
  \         ld      (de), a         ; write it back
  \         inc     d               ; point to next screen location
  \         inc     hl              ; point to next font data
  \         djnz    rl_lp           ; loop 8 times
  \         ret

also assembler max-labels c! previous

  \ doc{
  \
  \ (mode-64ao-output_  ( -- a ) "paren-mode-64-a-o-output"
  \
  \ _a_ is the address of a Z80 routine, the low-level
  \ `mode-64ao` driver, which displays the character in the A
  \ register. The Forth IP is not preserved.
  \
  \ ``mode-64ao-output_`` is called by `mode-64ao-output_` and
  \ `mode-64ao-emit``.
  \
  \ }doc

( mode-64ao-output_ mode-64ao-emit )

need assembler need (mode-64ao-output_

create mode-64ao-output_ ( -- a )
  asm b push, (mode-64ao-output_ call, b pop, ret, end-asm

  \ doc{
  \
  \ mode-64ao-output_ ( -- a ) "mode-64-a-o-output-underscore"
  \
  \ _a_ is the address of a Z80 routine, the entry to
  \ `mode-64ao` driver, which preserves the Forth IP and then
  \ displays the character in the A register by calling
  \ `(mode-64ao-output_`.
  \
  \ }doc

code mode-64ao-emit ( c -- )
  exx, b pop, c a ld, (mode-64ao-output_ call,
  exx, jpnext, end-code

  \ doc{
  \
  \ mode-64ao-emit ( c -- ) "mode-64-a-o-emit"
  \
  \ Display character _c_ in `mode-64ao`, by calling
  \ `(mode-64ao-output_`.
  \
  \ ``mode-64ao-emit`` is configured by `mode-64ao` as the action
  \ of `emit`.
  \
  \ }doc

( mode-64ao )

need mode-64ao-output_ need mode-64ao-emit need mode-64-font
need mode-32 need (at-xy need set-mode-output need >form

: mode-64ao-xy ( -- col row )
  mode-64ao-column c@ mode-64ao-row c@ ;

  \ doc{
  \
  \ mode-64ao-xy ( -- col row ) "mode-64-a-o-x-y"
  \
  \ Return the current cursor coordinates _col row_ in
  \ `mode-64ao`. ``mode-64ao-xy`` is the action of `xy` when
  \ `mode-64ao` is active.
  \
  \ }doc

: mode-64ao ( -- )
  [ latestxt ] literal current-mode !
  mode-64-font @ $71 - set-font
  mode-64ao-output_ set-mode-output
  ['] mode-64ao-emit ['] emit  defer!
  ['] (at-xy        ['] at-xy defer! 64 24 >form
  ['] mode-64ao-xy   ['] xy    defer! ;

  \ doc{
  \
  \ mode-64ao ( -- ) "mode-64-a-o"
  \
  \ Start the 64-cpl display mode based on:

  \ ....
  \ 4x8 FONT DRIVER
  \ (c) 2007, 2011 Andrew Owen
  \ optimized by Crisis (to 602 bytes)
  \ http://www.worldofspectrum.org/forums/discussion/14526/redirect/p1
  \ ....

  \ The control characters recognized are 8 (left), 13
  \ (carriage return) and 22 (at).
  \
  \ WARNING: the "at" control character is followed by column
  \ and row, i.e. the order of the coordinates is inverted
  \ compared to the Sinclair BASIC convention and `mode-32`.
  \ This will be changed in a later version of the code.
  \
  \ See: `current-mode`, `set-font`, `set-mode-output`,
  \ `columns`, `rows`, `mode-64ao-emit`, `mode-64ao-xy`,
  \ `mode-64-font`, `>form`, `mode-64ao-output_`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ --------------------------------------------
  \ Old, from <display.mode.64.fs>

  \ 2016-04-26: Update `latest name>` to `latestxt`.
  \
  \ 2016-05-07: Improve the file header.
  \
  \ 2016-08-11: Rename the filenames of the driver.
  \
  \ 2016-10-16: Fix credits.
  \
  \ 2017-01-02: Convert the new unfinished version of
  \ `mode64-emit` from `z80-asm` to `z80-asm,` and fix it.
  \
  \ 2017-01-05: Update `need z80-asm,` to `need assembler`.
  \
  \ 2017-02-08: Update the usage of `set-drive`, which now
  \ returns an error result.
  \
  \ 2017-02-11: Replace old `<file-as-is` with `0 0 file>`,
  \ after the improvements in the G+DOS module. Use `drive` to
  \ make the code compatible with any DOS.
  \
  \ 2017-02-21: Need `unresolved`, which now is optional, not
  \ part of the assembler.
  \
  \ 2017-04-21: Rename module and words after the new
  \ convention for display modes.
  \
  \ 2017-05-13: Prepare the integration of the driver, both the
  \ current version, adapted from the original version written
  \ by Andrew Owen (so far loaded from disk in binary form) and
  \ its reimplementation written by Einar Saukas.
  \
  \ 2017-05-14: Finish the implementation of `mode-64o`. Remove
  \ its old disk-based version `mode-64`. Rename `4x8font` to
  \ `4x8-font`. Improve documentation.
  \
  \ 2017-05-15: Use `>form` for mode transition. Improve
  \ documentation. Rename `4x8-font` to `owen-64cpl-font`,
  \ after the filenames of the fonts included in disk 0.

  \ --------------------------------------------
  \ New

  \ 2017-12-05: Extract from <display.mode.64.fs>. Move
  \ `mode-64o-font` to <display.mode.64.COMMON.fs> and rename
  \ it `mode-64-font`.
  \
  \ 2017-12-10: Update to `a push,` and `a pop,`, after the
  \ change in the assembler.
  \
  \ 2018-01-24: Update after the renaming of all display modes
  \ files and words: "64o" (Owen) -> "64ao" (Andrew Owen).
  \
  \ 2018-02-04: Improve documentation: add pronunciation to
  \ words that need it.

  \ vim: filetype=soloforth
