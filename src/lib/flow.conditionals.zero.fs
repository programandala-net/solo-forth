  \ flow.conditions.zero.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702280000
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Zero conditionals.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ XXX TODO Compilation stack notation.

( 0if 0while 0until 0exit )

[unneeded] 0if ?(
: 0if ( f -- )
  postpone ?branch >mark ; immediate compile-only ?)

  \ doc{
  \
  \ 0if ( f -- )
  \
  \ Faster and smaller alternative to the idiom ``0= if``.
  \
  \ ``0if`` is an `immediate` and `compile-only` word.
  \
  \ }doc

[unneeded] 0while ?( need 0if need cs-swap
: 0while ( f -- )
  postpone 0if  postpone cs-swap ; immediate compile-only ?)

  \ doc{
  \
  \ 0while ( f -- )
  \
  \ Faster and smaller alternative to the idiom `0= while`.
  \
  \ ``0while`` is an `immediate` and `compile-only` word.
  \
  \ }doc

[unneeded] 0until ?(
: 0until ( f -- )
  postpone ?branch <resolve ; immediate compile-only ?)

  \ doc{
  \
  \ 0until ( f -- )
  \
  \ Faster and smaller alternative to the idiom `0= until`.
  \
  \ ``0until`` is an `immediate` and `compile-only` word.
  \
  \ }doc

[unneeded] 0exit ?(
code 0exit ( f -- ) ( R: nest-sys | -- nest-sys | )
  E1 c,  78 04 + c,  B0 05 + c,  CA c, ' exit ,
  jpnext, end-code ?)
  \ pop hl
  \ ld a,h
  \ or l ; zero?
  \ jp z,exit_zx ; jump if zero
  \ jp next

  \ doc{
  \
  \ 0exit ( f -- ) ( R: nest-sys | -- nest-sys | )
  \
  \ If _f_ is zero, return control to the calling definition,
  \ specified by _nest-sys_.
  \
  \ `0exit` is not intended to be used within a do-loop. Use
  \ `0= if unloop exit then` instead.
  \
  \ In Solo Forth `0exit` can be used in interpretation mode to
  \ stop the interpretation of a block.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-11-24: Compact the code, saving one block. Add
  \ conditional compilation for `need`.
  \
  \ 2016-11-26: Fix needing of `0exit`.
  \
  \ 2016-12-20: Rename `jpnext` to `jpnext,` after the change
  \ in the kernel.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-19: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-02-27: Improve documentation.

  \ vim: filetype=soloforth
