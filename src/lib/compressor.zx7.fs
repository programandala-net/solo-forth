  \ compressor.zx7.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702282240

  \ XXX UNDER DEVELOPMENT

  \ -----------------------------------------------------------
  \ Description

  \ Port of the ZX7 decompressor written by
  \ block.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ History

  \ 2016-11-26: Create this module to combine the modules that
  \ contain small definers, in order to save blocks:
  \ <define.semicolon.code.fsb>, <define.colon-no-name.fsb>,
  \ <define.colon-nextname.fsb>, <flow.create-colon.fsb>.
  \

( zx7s )

need assembler need l:

code zx7s ( a1 a2 -- )

  exx, d pop, h pop, 80 a ld#, rbegin ldi,
  \         exx                             ; save Forth IP
  \         pop     de                      ; destination
  \         pop     hl                      ; origin
  \         ld      a, $80
  \ dzx7s_copy_byte_loop:
  \         ldi                             ; copy literal byte
  \ dzx7s_main_loop:
  2 l: 0 l# call, c? runtil
  \         call    dzx7s_next_bit
  \         jr      nc, dzx7s_copy_byte_loop ; next bit indicates either literal or sequence

  \ ; determine number of bits used for length (Elias gamma coding)
  d push, 0 b ldp#, b d ld, rbegin d inc, 0 l# call, c? runtil
  \         push    de
  \         ld      bc, 0
  \         ld      d, b
  \ dzx7s_len_size_loop:
  \         inc     d
  \         call    dzx7s_next_bit
  \         jr      nc, dzx7s_len_size_loop

  \ ; determine length
  \ dzx7s_len_value_loop:
  rbegin 0 l# nc? ?call, c rl, b rl, 1 l# c? ?jr, d dec,
         z? runtil b incp,
  \         call    nc, dzx7s_next_bit
  \         rl      c
  \         rl      b
  \         jr      c, dzx7s_exit           ; check end marker
  \         dec     d
  \         jr      nz, dzx7s_len_value_loop
  \         inc     bc                      ; adjust length

  \ ; determine offset
  m e ld, h incp, CB c, 33 c, c? rif 10 d ld#,
  \         ld      e, (hl)                 ; load offset flag (1 bit) + offset value (7 bits)
  \         inc     hl
  \         defb    $cb, $33                ; opcode for undocumented instruction "SLL E" aka "SLS E"
  \         jr      nc, dzx7s_offset_end    ; if offset flag is set, load 4 extra bits
  \         ld      d, $10                  ; bit marker to load 4 bits
  \ dzx7s_rld_next_bit:
  rbegin 0 l# call, d rl, c? runtil d inc, d srl,
  \         call    dzx7s_next_bit
  \         rl      d                       ; insert next bit into D
  \         jr      nc, dzx7s_rld_next_bit  ; repeat 4 times, until bit marker is out
  \         inc     d                       ; add 128 to DE
  \         srl	d			; retrieve fourth bit from D

  \ dzx7s_offset_end:
  rthen e rr,
  \         rr      e                       ; insert fourth bit into E

  \ ; copy previous sequence
  exsp, h push, d sbcp, d pop, ldir,
  \         ex      (sp), hl                ; store source, restore destination
  \         push    hl                      ; store destination
  \         sbc     hl, de                  ; HL = destination - offset - 1
  \         pop     de                      ; DE = destination
  \         ldir
  \ dzx7s_exit:
  1 l: h pop, 2 l# nc? ?jr,
  \         pop     hl                      ; restore source address (compressed data)
  \         jr      nc, dzx7s_main_loop
  0 l# call, exx, jpnext,
  \         call dzx7s_next_bit
  \         exx                             ; restore Forth IP
  \         _jp_next                        ; exit to next word

  \ dzx7s_next_bit:
  0 l: a add, nz? ?ret, m a ld, h incp, rla, ret, end-code
  \         add     a, a                    ; check next bit
  \         ret     nz                      ; no more bits left?
  \         ld      a, (hl)                 ; load another group of 8 bits
  \         inc     hl
  \         rla
  \         ret

  \ Credit:
  \
  \ ZX7 decoder by Einar Saukas, Antonio Villena & Metalbrain
  \ "Standard" version (69 bytes only)

  \ doc{
  \
  \ zx7s  ( a1 a2 -- )
  \
  \ Decompress data from _a1_ and copy the result to _a2_.
  \
  \ ``zx7s`` is the standard version of the ZX7 decoder written
  \ by Einar Saukas, Antonio Villena & Metalbrain.
  \
  \ For more information, see the following
  \ http://www.worldofspectrum.org/infoseekid.cgi?id=0027996[thread
  \ about ZX7 in the World of Spectrum forum].
  \
  \ }doc

  \ ============================================================
  \ Original code

  \ Credit:
  \
  \ ZX7 decoder by Einar Saukas, Antonio Villena & Metalbrain
  \ "Standard" version (69 bytes only)

  \ ------------------------------------------------------------
  \ Parameters:
  \   HL: source address (compressed data)
  \   DE: destination address (decompressing)
  \ ------------------------------------------------------------

  \ dzx7_standard:
  \         ld      a, $80
  \ dzx7s_copy_byte_loop:
  \         ldi                             ; copy literal byte
  \ dzx7s_main_loop:
  \         call    dzx7s_next_bit
  \         jr      nc, dzx7s_copy_byte_loop ; next bit indicates either literal or sequence

  \ ; determine number of bits used for length (Elias gamma coding)
  \         push    de
  \         ld      bc, 0
  \         ld      d, b
  \ dzx7s_len_size_loop:
  \         inc     d
  \         call    dzx7s_next_bit
  \         jr      nc, dzx7s_len_size_loop

  \ ; determine length
  \ dzx7s_len_value_loop:
  \         call    nc, dzx7s_next_bit
  \         rl      c
  \         rl      b
  \         jr      c, dzx7s_exit           ; check end marker
  \         dec     d
  \         jr      nz, dzx7s_len_value_loop
  \         inc     bc                      ; adjust length

  \ ; determine offset
  \         ld      e, (hl)                 ; load offset flag (1 bit) + offset value (7 bits)
  \         inc     hl
  \         defb    $cb, $33                ; opcode for undocumented instruction "SLL E" aka "SLS E"
  \         jr      nc, dzx7s_offset_end    ; if offset flag is set, load 4 extra bits
  \         ld      d, $10                  ; bit marker to load 4 bits
  \ dzx7s_rld_next_bit:
  \         call    dzx7s_next_bit
  \         rl      d                       ; insert next bit into D
  \         jr      nc, dzx7s_rld_next_bit  ; repeat 4 times, until bit marker is out
  \         inc     d                       ; add 128 to DE
  \         srl	d			; retrieve fourth bit from D
  \ dzx7s_offset_end:
  \         rr      e                       ; insert fourth bit into E

  \ ; copy previous sequence
  \         ex      (sp), hl                ; store source, restore destination
  \         push    hl                      ; store destination
  \         sbc     hl, de                  ; HL = destination - offset - 1
  \         pop     de                      ; DE = destination
  \         ldir
  \ dzx7s_exit:
  \         pop     hl                      ; restore source address (compressed data)
  \         jr      nc, dzx7s_main_loop
  \ dzx7s_next_bit:
  \         add     a, a                    ; check next bit
  \         ret     nz                      ; no more bits left?
  \         ld      a, (hl)                 ; load another group of 8 bits
  \         inc     hl
  \         rla
  \         ret

( zx7s-test )

page
  .( zx7s-test loading) cr
need zx7s  need file>
create compresssed 6912 allot

  .( Press any key to load the) cr
  .( compressed screen from the) cr
  .( first disk drive. 'Q' to quit.) key 'q' ?exit
s" img.zx7"  compressed 0 file>

  .( Press any key to decompress and) cr
  .( display the screen. 'Q' to quit.) key 'q' ?exit
compressed 16384 zx7s


  \ vim: filetype=soloforth
