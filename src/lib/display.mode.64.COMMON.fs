  \ display.mode.64.COMMON.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201802041811
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ 64-cpl display mode fonts.

  \ ===========================================================
  \ Authors

  \ Andrew Owen
  \ http://www.worldofspectrum.org/forums/discussion/14526/redirect/p1

  \ Einar Saukas
  \ https://sites.google.com/site/zxgraph/home/einar-saukas/fonts
  \ http://www.worldofspectrum.org/infoseekid.cgi?id=0027130

  \ Marcos Cruz (programandala.net) adapted the fonts to Solo
  \ Forth, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( mode-64-font )

variable mode-64-font

  \ doc{
  \
  \ mode-64-font ( -- a )
  \
  \ A variable. _a_ is the address of a cell containing the
  \ address of the 4x8-pixel font used by `mode-64o`. Note the
  \ address of the font must be the address of its character 32
  \ (space). The size of a 4x8-pixel font is 336 bytes. The
  \ program is responsible for initializing the contents of
  \ this variable before executing `mode-64o`.
  \
  \ NOTE: If ``mode-64-font`` is changed when `mode-64o` is on,
  \ for example to use a new font, `mode-64o` must be executed
  \ again in order to make the change effective.
  \
  \ See: `mini-64cpl-font`, `nbot-64cpl-font`,
  \ `omn1-64cpl-font`, `omn2-64cpl-font`, `owen-64cpl-font`.
  \
  \ }doc

( mini-64cpl-font )

create mini-64cpl-font ( -- a ) hex

  \ Half width 4x8 font.
  \ 336 bytes.

02 c, 02 c, 02 c, 02 c, 00 c, 02 c, 00 c, \  !
50 c, 55 c, 07 c, 05 c, 07 c, 05 c, 00 c, \ "#
20 c, 75 c, 61 c, 32 c, 74 c, 25 c, 00 c, \ $%
21 c, 51 c, 22 c, 50 c, 40 c, 30 c, 00 c, \ &'
14 c, 22 c, 22 c, 22 c, 22 c, 14 c, 00 c, \ ()
00 c, 52 c, 22 c, 77 c, 22 c, 52 c, 00 c, \ *+
00 c, 00 c, 00 c, 07 c, 20 c, 20 c, 40 c, \ ,-
01 c, 01 c, 02 c, 02 c, 04 c, 24 c, 00 c, \ ./
72 c, 56 c, 52 c, 52 c, 52 c, 77 c, 00 c, \ 01
77 c, 11 c, 73 c, 41 c, 41 c, 77 c, 00 c, \ 23
57 c, 54 c, 57 c, 71 c, 11 c, 17 c, 00 c, \ 45
77 c, 41 c, 71 c, 52 c, 52 c, 72 c, 00 c, \ 67
77 c, 55 c, 77 c, 51 c, 51 c, 77 c, 00 c, \ 89
-->

( mini-64cpl-font )

00 c, 00 c, 22 c, 00 c, 02 c, 22 c, 04 c, \ :;
00 c, 10 c, 27 c, 40 c, 27 c, 10 c, 00 c, \ <=
02 c, 45 c, 21 c, 12 c, 20 c, 42 c, 00 c, \ >?
23 c, 55 c, 75 c, 57 c, 45 c, 35 c, 00 c, \ @A
63 c, 54 c, 64 c, 54 c, 54 c, 63 c, 00 c, \ BC
67 c, 54 c, 56 c, 54 c, 54 c, 67 c, 00 c, \ DE
73 c, 44 c, 64 c, 45 c, 45 c, 43 c, 00 c, \ FG
57 c, 52 c, 72 c, 52 c, 52 c, 57 c, 00 c, \ HI
35 c, 15 c, 16 c, 16 c, 55 c, 65 c, 00 c, \ JK
45 c, 47 c, 47 c, 45 c, 45 c, 75 c, 00 c, \ LM
63 c, 55 c, 55 c, 55 c, 55 c, 56 c, 00 c, \ NO
66 c, 55 c, 55 c, 75 c, 47 c, 43 c, 00 c, \ PQ
63 c, 54 c, 56 c, 61 c, 51 c, 56 c, 00 c, \ RS
75 c, 25 c, 25 c, 25 c, 25 c, 27 c, 00 c, \ TU
-->

