  \ sound.128.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202005182059
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ ZX Spectrum 128 sound words.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018,
  \ 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( /sound sound-register-port sound-write-port !sound @sound )

unneeding /sound ?\ 14 cconstant /sound

  \ doc{
  \
  \ /sound ( -- b ) "slash-sound"
  \
  \ A character constant that returns 14, the number of
  \ sound registers used by ZX Spectrum 128.
  \
  \ See: `!sound`, `@sound`, `sound`, `play`.
  \
  \ }doc

unneeding sound-register-port
unneeding sound-write-port and ?( need const

$FFFD const sound-register-port $BFFD const sound-write-port ?)

  \ doc{
  \
  \ sound-register-port ( -- a )
  \
  \ The I/O port used to select a register of the AY-3-8912
  \ sound generator, before writing a value into it using
  \ `sound-write-port`, or before reading a value from it using
  \ ``sound-register-port`` again.
  \
  \ ``sound-register-port`` is a fast constant defined with
  \ `const`. Its value is $FFFD.
  \
  \ See: `sound-write-port`, `!sound`, `@sound`.
  \
  \ }doc

  \ doc{
  \
  \ sound-write-port ( -- a )
  \
  \ The I/O port used to write to a register of the AY-3-8912
  \ sound generator.
  \
  \ ``sound-write-port`` is a fast constant defined with
  \ `const`.  Its value is $BFFD.
  \
  \ See: `sound-register-port`, `!sound`, `@sound`.
  \
  \ }doc

unneeding !sound ?(

need !p need sound-register-port need sound-write-port

: !sound ( b1 b2 -- )
  sound-register-port !p sound-write-port !p ; ?)

  \ doc{
  \
  \ !sound ( b1 b2 -- ) "store-sound"
  \
  \ Set sound register _b2_ (0...13) to value _b1_.
  \
  \ See: `@sound`, `sound`, `play`, `sound-register-port`,
  \ `sound-write-port`.
  \
  \ }doc

unneeding @sound ?( need !p need @p need sound-register-port

: @sound ( b1 -- b2 )
  sound-register-port !p sound-register-port @p ; ?)

  \ doc{
  \
  \ @sound ( b1 -- b2 ) "fetch-sound"
  \
  \ Get the contents _b2_ of sound register _b1_ (0...13).
  \
  \ See: `!sound`, `sound`, `play`, `sound-register-port`.
  \
  \ }doc

( !volume @volume set-mixer get-mixer -mixer silence noise )

  \ Credit:
  \
  \ Code from Spectrum Forth-83.

  \ XXX TODO -- Finish, document and test.

unneeding !volume

?\ need !sound  : !volume ( b1 b2 -- ) 8 + !sound ;

  \ doc{
  \
  \ !volume ( b1 b2 -- ) "store-volume"
  \
  \ Store _b1_ at volume register of channel _b2_ (0..2,
  \ equivalent to notation 'A'..'C').
  \
  \ [quote,,Disassembly of the ZX Spectrum 128k ROM0]
  \ ____
  \ Registers 8..10 (Channels A..C Volume)

  \ [horizontal]
  \ Bits 0-4:: Channel volume level.
  \ Bit 5   :: 1=Use envelope defined by register 13 and ignore the volume setting.
  \ Bits 6-7:: Not used.
  \ ____
  \
  \ See: `@volume`, `!sound`.
  \
  \ }doc

unneeding @volume

?\ need @sound  : @volume ( b -- ) 8 + @sound ;

  \ doc{
  \
  \ @volume ( b1 -- b2 ) "fetch-volume"
  \
  \ Fetch _b2_ from the volume register of channel _b1_ (0..2,
  \ equivalent to notation 'A'..'C').
  \
  \ [quote,,Disassembly of the ZX Spectrum 128k ROM0]
  \ ____
  \ Registers 8..10 (Channels A..C Volume)

  \ [horizontal]
  \ Bits 0-4:: Channel volume level.
  \ Bit 5   :: 1=Use envelope defined by register 13 and ignore the volume setting.
  \ Bits 6-7:: Not used.
  \ ____
  \
  \ See: `!volume`, `@sound`.
  \
  \ }doc

unneeding set-mixer

