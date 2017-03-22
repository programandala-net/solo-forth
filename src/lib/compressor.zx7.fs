  \ compressor.zx7.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201703221600
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Port of the ZX7 decompressor written by Einar Saukas et al.

  \ ===========================================================
  \ Authors

  \ ZX7 decoder "Standard" by Einar Saukas, Antonio Villena &
  \ Metalbrain.
  \
  \ ZX7 decoder "Turbo" by Einar Saukas & Urusergi.
  \
  \ Port to Solo Forth by Marcos Cruz (programandala.net),
  \ 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( zx7s )

need assembler also assembler need l: previous

code zx7s ( a1 a2 -- )

exx, d pop, h pop, 80 a ld#, rbegin ldi,
  \         exx                           ; save Forth IP
  \         pop  de                       ; destination
  \         pop  hl                       ; origin
  \         ld   a, $80
  \ dzx7s_copy_byte_loop:
  \         ldi                           ; copy literal byte
  \ dzx7s_main_loop:
#0 l: #2 call, al# c? runtil
  \         call dzx7s_next_bit
  \         jr   nc, dzx7s_copy_byte_loop ; next bit indicates either literal or sequence

  \ ; determine number of bits used for length (Elias gamma coding)
d push, 0 b ldp#, b d ld, rbegin d inc,
  \         push de
  \         ld   bc, 0
  \         ld   d, b
  \ dzx7s_len_size_loop:
  \         inc  d
#2 call, al# c? runtil
  \         call dzx7s_next_bit
  \         jr   nc, dzx7s_len_size_loop

  \ ; determine length
  \ dzx7s_len_value_loop:
rbegin #2 nc? ?call, al# c rl, b rl, #1 rl# c? ?jr, d dec,
z? runtil b incp,
  \         call nc, dzx7s_next_bit
  \         rl   c
  \         rl   b
  \         jr   c, dzx7s_exit            ; check end marker
  \         dec  d
  \         jr   nz, dzx7s_len_value_loop
  \         inc  bc                       ; adjust length

  \ ; determine offset
m e ld, h incp, e sll, c? rif 10 d ld#,
  \         ld   e, (hl)                  ; load offset flag (1 bit) + offset value (7 bits)
  \         inc  hl
  \         defb $cb, $33                 ; opcode for undocumented instruction "SLL E" aka "SLS E"
  \         jr   nc, dzx7s_offset_end     ; if offset flag is set, load 4 extra bits
  \         ld   d, $10                   ; bit marker to load 4 bits
  \ dzx7s_rld_next_bit:
rbegin #2 call, al# d rl, c? runtil d inc, d srl,
  \         call dzx7s_next_bit
  \         rl   d                        ; insert next bit into D
  \         jr   nc, dzx7s_rld_next_bit   ; repeat 4 times, until bit marker is out
  \         inc  d                        ; add 128 to DE
  \         srl  d                        ; retrieve fourth bit from D

  \ dzx7s_offset_end:
rthen e rr, exsp, h push, d sbcp, d pop, ldir,
  \         rr   e                        ; insert fourth bit into E

  \ ; copy previous sequence
  \         ex   (sp), hl                 ; store source, restore destination
  \         push hl                       ; store destination
  \         sbc  hl, de                   ; HL = destination - offset - 1
  \         pop  de                       ; DE = destination
  \         ldir
  \ dzx7s_exit:
#1 l: h pop, #0 rl# nc? ?jr, #2 call, al# exx, jpnext,
  \         pop  hl                       ; restore source address (compressed data)
  \         jr   nc, dzx7s_main_loop
  \         call dzx7s_next_bit
  \         exx                           ; restore Forth IP
  \         _jp_next                      ; exit to next word

  \ dzx7s_next_bit:
#2 l: a add, nz? ?ret, m a ld, h incp, rla, ret, end-code
  \         add  a, a                     ; check next bit
  \         ret  nz                       ; no more bits left?
  \         ld   a, (hl)                  ; load another group of 8 bits
  \         inc  hl
  \         rla
  \         ret

  \ doc{
  \
  \ zx7s ( a1 a2 -- )
  \
  \ Decompress data from _a1_ and copy the result to _a2_.
  \
  \ ``zx7s`` is the port of the ZX7 decoder, "Standard"
  \ version, written by Einar Saukas, Antonio Villena &
  \ Metalbrain.
  \
  \ ``zx7s`` is the smallest (87 bytes) and slowest version of
  \ the decoder. `zx7t` is bigger and faster.
  \
  \ For more information, see the
  \ http://www.worldofspectrum.org/infoseekid.cgi?id=0027996[thread
  \ about ZX7 in the World of Spectrum forum].
  \
  \ }doc

( zx7t )

need assembler also assembler need l: previous unused

code zx7t ( a1 a2 -- )

exx, d pop, h pop, 80 a ld#, rbegin ldi,
  \         exx                           ; save Forth IP
  \         pop  de                       ; destination
  \         pop  hl                       ; origin
  \         ld      a, $80
  \ dzx7t_copy_byte_loop:
  \         ldi                             ; copy literal byte
  \ dzx7t_main_loop:
