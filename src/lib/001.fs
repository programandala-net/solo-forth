  \ 001.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201806041056

  \ ===========================================================
  \ Description

  \ Block 1 of the library, which is used to load the user
  \ application. By default it loads block 2, which contains
  \ the `need` utility.

(  )

2 load

  \ XXX TMP -- to debug `associative-list`:
  \ need associative-list

  \ exit

  \ XXX TMP -- to debug the floating point ROM
  \ implementation:

  \ need f0 need .fs
  \ need f>s
  \ need ftuck
  \ need fmax need fmin
  \ need --fp-rom--
  \ f1 f0 fpi2/ f1 f1
  \ need f= need f<
  \ f1 f0 f1 f0 f1 f0
  \ 100 s>f  200 s>f  300 s>f

  \ XXX TMP -- to debug the G+DOS support:

  \ need dos-in need dos-in,

  \ XXX TMP -- to debug the tape support:

  \ need write-tape-file
  \ : savescr 16384 6912 s" screen" write-tape-file ;
  \ : savetxt s" TEXT" s" txt" write-tape-file ;
  \ hex  ' (write-tape-file u.

  \ XXX TMP --

  \ need 8-bit-random-pix-benchmarks  exit

  \ XXX TMP --

  \ need wordlist-words need order
  \ need bench{ need indexer
  \ : iw ( -- ) index-wordlist wordlist-words ;

  \ vim: filetype=soloforth
