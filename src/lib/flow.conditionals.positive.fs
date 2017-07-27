  \ flow.conditionals.positive.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201707271622
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Positive conditionals.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( +if +while +until +exit )

[unneeded] +if ?( need -branch

: +if
  \ Compilation: ( C: -- orig )
  \ Run-time:    ( f -- )
  postpone -branch >mark ; immediate compile-only ?)

  \ doc{
  \
  \ +if
  \   Compilation: ( C: -- orig )
  \   Run-time:    ( f -- )
  \
  \ Faster and smaller alternative to the idiom `0>= if`.
  \
  \ ``+if`` is an `immediate` and `compile-only` word.
  \
  \ See also: `if`, `0if`, `-if`, `-branch`.
  \
  \ }doc

[unneeded] +while ?( need +if need cs-swap

: +while
  \ Compilation: ( C: dest -- orig dest )
  \ Run-time:    ( f -- )
  postpone +if  postpone cs-swap ; immediate compile-only ?)

  \ doc{
  \
  \ +while ( n -- )
  \   Compilation: ( C: dest -- orig dest )
  \   Run-time:    ( f -- )
  \
  \ Faster and smaller alternative to the idiom `0>= while`.
  \
  \ ``+while`` is an `immediate` and `compile-only` word.
  \
  \ See also: `while`, `0while`, `-while`.
  \
  \ }doc

[unneeded] +until ?( need -branch

: +until
  \ Compilation: ( C: dest -- )
  \ Run-time:    ( f -- )
  postpone -branch <resolve ; immediate compile-only ?)

  \ doc{
  \
  \ +until
  \   Compilation: ( C: dest -- )
  \   Run-time:    ( f -- )
  \
  \ Faster and smaller alternative to the idiom `0>= until`.
  \
  \ ``+until`` is an `immediate` and `compile-only` word.
  \
  \ See also: `until`, `0until`, `-until`, `-branch`.
  \
  \ }doc

[unneeded] +exit ?(

code +exit ( n -- ) ( R: nest-sys | -- nest-sys | )
  E1 c,  CB c, 7C c,  C2 c, ' exit ,  jpnext, end-code ?)
  \ pop hl
  \ bit 7,h ; positive?
  \ jp z,exit_xt ; exit if positive
  \ _jp_next

  \ doc{
  \
  \ +exit ( n -- ) ( R: nest-sys | -- nest-sys | )
  \
  \ If _n_ is positive, return control to the calling
  \ definition, specified by _nest-sys_.
  \
  \ `+exit` is not intended to be used within a `loop`.  Use
  \ ``0>= if unloop exit then`` instead.
  \
  \ ``+exit`` can be used in interpretation mode to stop the
  \ interpretation of a block.
  \
  \ See also: `exit`, `?exit`, `0exit`, `-exit`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-11-24: Compact the code, saving one block. Add
  \ conditional compilation for `need`. Rename `-branch` to
  \ `+branch` and move it to its own module.
  \
  \ 2016-12-20: Rename `jpnext` to `jpnext,` after the change
  \ in the kernel.
  \
  \ 2017-01-19: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-02-27: Improve documentation.
  \
  \ 2017-05-05: Improve documentation.
  \
  \ 2017-07-27: Replace `jp next` with the actual macro
  \ `_jp_next` in Z80 comments.

  \ vim: filetype=soloforth

