  \ tool.dump.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702220020

  \ -----------------------------------------------------------
  \ Description

  \ Two versions of the `dump` tool.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ Latest changes

  \ 2016-04-15: Fixed `dump` (the loop printed one byte more
  \ than requested). Improved `ascii-type` (now also characters
  \ above 127 are printed as dots, not masked).
  \
  \ 2016-04-24: Fix `dump`: nothing was printed when length was
  \ less than 8.
  \
  \ 2016-04-27: Move `ascii-char?` and `control-char?` to
  \ module "chars.fsb".  Move `ascii-type` to module
  \ "printing.type.fsb" and rename it to `type-ascii`. Replace
  \ `bs` with `backspace`, which is part of the library.
  \
  \ 2016-11-16: Improve documentation.
  \
  \ 2016-11-26: Need `?leave`, which has been moved to the
  \ library.
  \
  \ 2017-02-20: Replace `do`, which has been moved to the
  \ library, with `?do`.

( dump )

need 16hex. need type-ascii need backspace need ?leave

: dump ( ca len -- )
  8 max 8 2dup mod - + 8 / 1- 0
  ?do
    cr dup 16hex.
    8 0 ?do  i over + @ flip 16hex.  cell +loop
    dup backspace 8 type-ascii
    break-key? ?leave
  8 + loop  drop ;

  \ doc{
  \
  \ dump ( ca len -- )
  \
  \ Show the contents of _len_ address units from _ca_.
  \
  \ }doc

( wdump )

need 16hex. need ?leave

: wdump ( a len -- )
  0
  ?do
    i 4 mod 0= if  cr dup 16hex. space  then  \ show address
    dup @ 16hex. cell+
    break-key? ?leave
  loop  drop ;

  \ doc{
  \
  \ wdump ( a len -- )
  \
  \ Show the contents of _len_ cells from _a_.
  \
  \ }doc

  \ vim: filetype=soloforth
