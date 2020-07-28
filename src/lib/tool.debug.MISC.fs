  \ tool.debug.MISC.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202007282031
  \ See change log at the end of the file

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2017, 2018, 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( ?depth .unused .words )

unneeding ?depth ?( need depth need .s

: ?depth ( -- ) depth if decimal cr .s #-258 throw then ; ?)

  \ doc{
  \
  \ ?depth ( -- ) "question-depth"
  \
  \ If `depth` is not zero, set `base` to `decimal`, display
  \ the stack on a new line with `.s` and finally `throw`
  \ exception #-258 (stack imbalance).
  \
  \ See also: `?csp`.
  \
  \ }doc

unneeding .unused ?(

: .unused ( -- ) ram . ." KiB RAM" cr
                 unused u. ." B free data/code space" cr
                farunused u. ." B free name space" ; ?)

  \ doc{
  \
  \ .unused ( -- ) "dot-unused"
  \
  \ Display the total RAM in the system, and the amount of
  \ space remaining in the regions addressed by `here` and
  \ `np`, in bytes.
  \
  \ See also: `unused`, `farunused`, `.words`.
  \
  \ }doc

unneeding .words

?\ : .words ( -- ) #words u. ." words" ;

  \ doc{
  \
  \ .words ( -- ) "dot-words"
  \
  \ Display a message informing about the number of words
  \ defined in the system.
  \
  \ See also: `#words`, `greeting`, `.unused`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2017-12-06: Start. Move `?depth` from Nuclear Waste
  \ Invaders
  \ (http://programandala.net/en.program.nuclear_waste_invaders.html).
  \
  \ 2018-03-09: Add words' pronunciaton.
  \
  \ 2020-05-09: Update requirements: `depth` has been moved to
  \ the library.
  \
  \ 2020-05-18: Move `.unused` and `.words` here from the
  \ kernel. Fix requirements of `?depth`.

  \ vim: filetype=soloforth
