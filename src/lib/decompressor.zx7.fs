  \ decompressor.zx7.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201703252038
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ ZX7 decompressors.
  \ http://www.worldofspectrum.org/infoseekid.cgi?id=0027996

  \ ===========================================================
  \ Authors

  \ ZX7 decompressor "Standard" by Einar Saukas, Antonio
  \ Villena & Metalbrain, 2012, 2013.
  \
  \ ZX7 decompressor "Turbo" by Einar Saukas & Urusergi, 2012,
  \ 2013.
  \
  \ ZX7 decompressor "Mega" by Einar Saukas, 2012, 2013.
  \
  \ Solo Forth ports by Marcos Cruz (programandala.net), 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( dzx7s )

need assembler also assembler need l: previous

code dzx7s ( a1 a2 -- )

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
  \ dzx7s ( a1 a2 -- )
  \
  \ Decompress data, which has been compressed by ZX7, from
  \ _a1_ and copy the result to _a2_.
  \
  \ ``dzx7s`` is the port of the ZX7 decompressor, "Standard"
  \ version, written by Einar Saukas, Antonio Villena &
  \ Metalbrain.
  \
  \ ``dzx7s`` is the smallest (87 bytes) but slowest version of
  \ the decompressor. `dzx7t` and `dzx7m` are bigger but
  \ faster.
  \
  \ For more information, see
  \ http://www.worldofspectrum.org/infoseekid.cgi?id=0027996
  \ ZX7 in World of Spectrum].
  \
  \ }doc

( dzx7t )

need assembler also assembler need l: previous

code dzx7t ( a1 a2 -- )

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
  \ dzx7t ( a1 a2 -- )
  \
  \ Decompress data, which has been compressed by ZX7, from
  \ _a1_ and copy the result to _a2_.
  \
  \ ``dzx7t`` is the port of the ZX7 decompressor, "Turbo"
  \ version, written by Einar Saukas & Urusergi.
  \
  \ ``dzx7t`` is 25% faster than `dzx7s`, and needs only 10
  \ more bytes (97 bytes in total). `dzx7m` is bigger but
  \ faster.
  \
  \ For more information, see
  \ http://www.worldofspectrum.org/infoseekid.cgi?id=0027996
  \ ZX7 in World of Spectrum].
  \
  \ }doc

( dzx7m )

need assembler also assembler need l:
max-labels c@ #23 max-labels c! previous

code dzx7m ( a1 a2 -- )

exx, d pop, h pop, 80 a ld#, rbegin ldi,
  \         exx                           ; save Forth IP
  \         pop  de                       ; destination
  \         pop  hl                       ; origin
  \         ld   a, $80
  \ dzx7m_copy_byte_loop_ev:
  \         ldi                             ; copy literal byte
#0 l: a add, #8 rl# z? ?jr, #3 rl# c? ?jr,
  \ dzx7m_main_loop_ev:
  \         add     a, a                    ; check next bit
  \         jr      z, dzx7m_load_bits1     ; no more bits left?
  \         jr      c, dzx7m_len_size_start_od ; next bit indicates either literal or sequence
  \
#1 l: ldi, #22 l: a add, c? runtil
  \ dzx7m_copy_byte_loop_od:
  \         ldi                             ; copy literal byte
  \ dzx7m_main_loop_od:
  \         add     a, a                    ; check next bit
  \         jr      nc, dzx7m_copy_byte_loop_ev ; next bit indicates either literal or sequence
  \
  \ dzx7m_len_size_start_ev:
d push, 1 b ldp#, b d ld,
  \ ; determine number of bits used for length (Elias gamma coding)
  \         push    de
  \         ld      bc, 1
  \         ld      d, b
