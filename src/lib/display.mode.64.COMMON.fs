  \ display.mode.64.COMMON.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201712051621
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ A 64-cpl display mode font.

  \ ===========================================================
  \ Authors

  \ Author of the font: Andrew Owen.
  \ Published on the World of Spectrum forum:
  \ http://www.worldofspectrum.org/forums/discussion/14526/redirect/p1

  \ Marcos Cruz (programandala.net) adapted it to Solo Forth,
  \ 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

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

  \ --------------------------------------------
  \ Old, from <display.mode.64.fs>

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

  \ 2017-12-05: Extract Andrew Owen's font from
  \ <display.mode.64.fs>.

  \ vim: filetype=soloforth