( mini-64cpl-font )

55 c, 55 c, 55 c, 57 c, 57 c, 25 c, 00 c, \ VW
55 c, 55 c, 25 c, 52 c, 52 c, 52 c, 00 c, \ XY
77 c, 14 c, 24 c, 24 c, 44 c, 74 c, 07 c, \ Z[
47 c, 41 c, 21 c, 21 c, 11 c, 11 c, 07 c, \ \]
20 c, 50 c, 00 c, 00 c, 00 c, 00 c, 0F c, \ ^_
20 c, 50 c, 46 c, 63 c, 45 c, 77 c, 00 c, \ `a
40 c, 40 c, 63 c, 54 c, 54 c, 73 c, 00 c, \ bc
10 c, 10 c, 33 c, 55 c, 56 c, 73 c, 00 c, \ de
10 c, 20 c, 73 c, 25 c, 27 c, 21 c, 46 c, \ fg
42 c, 40 c, 66 c, 52 c, 52 c, 57 c, 00 c, \ hi
14 c, 04 c, 15 c, 16 c, 15 c, 55 c, 20 c, \ jk
60 c, 20 c, 25 c, 27 c, 25 c, 15 c, 00 c, \ lm
00 c, 00 c, 63 c, 55 c, 55 c, 56 c, 00 c, \ no
00 c, 00 c, 63 c, 55 c, 77 c, 41 c, 41 c, \ pq
-->

( mini-64cpl-font )

00 c, 00 c, 33 c, 46 c, 43 c, 46 c, 00 c, \ rs
20 c, 70 c, 25 c, 25 c, 25 c, 13 c, 00 c, \ tu
00 c, 00 c, 55 c, 55 c, 57 c, 25 c, 00 c, \ vw
00 c, 00 c, 55 c, 25 c, 23 c, 51 c, 06 c, \ xy
03 c, 02 c, 74 c, 22 c, 42 c, 73 c, 00 c, \ z{
26 c, 22 c, 21 c, 22 c, 22 c, 26 c, 00 c, \ |}
02 c, 35 c, 53 c, 03 c, 05 c, 02 c, 00 c, \ ~.

decimal

  \ Credit:
  \
  \ "Minix" font
  \ Author: Einar Saukas
  \ https://sites.google.com/site/zxgraph/home/einar-saukas/fonts
  \ http://www.worldofspectrum.org/infoseekid.cgi?id=0027130

  \ doc{
  \
  \ mini-64cpl-font ( -- a ) "mini-64-c-p-l-font"
  \
  \ _a_ is the address of a 4x8-pixel font compiled in data
  \ space (336 bytes used), to be used in `mode-64o` by setting
  \ `mode-64-font` first.
  \
  \ This font is included also in disk 0 as "mini.f64".
  \
  \ See: `nbot-64cpl-font`, `omn1-64cpl-font`,
  \ `omn2-64cpl-font`, `owen-64cpl-font`.
  \
  \ }doc

( nbot-64cpl-font )

create nbot-64cpl-font ( -- a ) hex

  \ Half width 4x8 font.
  \ 336 bytes.

00 c, 02 c, 02 c, 02 c, 00 c, 02 c, 00 c, \  !
00 c, 55 c, 57 c, 05 c, 07 c, 05 c, 00 c, \ "#
00 c, 24 c, 71 c, 62 c, 34 c, 71 c, 20 c, \ $%
00 c, 21 c, 51 c, 22 c, 50 c, 30 c, 00 c, \ &'
00 c, 14 c, 22 c, 22 c, 22 c, 14 c, 00 c, \ ()
00 c, 52 c, 22 c, 77 c, 22 c, 52 c, 00 c, \ *+
00 c, 00 c, 00 c, 07 c, 20 c, 20 c, 40 c, \ ,-
00 c, 01 c, 01 c, 02 c, 04 c, 24 c, 00 c, \ ./
00 c, 76 c, 52 c, 52 c, 52 c, 77 c, 00 c, \ 01
00 c, 77 c, 11 c, 73 c, 41 c, 77 c, 00 c, \ 23
00 c, 57 c, 54 c, 77 c, 11 c, 17 c, 00 c, \ 45
00 c, 77 c, 41 c, 71 c, 52 c, 72 c, 00 c, \ 67
00 c, 77 c, 55 c, 77 c, 51 c, 77 c, 00 c, \ 89
-->

( nbot-64cpl-font )

00 c, 00 c, 22 c, 00 c, 02 c, 22 c, 04 c, \ :;
00 c, 10 c, 27 c, 40 c, 27 c, 10 c, 00 c, \ <=
00 c, 47 c, 25 c, 11 c, 22 c, 40 c, 02 c, \ >?
00 c, 23 c, 75 c, 75 c, 47 c, 35 c, 00 c, \ @A
00 c, 63 c, 54 c, 64 c, 54 c, 77 c, 00 c, \ BC
00 c, 63 c, 54 c, 56 c, 54 c, 67 c, 00 c, \ DE
00 c, 73 c, 44 c, 65 c, 45 c, 47 c, 00 c, \ FG
00 c, 57 c, 52 c, 72 c, 52 c, 57 c, 00 c, \ HI
00 c, 35 c, 15 c, 16 c, 55 c, 75 c, 00 c, \ JK
00 c, 45 c, 47 c, 47 c, 45 c, 75 c, 00 c, \ LM
00 c, 53 c, 75 c, 75 c, 75 c, 57 c, 00 c, \ NO
00 c, 66 c, 55 c, 55 c, 77 c, 43 c, 00 c, \ PQ
00 c, 73 c, 54 c, 56 c, 61 c, 57 c, 00 c, \ RS
00 c, 75 c, 25 c, 25 c, 25 c, 27 c, 00 c, \ TU
-->

( nbot-64cpl-font )

00 c, 55 c, 55 c, 57 c, 57 c, 25 c, 00 c, \ VW
00 c, 55 c, 75 c, 27 c, 72 c, 52 c, 00 c, \ XY
00 c, 77 c, 14 c, 24 c, 44 c, 77 c, 00 c, \ Z[
00 c, 47 c, 41 c, 21 c, 11 c, 17 c, 00 c, \ \]
00 c, 20 c, 70 c, 20 c, 20 c, 20 c, 0F c, \ ^_
00 c, 70 c, 46 c, 23 c, 45 c, 77 c, 00 c, \ `a
00 c, 40 c, 43 c, 64 c, 54 c, 77 c, 00 c, \ bc
00 c, 10 c, 13 c, 35 c, 56 c, 77 c, 00 c, \ de
00 c, 30 c, 43 c, 65 c, 47 c, 41 c, 06 c, \ fg
00 c, 42 c, 40 c, 76 c, 52 c, 57 c, 00 c, \ hi
00 c, 14 c, 05 c, 17 c, 16 c, 55 c, 70 c, \ jk
00 c, 60 c, 25 c, 27 c, 25 c, 35 c, 00 c, \ lm
00 c, 00 c, 63 c, 75 c, 55 c, 57 c, 00 c, \ no
00 c, 00 c, 63 c, 55 c, 77 c, 41 c, 41 c, \ pq
-->