#0 l: a add, #3 z? ?call, al# c? runtil
  \         add     a, a                    ; check next bit
  \         call    z, dzx7t_load_bits      ; no more bits left?
  \         jr      nc, dzx7t_copy_byte_loop ; next bit indicates either literal or sequence
  \
  \ ; determine number of bits used for length (Elias gamma coding)
d push, 1 b ldp#, b d ld,
  \         push    de
  \         ld      bc, 1
  \         ld      d, b
  \ dzx7t_len_size_loop:
rbegin d inc, a add, #3 z? ?call, al# c? runtil #1 jp, al#
  \         inc     d
  \         add     a, a                    ; check next bit
  \         call    z, dzx7t_load_bits      ; no more bits left?
  \         jr      nc, dzx7t_len_size_loop
  \         jp      dzx7t_len_value_start
  \
  \ ; determine length
  \ dzx7t_len_value_loop:
rbegin a add, #3 z? ?call, al# c rl, b rl, #2 rl# c? ?jr,
  \         add     a, a                    ; check next bit
  \         call    z, dzx7t_load_bits      ; no more bits left?
  \         rl      c
  \         rl      b
  \         jr      c, dzx7t_exit           ; check end marker
  \ dzx7t_len_value_start:
#1 l: d dec, z? runtil b incp, m e ld, h incp, e sll,
  \         dec     d
  \         jr      nz, dzx7t_len_value_loop
  \         inc     bc                      ; adjust length
  \
  \ ; determine offset
  \
  \         ld      e, (hl)                 ; load offset flag (1 bit) + offset value (7 bits)
  \         inc     hl
  \         defb    $cb, $33                ; opcode for undocumented instruction "SLL E" aka "SLS E"

c? rif a add, #3 z? ?call, al# d rl, a add, #3 z? ?call, al#
  \         jr      nc, dzx7t_offset_end    ; if offset flag is set, load 4 extra bits
  \         add     a, a                    ; check next bit
  \         call    z, dzx7t_load_bits      ; no more bits left?
  \         rl      d                       ; insert first bit into D
  \         add     a, a                    ; check next bit
  \         call    z, dzx7t_load_bits      ; no more bits left?
d rl, a add, #3 z? ?call, al# d rl, a add, #3 z? ?call, al#
  \         rl      d                       ; insert second bit into D
  \         add     a, a                    ; check next bit
  \         call    z, dzx7t_load_bits      ; no more bits left?
  \         rl      d                       ; insert third bit into D
  \         add     a, a                    ; check next bit
  \         call    z, dzx7t_load_bits      ; no more bits left?
ccf, nc? rif d inc, rthen
  \         ccf
  \         jr      c, dzx7t_offset_end
  \         inc     d                       ; equivalent to adding 128 to DE
  \ dzx7t_offset_end:
rthen e rr, exsp, h push, d sbcp, d pop, ldir,
  \         rr      e                       ; insert inverted fourth bit into E
  \
  \ ; copy previous sequence
  \         ex      (sp), hl                ; store source, restore destination
  \         push    hl                      ; store destination
  \         sbc     hl, de                  ; HL = destination - offset - 1
  \         pop     de                      ; DE = destination
  \         ldir
  \ dzx7t_exit:
#2 l: h pop, #0 nc? ?jp, al# #3 call, al# exx, jpnext,
  \         pop     hl                      ; restore source address (compressed data)
  \         jp      nc, dzx7t_main_loop
  \         call dzx7t_load_bits
  \         exx                           ; restore Forth IP
  \         _jp_next                      ; exit to next word

#3 l: m a ld, h incp, rla, ret, end-code
  \ dzx7t_load_bits:
  \         ld      a, (hl)                 ; load another group of 8 bits
  \         inc     hl
  \         rla
  \         ret

  \ doc{
  \
  \ zx7t ( a1 a2 -- )
  \
  \ Decompress data from _a1_ and copy the result to _a2_.
  \
  \ ``zx7t`` is the port of the ZX7 decoder, "Turbo" version,
  \ written by Einar Saukas & Urusergi.
  \
  \ ``zx7t`` is 25% faster than `zx7s`, and needs only 10 more
  \ bytes (97 bytes in total).
  \
  \ For more information, see the following
  \ http://www.worldofspectrum.org/infoseekid.cgi?id=0027996[thread
  \ about ZX7 in the World of Spectrum forum].
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2017-02-28: Start adapting the original code.
  \
  \ 2017-03-02: Complete the standard version of the
  \ decompressor and a test code. The decompressed data is
  \ corrupted.
  \
  \ 2017-03-20: Fix file header and change log.
  \
  \ 2017-03-21: Adapt the code to the new implementation of
  \ `l:`, which supports relative and absolute references. The
  \ previous implementation of `l:` didn't support absolute
  \ references, and this caused the problems.
  \
  \ 2017-03-22: Add `zx7t`. Improve documentation.

  \ vim: filetype=soloforth