?\ need !sound : set-mixer ( b -- ) 7 !sound ;

  \ doc{
  \
  \ set-mixer ( b -- )
  \
  \ Set the mixer register of the AY-3-8912 sound
  \ generator to _b_.
  \
  \ [quote,,Disassembly of the ZX Spectrum 128k ROM0]
  \ ____
  \
  \ Register 7 (Mixer - I/O Enable)
  \
  \ This controls the enable status of the noise and tone
  \ mixers for the three channels, and also controls the I/O
  \ port used to drive the RS232 and Keypad sockets.

  \ [horizontal]
  \ Bit 0:: Channel A Tone Enable (0=enabled).
  \ Bit 1:: Channel B Tone Enable (0=enabled).
  \ Bit 2:: Channel C Tone Enable (0=enabled).
  \ Bit 3:: Channel A Noise Enable (0=enabled).
  \ Bit 4:: Channel B Noise Enable (0=enabled).
  \ Bit 5:: Channel C Noise Enable (0=enabled).
  \ Bit 6:: I/O Port Enable (0=input, 1=output).
  \ Bit 7:: Not used.
  \ ____
  \
  \ See: `get-mixer`, `-mixer`, `!sound`.
  \
  \ }doc

unneeding get-mixer

?\ need @sound : get-mixer ( -- b ) 7 @sound ;

  \ doc{
  \
  \ get-mixer ( -- b )
  \
  \ Get the contents _b_ of the mixer register of the
  \ AY-3-8912 sound generator.
  \
  \ [quote,,Disassembly of the ZX Spectrum 128k ROM0]
  \ ____
  \
  \ Register 7 (Mixer - I/O Enable)
  \
  \ This controls the enable status of the noise and tone
  \ mixers for the three channels, and also controls the I/O
  \ port used to drive the RS232 and Keypad sockets.

  \ [horizontal]
  \ Bit 0:: Channel A Tone Enable (0=enabled).
  \ Bit 1:: Channel B Tone Enable (0=enabled).
  \ Bit 2:: Channel C Tone Enable (0=enabled).
  \ Bit 3:: Channel A Noise Enable (0=enabled).
  \ Bit 4:: Channel B Noise Enable (0=enabled).
  \ Bit 5:: Channel C Noise Enable (0=enabled).
  \ Bit 6:: I/O Port Enable (0=input, 1=output).
  \ Bit 7:: Not used.
  \ ____
  \
  \ See: `set-mixer`, `-mixer`, `@sound`.
  \
  \ }doc

unneeding -mixer

?\ need set-mixer : -mixer ( -- ) %111111 set-mixer ;

  \ doc{
  \
  \ -mixer ( -- ) "minus-mixer"
  \
  \ Disable the noise and tone mixers for the three channels of
  \ the AY-3-8912 sound generator.
  \
  \ See: `set-mixer`, `get-mixer`, `silence`.
  \
  \ }doc

unneeding silence ?( need -mixer need !volume

: silence ( -- )
  -mixer 0 0 !volume 0 1 !volume 0 2 !volume ; ?)

  \ doc{
  \
  \ silence ( -- )
  \
  \ Execute `-mixer` to disable the noise and tone mixers for
  \ the three channels of the AY-3-8912 sound generator. Then
  \ set the volume of the three channels to zero.
  \
  \ See: `!volume`.
  \
  \ }doc

unneeding noise ?\ need !sound  : noise ( -- ) 7 7 !sound ;

( music )

need vocabulary need ms need roll need pick
need !sound need vol

vocabulary music  get-current  also music definitions

  \ Credit:
  \
  \ Code from Spectrum Forth-83.

  \ XXX TODO -- Finish, document and test.

: freq
  2* 109.375 3 roll  um/mod nip 256 /mod 2 pick
  1+ !sound  swap !sound ;

variable len  variable tempo  variable octave  variable volume

2 len !  200 tempo !  8 octave !  15 volume ! 1 15 vol

: tones ( -- ) 56 7 !sound ;

: note ( n "name" -- )
  create  ,  does>   @ octave @ * 16 /  1 freq tones
                     tempo @ len @ * ms -mixer ;

523 note c  554 note c# 583 note d  622 note d#
659 note e  698 note f  740 note f# 784 note g
831 note g# 880 note a  932 note a# 988 note b  -->

( music )

  \ Credit:
  \
  \ Code from Spectrum Forth-83.

  \ XXX TODO -- Finish, document and test.

: l  ( n -- ) len ! ;
: o+ ( -- )   octave @ 2 * octave ! ;
: o- ( -- )   octave @ 2 / octave ! ;
: r  ( -- )   tempo @ len @ * ms ;
: >> ( -- )   1 volume @ 1+ vol 1 volume +! ;
: << ( -- )   1 volume @ 1- vol -1 volume +! ;

set-current previous