#2 l: d inc, a add, #9 rl# z? ?jr, #2 rl# nc? ?jr, #5 jp, al#
  \ dzx7m_len_size_loop_ev:
  \         inc     d
  \         add     a, a                    ; check next bit
  \         jr      z, dzx7m_load_bits2_ev  ; no more bits left?
  \         jr      nc, dzx7m_len_size_loop_ev
  \         jp      dzx7m_len_value_start_ev
  \
#3 l: d push, 1 b ldp#, b d ld,
  \ dzx7m_len_size_start_od:
  \ ; determine number of bits used for length (Elias gamma coding)
  \         push    de
  \         ld      bc, 1
  \         ld      d, b
#4 l: d inc, a add, #10 rl# z? ?jr, #4 rl# nc? ?jr, #15 jp, al#
  \ dzx7m_len_size_loop_od:
  \         inc     d
  \         add     a, a                    ; check next bit
  \         jr      z, dzx7m_load_bits2_od  ; no more bits left?
  \         jr      nc, dzx7m_len_size_loop_od
  \         jp      dzx7m_len_value_start_od
  \
  \ ; determine length
rbegin a add, #11 rl# z? ?jr, c rl, b rl, #12 rl# c? ?jr,
  \ dzx7m_len_value_loop_ev:
  \         add     a, a                    ; check next bit
  \         jr      z, dzx7m_load_bits3_ev  ; no more bits left?
  \         rl      c
  \         rl      b
  \         jr      c, dzx7m_exit_ev        ; check end marker
#5 l: d dec, z? runtil b incp, m e ld, h incp, e sll,
  \ dzx7m_len_value_start_ev:
  \         dec     d
  \         jr      nz, dzx7m_len_value_loop_ev
  \         inc     bc                      ; adjust length
  \ dzx7m_offset_start_od:
  \ ; determine offset
  \         ld      e, (hl)                 ; load offset flag (1 bit) + offset value (7 bits)
  \         inc     hl
  \         defb    $cb, $33                ; opcode for undocumented instruction "SLL E" aka "SLS E"
#7 rl# nc? ?jr, a add, d rl, a add, #13 rl# z? ?jr,
  \         jr      nc, dzx7m_offset_end_od ; if offset flag is set, load 4 extra bits
  \         add     a, a                    ; check next bit
  \         rl      d                       ; insert first bit into D
  \         add     a, a                    ; check next bit
  \         jr      z, dzx7m_load_bits4     ; no more bits left?

d rl, a add, d rl, a add, #14 rl# z? ?jr, ccf, #7 rl# c? ?jr,
  \         rl      d                       ; insert second bit into D
  \         add     a, a                    ; check next bit
  \         rl      d                       ; insert third bit into D
  \         add     a, a                    ; check next bit
  \         jr      z, dzx7m_load_bits5     ; no more bits left?
  \         ccf
  \         jr      c, dzx7m_offset_end_od

-->

( dzx7m )

#6 l: d inc,
  \ dzx7m_offset_inc_od:
  \         inc     d                       ; equivalent to adding 128 to DE
#7 l: e rr, exsp, h push, d sbcp, d pop, ldir, h pop,
  \ dzx7m_offset_end_od:
  \         rr      e                       ; insert inverted fourth bit into E
  \ ; copy previous sequence
  \         ex      (sp), hl                ; store source, restore destination
  \         push    hl                      ; store destination
  \         sbc     hl, de                  ; HL = destination - offset - 1
  \         pop     de                      ; DE = destination
  \         ldir
  \         pop     hl                      ; restore source address (compressed data)
#22 jp, al#
  \         jp      dzx7m_main_loop_od
  \
#8 l: m a ld, h incp, rla, #3 rl# c? ?jr, #1 jp, al#
  \ dzx7m_load_bits1:
  \         ld      a, (hl)                 ; load another group of 8 bits
  \         inc     hl
  \         rla
  \         jr      c, dzx7m_len_size_start_od ; next bit indicates either literal or sequence
  \         jp      dzx7m_copy_byte_loop_od
  \
