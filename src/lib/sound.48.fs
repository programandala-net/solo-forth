  \ sound.48.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201806041338
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ ZX Spectrum 48 sound words.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( bleep hz>bleep dhz>bleep )

unneeding bleep ?(

code bleep ( duration pitch -- )
 E1 c, D1 c, C5 c, CD c, 03B5 , C1 c, DD c, 21 c, next ,
    \ pop hl
    \ pop de
    \ push bc ; save Forth IP
    \ call rom_beeper
    \ pop bc ; restore Forth IP
    \ ld ix,next ; restore IX
  jpnext, end-code ?)

  \ Credit:
  \
  \ Code modified from Abersoft Forth.

  \ doc{
  \
  \ bleep ( duration pitch -- )
  \
  \ Produce a tone in the internal beeper.
  \
  \ ``bleep`` calls the
  \ BEEPER ROM routine with _pitch_ in the HL register and
  \ _duration_ in the DE register.

  \ ////
  \ The following description is taken, with small layout
  \ changes, from _Spectrum Advanced Forth_ by Don Thomasson
  \ (Melbourne House, 1984), page 26:
  \ ////

  \ [quote, Don Thomasson&#44; Spectrum Advanced Forth (Melbourne House&#44; 1984)&#44; page 26]
  \ _____
  \
  \ (...) but while there is greater flexibility than is
  \ directly available in BASIC the system is more difficult to
  \ use.  Precalculation is necessary to obtain musical scales,
  \ on the following basis:
  \
  \ To generate a frequency of F Hz, _pitch_ must be set to:
  \
  \ ....
  \ pitch = (437500/F)-30
  \ ....
  \
  \ Looking in the opposite direction:
  \
  \ ....
  \ F = 437500/(pitch+30)
  \ ....
  \
  \ The duration of the note is determined as a number of
  \ cycles, so _duration_ must be set to ``F x T``, where _T_ is
  \ the duration in seconds.
  \
  \ A point to note is that if a very low frequency is
  \ selected, with a high duration, the system may appear to
  \ hang up, because the BEEPER ROM routine goes on and on...;
  \ whithout the user being able to use BREAK.
  \
  \ _____

  \ ////
  \ The following technical information about the BEEPER ROM
  \ routine is taken, with small layout changes, from the ZX
  \ Spectrum disassembly by Dr. Ian Logan, Dr.  Frank O'Hara et
  \ al.:
  \ ////

  \ [quote, Dr. Ian Logan&#44; Dr.  Frank O'Hara et al., ZX Spectrum disassembly]
  \ _____

  \ Output a square wave of given duration and frequency
  \ to the loudspeaker.
  \
  \ Enter with:

  \ - DE = #cycles - 1
  \ - HL = tone period as described next

  \ The tone period is measured in T states and consists of
  \ three parts: a coarse part (H register), a medium part
  \ (bits 7..2 of L) and a fine part (bits 1..0 of L) which
  \ contribute to the waveform timing as follows:

  \ ....
  \                          coarse    medium       fine
  \ duration of low  = 118 + 1024*H + 16*(L>>2) + 4*(L&$3)
  \ duration of hi   = 118 + 1024*H + 16*(L>>2) + 4*(L&$3)
  \ Tp = tone period = 236 + 2048*H + 32*(L>>2) + 8*(L&$3)
  \                  = 236 + 2048*H + 8*L = 236 + 8*HL
  \ ....

  \ As an example, to output five seconds of middle C (261.624
  \ Hz):

  \ 1. Tone period = 1/261.624 = 3.822ms
  \ 2. Tone period in T-States = 3.822ms*fCPU = 13378
  \    (where fCPU = clock frequency of the CPU = 3.5MHz)
  \ 3. Find H and L for desired tone period:
  \    HL = (Tp - 236) / 8 = (13378 - 236) / 8 = 1643 =
  \    $066B
  \ 4. Tone duration in cycles = 5s/3.822ms = 1308 cycles
  \ 5. DE = 1308 - 1 = $051B

  \
  \ The resulting waveform has a duty ratio of exactly 50%.
  \ _____

  \
  \ See: `hz>bleep`, `dzh>bleep`, `beep`.
  \
  \ }doc

unneeding hz>bleep ?(

: hz>bleep ( frequency duration1 -- duration2 pitch )
  over m* 1000 m/ nip swap  437500. rot m/ nip 30 - ; ?)

  \ XXX OLD -- 1.32 slower:
  \ over 1000 */ swap  4375 100 rot */ 30 - ;

  \ Credit:
  \
  \ Code adapted from v.Forth's `>bleep`.

  \ doc{
  \
  \ hz>bleep ( frequency duration1 -- duration2 pitch ) "hertz-to-bleep;
  \
  \ Convert _frequency_ (in Hz) and _duration1_ (in ms) to
  \ the parameters _duration2 pitch_ needed by `bleep`.
  \
  \ See: `dhz>bleep`.
  \
  \ }doc

unneeding dhz>bleep ?(

: dhz>bleep ( frequency duration1 -- duration2 pitch )
  over m* 10000 m/ nip swap  4375000. rot m/ nip 30 - ; ?)

  \ XXX OLD -- 1.32 slower:
  \ over 10000 */ swap  4375 1000 rot */ 30 - ;

  \ Credit:
  \
  \ Code adapted from v.Forth's `>bleep`.

  \ doc{
  \
  \ dhz>bleep ( frequency duration1 -- duration2 pitch ) "decihertz-to-bleep"
  \
  \ Convert _frequency_ (in dHz, i.e. tenths of hertzs) and
  \ _duration1_ (in ms) to the parameters _duration2 pitch_
  \ needed by `bleep`.
  \
  \ See: `hz>bleep`.
  \
  \ }doc

( middle-octave /octave octave-changer change-octave )

  \ ___________________________________________________________
  \
  \ Note Frequencies

  \ Credit:
  \
  \ https://www.seventhstring.com/resources/notefrequencies.html

  \ Here is a table giving the frequencies in Hz of musical
  \ pitches, covering the full range of all normal musical
  \ instruments I know of and then some. It uses an even
  \ tempered scale with A = 440 Hz.

  \ [cols="1,7*"]
  \ |===
  \ | Octave | C  | C#      | D       | D#      | E       | F       | F#      | G       | G#      | A       | A#      | B
  \
  \ | 0 |   16.35 |   17.32 |   18.35 |   19.45 |   20.60 |   21.83 |   23.12 |   24.50 |   25.96 |   27.50 |   29.14 |   30.87
  \ | 1 |   32.70 |   34.65 |   36.71 |   38.89 |   41.20 |   43.65 |   46.25 |   49.00 |   51.91 |   55.00 |   58.27 |   61.74
  \ | 2 |   65.41 |   69.30 |   73.42 |   77.78 |   82.41 |   87.31 |   92.50 |   98.00 |  103.80 |  110.00 |  116.50 |  123.50
  \ | 3 |  130.80 |  138.60 |  146.80 |  155.60 |  164.80 |  174.60 |  185.00 |  196.00 |  207.70 |  220.00 |  233.10 |  246.90
  \ | 4 |  261.60 |  277.20 |  293.70 |  311.10 |  329.60 |  349.20 |  370.00 |  392.00 |  415.30 |  440.00 |  466.20 |  493.90
  \ | 5 |  523.30 |  554.40 |  587.30 |  622.30 |  659.30 |  698.50 |  740.00 |  784.00 |  830.60 |  880.00 |  932.30 |  987.80
  \ | 6 | 1047.00 | 1109.00 | 1175.00 | 1245.00 | 1319.00 | 1397.00 | 1480.00 | 1568.00 | 1661.00 | 1760.00 | 1865.00 | 1976.00
  \ | 7 | 2093.00 | 2217.00 | 2349.00 | 2489.00 | 2637.00 | 2794.00 | 2960.00 | 3136.00 | 3322.00 | 3520.00 | 3729.00 | 3951.00
  \ | 8 | 4186.00 | 4435.00 | 4699.00 | 4978.00 | 5274.00 | 5588.00 | 5920.00 | 6272.00 | 6645.00 | 7040.00 | 7459.00 | 7902.00
  \ |===

  \ The octave number is in the left column so to find the
  \ frequency of middle C which is C4, look down the "C" column
  \ til you get to the "4" row : so middle C is 261.6 Hz.
  \ ___________________________________________________________

  \ XXX TODO -- Include the previus table into the manual.

unneeding middle-octave ?(

create middle-octave ( -- a )
  \ C      C#     D      D#     E      F
    2616 , 2772 , 2937 , 3111 , 3296 , 3492 ,
  \ F#     G      G#     A      A#     B
    3700 , 3920 , 4153 , 4400 , 4662 , 4939 , ?)

  \ XXX REMARK -- values in tenths of Hz
  \ XXX TODO -- use hundreths of Hz

  \ doc{
  \
  \ middle-octave ( -- a )
  \
  \ Return the address of a 12-cell table that contains the
  \ frequencies in dHz (tenths of Hz) of the middle octave.
  \ They are used by `beep>dhz` to calculate the frequency of
  \ any note.
  \
  \ Here is a diagram to show the offsets of all the notes in
  \ the table, on the piano (extracted from the manual of the
  \ ZX Spectrum +3 transcripted by Russell et al.):

  \ ....
  \ | | C#| D#| | | F#| G#| A#| |
  \ | | Db| Eb| | | Gb| Ab| Bb| |
  \ | | 1 | 3 | | | 6 | 8 |10 | |
  \ | |___|___| | |___|___|___| |
  \ |   |   |   |   |   |   |   |
  \ | 0 | 2 | 4 | 5 | 7 | 9 |11 |
  \ |___|___|___|___|___|___|___|
  \   C   D   E   F   G   A   B
  \ ....

  \ See: `beep`, `/octave`, `octave-changer`.
  \
  \ }doc

unneeding /octave ?\ 12 cconstant /octave

  \ doc{
  \
  \ /octave ( -- c ) "slash-octave"
  \
  \ A `cconstant` that returns the number of notes in one
  \ octave: 12.
  \
  \ See: `middle-octave`.
  \
  \ }doc

unneeding octave-changer ?( need rshift need lshift

        ' rshift  ,
here    ' drop    ,
        ' lshift  ,  constant octave-changer ?)

  \ doc{
  \
  \ octave-changer ( -- a )
  \
  \ _a_ is the address of an execution table that contains the
  \ three execution tokens used to calculate the frequency of
  \ notes from any octave.  _a_ is the address of the second
  \ execution token (cell offset 0).
  \
  \ See: `change-octave`, `beep>dhz`, `middle-octave`.
  \
  \ }doc

unneeding change-octave ?(

need polarity need octave-changer need array>

: change-octave ( u n -- u' )
  dup abs swap polarity octave-changer array> perform ; ?)

  \ doc{
  \
  \ change-octave ( u n -- u' )
  \
  \ Change the note frequency _u_ of the middle octave (octave
  \ zero) to its corresponding note frequency _u'_ in octave
  \ _n_.  If _n_ is zero, _u'_ equals _u_.
  \
  \ See: `octave-changer`, `beep>dhz`, `middle-octave`.
  \
  \ }doc

( -beep>note +beep>note beep>note )

unneeding -beep>note ?( need /octave

: -beep>note ( -n1 -- -n2 +n3 )
  abs [ /octave 1- ] literal + /octave /mod negate
      [ /octave 1- ] literal rot - ; ?)

  \ doc{
  \
  \ -beep>note ( -n1 -- -n2 +n3 ) "minus-beep-to-note"
  \
  \ Convert a negative pitch _-n1_ of `beep` to its
  \ corresponding note _+n3_ (0..11) in octave _-n2_, being
  \ zero the middle octave.
  \
  \ See: `beep>note`, `+beep>note`, `/octave`, `beep>dhz`,
  \ `beep>dhz`.
  \
  \ }doc

unneeding +beep>note ?( need /octave

: +beep>note ( +n1 -- +n2 +n3 ) /octave /mod swap ; ?)

  \ doc{
  \
  \ +beep>note ( +n1 -- +n2 +n3 ) "plus-beet-to-note"
  \
  \ Convert a positive pitch _+n1_ of `beep` to its
  \ corresponding note _+n3_ (0..11) in octave _+n2_, being
  \ zero the middle octave.
  \
  \ See: `beep>note`, `-beep>note`, `/octave`, `beep>dhz`,
  \ `beep>bleep`.
  \
  \ }doc

unneeding beep>note ?( need -beep>note need +beep>note

: beep>note ( n1 -- n2 +n3 ) dup 0< if   -beep>note exit
                                    then +beep>note ; ?)

  \ doc{
  \
  \ beep>note ( n1 -- n2 +n3 ) "beep-to-note"
  \
  \ Convert a pitch _n1_ of `beep` to its corresponding note
  \ _+n3_ (0..11) in octave _n2_, being zero the middle octave.
  \
  \ See: `-beep>note`, `+beep>note`, `beep>dhz`, `beep>bleep`,
  \ `/octave`.
  \
  \ }doc

( note>dhz beep>dhz beep>bleep beep )

unneeding note>dhz ?( need middle-octave need array>

: note>dhz ( +n1 -- +n2 ) middle-octave array> @ ; ?)

unneeding beep>dhz ?(

need beep>note need note>dhz need change-octave

: beep>dhz ( n -- u )
  beep>note note>dhz swap change-octave ; ?)

  \ doc{
  \
  \ beep>dhz ( n -- u ) "beep-to-decihertz"
  \
  \ Convert a pitch _n_ of `beep` to its corresponding
  \ frequency in dHz (tenths of hertzs) _u_.
  \
  \ See: `beep>note`, `beep>bleep`.
  \
  \ }doc

unneeding beep>bleep ?(

need beep>dhz need dhz>bleep

: beep>bleep ( duration1 pitch1 -- pitch2 duration2 )
  beep>dhz swap dhz>bleep ; ?)

  \ doc{
  \
  \ beep>bleep ( duration1 pitch1 -- pitch2 duration2 ) "beep-to-bleep"

  \
  \ Convert _duration1_ and _pitch1_ of `beep`, which are
  \ equivalent to the parameters used by Sinclar BASIC's
  \ ``BEEP`` command, to _pitch2_ and _duration2_, which are
  \ the parameters required by `bleep`.
  \
  \ NOTE: _duration1_ is in miliseconds (instead of seconds
  \ used by Sinclair BASIC).
  \
  \ _pitch1_ is identical to the Sinclair BASIC parameter:
  \ number of semitones from middle C (positive number for
  \ notes above, negative number for notes below).
  \
  \ See: `beep>dhz`, `beep>note`.
  \
  \ }doc

unneeding beep ?( need beep>bleep need bleep

: beep ( duration pitch -- ) beep>bleep bleep ; ?)

  \ doc{
  \
  \ beep ( duration pitch -- )
  \
  \ Produce a tone in the internal beeper, with parameters that
  \ are equivalent to those of the homonymous Sinclar BASIC
  \ command:
  \
  \ _duration_ is in miliseconds (instead of seconds used by
  \ BASIC).
  \
  \ _pitch_ is identical to the BASIC parameter: number of
  \ semitones from middle C (positive number for notes above,
  \ negative number for notes below).
  \
  \ Here is a diagram to show the pitch values of all the notes
  \ in one octave on the piano (extracted from the manual of
  \ the ZX Spectrum +3 transcripted by Russell et al.):

  \ ....
  \   |   | | | C#| D#| | | F#| G#| A#| | |   |   |
  \   |   | | | Db| Eb| | | Gb| Ab| Bb| | |   |   |
  \   |-2 | | | 1 | 3 | | | 6 | 8 |10 | | |13 |15 |
  \ __|___| | |___|___| | |___|___|___| | |___|___|
  \     |   |   |   |   |   |   |   |   |   |   |
  \  -3 |-1 | 0 | 2 | 4 | 5 | 7 | 9 |11 |12 |14 |16
  \ ____|___|___|___|___|___|___|___|___|___|___|____
  \           C   D   E   F   G   A   B   C
  \ ....


  \ Hence, to play the A above middle C for half a second, you
  \ would use:

  \ ----
  \ 500 9 beep
  \ ----

  \ And to play a scale (for example, C major) a complete
  \ (albeit short) program is needed:

  \ ----
  \ create scale
  \   0 c, 2 c, 4 c, 5 c, 7 c, 9 c, 11 c, 12 c,
  \
  \ 8 constant /scale
  \
  \ : play-scale ( -- ) /scale 0 ?do
  \                       500 scale i + c@ beep
  \                     loop ;
  \
  \ play-scale
  \ ----

  \
  \ See: `beep>bleep`, `bleep`, `beep>dhz`.
  \
  \ }doc

( laser-gun-sound ambulance-sound )

unneeding laser-gun-sound ?( need assembler

code laser-gun-sound ( -- ) b push, 5 b ld#, 0500 h ldp#,
  <mark  0001 d ldp#,
         h push, 03B5 call, h pop,  \ ROM beeper
         0010 d ldp#, d subp,  nz? ?jr,
  b pop, next ix ldp#, jpnext, end-code ?)

  \ Credit:
  \
  \ Author of the original code: Álvaro Corredor Lanas.
  \ Published on Microhobby, issue 126 (1987), page 7:
  \ http://microhobby.org/numero126.htm
  \ http://microhobby.speccy.cz/mhf/126/MH126_07.jpg

  \ doc{
  \
  \ laser-gun ( -- )
  \
  \ Laser gun sound for ZX Spectrum 48.
  \
  \ }doc

unneeding ambulance-sound ?( need assembler

code ambulance-sound ( n -- )

  d pop, b push, e b ld,

  rbegin  b push, 0320 h ldp#, 000A d ldp#,
          <mark   h push,
                  03B5 call,  \ ROM beeper
                  h pop, h decp, h tstp,  nz? ?jr,
          b pop,  rstep

  b pop, next ix ldp#, jpnext, end-code ?)

  \ Credit:
  \
  \ Author of the original code: Líder Software.
  \ Published on Microhobby, issue 142 (1987-09), page 7:
  \ http://microhobby.org/numero142.htm
  \ http://microhobby.speccy.cz/mhf/142/MH142_07.jpg

  \ doc{
  \
  \ ambulance ( n -- )
  \
  \ Ambulance sound for ZX Spectrum 48. Make it _n_ times.
  \
  \ }doc

( white-noise )

need assembler

code white-noise ( u -- )

  d pop,
  b push,  \ save the Forth IP
  d b ldp, 0000 h ldp#,  \ BC=duration, HL=start of ROM

  5C48 fta, a sra, a sra, a sra, 07 and#, a d ld,
    \ D = border color (in bits 0-2)

  <mark   m e ld, h incp, b decp, b push,
          08 b ld#,  \ bit counter
          rbegin   e a ld, 10 and#, e rl, d or, FE out,  \ beep
                  rstep
          b pop, b tstp,
          nz? ?jr,

  b pop, jpnext, \ restore the Forth IP and go next

  end-code

  \ Credit:
  \
  \ Author of the original code: Ricardo Serral Wigge.
  \ Published on Microhobby, issue 125 (1987), page 26:
  \ http://microhobby.org/numero125.htm
  \ http://microhobby.speccy.cz/mhf/125/MH125_26.jpg

  \ The original code was called "explosion" and had a fixed
  \ duration of 768 sample bytes, thus equivalent to `768
  \ white-noise`.

  \ doc{
  \
  \ white-noise ( -- )
  \
  \ White noise for ZX Spectrum 48.  _u_ is the duration in
  \ number of sample bytes.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015: Main development.
  \
  \ 2016-04-14: Documented `bleep` and `beep>bleep`.
  \
  \ 2016-12-20: Rename `jpnext` to `jpnext,` after the change
  \ in the kernel.
  \
  \ 2016-12-30: Compact the code, saving two blocks.
  \
  \ 2017-01-03: Convert `laser-gun`, `ambulance`, and
  \ `white-noise` from `z80-asm` to `z80-asm`. Fix needing of
  \ `laser-gun` and `ambulance`. Fix structures: use `<mark ...
  \ ?jr` instead of `rbegin ... ?jr`. Improve documentation.
  \
  \ 2017-01-05: Update `need z80-asm,` to `need assembler`.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-19: Rename `beep>bleep` to `>bleep`. Improve
  \ documentation.
  \
  \ 2017-01-23: Rename `>bleep` to `hz>bleep`. Add `dhz>bleep`,
  \ `middle-octave`, `/octave`, `octave-changer`, `beep>dhz`,
  \ `beep`.
  \
  \ 2017-01-24: Make `hz>bleep` and `dhz>bleep` faster (0.75
  \ the previous time). Factor `beep>bleep` from `beep`. Factor
  \ `beep>note` from `beep>bleep` and fix the calculation to
  \ support also negative values of pitch.
  \
  \ 2017-01-25: Typo.
  \
  \ 2017-02-17: Update cross references.  Change markup of code
  \ that is not a cross reference.
  \
  \ 2017-02-24: Improve documentation markup.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-05-05: Improve documentation.
  \
  \ 2018-01-23: Rename all sounds: add suffix "-sound".
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-08: Add words' pronunciaton.
  \
  \ 2018-04-11: Improve documentation of `beep`, `bleep` and
  \ friends.
  \
  \ 2018-06-04: Link `cconstant` in documentation.

  \ vim: filetype=soloforth