( play sound, sound )

  \ Credit:
  \
  \ Code inspired by the article "Las posibilidades sonoras del
  \ 128 K", written by Juan José Rosado Recio, published on
  \ Microhobby, issue 147 (1987-10), page 24:
  \
  \ http://microhobby.org/numero147.htm
  \ http://microhobby.speccy.cz/mhf/147/MH147_24.jpg

unneeding play ?( need /sound need !sound

: play ( ca -- ) /sound 0 ?do dup c@ i !sound 1+ loop drop ; ?)

  \ doc{
  \
  \ play ( ca -- )
  \
  \ Play a 14-byte sound definition stored at _ca_.
  \
  \ See: `sound,`, `sound`, `!sound`, `edit-sound`.
  \
  \ }doc

unneeding sound, ?( need /sound need +loop

: sound, ( b[0]..b[13] -- )
  here /sound allot here 1- ?do i c! -1 +loop ; ?)

  \ doc{
  \
  \ sound, ( b[0]..b[13] -- ) "sound-comma"
  \
  \ Compile the 14-byte sound definition _b[0]..b[13]_.
  \
  \ See: `play`, `sound`.
  \
  \ }doc

unneeding sound ?( need sound, need play

: sound ( b[0]..b[13] "name" -- )
  create  sound,  does> ( -- ) ( dfa ) play ; ?)

  \ doc{
  \
  \ sound ( b[0]..b[13] "name" -- )
  \
  \ Create a word _name_ that will play the 14-byte sound
  \ defined by _b[0]..b[13]_.
  \
  \ See: `sound,`, `play`, `edit-sound`.
  \
  \ }doc

( fplay )

  \ XXX UNDER DEVELOPMENT
  \ XXX TMP --
  \ XXX TODO -- Rename to `fast-play`.

need !p need c@+
need sound-register-port need sound-write-port need /sound

: fplay ( ca -- )
  /sound 0 ?do
    i sound-register-port !p c@+ sound-write-port !p
  loop  drop ;

  \
  \ fplay ( ca -- ) "f-play"
  \
  \ Play a sound whose 14 bytes are stored at _ca_.  ``fplay``
  \ is a faster version of `play`.
  \

( zplay )

  \ XXX UNDER DEVELOPMENT
  \ XXX TMP --

  \ Z80 version of `play`.

need assembler

need sound-register-port need sound-write-port need /sound

code zplay ( a -- )
  h pop, b push,
  /sound b ld#, 00 e ld#,
    \ B = loop counter
    \ E = register number
  rbegin  b push,
          e a ld, sound-register-port b ldp#, a outbc,
            \ select the register
          m a ld, sound-write-port b ldp#, a outbc,
            \ store the datum
          hl incp, e inc, b pop, \ next
  rstep
  b pop, jpnext, end-code
  \ Play a sound whose 14 bytes are stored at _a_.
  \
  \ XXX FIXME -- No sound! maybe `outbc` has a bug in the
  \ assembler.

( waves-sound shoot-sound helicopter1-sound train-sound )

need sound  hex

  \ Credit:
  \
  \ `waves-sound` and `shoot-sound` are adapted from code
  \ written by Juan José Ruiz, published on Microhobby, issue
  \ 139 (1987-07), page 7:
  \
  \ http://microhobby.org/numero139.htm
  \ http://microhobby.speccy.cz/mhf/139/MH139_07.jpg

unneeding waves-sound

?\ 00 00 00 00 00 00 07 47 14 14 14 00 26 0E sound waves-sound

unneeding shoot-sound

?\ 0A 00 B1 00 BF 00 1F 47 14 14 14 5C 1C 03 sound shoot-sound

  \ Credit:
  \
  \ `helicopter1-sound` and `train-sound` are adapted from code
  \ written by José Ángel Martín, published on Microhobby,
  \ issue 172 (1988-09), page 22:
  \
  \ http://microhobby.org/numero172.htm
  \ http://microhobby.speccy.cz/mhf/172/MH172_22.jpg

unneeding helicopter1-sound ?(

C8 0F C8 0F C8 0F 00 07 17 17 17 FF 01 0C
sound helicopter1-sound ?)

unneeding train-sound

?\ 64 78 30 61 0C C8 37 0F 09 0B 37 B4 04 08 sound train-sound

decimal

( airplane-sound helicopter2-sound )

  \ Credit:
  \
  \ `airplane-sound` and `helicopter2-sound` were extracted
  \ from a program written by Juan José Rosado Recio, published
  \ on Microhobby, issue 147 (1987-10), page 24:
  \
  \ http://microhobby.org/numero147.htm
  \ http://microhobby.speccy.cz/mhf/147/MH147_24.jpg

