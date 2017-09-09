  \ memory.code-bank.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ XXX UNDER DEVELOPMENT

  \ Last modified: 201709091154
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Tool to use a 16-KiB memory bank to store binary code,
  \ Forth words or data.  The intent is to use it mainly for
  \ binary modules, saving dictionary space.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( code-bank )

need save-here need call need there need bank-start

variable cp  bank-start cp !  \ code pointer

: code-here ( -- a ) cp @ ;
: code-there ( a -- ) cp ! ;
: code-allot ( n -- ) cp +! ;

variable code-bank#  3 code-bank# !
  \ Memory bank used as code bank.

: code-bank ( -- ) code-bank# @ bank ;
  \ Page the code bank in.

: code-bank{ ( -- ) save-here code-here there code-bank ;
  \ Start compiling code into the code bank.

: }code-bank ( -- ) default-bank restore-here ;
  \ End compiling code into the code bank.

: ?bank ( -- ) bank-start here u< #-276 ?throw ;
  \ If the dictionary has reached the zone of memory banks,
  \ throw error #-276; else do nothing.  This check is required
  \ after compiling code that manipulates memory banks.

: code-bank-caller ( i*x a "name" -- j*x )
  create ?bank ,
  does> ( -- ) ( dfa ) @ code-bank call default-bank ;
  \ Create a word "name" which will call the machine code
  \ routine at _a_, in the code bank.

?bank

  \ ===========================================================
  \ Change log

  \ 2016-03-19: First version.
  \
  \ 2016-06-01: Update: `there` was moved from the kernel to
  \ the library.
  \
  \ 2017-05-11: Add `need bank-start`, since `bank-start` has
  \ been moved to the library.
  \
  \ 2017-09-09: Update notation "pfa" to the standard "dfa".

  \ vim: filetype=soloforth