( nbot-64cpl-font )

00 c, 00 c, 77 c, 46 c, 43 c, 47 c, 00 c, \ rs
00 c, 20 c, 75 c, 25 c, 25 c, 37 c, 00 c, \ tu
00 c, 00 c, 55 c, 55 c, 57 c, 25 c, 00 c, \ vw
00 c, 00 c, 55 c, 25 c, 27 c, 51 c, 06 c, \ xy
00 c, 03 c, 72 c, 34 c, 62 c, 73 c, 00 c, \ z{
00 c, 26 c, 22 c, 21 c, 22 c, 26 c, 00 c, \ |}
0F c, 30 c, 56 c, 04 c, 06 c, 00 c, 0F c, \ ~.
decimal

  \ Credit:
  \
  \ "n-Bot" font
  \ Author: Einar Saukas
  \ https://sites.google.com/site/zxgraph/home/einar-saukas/fonts
  \ http://www.worldofspectrum.org/infoseekid.cgi?id=0027130

  \ doc{
  \
  \ nbot-64cpl-font ( -- a ) "n-bot-64-c-p-l-font"
  \
  \ _a_ is the address of a 4x8-pixel font compiled in data
  \ space (336 bytes used), to be used in `mode-64o` by setting
  \ `mode-64-font` first.
  \
  \ This font is included also in disk 0 as "nbot.f64".
  \
  \ See: `mini-64cpl-font`, `omn1-64cpl-font`,
  \ `omn2-64cpl-font`, `owen-64cpl-font`.
  \
  \ }doc


