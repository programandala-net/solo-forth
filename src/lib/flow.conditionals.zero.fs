  \ flow.conditionals.zero.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803052149
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Zero conditionals.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( 0if 0while 0until 0exit )

unneeding 0if ?(
: 0if
  \ Compilation: ( C: -- orig )
  \ Run-time:    ( f -- )
  postpone ?branch >mark ; immediate compile-only ?)

  \ doc{
  \
  \ 0if "zero-if"
  \   Compilation: ( C: -- orig )
  \   Run-time:    ( f -- )

  \
  \ Faster and smaller alternative to the idiom ``0= if``.
  \
  \ ``0if`` is an `immediate` and `compile-only` word.
  \
  \ See: `if`, `-if`, `+if`, `0while`, `0until`, `0exit`.
  \
  \ }doc

unneeding 0while ?( need 0if need cs-swap
: 0while
  \ Compilation: ( C: dest -- orig dest )
  \ Run-time:    ( f -- )
  postpone 0if  postpone cs-swap ; immediate compile-only ?)

  \ doc{
  \
  \ 0while "zero-while"
  \   Compilation: ( C: dest -- orig dest )
  \   Run-time:    ( f -- )

  \
  \ Faster and smaller alternative to the idiom ``0= while``.
  \
  \ ``0while`` is an `immediate` and `compile-only` word.
  \
  \ See: `while`, `-while`, `+while`, `0if`, `0until`, `0exit`.
  \
  \ }doc

unneeding 0until ?(
: 0until
  \ Compilation: ( C: dest -- )
  \ Run-time:    ( f -- )
  postpone ?branch <resolve ; immediate compile-only ?)

  \ doc{
  \
  \ 0until "zero-until"
  \   Compilation: ( C: dest -- )
  \   Run-time:    ( f -- )

  \
  \ Faster and smaller alternative to the idiom ``0= until``.
  \
  \ ``0until`` is an `immediate` and `compile-only` word.
  \
  \ See: `until`, `-until`, `+until`, `0if`, `0while`, `0exit`.
  \
  \ }doc

unneeding 0exit ?(
code 0exit ( f -- ) ( R: nest-sys | -- nest-sys | )
  E1 c,  78 04 + c,  B0 05 + c,  CA c, ' exit ,
  jpnext, end-code ?)
  \ pop hl
  \ ld a,h
  \ or l ; zero?
  \ jp z,exit_ ; jump if zero
  \ _jp_next

  \ doc{
  \
  \ 0exit ( f -- ) ( R: nest-sys | -- nest-sys | ) "zero-exit"
  \
  \ If _f_ is zero, return control to the calling definition,
  \ specified by _nest-sys_.
  \
  \ WARNING: ``0exit`` is not intended to be used within a
  \ `loop`.  Use ``0= if unloop exit then`` instead.
  \
  \ ``0exit`` can be used in interpretation mode to stop the
  \ interpretation of a block.
  \
  \ See: `?exit`, `exit`, `-exit` ,`+exit`, `0if`, `0while`,
  \ `0until`.
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
  \
  \ 2017-05-05: Improve documentation.
  \
  \ 2017-05-09: Fix typo.
  \
  \ 2017-05-21: Fix typo.
  \
  \ 2017-07-27: Replace `jp next` with the actual macro
  \ `_jp_next` in Z80 comments.
  \
  \ 2017-12-11: Improve documentation.
  \
  \ 2018-02-02: Improve documentation.
  \
  \ 2018-02-04: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.

  \ vim: filetype=soloforth