need sound  hex

unneeding airplane-sound ?(

0C 1F 00 00 00 1F 07 E8 0F 10 0F 9A 00 18
sound airplane-sound ?)

unneeding helicopter2-sound ?(

09 00 00 06 0C 00 0B C0 10 0E 10 3A 02 1C
sound helicopter2-sound ?)

decimal

( bomber-sound whip-sound metalic-sound )

  \ Credit:
  \
  \ `bomber-sound`, `whip-sound` and `metalic-sound` were
  \ adapted from data written by Francisco Majón, published on
  \ Microhobby, issue 194 (1989-12), page 26:
  \
  \ http://microhobby.org/numero194.htm
  \ http://microhobby.speccy.cz/mhf/194/MH194_26.jpg

need sound  hex

unneeding bomber-sound

?\ 49 52 3E A5 5A 8A 9F 8C 66 4D 64 A2 57 C9 sound bomber-sound

unneeding whip-sound

?\ 05 12 08 06 13 0B 05 0B 00 13 03 18 15 01 sound whip-sound

unneeding metalic-sound ?(

95 40 68 EC D2 B4 00 20 00 C2 92 49 51 B1
sound metalic-sound ?)

decimal

( lightning1-sound lightning2-sound )

  \ Credit:
  \
  \ `lightning1-sound` and `lightning2-sound` were adapted from
  \ data written by Francisco Majón, published on Microhobby,
  \ issue 194 (1989-12), page 26:
  \
  \ http://microhobby.org/numero194.htm
  \ http://microhobby.speccy.cz/mhf/194/MH194_26.jpg

need sound  hex

unneeding lightning1-sound ?(

01 04 00 10 24 43 08 04 1F F5 01 06 1E 02
sound lightning1-sound ?)

unneeding lightning2-sound ?(

00 00 00 00 00 FF 07 04 FF 19 00 3C 3C 03
sound lightning2-sound ?)

  \ #16 #17 #25 #10 #19 #9 #4 #31 #245 #1 #6 #30 #2 sound rain2-sound
  \ 10 11 19 0A 13 09 04 1F F5 01 06 1E 02 sound rain2-sound
  \
  \ XXX FIXME -- One number is missing.

decimal

( bell1-sound bell2-sound bell3-sound )

  \ Credit:
  \
  \ `bell1-sound`, `bell2-sound` and `bell3-sound` were
  \ extracted from a program written by Juan José Rosado Recio,
  \ published on Microhobby, issue 147 (1987-10), page 24:
  \
  \ http://microhobby.org/numero147.htm
  \ http://microhobby.speccy.cz/mhf/147/MH147_24.jpg

need sound  hex

unneeding bell1-sound

?\ AB 03 2A 02 0C 01 00 F8 10 10 10 00 71 10 sound bell1-sound

unneeding bell2-sound

?\ 66 00 4B 00 45 00 00 F8 10 10 10 00 22 10 sound bell2-sound

unneeding bell3-sound

?\ FC 06 DE 03 C3 04 00 F8 10 10 10 00 FF 10 sound bell3-sound

decimal

( rap-sound drum-sound cymbal-sound )

  \ Credit:
  \
  \ `rap-sound`, `drum-sound` and `cymbal-sound` were extracted
  \ from a program written by Juan José Rosado Recio, published
  \ on Microhobby, issue 147 (1987-10), page 24:
  \
  \ http://microhobby.org/numero147.htm
  \ http://microhobby.speccy.cz/mhf/147/MH147_24.jpg

need sound  hex

unneeding rap-sound

?\ 00 00 00 00 00 00 06 C0 10 10 10 00 05 18 sound rap-sound

unneeding drum-sound

?\ 00 06 00 00 00 05 11 E8 10 10 10 00 0A 10 sound drum-sound

unneeding cymbal-sound

?\ 09 00 00 00 00 00 00 C0 10 10 10 03 09 10 sound cymbal-sound

decimal

( applause-sound hammer-sound background-sound )

  \ Credit:
  \
  \ `applause-sound`, `hammer-sound` and `background-sound`
  \ were extracted from a program written by Juan José Rosado
  \ Recio, published on Microhobby, issue 147 (1987-10), page
  \ 24:
  \
  \ http://microhobby.org/numero147.htm
  \ http://microhobby.speccy.cz/mhf/147/MH147_24.jpg

need sound  hex

unneeding applause-sound ?(

00 00 00 00 00 00 1E 40 0F 10 0F 00 07 18
sound applause-sound ?)

