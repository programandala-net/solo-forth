  \ math.number.conversion.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 20160325

  \ ===========================================================
  \ Description

  \ Words related to number conversion.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( number )

: number ( ca len -- n | d ) number? 0= #-275 ?throw ;
  \ doc{
  \
  \ number ( ca len -- n | d )
  \
  \ Attempt to convert a string _ca len_ into a number. If
  \ a valid point is found, return _d_; if there is no
  \ valid point, return _n_. If conversion fails due to an
  \ invalid character, an exception #-275 is thrown.
  \
  \ }doc

  \ vim: filetype=soloforth

