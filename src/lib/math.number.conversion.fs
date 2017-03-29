  \ math.number.conversion.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201703292226
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words related to number conversion.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( number cell-bits )

[unneeded] number

?\ : number ( ca len -- n | d ) number? 0= #-275 ?throw ;

  \ doc{
  \
  \ number ( ca len -- n | d )
  \
  \ Attempt to convert a string _ca len_ into a number. If
  \ a valid point is found, return _d_; if there is no
  \ valid point, return _n_. If conversion fails due to an
  \ invalid character, an exception #-275 is thrown.
  \
  \ See also: `number?`, `>number`.
  \
  \ }doc

[unneeded] cell-bits ?\ 16 cconstant cell-bits

  \ doc{
  \
  \ cell-bits  ( -- n )
  \
  \ A constant. _n_ is the number of bits in a cell.
  \
  \ See also: `cell`, `environment?`.
  \
  \ }doc


  \ ===========================================================
  \ Change log

  \ 2016: `number`.
  \
  \ 2017-03-29: Move `cell-bits` from the 1-cell and 2-cell
  \ operators modules. Improve documentation.

  \ vim: filetype=soloforth

