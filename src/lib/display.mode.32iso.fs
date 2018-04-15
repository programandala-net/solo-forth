  \ display.mode.32iso.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201804142247
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ An alternative 32-cpl display mode that can use an ISO
  \ character set.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( mode-32iso )

need assembler need os-chars

create mode-32iso-output_ ( -- a ) asm

b push, 0 b ldp#, #128 cp#, nc? rif
  \   push bc                      ; preserve the Forth IP
  \   ld bc,0                      ; font offset for characters 0..127
  \   cp 128                       ; is the character in range 0..127?
  \   jr c,mode_32iso_emit.display ; if so, display it and finish

  #224 cp#, c? rif #96 sub#, #96 8 * b ldp#,
               relse #192 sub#, #192 8 * b ldp#, rthen
  \   cp 224                    ; is the character in range 128..223?
  \   jr nc,mode_32iso_emit.224

  \   ; character 128..223

  \   sub 96                     ; convert character range to 32..127
  \   ld bc,96*8                 ; offset for the font address
  \   jr mode_32iso_emit.display

  \ mode_32iso_emit.224:

  \   ; character 224..255

  \   sub 192                     ; convert character range to 32..63
  \   ld bc,192*8                 ; offset for the font address

  \ mode_32iso_emit.display:
rthen

  \   ; Display the character in the A register from the current
  \   ; font, adding offset BC to the font address.

os-chars h ftp, h push, b addp, os-chars h stp, FF 52 iy st#x,
  \   ld hl,(sys_chars)
  \   push hl                       ; save the font address
  \   add hl,bc                     ; apply the offset
  \   ld (sys_chars),hl             ; update the font address
  \   ld (iy+sys_scr_ct_offset),$FF ; no scroll message

  \ 10 rst,  \ XXX OLD
  \   rst $10
  09F4 call, \ XXX NEW
  \   call $09F4                    ; PRINT-OUT ROM routine

h pop, os-chars h stp, b pop, ret, end-asm
  \   pop hl
  \   ld (sys_chars),hl             ; restore the font address
  \   pop bc                        ; restore the Forth IP
  \   ret

  \ doc{
  \
  \ mode-32iso-output_  ( -- a ) "mode-32-iso-output-underscore"
  \
  \ _a_ is the address of a Z80 routine, the `mode-32iso`
  \ driver, which displays the character in the A register.
  \ calling the ROM routine at $09F4, but assuming the current
  \ font set by `set-font` contains printable characters
  \ 32..255.
  \
  \ In order to force the ROM routine interpret characters
  \ 128..255 as ordinary characters (not block graphics, user
  \ defined graphics or BASIC tokens, as `mode-32-emit` does),
  \ ``mode-32iso-emit`` modifies _c_ if needed and moves the
  \ current font address accordingly before calling the ROM.
  \ As a result, the ROM routine treats character ranges
  \ 128..223 and 224..255 as 32..127 and 32..63 respectively.
  \
  \ ``mode-32iso-output_`` is called by `mode-32iso-emit`.
  \
  \ ``mode-32iso-output_`` is activated by `mode-32iso`, i.e.
  \ it's set as the output routine of the current channel.
  \
  \ }doc

code mode-32iso-emit ( c -- )
  h pop, l a ld, mode-32iso-output_ call, jpnext, end-code
  \   pop hl            ; L = character
  \   ld a,l
  \   call mode_32_iso_output_
  \   _jp_next

  \ XXX TODO -- Add multitasker's `pause` when available.

  \ doc{
  \
  \ mode-32iso-emit ( c -- )
  \
  \ Display character _c_ in `mode-32iso`, i.e.  using the ROM
  \ routines but assuming the current font set by `set-font`
  \ contains printable characters 32..255. See the low-level
  \ factor `mode-32iso-output_` for details how this is
  \ achieved.
  \
  \ ``mode-32iso-emit`` is not affected by `last-font-char`.
  \
  \ ``mode-32iso-emit`` is a wrapper word which preserves the
  \ Forth IP and calls `mode-32iso-output_`.
  \
  \ }doc

-->

( mode-32iso )

need mode-32 need columns need rows need set-mode-output
need >form need rom-font

variable mode-32iso-font  rom-font bl 8 * + mode-32iso-font !

  \ doc{
  \
  \ mode-32iso-font ( -- a )
  \
  \ A variable. _a_ is the address of a cell containing the
  \ address of the font used by `mode-32iso`. Note the address
  \ of the font must be the address of its character 32
  \ (space).
  \
  \ The default value of ``mode-32iso-font`` is `rom-font` plus
  \ 256 (the address of the space character in the ROM font).
  \
  \ }doc

: mode-32iso ( -- )
  [ latestxt ] literal current-mode !
  mode-32iso-font @ 256 - set-font
  mode-32iso-output_ set-mode-output
  ['] mode-32iso-emit ['] emit  defer!
  ['] mode-32-at-xy   ['] at-xy defer! 32 24 >form
  ['] mode-32-xy      ['] xy    defer! ;

  \ doc{
  \
  \ mode-32iso ( -- )
  \
  \ Activate the 32-cpl display mode,  an alternative to the
  \ default `mode-32`.  The only difference with `mode-32` is
  \ ``mode-32iso`` can use a ISO character set, i.e. it
  \ displays characters 32..255 from the current font.  See
  \ `mode-32iso-emit` for details.
  \
  \ ``mode-32iso`` is useful when a ISO character set is
  \ required (or any character set with more than 128
  \ characters). A similar result could be obtained with
  \ `mode-32` and `last-font-char`, by treating the characters
  \ greater than 128 as UDG and using `set-udg`.  The advantage
  \ of `mode-32iso` is the ISO font can be managed (e.g.
  \ built, loaded from disk, allocated, etc.) as a whole, using
  \ only the font address, and reserving the full UDG set for
  \ graphics.
  \
  \ See: `current-mode`, `set-font`, `set-mode-output`,
  \ `columns`, `rows`, `mode-32-xy`, `mode-32-at-xy`, `>form`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2017-04-21: Start. Move `mode-32iso-emit` from the kernel.
  \ Write `mode-32iso`.
  \
  \ 2017-04-23: Improve documentation.
  \
  \ 2017-05-14: Improve layout of the Z80 source comments.
  \
  \ 2017-05-15: Use `>form` for mode transition. Add
  \ `mode-32iso-font`. Improve documentation. Factor
  \ `mode-32iso-output_` from `mode-32iso-emit` to reuse it
  \ with `set-mode-output`.
  \
  \ 2018-03-08: Add words' pronunciaton.
  \
  \ 2018-04-14: Fix markup in documentation.

  \ vim: filetype=soloforth