( omn1-64cpl-font )

create omn1-64cpl-font ( -- a ) hex

  \ Half width 4x8 font.
  \ 336 bytes.

06 c, 06 c, 06 c, 06 c, 06 c, 00 c, 06 c, \  !
05 c, 57 c, 57 c, 05 c, 07 c, 07 c, 05 c, \ "#
25 c, 71 c, 63 c, 72 c, 36 c, 74 c, 25 c, \ $%
60 c, 43 c, 23 c, 46 c, 50 c, 70 c, 30 c, \ &'
14 c, 36 c, 22 c, 22 c, 22 c, 36 c, 14 c, \ ()
00 c, 52 c, 22 c, 77 c, 77 c, 22 c, 52 c, \ *+
00 c, 00 c, 00 c, 07 c, 37 c, 30 c, 60 c, \ ,-
01 c, 01 c, 03 c, 02 c, 06 c, 24 c, 04 c, \ ./
76 c, 76 c, 52 c, 52 c, 52 c, 77 c, 77 c, \ 01
77 c, 77 c, 11 c, 73 c, 41 c, 77 c, 77 c, \ 23
57 c, 57 c, 74 c, 77 c, 71 c, 17 c, 17 c, \ 45
77 c, 77 c, 41 c, 71 c, 52 c, 72 c, 72 c, \ 67
77 c, 77 c, 55 c, 77 c, 51 c, 77 c, 77 c, \ 89
-->

( omn1-64cpl-font )

00 c, 00 c, 22 c, 00 c, 03 c, 23 c, 06 c, \ :;
00 c, 17 c, 37 c, 60 c, 37 c, 17 c, 00 c, \ <=
07 c, 47 c, 65 c, 31 c, 62 c, 40 c, 02 c, \ >?
27 c, 77 c, 75 c, 57 c, 67 c, 75 c, 25 c, \ @A
63 c, 77 c, 54 c, 64 c, 54 c, 77 c, 63 c, \ BC
67 c, 77 c, 54 c, 56 c, 54 c, 77 c, 67 c, \ DE
73 c, 77 c, 44 c, 65 c, 65 c, 47 c, 43 c, \ FG
57 c, 57 c, 72 c, 72 c, 72 c, 57 c, 57 c, \ HI
35 c, 35 c, 17 c, 16 c, 57 c, 75 c, 75 c, \ JK
45 c, 47 c, 47 c, 47 c, 75 c, 75 c, 75 c, \ LM
63 c, 77 c, 75 c, 55 c, 55 c, 57 c, 57 c, \ NO
67 c, 77 c, 55 c, 75 c, 77 c, 47 c, 43 c, \ PQ
73 c, 77 c, 56 c, 77 c, 63 c, 57 c, 56 c, \ RS
75 c, 75 c, 75 c, 25 c, 27 c, 27 c, 27 c, \ TU
-->

( omn1-64cpl-font )