#9 l: m a ld, h incp, rla, #2 rl# nc? ?jr, #5 jp, al#
  \ dzx7m_load_bits2_ev:
  \         ld      a, (hl)                 ; load another group of 8 bits
  \         inc     hl
  \         rla
  \         jr      nc, dzx7m_len_size_loop_ev
  \         jp      dzx7m_len_value_start_ev
  \
#10 l: m a ld, h incp, rla, #4 rl# nc? ?jr, #15 jp, al#
  \ dzx7m_load_bits2_od:
  \         ld      a, (hl)                 ; load another group of 8 bits
  \         inc     hl
  \         rla
  \         jr      nc, dzx7m_len_size_loop_od
  \         jp      dzx7m_len_value_start_od
  \
#11 l: m a ld, h incp, rla, c rl, b rl, #5 nc? ?jp, al#
  \ dzx7m_load_bits3_ev:
  \         ld      a, (hl)                 ; load another group of 8 bits
  \         inc     hl
  \         rla
  \         rl      c
  \         rl      b
  \         jp      nc, dzx7m_len_value_start_ev ; check end marker
#12 l: d pop, exx, jpnext,
  \ dzx7m_exit_ev:
  \         pop     de
  \         exx
  \         _jp_next
  \
#13 l: m a ld, h incp, rla, d rl, a add, d rl, a add, ccf,
  \ dzx7m_load_bits4:
  \         ld      a, (hl)                 ; load another group of 8 bits
  \         inc     hl
  \         rla
  \         rl      d                       ; insert second bit into D
  \         add     a, a                    ; check next bit
  \         rl      d                       ; insert third bit into D
  \         add     a, a                    ; check next bit
  \         ccf
#7 rl# c? ?jr, #6 jp, al#
  \         jr      c, dzx7m_offset_end_od
  \         jp      dzx7m_offset_inc_od
  \
#14 l: m a ld, h incp, rla, ccf, #7 rl# c? ?jr, #6 jp, al#
  \ dzx7m_load_bits5:
  \         ld      a, (hl)                 ; load another group of 8 bits
  \         inc     hl
  \         rla
  \         ccf
  \         jr      c, dzx7m_offset_end_od
  \         jp      dzx7m_offset_inc_od
  \
  \ ; determine length
rbegin a add, #18 rl# z? ?jr, c rl, b rl, #19 rl# c? ?jr,
  \ dzx7m_len_value_loop_od:
  \         add     a, a                    ; check next bit
  \         jr      z, dzx7m_load_bits3_od  ; no more bits left?
  \         rl      c
  \         rl      b
  \         jr      c, dzx7m_exit_od        ; check end marker
#15 l: d dec, z? runtil b incp, -->
  \ dzx7m_len_value_start_od:
  \         dec     d
  \         jr      nz, dzx7m_len_value_loop_od
  \         inc     bc                      ; adjust length

( dzx7m )

m e ld, h incp, e sll, #17 rl# nc? ?jr, a add,
  \ dzx7m_offset_start_ev:
  \ ; determine offset
  \         ld      e, (hl)                 ; load offset flag (1 bit) + offset value (7 bits)
  \         inc     hl
  \         defb    $cb, $33                ; opcode for undocumented instruction "SLL E" aka "SLS E"
  \         jr      nc, dzx7m_offset_end_ev ; if offset flag is set, load 4 extra bits
  \         add     a, a                    ; check next bit
#20 rl# z? ?jr, d rl, a add, d rl, a add,
  \         jr      z, dzx7m_load_bits6     ; no more bits left?
  \         rl      d                       ; insert first bit into D
  \         add     a, a                    ; check next bit
  \         rl      d                       ; insert second bit into D
  \         add     a, a                    ; check next bit
#21 rl# z? ?jr, d rl, a add, ccf, #17 rl# c? ?jr,
  \         jr      z, dzx7m_load_bits7     ; no more bits left?
  \         rl      d                       ; insert third bit into D
  \         add     a, a                    ; check next bit
  \         ccf
  \         jr      c, dzx7m_offset_end_ev
