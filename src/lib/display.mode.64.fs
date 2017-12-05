  \ display.mode.64.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201712051114
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
  \ 2015, 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( (mode-64o-output_ )


  \ Credit:
  \
  \ Code adapted from the 4x8 Font Driver written by Andrew
  \ Owen.
  \
  \ http://www.worldofspectrum.org/forums/discussion/14526/redirect/p1

need os-chars need assembler need l:

create mode-64o-at-flag 0 c, \ XXX TODO -- move to the code
create mode-64o-column 0 c,  create mode-64o-row 0 c,

also assembler max-labels c@ 9 max-labels c! previous

create (mode-64o-output_ ( -- a ) asm

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

  a b ld, mode-64o-at-flag fta, a and, z? rif

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

    #0 l: mode-64o-at-flag sta, ret,

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

    mode-64o-column sta, mode-64o-at-flag h ldp#, m dec, ret,

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

( (mode-64o-output_ )

  mode-64o-row sta, a xor, #0 rl# jr,
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
  mode-64o-column h ldp#, m inc, m a ld, #64 cp#, nz? ?ret,
  \         call    print_printable_character
  \         ld      hl, col         ; increment
  \         inc     (hl)            ; the column
  \         ld      a, (hl)         ; get it
  \         cp      64              ; column 64?
  \         ret     nz              ; return if not

  #2 l: a xor, mode-64o-column sta,
  \ do_cr:
  \         xor     a               ; set A to zero
  \         ld      (col), a        ; reset column
  mode-64o-row fta, #23 cp#, 0DFE z? ?jp,
  \         ld      a, (row)        ; get the row
  \         cp      23              ; row 23?
  \         jp      z,rom_cl_all    ; if so, scroll
  a inc, mode-64o-row sta, ret,
  \         inc     a               ; increment the row
  \         ld      (row), a        ; write it back
  \         ret

  #3 l: mode-64o-column fta, a and, z? rif
    mode-64o-row fta, a and, z? ?ret,
  \ do_left:
  \         ld      a, (col)        ; get the column
  \         and     a               ; is it zero?
  \         jr      nz,do_left.same_line
  \         ; the column is zero
  \         ld      a, (row)        ; get the row
  \         and     a               ; is it zero?
  \         ret z                   ; if so, return

    a dec, mode-64o-row sta, #64 a ld#,
  \         ; the column is zero
  \         ; the row is not zero
  \         dec     a
  \         ld      (row),a
  \         ld      a,64            ; last column

  rthen a dec, mode-64o-column sta, ret,
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

( (mode-64o-output_ )

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

  mode-64o-row fta, a b ld, %00011000 and#, a d ld, b a ld,
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

  mode-64o-column fta, rra, af push, 40 h ld#, a l ld, d addp,
  exde, af pop, h pop, 8 b ld#, c? rif
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

( (mode-64o-output_ )

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
  \ (mode-64o-output_  ( -- a )
  \
  \ _a_ is the address of a Z80 routine, the low-level
  \ `mode-64o` driver, which displays the character in the A
  \ register. The Forth IP is not preserved.
  \
  \ ``mode-64o-output_`` is called by `mode-64o-output_` and
  \ `mode-64o-emit``.
  \
  \ }doc

( mode-64o-output_ mode-64o-emit )

need assembler need (mode-64o-output_

create mode-64o-output_ ( -- a )
  asm b push, (mode-64o-output_ call, b pop, ret, end-asm

  \ doc{
  \
  \ mode-64o-output_ ( -- a )
  \
  \ _a_ is the address of a Z80 routine, the entry to
  \ `mode-64o` driver, which preserves the Forth IP and then
  \ displays the character in the A register by calling
  \ `(mode-64o-output_`.
  \
  \ }doc

code mode-64o-emit ( c -- )
  exx, b pop, c a ld, (mode-64o-output_ call,
  exx, jpnext, end-code

  \ doc{
  \
  \ mode-64o-emit ( c -- )
  \
  \ Display character _c_ in `mode-64o`, by calling
  \ `(mode-64o-output_`.
  \
  \ ``mode-64o-emit`` is configured by `mode-64o` as the action
  \ of `emit`.
  \
  \ }doc

( mode-64o )

need mode-64o-output_ need mode-64o-emit
need mode-32 need (at-xy need set-mode-output need >form

: mode-64o-xy ( -- col row )
  mode-64o-column c@ mode-64o-row c@ ;

  \ doc{
  \
  \ mode-64o-xy ( -- col row )
  \
  \ Return the current cursor coordinates _col row_ in
  \ `mode-64o`. ``mode-64o-xy`` is the action of `xy` when
  \ `mode-64o` is active.
  \
  \ }doc

variable mode-64o-font

  \ doc{
  \
  \ mode-64o-font ( -- a )
  \
  \ A variable. _a_ is the address of a cell containing the
  \ address of the 4x8-pixel font used by `mode-64o`. Note the
  \ address of the font must be the address of its character 32
  \ (space). The size of a 4x8-pixel font is 336 bytes. The
  \ program is responsible for initializing the contents of
  \ this variable before executing `mode-64o`.
  \
  \ See also: `owen-64cpl-font`.
  \
  \ }doc

: mode-64o ( -- )
  [ latestxt ] literal current-mode !
  mode-64o-font @ $71 - set-font
  mode-64o-output_ set-mode-output
  ['] mode-64o-emit ['] emit  defer!
  ['] (at-xy        ['] at-xy defer! 64 24 >form
  ['] mode-64o-xy   ['] xy    defer! ;

  \ doc{
  \
  \ mode-64o ( -- )
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
  \ See also: `current-mode`, `set-font`, `set-mode-output`,
  \ `columns`, `rows`, `mode-64o-emit`, `mode-64o-xy`,
  \ `mode-64o-font`, `>form`, `mode-64o-output_`.
  \
  \ }doc

( mode-64s )

  \ Version with integrated driver, adapted from 64#4, written
  \ by Einar Saukas.
  \
  \ https://sites.google.com/site/zxgraph/home/einar-saukas/fonts
  \ http://www.worldofspectrum.org/infoseekid.cgi?id=0027130
  \
  \ XXX UNDER DEVELOPMENT --

need assembler need l:

  \ XXX TODO use common variables for all modes?

create mode-64s-at-flag 0 c,
create mode-64s-column 0 c,
create mode-64s-row 0 c,
variable mode-64s-chars

create mode-64s-emit_ ( -- a ) asm

  \ Input:
  \   A = character code

  \ ; -----------------------------------------------------------------------------
  \ ; 64#4 - 4x8 FONT DRIVER FOR 64 COLUMNS (c) 2007, 2011
  \ ;
  \ ; Original by Andrew Owen (657 bytes)
  \ ; Optimized by Crisis (602 bytes)
  \ ; Reimplemented by Einar Saukas (494 bytes)
  \ ; -----------------------------------------------------------------------------

  \ ; -----------------------------------------------------------------------------
  \ ; CHANNEL WRAPPER FOR THE 64-COLUMN DISPLAY DRIVER
  \ ; Based on code by Tony Samuels from Your Spectrum issue 20, November 1985.

  0 b ld#, mode-64s-at-flag h ldp#, m dec,
  #1 m? ?jp #al  #2 rl# z? ?jr

  \ CH_ADDR:
  \         ld      b, 0            ; save a few bytes later using B instead of 0
  \         ld      hl, AT_FLAG     ; initial address of local variables
  \         dec     (hl)            ; check AT_FLAG value by decrementing it
  \         jp      m, CHK_AT       ; expecting a regular character?
  \         jr      z, GET_COL      ; expecting the AT column?
  \
  \ ; -----------------------------------------------------------------------------
  \ ; UNCOMMENT TO ENABLE STANDARD INVERSE (use INVERSE 1 for inversed characters)
  \ ;
  \ ; #ifdef _STANDARD_INVERSE
  \ ;        dec     (hl)            ; check AT_FLAG value by decrementing it again
  \ ;        jr      nz, GET_ROW     ; expecting the AT row?
  \ ;        and     a               ; check INVERSE parameter
  \ ;        jr      z, SET_INV      ; specified INVERSE zero?
  \ ;        ld      a, 0x2f         ; opcode for 'CPL'
  \ ;SET_INV:
  \ ;        ld      (INV_C), a      ; either 'NOP' or 'CPL'
  \ ;        ret
  \ ; #endif _STANDARD_INVERSE
  \ ; -----------------------------------------------------------------------------
  \

  #24 cp#, h incp,

  \ GET_ROW:
  \         cp      24              ; specified row greater than 23?
  \         jr      nc, ERROR_B     ; error if so
  \         inc     hl              ; dirty trick to store new row into AT_ROW

  #2 l: #64 cp#, nc? rif h incp, a m ld, ret, rthen

  \ GET_COL:
  \         cp      64              ; specified column greater than 63?
  \         jr      nc, ERROR_B     ; error if so
  \         inc     hl
  \         ld      (hl), a         ; store new column into AT_COL
  \         ret

  b m ld, #10 hook,

  \ ERROR_B:
  \         ld      (hl), b         ; reset AT_FLAG
  \         rst     8               ; error "B Integer out of range"
  \         defb    10
  \

  #1 l: 16 cp#,

  \ CHK_AT:
  \         cp      0x16            ; specified keyword 'AT'?
  \
  \ ; -----------------------------------------------------------------------------
  \ ; UNCOMMENT TO ENABLE STANDARD INVERSE (use INVERSE 1 for inversed characters)
  \ ;
  \ ; #ifdef _STANDARD_INVERSE
  \ ;        jr      nz, CHK_INV     ; continue otherwise
  \ ;        ld      (hl), 3         ; change AT_FLAG to expect row value next time
  \ ;        ret
  \ ;CHK_INV:
  \ ;        cp      0x14            ; specified keyword 'INVERSE'?
  \ ; #endif _STANDARD_INVERSE
  \ ; -----------------------------------------------------------------------------
  \

  z? rif 2 m ld#, ret, rthen

  \         jr      nz, CHK_CR      ; continue otherwise
  \         ld      (hl), 2         ; change AT_FLAG to expect row value next time
  \         ret                     ;   (or to expect INVERSE parameter next time)
  \

  -->

( mode-64s )

  m inc, h incp, 0D cp#, #3 rl# z? ?jr,

  \ CHK_CR:
  \         inc     (hl)            ; increment AT_FLAG to restore previous value
  \         inc     hl              ; now HL references AT_COL address
  \         cp      0x0d            ; specified carriage return?
  \         jr      z, NEXT_ROW     ; change row if so
  \
  \ ; -----------------------------------------------------------------------------
  \ ; UNCOMMENT TO ENABLE FAST COMMA (jump directly to next column multiple of 16)
  \ ;
  \ ; #ifdef _FAST_COMMA
  \ ;        cp      0x06            ; specified comma?
  \ ;        jr      nz, DRIVER      ; continue otherwise
  \ ;        ld      a, (hl)
  \ ;        or      0x0f            ; change column to destination minus 1
  \ ;        ld      (hl),a
  \ ;        jr      END_LOOP + 1    ; increment column and row if needed
  \ ; #endif _FAST_COMMA
  \ ; -----------------------------------------------------------------------------
  \
  \ ; -----------------------------------------------------------------------------
  \ ; UNCOMMENT TO ENABLE STANDARD COMMA (print spaces until column multiple of 16)
  \ ;
  \ ; #ifdef _STANDARD_COMMA
  \ ;        cp      0x06            ; specified comma?
  \ ;        jr      nz, DRIVER      ; continue otherwise
  \ ;LOOP:   ld      a, 32           ; print space
  \ ;        call    DRIVER
  \ ;        ret     c               ; stop if row changed (reached column zero)
  \ ;        ld      a, (hl)
  \ ;        and     0x0f
  \ ;        ret     z               ; stop if reached column 16, 32 or 48
  \ ;        jr      LOOP            ; repeat otherwise
  \ ; #endif _STANDARD_COMMA
  \ ; -----------------------------------------------------------------------------
  \
  \ ; -----------------------------------------------------------------------------
  \ ; 64-COLUMN DISPLAY DRIVER
  \

  h push, a e ld, m c ld,

  \ DRIVER:
  \
  \         push    hl              ; save AT_COL address for later
  \         ld      e, a            ; store character value in E
  \         ld      c, (hl)         ; store current column in BC
  \
  \ ; Check if character font must be rotated, self-modifying the code accordingly

  c xor, rra,
  \ XXX TODO --

  \         xor     c               ; compare BIT 0 from character value and column
  \         rra
  \         ld      a, 256-(END_LOOP-SKIP_RLC) ; instruction DJNZ skipping rotation
 
  nc? rif
  \ XXX TODO --

  \         jr      nc, NOT_RLC             ; decide based on BIT 0 comparison
  \         ld      a, 256-(END_LOOP-INIT_RLC) ; instruction DJNZ using rotation
  \ NOT_RLC:
  \         ld      (END_LOOP - 1), a       ; modify DJNZ instruction directly
  \
  \ ; Check the half screen byte to be changed, self-modifying the code accordingly
  \
  \         srl     c               ; check BIT 0 from current column
  \         ld      a, %00001111    ; mask to change left half of the screen byte
  \         jr      nc, SCR_LEFT    ; decide based on odd or even column
  \         cpl                     ; mask to change right half of the screen byte
  \ SCR_LEFT:
  \         ld      (SCR_MASK + 1), a   ; modify screen mask value directly
  \         cpl
  \         ld      (FONT_MASK + 1), a  ; modify font mask value directly
  \
  \ ; Calculate location of the first byte to be changed on screen
  \ ; The row value is a 5 bits value (0-23), here represented as %000RRrrr
  \ ; The column value is a 6 bits value (0-63), here represented as %00CCCCCc
  \ ; Formula: 0x4000 + ((row & 0x18) << 8) + ((row & 0x07) << 5) + (col >> 1)
  \
  \         inc     hl              ; now HL references AT_ROW address
  \         ld      a, (hl)         ; now A = %000RRrrr
  \         call    0x0e9e          ; now HL = %010RR000rrr00000
  \         add     hl, bc          ; now HL = %010RR000rrrCCCCC
  \         ex      de, hl          ; now DE = %010RR000rrrCCCCC
  \
  \ ; Calculate location of the character font data in FONT_ADDR
  \ ; Formula: FONT_ADDR + 7 * INT ((char-32)/2) - 1

  b m ld, l srl, l c ld, h addp, h addp, h addp, b sbcp,
  xxx
  b addp,
  \
  \         ld      h, b            ; now HL = char
  \         srl     l               ; now HL = INT (char/2)
  \         ld      c, l            ; now BC = INT (char/2)
  \         add     hl, hl          ; now HL = 2 * INT (char/2)
  \         add     hl, hl          ; now HL = 4 * INT (char/2)
  \         add     hl, hl          ; now HL = 8 * INT (char/2)
  \         sbc     hl, bc          ; now HL = 7 * INT (char/2)
  \         ld      bc, FONT_ADDR - 0x71
  \         add     hl, bc          ; now HL = FONT_ADDR + 7 * INT (char/2) - 0x71
  \
  \ ; Main loop to copy 8 font bytes into screen (1 blank + 7 from font data)
  \
  \         xor     a               ; first font byte is always blank
  \         ld      b, 8            ; execute loop 8 times

  rbegin rlca, rlca, rlca, rlca,

  \ INIT_RLC:
  \         rlca                    ; switch position between bits 0-3 and bits 4-7
  \         rlca
  \         rlca
  \         rlca
  \ SKIP_RLC:
  \
  \ ; -----------------------------------------------------------------------------
  \ ; UNCOMMENT TO ENABLE STANDARD INVERSE
  \ ;
  \ ; #ifdef _STANDARD_INVERSE
  \ ;INV_C:  nop                     ; either 'NOP' or 'CPL'
  \ ; #endif _STANDARD_INVERSE
  \ ; -----------------------------------------------------------------------------

  %11110000 and#, a c ld, d ftp,

  \ FONT_MASK:
  \         and     %11110000       ; mask half of the font byte
  \         ld      c, a            ; store half of the font byte in C
  \         ld      a, (de)         ; get screen byte

  %00001111 and#, c or, d ftp, d inc, m inc, m a ld, rstep

  \ SCR_MASK:
  \         and     %00001111       ; mask half of the screen byte
  \         or      c               ; combine half screen and half font
  \         ld      (de), a         ; write result back to screen
  \         inc     d               ; next screen location
  \         inc     hl              ; next font data location
  \         ld      a, (hl)         ; store next font byte in A
  \         djnz    INIT_RLC        ; repeat loop 8 times
  \ END_LOOP:

  h pop, m inc, m 6 bit, z? ?ret,

  \         pop     hl              ; restore AT_COL address
  \         inc     (hl)            ; next column
  \         bit     6, (hl)         ; column lower than 64?
  \         ret     z               ; return if so

  #3 l: b m ld, h incp, m inc, m a ld, #24 cp#, c? ?ret,
        b m ld, ret,

  \ NEXT_ROW:
  \         ld      (hl), b         ; reset AT_COL
  \         inc     hl              ; store AT_ROW address in HL
  \         inc     (hl)            ; next row
  \         ld      a, (hl)
  \         cp      24              ; row lower than 23?
  \         ret     c               ; return if so
  \         ld      (hl), b         ; reset AT_ROW
  \         ret                     ; done!
  \
  \ ; -----------------------------------------------------------------------------
  \ ; LOCAL VARIABLES
  \
  \ AT_FLAG:
  \         defb    0               ; flag to control processing keyword 'AT'
  \                                 ;   value 2 if received 'AT', expecting row
  \                                 ;   value 1 if received row, expecting column
  \                                 ;   value 0 if expecting regular character
  \ AT_COL:
  \         defb    0               ; current column position (0-31)
  \ AT_ROW:
  \         defb    0               ; current row position (0-23)


( mode-64s-emit )

need assembler need mode-64s-emit_

code mode-64s-emit ( c -- )
  exx, b pop, c a ld, mode-64s-emit_ call,
  exx, jpnext, end-code

( mode-64s )

need (at-xy need set-mode-output need mode-64s-emit

: mode-64s-xy ( -- col row ) 0 0 ;  \ XXX TODO

: mode-64s ( -- )
  [ latestxt ] literal current-mode !
  2548 set-mode-output
  mode-64s-chars @ set-font
  64 to columns  24 to rows
  ['] mode-64s-emit ['] emit  defer!
  ['] mode-64s-xy   ['] xy    defer!
  ['] (at-xy       ['] at-xy defer! ;

( owen-64cpl-font )

create owen-64cpl-font ( -- a ) hex

  \ Half width 4x8 font.
  \ 336 bytes.
  \ Top row is always zero and not stored.

02 c, 02 c, 02 c, 02 c, 00 c, 02 c, 00 c,  \  !
52 c, 57 c, 02 c, 02 c, 07 c, 02 c, 00 c,  \ "#
25 c, 71 c, 62 c, 32 c, 74 c, 25 c, 00 c,  \ $%
22 c, 42 c, 30 c, 50 c, 50 c, 30 c, 00 c,  \ &'
14 c, 22 c, 41 c, 41 c, 41 c, 22 c, 14 c,  \ ()
20 c, 70 c, 22 c, 57 c, 02 c, 00 c, 00 c,  \ *+
00 c, 00 c, 00 c, 07 c, 00 c, 20 c, 20 c,  \ ,-
01 c, 01 c, 02 c, 02 c, 04 c, 14 c, 00 c,  \ ./
22 c, 56 c, 52 c, 52 c, 52 c, 27 c, 00 c,  \ 01
27 c, 51 c, 12 c, 21 c, 45 c, 72 c, 00 c,  \ 23
57 c, 54 c, 56 c, 71 c, 15 c, 12 c, 00 c,  \ 45
17 c, 21 c, 61 c, 52 c, 52 c, 22 c, 00 c,  \ 67
22 c, 55 c, 25 c, 53 c, 52 c, 24 c, 00 c,  \ 89
-->

( owen-64cpl-font )

00 c, 00 c, 22 c, 00 c, 00 c, 22 c, 02 c,  \ :;
00 c, 10 c, 27 c, 40 c, 27 c, 10 c, 00 c,  \ <=
02 c, 45 c, 21 c, 12 c, 20 c, 42 c, 00 c,  \ >?
23 c, 55 c, 75 c, 77 c, 45 c, 35 c, 00 c,  \ @A
63 c, 54 c, 64 c, 54 c, 54 c, 63 c, 00 c,  \ BC
67 c, 54 c, 56 c, 54 c, 54 c, 67 c, 00 c,  \ DE
73 c, 44 c, 64 c, 45 c, 45 c, 43 c, 00 c,  \ FG
57 c, 52 c, 72 c, 52 c, 52 c, 57 c, 00 c,  \ HI
35 c, 15 c, 16 c, 55 c, 55 c, 25 c, 00 c,  \ JK
45 c, 47 c, 45 c, 45 c, 45 c, 75 c, 00 c,  \ LM
62 c, 55 c, 55 c, 55 c, 55 c, 52 c, 00 c,  \ NO
62 c, 55 c, 55 c, 65 c, 45 c, 43 c, 00 c,  \ PQ
63 c, 54 c, 52 c, 61 c, 55 c, 52 c, 00 c,  \ RS
75 c, 25 c, 25 c, 25 c, 25 c, 22 c, 00 c,  \ TU
-->

( owen-64cpl-font )

55 c, 55 c, 55 c, 55 c, 27 c, 25 c, 00 c,  \ VW
55 c, 55 c, 25 c, 22 c, 52 c, 52 c, 00 c,  \ XY
73 c, 12 c, 22 c, 22 c, 42 c, 72 c, 03 c,  \ Z[
46 c, 42 c, 22 c, 22 c, 12 c, 12 c, 06 c,  \ \]
20 c, 50 c, 00 c, 00 c, 00 c, 00 c, 0F c,  \ ^_
20 c, 10 c, 03 c, 05 c, 05 c, 03 c, 00 c,  \ ?a
40 c, 40 c, 63 c, 54 c, 54 c, 63 c, 00 c,  \ bc
10 c, 10 c, 32 c, 55 c, 56 c, 33 c, 00 c,  \ de
10 c, 20 c, 73 c, 25 c, 25 c, 43 c, 06 c,  \ fg
42 c, 40 c, 66 c, 52 c, 52 c, 57 c, 00 c,  \ hi
14 c, 04 c, 35 c, 16 c, 15 c, 55 c, 20 c,  \ jk
60 c, 20 c, 25 c, 27 c, 25 c, 75 c, 00 c,  \ lm
00 c, 00 c, 62 c, 55 c, 55 c, 52 c, 00 c,  \ no
00 c, 00 c, 63 c, 55 c, 55 c, 63 c, 41 c,  \ pq
-->

( owen-64cpl-font )

00 c, 00 c, 53 c, 66 c, 43 c, 46 c, 00 c,  \ rs
00 c, 20 c, 75 c, 25 c, 25 c, 12 c, 00 c,  \ tu
00 c, 00 c, 55 c, 55 c, 27 c, 25 c, 00 c,  \ vw
00 c, 00 c, 55 c, 25 c, 25 c, 53 c, 06 c,  \ xy
01 c, 02 c, 72 c, 34 c, 62 c, 72 c, 01 c,  \ z{
24 c, 22 c, 22 c, 21 c, 22 c, 22 c, 04 c,  \ |}
56 c, A9 c, 06 c, 04 c, 06 c, 09 c, 06 c,  \ ~?

decimal

  \ Credit:
  \
  \ Author of the font: Andrew Owen.
  \ Published on the World of Spectrum forum:
  \ http://www.worldofspectrum.org/forums/discussion/14526/redirect/p1

  \ doc{
  \
  \ owen-64cpl-font ( -- a )
  \
  \ _a_ is the address of a 4x8-pixel font compiled in data
  \ space (336 bytes used), to be used in `mode-64o` by setting
  \ `mode-64o-font`.
  \
  \ This font is included also is disk 0 as "owen.f64".
  \
  \ Author of the font: Andrew Owen.
  \
  \ }doc

  \ ===========================================================
  \ Change log

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
  \
  \ 2017-12-05: Advance the conversion of the `mode-64s`'s
  \ code.

  \ vim: filetype=soloforth
