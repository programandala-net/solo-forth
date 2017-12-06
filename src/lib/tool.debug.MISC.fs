  \ tool.debug.MISC.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201712061500
  \ See change log at the end of the file

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( ?depth )

: ?depth ( -- ) depth if decimal cr .s #-258 throw then ;

  \ doc{
  \
  \ ?depth ( -- )
  \
  \ If `depth` is not zero, set `base` to `decimal`, display
  \ the stack on a new line with `.s` and finally `throw`
  \ exception #-258 (stack imbalance).
  \
  \ See: `?csp`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2017-12-06: Start. Move `?depth` from Nuclear Waste
  \ Invaders
  \ (http://programandala.net/en.program.nuclear_waste_invaders.html).

  \ vim: filetype=soloforth
