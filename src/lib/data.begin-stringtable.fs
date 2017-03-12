  \ data.begin-stringtable.fsb
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 20160324

  \ ===========================================================
  \ Description

  \ `begin-stringtable end-stringtable`.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( begin-stringtable end-stringtable )

  \ Credit:
  \
  \ Code adapted from Forth Foundation Library (stt module).
  \ XXX TODO Published under LGPL ?

: begin-stringtable ( "name" -- stringtable-sys )
  \ Start a named stringtable definition.
  create  here ( a1 ) cell allot here ( a1 a2 )
    \ stringtable-sys:
    \   a1 = pointer (address of address) to the strings index
    \   a2 = address of the compiled strings
  does> ( n -- ca len )
    \ Return the nth string.
    ( n pfa ) @ swap cells + @ count ;

: end-stringtable ( stringtable-sys -- )
  \ End the stringtable definition.
  \ stringtable-sys:
  \   a1 = pointer (address of address) to the strings index
  \   a2 = address of the compiled strings
  ( a1 a2 )
  here rot !   \ set the index
  here swap ( a3 a2 )
  begin  2dup <>  while
    dup ,   \ store the start of the string in the index
    count chars +  \ move to the next string
  repeat  2drop ;

  \ Usage example:
  \
  \ begin-stringtable esperanto-number
  \   s" nulo" s,  s" unu" s,  s" du" s,  s" tri" s,
  \ end-stringtable
  \ 0 esperanto-number type
  \ 3 esperanto-number type

  \ vim: filetype=soloforth
