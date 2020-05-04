  \ display.mode.64es.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201802012254
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ A 64-cpl display mode.

  \ XXX UNDER DEVELOPMENT --

  \ ===========================================================
  \ Authors

  \ Author of the original code: Andrew Owen, 2007.
  \ Published on the World of Spectrum forum:
  \ http://www.worldofspectrum.org/forums/discussion/14526/redirect/p1
  \
  \ Version with integrated driver, adapted from 64#4, written
  \ by Einar Saukas.
  \
  \ https://sites.google.com/site/zxgraph/home/einar-saukas/fonts
  \ http://www.worldofspectrum.org/infoseekid.cgi?id=0027130

  \ Marcos Cruz (programandala.net) adapted it to Solo Forth,
  \ 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( mode-64es )

need assembler need l: need hook,

  \ XXX TODO use common variables for all modes?

create mode-64es-at-flag 0 c,
create mode-64es-column 0 c,
create mode-64es-row 0 c,

create mode-64es-emit_ ( -- a ) asm

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

  0 b ld#, mode-64es-at-flag h ldp#, m dec,
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

( mode-64es )

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


( mode-64es-emit )

need assembler need mode-64es-emit_

code mode-64es-emit ( c -- )
  exx, b pop, c a ld, mode-64es-emit_ call,
  exx, jpnext, end-code

( mode-64es )

need (at-xy need set-mode-output need mode-64es-emit
need mode-64-font

: mode-64es-xy ( -- col row ) 0 0 ;  \ XXX TODO

: mode-64es ( -- )
  [ latestxt ] literal current-mode !
  2548 set-mode-output
  mode-64-font @ set-font
  64 to columns  24 to rows
  ['] mode-64es-emit ['] emit  defer!
  ['] mode-64es-xy   ['] xy    defer!
  ['] (at-xy       ['] at-xy defer! ;

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

  \ 2017-12-05: Advance the conversion of the `mode-64s`'s
  \ code.  Extract from <display.mode.64.fs>.  Replace
  \ `mode-64s-chars` with `mode-64-font`.
  \
  \ 2018-01-24: Update after the renaming of all display modes
  \ files and words: "64s" (Saukas) -> "64es" (Einar Saukas).
  \
  \ 2018-02-01: Need `hook,`, which has been made optional.

  \ vim: filetype=soloforth