55 c, 55 c, 55 c, 57 c, 77 c, 77 c, 25 c, \ VW
55 c, 75 c, 77 c, 27 c, 77 c, 72 c, 52 c, \ XY
77 c, 77 c, 14 c, 24 c, 44 c, 77 c, 77 c, \ Z[
47 c, 47 c, 61 c, 21 c, 31 c, 17 c, 17 c, \ \]
20 c, 70 c, 70 c, 70 c, 20 c, 20 c, 2F c, \ ^_
10 c, 20 c, 76 c, 21 c, 77 c, 27 c, 17 c, \ `a
40 c, 40 c, 73 c, 77 c, 54 c, 77 c, 77 c, \ bc
10 c, 10 c, 73 c, 77 c, 57 c, 74 c, 73 c, \ de
30 c, 30 c, 23 c, 77 c, 77 c, 21 c, 26 c, \ fg
42 c, 40 c, 66 c, 72 c, 72 c, 57 c, 57 c, \ hi
14 c, 04 c, 15 c, 17 c, 56 c, 77 c, 75 c, \ jk
60 c, 60 c, 25 c, 27 c, 27 c, 35 c, 35 c, \ lm
00 c, 00 c, 67 c, 77 c, 75 c, 57 c, 57 c, \ no
00 c, 00 c, 63 c, 77 c, 77 c, 77 c, 41 c, \ pq
-->

( omn1-64cpl-font )

00 c, 00 c, 77 c, 76 c, 47 c, 43 c, 47 c, \ rs
20 c, 70 c, 75 c, 25 c, 25 c, 37 c, 37 c, \ tu
00 c, 00 c, 55 c, 55 c, 77 c, 77 c, 25 c, \ vw
00 c, 00 c, 55 c, 77 c, 27 c, 73 c, 56 c, \ xy
03 c, 03 c, 72 c, 34 c, 72 c, 63 c, 73 c, \ z{
26 c, 26 c, 22 c, 21 c, 22 c, 26 c, 26 c, \ |}
37 c, 70 c, 57 c, 06 c, 07 c, 00 c, 07 c, \ ~.

decimal

  \ Credit:
  \ 
  \ "Omni1" font
  \ Author: Einar Saukas
  \ https://sites.google.com/site/zxgraph/home/einar-saukas/fonts
  \ http://www.worldofspectrum.org/infoseekid.cgi?id=0027130

  \ doc{
  \
  \ omn1-64cpl-font ( -- a ) "omn-1-64-c-p-l-font"
  \
  \ _a_ is the address of a 4x8-pixel font compiled in data
  \ space (336 bytes used), to be used in `mode-64o` by setting
  \ `mode-64-font` first.
  \
  \ This font is included also in disk 0 as "omn1.f64".
  \
  \ See: `mini-64cpl-font`, `nbot-64cpl-font`,
  \ `omn2-64cpl-font`, `owen-64cpl-font`.
  \
  \ }doc

( omn2-64cpl-font )

create omn2-64cpl-font ( -- a ) hex

  \ Half width 4x8 font.
  \ 336 bytes.

06 c, 06 c, 06 c, 06 c, 06 c, 00 c, 06 c, \  !
05 c, 57 c, 57 c, 05 c, 07 c, 07 c, 05 c, \ "#
25 c, 71 c, 63 c, 72 c, 36 c, 74 c, 25 c, \ $%
60 c, 43 c, 23 c, 46 c, 50 c, 70 c, 30 c, \ &'
14 c, 36 c, 22 c, 22 c, 22 c, 36 c, 14 c, \ ()
00 c, 52 c, 22 c, 77 c, 77 c, 22 c, 52 c, \ *+
00 c, 00 c, 00 c, 07 c, 37 c, 30 c, 60 c, \ ,-
01 c, 01 c, 03 c, 02 c, 06 c, 24 c, 04 c, \ ./
76 c, 76 c, 52 c, 52 c, 52 c, 77 c, 77 c, \ 01
77 c, 77 c, 11 c, 73 c, 41 c, 77 c, 77 c, \ 23
57 c, 57 c, 74 c, 77 c, 71 c, 17 c, 17 c, \ 45
77 c, 77 c, 41 c, 71 c, 52 c, 72 c, 72 c, \ 67
77 c, 77 c, 55 c, 77 c, 51 c, 77 c, 77 c, \ 89
-->

( omn2-64cpl-font )

00 c, 00 c, 22 c, 00 c, 03 c, 23 c, 06 c, \ :;
00 c, 17 c, 37 c, 60 c, 37 c, 17 c, 00 c, \ <=
07 c, 47 c, 65 c, 31 c, 62 c, 40 c, 02 c, \ >?
27 c, 77 c, 75 c, 57 c, 67 c, 75 c, 25 c, \ @A
63 c, 77 c, 54 c, 64 c, 54 c, 77 c, 63 c, \ BC
67 c, 77 c, 54 c, 56 c, 54 c, 77 c, 67 c, \ DE
73 c, 77 c, 44 c, 65 c, 65 c, 47 c, 43 c, \ FG
57 c, 57 c, 72 c, 72 c, 72 c, 57 c, 57 c, \ HI
35 c, 35 c, 17 c, 16 c, 57 c, 75 c, 75 c, \ JK
45 c, 47 c, 47 c, 47 c, 75 c, 75 c, 75 c, \ LM
63 c, 77 c, 75 c, 55 c, 55 c, 57 c, 57 c, \ NO
67 c, 77 c, 55 c, 75 c, 77 c, 47 c, 43 c, \ PQ
73 c, 77 c, 56 c, 77 c, 63 c, 57 c, 56 c, \ RS
75 c, 75 c, 75 c, 25 c, 27 c, 27 c, 27 c, \ TU
-->

( omn2-64cpl-font )

55 c, 55 c, 55 c, 57 c, 77 c, 77 c, 25 c, \ VW
55 c, 75 c, 77 c, 27 c, 77 c, 72 c, 52 c, \ XY
77 c, 77 c, 14 c, 24 c, 44 c, 77 c, 77 c, \ Z[
47 c, 47 c, 61 c, 21 c, 31 c, 17 c, 17 c, \ \]
20 c, 70 c, 70 c, 70 c, 20 c, 20 c, 2F c, \ ^_
10 c, 26 c, 71 c, 27 c, 75 c, 27 c, 17 c, \ `a
40 c, 40 c, 73 c, 77 c, 54 c, 77 c, 77 c, \ bc
10 c, 13 c, 75 c, 77 c, 54 c, 77 c, 77 c, \ de
30 c, 33 c, 27 c, 75 c, 77 c, 21 c, 26 c, \ fg
42 c, 40 c, 66 c, 72 c, 72 c, 57 c, 57 c, \ hi
14 c, 04 c, 15 c, 17 c, 56 c, 77 c, 75 c, \ jk
60 c, 60 c, 25 c, 27 c, 27 c, 35 c, 35 c, \ lm
00 c, 00 c, 67 c, 77 c, 75 c, 57 c, 57 c, \ no
00 c, 00 c, 63 c, 55 c, 77 c, 77 c, 41 c, \ pq
-->

