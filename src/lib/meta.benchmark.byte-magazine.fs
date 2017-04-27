  \ benchmark.byte-magazine.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201704271853
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ BYTE Magazine benchmark.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( do-prime )

  \ Credit:
  \
  \ Eratosthenes Sieve Prime Number program in Forth
  \ by Jim Gilbreath, BYTE Magazine, 1981-09, page 190.

only forth definitions decimal  need do

8190 constant size  variable flags  size allot

: do-prime ( -- )
  flags size 1 fill
  0 size 0
  do flags i + c@
     if i dup + 3 + dup i +
          begin   dup size <
          while   0 over flags + c! over +
          repeat  drop drop 1+
     then
  loop  .  ." primes " ;

( byte-magazine-benchmark )

  \ Credit:
  \
  \ Code adapted from: Forth Dimensions (volume 17, number 4,
  \ page 11, 1995-11).
  \
  \ 2015-12-24. Modified: no printing.

need bench{ need do

8190 constant size  variable bflags size allot

: c<- ( a b -- ) swap c! ;

: do-prime ( -- )
  bflags size 1 fill  0
  size 0 do   bflags i + c@ if
                i 2* 3 + dup i + bflags +
                begin   dup size bflags +  u<
                while   dup 0 c<- over +
                repeat  drop drop 1+
              then
  loop
  \ u. ." PRIMES" cr  \ XXX OLD
  drop  \ XXX NEW
  ;  -->

( byte-magazine-benchmark )

: byte-magazine-benchmark ( n -- )
  cr ." BYTE Magazine Benchmark:" cr
  dup u. ." iterations..." cr
  bench{ 0 ?do  do-prime  loop }bench. ;

  cr
  \  <------------------------------>
  .( To run the BYTE Magazine) cr
  .( benchmark type:) cr
  .(   n byte-magazine-benchmark) cr
  .( where _n_ is the number of) cr
  .( iterations. The original code) cr
  .( used 1000 iterations.) cr


  \ 2015-12-24:
  \
  \ Times Frames (1 frame = 50th of second)
  \ ----- -----------------------------------
  \       ITC           DTC
  \       -----         -----
  \ 00010  6397          5216
  \ 00100 63970 (1.00)  52159 (0.81)

  \ 2016-03-16:
  \
  \ Times Frames (1 frame = 50th of second)
  \ ----- --------------------------------------
  \       jp pushhl        push hl + jp (ix) [1]
  \       ------------     ---------------------
  \ 00001                    517
  \ 00010                   5164
  \ 00100 52161 (1.00)     51635 (0.98)
  \
  \ [1] Changed only in the kernel.

  \ Evolution for 100 iterations
  \
  \ Date       Frames Note
  \ ---------- ------ -------
  \ 2015-12-24  63970 ITC (old)
  \ 2015-12-24  52159 DTC (new)
  \ 2016-03-16  52161 `jp pushhl` (old)
  \ 2016-03-16  51635 `push hl + jp (ix)` (new, kernel only)
  \ 2017-04-27  49270

  \ ===========================================================
  \ Change log

  \ 2015-12-24: Modify `do-prime`: no printing.
  \
  \ 2016-03-24: Changes.
  \
  \ 2017-04-27: Add `need do`.
  \
  \ 2017-04-27: Rename the file in order to move the code to
  \ the "workbench" disk image.

  \ vim: filetype=soloforth