#16 l: d inc,
  \ dzx7m_offset_inc_ev:
  \         inc     d                       ; equivalent to adding 128 to DE
#17 l:
  \ dzx7m_offset_end_ev:
e rr, exsp, h push, d sbcp, d pop, ldir, h pop, #0 jp, al#
  \         rr      e                       ; insert inverted fourth bit into E
  \ ; copy previous sequence
  \         ex      (sp), hl                ; store source, restore destination
  \         push    hl                      ; store destination
  \         sbc     hl, de                  ; HL = destination - offset - 1
  \         pop     de                      ; DE = destination
  \         ldir
  \         pop     hl                      ; restore source address (compressed data)
  \         jp      dzx7m_main_loop_ev
  \
#18 l: m a ld, h incp, rla, c rl, b rl, #15 nc? ?jp, al#
  \ dzx7m_load_bits3_od:
  \         ld      a, (hl)                 ; load another group of 8 bits
  \         inc     hl
  \         rla
  \         rl      c
  \         rl      b
  \         jp      nc, dzx7m_len_value_start_od ; check end marker
#19 l: d pop, exx, jpnext,
  \ dzx7m_exit_od:
  \         pop     de
  \         exx
  \         _jp_next
  \
#20 l: m a ld, h incp, rla, d rl, a add, d rl, a add,
  \ dzx7m_load_bits6:
  \         ld      a, (hl)                 ; load another group of 8 bits
  \         inc     hl
  \         rla
  \         rl      d                       ; insert first bit into D
  \         add     a, a                    ; check next bit
  \         rl      d                       ; insert second bit into D
  \         add     a, a                    ; check next bit
d rl, a add, ccf, #17 rl# c? ?jr, #16 jp, al#
  \         rl      d                       ; insert third bit into D
  \         add     a, a                    ; check next bit
  \         ccf
  \         jr      c, dzx7m_offset_end_ev
  \         jp      dzx7m_offset_inc_ev
  \
#21 l: m a ld, h incp, rla, d rl, a add, ccf,
       #17 rl# c? ?jr, #16 jp, al# end-code
  \ dzx7m_load_bits7:
  \         ld      a, (hl)                 ; load another group of 8 bits
  \         inc     hl
  \         rla
  \         rl      d                       ; insert third bit into D
  \         add     a, a                    ; check next bit
  \         ccf
  \         jr      c, dzx7m_offset_end_ev
  \         jp      dzx7m_offset_inc_ev

also assembler max-labels c! previous
  \ Restore the default values.

  \ doc{
  \
  \ dzx7m ( a1 a2 -- )
  \
  \ Decompress data, which has been compressed by ZX7, from
  \ _a1_ and copy the result to _a2_.
  \
  \ ``dzx7m`` is the port of the ZX7 decompressor, "Mega"
  \ version, written by Einar Saukas.
  \
  \ ``dzx7m`` is the fastest (30% faster than `dzx7s`) but
  \ biggest (251 bytes) version of the decompressor. `dzx7s`
  \ and `dzx7t` are smaller but slower.
  \
  \ For more information, see
  \ http://www.worldofspectrum.org/infoseekid.cgi?id=0027996
  \ ZX7 in World of Spectrum].
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
  \ 2017-03-22: Add `zx7t`. Improve documentation. Start
  \ `zx7m`, the port of ZX7 decompressor "Mega" version.
  \
  \ 2017-03-23: Finish `dzx7m` (the port of ZX7 decompressor
  \ "Mega" version).  Rename the words an the module.  Complete
  \ the documentation.
  \
  \ 2017-03-25: Adapt to the new version of `l:`, which is
  \ configurable.
  \
  \ 2017-03-26: Fix description of ZX7 links in documentation.

  \ vim: filetype=soloforth
