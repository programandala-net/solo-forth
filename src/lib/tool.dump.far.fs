  \ tool.dump.far.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803091543
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Two versions of the `dump` tool that work on far memory.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( fardump )

need 16hex. need fartype-ascii need backspace need ?leave

: fardump ( ca len -- )
  8 max 8 2dup mod - + 8 / 1- 0
  ?do
    cr dup 16hex.
    8 0 ?do  i over + far@ flip 16hex.  cell +loop
    dup backspace 8 fartype-ascii
    break-key? ?leave
  8 + loop  drop ;

  \ doc{
  \
  \ fardump ( ca len -- ) "far-dump"
  \
  \ Show the contents of _len_ bytes from far-memory address
  \ _ca_.
  \
  \ }doc

( farwdump )

need 16hex. need ?leave

: farwdump ( a len -- )
  0
  ?do
    i 4 mod 0= if  cr dup 16hex. space  then  \ show address
    dup far@ 16hex. cell+
    break-key? ?leave
  loop  drop ;

  \ doc{
  \
  \ farwdump ( a len -- ) "far-w-dump"
  \
  \ Show the contents of _len_ cells from far-memory address
  \ _a_.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-11-16: Start. Copy and modify the code of ordinary
  \ `dump` and `wdump`.
  \
  \ 2016-11-26: Need `?leave`, which has been moved to the
  \ library.
  \
  \ 2017-02-20: Replace `do`, which has been moved to the
  \ library, with `?do`.
  \
  \ 2018-03-09: Add words' pronunciaton.  Update notation
  \ "address units" to "bytes".

  \ vim: filetype=soloforth