( omn2-64cpl-font )

00 c, 00 c, 77 c, 76 c, 47 c, 43 c, 47 c, \ rs
20 c, 70 c, 75 c, 25 c, 25 c, 37 c, 37 c, \ tu
00 c, 00 c, 55 c, 55 c, 77 c, 77 c, 25 c, \ vw
00 c, 00 c, 55 c, 77 c, 27 c, 71 c, 56 c, \ xy
03 c, 03 c, 72 c, 34 c, 72 c, 63 c, 73 c, \ z{
26 c, 26 c, 22 c, 21 c, 22 c, 26 c, 26 c, \ |}
37 c, 70 c, 57 c, 06 c, 07 c, 00 c, 07 c, \ ~.
decimal

  \ Credit:
  \
  \ "Omni2" font
  \ Author: Einar Saukas
  \ https://sites.google.com/site/zxgraph/home/einar-saukas/fonts
  \ http://www.worldofspectrum.org/infoseekid.cgi?id=0027130

  \ doc{
  \
  \ omn2-64cpl-font ( -- a ) "omn-2-64-c-p-l-font"
  \
  \ _a_ is the address of a 4x8-pixel font compiled in data
  \ space (336 bytes used), to be used in `mode-64o` by setting
  \ `mode-64-font` first.
  \
  \ This font is included also in disk 0 as "omn2.f64".
  \
  \ See: `mini-64cpl-font`, `nbot-64cpl-font`,
  \ `omn1-64cpl-font`, `owen-64cpl-font`.
  \
  \ }doc

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
  \ owen-64cpl-font ( -- a ) "owen-64-c-p-l-font"
  \
  \ _a_ is the address of a 4x8-pixel font compiled in data
  \ space (336 bytes used), to be used in `mode-64o` by setting
  \ `mode-64-font` first.
  \
  \ This font is included also in disk 0 as "owen.f64".
  \
  \ See: `mini-64cpl-font`, `nbot-64cpl-font`,
  \ `omn1-64cpl-font`, `omn2-64cpl-font`.
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
  \ <display.mode.64.fs>. Move `mode-64-font` from
  \ <display.mode.64o.fs>. Add the fonts by Einar Saukas,
  \ already included in disk 0: `mini-64cpl-font`,
  \ `nbot-64cpl-font`, `omn1-64cpl-font` and `omn2-64cpl-font`.
  \
  \ 2018-02-04: Improve documentation: add pronunciation to
  \ words that need it.

  \ vim: filetype=soloforth