unneeding hammer-sound

?\ 1B 00 09 00 00 00 1F C8 10 10 10 00 6B 10 sound hammer-sound

unneeding background-sound ?(

03 05 FC 04 0C 05 00 F8 10 10 10 FF FF 0E
sound background-sound ?)

decimal

( beach-sound waterdrop2-sound rain1-sound waterdrop1-sound )

  \ Credit:
  \
  \ `beach-sound` and `waterdrop2-sound` were extracted from a
  \ program written by Juan José Rosado Recio, published on
  \ Microhobby, issue 147 (1987-10), page 24:
  \
  \ http://microhobby.org/numero147.htm
  \ http://microhobby.speccy.cz/mhf/147/MH147_24.jpg

need sound  hex

unneeding beach-sound

?\ 00 00 00 00 00 00 0F C0 0B 10 10 FF 50 0E sound beach-sound

unneeding waterdrop2-sound ?(

?\ 24 00 12 00 16 00 00 F8 10 10 10 00 10 18
sound waterdrop2-sound ?)

  \ Credit:
  \
  \ `rain1-sound` and `waterdrop1-sound` were extracted from a
  \ program written by Carlos Ventura, published on Microhobby,
  \ issue 198 (1990-05), page 16:
  \
  \ http://microhobby.org/numero198.htm
  \ http://microhobby.speccy.cz/mhf/198/MH198_16.jpg

unneeding rain1-sound

?\ 2C 18 06 06 07 03 03 05 2C 06 03 05 03 03 sound rain1-sound

unneeding waterdrop1-sound ?(

14 53 5E 27 00 08 1F 47 17 17 16 5A 00 00
sound waterdrop1-sound ?)

decimal

( explosion1-sound explosion2-sound )

  \ Credit:
  \
  \ `explosion1-sound` and `explosion2-sound` are adapted from
  \ the SE BASIC manual, page 10, where they were taken from
  \ the Timex Sinclair TS2068 User Manual.
  \
  \ XXX FIXME -- 2016-10-10: Finish the conversion: Registers
  \ not specified in the examples are set to zero, but they
  \ should keep the default values. Consult the TS2068 User
  \ Manual.

need sound  hex

unneeding explosion1-sound ?(

00 00 00 00 00 06 07 10 10 10 38 08 00 00
sound explosion1-sound ?)

unneeding explosion2-sound ?(

00 00 00 00 00 06 07 10 10 10 38 08 00 00
sound explosion2-sound ?)

decimal

  \ ===========================================================
  \ Change log

  \ 2016-04-24: Need `pick`, which has been moved to the
  \ library.
  \
  \ 2016-05-18: Need `vocabulary`, which has been moved to the
  \ library.
  \
  \ 2016-10-10: Fix name of `applause`. Define `/sound` only
  \ once.
  \
  \ 2016-12-20: Rename `jpnext` to `jpnext,` after the change
  \ in the kernel.
  \
  \ 2017-01-02: Convert `zplay` from `z80-asm` to `z80-asm,`.
  \
  \ 2017-01-05: Update `need z80-asm,` to `need assembler`.
  \
  \ 2017-01-13: Improve documentation.
  \
  \ 2017-01-23: Make all remaining words individually
  \ accessible to `need`.
  \
  \ 2017-02-17: Fix typo in documentation.  Update cross
  \ references.
  \
  \ 2017-02-20: Replace `do`, which has been moved to the
  \ library, with `?do`.
  \
  \ 2017-02-28: Fix names of "lightning" effects.  Rename `vol`
  \ to `!volume`. Add `@volume`. Rename `shutup` to `-mixer`.
  \ Write `silence`, an upper layer on `-mixer`. Add `@sound`.
  \ Add `get-mixer`, `set-mixer`.  Improve documentation.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-04-18: Fix needing of `applause`. Improve
  \ documentation. Fix references to `!mixer` and `@mixer`:
  \ they are `set-mixer` and `get-mixer`. Update old reference
  \ to `shutup`, now `-mixer`.
  \
  \ 2017-09-09: Update notation "pfa" to the standard "dfa".
  \
  \ 2018-01-23: Update source style.  Rename all sounds defined
  \ with `sound`: add suffix "-sound".
  \
  \ 2018-03-01: Improve documentation.
  \
  \ 2018-03-02: Fix word names in credit note.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-08: Add words' pronunciaton.
  \
  \ 2020-05-18: Update: `+loop` was moved to the library.

  \ vim: filetype=soloforth
